using System;

public class EquipDrop : GoodsDrop
{
    protected override void OnInit()
    {
        base.Drop_jumpTime = 1f;
    }

    protected override string JumpAnimation =>
        "EquipJump1";
}

