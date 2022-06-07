//------------------------------------------------------------------------------
// <copyright file="ResultsToolWindow.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using CxViewerAction2022.Views.DockedView;
using EnvDTE;

namespace CxViewerVSIX.ToolWindows
{
    using System;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using Microsoft.VisualStudio.Shell;
    using EnvDTE;

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
    [Guid("14306b97-381f-4f13-8795-cb6fc39aff5d")]
    public class ResultsToolWindow : ToolWindowPane
    {
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ResultsToolWindow"/> class.
        /// </summary>
        public ResultsToolWindow() : base(null)
        {
            this.Caption = "CxViewer Result";

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.
        }

        private PerspectiveResultCtrl ctrl = new PerspectiveResultCtrl();
        public override IWin32Window Window
        {
            get
            {
                return ctrl;
            }
        }

        private static int id = 3;
        public static int ID
        {
            get { return id; }
        }

        protected override void Dispose(bool disposing)
        {
            PerspectiveResultCtrl ctrl = this.Window as PerspectiveResultCtrl;
            // clear? 
            base.Dispose(disposing);
        }
    }
}
