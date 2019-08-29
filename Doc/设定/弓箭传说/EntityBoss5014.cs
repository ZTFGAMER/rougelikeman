using System;

public class EntityBoss5014 : EntityBossBase
{
    private float up;

    protected override long GetBossHP()
    {
        long maxHP = base.m_EntityData.MaxHP;
        long num2 = GameLogic.GetMaxHP(0xbf2);
        long num3 = GameLogic.GetMaxHP(0xbf3);
        long num4 = GameLogic.GetMaxHP(0xbf4);
        return (((maxHP + (num2 * 2L)) + ((num3 * 2L) * 2L)) + (((num4 * 2L) * 2L) * 2L));
    }

    private void InitAfter()
    {
        int num = 0x2d + (90 * GameLogic.Random(0, 4));
        base.m_AttackCtrl.SetRotate((float) num);
    }

    protected override void OnInit()
    {
        base.OnInit();
        this.InitAfter();
    }
}

