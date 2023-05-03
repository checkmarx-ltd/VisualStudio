using CefSharp.DevTools.Network;
using Common;
using CxViewerAction.Entities;
using CxViewerAction.Helpers;
using CxViewerAction.WebPortal;
using System;
using System.Net;

namespace CxViewerAction.Services
{
    public class CxRESTApiCommon
    {
        #region Fields

        private string _restAPIRelativePath = "/cxrestapi/";
        private string _apiCallingPath;
        private const string requestContentType = "application/json, text/plain, */*";

        #endregion

        #region ctor
        public CxRESTApiCommon(string apiCallingPath)
        {
            _apiCallingPath = apiCallingPath;
        }
        #endregion

        #region API

        public HttpWebResponse InitPortalBaseUrl()
        {
            try
            {
                LoginHelper.PortalConfiguration = new CxPortalConfiguration();
                Uri uri = GetLoginUri();
                HttpWebRequest webRequest = GetWebRequest(uri);
                return GetWebResponse(webRequest);
            }
            catch (System.Net.WebException ex)
            {
                var response = (System.Net.HttpWebResponse)ex.Response;

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    LoginHelper.PortalConfiguration.WebServer = LoginHelper.ServerBaseUrl;
                }
                else
                {
                    Logger.Create().Error("CxRESTApiCommon-> " + _apiCallingPath + ": " + ex.ToString());
                }
            }
            return null;
        }

        #endregion

        #region Private methods

        private HttpWebRequest GetWebRequest(Uri uri)
        {
            HttpWebRequest webRequest = new CxRESTApiWebRequestCore().Create(uri, "GET");
            webRequest.Accept = requestContentType;
            OidcLoginData oidcLoginData = OidcLoginData.GetOidcLoginDataInstance();
            if (CxVSWebServiceWrapper.IsTokenExpired(oidcLoginData))
            {
                oidcLoginData = OidcLoginData.GetOidcLoginDataInstance();
            };
            webRequest.Headers.Clear();
            webRequest.Headers.Add(Constants.AUTHORIZATION_HEADER, Constants.BEARER + oidcLoginData.AccessToken);
            return webRequest;
        }

        private HttpWebResponse GetWebResponse(HttpWebRequest webRequest)
        {
            HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();

            if (webResponse.StatusCode != HttpStatusCode.OK)
            {
                Logger.Create().Error("CxRESTApiCoomon->HandleWebResponse->Rest API, status message: " + webResponse.StatusDescription);
            }
            return webResponse;
        }

        private Uri GetLoginUri()
        {
            string url = string.Format("{0}{1}",
                Common.Text.Text.RemoveTrailingSlash(LoginHelper.ServerBaseUrl),
                _restAPIRelativePath + _apiCallingPath);

            return new Uri(url);
        }

        #endregion
    }
}
