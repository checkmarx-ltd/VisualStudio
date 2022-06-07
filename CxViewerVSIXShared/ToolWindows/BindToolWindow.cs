//------------------------------------------------------------------------------
// <copyright file="BindToolWindow.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace CxViewerVSIX.ToolWindows
{
    using System;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Shell;
    using CxViewerAction.Views;
    using System.Windows.Forms;

    /// <summary>
    /// This class implements the tool window exposed by this package and hosts a user control.
    /// </summary>
    /// <remarks>
    /// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane,
    /// usually implemented by the package implementer.
    /// <para>
    /// This class derives from the ToolWindowPane class provided from the MPF in order to use its
    /// implementation of the IVsUIElementPane interface.
    /// </para>
    /// </remarks>
    [Guid("79837dd0-fdf7-4a00-8d78-19147dec642b")]
    public class BindToolWindow : ToolWindowPane
    {
        private frmBindingPrjList _window;

        /// <summary>
        /// Initializes a new instance of the <see cref="BindToolWindow"/> class.
        /// </summary>
        public BindToolWindow() : base(null)
        {
            this.Caption = "BindToolWindow";

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.
            //this.Content = new BindToolWindowControl();
            _window = new frmBindingPrjList();
        }

        public override IWin32Window Window
        {
            get
            {
                return this._window;
            }
        }
    }
}
