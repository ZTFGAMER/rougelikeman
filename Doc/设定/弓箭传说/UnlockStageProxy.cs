using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;

public class UnlockStageProxy : Proxy, IProxy, INotifier
{
    public const string NAME = "UnlockStageProxy";

    public UnlockStageProxy(object data) : base("UnlockStageProxy", data)
    {
    }

    public class Transfer
    {
        public int StageID;
    }
}

