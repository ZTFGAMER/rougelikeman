using System;

public class Food2002 : Food2001
{
    protected override void OnAbsorb()
    {
        base.OnAbsorb();
        base.RotateEnable(true);
    }

    protected override void OnDropEnd()
    {
        base.OnDropEnd();
        base.RotateEnable(false);
    }

    protected override void OnEnables()
    {
        base.OnEnables();
        base.RotateEnable(true);
    }
}

