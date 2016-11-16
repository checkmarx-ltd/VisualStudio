using CxViewerAction.Helpers;

namespace CxViewerAction.QueryDescription
{
    public class QueryDescriptionUrlBuilder
    {
        #region API

        public string Build(int queryId, string queryName, string scheme = null)
        {
            return string.Format("{0}/CxWebClient/ScanQueryDescription.aspx?cxsid={1}&queryID={2}&queryTitle={3}",
                    Common.Text.Text.RemoveTrailingSlash(LoginHelper.PortalConfiguration.WebServer),
                    LoginHelper.SessionId,
                    queryId,
                    queryName);
        }

        #endregion
    }
}
