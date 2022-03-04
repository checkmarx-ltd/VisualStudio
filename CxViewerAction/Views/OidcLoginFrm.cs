using System;
using System.Windows.Forms;
using System.Web;
using System.Text;
using System.IO;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using System.Collections.Specialized;
using Common;

namespace CxViewerAction.Views
{
    public partial class OidcLoginFrm : Form
    {
        public event EventHandler UserClosedForm;
        public event EventHandler<string> NavigationError;
        public event EventHandler<string> NavigationCompleted;

        public OidcLoginFrm()
        {
            InitializeComponent();
            webView21.NavigationCompleted += OnDocumentCompleted;
        }

        public void ConnectToIdentidyProvider(string serverUri)
        {            
            CoreWebView2Environment webEnvironment = webView21.CoreWebView2.Environment;
            string serverURL = serverUri + Constants.AUTHORIZATION_ENDPOINT;
            string redirectUri = serverUri;
            if (!redirectUri.EndsWith("/"))
            {
                redirectUri = redirectUri + "/";
            }
            string postDataString = Constants.CLIENT_ID_KEY + "=" + Constants.CLIENT_VALUE + "&" +
                Constants.SCOPE_KEY + "=" + Constants.SCOPE_VALUE + "&" +
                Constants.RESPONSE_TYPE_KEY + "=" + Constants.RESPONSE_TYPE_VALUE + "&" +
                Constants.REDIRECT_URI_KEY + "=" + redirectUri;
            UTF8Encoding utfEncoding = new UTF8Encoding();
            byte[] postData = utfEncoding.GetBytes(postDataString);
            MemoryStream postDataStream = new MemoryStream(postDataString.Length);
            postDataStream.Write(postData, 0, postData.Length);
            postDataStream.Seek(0, SeekOrigin.Begin);
            CoreWebView2WebResourceRequest httpRequest =
                webEnvironment.CreateWebResourceRequest(
                    serverURL,
                    "POST",
                    postDataStream,
                    "Content-Type: application/x-www-form-urlencoded\r\n");
            webView21.CoreWebView2.NavigateWithWebResourceRequest(httpRequest);
        }

        public void CloseForm()
        {
            webView21.Hide();
            Hide();
        }

        private void SamlLoginFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                CloseForm();
                e.Cancel = true;
                if (UserClosedForm != null)
                {
                    UserClosedForm(this, new EventArgs());
                }
            }
        }

        /// <summary>
        /// This fires every time WebView2 loads a form but it only does something when a page load
        /// URL contains a "code=" values, this happens after a successful login.
        /// </summary>
        private void OnDocumentCompleted(object sender, CoreWebView2NavigationCompletedEventArgs eventArgs)
        {
            WebView2 webView2 = (WebView2)sender;
            String testIt = webView2.Source.AbsoluteUri;
            if (webView2.Source.AbsoluteUri.ToLower().Contains("code="))
            {
                CloseForm();
                string code = ExtractCodeFromUrl(webView2.Source.AbsoluteUri);
                if (NavigationCompleted != null)
                {
                    NavigationCompleted(this, code);
                }
            }
        }

        private string ExtractCodeFromUrl(string absoluteUri)
        {
            Uri myUri = new Uri(absoluteUri);
            NameValueCollection qscoll = HttpUtility.ParseQueryString(myUri.Query);
            return HttpUtility.ParseQueryString(myUri.Query).Get("code");
        }
    }
}