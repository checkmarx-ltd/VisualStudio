using System;
using CxViewerAction.Entities;
using CxViewerAction.Commands;

namespace CxViewerAction.Presenters
{
    /// <summary>
    /// Represent main scan view class methods
    /// </summary>
    public interface IScanPresenter : IPresenter, ICommandResult
    {
        /// <summary>
        /// Perform scan
        /// </summary>
        /// <param name="parentView">Parent view</param>
        /// <param name="scanData">Scan data</param>
        void Scan(IView parentView, Scan scanData);
    }
}
