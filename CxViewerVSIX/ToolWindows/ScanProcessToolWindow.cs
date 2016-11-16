//------------------------------------------------------------------------------
// <copyright file="ScanProcessToolWindow.cs" company="Company">
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
    [Guid("a2ba5807-2044-4db7-9ecf-b1fd8d2ed894")]
    public class ScanProcessToolWindow : ToolWindowPane
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScanProcessToolWindow"/> class.
        /// </summary>
        public ScanProcessToolWindow() : base(null)
        {
            this.Caption = "CxViewer Scan Progress";

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.
            //this.Content = ctrl;
            //((Window) Content).IsFloating = false;
        }


        private ScanProcessCtrl ctrl = new ScanProcessCtrl();
        public override IWin32Window Window
        {
            get
            {
                return ctrl;
            }
        }

        private static int id = 4;
        public static int ID
        {
            get { return id; }
        }

        protected override void Dispose(bool disposing)
        {
            ScanProcessCtrl ctrl = this.Window as ScanProcessCtrl;
            ctrl.Clear();
            base.Dispose(disposing);
        }
    }
}
