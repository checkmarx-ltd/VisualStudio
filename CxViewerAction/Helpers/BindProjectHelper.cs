using System;
using System.Collections.Generic;
using CxViewerAction.Entities.WebServiceEntity;
using CxViewerAction.Entities;
using CxViewerAction.Entities.Enum;
using CxViewerAction.Dispatchers;
using CxViewerAction.Services;
using CxViewerAction.CxVSWebService;
using Common;
using CxViewerAction.MenuLogic;

namespace CxViewerAction.Helpers
{
	public class BindProjectHelper
	{
		#region [Constants]

		/// <summary>
		/// Wait dialog caption message
		/// </summary>
		private const string RETRIEVE_RESULTS_LOADING_TEXT = "Retrieving data...";

		#endregion

		#region [Private Members]

		private static IDispatcher _dispatcher;
		private static bool _canceled;
		static bool isSelectingChanging;
		static bool isSelectionBlocked;

		#endregion [Private Members]

		#region [Public Members]

		public static bool IsSelectingChanging
		{
			get { return isSelectingChanging; }
			set { isSelectingChanging = value; }
		}

		public static bool IsSelectionBlocked
		{
			get { return isSelectionBlocked; }
			set { isSelectionBlocked = value; }
		}

		#endregion


		#region Private Methods

        internal static ProjectScanStatuses BindProject(Entities.Project project)
		{
            Logger.Create().Debug("BindProjectByType in");
			ProjectScanStatuses status = ProjectScanStatuses.CanceledByUser;
			try
			{
                
                    status = LoginAndBindSelectedProject(project);
                if (status == ProjectScanStatuses.Success)
                {

                    ShowResultLogic showResultLogic = new ShowResultLogic();
                    showResultLogic.Act();

                    status = ProjectScanStatuses.Success;
                }
                else if (status == ProjectScanStatuses.CanceledByUser)
                {
                    //Do nothing...
                    CommonData.IsProjectBound = false;
                }
                else
                {
                    TopMostMessageBox.Show("Unable to retrieve results.", "Error", System.Windows.Forms.MessageBoxButtons.OK);
                }

                CommonActionsInstance.getInstance().ClearScanProgressView();

                CommonActionsInstance.getInstance().CloseScanProgressView();
             
            }
            catch (Exception err)
			{
                Logger.Create().Error(err);
			}
			return status;
		}

        static ProjectScanStatuses LoginAndBindSelectedProject(Entities.Project project)
		{

            OidcLoginData oidcLoginData = OidcLoginData.GetOidcLoginDataInstance();
            LoginData loginData = LoginHelper.LoadSaved();
            LoginResult loginResult = new LoginResult();
            bool cancelPressed = false;
            if (oidcLoginData.AccessToken == null)
            {
                //Execute login
                loginResult = LoginHelper.DoLoginWithoutForm(out cancelPressed, false);
                if (!loginResult.IsSuccesfull)
                    loginResult = LoginHelper.DoLogin(out cancelPressed);

            }
            else
            {
                loginResult.AuthenticationData = loginData;
                loginResult.IsSuccesfull = true;
            }

            if (loginResult.IsSuccesfull)
			{
				_canceled = false;
                BindSelectedProject(loginResult, project);
				if (!_canceled)
					return ProjectScanStatuses.Success;
				else
					return ProjectScanStatuses.CanceledByUser;
			}
			else if (!cancelPressed)
			{
				TopMostMessageBox.Show("Unable to connect to server.", "Log in problem");
				return ProjectScanStatuses.Error;
			}

			return ProjectScanStatuses.CanceledByUser;
		}

        static void BindSelectedProject(LoginResult loginResult, Entities.Project project)
        {
            CxWSResponseProjectsDisplayData cxWSResponseProjectsDisplayData = null;
            // show bind project form
            CxWebServiceClient client = null;
            bool isThrewError = false;
            BackgroundWorkerHelper bg = new BackgroundWorkerHelper(delegate
            {
                try
                {
                    client = new CxWebServiceClient(loginResult.AuthenticationData);
                }
                catch (Exception e)
                {
                    Logger.Create().Error(e.ToString());
                    System.Windows.Forms.MessageBox.Show(e.Message, "Error", System.Windows.Forms.MessageBoxButtons.OK);
                    isThrewError = true;
                    return;
                }

                if (client == null)
                {
                    System.Windows.Forms.MessageBox.Show("Cannot connect to server", "Error", System.Windows.Forms.MessageBoxButtons.OK);
                    isThrewError = true;
                    return;
                }
                Logger.Create().Debug("Getting project display data.");
                cxWSResponseProjectsDisplayData = client.ServiceClient.GetProjectsDisplayData(loginResult.SessionId);
                Logger.Create().Debug("Received project display data. Count "+ cxWSResponseProjectsDisplayData.projectList.Length);

            }, loginResult.AuthenticationData.ReconnectInterval * 1000, loginResult.AuthenticationData.ReconnectCount);

            //Show wait dialog and perform server request in different thread to safe UI responsibility
            if (!bg.DoWork(RETRIEVE_RESULTS_LOADING_TEXT))
                return;

            if (cxWSResponseProjectsDisplayData == null || !cxWSResponseProjectsDisplayData.IsSuccesfull || isThrewError)
                return;

            var bindProjectEntity = new BindProjectEntity {CxProjectsDisplayData = cxWSResponseProjectsDisplayData};

            #region show Select Project window

            //            if (projectID < 0)
            //            {
            if (_dispatcher == null)
                _dispatcher = ServiceLocators.ServiceLocator.GetDispatcher();

            if (_dispatcher != null)
            {
                _dispatcher.Dispatch(bindProjectEntity);
            }


            if (bindProjectEntity.CommandResult == System.Windows.Forms.DialogResult.Cancel)
            {
                _canceled = true;
                return;
            }
            //            }

            #endregion

            long selectedProjectId = 0;
            if (client != null && ((bindProjectEntity.SelectedProject != null && bindProjectEntity.CommandResult == System.Windows.Forms.DialogResult.OK)))
            {
                Logger.Create().Info("Loading project id: " + bindProjectEntity.SelectedProject.projectID);
                bg.DoWorkFunc = delegate(object obj)
                               {
                                   selectedProjectId = bindProjectEntity.SelectedProject.projectID;
                                   if (loginResult.AuthenticationData.BindedProjects == null)
                                   {
                                       loginResult.AuthenticationData.BindedProjects = new List<LoginData.BindProject>();
                                   }

                                   LoginData.BindProject bindProject = loginResult.AuthenticationData.BindedProjects.Find(delegate(LoginData.BindProject bp)
                                   {
                                       return bp.ProjectName == project.ProjectName && bp.RootPath == project.RootPath;
                                   }
                                    );
                                   bool isNewProject = true;
                                   if (bindProject != null)
                                   {
                                       bindProject.BindedProjectId = selectedProjectId;
                                       bindProject.ScanReports = new List<ScanReportInfo>();
                                       bindProject.IsBound = true;
                                       bindProject.SelectedScanId = 0;
                                       bindProject.IsPublic = bindProjectEntity.isPublic;
                                       isNewProject = false;
                                   }
                                   else
                                   {
                                       bindProject = new LoginData.BindProject()
                                       {
                                           BindedProjectId = selectedProjectId,
                                           RootPath = project.RootPath,
                                           ProjectName = project.ProjectName,
                                           ScanReports = new List<ScanReportInfo>(),
                                           IsPublic = bindProjectEntity.isPublic,
                                           IsBound = true,
                                       };
                                       isNewProject = true;
                                   }

                                   Logger.Create().Info("Getting Scans display data for selected project " + selectedProjectId);
                                   CxWSResponseScansDisplayData cxWSResponseScansDisplayData = PerspectiveHelper.GetScansDisplayData(selectedProjectId);
                                   if (cxWSResponseScansDisplayData.ScanList.Length == 0)
                                   {
                                       // show error about 0 scan list
                                       System.Windows.Forms.MessageBox.Show("The chosen project doesn't contain scans", "Error", System.Windows.Forms.MessageBoxButtons.OK);
                                       isThrewError = true;
                                       return;
                                   }

                                   Logger.Create().Info("Received Scans display data for selected project. Count " + cxWSResponseScansDisplayData.ScanList.Length);
                                   foreach (ScanDisplayData item in cxWSResponseScansDisplayData.ScanList)
                                   {

                                       // Add relation to scanned project and scan report
                                       ScanReportInfo scanReportInfo = new ScanReportInfo { Id = item.ScanID };
                                       string minutes = item.QueuedDateTime.Minute.ToString().Length > 1 ? item.QueuedDateTime.Minute.ToString() : "0" + item.QueuedDateTime.Minute;

                                       scanReportInfo.Name = string.Format("{0}/{1}/{2} {3}:{4}", item.QueuedDateTime.Month,
                                                                               item.QueuedDateTime.Day,
                                                                               item.QueuedDateTime.Year,
                                                                               item.QueuedDateTime.Hour,
                                                                               minutes);

                                       bindProject.AddScanReport(scanReportInfo);
                                   }

                                   if (bindProject.ScanReports.Count > 0)
                                   {
                                       CommonData.SelectedScanId = bindProject.ScanReports[0].Id;
                                       bindProject.SelectedScanId = CommonData.SelectedScanId;
                                   }

                                   if (isNewProject)
                                   {
                                       loginResult.AuthenticationData.BindedProjects.Add(bindProject);
                                   }
                               };
                bool bCancel = !bg.DoWork("Downloading project data...");

                if (!bCancel && !isThrewError)
                {
                    CommonData.ProjectId = selectedProjectId;
                    LoginHelper.Save(loginResult.AuthenticationData);
                }
            }
        }

		#endregion
	}
}
