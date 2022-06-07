using System;

namespace CxViewerAction2022.Views
{
    public interface IWaitView : IView
    {
        #region [Properties]

        /// <summary>
        /// Gets or sets progress title message
        /// </summary>
        string ProgressDialogMessage { get; set; }

        /// <summary>
        /// Gets or sets cancel button handler
        /// </summary>
        EventHandler CancelHandler { set; }

        #endregion
    }
}
