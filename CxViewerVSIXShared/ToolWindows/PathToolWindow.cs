//------------------------------------------------------------------------------
// <copyright file="PathToolWindow.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using CxViewerAction2022.Views.DockedView;
using EnvDTE;

namespace CxViewerVSIX.ToolWindows
{
    using System;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Shell;
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
    [Guid("9076c67c-b323-4590-a171-f0a657efa281")]
    public class PathToolWindow : ToolWindowPane
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PathToolWindow"/> class.
        /// </summary>
        public PathToolWindow() : base(null)
        {
            this.Caption = "CxViewer Path";

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.

        }

        private PerspectivePathCtrl ctrl = new PerspectivePathCtrl();
        public override IWin32Window Window
        {
            get
            {
                return ctrl;
            }
        }

        private static int id = 1;
        public static int ID
        {
            get { return id; }
        }

        protected override void Dispose(bool disposing)
        {
            PerspectivePathCtrl ctrl = this.Window as PerspectivePathCtrl;
            ctrl.ClearView();
            base.Dispose(disposing);
        }
    }
}
