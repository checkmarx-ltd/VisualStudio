using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE;
using CxViewerAction2022.Views.DockedView;
using CxViewerAction2022.Helpers;
using Common;
using Microsoft.VisualStudio.Shell;

namespace CxViewerAction2022.MenuLogic
{
    public class ShowResultLogic : IMenuLogic
    {

        public ActionStatus Act()
        {
            CommonActionsInstance.getInstance().BuildFileMapping();
            DoPrevResult();
            CommonData.IsWorkingOffline = false;
            return ActionStatus.Success;
        }

        /// <summary>
        /// Show previous scanned result for selected project
        /// </summary>
        private void DoPrevResult()
        {

            System.Threading.ThreadPool.QueueUserWorkItem(delegate (object state)
            {
                try
                {

                    CommonActionsInstance.getInstance().ReportDoPrevResults();
                    CommonActionsInstance.getInstance().ShowReportView();
                }
                catch (Exception ex)
                {
                    Logger.Create().Error(ex.ToString());
                }

            });
        }

        public CommandStatus GetStatus()
        {
            return (CommandStatus)CommandStatus.CommandStatusSupported |
                                 CommandStatus.CommandStatusEnabled;
        }
    }
}
