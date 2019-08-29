using System;

public class EntitySoldier3005 : EntityMonsterBase
{
    protected override void StartInit()
    {
        base.StartInit();
        this.InitWeapon(base.m_Data.WeaponID);
    }
}

