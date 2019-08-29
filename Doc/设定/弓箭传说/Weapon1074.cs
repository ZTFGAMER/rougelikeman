using System;

public class Weapon1074 : Weapon1024
{
    protected override void OnInstall()
    {
        base.AttackEffect = "WeaponHand1074Effect";
        base.OnInstall();
    }

    protected override void OnUnInstall()
    {
        base.OnUnInstall();
    }
}

