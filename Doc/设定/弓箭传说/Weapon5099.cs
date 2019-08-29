using System;

public class Weapon5099 : WeaponSprintBase
{
    protected override void OnInit()
    {
        base.distance = 10f;
        base.delaytime = 0.7f;
        base.updatetime = 0.15f;
        base.OnInit();
    }
}

