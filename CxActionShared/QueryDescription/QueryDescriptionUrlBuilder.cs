using CxViewerAction2022.Helpers;

namespace CxViewerAction2022.QueryDescription
{
    public class QueryDescriptionUrlBuilder
    {
        #region API

        public string Build(int queryId, string queryName, long queryVersionCode, string scheme = null)
        {
            return string.Format("{0}/CxWebClient/ScanQueryDescription.aspx?queryVersionCode={1}&queryID={2}&queryTitle={3}",
                    Common.Text.Text.RemoveTrailingSlash(LoginHelper.PortalConfiguration.WebServer),
                    queryVersionCode,
                    queryId,
                    queryName);
        }

        #endregion
    }
}
