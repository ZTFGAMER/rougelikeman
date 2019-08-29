using PureMVC.Patterns;
using System;
using System.Collections.Generic;

public class RewardShowProxy : Proxy
{
    public const string NAME = "RewardShowProxy";

    public RewardShowProxy(object data) : base("RewardShowProxy", data)
    {
    }

    public class Transfer
    {
        public List<Drop_DropModel.DropData> list;
    }
}

