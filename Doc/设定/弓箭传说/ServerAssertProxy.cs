using PureMVC.Patterns;
using System;

public class ServerAssertProxy : Proxy
{
    public const string NAME = "ServerAssertProxy";

    public ServerAssertProxy(object data) : base("ServerAssertProxy", data)
    {
    }

    public class Transfer
    {
        public long assertendtime;
    }
}

