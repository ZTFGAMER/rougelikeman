using System;
using System.Collections.Generic;

public class ShopSingleMediator : MediatorBase
{
    public const string NAME = "ShopSingleMediator";

    public ShopSingleMediator() : base("ShopSingleUIPanel")
    {
    }

    public override List<string> OnListNotificationInterests
    {
        get
        {
            List<string> onListNotificationInterests = base.OnListNotificationInterests;
            onListNotificationInterests.Add("ShopUI_Update");
            return onListNotificationInterests;
        }
    }
}

