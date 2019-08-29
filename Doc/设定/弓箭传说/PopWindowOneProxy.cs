using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;

public class PopWindowOneProxy : Proxy, IProxy, INotifier
{
    public const string NAME = "PopWindowOneProxy";

    public PopWindowOneProxy(object data) : base("PopWindowOneProxy", data)
    {
    }

    public class Transfer
    {
        public string title;
        public string content;
        public string sure;
        public bool showclosebutton;
        public Action callback;
    }
}

