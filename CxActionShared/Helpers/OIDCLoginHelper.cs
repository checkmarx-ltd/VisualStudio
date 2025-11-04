using System;
using System.Threading;
using System.Windows.Forms;
using Common;
using CxViewerAction.Entities;
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
        private LoginData _pluginConfiguration = null;

        public OIDCLoginHelper()
        {
            _pluginConfiguration = null;
        }

        public void resetLatestResult()
        {
            _latestResult = new OidcLoginResult(false, string.Empty, null);
        }

        private void OnUserClosedForm(object sender, EventArgs e)
        {
            _latestResult = new OidcLoginResult(false, "Exit", null);
            _oidcLoginEvent.Set();
        }

        private void OidcLoginCtrlOnNavigationError(object sender, string errorMessage)
        {
            errorWasShown = true;
            MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK);
            _latestResult = new OidcLoginResult(false, errorMessage, null);
            _oidcLoginEvent.Set();
        }

        private void OidcLoginCtrlOnNavigationCompleted(object sender, string code)
        {
            _latestResult = new OidcLoginResult(true, string.Empty, code);
            _oidcLoginEvent.Set();
        }

        private void ConectAndWait(string baseServerUri, string AuthenticationType)
        {
            if (AuthenticationType == Constants.AuthenticationaType_DefaultValue)
            {
                Thread.Sleep(3000);
                Application.DoEvents();
                var browserForm = new BrowserForm();
                BrowserForm.NavigationCompleted += OidcLoginCtrlOnNavigationCompleted;
                BrowserForm.NavigationError += OidcLoginCtrlOnNavigationError;
                BrowserForm.IsbrowserIntialized(_pluginConfiguration);
                browserForm.Show();
                string txturl = "about:blank";
                browserForm.Invoke(new MethodInvoker(() =>
                {
                    browserForm.ConnectToIdentidyProvider(txturl);
                    Application.Run(browserForm);

                }));
            }
            else
            {
                _oidcLoginFrm.OidcLoginCtrl2.NavigationCompleted += OidcLoginCtrlOnNavigationCompleted;
                _oidcLoginFrm.OidcLoginCtrl2.NavigationError += OidcLoginCtrlOnNavigationError;
                _oidcLoginFrm.UserClosedForm += OnUserClosedForm;
                _latestResult = new OidcLoginResult(false, string.Empty, null);
                _oidcLoginFrm.OidcLoginCtrl2.Invoke(new MethodInvoker(() =>
                {
                    _oidcLoginFrm.Show();
                    _oidcLoginFrm.ConnectToIdentidyProvider(baseServerUri);
                }));
                _oidcLoginEvent.WaitOne();
            }
        }

        public OidcLoginResult ConnectToIdentidyProvider(string baseServerUri, string AuthenticationType)
        {

            ConectAndWait(baseServerUri, AuthenticationType);
            if (AuthenticationType == Constants.AuthenticationaType_IE)
            {
                _oidcLoginFrm.CloseForm();
            }

            return _latestResult;
        }

        public void CloseLoginWindow()
        {
            if (_oidcLoginFrm.Visible == true)
            {
                _oidcLoginFrm.CloseForm();
            }
        }

        /// <summary>
        /// plugin's configuration
        /// </summary>
        public LoginData PluginConfiguration
        {
            get { return _pluginConfiguration; }
            set { _pluginConfiguration = value; }
        }
    }
}
