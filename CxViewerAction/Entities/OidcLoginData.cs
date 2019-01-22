using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CxViewerAction.Entities
{
	public class OidcLoginData
	{

		private string _accessToken;
		private string _refreshToken;
		private long _accessTokenExpiration;
        private static OidcLoginData oidcLoginDataInstance;

        public static OidcLoginData GetOidcLoginDataInstance()
		{
            if (oidcLoginDataInstance == null)
                oidcLoginDataInstance = new OidcLoginData();
                return oidcLoginDataInstance;   
        }

        public OidcLoginData() {
        }

		public string AccessToken { get => _accessToken; set => _accessToken = value; }
		public string RefreshToken { get => _refreshToken; set => _refreshToken = value; }
		public long AccessTokenExpiration { get => _accessTokenExpiration; set => _accessTokenExpiration = value; }



	}
}
