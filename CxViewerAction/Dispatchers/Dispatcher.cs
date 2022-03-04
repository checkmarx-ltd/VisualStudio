using System;
using System.Collections.Generic;
using CxViewerAction.Commands;
using CxViewerAction.Entities;

// using CxViewerAction.CommandExecutors;

namespace CxViewerAction.Dispatchers
{
    /// <summary>
    /// Handler for execution
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public delegate ICommandResult ExecuteCommandHandler(IEntity entity);

    /// <summary>
    /// Represent dispatcher object
    /// </summary>
    public class Dispatcher : IDispatcher
    {
        public static Dictionary<Type, ExecuteCommandHandler> CommandExecutors = new Dictionary<Type, ExecuteCommandHandler>();
        
        #region IDispatcher Members

        /// <summary>
        /// Dispatch command
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ICommandResult Dispatch(IEntity entity)
        {
            ExecuteCommandHandler handler;
            CommandExecutors.TryGetValue(entity.GetType(), out handler);
            
            if(handler == null)
                throw new ArgumentOutOfRangeException(string.Format("{0} entity is not supported.", entity.GetType()));

            return handler.Invoke(entity);
        }

        #endregion
    }
}
