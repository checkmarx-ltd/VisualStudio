using System;
using CxViewerAction.ServiceLocators;
using CxViewerAction.Presenters;
using CxViewerAction.Views;
using CxViewerAction.Entities.WebServiceEntity;
using CxViewerAction.Entities;

namespace CxViewerAction.Commands
{
    /// <summary>
    /// Execute command
    /// </summary>
    public static class CommandExecutor
    {
        /// <summary>
        /// Execute login command
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static ICommandResult Login(IEntity entity)
        {
            if (entity.GetType() == typeof(LoginData))
            {
                ILoginPresenter presenter = ServiceLocator.GetObject<ILoginPresenter>(ServiceLocator.GetObject<ILoginView>());
                var login = (LoginData)entity;

                presenter.Login(null, ref login);
                entity = login;
                
                return presenter;
            }

            throw new ArgumentOutOfRangeException("entity", string.Format("{0} Entity is not supported.", entity.GetType()));
        }

        /// <summary>
        /// Execute upload command
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static ICommandResult Upload(IEntity entity)
        {
            if (entity.GetType() == typeof(Upload))
            {
                var uploadData = (Upload)entity;
                LoginResult loginResult = (LoginResult)uploadData.ID.Id;

                IUploadPresenter presenter = ServiceLocator.GetObject<IUploadPresenter>(ServiceLocator.GetObject<IUploadView>(loginResult.SessionId));
                presenter.Upload(null, uploadData);

                return presenter;
            }

            throw new ArgumentOutOfRangeException("entity", string.Format("{0} Entity is not supported.", entity.GetType()));
        }

        /// <summary>
        /// Execute scan command
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static ICommandResult Scan(IEntity entity)
        {
            if (entity.GetType() == typeof(Scan))
            {
                Scan scanData = (Scan)entity;

                IScanPresenter presenter = ServiceLocator.GetObject<IScanPresenter>(ServiceLocator.GetObject<IScanView>(scanData));
                presenter.Scan(null, scanData);

                return presenter;
            }

            throw new ArgumentOutOfRangeException("entity", string.Format("{0} Entity is not supported.", entity.GetType()));
        }

        /// <summary>
        /// Execute scan command
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static ICommandResult RetrieveResultsFromServer(IEntity entity)
        {
            
            IRetrieveResultsFromServerPresenter presenter = ServiceLocator.GetObject<IRetrieveResultsFromServerPresenter>(ServiceLocator.GetObject<IScanView>(string.Empty));
            presenter.BindProject(null, string.Empty);

            return presenter;
        }

        /// <summary>
        /// Execute scan command
        /// </summary>

        /// <param name="entity"></param>
        /// <returns></returns>
        public static ICommandResult BindProject(IEntity entity)
        {
            if (entity.GetType() == typeof(BindProjectEntity))
            {
                var scanData = (BindProjectEntity)entity;

                IBindProjectPresenter presenter = ServiceLocator.GetObject<IBindProjectPresenter>(ServiceLocator.GetObject<IBindProjectView>(string.Empty));
                presenter.BindProject(null, scanData);

                return presenter;
            }

            throw new ArgumentOutOfRangeException("entity", string.Format("{0} Entity is not supported.", entity.GetType()));
        }
    }
}
