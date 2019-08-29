using System;

public class Weapon4004 : Weapon1024
{
    protected override void OnInstall()
    {
        base.AttackEffect = "WeaponHand4004Effect";
        base.OnInstall();
    }
}

