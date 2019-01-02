
using System.Collections;
using Newtonsoft.Json;

namespace CxViewerAction.Entities.RestEntities
{
	class UserInfoDTO
	{
		[JsonProperty("sast-permissions")]
		private ArrayList sastPermissions;

		[JsonProperty("sub")]
		private string sub;

		public ArrayList SastPermissions { get => sastPermissions; set => sastPermissions = value; }
		public string Sub { get => sub; set => sub = value; }
	}
}
