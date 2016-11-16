using System;
using System.Collections.Generic;

using System.Text;
using CxViewerAction.Entities.WebServiceEntity;
using CxViewerAction.CxVSWebService;
using System.Windows.Forms;

namespace CxViewerAction.Helpers
{
    /// <summary>
    /// Helper class that manipulate with stored problem description
    /// </summary>
    public class ProblemDescriptionHelper
    {
        /// <summary>
        /// Find specified problem description
        /// </summary>
        /// <param name="queryId">Problem identifier</param>
        /// <returns>If file description was found method returns full path to file, otherwise retutn null</returns>
        public static string GetStoredProblem(int queryId)
        {
            QueryDescriptionResult queryResult = new QueryDescriptionResult();
            queryResult.QueryId = queryId;

            string file = queryResult.GetFileDescription();

            if (string.IsNullOrEmpty(file))
            {
                if (CommonData.IsWorkingOffline)
                {
                    MessageBox.Show("You are working offline. \rCannot get data", "Error", MessageBoxButtons.OK);
                    return null;
                }
                BackgroundWorkerHelper bg = new BackgroundWorkerHelper(delegate(object obj)
                {
                    CxWSResponseQueryDescription cxWSResponseQueryDescription = QueryDescriptionResult.GetById(queryResult.QueryId);
                    queryResult = new QueryDescriptionResult();
                    queryResult.Description = cxWSResponseQueryDescription.QueryDescription;
                    queryResult.IsSuccesfull = cxWSResponseQueryDescription.IsSuccesfull;
                    queryResult.QueryId = queryId;
                }, 0, 0);

                if (!bg.DoWork("Downloading description..."))
                    return null;

                if (queryResult.IsSuccesfull && queryResult.Save())
                    file = queryResult.GetFileDescription();
            }

            return file;
        }
    }
}
