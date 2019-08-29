using System;

public class AI2008 : AIBabyBase
{
    protected override void OnInit()
    {
        base.m_Entity.m_EntityData.Modify_ThroughEnemy(1, 1f);
        base.m_Entity.ChangeWeapon(base.m_Entity.m_Data.WeaponID);
        base.OnInit();
    }
}

