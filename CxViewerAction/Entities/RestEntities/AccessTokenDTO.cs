using Newtonsoft.Json;

namespace CxViewerAction.Entities.RestEntities
{
	class AccessTokenDTO
	{
		[JsonProperty("id_token")]
		private string idToken;

		[JsonProperty("access_token")]
		private string accessToken;

		[JsonProperty("expires_in")]
		private int expiresIn;

		[JsonProperty("token_type")]
		private string tokenType;

		[JsonProperty("refresh_token")]
		private string refreshToken;

		public string IdToken { get => idToken; set => idToken = value; }
		public string AccessToken { get => accessToken; set => accessToken = value; }
		public int ExpiresIn { get => expiresIn; set => expiresIn = value; }
		public string TokenType { get => tokenType; set => tokenType = value; }
		public string RefreshToken { get => refreshToken; set => refreshToken = value; }
	}
}
