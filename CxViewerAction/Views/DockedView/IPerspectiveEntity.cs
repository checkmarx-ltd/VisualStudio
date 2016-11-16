using System;
using CxViewerAction.Entities.WebServiceEntity;

namespace CxViewerAction.Views.DockedView
{
    /// <summary>
    /// Interface that represent main problematic file entity properties and methods
    /// </summary>
    public interface IPerspectiveProblemFile : IComparable
    {
        /// <summary>
        /// Problem file name
        /// </summary>
        string FileName { get; set; }

        /// <summary>
        /// Problem line position
        /// </summary>
        int Line { get; set; }

        /// <summary>
        /// Problem column position
        /// </summary>
        int Column { get; set; }

        /// <summary>
        /// Problem source code element name
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Length of name param
        /// </summary>
        int Length { get; set; }

        int CurrentPathIndex
        {
            get;
            set;
        }

        TreeNodeData NodeData
        {
            get;
            set;
        }
        /// <summary>
        /// Parent object container
        /// </summary>
        ReportQueryItemResult QueryItem { get; set; }
    }
}
