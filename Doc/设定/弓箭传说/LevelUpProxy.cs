using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;

public class LevelUpProxy : Proxy, IProxy, INotifier
{
    public const string NAME = "LevelUpProxy";

    public LevelUpProxy(object data) : base("LevelUpProxy", data)
    {
    }

    public class Transfer
    {
        public int level;
        public Action onclose;
    }
}

