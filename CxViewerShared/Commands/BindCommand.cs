//------------------------------------------------------------------------------
// <copyright file="BindCommand.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using Microsoft.VisualStudio.Shell;
using EnvDTE;
using CxViewerAction.MenuLogic;

namespace CxViewerVSIX.Commands
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class BindCommand : CommandBase
    {        
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0102;

        /// <summary>
        /// Bind Project Logic
        /// </summary>
        private BindProjectLogic _logic = new BindProjectLogic();

        /// <summary>
        /// Initializes a new instance of the <see cref="BindCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        private BindCommand(Package package) : base (package)
        {
            
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static BindCommand Instance
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
            Instance = new BindCommand(package);
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
            var bindCommand = sender as OleMenuCommand;
            if(bindCommand.Text == "Unbind")
            {
                _logic.UnBindProject();
            }
            _logic.Act();           
        }


        protected override CommandStatus GetStatus()
        {
            return _logic.GetStatus();
        }

        protected override void OnBeforeQueryStatus(object sender, EventArgs e)
        {
            var bindCommand = sender as OleMenuCommand;
            CommandStatus status = GetStatus();
            if (null != bindCommand)
            {
                if (status != null)
                {
                    if (status == (CommandStatus.CommandStatusSupported | CommandStatus.CommandStatusEnabled)
                         && !_logic.IsBinded)
                    {
                        bindCommand.Text = "Bind...";
                    }
                    else
                    {
                        bindCommand.Text = "Unbind";
                    }
                    bindCommand.Supported = true;
                    bindCommand.Enabled = true;
                }
            }
        }
    }
}
