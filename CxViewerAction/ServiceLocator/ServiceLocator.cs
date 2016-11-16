using System;
using CxViewerAction.Dispatchers;
using CxViewerAction.Presenters;
using CxViewerAction.Views;

namespace CxViewerAction.ServiceLocators
{
    public static class ServiceLocator
    {
        public static IDispatcher GetDispatcher()
        {
            return new Dispatcher();
        }

        public static T GetObject<T>(params object[] objs) where T : class
        {
            Type type = typeof(T);

            if (type == typeof(ILoginPresenter))
                return new LoginPresenter((ILoginView)objs[0]) as T;
            if (type == typeof(IUploadPresenter))
                return new UploadPresenter((IUploadView)objs[0]) as T;
            if (type == typeof(IScanPresenter))
                return new ScanPresenter((IScanView)objs[0]) as T;
            if (type == typeof(IBindProjectPresenter))
                return new BindProjectPresenter((IBindProjectView)objs[0]) as T;
            if (type == typeof(ILoginView))
                return new LoginFrm() as T;
            if (type == typeof(IUploadView))
                return new UploadFrm((string)objs[0]) as T;
            if (type == typeof(IScanView))
                return new ScanProcessFrm() as T;
            if (type == typeof(IBindProjectView))
                return new frmBindingPrjList() as T;

            return null;
        }
    }
}
