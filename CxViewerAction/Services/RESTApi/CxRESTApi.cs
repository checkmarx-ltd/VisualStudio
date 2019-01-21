using System;
using Common;
using System.IO;
using System.Net;
using System.Text;
using CxViewerAction.Entities;
using System.Web.Script.Serialization;
using CxViewerAction.Entities.RestEntities;
using System.Collections;
using Newtonsoft.Json;

namespace CxViewerAction.Services
{
    public class CxRESTApi
    {
        #region Fields

        private LoginData _login;
        private const string requestContentType = "application/x-www-form-urlencoded; charset=UTF-8";
        private const string _messageBodyTemplateTokenFromCode = Constants.GRANT_TYPE_KEY + "={0}&" + Constants.CLIENT_ID_KEY + "={1}&" 
			+ Constants.REDIRECT_URI_KEY + "={2}/&" + Constants.CODE_KEY +"={3}";
        private const string _messageBodyTemplateTokenFromRefreshToken = Constants.GRANT_TYPE_KEY + "={0}&" + Constants.CLIENT_ID_KEY + "={1}&"
            + Constants.REFRESH_TOKEN + "={2}" ;

        #endregion

        #region Ctor

        public CxRESTApi(LoginData login)
        {
            _login = login;
        }

        #endregion

        #region API

        public string Login(string code)
        {
			OidcLoginData oidcLoginData = null;
            Uri uri = GetTokenEndpointUri();
            string messageBody = GetLoginMesageBody(code);
            byte[] messageBodyAsByteArray = GetMesageBodyEncoded(code);
            HttpWebRequest webRequest = CreateWebRequest(uri, messageBody, messageBodyAsByteArray, null);
			HttpWebResponse webResponse = HandleWebResponse(webRequest, "CxRESTApiLogin->Login->Rest API, status message: ", "Login Failed");
			oidcLoginData =  ParseOidcInfo(webResponse);
			_login.AccessToken = oidcLoginData.AccessToken;
			_login.RefreshToken = oidcLoginData.RefreshToken;
			_login.AccessTokenExpiration = oidcLoginData.AccessTokenExpiration;
            Helpers.LoginHelper.Save(_login);
            return _login.AccessToken;

		}

        internal void getAccessTokenFromRefreshToken(string refreshToken)
        {
            OidcLoginData oidcLoginData = null;
            Uri uri = GetTokenEndpointUri();
            string messageBody = GetAccessTokenFromRefreshTokenMessageBody(refreshToken);
            byte[] messageBodyAsByteArray = GetRefTokenMessageBodyEncoded(refreshToken);
            HttpWebRequest webRequest = CreateWebRequest(uri, messageBody, messageBodyAsByteArray, null);
            HttpWebResponse webResponse = HandleWebResponse(webRequest, "CxRESTApiLogin->getAccessTokenFromRefreshToken->Rest API, status message: ", "Session expired. Please login.");
            oidcLoginData = ParseOidcInfo(webResponse);
            _login.AccessToken = oidcLoginData.AccessToken;
            _login.RefreshToken = oidcLoginData.RefreshToken;
            _login.AccessTokenExpiration = oidcLoginData.AccessTokenExpiration;
            Helpers.LoginHelper.Save(_login);
        }

        private byte[] GetRefTokenMessageBodyEncoded(string refreshToken)
        {
            string messageBody = GetAccessTokenFromRefreshTokenMessageBody(refreshToken);
            return Encoding.UTF8.GetBytes(messageBody);
        }

        private string GetAccessTokenFromRefreshTokenMessageBody(string refreshToken)
        {
            return string.Format(_messageBodyTemplateTokenFromRefreshToken, Constants.REFRESH_TOKEN, Constants.CLIENT_VALUE, refreshToken);
        }

        private OidcLoginData ParseOidcInfo(HttpWebResponse webResponse)
		{ 
			AccessTokenDTO jsonResponse = ParseAccessTokenJsonFromResponse(webResponse);
			long accessTokenExpirationInMillis = GetAccessTokenExpirationInMillis(jsonResponse.ExpiresIn);
			return new OidcLoginData(jsonResponse.AccessToken, jsonResponse.RefreshToken, accessTokenExpirationInMillis);
		}

		internal void GetPermissions(string accessToken)
		{
			Uri uri = GetUserInfoUri();
			byte[] messageEmptyBody = Encoding.UTF8.GetBytes("");
			HttpWebRequest webRequest = CreateWebRequest(uri, "", messageEmptyBody, accessToken);
			HttpWebResponse webResponse = HandleWebResponse(webRequest, "CxRESTApiLogin->GetPermissions->Rest API, status message: ", "Login Failed");
			ArrayList sastPermissions = ParseJsonPermissionsFromResponse(webResponse);
			_login.SaveSastScan = sastPermissions.Contains(Constants.SAVE_SAST_SCAN);
			_login.ManageResultsComment = sastPermissions.Contains(Constants.MANAGE_RESULTS_COMMENT);
			_login.ManageResultsExploitability = sastPermissions.Contains(Constants.MANAGE_RESULTS_EXPLOITABILITY);
		}

		private ArrayList ParseJsonPermissionsFromResponse(HttpWebResponse webResponse)
		{
			ArrayList sastPermissions = null;
			using (var reader = new StreamReader(webResponse.GetResponseStream()))
			{
				var json = reader.ReadToEnd();
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.TypeNameHandling = TypeNameHandling.None;
                UserInfoDTO userInfoDTO = JsonConvert.DeserializeObject<UserInfoDTO>(json, settings);
				sastPermissions = userInfoDTO.SastPermissions;
			}
			return sastPermissions;
		}


		#endregion

		#region Private methods

		private HttpWebRequest GetWebRequest(Uri uri, string messageBody, string accessToken)
        {
            HttpWebRequest webRequest = new CxRESTApiWebRequestCore().Create(uri, "POST");
            webRequest.ContentType = requestContentType;
            webRequest.ContentLength = messageBody.Length;
			if (!string.IsNullOrEmpty(accessToken))
			{
				webRequest.Headers[Constants.AUTHORIZATION_HEADER] = Constants.BEARER + accessToken;
			}
			return webRequest;
        }

        private HttpWebRequest CreateWebRequest(Uri uri, string messageBody, byte[] messageBodyAsByteArray, string accessToken)
        {
            HttpWebRequest webRequest = GetWebRequest(uri, messageBody, accessToken);

            using (Stream requestStream = webRequest.GetRequestStream())
            {
				requestStream.Write(messageBodyAsByteArray, 0, messageBody.Length);
                requestStream.Close();
            }

            return webRequest;
        }

        private HttpWebResponse HandleWebResponse(HttpWebRequest webRequest, string logErrorMessage, string errorMessage)
        {
            HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();

            if (webResponse.StatusCode != HttpStatusCode.OK)
            {
				string message = logErrorMessage + webResponse.StatusDescription;
				Logger.Create().Error(message);
				System.Windows.Forms.MessageBox.Show(errorMessage, "Error", System.Windows.Forms.MessageBoxButtons.OK);
				throw new WebException(message);
			}
			return webResponse;
        }

		private long GetAccessTokenExpirationInMillis(int accessTokenExpInSec)
		{
			long currentTimeInMillis = DateTimeOffset.Now.ToUnixTimeMilliseconds();
			long accessTokenExpInMilli = accessTokenExpInSec * 1000;
			return currentTimeInMillis + accessTokenExpInMilli;
		}

		private AccessTokenDTO ParseAccessTokenJsonFromResponse(HttpWebResponse webResponse)
		{
			AccessTokenDTO accessTokenDTO = null;
			using (var reader = new StreamReader(webResponse.GetResponseStream()))
			{
				var json = reader.ReadToEnd();
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.TypeNameHandling = TypeNameHandling.None;
                accessTokenDTO = JsonConvert.DeserializeObject<AccessTokenDTO>(json, settings);
			}
			return accessTokenDTO;
		}

		private string GetLoginMesageBody(string code)
        {
            return string.Format(_messageBodyTemplateTokenFromCode, Constants.AUTHORIZATION_CODE_GRANT_TYPE, Constants.CLIENT_VALUE, _login.ServerBaseUri, code);
        }

        private byte[] GetMesageBodyEncoded(string code)
        {
            string messageBody = GetLoginMesageBody(code);
            return Encoding.UTF8.GetBytes(messageBody);
        }

        private Uri GetTokenEndpointUri()
        {
            string url = string.Format("{0}{1}", Common.Text.Text.RemoveTrailingSlash(_login.ServerBaseUri), Constants.TOKEN_ENDPOINT);

            Uri uri = new Uri(url);

            return uri;
        }

		private Uri GetUserInfoUri()
		{
			string url = string.Format("{0}{1}", Common.Text.Text.RemoveTrailingSlash(_login.ServerBaseUri), Constants.USER_INFO_ENDPOINT);

			Uri uri = new Uri(url);

			return uri;
		}

		#endregion
	}

	

		
}
