using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;
using System.Collections.Generic;

public class RewardSimpleProxy : Proxy, IProxy, INotifier
{
    public const string NAME = "RewardSimpleProxy";

    public RewardSimpleProxy(object data) : base("RewardSimpleProxy", data)
    {
    }

    public class Transfer
    {
        public List<Drop_DropModel.DropData> list;
    }
}

