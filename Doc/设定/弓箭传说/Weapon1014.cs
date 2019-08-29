using System;

public class Weapon1014 : WeaponSprintBase
{
    protected override void OnInit()
    {
        base.OnInit();
        if (base.m_Entity.IsElite)
        {
            base.distance = 4f;
        }
        else
        {
            base.distance = 2f;
        }
    }
}

