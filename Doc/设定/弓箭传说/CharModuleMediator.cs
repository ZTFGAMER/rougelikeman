using System;
using System.Collections.Generic;

public class CharModuleMediator : MediatorBase
{
    public const string NAME = "CharModuleMediator";

    public CharModuleMediator() : base("CharacterUIPanel2")
    {
    }

    public override List<string> OnListNotificationInterests =>
        new List<string>();
}

