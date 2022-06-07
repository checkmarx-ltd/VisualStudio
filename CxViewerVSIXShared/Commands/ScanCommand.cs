//------------------------------------------------------------------------------
// <copyright file="MyCommand.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using Microsoft.VisualStudio.Shell;
using CxViewerAction2022.MenuLogic;
using CxViewerVSIX.ToolWindows;
using EnvDTE;

namespace CxViewerVSIX.Commands
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class ScanCommand : CommandBase/*: IPluginObserver, IDisposable*/
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        /// Scan Logic.
        /// </summary>
        private ScanLogic _logic = new ScanLogic();
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ScanCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        private ScanCommand(Package package) : base(package)
        {
           
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static ScanCommand Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static void Initialize(Package package)
        {
            Instance = new ScanCommand(package);
        }

        protected override int GetCommandId
        {
            get { return CommandId; }
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        protected override void MenuItemCallback(object sender, EventArgs e)
        {
            base.MenuItemCallback(sender, e);

            _logic.IsIncremental = false;
            _logic.Act();

            ToolWindowPane window = _package.FindToolWindow(typeof(ReportToolWindow), ReportToolWindow.ID, true);
            window.Caption = "CxViewer Scan Progress";

            ShowToolWindow(window);
        }

        protected override CommandStatus GetStatus()
        {
            return _logic.GetStatus();
        }

        //#region IPluginObserver
        //public void Handle(object data)
        //{
        //    var value = data as ScanStatusBar;

        //    if (value == null) return;

        //    IVsStatusbar statusbar = (IVsStatusbar)ServiceProvider.GetService(typeof(ScanStatusBar));

        //    if (value.ClearBeforeUpdateProgress)
        //    {
        //        statusbar.Clear();
        //    }

        //    uint cookie = 0;
        //    statusbar.Progress(ref cookie, value.InProgress ? 1 : 0, value.Label, value.Completed, value.Total);
        //}
        //#endregion

        //public void Dispose()
        //{
        //    ObserversManager.Instance.Unsubscribe(typeof(ScanStatusBar), this);
        //}
    }
}
