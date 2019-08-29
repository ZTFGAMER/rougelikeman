using Dxx.Util;
using System;
using UnityEngine;

public class Bullet3004 : BulletBase
{
    protected override void OnUpdate()
    {
        base.CurrentDistance += base.Speed * Updater.delta;
        base.mTransform.Translate((Vector3.forward * base.Speed) * Updater.delta);
        if ((base.mTransform.position.y < 0f) || (base.CurrentDistance > 100f))
        {
            this.overDistance();
        }
    }
}

