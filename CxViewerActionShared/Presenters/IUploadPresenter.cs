using CxViewerAction2022.Entities;
using CxViewerAction2022.Commands;

namespace CxViewerAction2022.Presenters
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
