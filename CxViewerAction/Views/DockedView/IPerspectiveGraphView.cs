using System;
using CxViewerAction.BaseInterfaces;
using CxViewerAction.Entities.WebServiceEntity;
using CxViewerAction.Entities;
using CxViewerAction.CxVSWebService;

namespace CxViewerAction.Views.DockedView
{
    /// <summary>
    /// Represent main method-properties for perspective graph view
    /// </summary>
    public interface IPerspectiveGraphView : ISelectableProblem
    {
        /// <summary>
        /// Graph to represent on view
        /// </summary>
        IGraph Graph { get; set; }

        /// <summary>
        /// Find path in graph by query item data
        /// </summary>
        /// <param name="queryItem"></param>
        /// <returns></returns>
        IGraphPath FindPath(CxWSResultPath queryItem);

        /// <summary>
        /// Generate view representation
        /// </summary>
        void BindData();

        /// <summary>
        /// Gets or sets event handler on path item button click
        /// </summary>
        EventHandler PathItemClick { get; set; }
    }
}
