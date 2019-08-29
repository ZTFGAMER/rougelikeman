using System;

public class Weapon5120 : Weapon5005
{
    protected override void OnInstall()
    {
        base.OnInstall();
        base.posoffset = 0.5f;
        base.bulletcount = 15;
    }
}

