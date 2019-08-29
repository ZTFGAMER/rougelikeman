namespace PureMVC.Interfaces
{
    using System;

    public interface IFacade : INotifier, IDisposable
    {
        bool HasCommand(string notificationName);
        bool HasMediator(string mediatorName);
        bool HasProxy(string proxyName);
        void NotifyObservers(INotification notification);
        void RegisterCommand(string notificationName, ICommand command);
        void RegisterCommand(string notificationName, Type commandType);
        void RegisterMediator(IMediator mediator);
        void RegisterProxy(IProxy proxy);
        object RemoveCommand(string notificationName);
        IMediator RemoveMediator(string mediatorName);
        IProxy RemoveProxy(string proxyName);
        IMediator RetrieveMediator(string mediatorName);
        IProxy RetrieveProxy(string proxyName);
    }
}

