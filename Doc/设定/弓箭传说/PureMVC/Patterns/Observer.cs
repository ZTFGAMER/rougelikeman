namespace PureMVC.Patterns
{
    using PureMVC.Interfaces;
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class Observer : IObserver
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <NotifyMethod>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private object <NotifyContext>k__BackingField;

        public Observer(string notifyMethod, object notifyContext)
        {
            this.NotifyMethod = notifyMethod;
            this.NotifyContext = notifyContext;
        }

        public bool CompareNotifyContext(object obj)
        {
            object obj2 = this;
            lock (obj2)
            {
                return this.NotifyContext.Equals(obj);
            }
        }

        public void NotifyObserver(INotification notification)
        {
            object notifyContext;
            object obj3 = this;
            lock (obj3)
            {
                notifyContext = this.NotifyContext;
            }
            if (notifyContext is IMediator)
            {
                (notifyContext as IMediator).HandleNotification(notification);
            }
            else if (notifyContext is IController)
            {
                (notifyContext as IController).ExecuteCommand(notification);
            }
        }

        public string NotifyMethod { private get; set; }

        public object NotifyContext { private get; set; }
    }
}

