using CxViewerAction2022.Entities;
using CxViewerAction2022.Commands;

namespace CxViewerAction2022.Presenters
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
