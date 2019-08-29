using System;

public class Weapon5033 : WeaponSprintBase
{
    private float movedis;

    protected override void OnInit()
    {
        base.distance = 8f;
        base.delaytime = 0.2f;
        this.movedis = 0f;
        base.OnInit();
    }

    protected override void OnUpdateMove(float currentdis)
    {
        this.movedis += currentdis;
    }
}

