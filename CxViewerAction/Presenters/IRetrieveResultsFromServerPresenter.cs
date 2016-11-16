using System;
using CxViewerAction.Entities;
using CxViewerAction.Commands;

namespace CxViewerAction.Presenters
{
    /// <summary>
    /// Represent main scan view class methods
    /// </summary>
    public interface IRetrieveResultsFromServerPresenter : IPresenter, ICommandResult
    {
        /// <summary>
        /// Perform scan
        /// </summary>
        /// <param name="parentView">Parent view</param>
        /// <param name="data">data</param>
        void BindProject(IView parentView, string data);
    }
}