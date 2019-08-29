using PureMVC.Patterns;
using System;

public class BattleProxy : Proxy
{
    public const string NAME = "BattleProxy";

    public BattleProxy(object data) : base("BattleProxy", data)
    {
    }
}

