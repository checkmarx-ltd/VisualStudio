using CxViewerAction.Commands;
using CxViewerAction.Entities;

namespace CxViewerAction.Dispatchers
{
    /// <summary>
    /// Represent dispatcher class structure
    /// </summary>
    public interface IDispatcher
    {
        /// <summary>
        /// Dispatch command
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        ICommandResult Dispatch(IEntity entity);
    }
}
