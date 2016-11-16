using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace CxViewerAction.Helpers
{
    public class ObserversManager : IObserversManager
    {
        private static IObserversManager _instance;
        private readonly ConcurrentDictionary<Type, IList<IPluginObserver>> _dicObservers;
        protected ObserversManager()
        {
            _dicObservers = new ConcurrentDictionary<Type, IList<IPluginObserver>>();
        }

        public static IObserversManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ObserversManager();
                }

                return _instance;
            }

        }

        public void Subscribe(Type messageType, IPluginObserver observer)
        {
            IList<IPluginObserver> observersList;
            if (!_dicObservers.TryGetValue(messageType, out observersList))
            {
                _dicObservers.TryAdd(messageType, new List<IPluginObserver> {observer});
            }
            else
            {
                if (!observersList.Contains(observer))
                {
                    observersList.Add(observer);
                }
            }
        }

        public void Publish(Type messageType, object data)
        {
            IList<IPluginObserver> observersList;
            if (_dicObservers.TryGetValue(messageType, out observersList))
            {
                foreach (var pluginObserver in observersList)
                {
                    pluginObserver.Handle(data);
                }
            }
        }

        public void Unsubscribe(Type messageType, IPluginObserver observer)
        {
            IList<IPluginObserver> observersList;
            if (_dicObservers.TryGetValue(messageType, out observersList))
            {
                if (observersList.Contains(observer))
                {
                    observersList.Remove(observer);
                }
            }
        }
    }

    public interface IObserversManager
    {
        void Subscribe(Type messageType, IPluginObserver observer);
        void Publish(Type messageType, object data);
        void Unsubscribe(Type messageType, IPluginObserver observer);
    }

    public interface IPluginObserver
    {
        void Handle(object data);
    }
}
