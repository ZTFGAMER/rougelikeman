namespace PureMVC.Patterns
{
    using PureMVC.Interfaces;
    using System;
    using System.Collections.Generic;

    public class Mediator : Notifier, IMediator, INotifier
    {
        public const string NAME = "Mediator";
        protected string m_mediatorName;
        protected object m_viewComponent;

        public Mediator() : this("Mediator", null)
        {
        }

        public Mediator(string mediatorName) : this(mediatorName, null)
        {
        }

        public Mediator(string mediatorName, object viewComponent)
        {
            if (mediatorName == null)
            {
            }
            this.m_mediatorName = "Mediator";
            this.m_viewComponent = viewComponent;
        }

        public virtual void Blur(bool blur)
        {
        }

        public virtual object GetEvent(string eventName) => 
            null;

        public virtual void HandleNotification(INotification notification)
        {
        }

        public virtual void OnRegister()
        {
        }

        public virtual void OnRemove()
        {
        }

        public virtual void PublicNotification(INotification notification)
        {
        }

        public virtual IEnumerable<string> ListNotificationInterests =>
            new List<string>();

        public virtual string MediatorName =>
            this.m_mediatorName;

        public object ViewComponent
        {
            get => 
                this.m_viewComponent;
            set => 
                (this.m_viewComponent = value);
        }
    }
}

