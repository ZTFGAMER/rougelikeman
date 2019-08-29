using System;

public class EntitySoldier3089 : EntityMonsterBase
{
    private void InitAfter()
    {
        int num = 0x2d + (90 * GameLogic.Random(0, 4));
        base.m_AttackCtrl.SetRotate((float) num);
    }
}

