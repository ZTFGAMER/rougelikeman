using PureMVC.Patterns;
using System;

public class NetDoingProxy : Proxy
{
    public const string NAME = "NetDoingProxy";

    public NetDoingProxy(object data) : base("NetDoingProxy", data)
    {
    }

    public class Transfer
    {
        public NetDoingType type;
    }
}

