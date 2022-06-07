using System;
using System.Collections.Generic;
using CxViewerAction2022.Commands;
using CxViewerAction2022.Entities;

// using CxViewerAction2022.CommandExecutors;

namespace CxViewerAction2022.Dispatchers
{
    /// <summary>
    /// Handler for excution
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
