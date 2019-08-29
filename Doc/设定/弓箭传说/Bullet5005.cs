using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class Bullet5005 : BulletBase
{
    public override void SetTarget(EntityBase entity, int size = 1)
    {
        base.SetTarget(entity, size);
        base.PosFromStart2Target += Random.Range((float) -5f, (float) 5f);
    }
}

