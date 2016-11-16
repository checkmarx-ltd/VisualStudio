using System;
using System.Threading;
using System.Windows.Forms;
using CxViewerAction.ValueObjects;
using CxViewerAction.Views;

namespace CxViewerAction.Helpers
{
    public class SamlLoginHelper
    {
        private readonly SamlLoginFrm _samlLoginFrm = new SamlLoginFrm();
        private readonly AutoResetEvent _samlLoginEvent = new AutoResetEvent(false);
        public static bool errorWasShown = false;

        private SamlLoginResult _latestResult;

        public SamlLoginHelper()
        {
            _samlLoginFrm.SamlLoginCtrl.NavigationCompleted += SamlLoginCtrlOnNavigationCompleted;
            _samlLoginFrm.SamlLoginCtrl.NavigationError += SamlLoginCtrlOnNavigationError;
            _samlLoginFrm.UserClosedForm += OnUserClosedForm;
            _latestResult = new SamlLoginResult(false, string.Empty, null);
        }

        private void OnUserClosedForm(object sender, EventArgs e)
        {
            _latestResult = new SamlLoginResult(false, string.Empty, null);
            _samlLoginEvent.Set();
        }

        private void SamlLoginCtrlOnNavigationError(object sender, string errorMessage)
        {
            errorWasShown = true;
            MessageBox.Show(errorMessage,"Error", MessageBoxButtons.OK);
            _latestResult = new SamlLoginResult(false, errorMessage, null);
            _samlLoginEvent.Set();
        }

        private void SamlLoginCtrlOnNavigationCompleted(object sender, string ottValue)
        {
            _latestResult = new SamlLoginResult(true, string.Empty, ottValue);
            _samlLoginEvent.Set();
        }

        private void ConectAndWait(string baseServerUri)
        {
            _samlLoginFrm.SamlLoginCtrl.Invoke(new MethodInvoker(() =>
            {
                _samlLoginFrm.Show();
                _samlLoginFrm.ConnectToIdentidyProvider(baseServerUri);
            }));
            _samlLoginEvent.WaitOne();
        }

        public SamlLoginResult ConnectToIdentidyProvider(string baseServerUri)
        {
            ConectAndWait(baseServerUri);

            _samlLoginFrm.CloseForm();
            return _latestResult;
        }
    }
}
