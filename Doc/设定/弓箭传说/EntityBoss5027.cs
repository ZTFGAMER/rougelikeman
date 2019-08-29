using System;

public class EntityBoss5027 : EntityBossBase
{
    private float up;

    protected override long GetBossHP()
    {
        long maxHP = base.m_EntityData.MaxHP;
        long num2 = GameLogic.GetMaxHP(0xc01);
        long num3 = GameLogic.GetMaxHP(0xc02);
        return ((maxHP + (num2 * 6L)) + ((num3 * 6L) * 2L));
    }

    protected override void OnInit()
    {
        base.OnInit();
        base.InitDivideID();
        base.PlayEffect(0x2f4d75);
    }
}

