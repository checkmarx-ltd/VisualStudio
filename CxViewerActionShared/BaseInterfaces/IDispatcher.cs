using CxViewerAction2022.Commands;
using CxViewerAction2022.Entities;

namespace CxViewerAction2022.Dispatchers
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
