using CxViewerAction.Entities;
using CxViewerAction.Helpers;

namespace CxViewerAction.MenuLogic
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
            LoginData login = LoginHelper.LoadSaved();
            Entities.Project selectedProject = CommonActionsInstance.getInstance().GetSelectedProject();
            if (selectedProject == null)
            {
                return CommandStatus.CommandStatusUnsupported;
            }
            if (login != null && login.BindedProjects != null)
            {
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
