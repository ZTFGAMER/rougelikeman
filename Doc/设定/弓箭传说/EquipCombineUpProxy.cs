using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;
using System.Collections.Generic;

public class EquipCombineUpProxy : Proxy, IProxy, INotifier
{
    public const string NAME = "EquipCombineUpProxy";

    public EquipCombineUpProxy(object data) : base("EquipCombineUpProxy", data)
    {
    }

    public class Transfer
    {
        public LocalSave.EquipOne equip;
        public Action onClose;
        public List<string> mats = new List<string>();

        public void AddMatUniqueID(string uniqueid)
        {
            this.mats.Add(uniqueid);
        }
    }
}

