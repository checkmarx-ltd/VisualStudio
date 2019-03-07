using CxViewerAction.Services;
using System;
using System.Web;
using System.Windows.Forms;
using Common;
using Microsoft.Win32;


namespace CxViewerAction.Views.DockedView
{

	public partial class OidcLoginCtrl : UserControl
    {
        #region Fields

        public event EventHandler<string> NavigationError;
        public event EventHandler<string> NavigationCompleted;
        public const string ERROR_QUERY_KEY = "Error";
        public const string BLANK_PAGE = "about:blank"; 
        
        #endregion

        #region Ctor

        public OidcLoginCtrl()
        {
            InitializeComponent();
			webBrowserIdentityProvider.ScriptErrorsSuppressed = true;
			ChangeIeVersion();
			webBrowserIdentityProvider.DocumentCompleted += OnDocumentCompleted;
			webBrowserIdentityProvider.Navigated += OnNavigated;
		}

		private void ChangeIeVersion() {
			int RegVal;

			// set the appropriate IE 11 version
			RegVal = 11001;
	
			// set the actual key
			using (RegistryKey Key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION", RegistryKeyPermissionCheck.ReadWriteSubTree))
			Key.SetValue(System.Diagnostics.Process.GetCurrentProcess().ProcessName + ".exe", RegVal, RegistryValueKind.DWord);
		}

        #endregion

        #region Private methods

        private void OnNavigated(object sender, WebBrowserNavigatedEventArgs eventArgs)
        {
			webBrowserIdentityProvider.AllowNavigation = true;
			
			Uri urlAddress = new Uri(eventArgs.Url.ToString());
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
            if (NavigationError != null)
            {
                webBrowserIdentityProvider.Navigate(BLANK_PAGE);
                NavigationError(this, errorMessage);
            }
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

        private void OnDocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs eventArgs)
        {
            if (!eventArgs.Url.AbsoluteUri.ToLower().Contains("code="))
            {
                return;
            }

			string code = ExtractCodeFromUrl(eventArgs.Url.AbsoluteUri);

            if (NavigationCompleted != null)
            {
                webBrowserIdentityProvider.Navigate(BLANK_PAGE);
                NavigationCompleted(this, code);
            }
        }

		private string ExtractCodeFromUrl(string absoluteUri)
		{
			Uri myUri = new Uri(absoluteUri);
			return HttpUtility.ParseQueryString(myUri.Query).Get("code");
		}

		public void ConnectToIdentidyProvider(String serverUri)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => ConnectToIdentidyProvider(serverUri)));
                return;
            }
            NavigateToOidcLogin(serverUri);
        }

        private void NavigateToOidcLogin(String serverUri)
        {
			string serverURL = serverUri + Constants.AUTHORIZATION_ENDPOINT;
			string header = string.Format("Content-Type: application/x-www-form-urlencoded", Environment.NewLine);
            string redirectUri = serverUri;
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

			WinCookieHelper.SupressCookiePersist();
			webBrowserIdentityProvider.Navigate(serverURL, string.Empty, postDataBytes, header);
        }

		#endregion
	}
}
