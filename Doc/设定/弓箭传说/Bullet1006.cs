using System;
using UnityEngine;

public class Bullet1006 : BulletBase
{
    protected override void AwakeInit()
    {
    }

    protected override void OnInit()
    {
        base.OnInit();
    }

    protected override void OnUpdate()
    {
        float frameDistance = base.FrameDistance;
        base.mTransform.position += new Vector3(base.moveX, 0f, base.moveY * 1.23f) * frameDistance;
        base.childMesh.rotation = Quaternion.Euler(55f, 0f, 0f);
        base.bulletAngle += this.RotateAngle;
        base.UpdateMoveDirection();
        base.CurrentDistance += frameDistance;
        if (base.CurrentDistance >= base.Distance)
        {
            this.overDistance();
        }
    }

    protected virtual float RotateAngle =>
        -1.1f;
}

