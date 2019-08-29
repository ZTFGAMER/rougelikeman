using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;

public class PopWindowProxy : Proxy, IProxy, INotifier
{
    public const string NAME = "PopWindowProxy";

    public PopWindowProxy(object data) : base("PopWindowProxy", data)
    {
    }

    public class Transfer
    {
        public string title;
        public string content;
        public Action callback;
    }
}

