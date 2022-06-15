using EnvDTE;
using CxViewerAction2022.Entities;
using CxViewerAction2022.Helpers;
using CxViewerAction2022.CxVSWebService;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Common;

namespace CxViewerAction2022.MenuLogic
{
    public class RetrievedScanResultLogic : IMenuLogic
    {
        public ActionStatus Act()
        {
            if (PerspectiveHelper.LoginToServer() == null)
            {
                return ActionStatus.Failed;
            }
            LoginData login = LoginHelper.LoadSaved();
            Logger.Create().Debug("Getting selected project");
            Entities.Project selectedProject = CommonActionsInstance.getInstance().GetSelectedProject();
            
            if (selectedProject == null)
            {
                return ActionStatus.Failed;
            }
            LoginData.BindProject bindPro = null;
            if (login != null && login.BindedProjects != null)
            {
                Logger.Create().Debug("Found selected project " + selectedProject.ProjectName);
                bindPro = login.BindedProjects.Find(delegate (LoginData.BindProject bp)
                {
                    return bp.ProjectName == selectedProject.ProjectName && bp.RootPath == selectedProject.RootPath && bp.IsBound == true;
                }
                        );

                if (bindPro != null)
                {
                    CommonData.ProjectId = bindPro.BindedProjectId;
                    CommonData.ProjectName = selectedProject.ProjectName;
                    CommonData.ProjectRootPath = selectedProject.RootPath;
                }
                else
                {
                    return ActionStatus.Failed;
                }
            }

            RetrieveResultsFromServer(bindPro, login);

            return ActionStatus.Success;
        }

        private void RetrieveResultsFromServer(LoginData.BindProject bindPro, LoginData login)
        {
            Logger.Create().Debug("Retrieving  results from server.");
            CxWSResponseScansDisplayData cxWSResponseScansDisplayData = PerspectiveHelper.GetScansDisplayData(CommonData.ProjectId);
            if (cxWSResponseScansDisplayData == null)
            { //error occured
                return;
            }
            if (cxWSResponseScansDisplayData.ScanList.Length == 0)
            {
                // show error about 0 scan list
                System.Windows.Forms.MessageBox.Show("The chosen project doesn't contain scans", "Error", System.Windows.Forms.MessageBoxButtons.OK);
                return;
            }
            bindPro.ScanReports.Clear();
            Logger.Create().Debug("Received scan results data. Size=" + cxWSResponseScansDisplayData.ScanList.Length);
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

                bindPro.AddScanReport(scanReportInfo);
            }
            LoginHelper.Save(login);


            ShowResultLogic showResultLogic = new ShowResultLogic();
            
            showResultLogic.Act();
            

            return;
        }

        public CommandStatus GetStatus()
        {
            LoginData login = LoginHelper.LoadSaved();
            Logger.Create().Debug("Getting selected project");
            Entities.Project selectedProject = CommonActionsInstance.getInstance().GetSelectedProject();
            if (selectedProject == null)
            {
                return CommandStatus.CommandStatusNull;
            }
            if (login != null && login.BindedProjects != null)
            {
                Logger.Create().Debug("Found selected project " + selectedProject.ProjectName);
                LoginData.BindProject bindPro = login.BindedProjects.Find(delegate (LoginData.BindProject bp)
                {
                    return bp.ProjectName == selectedProject.ProjectName && bp.RootPath == selectedProject.RootPath && bp.IsBound == true;
                }
                       );
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
