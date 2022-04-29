using System;
using System.Net;

namespace CxViewerAction.Services
{
    public class CxRESTApiWebRequestCore
    {
        #region Fields

        public const string CX_ORIGIN_CUSTOM_HEADER_NAME = "cxOrigin";
        public string _visualStudioPluginName = "cx-VS";

        #endregion

        #region API

        public HttpWebRequest Create(Uri uri)
        {
            return CreateRequest(uri);
        }

        public HttpWebRequest Create(Uri uri, string httpMethod)
        {
            return CreateRequest(uri, httpMethod);
        }

        #endregion

        #region Private methods

        private HttpWebRequest CreateRequest(Uri uri, string httpMethod = null)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(uri);
            webRequest.Headers[CX_ORIGIN_CUSTOM_HEADER_NAME] = _visualStudioPluginName;
            webRequest.CookieContainer = new CookieContainer();
            webRequest.Credentials = CredentialCache.DefaultNetworkCredentials;
            webRequest.UseDefaultCredentials = true;

            if (!string.IsNullOrEmpty(httpMethod))
            {
                webRequest.Method = httpMethod;
            }

            return webRequest;
        }

        #endregion
    }
}
