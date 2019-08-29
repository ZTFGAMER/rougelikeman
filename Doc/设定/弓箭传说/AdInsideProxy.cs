using PureMVC.Patterns;
using System;

public class AdInsideProxy : Proxy
{
    public const string NAME = "AdInsideProxy";

    public AdInsideProxy(object data) : base("AdInsideProxy", data)
    {
    }

    public enum EnterSource
    {
        eKey,
        eGameTurn
    }

    public class Transfer
    {
        public AdInsideProxy.EnterSource source;
        public Action finish_callback;
    }
}

