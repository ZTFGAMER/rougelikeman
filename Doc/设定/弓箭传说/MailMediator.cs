using System;
using System.Collections.Generic;

public class MailMediator : MediatorBase
{
    public const string NAME = "MailMediator";

    public MailMediator() : base("MailUIPanel")
    {
    }

    public override List<string> OnListNotificationInterests
    {
        get
        {
            List<string> onListNotificationInterests = base.OnListNotificationInterests;
            onListNotificationInterests.Add("MailUI_MailUpdate");
            return onListNotificationInterests;
        }
    }
}

