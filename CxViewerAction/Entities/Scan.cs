using System;
using System.Collections.Generic;

using System.Text;
using CxViewerAction.Services;
using CxViewerAction.Entities.WebServiceEntity;
using System.Threading;
using CxViewerAction.Views;

namespace CxViewerAction.Entities
{
    /// <summary>
    /// Scan Data Class
    /// </summary>
    public class Scan : IEntity
    {
        #region [Private Variables]

        /// <summary>
        /// Server data that was received on user auth request
        /// </summary>
        private LoginResult _loginResult;

        /// <summary>
        /// Upload project settings
        /// </summary>
        private Upload _uploadSettings;

        /// <summary>
        /// Server data that was received on start scan request
        /// </summary>
        private RunScanResult _runScanResult;

        /// <summary>
        /// Scan executing now
        /// </summary>
        private bool _inProcess;

        /// <summary>
        /// Check for cancel button state
        /// </summary>
        private ManualResetEvent _cancelEvent = new ManualResetEvent(false);

        /// <summary>
        /// VS-addin scan dock window to represent scan data
        /// </summary>
        private IScanView _scanView = null;

        /// <summary>
        /// VS-addin scan dock window to represent scan data
        /// </summary>
        private IScanView _dockView = null;

        /// <summary>
        /// Associated scan project
        /// </summary>
        private Project _scanProject = null;

        /// <summary>
        /// Event handler for RunInBackground button
        /// </summary>
        private EventHandler _runInBackgroundHandler;

        /// <summary>
        /// Event handler for Cancel button
        /// </summary>
        private EventHandler _cancelHandler;

        /// <summary>
        /// Event handler for Details button
        /// </summary>
        private EventHandler _detailsHandler;

        #endregion

        #region [Public Properties]

        public LoginResult LoginResult 
        { 
            get { return _loginResult; }
            set { _loginResult = value; }
        }
        public Upload UploadSettings
        {
            set { _uploadSettings = value; }
            get { return _uploadSettings; }
        }
        public RunScanResult RunScanResult
        {
            get { return _runScanResult; }
            set { _runScanResult = value; }
        }
        public bool InProcess
        {
            set { _inProcess = value; }
            get { return _inProcess; }
        }

        public bool IsIncremental { set; get; }

        public IScanView ScanView
        {
            get { return _scanView; }
            set { _scanView = value; }
        }
        public IScanView DockView
        {
            get { return _dockView; }
            set { _dockView = value; }
        }
        public Project ScanProject
        {
            get { return _scanProject; }
            set { _scanProject = value; }
        }
        public bool IsPublic { get; set; }

        /// <summary>
        /// Check for cancel button state
        /// </summary>
        public ManualResetEvent CancelEvent
        {
            get { return _cancelEvent; }
        }

        /// <summary>
        /// Verify that user press cancel button
        /// </summary>
        public bool IsCancelPressed
        {
            get { return _cancelEvent.WaitOne(0, false); }
        }
        #endregion

        #region [Events]

        public EventHandler RunInBackgroundHandler
        {
            get { return _runInBackgroundHandler; }
            set { _runInBackgroundHandler = value; }
        }

        public EventHandler CancelHandler
        {
            get { return _cancelHandler; }
            set { _cancelHandler = value; }
        }

        public EventHandler DetailsHandler
        {
            get { return _detailsHandler; }
            set { _detailsHandler = value; }
        }

        #endregion

        #region [Constructors]

        /// <summary>
        /// Empty constructor
        /// </summary>
        public Scan()
        {
        }

        /// <summary>
        /// Constructor with param
        /// </summary>
        /// <param name="login">auth server result</param>
        public Scan(LoginResult login)
        {
            _loginResult = login;
        }

        /// <summary>
        /// Constructor with param
        /// </summary>
        /// <param name="login">auth server result</param>
        /// <param name="runInBackgroundHandler">handler for runInBackground button</param>
        /// <param name="cancelHandler">handler for cancel button</param>
        /// <param name="detailsHandler">handler for details button</param>
        public Scan(LoginResult login, EventHandler runInBackgroundHandler, EventHandler cancelHandler, EventHandler detailsHandler)
        {
            _loginResult = login;
            _runInBackgroundHandler = runInBackgroundHandler;
            _cancelHandler = cancelHandler;
            _detailsHandler = detailsHandler;
        }

        #endregion

        #region [Public Methods]

        /// <summary>
        /// Verify if Cancel button pressed. Thread sleep until timeout expired or cancel button pressed
        /// </summary>
        /// <returns></returns>
        public bool WaitForCancel()
        {
            return _cancelEvent.WaitOne(_loginResult.AuthenticationData.UpdateStatusInterval * 1000, false);
        }

        #endregion
    }
}
