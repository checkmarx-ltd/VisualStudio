using System;
using System.Collections.Generic;
using CxViewerAction.Entities;
using CxViewerAction.CxVSWebService;

namespace CxViewerAction.Views
{
    /// <summary>
    /// Represent upload view obligatory methods and properties
    /// </summary>
    public interface IBindProjectView : IView
    {
        ProjectDisplayData[] ProjectList { get;set;}
        ProjectDisplayData SelectedProject { get;}
        bool isPublic { get; }
        
    }
}
