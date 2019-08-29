using System;

public class EntitySoldier3113 : EntityMonsterStrenghBase
{
    protected override void StartInit()
    {
        base.StartInit();
        this.InitWeapon(base.m_Data.WeaponID);
    }
}

