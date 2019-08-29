using System;

public class EntityPartBody1804 : EntityPartBodyBase
{
    protected override void StartInit()
    {
        base.StartInit();
        this.InitWeapon(base.m_Data.WeaponID);
    }
}

