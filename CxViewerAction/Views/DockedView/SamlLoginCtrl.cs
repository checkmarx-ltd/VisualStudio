using System;
using System.Web;
using System.Windows.Forms;

namespace CxViewerAction.Views.DockedView
{
    public partial class SamlLoginCtrl : UserControl
    {
        public event EventHandler<string> NavigationError;
        public event EventHandler<string> NavigationCompleted;

        public const string ERROR_QUERY_KEY = "Error";
        public const string BLANK_PAGE = "about:blank";

        public SamlLoginCtrl()
        {
            InitializeComponent();
            webBrowserIdentityProvider.DocumentCompleted += OnDocumentCompleted;
            webBrowserIdentityProvider.Navigated += OnNavigated;
        }

        private void OnNavigated(object sender, WebBrowserNavigatedEventArgs eventArgs)
        {
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
            if (!eventArgs.Url.AbsolutePath.ToLower().Contains("samlacs"))
            {
                return;
            }
            WebBrowser browser = sender as WebBrowser;
            string ottValue = browser.DocumentText;
            if (NavigationCompleted != null)
            {
                webBrowserIdentityProvider.Navigate(BLANK_PAGE);
                NavigationCompleted(this, ottValue);
            }
        }

        public void ConnectToIdentidyProvider(Uri serverUri)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => ConnectToIdentidyProvider(serverUri)));
                return;
            }
            NavigateToSamlLogin(serverUri);
        }

        private void NavigateToSamlLogin(Uri serverUri)
        {
            string header = string.Format("User-Agent: {0}{1}", CxWsResolver.CxClientType.VS.ToString("G"), Environment.NewLine);
            webBrowserIdentityProvider.Navigate(serverUri, string.Empty, null, header);
        }
    }
}
