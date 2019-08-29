using System;
using UnityEngine;

public class BulletSlopeBase : BulletBase
{
    private Vector3 endpos;
    private Vector3 dir;

    protected override void OnInit()
    {
        base.OnInit();
    }

    protected override void OnUpdate()
    {
        Vector3 vector = base.mTransform.position - this.endpos;
        this.dir = vector.normalized;
        base.mTransform.position = Vector3.MoveTowards(base.mTransform.position, this.endpos, base.FrameDistance);
        Vector3 vector2 = base.mTransform.position - this.endpos;
        if (vector2.magnitude < 0.1f)
        {
            this.overDistance();
        }
    }

    public void SetEndPos(Vector3 endpos)
    {
        this.endpos = endpos;
    }
}

