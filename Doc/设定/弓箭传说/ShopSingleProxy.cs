using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;

public class ShopSingleProxy : Proxy, IProxy, INotifier
{
    public const string NAME = "ShopSingleProxy";

    public ShopSingleProxy(object data) : base("ShopSingleProxy", data)
    {
    }

    public enum SingleType
    {
        eDiamond
    }

    public class Transfer
    {
        public ShopSingleProxy.SingleType type;
    }
}

