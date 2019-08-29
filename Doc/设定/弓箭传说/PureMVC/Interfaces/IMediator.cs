namespace PureMVC.Interfaces
{
    using System;
    using System.Collections.Generic;

    public interface IMediator : INotifier
    {
        object GetEvent(string eventName);
        void HandleNotification(INotification notification);
        void OnRegister();
        void OnRemove();

        string MediatorName { get; }

        object ViewComponent { get; set; }

        IEnumerable<string> ListNotificationInterests { get; }
    }
}

