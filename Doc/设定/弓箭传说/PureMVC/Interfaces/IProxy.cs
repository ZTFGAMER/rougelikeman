namespace PureMVC.Interfaces
{
    using System;

    public interface IProxy : INotifier
    {
        void OnRegister();
        void OnRemove();

        string ProxyName { get; }

        object Data { get; set; }

        Action Event_Para0 { get; set; }

        Action<object> Event_Para1 { get; set; }
    }
}

