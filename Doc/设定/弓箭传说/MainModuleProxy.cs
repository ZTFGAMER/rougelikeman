using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;

public class MainModuleProxy : Proxy, IProxy, INotifier
{
    public const string NAME = "MainModule";

    public MainModuleProxy() : base("MainModule")
    {
    }
}

