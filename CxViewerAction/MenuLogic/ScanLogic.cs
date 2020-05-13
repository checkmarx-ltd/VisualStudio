using Common;
using CxViewerAction.Entities;
using CxViewerAction.Entities.Enum;
using CxViewerAction.Helpers;
using CxViewerAction.Views;
using CxViewerAction.Views.DockedView;
using EnvDTE;
using EnvDTE80;
using System;
using Microsoft.VisualStudio.Shell;

namespace CxViewerAction.MenuLogic
{
    public class ScanLogic : IMenuLogic
    {
        #region Properties

        public bool IsIncremental { get; set; }

        IScanHelper _scanHelper;

        #endregion Properties

        public ScanLogic()
        {
            _scanHelper = new ScanHelper(new ConfigurationHelper());
        }

        public ActionStatus Act()
        {
            if (PerspectiveHelper.LoginToServer() == null)
            {
                return ActionStatus.Failed;
            }
            LoginData login = LoginHelper.LoadSaved();
            Entities.Project selectedProject = CommonActionsInstance.getInstance().GetSelectedProject();
            if (selectedProject == null)
            {
                return ActionStatus.Failed;
            }
            CommonData.ProjectName = selectedProject.ProjectName;
            CommonData.ProjectRootPath = selectedProject.RootPath;
            if (login != null && login.BindedProjects != null)
            {
                LoginData.BindProject bindPro = login.BindedProjects.Find(delegate (LoginData.BindProject bp)
                {
                    return bp.ProjectName == selectedProject.ProjectName && bp.RootPath == selectedProject.RootPath;
                }
                        );

                if (bindPro != null)
                {
                    CommonData.ProjectId = bindPro.BindedProjectId;
                    CommonData.SelectedScanId = bindPro.SelectedScanId;
                    CommonData.IsProjectBound = bindPro.IsBound;
                    CommonData.IsProjectPublic = bindPro.IsPublic;
                }
                else
                {
                    CommonData.IsProjectBound = false;
                }
            }
            CommonActionsInstance.getInstance().BuildFileMapping();
            CommonActionsInstance.getInstance().ExecuteSystemCommand("File.SaveAll", string.Empty);
            DoScan(selectedProject);
            CommonData.IsWorkingOffline = false;
            return ActionStatus.Success;
        }

        private void DoScan(Entities.Project project)
        {
            try
            {
                if (!LoginHelper.IsScanner)
                {
                    TopMostMessageBox.Show("User is not allowed to scan", "Error");
                    return;
                }

                // verify that was selected correct project
                if (!string.IsNullOrEmpty(project.RootPath))
                {
                    System.Threading.ThreadPool.QueueUserWorkItem(delegate (object state)
                    {
                        try
                        {
                            CxViewerAction.CxVSWebService.CxWSQueryVulnerabilityData[] scanData = null;
                            long scanId = 0;

                            ProjectScanStatuses status = _scanHelper.DoScan(project, IsIncremental, ref scanData, ref scanId);

                            if (status == ProjectScanStatuses.Success)
                            {
                                LoginData login = LoginHelper.LoadSaved();
                                PerspectiveHelper.ShowStored(scanData, login, scanId);
                                CommonActionsInstance.getInstance().ShowReportView();
                            }

                            CommonActionsInstance.getInstance().ClearScanProgressView();
                            CommonActionsInstance.getInstance().CloseScanProgressView();
                        }
                        catch (Exception err)
                        {
                            Logger.Create().Error(err);
                            LoginHelper.DoLogout();
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                TopMostMessageBox.Show(ex.Message, "Error");
                Logger.Create().Error(ex.ToString());
            }
        }

        public CommandStatus GetStatus()
        {
            CommandStatus status;
            if (LoginHelper.IsScanner)
            {
                status = (CommandStatus)CommandStatus.CommandStatusSupported |
                         CommandStatus.CommandStatusEnabled;
            }
            else
            {
                status = (CommandStatus)CommandStatus.CommandStatusSupported;
            }
            return status;
        }
    }
}
