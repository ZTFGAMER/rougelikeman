using PureMVC.Patterns;
using System;

public class BoxOpenOneProxy : Proxy
{
    public const string NAME = "BoxOpenOneProxy";

    public BoxOpenOneProxy(object data) : base("BoxOpenOneProxy", data)
    {
    }
}

