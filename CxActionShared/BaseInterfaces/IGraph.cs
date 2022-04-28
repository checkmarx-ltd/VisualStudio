using System;
using System.Drawing;
using System.Collections.Generic;
using CxViewerAction.Entities;
using CxViewerAction.Entities.WebServiceEntity;
using CxViewerAction.Views.DockedView;

namespace CxViewerAction.BaseInterfaces
{
    /// <summary>
    /// Represent base methods-properties graph object
    /// </summary>
    public interface IGraph
    {
        /// <summary>
        /// Gets or sets graph severity
        /// </summary>
        ReportQuerySeverityType Severity { get; set; }

        /// <summary>
        /// Gets or sets current selected graph path
        /// </summary>
        IGraphPath Current { get; set; }

        /// <summary>
        /// Gets graph horizontal length (max horizontal elements)
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Gets graph vertical length (max vertical elements)
        /// </summary>
        int Height { get; }

        /// <summary>
        /// Gets or sets list of path sequances
        /// </summary>
        List<GraphPath> Paths { get; set; }

        /// <summary>
        /// Gets or sets the max node relations comparing to all graph nodes
        /// </summary>
        int MaxRelations { get; }

        /// <summary>
        /// Gets item position in graph matrix start from top-left corner
        /// </summary>
        /// <param name="item">Grapth element</param>
        /// <returns>X-Y position of element inside graph</returns>
        Point GetPosition(IGraphItem item);

        /// <summary>
        /// Perform adding new path to graph and updating item references
        /// </summary>
        /// <param name="path"></param>
        void AddNewPath(IGraphPath path);
    }

    /// <summary>
    /// Represent main methods-properties for path object
    /// </summary>
    public interface IGraphPath : IComparable
    {
        /// <summary>
        /// Gets or sets current selected graph item
        /// </summary>
        IGraphItem Current { get; set; }

        /// <summary>
        /// Problem sequance length
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Gets or sets problems sequance from begin to end
        /// </summary>
        List<GraphItem> DirectFlow { get; set; }

        /// <summary>
        /// Get the position of first path element in comparission 
        /// with most top element in all graph paths
        /// </summary>
        int Top { get; }

        /// <summary>
        /// Gets the vertical position of path in graph matrix
        /// </summary>
        int Left { get; }

        ReportQueryItemResult QueryItemResult { set;get;}
    }

    /// <summary>
    /// Represent main methods-properties for graph path item object
    /// </summary>
    public interface IGraphItem : IComparable, IPerspectiveProblemFile
    {
        /// <summary>
        /// Gets or sets problem file name
        /// </summary>
        string FileName { get; set; }

        /// <summary>
        /// Gets or sets item name
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets problem line position
        /// </summary>
        int Line { get; set; }

        /// <summary>
        /// Gets or sets problem column position
        /// </summary>
        int Column { get; set; }

        /// <summary>
        /// Gets or sets problem text length
        /// </summary>
        int Length { get; set; }

        /// <summary>
        /// Determine that this item exist in other graph paths. 
        /// </summary>
        IGraphItem RelatedTo { get; set; }

        /// <summary>
        /// Determine that this item exist in other graph paths. 
        /// </summary>
        List<GraphItem> RelationsFrom { get; set; }

        /// <summary>
        /// Gets or sets parent item
        /// </summary>
        IGraphPath Parent { get; set; }

        /// <summary>
        /// Gets or sets value indicated that current graph item id connection point
        /// for all same items
        /// </summary>
        bool IsPrimary { get; set; }

        /// <summary>
        /// Gets or sets column element position in graph object
        /// </summary>
        int GraphX { get; set; }

        /// <summary>
        /// Gets or sets row element position in graph object
        /// </summary>
        int GraphY { get; set; }

        string UniqueID { get;}

        bool IsSelected { get;set;}
        bool IsMultiReletions { get;set;}
    }
}
