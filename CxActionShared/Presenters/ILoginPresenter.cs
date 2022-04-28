using CxViewerAction.Entities;
using CxViewerAction.Commands;

namespace CxViewerAction.Presenters
{
    /// <summary>
    /// Represent main login view class methods
    /// </summary>
    public interface ILoginPresenter : IPresenter, ICommandResult
    {
        /// <summary>
        /// Perform login
        /// </summary>
        /// <param name="parentView">Parent view</param>
        /// <param name="login">Login data</param>
        void Login(IView parentView, ref LoginData login);
    }
}
