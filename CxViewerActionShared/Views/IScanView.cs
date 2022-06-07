﻿using System;
using CxViewerAction2022.Entities;
using CxViewerAction2022.Entities.FormEntity;

namespace CxViewerAction2022.Views
{
    /// <summary>
    /// Represent scan view obligatory properties and methods
    /// </summary>
    public interface IScanView : IView
    {
        #region [Properties]

        /// <summary>
        /// Get or set entity identifier
        /// </summary>
        EntityId EntityId { get; set; }

        /// <summary>
        /// Get or set scan progress
        /// </summary>
        ScanProgress Progress { set; }

        /// <summary>
        /// Get or set to run scan in background mode
        /// </summary>
        bool AlwaysInBackground { get; }

        /// <summary>
        /// Get or set form visibility
        /// </summary>
        bool Visibility { get; set; }
        #endregion

        #region [Methods]

        /// <summary>
        /// Increment progress for num positions
        /// </summary>
        /// <param name="num">position</param>
        void Increment(int num);

        /// <summary>
        ///Clear progress and start from begin
        /// </summary>
        void Clear();
        #endregion

        #region [Events]

        /// <summary>
        /// RunInBackground button handler
        /// </summary>
        EventHandler RunInBackgroundHandler { set; get; }

        /// <summary>
        /// Cancel buttom handler
        /// </summary>
        EventHandler CancelHandler { set; get; }

        /// <summary>
        /// Details button handler
        /// </summary>
        EventHandler DetailsHandler { set; get; }
        #endregion
    }
}
