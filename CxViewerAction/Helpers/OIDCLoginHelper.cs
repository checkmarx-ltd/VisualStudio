using System;
using System.Threading;
using System.Windows.Forms;
using CxViewerAction.ValueObjects;
using CxViewerAction.Views;

namespace CxViewerAction.Helpers
{
    public class OIDCLoginHelper
    {
        private readonly OidcLoginFrm _oidcLoginFrm = new OidcLoginFrm();
        private OidcLoginResult _latestResult;
        public static bool errorWasShown = false;

        public OIDCLoginHelper()
        {
            _oidcLoginFrm.NavigationCompleted += OidcLoginCtrlOnNavigationCompleted;
            _oidcLoginFrm.NavigationError += OidcLoginCtrlOnNavigationError;
            _oidcLoginFrm.UserClosedForm += OnUserClosedForm;
			_latestResult = new OidcLoginResult(false, string.Empty, null);
		}

		public void resetLatestResult()
		{
			_latestResult = new OidcLoginResult(false, string.Empty, null);
		}

        private void OnUserClosedForm(object sender, EventArgs e)
        {
            _latestResult = new OidcLoginResult(false, "Exit", null);
        }

        private void OidcLoginCtrlOnNavigationError(object sender, string errorMessage)
        {
            errorWasShown = true;
            MessageBox.Show(errorMessage,"Error", MessageBoxButtons.OK);
            _latestResult = new OidcLoginResult(false, errorMessage, null);
        }

        private void OidcLoginCtrlOnNavigationCompleted(object sender, string code)
        {
            _latestResult = new OidcLoginResult(true, string.Empty, code);            
        }

        public OidcLoginResult ConnectToIdentidyProvider(string baseServerUri)
        {
            _oidcLoginFrm.ConnectToIdentidyProvider(baseServerUri);
            _oidcLoginFrm.ShowDialog();
            return _latestResult;
        }

		public void CloseLoginWindow()
		{
			_oidcLoginFrm.CloseForm();
		}
    }
}
