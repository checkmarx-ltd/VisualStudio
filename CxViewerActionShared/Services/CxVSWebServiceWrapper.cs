using System;
using Common;
using System.Net;
using CxViewerAction2022.Entities;
using CxViewerAction2022.Helpers;

namespace CxViewerAction2022.Services
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
                Logger.Create().Info("IsTokenExpired: Token is not null.");
                DateTime tokenTime = DateTimeOffset.FromUnixTimeMilliseconds(oidcLoginData.AccessTokenExpiration).UtcDateTime;
                isExpired = DateTime.Compare(DateTime.UtcNow, tokenTime ) > 0 ? true : false;
                if (isExpired)
                {
                    Logger.Create().Info("Access token has expired. Renewing.");
                    LoginData loginData = LoginHelper.LoadSaved();
                    CxRESTApi cxRestApi = new CxRESTApi(loginData);
                    cxRestApi.getAccessTokenFromRefreshToken(oidcLoginData.RefreshToken);
                    Logger.Create().Info("Access token has expired. Renewed.");
                }
            }
            else
            {
                Logger.Create().Info("IsTokenExpired: Access token is null.");
            }
            return isExpired;
        }
    }
}
