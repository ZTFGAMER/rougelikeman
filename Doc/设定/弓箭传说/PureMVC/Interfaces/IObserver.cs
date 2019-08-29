namespace PureMVC.Interfaces
{
    using System;

    public interface IObserver
    {
        bool CompareNotifyContext(object obj);
        void NotifyObserver(INotification notification);

        string NotifyMethod { set; }

        object NotifyContext { set; }
    }
}

