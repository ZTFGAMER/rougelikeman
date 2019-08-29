using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;

public class BuyGoldSureProxy : Proxy, IProxy, INotifier
{
    public const string NAME = "BuyGoldSureProxy";

    public BuyGoldSureProxy(object data) : base("BuyGoldSureProxy", data)
    {
    }

    public class Transfer
    {
        public int index;
        public ShopItemGold item;
        public Action<int, ShopItemGold> callback;
    }
}

