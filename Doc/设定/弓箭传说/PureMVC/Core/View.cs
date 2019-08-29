namespace PureMVC.Core
{
    using PureMVC.Interfaces;
    using PureMVC.Patterns;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class View : IView, IDisposable
    {
        protected string m_multitonKey;
        protected IDictionary<string, IMediator> m_mediatorMap;
        protected IDictionary<string, IList<IObserver>> m_observerMap;
        protected static volatile IView m_instance;
        protected static readonly IDictionary<string, IView> m_instanceMap = new Dictionary<string, IView>();
        public const string DEFAULT_KEY = "PureMVC";
        protected const string MULTITON_MSG = "View instance for this Multiton key already constructed!";

        protected View() : this("PureMVC")
        {
        }

        protected View(string key)
        {
            this.m_multitonKey = key;
            this.m_mediatorMap = new Dictionary<string, IMediator>();
            this.m_observerMap = new Dictionary<string, IList<IObserver>>();
            if (m_instanceMap.ContainsKey(key))
            {
                throw new Exception("View instance for this Multiton key already constructed!");
            }
            m_instanceMap[key] = this;
            this.InitializeView();
        }

        public void Dispose()
        {
            RemoveView(this.m_multitonKey);
            this.m_observerMap.Clear();
            this.m_mediatorMap.Clear();
        }

        public static IView GetInstance() => 
            GetInstance("PureMVC");

        public static IView GetInstance(string key)
        {
            if (!m_instanceMap.TryGetValue(key, out IView view))
            {
                view = new View(key);
                m_instanceMap[key] = view;
            }
            return view;
        }

        public virtual bool HasMediator(string mediatorName) => 
            this.m_mediatorMap.ContainsKey(mediatorName);

        protected virtual void InitializeView()
        {
        }

        public virtual void NotifyObservers(INotification notification)
        {
            IList<IObserver> list = null;
            if (this.m_observerMap.ContainsKey(notification.Name))
            {
                IList<IObserver> collection = this.m_observerMap[notification.Name];
                list = new List<IObserver>(collection);
            }
            if (list != null)
            {
                IEnumerator<IObserver> enumerator = list.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    enumerator.Current.NotifyObserver(notification);
                }
            }
        }

        public virtual void RegisterMediator(IMediator mediator)
        {
            object mediatorMap = this.m_mediatorMap;
            lock (mediatorMap)
            {
                if (this.m_mediatorMap.ContainsKey(mediator.MediatorName))
                {
                    return;
                }
                mediator.InitializeNotifier(this.m_multitonKey);
                this.m_mediatorMap[mediator.MediatorName] = mediator;
                IEnumerable<string> listNotificationInterests = mediator.ListNotificationInterests;
                IObserver observer = new Observer("HandleNotification", mediator);
                IEnumerator<string> enumerator = listNotificationInterests.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    string current = enumerator.Current;
                    this.RegisterObserver(current, observer);
                }
                IObserver observer2 = new Observer("PublicNotification", mediator);
                this.RegisterObserver("PUB_NOTIFICATION", observer2);
            }
            mediator.OnRegister();
        }

        public virtual void RegisterObserver(string notificationName, IObserver observer)
        {
            if (!this.m_observerMap.ContainsKey(notificationName))
            {
                this.m_observerMap[notificationName] = new List<IObserver>();
            }
            this.m_observerMap[notificationName].Add(observer);
        }

        public virtual IMediator RemoveMediator(string mediatorName)
        {
            object mediatorMap = this.m_mediatorMap;
            lock (mediatorMap)
            {
                if (!this.m_mediatorMap.ContainsKey(mediatorName))
                {
                    return null;
                }
                IMediator notifyContext = this.m_mediatorMap[mediatorName];
                IEnumerator<string> enumerator = notifyContext.ListNotificationInterests.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    string current = enumerator.Current;
                    this.RemoveObserver(current, notifyContext);
                }
                this.RemoveObserver("PUB_NOTIFICATION", notifyContext);
                this.m_mediatorMap.Remove(mediatorName);
                notifyContext.OnRemove();
                return notifyContext;
            }
        }

        public virtual void RemoveObserver(string notificationName, object notifyContext)
        {
            if (this.m_observerMap.ContainsKey(notificationName))
            {
                IList<IObserver> list = this.m_observerMap[notificationName];
                object obj2 = list;
                lock (obj2)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].CompareNotifyContext(notifyContext))
                        {
                            list.RemoveAt(i);
                            break;
                        }
                    }
                    if (list.Count == 0)
                    {
                        this.m_observerMap.Remove(notificationName);
                    }
                }
            }
        }

        public static void RemoveView(string key)
        {
            if (m_instanceMap.TryGetValue(key, out IView view))
            {
                m_instanceMap.Remove(key);
                view.Dispose();
            }
        }

        public virtual IMediator RetrieveMediator(string mediatorName)
        {
            if (!this.m_mediatorMap.ContainsKey(mediatorName))
            {
                return null;
            }
            return this.m_mediatorMap[mediatorName];
        }

        public IEnumerable<string> ListMediatorNames =>
            this.m_mediatorMap.Keys;

        public static IView Instance =>
            GetInstance("PureMVC");
    }
}

