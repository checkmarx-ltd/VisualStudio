﻿using System;
using System.Collections.Generic;

using System.Text;
using CxViewerAction2022.Entities.WebServiceEntity;

namespace CxViewerAction2022.Views.DockedView
{
    /// <summary>
    /// Interface that represent main report view problem properties and methods
    /// </summary>
    public interface IPerspectiveResultView
    {
        #region [Public Events]

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

        bool IsActive { get;set;}

        IPerspectiveView PerspectiveView { get;set;}

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

        #endregion
    }
}
