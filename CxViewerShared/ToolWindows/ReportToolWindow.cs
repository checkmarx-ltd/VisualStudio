//------------------------------------------------------------------------------
// <copyright file="ReportToolWindow.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using CxViewerAction.Views.DockedView;
using EnvDTE;

namespace CxViewerVSIX.ToolWindows
{
    using System;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Shell;
    using System.Windows.Forms;
    using Microsoft.VisualStudio.Utilities;

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
    [Guid("adbf8b86-91e9-4f27-ab37-3614f17c594d")]
    public class ReportToolWindow : ToolWindowPane
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReportToolWindow"/> class.
        /// </summary>
        public ReportToolWindow() : base(null)
        {
            this.Caption = "CxViewer Tree";

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.
        }

        private PerspectiveCtrl ctrl;
        override public IWin32Window Window { get { using (DpiAwareness.EnterDpiScope(DpiAwarenessContext.SystemAware)) { if (ctrl == null) ctrl = new PerspectiveCtrl(); return ctrl; } } }

        private static int id = 2;
        public static int ID
        {
            get { return id; }
        }

        protected override void Dispose(bool disposing)
        {
            PerspectiveCtrl ctrl = this.Window as PerspectiveCtrl;
            // clear? 
            base.Dispose(disposing);
        }
    }
}
