using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;

public class EquipCombineProxy : Proxy, IProxy, INotifier
{
    public const string NAME = "EquipCombineProxy";

    public EquipCombineProxy(object data) : base("EquipCombineProxy", data)
    {
    }

    public class Transfer
    {
        public Action onClose;
    }
}

