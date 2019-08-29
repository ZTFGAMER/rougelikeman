namespace PureMVC.Patterns
{
    using PureMVC.Interfaces;
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class Notifier : INotifier
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <MultitonKey>k__BackingField;
        protected const string MULTITON_MSG = "Multiton key for this Notifier not yet initialized!";

        public void InitializeNotifier(string key)
        {
            this.MultitonKey = key;
        }

        public virtual void SendNotification(string notificationName)
        {
            this.Facade.SendNotification(notificationName);
        }

        public virtual void SendNotification(string notificationName, object body)
        {
            this.Facade.SendNotification(notificationName, body);
        }

        public virtual void SendNotification(string notificationName, object body, string type)
        {
            this.Facade.SendNotification(notificationName, body, type);
        }

        public string MultitonKey { get; protected set; }

        protected IFacade Facade
        {
            get
            {
                if (this.MultitonKey == null)
                {
                    throw new Exception("Multiton key for this Notifier not yet initialized!");
                }
                return PureMVC.Patterns.Facade.GetInstance(this.MultitonKey);
            }
        }
    }
}

