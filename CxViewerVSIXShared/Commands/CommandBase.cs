using System;
using System.ComponentModel.Design;
using CxViewerAction2022;
using CxViewerAction2022.MenuLogic;
using CxViewerVSIX.ToolWindows;
using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace CxViewerVSIX.Commands
{
    public abstract class CommandBase
    {
        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid(CxViewerPackageGuids.guidCxViewerPackageCmdSetString);

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        protected readonly Package _package;

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        protected IServiceProvider ServiceProvider
        {
            get
            {
                return _package;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScanCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        protected CommandBase(Package package)
        {
            if (package == null)
            {
                throw new ArgumentNullException("package");
            }

            _package = package;

            OleMenuCommandService commandService = ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null)
            {
                var menuCommandId = new CommandID(CommandSet, GetCommandId);
                var menuItem = new OleMenuCommand(MenuItemCallback, menuCommandId) {Enabled = true};
                menuItem.BeforeQueryStatus += OnBeforeQueryStatus;
                commandService.AddCommand(menuItem);
            }
        }

        protected abstract int GetCommandId { get; }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        protected virtual void MenuItemCallback(object sender, EventArgs e)
        {
            CommonActions ca = CommonActionsInstance.getInstance();

            ca.ScanProgressWin = _package.FindToolWindow(typeof(ScanProcessToolWindow), ScanProcessToolWindow.ID, true);
            ca.ResultWin = _package.FindToolWindow(typeof(ResultsToolWindow), ResultsToolWindow.ID, true);
            ca.ReportWin = _package.FindToolWindow(typeof(ReportToolWindow), ReportToolWindow.ID, true);
            ca.PathWin = _package.FindToolWindow(typeof(PathToolWindow), PathToolWindow.ID, true);
            ca.GraphWin = _package.FindToolWindow(typeof(GraphToolWindow), GraphToolWindow.ID, true);
        }

        protected void ShowToolWindow(ToolWindowPane windowPane)
        {
            ShowView(windowPane);
        }

        protected abstract CommandStatus GetStatus();

        protected virtual void OnBeforeQueryStatus(object sender, EventArgs e)
        {
            OleMenuCommand scanCommand = sender as OleMenuCommand;

            if (scanCommand != null)
            {
                var status = GetStatus();

                scanCommand.Enabled = (status == (CommandStatus.CommandStatusSupported | CommandStatus.CommandStatusEnabled));
                scanCommand.Supported = true;
            }
        }

        protected virtual void InitLogic()
        {
            //ObserversManager.Instance.Subscribe(typeof(ScanStatusBar), this);
        }

        private void ShowView(ToolWindowPane window)
        {
            if ((null == window) || (null == window.Frame))
            {
                throw new NotSupportedException("Cannot create tool window");
            }

            IVsWindowFrame windowFrame = (IVsWindowFrame)window.Frame;
           // Microsoft.VisualStudio.Shell.eErrorHandler.ThrowOnFailure(windowFrame.Show());
        }
    }
}
