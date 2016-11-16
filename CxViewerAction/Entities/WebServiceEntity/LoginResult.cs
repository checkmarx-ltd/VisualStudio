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
        private bool _isSaml = false;
        private string _sessionId = null;
        private CxWSResponseLoginData _cxWSResponseLoginData;
        private LoginData _auth = null;
        #endregion

        #region [Public properties]

		public LoginResultType LoginResultType { get; set; }

        public CxWSResponseLoginData CxWSResponseLoginData
        {
            get { return _cxWSResponseLoginData; }
            set { _cxWSResponseLoginData = value; }
        }

        /// <summary>
        /// Get or set service perform status
        /// </summary>
        public bool IsSuccesfull
        {
            get { return _isSuccesfull; }
            set { _isSuccesfull = value; }
        }

        /// <summary>
        /// Get or set saml
        /// </summary>
        public bool IsSaml
        {
            get { return _isSaml; }
            set { _isSaml = value; }
        }

        /// <summary>
        /// Current user session identifier
        /// </summary>
        public string SessionId
        {
            get { return _sessionId; }
            set { _sessionId = value; }
        }

        /// <summary>
        /// User authentification data
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

        #region [Static methods]

        /// <summary>
        /// Convert LoginResult object from xml
        /// </summary>
        /// <param name="xml">xml string</param>
        /// <returns></returns>
        public static LoginResult FromXml(string xml)
        {
            return XmlHelper.ParseLoginResult(xml);
        }

        #endregion
    }
}
