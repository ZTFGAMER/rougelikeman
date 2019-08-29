using System;

public class AI3057 : AITowerCallBase
{
    protected override void OnInitOnce()
    {
        base.callid = 0xc23;
        base.callcount = 1;
        base.OnInitOnce();
    }
}

