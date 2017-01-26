using CxViewerAction.Helpers;

namespace CxViewerAction.QueryDescription
{
    public class QueryDescriptionUrlBuilder
    {
        #region API

        public string Build(int queryId, string queryName, long queryVersionCode, string scheme = null)
        {
            return string.Format("{0}/CxWebClient/ScanQueryDescription.aspx?cxsid={1}&queryVersionCode={2}&queryID={3}&queryTitle={4}",
                    Common.Text.Text.RemoveTrailingSlash(LoginHelper.PortalConfiguration.WebServer),
                    LoginHelper.SessionId,
                    queryVersionCode,
                    queryId,
                    queryName);
        }

        #endregion
    }
}
