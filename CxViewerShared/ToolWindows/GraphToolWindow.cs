//------------------------------------------------------------------------------
// <copyright file="GraphToolWindow.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using CxViewerAction.Views.DockedView;
using EnvDTE;

namespace CxViewerVSIX.ToolWindows
{
    using System;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using Microsoft.VisualStudio.Shell;

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
    [Guid("10636cbc-e7e2-4d03-b905-e91de94c7cb1")]
    public class GraphToolWindow : ToolWindowPane
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GraphToolWindow"/> class.
        /// </summary>
        public GraphToolWindow() : base(null)
        {
            this.Caption = "CxViewer Graph";

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.

        }

        private PerspectiveGraphCtrl ctrl = new PerspectiveGraphCtrl(); 
        public override IWin32Window Window
        {
            get
            {
                return ctrl;
            }
        }

        private static int id = 0;
        public static int ID
        {
            get { return id; }
        }

        protected override void Dispose(bool disposing)
        {
            PerspectiveGraphCtrl ctrl = this.Window as PerspectiveGraphCtrl;
            ctrl.ClearGraphView();
            base.Dispose(disposing);
        }
    }
}
