using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE;
using CxViewerAction.Entities;
using CxViewerAction.Helpers;
using CxViewerAction.Views;
using CxViewerAction.Views.DockedView;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;

namespace CxViewerAction.MenuLogic
{
    public class ShowSavedScanResultLogic : IMenuLogic
    {
        #region Properties

        #endregion Properties

        public ActionStatus Act()
        {
            LoginData login = LoginHelper.LoadSaved();
            Entities.Project selectedProject = CommonActionsInstance.getInstance().GetSelectedProject();
            if (selectedProject == null)
            {
                return ActionStatus.Failed;
            }
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
                }
            }
            CommonActionsInstance.getInstance().BuildFileMapping();
            ShowSavedResults();
            CommonData.IsWorkingOffline = true;
            return ActionStatus.Success;
        }

        private void ShowSavedResults()
        {
            LoginData login = LoginHelper.LoadSaved();
  
            CxViewerAction.CxVSWebService.CxWSQueryVulnerabilityData[] scanData = null;
            long scanId = 0;
            
            scanId = CommonData.SelectedScanId;
            if (scanId == 0)
            {
                TopMostMessageBox.Show("No Stored Results Found", "Information");
                return;
            }
            scanData = SavedResultsManager.Instance.LoadStoredScanData(scanId);

            if (scanData.Length > 0)
            {
                PerspectiveHelper.ShowStored(scanData, login, scanId);
                CommonActionsInstance.getInstance().ShowReportView();
            }
            else
            {
                TopMostMessageBox.Show("No Stored Results Found", "Information");
            }
        }

        public CommandStatus GetStatus()
        {
            LoginData login = LoginHelper.LoadSaved();
            Entities.Project selectedProject = CommonActionsInstance.getInstance().GetSelectedProject();
            if (selectedProject == null)
            {
                return CommandStatus.CommandStatusNull;
            }
            if (login != null && login.BindedProjects != null)
            {
                LoginData.BindProject bindPro = login.BindedProjects.Find(delegate (LoginData.BindProject bp)
                {
                    return bp.ProjectName == selectedProject.ProjectName && bp.RootPath == selectedProject.RootPath;
                });
                if (bindPro != null)
                {
                   return (CommandStatus)CommandStatus.CommandStatusSupported |
                            CommandStatus.CommandStatusEnabled;
                }
            }
            return (CommandStatus)CommandStatus.CommandStatusSupported;           
        }
    }
}
