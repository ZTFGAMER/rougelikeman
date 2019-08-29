using System;

public class Weapon1015 : WeaponSprintBase
{
    protected override void OnInit()
    {
        if (base.m_Entity.IsElite)
        {
            base.distance = 4f;
        }
        else
        {
            base.distance = 2f;
        }
        base.delaytime = 0.4f;
        base.OnInit();
    }
}

