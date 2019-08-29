using System;

public class AI3020 : AI3011
{
    protected override void OnDeadBefore()
    {
        base.Divide(0xbc3, 2);
    }
}

