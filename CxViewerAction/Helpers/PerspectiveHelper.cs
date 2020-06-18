using System;
using System.Threading;
using CxViewerAction.CxVSWebService;
using CxViewerAction.Entities;
using System.Xml.Serialization;
using System.IO;
using CxViewerAction.Services;
using CxViewerAction.Views;
using System.Windows.Forms;
using CxViewerAction.Entities.Enum;
using CxViewerAction.Views.DockedView;
using CxViewerAction.Entities.WebServiceEntity;
using System.Collections.Generic;
using Common;

namespace CxViewerAction.Helpers
{
    /// <summary>
    /// Helper class for perspective manipulation
    /// </summary>
    public class PerspectiveHelper
    {
        #region [ Constants ]

        /// <summary>
        /// Perspective not exist message
        /// </summary>
        private const string _perspectiveNotExist = "Current project perspective not exist";

        #endregion [ Constants ]

        /// <summary>
        /// Check if perspective exist for selected project path
        /// </summary>
        /// <param name="projectPath">project path to find connected perspective</param>
        /// <returns></returns>
        public static bool ExistPerspective(string projectPath)
        {
            // Get logged user credentials and project relation data
            LoginData login = Helpers.LoginHelper.Load(0);

            if (login != null && login.Perspectives != null && !string.IsNullOrEmpty(projectPath))
            {
                string perspective;
                if (login.Perspectives.TryGetValue(projectPath, out perspective))
                    return true;
            }

            TopMostMessageBox.Show(_perspectiveNotExist);
            return false;
        }

        /// <summary>
        /// Execute 'Previous Result' command
        /// </summary>
        public static void DoPrevResult()
        {

            Logger.Create().Debug("DoPrevResult in");
            // Get logged user credentials and project relation data
            LoginData login = Helpers.LoginHelper.Load(0);
            CxWSQueryVulnerabilityData[] perspective = null;
            login.IsOpenPerspective = Entities.Enum.SimpleDecision.Yes;

            LoginData.BindProject bindProject = login.BindedProjects.Find(delegate(LoginData.BindProject bp)
                                    {
                                        return bp.BindedProjectId == CommonData.ProjectId &&
                                               bp.ProjectName == CommonData.ProjectName &&
                                               bp.RootPath == CommonData.ProjectRootPath;
                                    }
                                    );

            if (bindProject != null && bindProject.ScanReports != null && bindProject.ScanReports.Count > 0)
            {
                ScanReportInfo tmp = bindProject.ScanReports.Find(delegate(ScanReportInfo sri)
                {
                    return sri.Id == CommonData.SelectedScanId;
                }
                                    );
                if (tmp == null || tmp.Id == 0)
                {
                    tmp = bindProject.ScanReports[0];
                    CommonData.SelectedScanId = tmp.Id;
                }

                perspective = GetScanResultsPath(tmp.Id);
                bindProject.SelectedScanId = tmp.Id;
                if (!string.IsNullOrEmpty(tmp.Path))
                {
                    StorageHelper.Delete(tmp.Path);
                }

                BackgroundWorkerHelper bgWork = new BackgroundWorkerHelper(delegate
                    {
                        tmp.Path = PerspectiveHelper.GetScanXML(CommonData.SelectedScanId);

                        LoginHelper.Save(login);
                    });

                bgWork.DoWork();




                Dictionary<string, long> list = new Dictionary<string, long>();
                foreach (ScanReportInfo item in bindProject.ScanReports)
                {
                    if (!list.ContainsKey(item.Name))
                    {
                        list.Add(item.Name, item.Id);
                    }
                }
                CommonActionsInstance.getInstance().ReportPersepectiveView.SetScanList(list, tmp.Id);

                if (perspective != null)
                {
                    ShowStored(perspective, login, tmp.Id);
                }

            }
        }

        public static ProjectConfiguration GetProjectConfiguration(long projectId)
        {
            ProjectConfiguration res = null;
            bool cancelPressed;
            LoginResult loginResult = LoginHelper.DoLoginWithoutForm(out cancelPressed, false);
            if (!loginResult.IsSuccesfull)
                loginResult = LoginHelper.DoLogin(out cancelPressed);



            CxWebServiceClient client;
            try
            {
                client = new CxWebServiceClient(loginResult.AuthenticationData);
            }
            catch (Exception e)
            {
                Logger.Create().Error(e.ToString());
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK);
                return null;
            }

            CxWSResponseProjectConfig cXWSResponseResults = client.ServiceClient.GetProjectConfiguration(loginResult.SessionId, projectId);
            if (!cXWSResponseResults.IsSuccesfull)
            {
                // show error message
                MessageBox.Show(cXWSResponseResults.ErrorMessage, "Error", MessageBoxButtons.OK);
                return null;
            }

            res = cXWSResponseResults.ProjectConfig;

            return res;
        }

        public static CxWSQueryVulnerabilityData[] GetScanResultsPath(long scanId)
        {
            CxWSQueryVulnerabilityData[] res = null;
            bool cancelPressed;
            LoginResult loginResult = LoginHelper.DoLoginWithoutForm(out cancelPressed, false);
            if (!loginResult.IsSuccesfull)
                loginResult = LoginHelper.DoLogin(out cancelPressed);



           CxWebServiceClient client;
           try
           {
               client = new CxWebServiceClient(loginResult.AuthenticationData);
           }
           catch (Exception e)
           {
               Logger.Create().Error(e.ToString());
               MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK);
               return null;
           }

           CxWSResponceQuerisForScan cXWSResponseResults = client.ServiceClient.GetQueriesForScan(loginResult.SessionId, scanId);
           if (!cXWSResponseResults.IsSuccesfull)
           {
               // show error message
               MessageBox.Show(cXWSResponseResults.ErrorMessage, "Error", MessageBoxButtons.OK);
               return null;
           }

           res = cXWSResponseResults.Queries;

           return res;
        }

        public static CxWSQueryVulnerabilityData[] GetScanResultsPaths(string scanTaskId, ref long scaId)
        {
            CxWSQueryVulnerabilityData[] res = null;
            bool cancelPressed;
            LoginResult loginResult = LoginHelper.DoLoginWithoutForm(out cancelPressed, false);
            if (!loginResult.IsSuccesfull)
                loginResult = LoginHelper.DoLogin(out cancelPressed);



            CxWebServiceClient client;
            try
            {
                client = new CxWebServiceClient(loginResult.AuthenticationData);
            }
            catch (Exception e)
            {
                Logger.Create().Error(e.ToString());
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK);
                return null;
            }

            //check for format compatability - scan id should be a long number
            long scanTaskIdNum;
            bool resParse = long.TryParse(scanTaskId, out scanTaskIdNum);
            if (!resParse)
            {
                // show error message
                MessageBox.Show("Scan ID is in wrong format", "Error", MessageBoxButtons.OK);
                return null;
            }

            CxWSResponceQuerisForScan cXWSResponseResults = client.ServiceClient.GetQueriesForScan(loginResult.SessionId, scanTaskIdNum);

            if (!cXWSResponseResults.IsSuccesfull)
            {
                // show error message
                MessageBox.Show(cXWSResponseResults.ErrorMessage, "Error", MessageBoxButtons.OK);
                return null;
            }

            res = cXWSResponseResults.Queries;

            scaId = scanTaskIdNum;

            return res;
        }

        public static string GetScanXML(long scanId)
        {
            string path = "";
            bool cancelPressed;
            LoginResult loginResult = LoginHelper.DoLoginWithoutForm(out cancelPressed, false);



            CxWebServiceClient client;
            try
            {
                client = new CxWebServiceClient(loginResult.AuthenticationData);
            }
            catch (Exception e)
            {
                Logger.Create().Error(e.ToString());
                return null;
            }

            string savedFileName = string.Format("report{0}", Guid.NewGuid());
            // create status report 
            CxWSReportRequest reportRequest = new CxWSReportRequest();
            reportRequest.ScanID = scanId;
            reportRequest.Type = CxWSReportType.XML;
            CxWSCreateReportResponse cXWSCreateReportResponse = client.ServiceClient.CreateScanReport(loginResult.SessionId, reportRequest);
            long reportID = cXWSCreateReportResponse.ID;
            int numOfTrials = 0;
            bool resultsObtained = false;
            while (!resultsObtained && numOfTrials < 100)
            {
                CxWSReportStatusResponse cxWSReportStatusResponse = client.ServiceClient.GetScanReportStatus(loginResult.SessionId, reportID);
                if (cxWSReportStatusResponse.IsReady)
                {
                    resultsObtained = true;
                }
                else
                {
                    Thread.Sleep(500); 
                }
                numOfTrials++;
            }
            
            CxWSResponseScanResults cxWSResponseScanResults = client.ServiceClient.GetScanReport(loginResult.SessionId, reportID);
            if (!cxWSResponseScanResults.IsSuccesfull)
            {
                Logger.Create().Error(cxWSResponseScanResults.ErrorMessage);
                return null;
            }

            StorageHelper.Save(cxWSResponseScanResults.ScanResults, savedFileName);
            path = savedFileName;



            return path;
        }

        /// <summary>
        /// Ask user to open perspective now
        /// </summary>
        /// <param name="login">user auth data</param>
        /// <returns></returns>
        internal static bool IsOpenNow(LoginData login)
        {
            OpenPercspectiveDialog dlg = new OpenPercspectiveDialog();
            DialogResult r = dlg.ShowDialog();

            if (dlg.RememberDecision)
            {
                login.IsOpenPerspective = (r == DialogResult.OK) ? CxViewerAction.Entities.Enum.SimpleDecision.Yes : Entities.Enum.SimpleDecision.No;
                LoginHelper.Save(login);
            }

            return r == DialogResult.Yes;
        }

        /// <summary>
        /// Execute 'Show Stored' command
        /// </summary>
        /// <param name="queries"></param>
        /// <param name="login"></param>
        /// <param name="scanId"></param>
        public static void ShowStored(CxWSQueryVulnerabilityData[] queries, LoginData login, long scanId)
        {

                Dictionary<ReportQuerySeverityType, List<CxWSQueryVulnerabilityData>> queriesGroups = new Dictionary<ReportQuerySeverityType, List<CxWSQueryVulnerabilityData>>();
                List<CxWSQueryVulnerabilityData> sev0 = new List<CxWSQueryVulnerabilityData>();
                List<CxWSQueryVulnerabilityData> sev1 = new List<CxWSQueryVulnerabilityData>();
                List<CxWSQueryVulnerabilityData> sev2 = new List<CxWSQueryVulnerabilityData>();
                List<CxWSQueryVulnerabilityData> sev3 = new List<CxWSQueryVulnerabilityData>();

                for (int i = 0; i < queries.Length; i++)
                {
                    CxWSQueryVulnerabilityData cur = queries[i];
                    switch (cur.Severity)
                    {
                        case 0:
                            sev0.Add(cur);
                            break;
                        case 1:
                            sev1.Add(cur);
                            break;
                        case 2:
                            sev2.Add(cur);
                            break;
                        case 3:
                            sev3.Add(cur);
                            break;
                    }
                }
                if (sev3.Count > 0)
                {
                    queriesGroups.Add(ReportQuerySeverityType.High, sev3);
                }
                if (sev2.Count > 0)
                {
                    queriesGroups.Add(ReportQuerySeverityType.Medium, sev2);
                }
                if (sev1.Count > 0)
                {
                    queriesGroups.Add(ReportQuerySeverityType.Low, sev1);
                }
                if (sev0.Count > 0)
                {
                    queriesGroups.Add(ReportQuerySeverityType.Information, sev0);
                }
               

                Dictionary<ReportQuerySeverityType, List<ReportQueryResult>> tree = new Dictionary<ReportQuerySeverityType, List<ReportQueryResult>>();

                foreach (var queryGroup in queriesGroups)
                {
                    List<ReportQueryResult> list = new List<ReportQueryResult>();

                    for (int i = 0; i < queryGroup.Value.Count; i++)
                    {
                        CxWSQueryVulnerabilityData query = queryGroup.Value[i];

                        ReportQueryResult queryResult = new ReportQueryResult()
                        {
                            CweId = (int)query.CWE,
                            Group = query.GroupName,
                            Id = (int)query.QueryId,
                            Name = query.QueryName,
                            Paths = null,
                            Report = null,
                            Severity = (ReportQuerySeverityType)query.Severity,
                            AmountOfResults = query.AmountOfResults,
                            ScanId = scanId,
                            QueryVersionCode = query.QueryVersionCode
                        };

                        list.Add(queryResult);
                    }

                    tree.Add(queryGroup.Key, list);
                }

                ReportResult report = new ReportResult();

                report.Tree = tree;

                var reportWinObject = CommonActionsInstance.getInstance().ReportPersepectiveView;

                reportWinObject.Report = report;

                reportWinObject.BindData();

        }

        public static CxWSSingleResultData[] GetScanResultsForQuery(long scanId, long queryId)
        {
            if (CommonData.IsWorkingOffline)
            {
                return SavedResultsManager.Instance.GetScanResultsForQuery(scanId, queryId);
            }
            CxWSSingleResultData[] res = null;
            bool cancelPressed;
            LoginResult loginResult = LoginHelper.DoLoginWithoutForm(out cancelPressed, false);
            if (!loginResult.IsSuccesfull)
            {
                loginResult = LoginHelper.DoLogin(out cancelPressed);
            }

            
                CxWebServiceClient client;
                try
                {
                    client = new CxWebServiceClient(loginResult.AuthenticationData);
                }
                catch (Exception e)
                {
                    Logger.Create().Error(e.ToString());
                    MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK);
                    return null;
                }

                CxWSResponceScanResults cXWSResponseResults = client.ServiceClient.GetResultsForQuery(loginResult.SessionId, scanId, queryId);
                if (!cXWSResponseResults.IsSuccesfull)
                {
                    // show error message
                    MessageBox.Show(cXWSResponseResults.ErrorMessage, "Error", MessageBoxButtons.OK);
                    return null;
                }

                res = cXWSResponseResults.Results;
            
            return res;
        }

        public static CxWSResultPath GetResultPath(long scanId, long pathId)
        {
            if (CommonData.IsWorkingOffline)
            {
                return SavedResultsManager.Instance.GetResultPath(scanId, pathId);
            }
            CxWSResultPath res = null;
            bool cancelPressed;
            LoginResult loginResult = LoginHelper.DoLoginWithoutForm(out cancelPressed, false);
            if (!loginResult.IsSuccesfull)
            {
                loginResult = LoginHelper.DoLogin(out cancelPressed);
            }

            
                CxWebServiceClient client;
                try
                {
                    client = new CxWebServiceClient(loginResult.AuthenticationData);
                }
                catch (Exception e)
                {
                    Logger.Create().Error(e.ToString());
                    MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK);
                    return null;
                }

                CxWSResponceResultPath cXWSResponseResults = client.ServiceClient.GetResultPath(loginResult.SessionId, scanId, pathId);
                if (!cXWSResponseResults.IsSuccesfull)
                {
                    // show error message
                    MessageBox.Show(cXWSResponseResults.ErrorMessage, "Error", MessageBoxButtons.OK);
                    return null;
                }

                res = cXWSResponseResults.Path;
            
            return res;
        }

        public static CxWSResultPath GetPathCommentsHistory(long scanId, long pathId)
        {
            if (CommonData.IsWorkingOffline)
            {
                return SavedResultsManager.Instance.GetResultPath(scanId, pathId);
            }
            CxWSResultPath res = null;
            bool cancelPressed;
            LoginResult loginResult = LoginHelper.DoLoginWithoutForm(out cancelPressed, false);
            if (!loginResult.IsSuccesfull)
            {
                loginResult = LoginHelper.DoLogin(out cancelPressed);
            }


            CxWebServiceClient client;
            try
            {
                client = new CxWebServiceClient(loginResult.AuthenticationData);
            }
            catch (Exception e)
            {
                Logger.Create().Error(e.ToString());
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK);
                return null;
            }
            CxWSResponceResultPath cXWSResponseResults = client.ServiceClient.GetPathCommentsHistory(loginResult.SessionId, scanId, pathId, CxVSWebService.ResultLabelTypeEnum.Remark);
            if (!cXWSResponseResults.IsSuccesfull)
            {
                // show error message
                MessageBox.Show(cXWSResponseResults.ErrorMessage, "Error", MessageBoxButtons.OK);
                return null;
            }

            res = cXWSResponseResults.Path;

            return res;
        }

        public static CxWSResultPath[] GetResultPathsForQuery(long scanId, long queryId)
        {
            if (CommonData.IsWorkingOffline)
            {
                return SavedResultsManager.Instance.GetResultPathsForQuery(scanId, queryId);
            }
            CxWSResultPath[] res = null;
            bool cancelPressed;
            LoginResult loginResult = LoginHelper.DoLoginWithoutForm(out cancelPressed, false);
            if (!loginResult.IsSuccesfull)
            {
                loginResult = LoginHelper.DoLogin(out cancelPressed);
            }


            CxWebServiceClient client;
            try
            {
                client = new CxWebServiceClient(loginResult.AuthenticationData);
            }
            catch (Exception e)
            {
                Logger.Create().Error(e.ToString());
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK);
                return null;
            }

            CxWSResponseResultPaths cXWSResponseResults = client.ServiceClient.GetResultPathsForQuery(loginResult.SessionId, scanId, queryId);
            if (!cXWSResponseResults.IsSuccesfull)
            {
                // show error message
                MessageBox.Show(cXWSResponseResults.ErrorMessage, "Error", MessageBoxButtons.OK);
                return null;
            }

            res = cXWSResponseResults.Paths;

            return res;
        }


        public static ResultState[] GetResultStateList()
        {
            ResultState[] res = null;
            bool cancelPressed;
            LoginResult loginResult = LoginHelper.DoLoginWithoutForm(out cancelPressed, false);
            if (!loginResult.IsSuccesfull)
            {
                loginResult = LoginHelper.DoLogin(out cancelPressed);
            }


            CxWebServiceClient client;
            try
            {
                client = new CxWebServiceClient(loginResult.AuthenticationData);
            }
            catch (Exception e)
            {
                Logger.Create().Error(e.ToString());
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK);
                return null;
            }

            CxWSResponseResultStateList CxWSResponseResults = client.ServiceClient.GetResultStateList(loginResult.SessionId);
            if (!CxWSResponseResults.IsSuccesfull)
            {
                // show error message
                MessageBox.Show(CxWSResponseResults.ErrorMessage, "Error", MessageBoxButtons.OK);
                return null;
            }

            res = loginResult.CxWSResponseLoginData.AllowedToChangeNotExploitable ? CxWSResponseResults.ResultStateList : RemoveNotExploitableFromArray(CxWSResponseResults.ResultStateList );
            

            return res;
        }

        private static ResultState[] RemoveNotExploitableFromArray(ResultState[] resultStates)
        {
            ResultState [] statesWithoutNotExploitable = new ResultState[resultStates.Length-1];

            int currentFreeIndex= 0;
            foreach (ResultState resultState in resultStates)
                if(resultState.ResultID != 1)
                {
                    statesWithoutNotExploitable[currentFreeIndex] = resultState;
                    currentFreeIndex++;
                }
            return statesWithoutNotExploitable;
        }


        public static AssignUser[] GetProjectAssignUsers()
        {
            if (CommonData.IsWorkingOffline)
            {
                return new AssignUser[0];
            }
            AssignUser[] res = null;
            bool cancelPressed;
            LoginResult loginResult = LoginHelper.DoLoginWithoutForm(out cancelPressed, false);
            if (!loginResult.IsSuccesfull)
            {
                loginResult = LoginHelper.DoLogin(out cancelPressed);
            }


            CxWebServiceClient client;
            try
            {
                client = new CxWebServiceClient(loginResult.AuthenticationData);
            }
            catch (Exception e)
            {
                Logger.Create().Error(e.ToString());
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK);
                return null;
            }
            
            CxWSResponseAssignUsers CxWSResponseResults = client.ServiceClient.GetProjectAssignUsersList(loginResult.SessionId, CommonData.ProjectId);
            if (CommonData.IsProjectBound)
            {
                if (!CxWSResponseResults.IsSuccesfull)
                {
                    // show error message
                    MessageBox.Show(CxWSResponseResults.ErrorMessage, "Error", MessageBoxButtons.OK);
                    return null;
                }

                
            }
            res = CxWSResponseResults.AssignUsers;
            return res;
        }

        public static bool UpdateResultState(ResultStateData[] dataArr)
        {
            if (CommonData.IsWorkingOffline)
            {
                MessageBox.Show("You are working offline. \rCannot update data", "Error", MessageBoxButtons.OK);
                return false;
            }

            CxWSBasicRepsonse res = null;
            try
            {
                CxWebServiceClient client = null;
                bool cancelPressed;
                LoginResult loginResult = LoginHelper.DoLoginWithoutForm(out cancelPressed, false);
                if (!loginResult.IsSuccesfull)
                {
                    loginResult = LoginHelper.DoLogin(out cancelPressed);
                }
                try
                {
                    client = new CxWebServiceClient(loginResult.AuthenticationData);
                }
                catch (Exception e)
                {
                    Logger.Create().Error(e.ToString());
                    MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK);
                    return false;
                }

                foreach (ResultStateData data in dataArr)
                {
                    data.projectId = CommonData.ProjectId;
                }
                res = client.ServiceClient.UpdateSetOfResultState(LoginHelper.SessionId, dataArr);
                
                if (!res.IsSuccesfull)
                {
                    // show error message
                    MessageBox.Show(res.ErrorMessage, "Error", MessageBoxButtons.OK);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.Create().Error(ex.ToString());
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK);
                return false;
            }
            return res.IsSuccesfull;
        }

        public static CxWSResponseScansDisplayData GetScansDisplayData(long selectedProjectId)
        {
            CxWSResponseScansDisplayData res = null;
            bool cancelPressed;
            LoginResult loginResult = LoginHelper.DoLoginWithoutForm(out cancelPressed, false);
            if (!loginResult.IsSuccesfull)
                loginResult = LoginHelper.DoLogin(out cancelPressed);



            CxWebServiceClient client;
            try
            {
                client = new CxWebServiceClient(loginResult.AuthenticationData);
            }
            catch (Exception e)
            {
                Logger.Create().Error(e.ToString());
                if (!String.IsNullOrEmpty(loginResult.LoginResultMessage))
                {
                    MessageBox.Show(loginResult.LoginResultMessage, "Error", MessageBoxButtons.OK);
                }
                else
                {
                    MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK);
                }
                return null;
            }
            if (!loginResult.IsSuccesfull)
            {
                if (!String.IsNullOrEmpty(loginResult.LoginResultMessage))
                {
                    MessageBox.Show(loginResult.LoginResultMessage, "Error", MessageBoxButtons.OK);
                }
                else
                {
                    MessageBox.Show("Unknown error", "Error", MessageBoxButtons.OK);
                }
                return null;
            }

            res = client.ServiceClient.GetScansDisplayData(loginResult.SessionId, selectedProjectId);
            if (!res.IsSuccesfull)
            {
                // show error message
                MessageBox.Show(res.ErrorMessage, "Error", MessageBoxButtons.OK);
                return null;
            }


            return res;
        }

        public static LoginResult LoginToServer()
        {
            LoginResult loginResult = null;
            bool cancelPressed;
            try
            {
                loginResult = LoginHelper.DoLoginWithoutForm(out cancelPressed, false);
                if (!loginResult.IsSuccesfull)
                {
                    loginResult = LoginHelper.DoLogin(out cancelPressed);
                }
            }
            catch (Exception e)
            {
                Logger.Create().Error(e.ToString());
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK);
                return null;
            }
            if (cancelPressed)
            {
                return null;
            }
            if (!loginResult.IsSuccesfull)
            {
                if (loginResult.IsSaml)
                {
                    if (!SamlLoginHelper.errorWasShown)
                    {
                        SamlLoginHelper.errorWasShown = false;
                        showErrorMessage("Unable to connect to the server. Please verify data: server path, saml configuration");
                    }
                }
                else
                {
                    showErrorMessage("Unable to connect to the server or user credentials are invalid. Please verify data: server path, login, password");
                }
                return LoginToServer();
            }
            return loginResult;
        }

        private static void showErrorMessage(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK);
        }
    }
}
