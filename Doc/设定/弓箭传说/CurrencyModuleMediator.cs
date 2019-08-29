using System;
using System.Collections.Generic;

public class CurrencyModuleMediator : MediatorBase
{
    public const string NAME = "CurrencyModuleMediator";

    public CurrencyModuleMediator() : base("CurrencyUIPanel")
    {
    }

    public override List<string> OnListNotificationInterests =>
        new List<string> { 
            "PUB_UI_UPDATE_CURRENCY",
            "UseCurrency",
            "GetCurrency",
            "UseCurrencyKey",
            "CurrencyKeyRotate"
        };
}

