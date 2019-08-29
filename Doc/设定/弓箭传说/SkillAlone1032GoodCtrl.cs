using System;

public class SkillAlone1032GoodCtrl : SkillAloneAttrGoodBase
{
    protected override void TriggerEnter(EntityBase entity)
    {
        GameLogic.SendBuff(entity, base.m_Entity, 0x3f9, Array.Empty<float>());
    }
}

