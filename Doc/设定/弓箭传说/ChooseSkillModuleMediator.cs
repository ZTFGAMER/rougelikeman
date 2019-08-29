using System;
using System.Collections.Generic;

public class ChooseSkillModuleMediator : MediatorBase
{
    public const string NAME = "ChooseSkillModuleMediator";

    public ChooseSkillModuleMediator() : base("ChooseSkillUIPanel")
    {
    }

    public override List<string> OnListNotificationInterests =>
        new List<string> { 
            "BATTLE_CHOOSESKILL_ACTION_END",
            "BATTLE_CHOOSESKILL_SKILL_CHOOSE"
        };
}

