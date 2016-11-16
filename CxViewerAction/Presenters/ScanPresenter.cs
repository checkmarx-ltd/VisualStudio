using System;
using System.Collections.Generic;

using System.Text;
using CxViewerAction.Views;
using CxViewerAction.ServiceLocators;
using CxViewerAction.Dispatchers;
using CxViewerAction.Entities;

namespace CxViewerAction.Presenters
{
    /// <summary>
    /// Represent scan controller
    /// </summary>
    public class ScanPresenter : IScanPresenter
    {
        #region [Private Members]

        private IDispatcher _dispatcher;
        private IScanView _view;
        
        #endregion

        #region [Public Properties]

        /// <summary>
        /// Get Related View
        /// </summary>
        public IScanView View { get { return _view; } }

        #endregion

        #region [Events Handlers]

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="view">Scan data</param>
        public ScanPresenter(IScanView view)
        {
            this._view = view;
            this._view.Load += View_Load;

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

        #endregion

        /// <summary>
        /// Perform scan
        /// </summary>
        /// <param name="parentView">Parent view</param>
        /// <param name="scanData">Scan data</param>
        void IScanPresenter.Scan(IView parentView, Scan scanData)
        {
            _view.RunInBackgroundHandler = scanData.RunInBackgroundHandler;
            _view.CancelHandler = scanData.CancelHandler;
            _view.DetailsHandler = scanData.DetailsHandler;

            _view.ShowView();
        }
    }
}
