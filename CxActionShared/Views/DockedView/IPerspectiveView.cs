using System;
using System.Collections.Generic;

using System.Text;
using CxViewerAction.Entities.WebServiceEntity;

namespace CxViewerAction.Views.DockedView
{
    /// <summary>
    /// Interface that represent main report view problem properties and methods
    /// </summary>
    public interface IPerspectiveView
    {
        #region [Public Events]

        /// <summary>
        /// Event that fired when user select Node in tree
        /// </summary>
        event Action<TreeNodeData> SelectedNodeChanged;

        /// <summary>
        /// Event that fired when user binded project and changed a scan by using UI
        /// </summary>
        event Action<long> SelectedScanChanged;

        /// <summary>
        /// Event that fired when user select view problem description inside control
        /// </summary>
        event EventHandler SelectedReportItemChanged;
        #endregion

        #region [Public Properties]

        /// <summary>
        /// Gets or sets currently selected problem type
        /// </summary>
        ReportQueryResult SelectedReportItem { get; set; }

        /// <summary>
        /// Gets or sets report
        /// </summary>
        ReportResult Report { get; set; }        

        #endregion

        #region[Public Methods]

        void UpdateTreeItemInfo();
        /// <summary>
        /// Bind contols with object values
        /// </summary>
        void BindData();

        /// <summary>
        /// Change control activity and set loading message when control perform data binding
        /// </summary>
        /// <param name="active">Activity state</param>
        /// <param name="loadingMessage">Message to show while control is perform data binding</param>
        void SetActivity(bool active, string loadingMessage);

        /// <summary>
        /// Added scan list control to the perspective control
        /// </summary>
        /// <param name="scanList"></param>
        void SetScanList(Dictionary<string, long> scanList, long selectedValue);

        /// <summary>
        /// Remove scan list control from the perspective control
        /// </summary>
        void RemoveScanList();

        #endregion
    }
}
