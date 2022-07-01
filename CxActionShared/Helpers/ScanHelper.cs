using System;
using System.Collections.Generic;
using CxViewerAction.Dispatchers;
using CxViewerAction.Entities.WebServiceEntity;
using CxViewerAction.Commands;
using CxViewerAction.Entities;
using CxViewerAction.Services;
using CxViewerAction.Presenters;
using CxViewerAction.Views;
using System.Windows.Forms;
using CxViewerAction.Entities.Enum;
using CxViewerAction.Entities.FormEntity;
using System.Threading;
using CxViewerAction.CxVSWebService;
using Common;

namespace CxViewerAction.Helpers
{
    /// <summary>
    /// Helper class for scan execution
    /// </summary>
    internal class ScanHelper : IScanHelper
    {

        static IDispatcher _dispatcher;
        static Scan _scan;
        static bool _cancelPressed;
        static Upload _uploadSettings;
        readonly IConfigurationHelper _configurationHelper;


        public ScanHelper(IConfigurationHelper configurationHelper)
        {
            _configurationHelper = configurationHelper;
        }

        /// <summary>
        /// Execute scan
        /// </summary>
        /// <param name="project">Upload project folder</param>
        /// <param name="isIncremental"></param>
        /// <param name="scanData"></param>
        /// <param name="scanId"></param>
        /// <returns></returns>
        public ProjectScanStatuses DoScan(Project project, bool isIncremental, ref CxWSQueryVulnerabilityData[] scanData, ref long scanId)
        {
            Logger.Create().Info("DoScan():");
            if (_scan != null && _scan.InProcess)
                {
                return ProjectScanStatuses.CanceledByUser;
            }
            LoginResult loginResult = new LoginResult();
            try
            {
                //Release old view data

                CommonActionsInstance.getInstance().ClearScanProgressView();
                Logger.Create().Info("Released old view data.");
                LoginData logindata = LoginHelper.LoadSaved();
                OidcLoginData oidcLoginData = OidcLoginData.GetOidcLoginDataInstance();
                //Execute login
                if (oidcLoginData.AccessToken == null)
                {
                    loginResult = Login();
                    if (loginResult == null || loginResult.AuthenticationData == null)
                    {
                        LoginHelper.ShowLoginErrorMessage(loginResult);
                        return ProjectScanStatuses.Error;
                    }
                }
                else
                {
                    loginResult.AuthenticationData = logindata;
                    loginResult.IsSuccesfull = true;
                }
                
                if (_cancelPressed)
                {
                    return ProjectScanStatuses.CanceledByUser;
                }

                if (loginResult.IsSuccesfull)
                {
                    _scan = new Scan(loginResult, onScanInBackground, onCancel, onDetails)
                    {
                        InProcess = true,
                        DockView = CommonActionsInstance.getInstance().ScanProgressView,
                        ScanProject = project,
                        IsIncremental = isIncremental
                    };

                    //Execute setting upload project properties
                    _uploadSettings = GetUploadSettings(project, loginResult);

                    if (_uploadSettings == null || _cancelPressed)
                        return ProjectScanStatuses.CanceledByUser;

                    if (_uploadSettings.IsUploading)
                    {
                        _scan.UploadSettings = _uploadSettings;

                        return ExecuteScan(project, ref scanData, ref scanId);
                    }
                }
                else if (!_cancelPressed)
                {
                    TopMostMessageBox.Show("Unable to connect to server or user creadentials are invalid. Please verify data", "Log in problem");
                    return ProjectScanStatuses.Error;
                }
            }
            finally
            {
                if (_scan != null)
                    _scan.InProcess = false;
            }

            return ProjectScanStatuses.CanceledByUser;
        }

        private Upload GetUploadSettings(Project project, LoginResult loginResult)
        {
            Upload uploadSettings;
            Logger.Create().Info("Getting upload settings.");
            if (!CommonData.IsProjectBound)
            {
                uploadSettings = UploadHelper.SetUploadSettings(loginResult, project, _cancelPressed);

                if (uploadSettings == null)
                {
                    LoginHelper.DoLogout();
                    _scan.InProcess = false;
                }
            }
            else
            {
                uploadSettings = new Upload
                {
                    ProjectName = CommonData.ProjectName,
                    IsUploading = true,
                    IsPublic = CommonData.IsProjectPublic
                };
            }
            return uploadSettings;
        }

        private LoginResult Login()
        {
            Logger.Create().Info("Login for scan operation.");
            LoginResult loginResult = LoginHelper.DoLoginWithoutForm(out _cancelPressed, true);

            if (!loginResult.IsSuccesfull)
            {
                loginResult = LoginHelper.DoLogin(out _cancelPressed);
            }

            return loginResult;
        }

        private bool SetScanPrivacy()
        {
            bool isPublic = false;
            DialogResult result = TopMostMessageBox.Show("Make scan results visible to other users", "Results visibility", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                isPublic = true;
            }

            return isPublic;
        }

        /// <summary>
        /// Execute scan
        /// </summary>
        /// <param name="project">Upload project folder</param>
        /// <param name="scanData"></param>
        /// <param name="scanId"></param>
        /// <returns></returns>
        private ProjectScanStatuses ExecuteScan(Project project, ref CxWSQueryVulnerabilityData[] scanData, ref long scanId)
        {
            Logger.Create().Info("DoScan in");
            bool bCancel = false;
            bool backgroundMode = _scan.LoginResult.AuthenticationData.IsRunScanInBackground == SimpleDecision.Yes;

            if (_dispatcher == null)
                _dispatcher = ServiceLocators.ServiceLocator.GetDispatcher();

            if (_dispatcher != null)
            {
                IScanView view = null;
                var waitEnd = new ManualResetEvent(false);

                //if was selected "always run in background" checkbox - hide dialog
                if (!backgroundMode)
                {                    
                    ICommandResult commandResult = _dispatcher.Dispatch(_scan);
                    view = ((ScanPresenter)commandResult).View;
                }

                _scan.ScanView = view;

                BackgroundWorkerHelper bg = new BackgroundWorkerHelper(_scan.LoginResult.AuthenticationData.ReconnectInterval * 1000, _scan.LoginResult.AuthenticationData.ReconnectCount);

                CxWebServiceClient client = new CxWebServiceClient(_scan.LoginResult.AuthenticationData);
                client.ServiceClient.Timeout = 1800000;

                bool isIISStoped = false;
                bool isScanningEror = false;

                //User click cancel while info dialog was showed
                if (!bCancel)
                {
                    ShowScanProgressBar();

                    ConfigurationResult configuration = _configurationHelper.GetConfigurationList(_scan.LoginResult.SessionId, bg, client);
                    
                    if (configuration == null)
                        _cancelPressed = true;

                    if (!configuration.IsSuccesfull)
                    {
                        LoginHelper.DoLogout();
                        if (client != null)
                            client.Close();
                        if (view != null)
                            view.CloseView();

                        _scan.InProcess = false;
                        return ProjectScanStatuses.CanceledByUser;
                    }

                    //User click cancel while info dialog was showed
                    if (!bCancel)
                    {
                        Logger.Create().Info("Zipping the proeject.");
                        byte[] zippedProject = ZipProject(_scan, project, bg);
                        Logger.Create().Info("Zipping is complete.");

                        if (!_scan.IsCancelPressed && zippedProject != null)
                        {
                            if (configuration.Configurations.Count > 0)
                            {
                                RunScanResult runScanResult = null;

                                if (!CommonData.IsProjectBound)
                                {
                                    if (_uploadSettings.IsPublic)
                                    {
                                        _scan.IsPublic = SetScanPrivacy();
                                    }

                                    runScanResult = RunScan(bg, client, configuration, zippedProject);

                                }
                                else
                                {
                                    if (_scan.UploadSettings.IsPublic)
                                    {
                                        _scan.IsPublic = SetScanPrivacy();
                                    }

                                    runScanResult = RunBoundedProjectScan(_scan, bg, client, zippedProject);
                                }

                                if (runScanResult == null || !runScanResult.IsSuccesfull)
                                {
                                    bCancel = true;
                                    isIISStoped = true;
                                    isScanningEror = true;
                                }

                                // Continue if project uploaded succesfull and cancel button while process wasn't pressed
                                if (runScanResult != null && runScanResult.IsSuccesfull)
                                {
                                    _scan.RunScanResult = runScanResult;

                                    //perform scan work in separated thread to improve UI responsibility
                                    System.Threading.ThreadPool.QueueUserWorkItem(delegate(object stateInfo)
                                    {
                                        try
                                        {
                                            // Wait while scan operation complete
                                            Logger.Create().Info("Wait till scan operation complete.");
                                            while (true)
                                            {
                                                StatusScanResult statusScan = UpdateScanStatus(ref bCancel, backgroundMode, view, bg, client, ref isIISStoped);

                                                // if scan complete with sucess or failure or cancel button was pressed
                                                // operation complete
                                                Logger.Create().Info("If scan complete with sucess or failure or cancel button was pressed operation complete.");
                                                bCancel = bCancel ? bCancel : _scan.WaitForCancel();
                                                
                                                if (isIISStoped || bCancel ||
                                                    (statusScan != null && statusScan.RunStatus == CurrentStatusEnum.Finished) ||
                                                    (statusScan != null && statusScan.RunStatus == CurrentStatusEnum.Failed))
                                                {
                                                    break;
                                                }
                                            }

                                            waitEnd.Set();
                                        }
                                        catch (Exception err)
                                        {
                                            Logger.Create().Error(err.ToString());
                                            // show error
                                            waitEnd.Set();
                                            isIISStoped = true;
                                            Logger.Create().Debug("Error: " +err);

                                        }

                                        if (_scan.ScanView == null || _scan.ScanView.Visibility == false)
                                        {
                                            var scanStatusBar = new ScanStatusBar(false, "", 0, 0, true);

                                            CommonActionsInstance.getInstance().UpdateScanProgress(scanStatusBar);

                                            //ObserversManager.Instance.Publish(typeof (ScanStatusBar), scanStatusBar);
                                        }
                                    });

                                    while (!waitEnd.WaitOne(0, false))
                                    {
                                        Application.DoEvents();
                                        Thread.Sleep(10);
                                    }
                                }
                            }

                            #region [Scan completed. Open perspective]

                            if (!bCancel && !isIISStoped)
                            {
                                if (scanData == null)
                                {
                                    TopMostMessageBox.Show("There are no vulnerabilities to show.");
                                }
                                ShowScanData(ref scanData, ref scanId, client);
                            }
                            else
                            {
                                #region [Stop scan in cancel pressed]
                                if (_scan.RunScanResult != null && !isIISStoped)
                                {
                                    bg.DoWorkFunc = delegate
                                    {
                                        if (!isIISStoped)
                                        {
                                            client.ServiceClient.CancelScan(_scan.LoginResult.SessionId, _scan.RunScanResult.ScanId);
                                        }
                                    };
                                    bg.DoWork("Stop scan...");
                                }
                                #endregion
                            }

                            #endregion

                            client.Close();
                        }
                        else
                        {
                            client.Close();
                            bCancel = true;
                        }
                    }
                    else
                    {
                    }

                }
                else
                {
                }
                if (!backgroundMode && view != null)
                    view.CloseView();

                if (isIISStoped)
                {
                   
                    if (isScanningEror)
                        return ProjectScanStatuses.Error;
                    else
                        return ProjectScanStatuses.CanceledByUser;
                }

                if (!bCancel)
                    return ProjectScanStatuses.Success;
                else
                {
                    if (isScanningEror)
                        return ProjectScanStatuses.Error;
                    else
                        return ProjectScanStatuses.CanceledByUser;
                }
            }
            
            return ProjectScanStatuses.CanceledByUser;
        }

        private void ShowScanData(ref CxWSQueryVulnerabilityData[] scanData, ref long scanId, CxWebServiceClient client)
        {
            // Get url to scanned project result
            string savedFileName = string.Format("report{0}", Guid.NewGuid());
            long id = 0;
            CxWSResponseScanStatus scanStatus = client.ServiceClient.GetStatusOfSingleScan(_scan.LoginResult.SessionId, _scan.RunScanResult.ScanId);
            string ScanTaskId = scanStatus.ScanId.ToString();//after scan is finished the server replaces the scan id with task scan id
            scanData = PerspectiveHelper.GetScanResultsPaths(ScanTaskId, ref scanId);
            id = scanId;
            LoginData.BindProject bindProject = _scan.LoginResult.AuthenticationData.BindedProjects.Find(project => project.BindedProjectId == CommonData.ProjectId &&
                                                                                                                    project.ProjectName == CommonData.ProjectName &&
                                                                                                                    project.RootPath == CommonData.ProjectRootPath);

            if (bindProject == null) // scaned for the first time
            {
                bindProject = new LoginData.BindProject();
                bindProject.ProjectName = CommonData.ProjectName;
                bindProject.RootPath = CommonData.ProjectRootPath;
                bindProject.IsBound = true;
                _scan.LoginResult.AuthenticationData.BindedProjects.Add(bindProject);
            }
            if (!CommonData.IsProjectBound) // its new project
            {

                bindProject.BindedProjectId = scanStatus.ProjectId;
                CommonData.ProjectId = scanStatus.ProjectId;
                CommonData.IsProjectPublic = scanStatus.IsPublic;
                bindProject.IsPublic = scanStatus.IsPublic;
                bindProject.IsBound = true;
            }
            bindProject.SelectedScanId = id;
            bindProject.ScanReports = new List<ScanReportInfo>();
            LoginHelper.Save(_scan.LoginResult.AuthenticationData);


            BackgroundWorkerHelper bgWork = new BackgroundWorkerHelper(delegate
            {

                CommonData.SelectedScanId = id;
                String path = PerspectiveHelper.GetScanXML(id);
                ScanReportInfo scanReportInfo = new ScanReportInfo();
                scanReportInfo.Path = path;
                scanReportInfo.Id = id;
                LoginData.BindProject projectToUpdate = _scan.LoginResult.AuthenticationData.BindedProjects.Find(delegate(LoginData.BindProject bp)
                {
                    return bp.BindedProjectId == CommonData.ProjectId;
                }
                );

                projectToUpdate.ScanReports = new List<ScanReportInfo>();
                projectToUpdate.ScanReports.Add(scanReportInfo);

                LoginHelper.Save(_scan.LoginResult.AuthenticationData);
            });

            bgWork.DoWork();
        }

        private StatusScanResult UpdateScanStatus(ref bool bCancel, bool backgroundMode, IScanView view, BackgroundWorkerHelper bg, CxWebServiceClient client, ref bool isIISStoped)
        {
            // Get current scan status
            CxWSResponseScanStatus cxWSResponseScanStatus = null;
            StatusScanResult statusScan = null;

            bg.DoWorkFunc = delegate(object obj)
            {
                cxWSResponseScanStatus = client.ServiceClient.GetStatusOfSingleScan(_scan.LoginResult.SessionId, _scan.RunScanResult.ScanId);
                statusScan = new StatusScanResult();
                statusScan.CurrentStage = cxWSResponseScanStatus.CurrentStage;
                statusScan.CurrentStagePercent = cxWSResponseScanStatus.CurrentStagePercent;
                statusScan.Details = cxWSResponseScanStatus.StepDetails;
                statusScan.IsSuccesfull = cxWSResponseScanStatus.IsSuccesfull;
                statusScan.QueuePosition = cxWSResponseScanStatus.QueuePosition;
                statusScan.RunId = cxWSResponseScanStatus.RunId;
                statusScan.RunStatus = cxWSResponseScanStatus.CurrentStatus;
                statusScan.StageMessage = cxWSResponseScanStatus.StageMessage;
                statusScan.StageName = cxWSResponseScanStatus.StageName;
                statusScan.StepMessage = cxWSResponseScanStatus.StepMessage;
                statusScan.TimeFinished = cxWSResponseScanStatus.TimeFinished != null ? cxWSResponseScanStatus.TimeFinished.ToString() : null;
                statusScan.TimeStarted = cxWSResponseScanStatus.TimeScheduled != null ? cxWSResponseScanStatus.TimeScheduled.ToString() : null;
                statusScan.TotalPercent = cxWSResponseScanStatus.TotalPercent;
            };
            bCancel = !bg.DoWork(null);

            if (!BackgroundWorkerHelper.IsReloginInvoked)
            {
                isIISStoped = !_scan.LoginResult.AuthenticationData.SaveSastScan;
            }
            else
            {
                BackgroundWorkerHelper.IsReloginInvoked = false;
            }

            if (!bCancel && cxWSResponseScanStatus != null && !isIISStoped)
            {
                ScanProgress progress = new ScanProgress(
                    _scan.UploadSettings.ProjectName,
                    statusScan.RunStatus.ToString(),
                    statusScan.StageName,
                    statusScan.StepMessage,
                    statusScan.CurrentStagePercent,
                    0,
                    100,
                    statusScan.TotalPercent);

                if (!backgroundMode)
                    view.Progress = progress;
                try {
                    CommonActionsInstance.getInstance().ScanProgressView.Progress = progress;
                }
                catch (Exception ex) {
                    
                    Logger.Create().Error(ex.ToString());
                }

                if (statusScan.RunStatus == CurrentStatusEnum.Failed)
                {
                    TopMostMessageBox.Show(statusScan.StageMessage, "Scan Error");
                    bCancel = true;
                }

                if (_scan.ScanView == null || _scan.ScanView.Visibility == false)
                {
                    var scanStatusBar = new ScanStatusBar(true, string.Format("Scaning project {0}: {1} {2}",
                        _scan.UploadSettings.ProjectName,
                        statusScan.StepMessage, statusScan.RunStatus), statusScan.TotalPercent, 100);

                    CommonActionsInstance.getInstance().UpdateScanProgress(scanStatusBar);
                }
            }
            return statusScan;
        }

        private RunScanResult RunBoundedProjectScan(Scan scan, BackgroundWorkerHelper bg, CxWebServiceClient client, byte[] zippedProject)
        {
            RunScanResult runScanResult = null;
            bg.DoWorkFunc = delegate(object obj)
            {
                ProjectSettings projectSettings = new ProjectSettings();
                projectSettings.projectID = CommonData.ProjectId;
                LocalCodeContainer localCodeContainer = new LocalCodeContainer();
                localCodeContainer.FileName = "zipCxViewer";
                localCodeContainer.ZippedFile = zippedProject;
                try
                {
                    CxWSResponseRunID cxWSResponseRunID;

                    if (_scan.IsIncremental)
                    {
                        cxWSResponseRunID = client.ServiceClient.RunIncrementalScan(
                        scan.LoginResult.SessionId,
                        projectSettings,
                        localCodeContainer
                        , CommonData.IsProjectPublic,
                        scan.IsPublic
                        );
                    }
                    else
                    {
                        cxWSResponseRunID = client.ServiceClient.RunScanAndAddToProject(
                        scan.LoginResult.SessionId,
                        projectSettings,
                        localCodeContainer
                        , CommonData.IsProjectPublic,
                        scan.IsPublic
                        );
                    }

                    runScanResult = new RunScanResult(); // RunScanResult.FromXml(scanZipedSource);  
                    runScanResult.IsSuccesfull = cxWSResponseRunID.IsSuccesfull;
                    runScanResult.ScanId = cxWSResponseRunID.RunId;//Server actually returns the scanId which is a long number (and not the runID)
                    scan.LoginResult.AuthenticationData.UnboundRunID = cxWSResponseRunID.RunId;
                    CommonData.ProjectId = cxWSResponseRunID.ProjectID;
                    _scan.RunScanResult = runScanResult;
                    if (!cxWSResponseRunID.IsSuccesfull)
                    {                        
                        TopMostMessageBox.Show(string.Format("Scan Error: {0}", cxWSResponseRunID.ErrorMessage), "Scanning Error", MessageBoxButtons.OK);
                    }
                    else
                    {
                        LoginHelper.Save(scan.LoginResult.AuthenticationData);
                    }
                }
                catch (Exception err)
                {
                    Logger.Create().Error(err.ToString());
                    TopMostMessageBox.Show(string.Format("Scan Error: {0}", err.Message), "Scanning Error", MessageBoxButtons.OK);
                }

            };
            if (!bg.DoWork("Upload project zipped source for scanning..."))
                return null;

            return runScanResult;
        }

        private void ShowScanProgressBar()
        {
            if (_scan != null && _scan.UploadSettings != null && (_scan.ScanView == null || _scan.ScanView.Visibility == false))
            {
                var scanStatusBar = new ScanStatusBar(true,
                    string.Format("Scaning project {0}: {1}",
                        _scan.UploadSettings.ProjectName,
                        "..."), 0, 100);

                CommonActionsInstance.getInstance().UpdateScanProgress(scanStatusBar);
            }
        }

        private byte[] ZipProject(Scan scan, Project project, BackgroundWorkerHelper bg)
        {
            byte[] zippedProject = null;
            bg.DoWorkFunc = delegate(object obj)
            {
                string error = string.Empty;
                zippedProject = ZipHelper.Compress(project, scan.LoginResult.AuthenticationData.ExcludeFileExt, scan.LoginResult.AuthenticationData.ExcludeFolder, scan.LoginResult.AuthenticationData.MaxZipFileSize * 1048576, out error);
                if (zippedProject == null)
                {
                    TopMostMessageBox.Show(string.Format("Zip Error: {0}", error), "Zip Error", MessageBoxButtons.OK);
                }

            }; //Convert mb to byte
            bg.DoWork("Zip project before sending...");

            return zippedProject;
        }

        private RunScanResult RunScan(BackgroundWorkerHelper bg, CxWebServiceClient client, ConfigurationResult configuration, byte[] zippedProject)
        {
            RunScanResult runScanResult = null;

            bg.DoWorkFunc = delegate(object obj)
            {
                ProjectSettings projectSettings = new ProjectSettings();
                projectSettings.AssociatedGroupID = _scan.UploadSettings.Team.ToString();
                projectSettings.PresetID = _scan.UploadSettings.Preset;
                projectSettings.ProjectName = _scan.UploadSettings.ProjectName;
                projectSettings.ScanConfigurationID = configuration.FirstConfigurationKey;
                LocalCodeContainer localCodeContainer = new LocalCodeContainer();
                localCodeContainer.FileName = "zipCxViewer";
                localCodeContainer.ZippedFile = zippedProject;
                try
                {
                    CxWSResponseRunID cxWSResponseRunID;
                    if (_scan.IsIncremental)
                    {
                        cxWSResponseRunID = client.ServiceClient.RunIncrementalScan(
                        _scan.LoginResult.SessionId,
                        projectSettings,
                        localCodeContainer
                        , _scan.UploadSettings.IsPublic, _scan.IsPublic
                        );
                    }
                    else
                    {
                        cxWSResponseRunID = client.ServiceClient.CreateAndRunProject(
                        _scan.LoginResult.SessionId,
                        projectSettings,
                        localCodeContainer
                        , _scan.UploadSettings.IsPublic, _scan.IsPublic
                        );
                    }
                    

                    runScanResult = new RunScanResult();
                    runScanResult.IsSuccesfull = cxWSResponseRunID.IsSuccesfull;
                    runScanResult.ScanId = cxWSResponseRunID.RunId;//Server actually returns the scanId which is a long number (and not the runID)
                    _scan.LoginResult.AuthenticationData.UnboundRunID = cxWSResponseRunID.RunId;
                    runScanResult.ProjectId = cxWSResponseRunID.ProjectID;
                    _scan.RunScanResult = runScanResult;
                    if (!cxWSResponseRunID.IsSuccesfull)
                    {
                        TopMostMessageBox.Show(string.Format("Scan Error: {0}", cxWSResponseRunID.ErrorMessage), "Scanning Error", MessageBoxButtons.OK);
                    }
                    else
                    {
                        Logger.Create().Info("Scan completed successfully.");
                        LoginHelper.Save(_scan.LoginResult.AuthenticationData);
                    }
                }
                catch (Exception err)
                {
                    Logger.Create().Error(err.ToString());

                    TopMostMessageBox.Show(string.Format("Scan Error: {0}", err.Message), "Scanning Error", MessageBoxButtons.OK);
                }

            };
            Logger.Create().Info("Uploading project zipped source for scanning.");
            if (!bg.DoWork("Upload project zipped source for scanning..."))
                return null;

            return runScanResult;
        }

        /// <summary>
        /// Handler for ScanInBackground button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void onScanInBackground(object sender, EventArgs e)
        {
            if (_scan != null && _scan.ScanView != null)
                _scan.ScanView.Visibility = false;
        }

        /// <summary>
        /// Handler for Cancel button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void onCancel(object sender, EventArgs e)
        {
            _scan.CancelEvent.Set();
        }

        /// <summary>
        /// Handler for Details button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void onDetails(object sender, EventArgs e)
        {
        }

    }
}
