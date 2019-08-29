namespace PureMVC.Interfaces
{
    using System;

    public interface ICommand : INotifier
    {
        void Execute(INotification notification);
    }
}

