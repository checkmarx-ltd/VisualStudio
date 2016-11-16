using System;
using System.Collections.Generic;
using CxViewerAction.Entities.WebServiceEntity;
using CxViewerAction.BaseInterfaces;

namespace CxViewerAction.Views.DockedView
{
    /// <summary>
    /// Interface that represent main problematic file options and events
    /// </summary>
    public interface IPerspectivePathView : ISelectableProblem
    {
        /// <summary>
        /// QueryItem attached to current problem
        /// </summary>
        ReportQueryItemResult QueryItemResult { get; set; }

        /// <summary>
        /// Bind view controls with new data
        /// </summary>
        void BindData(int index);

        void SetZoom();

        void ClearView();
        /// <summary>
        /// Event handler, attached to path buttons
        /// </summary>
        EventHandler PathButtonClickHandler { set; }


    }
}
