//------------------------------------------------------------------------------
// <copyright file="RetrieveResultsCommand.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using Microsoft.VisualStudio.Shell;
using EnvDTE;
using CxViewerVSIX.ToolWindows;
using CxViewerAction.MenuLogic;

namespace CxViewerVSIX.Commands
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class RetrieveResultsCommand : CommandBase
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0103;

        /// <summary>
        /// Retrieved Scan Result Logic
        /// </summary>
        private RetrievedScanResultLogic _logic = new RetrievedScanResultLogic();
        
        /// <summary>
        /// Initializes a new instance of the <see cref="RetrieveResultsCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        private RetrieveResultsCommand(Package package) : base (package)
        {
           
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static RetrieveResultsCommand Instance
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
            Instance = new RetrieveResultsCommand(package);
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
            _logic.Act();

            // Get the instance number 0 of this tool window. This window is single instance so this instance
            // is actually the only one.
            // The last flag is set to true so that if the tool window does not exists it will be created.
            ToolWindowPane window = _package.FindToolWindow(typeof(ReportToolWindow), ReportToolWindow.ID, true);
            ShowToolWindow(window);
        }

        protected override CommandStatus GetStatus()
        {
            return _logic.GetStatus();
        }
    }
}
