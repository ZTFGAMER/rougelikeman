using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;

public class EquipBuyInfoProxy : Proxy, IProxy, INotifier
{
    public const string NAME = "EquipBuyInfoProxy";

    public EquipBuyInfoProxy(object data) : base("EquipBuyInfoProxy", data)
    {
    }

    public class Transfer
    {
        public BlackItemOnectrl itemone;
        public Action<BlackItemOnectrl> callback;
    }
}

