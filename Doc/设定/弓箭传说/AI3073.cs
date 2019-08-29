using System;

public class AI3073 : AI3019
{
    public const int DivideCount = 2;

    protected override void OnDeadBefore()
    {
        base.Divide(0xc02, 2);
    }
}

