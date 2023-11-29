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
                Logger.Create().Debug("Project has not been binded yet.");
                //bind
                if (PerspectiveHelper.LoginToServer() == null)
                {
                    return ActionStatus.Failed;
                }
                Entities.Project project = CommonActionsInstance.getInstance().GetSelectedProject();
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

        public void UnBindProject(bool unBindAllData = false)
        {
            LoginData login = LoginHelper.LoadSaved();
            LoginData.BindProject unBindProject = null;
            if (login.BindedProjects != null)
            {
                if (unBindAllData)
                {
                    login.BindedProjects.Clear();
                    CommonData.IsProjectBound = false;
                    LoginHelper.IsLogged = false;
                    CommonData.IsWorkingOffline = false;
                    Logger.Create().Info("Clearing for current solution bound projects.");
                }
                else
                {
                    Logger.Create().Info("GetSatus():Checking bound projects not empty.");
                    Entities.Project selectedProject2 = CommonActionsInstance.getInstance().GetSelectedProject();
                    foreach (LoginData.BindProject project in login.BindedProjects)
                    {
                        Logger.Create().Info("Checking for current solution bound projects.");
                        if (selectedProject2.RootPath == project.RootPath && selectedProject2.ProjectName == project.ProjectName)
                        {
                            unBindProject = project;
                        }
                    }

                    if (unBindProject != null)
                    {
                        login.BindedProjects.Remove(unBindProject);
                    }
                }
                LoginHelper.Save(login);
                Logger.Create().Info("Saving data in conf file.");
            }
        }

        public CommandStatus GetStatus()
        {
            Logger.Create().Info("Bind operation getting status.");
            CommandStatus status = CommandStatus.CommandStatusNull;
            _isBinded = false;
            status = (CommandStatus)CommandStatus.CommandStatusSupported |
                         CommandStatus.CommandStatusEnabled;
            LoginData login = LoginHelper.LoadSaved();
            Logger.Create().Debug("For bind operation saved login data loaded.");
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
