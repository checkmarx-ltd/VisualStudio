using Common;
using CxViewerAction.Entities;
using CxViewerAction.Helpers;
using CxViewerAction.WebPortal;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;

namespace CxViewerAction.Services
{
    public class CxRESTApiNoneConfiguration
    {
        #region Fields

        private string _restAPIRelativePath = "/cxrestapi/Configurations/None";
        private const string requestContentType = "application/json, text/plain, */*";

        #endregion

        #region API

        public void InitNoneBaseUrl()
        {
            try
            {
                LoginHelper.PortalConfiguration = new CxPortalConfiguration();
                Uri uri = GetLoginUri();
                HttpWebRequest webRequest = GetWebRequest(uri);
                CxPortalConfiguration result = GetWebResponse(webRequest);
                LoginHelper.PortalConfiguration.WebServer = GetWebServerBaseUrl(result.WebServer);
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
                    Logger.Create().Error("CxRESTApiPortalConfiguration->GetPortalBaseUrl: " + ex.ToString());
                }
            }
        }

        public CxPortalConfiguration InitNoneConfigurationDetails()
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
                    Logger.Create().Error("CxRESTApiPortalConfiguration->InitPortalConfigurationDetails: " + ex.ToString());
                }
            }
            return null;
        }
        #endregion

        #region Private methods

        private string GetWebServerBaseUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return LoginHelper.ServerBaseUrl;
            }

            Uri uri = new Uri(LoginHelper.ServerBaseUrl);

            if (!url.StartsWith(uri.Scheme))
            {
                return string.Format("{0}://{1}", uri.Scheme, url);
            }

            return url;
        }

        private HttpWebRequest GetWebRequest(Uri uri)
        {
            HttpWebRequest webRequest = new CxRESTApiWebRequestCore().Create(uri, "GET");
            webRequest.Accept = requestContentType;
            OidcLoginData oidcLoginData = OidcLoginData.GetOidcLoginDataInstance();
            if (CxVSWebServiceWrapper.IsTokenExpired(oidcLoginData))
            {
                //get the login data again with the new access token
                oidcLoginData = OidcLoginData.GetOidcLoginDataInstance();
            };
            webRequest.Headers.Clear();
            webRequest.Headers.Add(Constants.AUTHORIZATION_HEADER, Constants.BEARER + oidcLoginData.AccessToken);
            return webRequest;
        }

        private CxPortalConfiguration GetWebResponse(HttpWebRequest webRequest)
        {
            string responseText = string.Empty;
            CxPortalConfiguration portalConfiguration = new CxPortalConfiguration();
            HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();

            if (webResponse.StatusCode != HttpStatusCode.OK)
            {
                Logger.Create().Error("CxRESTApiPortalConfiguration->HandleWebResponse->Rest API, status message: " + webResponse.StatusDescription);
            }

            using (StreamReader reader = new StreamReader(webResponse.GetResponseStream(), ASCIIEncoding.ASCII))
            {
                responseText = reader.ReadToEnd();
            }

            if (!string.IsNullOrEmpty(responseText))
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                portalConfiguration = (CxPortalConfiguration)javaScriptSerializer.Deserialize(responseText, typeof(CxPortalConfiguration));
            }

            return portalConfiguration;
        }

        private Uri GetLoginUri()
        {
            string url = string.Format("{0}{1}",
                Common.Text.Text.RemoveTrailingSlash(LoginHelper.ServerBaseUrl),
                _restAPIRelativePath);

            return new Uri(url);
        }

        #endregion
    }
}
