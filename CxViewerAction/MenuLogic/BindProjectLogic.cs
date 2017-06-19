using System;
using EnvDTE;
using CxViewerAction.Helpers;
using CxViewerAction.Entities;
using Common;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;

namespace CxViewerAction.MenuLogic
{
    public class BindProjectLogic : IMenuLogic
    {
        private bool _isBinded = false;
        public bool IsBinded
        {
            get { return _isBinded; }
        }

        public ActionStatus Act()
        {
            if (!_isBinded)
            {
                //bind
                if (PerspectiveHelper.LoginToServer() == null)
                {
                    return ActionStatus.Failed;
                }
                Entities.Project project = CommonActionsInstance.getInstance().GetSelectedProject();
                CommonData.ProjectName = project.ProjectName;
                CommonData.ProjectRootPath = project.RootPath;
                if (!setBindProject(true))
                {
                    return ActionStatus.Failed;
                }
                CommonData.IsProjectBound = true;
                DoRetrieveResults(project);
                CommonData.IsWorkingOffline = false;
                return ActionStatus.Success;
            }
            else
            {
                //unbind
                if (!setBindProject(false))
                {
                    return ActionStatus.Failed;
                }
                CommonData.IsProjectBound = false;
                CommonData.IsWorkingOffline = false;
                return ActionStatus.Success;
            }
        }

        private bool setBindProject(bool isBound)
        {
            LoginData login = LoginHelper.LoadSaved();
            Entities.Project selectedProject = CommonActionsInstance.getInstance().GetSelectedProject();
            if (selectedProject == null)
            {
                return false;
            }
            foreach (LoginData.BindProject project in login.BindedProjects)
            {
                if (selectedProject.RootPath == project.RootPath && selectedProject.ProjectName == project.ProjectName)
                {
                    project.IsBound = isBound;
                }
            }
            LoginHelper.Save(login);
            return true;
        }

        void DoRetrieveResults(Entities.Project project)
        {
            try
            {

                // First call save all command to save project changes
                CommonActionsInstance.getInstance().ExecuteSystemCommand("File.SaveAll", string.Empty);

                // verify that was selected correct project           
                System.Threading.ThreadPool.QueueUserWorkItem(delegate (object state)
                {
                    try
                    {
                        Helpers.BindProjectHelper.BindProject(project);
                    }
                    catch (Exception ex)
                    {
                        Logger.Create().Error(ex.ToString());
                    }
                });
            }
            catch (Exception ex)
            {
                Logger.Create().Error(ex.ToString());
            }
        }

        public CommandStatus GetStatus()
        {
            CommandStatus status = CommandStatus.CommandStatusNull;           
            _isBinded = false;
            status = (CommandStatus)CommandStatus.CommandStatusSupported |
                         CommandStatus.CommandStatusEnabled;
            LoginData login = LoginHelper.LoadSaved();
            Entities.Project selectedProject = CommonActionsInstance.getInstance().GetSelectedProject();
            if (selectedProject == null)
            {
                return CommandStatus.CommandStatusNull;
            }
            if (login != null && login.BindedProjects != null)
            {
                LoginData.BindProject bindPro = login.BindedProjects.Find(project => project.ProjectName == selectedProject.ProjectName && 
                                                                                     project.RootPath == selectedProject.RootPath && 
                                                                                     project.IsBound == true);

                if (bindPro != null)
                {
                    _isBinded = true;
                    status = (CommandStatus)CommandStatus.CommandStatusUnsupported;
                    //bind is unsupported -> unbind is supported
                }
            }
            return status;
        }
    }
}
