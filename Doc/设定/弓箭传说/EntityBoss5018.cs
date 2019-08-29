using System;

public class EntityBoss5018 : EntityBossBase
{
    private float up;

    protected override long GetBossHP()
    {
        long maxHP = base.m_EntityData.MaxHP;
        long num2 = GameLogic.GetMaxHP(0xbf5);
        long num3 = GameLogic.GetMaxHP(0xbf6);
        return ((maxHP + (num2 * 2L)) + ((num3 * 2L) * 3L));
    }

    private void InitAfter()
    {
    }

    protected override void OnInit()
    {
        base.OnInit();
        this.InitAfter();
    }
}

