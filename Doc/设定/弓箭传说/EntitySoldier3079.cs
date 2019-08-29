using System;

public class EntitySoldier3079 : EntityMonsterBase
{
    protected override void OnHitEntity(EntityBase e)
    {
        GameLogic.SendBuff(e, this, 0x7d4, Array.Empty<float>());
    }

    protected override void StartInit()
    {
        base.StartInit();
        base.PlayEffect(0x2f4d76);
        base.m_Body.AddElement(EElementType.eFire);
    }
}

