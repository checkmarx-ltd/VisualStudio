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
        private readonly AutoResetEvent _oidcLoginEvent = new AutoResetEvent(false);
        public static bool errorWasShown = false;

        private OidcLoginResult _latestResult;

        public OIDCLoginHelper()
        {
			_oidcLoginFrm.OidcLoginCtrl1.NavigationCompleted += OidcLoginCtrlOnNavigationCompleted;
			_oidcLoginFrm.OidcLoginCtrl1.NavigationError += OidcLoginCtrlOnNavigationError;
			_oidcLoginFrm.UserClosedForm += OnUserClosedForm;
			_latestResult = new OidcLoginResult(false, string.Empty, null);
		}

		public void resetLatestResult()
		{
			_latestResult = new OidcLoginResult(false, string.Empty, null);
		}

        private void OnUserClosedForm(object sender, EventArgs e)
        {
            _latestResult = new OidcLoginResult(false, string.Empty, null);
            _oidcLoginEvent.Set();
        }

        private void OidcLoginCtrlOnNavigationError(object sender, string errorMessage)
        {
            errorWasShown = true;
            MessageBox.Show(errorMessage,"Error", MessageBoxButtons.OK);
            _latestResult = new OidcLoginResult(false, errorMessage, null);
            _oidcLoginEvent.Set();
        }

        private void OidcLoginCtrlOnNavigationCompleted(object sender, string code)
        {
            _latestResult = new OidcLoginResult(true, string.Empty, code);
            _oidcLoginEvent.Set();
        }

        private void ConectAndWait(string baseServerUri)
        {
            _oidcLoginFrm.OidcLoginCtrl1.Invoke(new MethodInvoker(() =>
            {
                _oidcLoginFrm.Show();
                _oidcLoginFrm.ConnectToIdentidyProvider(baseServerUri);
            }));
            _oidcLoginEvent.WaitOne();
        }

        public OidcLoginResult ConnectToIdentidyProvider(string baseServerUri)
        {

			ConectAndWait(baseServerUri);

            _oidcLoginFrm.CloseForm();
            return _latestResult;
        }

		public void CloseLoginWindow()
		{
			_oidcLoginFrm.CloseForm();
		}
    }
}
