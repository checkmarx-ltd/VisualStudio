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

		public OidcLoginData()
		{
		}

		public OidcLoginData(string accessToken, string refreshToken, long accessTokenExpiration)
		{
			_accessToken = accessToken;
			_refreshToken = refreshToken;
			_accessTokenExpiration = accessTokenExpiration;
		}

		public string AccessToken { get => _accessToken; set => _accessToken = value; }
		public string RefreshToken { get => _refreshToken; set => _refreshToken = value; }
		public long AccessTokenExpiration { get => _accessTokenExpiration; set => _accessTokenExpiration = value; }



	}
}
