using System;

public class EntitySoldier3019 : EntityMonsterBase
{
    protected override void StartInit()
    {
        base.StartInit();
        base.InitDivideID();
        base.PlayEffect(0x2f4d75);
    }
}

