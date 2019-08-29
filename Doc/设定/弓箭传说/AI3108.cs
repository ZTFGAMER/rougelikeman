using System;

public class AI3108 : AITowerCallBase
{
    protected override void OnInitOnce()
    {
        base.callid = 0xc25;
        base.callcount = 1;
        base.OnInitOnce();
    }
}

