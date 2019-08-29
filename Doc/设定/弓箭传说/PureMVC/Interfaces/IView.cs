namespace PureMVC.Interfaces
{
    using System;
    using System.Collections.Generic;

    public interface IView : IDisposable
    {
        bool HasMediator(string mediatorName);
        void NotifyObservers(INotification notification);
        void RegisterMediator(IMediator mediator);
        void RegisterObserver(string notificationName, IObserver observer);
        IMediator RemoveMediator(string mediatorName);
        void RemoveObserver(string notificationName, object notifyContext);
        IMediator RetrieveMediator(string mediatorName);

        IEnumerable<string> ListMediatorNames { get; }
    }
}

