using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;

public class CardLevelUpProxy : Proxy, IProxy, INotifier
{
    public const string NAME = "CardLevelUpProxy";

    public CardLevelUpProxy(object data) : base("CardLevelUpProxy", data)
    {
    }
}

