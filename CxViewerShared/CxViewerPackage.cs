﻿//------------------------------------------------------------------------------
// <copyright file="CxViewerPackage.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;
using CxViewerVSIX.OptionsPages;
using CxViewerVSIX.Commands;
using EnvDTE80;
using CxViewerAction;
using Microsoft.VisualStudio.Shell.Interop;
using Task = System.Threading.Tasks.Task;


namespace CxViewerVSIX
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    /// 
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "9.00.8", IconResourceID = 400)] // Info on this package for Help/About
    [ProvideMenuResource("Menus.ctmenu", 1)]
    //[ProvideAutoLoad("ADFC4E64-0397-11D1-9F4E-00A0C911004F")]
    [Guid(CxViewerPackage.PackageGuidString)]
    [ProvideOptionPage(typeof(AuthenticationOptionPage),"CxViewer", "Authentication", 0, 0, true)]
    [ProvideOptionPage(typeof(ConnectionOptionPage), "CxViewer", "Connection", 0, 1, true)]
    [ProvideOptionPage(typeof(CompressionOptionPage), "CxViewer", "Compression", 0, 2, true)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    [ProvideToolWindow(typeof(CxViewerVSIX.ToolWindows.ResultsToolWindow))]
    //[ProvideToolWindowVisibility(typeof(CxViewerVSIX.ToolWindows.ResultsToolWindow), VSConstants.UICONTEXT.SolutionExists_string)]
    [ProvideToolWindow(typeof(CxViewerVSIX.ToolWindows.ScanProcessToolWindow))]
    //[ProvideToolWindowVisibility(typeof(CxViewerVSIX.ToolWindows.ScanProcessToolWindow), VSConstants.UICONTEXT.SolutionExists_string)]
    [ProvideToolWindow(typeof(CxViewerVSIX.ToolWindows.ReportToolWindow))]
    //[ProvideToolWindowVisibility(typeof(CxViewerVSIX.ToolWindows.ReportToolWindow), VSConstants.UICONTEXT.SolutionExists_string)]
    [ProvideToolWindow(typeof(CxViewerVSIX.ToolWindows.PathToolWindow))]
    //[ProvideToolWindowVisibility(typeof(CxViewerVSIX.ToolWindows.PathToolWindow), VSConstants.UICONTEXT.SolutionExists_string)]
    [ProvideToolWindow(typeof(CxViewerVSIX.ToolWindows.GraphToolWindow))]
    //[ProvideToolWindowVisibility(typeof(CxViewerVSIX.ToolWindows.GraphToolWindow), VSConstants.UICONTEXT.SolutionExists_string)]
    public sealed class CxViewerPackage : Package
    {
        /// <summary>
        /// CxViewerPackage GUID string.
        /// </summary>
        public const string PackageGuidString = "a0f2f05a-074f-4c52-9af8-a14f1e28cf09";
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ScanCommand"/> class.
        /// </summary>
        public CxViewerPackage()
        {
            // Inside this method you can place any initialization code that does not require
            // any Visual Studio service because at this point the package object is created but
            // not sited yet inside Visual Studio environment. The place to do all the other
            // initialization is the Initialize method.
        }

        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            ScanCommand.Initialize(this);
            BindCommand.Initialize(this);
            ShowStoredResultsCommand.Initialize(this);
            RetrieveResultsCommand.Initialize(this);
            IncrementalScanCommand.Initialize(this);

            Connect connect = new Connect();
            connect.OnConnection(GetDTE());
            

            CommonActions ca = CommonActionsInstance.getInstance();
            ca.ApplicationObject = GetDTE();
        }
#if CxViewer2022
        public DTE2 GetDTE() {
            return GetService(typeof(SDTE)) as DTE2;
        }

#else
        public DTE2 GetDTE()
        {
            return GetService(typeof(SDTE)) as DTE2;
        }
#endif

#endregion

    }
}
