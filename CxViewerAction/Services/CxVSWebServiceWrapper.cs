using System;
using Common;
using System.Net;
using CxViewerAction.Entities;
using CxViewerAction.Helpers;

namespace CxViewerAction.Services
{
    public class CxVSWebServiceWrapper : CxVSWebService.CxVSWebService
    {
        public bool DisableConnectionOptimizations = false;

        protected override WebRequest GetWebRequest(Uri uri)
        {
            OidcLoginData oidcLoginData = OidcLoginData.GetOidcLoginDataInstance();
            if (IsTokenExpired(oidcLoginData))
            {
                //get the login data again with the new access token
                oidcLoginData = OidcLoginData.GetOidcLoginDataInstance();
            }
            WebRequest request = base.GetWebRequest(uri);
            request.Headers.Clear();
            request.Headers.Add(Constants.AUTHORIZATION_HEADER, Constants.BEARER + oidcLoginData.AccessToken);
            if (DisableConnectionOptimizations)
            {
                ((HttpWebRequest)request).ServicePoint.UseNagleAlgorithm = false;
                ((HttpWebRequest)request).ServicePoint.Expect100Continue = false;
                ((HttpWebRequest)request).KeepAlive = false;
                ((HttpWebRequest)request).ServicePoint.ConnectionLimit = 10;
            }
            return request;
        }

        public static bool IsTokenExpired(OidcLoginData oidcLoginData)
        {
            bool isExpired = false;
            if (oidcLoginData.AccessToken != null)
            {
                long currentTimeInMilli = DateTime.Now.Ticks;
                isExpired = DateTime.Compare(new DateTime(currentTimeInMilli), DateTimeOffset.FromUnixTimeMilliseconds(oidcLoginData.AccessTokenExpiration).UtcDateTime) > 0 ? false : true;
                if (isExpired)
                {
                    LoginData loginData = LoginHelper.LoadSaved();
                    CxRESTApi cxRestApi = new CxRESTApi(loginData);
                    cxRestApi.getAccessTokenFromRefreshToken(oidcLoginData.RefreshToken);
                }
            }
            return isExpired;
        }
    }
}
