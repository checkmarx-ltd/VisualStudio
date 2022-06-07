using System;

// using CxViewerAction2022.Managers;
using CxViewerAction2022.Entities;
using CxViewerAction2022.Dispatchers;
using CxViewerAction2022.ServiceLocators;
using CxViewerAction2022.Views;
using CxViewerAction2022.Services;
using CxViewerAction2022.Helpers;

namespace CxViewerAction2022.Presenters
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

            if (_view != null)
            {
                url = _view.ServerDomain;
            }
            _view = view;
            if (string.IsNullOrEmpty(_view.ServerDomain))
                _view.ServerDomain = url;
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
			CxRESTApi cxRestApiLogin = new CxRESTApi(login);
            bool cancelPressed = false;
            LoginHelper.DoLogin(out cancelPressed);
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
        }

        /// <summary>
        /// Map view data to object
        /// </summary>
        /// <param name="login"></param>
        private void MapFromView(ref LoginData login)
        {
            login.Ssl = _view.Ssl;
            login.ServerDomain = _view.ServerDomain;
        }
        
        #endregion
    }
}

