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
            LoginData loginData = LoginHelper.LoadSaved();
            if (IsTokenExpired(loginData))
            {
                //get the login data again with the new access token
                loginData = LoginHelper.LoadSaved();
            };
            WebRequest request = base.GetWebRequest(uri);
            request.Headers.Clear();
            request.Headers.Add(Constants.AUTHORIZATION_HEADER, Constants.BEARER + loginData.AccessToken);
            if (DisableConnectionOptimizations)
            {
                ((HttpWebRequest)request).ServicePoint.UseNagleAlgorithm = false;
                ((HttpWebRequest)request).ServicePoint.Expect100Continue = false;
                ((HttpWebRequest)request).KeepAlive = false;
                ((HttpWebRequest)request).ServicePoint.ConnectionLimit = 10;
            }
            return request;
        }

        public static bool IsTokenExpired(LoginData loginData)
        {
            bool isExpired = false;
            if (loginData.AccessToken != null)
            {
                long currentTimeInMilli = DateTime.Now.Ticks;
                isExpired = DateTime.Compare(new DateTime(currentTimeInMilli), DateTimeOffset.FromUnixTimeMilliseconds(loginData.AccessTokenExpiration).UtcDateTime) > 0 ? false : true;
                if (isExpired)
                {
                    CxRESTApi cxRestApi = new CxRESTApi(loginData);
                    cxRestApi.getAccessTokenFromRefreshToken(loginData.RefreshToken);
                }
            }
            return isExpired;
        }
    }
}
