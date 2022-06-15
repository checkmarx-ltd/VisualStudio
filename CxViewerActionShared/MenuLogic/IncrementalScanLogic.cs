using CxViewerAction2022.Entities;
using CxViewerAction2022.Helpers;
using EnvDTE;
using Common;

namespace CxViewerAction2022.MenuLogic
{
    public class IncrementalScanLogic : IMenuLogic
    {
        #region Properties

        private readonly ScanLogic _logic;

        #endregion Properties

        public IncrementalScanLogic()
        {
            _logic = new ScanLogic();
        }

        public ActionStatus Act()
        {
            _logic.IsIncremental = true;
            return _logic.Act();
        }

        public CommandStatus GetStatus()
        {
            Logger.Create().Info("For incremental scan getting status from conf file");
            LoginData login = LoginHelper.LoadSaved();
            Logger.Create().Debug("Getting selected project");
            Entities.Project selectedProject = CommonActionsInstance.getInstance().GetSelectedProject();            
            if (selectedProject == null)
            {
                return CommandStatus.CommandStatusUnsupported;
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
                    return CommandStatus.CommandStatusSupported |
                         CommandStatus.CommandStatusEnabled;
                }
            }
            return CommandStatus.CommandStatusUnsupported;
        }
    }
}
