using System;

// using CxViewerAction.Managers;
using CxViewerAction.Entities;
using CxViewerAction.Dispatchers;
using CxViewerAction.ServiceLocators;
using CxViewerAction.Views;
using CxViewerAction.Services;

namespace CxViewerAction.Presenters
{
    /// <summary>
    /// Represent login controller object
    /// </summary>
    public class LoginPresenter : ILoginPresenter
    {
        #region Private Members

        private IDispatcher _dispatcher;
        private static ILoginView _view;

        #endregion

        #region Events Handlers

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="view">Related view</param>
        public LoginPresenter(ILoginView view)
        {
            string url = string.Empty;
            string userName = string.Empty;
            string password = string.Empty;
            bool sso = false;
            bool saml = false;

            if (_view != null)
            {
                url = _view.ServerDomain;
                userName = _view.UserName;
                password = _view.Password;
                sso = _view.SSO;
                saml = _view.Saml;
            }
            _view = view;
            if (string.IsNullOrEmpty(_view.ServerDomain))
                _view.ServerDomain = url;
            if (string.IsNullOrEmpty(_view.UserName))
                _view.UserName = userName;
            if (string.IsNullOrEmpty(_view.Password))
                _view.Password = password;
            _view.SSO = sso;
            _view.Saml = saml;
            _view.Load += View_Load;

            _dispatcher = ServiceLocator.GetDispatcher();
        }

        /// <summary>
        /// Load event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void View_Load(object sender, EventArgs args)
        {

        }

        #endregion Event Handlers

        #region Public Methods

        /// <summary>
        /// Perform login
        /// </summary>
        /// <param name="parentView">Parent view</param>
        /// <param name="login">Login data</param>
        public void Login(IView parentView, ref LoginData login)
        {
            MapToView(login);

            System.Windows.Forms.DialogResult dialogResult = _view.ShowModalView(parentView);
            
            MapFromView(ref login);
            login.IsLogging = (dialogResult == System.Windows.Forms.DialogResult.OK);
            string sessionId = string.Empty;

            if (login.IsLogging)
            {
                LoginToRESTAPI(login);
            }

        }
        #endregion

        #region Private Methods

        private void LoginToRESTAPI(LoginData login)
        {
            new CxRESTApiLogin(login).Login();
        }

        /// <summary>
        /// Map object data to view
        /// </summary>
        /// <param name="login"></param>
        private void MapToView(LoginData login)
        {
            _view.EntityId = login.ID;
            if (!string.IsNullOrEmpty(login.ServerDomain))
                _view.ServerDomain = login.ServerDomain;
            if (!string.IsNullOrEmpty(login.UserName))
                _view.UserName = login.UserName;
            if (!string.IsNullOrEmpty(login.Password))
                _view.Password = login.Password;
            _view.SSO = login.SSO;
            _view.Saml = login.isSaml;
        }

        /// <summary>
        /// Map view data to object
        /// </summary>
        /// <param name="login"></param>
        private void MapFromView(ref LoginData login)
        {
            login.Ssl = _view.Ssl;
            login.ServerDomain = _view.ServerDomain;
            login.UserName = _view.UserName;
            login.Password = _view.Password;
            login.SSO = _view.SSO;
            login.isSaml = _view.Saml;
        }
        
        #endregion
    }
}

