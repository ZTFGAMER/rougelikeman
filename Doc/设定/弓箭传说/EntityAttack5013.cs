using System;

public class EntityAttack5013 : EntityAttack1007
{
    protected override int ReboundCount =>
        1;

    protected override float linetimemin =>
        0.2f;

    protected override float linetimemax =>
        0.5f;

    protected override int count =>
        3;

    protected override float perangle =>
        45f;
}

