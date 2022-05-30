using System;
using CxViewerAction2022.Entities;
using CxViewerAction2022.Commands;

namespace CxViewerAction2022.Presenters
{
    /// <summary>
    /// Represent main scan view class methods
    /// </summary>
    public interface IBindProjectPresenter : IPresenter, ICommandResult
    {
        /// <summary>
        /// Perform scan
        /// </summary>
        /// <param name="parentView">Parent view</param>
        /// <param name="data">data</param>
        void BindProject(IView parentView, BindProjectEntity upload);
    }
}