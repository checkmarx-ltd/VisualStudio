namespace Common
{
    public class Constants
    {
        public const string COL_NAME_NUMBER = "No.";
        public const string COL_NAME_STATUS = "Status";
        public const string COL_NAME_SOURCE_FOLDER = "Source Folder";
        public const string COL_NAME_SOURCE_FILE_NAME = "Source Filename";
        public const string COL_NAME_SOURCE_LINE = "Source Line";
        public const string COL_NAME_SOURCE_OBJECT = "Source Object";
        public const string COL_NAME_DEST_FOLDER = "Destination Folder";
        public const string COL_NAME_DEST_FILE_NAME = "Destination Filename";
        public const string COL_NAME_DEST_LINE = "Destination Line";
        public const string COL_NAME_DEST_OBJECT = "Destination Object";
        public const string COL_NAME_SHOW_PATH = "Result State";
        public const string COL_NAME_SEVERITY = "Severity";
        public const string COL_NAME_ASSIGN = "Assigned User";
        public const string COL_NAME_REMARK = "Comment";

		public const string ERR_TITLE = "Error Message";
		public const string ERR_UNKNOWN = "Cannot establish connection with the server. Unknown error.";
		public const string ERR_UNKNOWN_SERVER = "Cannot establish connection with the server. This may be an issue of incorrect server name or address";
		public const string ERR_UNKNOWN_USER_PASSWORD = "Cannot establish connection with the server. This may be an issue of incorrect user name or password";

		public const string CLIENT_ID_KEY = "client_id";
		public const string CLIENT_VALUE = "ide_client";
		public const string SCOPE_KEY = "scope";
		public const string CODE_KEY = "code";
		public const string GRANT_TYPE_KEY = "grant_type";
		public const string AUTHORIZATION_CODE_GRANT_TYPE = "authorization_code";
		public const string SAST_PREFIX = "/cxrestapi/auth";
		public const string REFRESH_TOKEN = "refresh_token";
		public const string REDIRECT_URI_KEY = "redirect_uri";
		public const string RESPONSE_TYPE_VALUE = "code";
		public const string SCOPE_VALUE = "offline_access openid sast_api sast-permissions";
		public const string AUTHORIZATION_ENDPOINT = SAST_PREFIX + "/identity/connect/authorize";
		public const string AUTHORIZATION_HEADER = "Authorization";
		public const string BEARER = "Bearer ";
		public const string USER_INFO_ENDPOINT = SAST_PREFIX + "/identity/connect/userinfo";
		public const string RESPONSE_TYPE_KEY = "response_type";
		public const string TOKEN_ENDPOINT = SAST_PREFIX + "/identity/connect/token";
		public const string SAVE_SAST_SCAN = "save-sast-scan";
		public const string MANAGE_RESULTS_COMMENT = "manage-result-comment";
		public const string MANAGE_RESULTS_EXPLOITABILITY = "manage-result-exploitability";

	}
}
