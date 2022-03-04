using System;
using System.Collections.Generic;

using System.Text;
using CxViewerAction.Helpers;
using CxViewerAction.CxVSWebService;
using Common;

namespace CxViewerAction.Entities.WebServiceEntity
{
    /// <summary>
    /// Represent service login object
    /// </summary>
    public class LoginResult
    {
        #region [Private memebers]

        private bool _isSuccesfull = false;
        private string _sessionId = "";
        private LoginData _auth = null;
		#endregion

		#region [Public properties]

		public LoginResultType LoginResultType { get; set; }

        /// <summary>
        /// Get or set service perform status
        /// </summary>
        public bool IsSuccesfull
        {
            get { return _isSuccesfull; }
            set { _isSuccesfull = value; }
        }

        /// <summary>
        /// Current user session identifier
        /// </summary>
        public string SessionId
        {
            get { return _sessionId; }
        }

        /// <summary>
        /// User authentication data
        /// </summary>
        public LoginData AuthenticationData
        {
            get { return _auth; }
            set { _auth = value; }
        }

        public string LoginResultMessage
        {
            get;
            set;
        }

		#endregion
	}
}
