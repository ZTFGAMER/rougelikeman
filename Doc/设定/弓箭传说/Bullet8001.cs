using Dxx.Util;
using System;
using UnityEngine;

public class Bullet8001 : BulletBase
{
    private TrailRenderer trail1;
    private float trail1time;
    private int state;

    protected override void AwakeInit()
    {
    }

    protected override void BoxEnable(bool enable)
    {
        base.BoxEnable(enable);
    }

    protected override void OnInit()
    {
        base.OnInit();
        base.HitWallAction = new Action<Collider>(this.OnThroughWalls);
        this.state = 0;
    }

    protected override void OnOverDistance()
    {
    }

    protected virtual void OnRotate()
    {
        if (base.childMesh != null)
        {
            base.childMesh.rotation = Quaternion.Euler(base.childMesh.eulerAngles.x, base.childMesh.eulerAngles.y + base.m_Data.RotateSpeed, base.childMesh.eulerAngles.z);
        }
    }

    protected override void OnThroughTrailShow(bool show)
    {
    }

    private void OnThroughWalls(Collider o)
    {
        if (this.state == 0)
        {
            base.mHitList.Clear();
            this.state = 1;
            base.m_Entity.PlayEffect(0xf6181, base.mTransform.position);
            float num = base.m_Entity.m_EntityData.attribute.WeaponRoundBackAttackPercent.Value;
            if (num > 0f)
            {
                base.mBulletTransmit.AddAttackRatio(1f + num);
            }
        }
    }

    protected override void OnUpdate()
    {
        if ((this.state != 0) && (this.state == 1))
        {
            float x = base.m_Entity.position.x - base.mTransform.position.x;
            float y = base.m_Entity.position.z - base.mTransform.position.z;
            base.bulletAngle = Utils.getAngle(x, y);
        }
        float frameDistance = base.FrameDistance;
        base.UpdateMoveDirection();
        base.mTransform.position += new Vector3(base.moveX, 0f, base.moveY) * frameDistance;
        this.OnRotate();
        if ((this.state == 1) && (Vector3.Distance(base.mTransform.position, base.m_Entity.position) < 2f))
        {
            this.overDistance();
        }
    }
}

