using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;

public class EquipInfoModuleProxy : Proxy, IProxy, INotifier
{
    public const string NAME = "EquipInfoModuleProxy";

    public EquipInfoModuleProxy(object data) : base("EquipInfoModuleProxy", data)
    {
    }

    public enum InfoType
    {
        eNormal,
        eBuy
    }

    public class Transfer
    {
        public LocalSave.EquipOne one;
        public EquipInfoModuleProxy.InfoType type;
        public BlackItemOnectrl buy_itemone;
        public Action<BlackItemOnectrl> buy_callback;
        public Action updatecallback;
        public Action wearcallback;
    }
}

