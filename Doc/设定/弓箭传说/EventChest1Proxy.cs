using PureMVC.Patterns;
using System;

public class EventChest1Proxy : Proxy
{
    public const string NAME = "EventChest1Proxy";

    public EventChest1Proxy(object data) : base("EventChest1Proxy", data)
    {
    }
}

