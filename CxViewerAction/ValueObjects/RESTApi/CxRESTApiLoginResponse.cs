using System.Net;

namespace CxViewerAction.ValueObjects
{
    public class CxRESTApiLoginResponse : Results
    {
        #region Properties

        public HttpStatusCode ResponseStatusCode { get; set; }

        #endregion
    }
}