using System;
using System.Collections.Generic;
using CxViewerAction2022.Entities;
using CxViewerAction2022.CxVSWebService;

namespace CxViewerAction2022.Views
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
