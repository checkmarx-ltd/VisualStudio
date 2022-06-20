using System;
using EnvDTE;
using CxViewerAction2022.Helpers;
using CxViewerAction2022.Entities;
using Common;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;

namespace CxViewerAction2022.MenuLogic
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
                Logger.Create().Debug("Project has not been binded yet.");
                //bind
                if (PerspectiveHelper.LoginToServer() == null)
                {
                    return ActionStatus.Failed;
                }
                Entities.Project project = CommonActionsInstance.getInstance().GetSelectedProject();
                Logger.Create().Debug("Getting selected projects");
                CommonData.ProjectName = project.ProjectName;
                CommonData.ProjectRootPath = project.RootPath;
                DoRetrieveResults(project);
                if (CommonData.IsProjectBound)
                {
                    if (!setBindProject(true))
                    {
                        return ActionStatus.Failed;
                    }
                }
                CommonData.IsWorkingOffline = false;
                return ActionStatus.Success;
            }
            else
            {
                //unbind
                if (!setBindProject(false))
                {
                    Logger.Create().Debug("Unbound projects");
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
            Logger.Create().Debug("Set Bind project successful");
            return true;
        }

        void DoRetrieveResults(Entities.Project project)
        {
            try
            {

                // First call save all command to save project changes
                CommonActionsInstance.getInstance().ExecuteSystemCommand("File.SaveAll", string.Empty);
                Logger.Create().Debug("File save all called to save project changes");
                // verify that was selected correct project           
                System.Threading.ThreadPool.QueueUserWorkItem(delegate (object state)
                {
                    try
                    {
                        Helpers.BindProjectHelper.BindProject(project);
                        Logger.Create().Debug("Project bind successful");
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
            Logger.Create().Info("Bind operation getting status.");
            CommandStatus status = CommandStatus.CommandStatusNull;
            _isBinded = false;
            bool currentBind = false;
            status = (CommandStatus)CommandStatus.CommandStatusSupported |
                         CommandStatus.CommandStatusEnabled;
            LoginData login = LoginHelper.LoadSaved();
            Logger.Create().Debug("For bind operation saved login data loaded.");
            ///<summary>
            /// Changes for Plug-488 clear bound project when switching to another solution 
            ///</summary>
            ///Start
            if (login.BindedProjects != null)
            {
                Logger.Create().Info("GetSatus():Checking bound projects not empty.");
                Entities.Project selectedProject2 = CommonActionsInstance.getInstance().GetSelectedProject();
                foreach (LoginData.BindProject project in login.BindedProjects)
                {
                    Logger.Create().Info("Checking for current solution bound projects.");
                    if (selectedProject2.RootPath == project.RootPath && selectedProject2.ProjectName == project.ProjectName)
                    {
                        currentBind = true;
                    }
                }
                CommonData.IsWorkingOffline = false;
                LoginHelper.Save(login);

                if (!currentBind)
                {
                    Logger.Create().Info("Checking for current solution bound projects false.");
                    login.BindedProjects.Clear();
                    Logger.Create().Info("Clearing for current solution bound projects.");
                    CommonData.IsProjectBound = false;
                    LoginHelper.IsLogged = false;
                    LoginHelper.Save(login);
                    Logger.Create().Info("Saving data in conf file.");
                }
            }
            ///End
            Logger.Create().Info("Bind logic getting selected project.");
            Entities.Project selectedProject = CommonActionsInstance.getInstance().GetSelectedProject();
            if (selectedProject == null)
            {
                Logger.Create().Info("Bind logic getting selected project returned null.");
                return CommandStatus.CommandStatusNull;
            }
            if (login != null && login.BindedProjects != null)
            {
                LoginData.BindProject bindPro = login.BindedProjects.Find(project => project.ProjectName == selectedProject.ProjectName &&
                                                                                     project.RootPath == selectedProject.RootPath &&
                                                                                     project.IsBound == true);

                if (bindPro != null)
                {
                    Logger.Create().Debug("BindLogic project is found " + bindPro.ProjectName);
                    _isBinded = true;
                    status = (CommandStatus)CommandStatus.CommandStatusUnsupported;
                    //bind is unsupported -> unbind is supported
                }
            }
            return status;
        }
    }
}
