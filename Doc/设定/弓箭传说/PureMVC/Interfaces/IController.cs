namespace PureMVC.Interfaces
{
    using System;
    using System.Collections.Generic;

    public interface IController : IDisposable
    {
        void ExecuteCommand(INotification notification);
        bool HasCommand(string notificationName);
        void RegisterCommand(string notificationName, ICommand command);
        void RegisterCommand(string notificationName, Type commandType);
        object RemoveCommand(string notificationName);

        IEnumerable<string> ListNotificationNames { get; }
    }
}

