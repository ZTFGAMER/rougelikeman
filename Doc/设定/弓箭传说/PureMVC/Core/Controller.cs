namespace PureMVC.Core
{
    using PureMVC.Interfaces;
    using PureMVC.Patterns;
    using System;
    using System.Collections.Generic;

    public class Controller : IController, IDisposable
    {
        protected string m_multitonKey;
        private IView m_view;
        private readonly IDictionary<string, object> m_commandMap;
        protected static readonly IDictionary<string, IController> m_instanceMap = new Dictionary<string, IController>();
        public const string DEFAULT_KEY = "PureMVC";
        protected const string MULTITON_MSG = "Controller instance for this Multiton key already constructed!";

        public Controller() : this("PureMVC")
        {
        }

        public Controller(string key)
        {
            this.m_multitonKey = key;
            this.m_commandMap = new Dictionary<string, object>();
            if (m_instanceMap.ContainsKey(key))
            {
                throw new Exception("Controller instance for this Multiton key already constructed!");
            }
            m_instanceMap[key] = this;
            this.InitializeController();
        }

        public void Dispose()
        {
            RemoveController(this.m_multitonKey);
            this.m_commandMap.Clear();
        }

        public void ExecuteCommand(INotification notification)
        {
            if (this.m_commandMap.ContainsKey(notification.Name))
            {
                ICommand command;
                object obj2 = this.m_commandMap[notification.Name];
                Type type = obj2 as Type;
                if (type != null)
                {
                    command = Activator.CreateInstance(type) as ICommand;
                    if (command == null)
                    {
                        return;
                    }
                }
                else
                {
                    command = obj2 as ICommand;
                    if (command == null)
                    {
                        return;
                    }
                }
                command.InitializeNotifier(this.m_multitonKey);
                command.Execute(notification);
            }
        }

        public static IController GetInstance() => 
            GetInstance("PureMVC");

        public static IController GetInstance(string key)
        {
            if (!m_instanceMap.TryGetValue(key, out IController controller))
            {
                controller = new Controller(key);
                m_instanceMap[key] = controller;
            }
            return controller;
        }

        public bool HasCommand(string notificationName) => 
            this.m_commandMap.ContainsKey(notificationName);

        private void InitializeController()
        {
            this.m_view = View.GetInstance(this.m_multitonKey);
        }

        public void RegisterCommand(string notificationName, ICommand command)
        {
            if (!this.m_commandMap.ContainsKey(notificationName))
            {
                this.m_view.RegisterObserver(notificationName, new Observer("executeCommand", this));
            }
            command.InitializeNotifier(this.m_multitonKey);
            this.m_commandMap[notificationName] = command;
        }

        public void RegisterCommand(string notificationName, Type commandType)
        {
            if (!this.m_commandMap.ContainsKey(notificationName))
            {
                this.m_view.RegisterObserver(notificationName, new Observer("executeCommand", this));
            }
            this.m_commandMap[notificationName] = commandType;
        }

        public object RemoveCommand(string notificationName)
        {
            if (!this.m_commandMap.ContainsKey(notificationName))
            {
                return null;
            }
            this.m_view.RemoveObserver(notificationName, this);
            object obj2 = this.m_commandMap[notificationName];
            this.m_commandMap.Remove(notificationName);
            return obj2;
        }

        public static void RemoveController(string key)
        {
            if (m_instanceMap.TryGetValue(key, out IController controller))
            {
                m_instanceMap.Remove(key);
                controller.Dispose();
            }
        }

        public static IController Instance =>
            GetInstance("PureMVC");

        public IEnumerable<string> ListNotificationNames =>
            this.m_commandMap.Keys;
    }
}

