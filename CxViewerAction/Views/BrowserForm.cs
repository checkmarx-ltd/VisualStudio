using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using CefSharp.Web;
using CefSharp.Handler;
using Common;
using System.Web;
using CxViewerAction.Helpers;
using CefSharp.WinForms;
using System.Collections.Specialized;

//This code is using CefSharp Browser for Login.
// Use of this source code is governed by a BSD-style license that can be found in the CxViewerVSIX Resource LICENSE file.
namespace CxViewerAction.Views
{
    public partial class BrowserForm : Form
    {
        public event EventHandler<string> NavigationCompleted;
        public event EventHandler<string> NavigationError;
        public event EventHandler UserClosedForm;
        public const string ERROR_QUERY_KEY = "Error";
        public const string BLANK_PAGE = "about:blank";
        public BrowserForm()
        {
            InitializeComponent();
        }
        ChromiumWebBrowser browser;
        static bool IsIntialized = false;
        public static void IsbrowserIntialized()
        {
            if (IsIntialized == false)
            {
                CefSharpSettings.ShutdownOnExit = false;
                CefSettings settings = new CefSettings();
                Cef.Initialize(settings);
                IsIntialized = true;
            }
        }

        private void BrowserForm_Load(object sender, EventArgs e)
        {

            browser = new ChromiumWebBrowser();
            this.pContainer.Controls.Add(browser);
            browser.Dock = DockStyle.Fill;
            browser.AddressChanged += OnBrowserAddressChanged;
            browser.FrameLoadEnd += chromium_FrameLoadEnd;

        }

        public void LoadUrl(string url)
        {
            browser.ExecuteScriptAsync("document.oncontextmenu = function() { return false; };");
            browser.Load(url);

        }

        private void OnBrowserAddressChanged(object sender, AddressChangedEventArgs e)
        {
            browser.ExecuteScriptAsync("document.oncontextmenu = function() { return false; };");
            Uri urlAddress = new Uri(browser.Address.ToString());
            if (!urlAddress.ToString().Contains("code="))
            {
                Logger.Create().Debug("Navigation complete for " + urlAddress.ToString());
            }
            string queryString = urlAddress.Query;

            if (string.IsNullOrWhiteSpace(queryString))
            {

                return;

            }

            string errorMessage = TryGetErrorFromQueryString(queryString);
            if (errorMessage == null)
            {
                return;
            }
            errorMessage = string.IsNullOrWhiteSpace(errorMessage) ? "Internal server error" : errorMessage;
            Logger.Create().Debug("Navigation complete with error " + errorMessage);
            if (NavigationError != null)
            {

                browser.LoadUrl(BLANK_PAGE);
                NavigationError(this, errorMessage);

            }
        }
        private void chromium_FrameLoadEnd(object sender, CefSharp.FrameLoadEndEventArgs e)
        {
            // Was the loaded page the first page load?
            browser.ExecuteScriptAsync("document.oncontextmenu = function() { return false; };");

            if (!e.Url.ToLower().Contains("code="))
            {
                Logger.Create().Debug("Checking for presence of authorization code in the URL. " + e.Url.ToLower());
                return;
            }
            Logger.Create().Debug("Authorization code found. Extracting authorization code from the URL. ");
            string code = ExtractCodeFromUrl(e.Url);

            if (NavigationCompleted != null)
            {
                NavigationCompleted(this, code);
                browser.LoadUrl(BLANK_PAGE);
                browser.FrameLoadEnd -= chromium_FrameLoadEnd;
                browser.Invoke(new MethodInvoker(() =>
                {
                    this.Close();

                }));
            }
        }
        private string ExtractCodeFromUrl(string absoluteUri)
        {
            Uri myUri = new Uri(absoluteUri);
            return HttpUtility.ParseQueryString(myUri.Query).Get("code");
        }
        private string TryGetErrorFromQueryString(string queryString)
        {
            string errorMessage = string.Empty;
            var queryParameters = HttpUtility.ParseQueryString(queryString);
            if (queryParameters.HasKeys())
            {
                errorMessage = queryParameters[ERROR_QUERY_KEY];
            }
            return errorMessage;
        }

        public void ConnectToIdentidyProvider(String serverUri)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => ConnectToIdentidyProvider(serverUri)));
                return;
            }
            Logger.Create().Debug("Initiating navigation to OIDC authorization endpoint " + serverUri);
            NavigateToOidcLogin(serverUri);
        }
        private void NavigateToOidcLogin(String serverUri)
        {
            string address = serverUri.Substring(7);
            string serverURL1 = serverUri + Constants.SAST_Login + address + Constants.SAST_Suffix;
            string serverurl = serverUri + Constants.AUTHORIZATION_ENDPOINT;
            string header = string.Format("Content-Type: application/x-www-form-urlencoded", Environment.NewLine);
            string redirectUri = serverUri;
            string contentType = " application/x-www-form-urlencoded";
            if (!redirectUri.EndsWith("/"))
            {
                redirectUri = redirectUri + "/";
            }
            string postData = Constants.CLIENT_ID_KEY + "=" + Constants.CLIENT_VALUE + "&" +
                Constants.SCOPE_KEY + "=" + Constants.SCOPE_VALUE + "&" +
                Constants.RESPONSE_TYPE_KEY + "=" + Constants.RESPONSE_TYPE_VALUE + "&" +
                Constants.REDIRECT_URI_KEY + "=" + redirectUri;
            System.Text.Encoding encoding = System.Text.Encoding.UTF8;
            byte[] postDataBytes = encoding.GetBytes(postData);

            if (browser.IsBrowserInitialized == true)
            {

                browser.LoadUrlWithPostData(serverurl, postDataBytes, contentType);

            }
            else
            {
                browser.LoadUrl(serverURL1);

            }

        }

        private void BrowserForm_FormClosed(object sender, FormClosedEventArgs e)
        {

            CloseForm();
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (UserClosedForm != null)
                {
                    UserClosedForm(this, new EventArgs());
                }
            }
        }
        public void CloseForm()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(CloseForm));
                return;
            }

            browser.Hide();
        }

    }
}
