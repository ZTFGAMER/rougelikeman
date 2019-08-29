namespace PureMVC.Patterns
{
    using PureMVC.Core;
    using PureMVC.Interfaces;
    using System;
    using System.Collections.Generic;

    public class Facade : Notifier, IFacade, INotifier, IDisposable
    {
        protected IController m_controller;
        protected IModel m_model;
        protected IView m_view;
        protected static readonly IDictionary<string, IFacade> m_instanceMap = new Dictionary<string, IFacade>();
        public const string DEFAULT_KEY = "PureMVC";
        protected const string MULTITON_MSG = "Facade instance for this Multiton key already constructed!";

        public Facade() : this("PureMVC")
        {
        }

        public Facade(string key)
        {
            base.InitializeNotifier(key);
            m_instanceMap[key] = this;
            this.InitializeFacade();
        }

        public static void BroadcastNotification(INotification notification)
        {
            IEnumerator<KeyValuePair<string, IFacade>> enumerator = m_instanceMap.GetEnumerator();
            while (enumerator.MoveNext())
            {
                KeyValuePair<string, IFacade> current = enumerator.Current;
                current.Value.NotifyObservers(notification);
            }
        }

        public static void BroadcastNotification(string notificationName)
        {
            BroadcastNotification(new Notification(notificationName));
        }

        public static void BroadcastNotification(string notificationName, object body)
        {
            BroadcastNotification(new Notification(notificationName, body));
        }

        public static void BroadcastNotification(string notificationName, object body, string type)
        {
            BroadcastNotification(new Notification(notificationName, body, type));
        }

        public void Dispose()
        {
            this.m_view = null;
            this.m_model = null;
            this.m_controller = null;
            m_instanceMap.Remove(base.MultitonKey);
        }

        public static IFacade GetInstance() => 
            GetInstance("PureMVC");

        public static IFacade GetInstance(string key)
        {
            if (!m_instanceMap.TryGetValue(key, out IFacade facade))
            {
                facade = new Facade(key);
                m_instanceMap[key] = facade;
            }
            return facade;
        }

        public bool HasCommand(string notificationName) => 
            this.m_controller.HasCommand(notificationName);

        public static bool HasCore(string key) => 
            m_instanceMap.ContainsKey(key);

        public bool HasMediator(string mediatorName) => 
            this.m_view.HasMediator(mediatorName);

        public bool HasProxy(string proxyName) => 
            this.m_model.HasProxy(proxyName);

        protected virtual void InitializeController()
        {
            if (this.m_controller == null)
            {
                this.m_controller = Controller.GetInstance(base.MultitonKey);
            }
        }

        protected virtual void InitializeFacade()
        {
            this.InitializeModel();
            this.InitializeController();
            this.InitializeView();
        }

        protected virtual void InitializeModel()
        {
            if (this.m_model == null)
            {
                this.m_model = Model.GetInstance(base.MultitonKey);
            }
        }

        protected virtual void InitializeView()
        {
            if (this.m_view == null)
            {
                this.m_view = View.GetInstance(base.MultitonKey);
            }
        }

        public void NotifyObservers(INotification notification)
        {
            this.m_view.NotifyObservers(notification);
        }

        public void RegisterCommand(string notificationName, ICommand command)
        {
            this.m_controller.RegisterCommand(notificationName, command);
        }

        public void RegisterCommand(string notificationName, Type commandType)
        {
            this.m_controller.RegisterCommand(notificationName, commandType);
        }

        public void RegisterMediator(IMediator mediator)
        {
            this.m_view.RegisterMediator(mediator);
        }

        public void RegisterProxy(IProxy proxy)
        {
            this.m_model.RegisterProxy(proxy);
        }

        public object RemoveCommand(string notificationName) => 
            this.m_controller.RemoveCommand(notificationName);

        public static void RemoveCore(string key)
        {
            if (m_instanceMap.TryGetValue(key, out IFacade facade))
            {
                m_instanceMap.Remove(key);
                facade.Dispose();
                Model.RemoveModel(key);
                Controller.RemoveController(key);
                View.RemoveView(key);
            }
        }

        public IMediator RemoveMediator(string mediatorName) => 
            this.m_view.RemoveMediator(mediatorName);

        public IProxy RemoveProxy(string proxyName) => 
            this.m_model.RemoveProxy(proxyName);

        public IMediator RetrieveMediator(string mediatorName) => 
            this.m_view.RetrieveMediator(mediatorName);

        public IProxy RetrieveProxy(string proxyName) => 
            this.m_model.RetrieveProxy(proxyName);

        public override void SendNotification(string notificationName)
        {
            this.NotifyObservers(new Notification(notificationName));
        }

        public override void SendNotification(string notificationName, object body)
        {
            this.NotifyObservers(new Notification(notificationName, body));
        }

        public override void SendNotification(string notificationName, object body, string type)
        {
            this.NotifyObservers(new Notification(notificationName, body, type));
        }

        public static IFacade Instance =>
            GetInstance("PureMVC");

        public static IEnumerable<string> ListCore =>
            m_instanceMap.Keys;
    }
}

