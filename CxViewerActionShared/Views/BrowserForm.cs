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
using CxViewerAction2022.Helpers;
using CefSharp.WinForms;
using System.Collections.Specialized;

//This code is using CefSharp Browser for Login.
// Use of this source code is governed by a BSD-style license that can be found in the CxViewerVSIX Resource LICENSE file.
namespace CxViewerAction2022.Views
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
            Logger.Create().Debug("Chrome browser form loading");
            browser = new ChromiumWebBrowser();
            this.pContainer.Controls.Add(browser);
            browser.Dock = DockStyle.Fill;
            Logger.Create().Debug("Chrome browser address changed event calling");
            browser.AddressChanged += OnBrowserAddressChanged;
            browser.FrameLoadEnd += chromium_FrameLoadEnd;
            Logger.Create().Debug("Chrome browser load end");
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
            
            if (!urlAddress.ToString().Contains("code=") )
            {

                if (!urlAddress.ToString().Contains("CxRestAPI"))
                {
                    string serverurl = urlAddress + Constants.AUTHORIZATION_ENDPOINT1;
                    Logger.Create().Debug("On browser adress changed server url " + serverurl);
                    string header = string.Format("Content-Type: application/x-www-form-urlencoded", Environment.NewLine);
                    string redirectUri = (browser.Address.ToString());
                    Logger.Create().Debug("Redirect url " + redirectUri);
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
                    Logger.Create().Debug("On browser adress changed post data " + postData);
                    browser.LoadUrlWithPostData(serverurl, postDataBytes, contentType);
                    Logger.Create().Debug("Url loaded with post data ");
                }
                else 
                {
                    Logger.Create().Debug("Navigating to " + browser.Address.ToString());
                }

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
                if (e.Url.ToString().Contains("CxRestAPI"))
                {
                    Logger.Create().Debug("Navigation complete for " + e.Url.ToString());
                    Logger.Create().Debug("Checking for presence of authorization code in the URL. " + e.Url.ToLower());
                }
                return;
            }
            Logger.Create().Debug("Authorization code found. Extracting authorization code from the URL. ");
            string code = ExtractCodeFromUrl(e.Url);

            if (NavigationCompleted != null)
            {
               
                NavigationCompleted(this, code);
                Logger.Create().Debug("Navigation completed with authorization code: " + code);
                browser.LoadUrl(BLANK_PAGE);
                browser.FrameLoadEnd -= chromium_FrameLoadEnd;
                Logger.Create().Debug("Chrome browser load ended");
                browser.Invoke(new MethodInvoker(() =>
                {
                    Application.DoEvents();
                    this.Close();

                }));
            }
        }
        private string ExtractCodeFromUrl(string absoluteUri)
        {
            Uri myUri = new Uri(absoluteUri);
            Logger.Create().Debug("Extracted authorization code from url is " + HttpUtility.ParseQueryString(myUri.Query).Get("code"));
            return HttpUtility.ParseQueryString(myUri.Query).Get("code");
        }
        private string TryGetErrorFromQueryString(string queryString)
        {
            string errorMessage = string.Empty;
            var queryParameters = HttpUtility.ParseQueryString(queryString);
            Logger.Create().Debug("Query string is " + queryString);
            Logger.Create().Debug("Query parameters are " + queryParameters);
            if (queryParameters.HasKeys())
            {
                errorMessage = queryParameters[ERROR_QUERY_KEY];
                Logger.Create().Debug("Query string has errors ");
            }
            return errorMessage;
        }

        public void ConnectToIdentidyProvider(String serverUri)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => ConnectToIdentidyProvider(serverUri)));
                Logger.Create().Debug("Connection to identity provider successful");
                return;
            }
           
            NavigateToOidcLogin(serverUri);
        }
        private void NavigateToOidcLogin(String serverUri)
        {
            Logger.Create().Debug("Oidc login server uri " + serverUri);
            browser.LoadUrl(serverUri);
        
        }

        private void BrowserForm_FormClosed(object sender, FormClosedEventArgs e)
        {

            CloseForm();
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (UserClosedForm != null)
                {
                    Logger.Create().Debug("Browser form closed");
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
            Application.DoEvents();
            Hide();
        }

        /// <summary>
        /// Plug-533 Authentication fix that impacted hosted customers
        /// </summary>
        public class MyCustomResourceRequestHandler : CefSharp.Handler.ResourceRequestHandler
        {
            private readonly System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();

            protected override IResponseFilter GetResourceResponseFilter(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response)
            {
                return new CefSharp.ResponseFilter.StreamResponseFilter(memoryStream);
            }

            public class NewCustomRequestHandler : RequestHandler
            {
                protected override IResourceRequestHandler GetResourceRequestHandler(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame,
                    IRequest request, bool isNavigation, bool isDownload, string requestInitiator, ref bool disableDefaultHandling)
                {
                    return new CustomResourceRequestHandler();
                }
            }

            public class CustomResourceRequestHandler : ResourceRequestHandler
            {
                public readonly BrowserForm _oidcLoginHelper = new BrowserForm();
                protected override CefReturnValue OnBeforeResourceLoad(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request,
                    IRequestCallback callback)
                {
                    var Url = request.Url.ToString();
                    Uri myUri = new Uri(request.Url);
                    Logger.Create().Debug("new url " + Url);
                    if (Url.ToLower().Contains("code="))
                    {

                        string code = HttpUtility.ParseQueryString(myUri.Query).Get("code");
                        _oidcLoginHelper.NavigationCompleted(this, code);
                        Logger.Create().Debug("Authorization code found. Extracting authorization code from the URL. ");
                        browser.CloseBrowser(false);

                    }
                    if (Url.ToLower().Contains("error="))
                    {

                        string error = HttpUtility.ParseQueryString(myUri.Query).Get("error");
                        _oidcLoginHelper.NavigationError(this, error);
                        browser.CloseBrowser(false);

                    }

                    return CefReturnValue.Continue;
                }
            }
        }
    }
}
