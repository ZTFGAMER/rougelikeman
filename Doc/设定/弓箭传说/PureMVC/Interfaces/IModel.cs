namespace PureMVC.Interfaces
{
    using System;
    using System.Collections.Generic;

    public interface IModel : IDisposable
    {
        bool HasProxy(string proxyName);
        void RegisterProxy(IProxy proxy);
        IProxy RemoveProxy(string proxyName);
        IProxy RetrieveProxy(string proxyName);

        IEnumerable<string> ListProxyNames { get; }
    }
}

