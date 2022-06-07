using System.Net;

namespace CxViewerAction2022.ValueObjects
{
    public class CxRESTApiLoginResponse : Results
    {
        #region Properties

        public HttpStatusCode ResponseStatusCode { get; set; }

        #endregion
    }
}