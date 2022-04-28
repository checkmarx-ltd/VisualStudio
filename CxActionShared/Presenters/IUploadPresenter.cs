using CxViewerAction.Entities;
using CxViewerAction.Commands;

namespace CxViewerAction.Presenters
{
    /// <summary>
    /// Represent main upload view class methods
    /// </summary>
    public interface IUploadPresenter : IPresenter, ICommandResult
    {
        /// <summary>
        /// Perform project upload
        /// </summary>
        /// <param name="parentView">Parent view</param>
        /// <param name="upload">Upload data</param>
        void Upload(IView parentView, Upload upload);
    }
}
