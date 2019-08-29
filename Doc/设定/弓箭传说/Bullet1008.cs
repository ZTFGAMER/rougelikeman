using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class Bullet1008 : BulletBase
{
    public override void SetTarget(EntityBase entity, int size = 1)
    {
        base.SetTarget(entity, size);
        base.PosFromStart2Target += Random.Range((float) -3f, (float) 3f);
        if (base.PosFromStart2Target < 8f)
        {
            base.PosFromStart2Target = 8f;
        }
    }
}

