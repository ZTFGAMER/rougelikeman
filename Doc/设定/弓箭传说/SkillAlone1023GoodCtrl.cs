using Dxx.Util;
using System;

public class SkillAlone1023GoodCtrl : SkillAloneAttrGoodBase
{
    protected override void TriggerEnter(EntityBase entity)
    {
        int num = MathDxx.CeilToInt(base.m_Entity.m_EntityData.attribute.AttackValue.Value * base.args[0]);
        GameLogic.SendHit_Skill(entity, (long) -num);
    }
}

