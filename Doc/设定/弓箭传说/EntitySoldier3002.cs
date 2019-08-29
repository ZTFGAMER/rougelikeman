using System;

public class EntitySoldier3002 : EntityMonsterBase
{
    protected override void StartInit()
    {
        base.StartInit();
        base.PlayEffect(0x2f4d74);
    }
}

