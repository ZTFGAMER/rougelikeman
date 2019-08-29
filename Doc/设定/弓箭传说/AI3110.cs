using System;

public class AI3110 : AITowerCallBase
{
    protected override void OnInitOnce()
    {
        base.callid = 0xc27;
        base.callcount = 1;
        base.OnInitOnce();
    }
}

