using Dxx.Util;
using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class Bullet5060 : BulletBase
{
    private bool bShow;
    private GameObject circle;
    private const float BombTime = 0.3f;
    private const float MaxColliderSize = 11f;
    private float mBombTime;
    private bool bColliderUpdate;

    protected override void AwakeInit()
    {
        base.Parabola_MaxHeight = 4f;
    }

    protected override void OnDeInit()
    {
        base.OnDeInit();
    }

    protected override void OnInit()
    {
    }

    protected override void overDistance()
    {
        if (!this.bColliderUpdate)
        {
            base.bMoveEnable = false;
            this.bColliderUpdate = true;
            base.m_Entity.PlayEffect(0xf55d7, base.mTransform.position);
            base.mTransform.rotation = Quaternion.identity;
            base.childMesh.gameObject.SetActive(false);
            if (base.shadowGameObject != null)
            {
                base.shadowGameObject.SetActive(false);
            }
            this.BoxEnable(true);
        }
    }

    public override void SetTarget(EntityBase entity, int size = 1)
    {
        base.SetTarget(entity, size);
        if (base.PosFromStart2Target < 3f)
        {
            base.PosFromStart2Target = 3f;
        }
        this.mBombTime = 0f;
        this.bColliderUpdate = false;
        this.BoxEnable(false);
        base.childMesh.gameObject.SetActive(true);
    }

    protected override void UpdateProcess()
    {
        base.UpdateProcess();
        if (this.bColliderUpdate)
        {
            this.mBombTime += Updater.delta;
            if (base.boxListCount == 2)
            {
                base.boxList[0].size = new Vector3((this.mBombTime / 0.3f) * 11f, 1f, 1f);
                base.boxList[1].size = new Vector3(1f, 1f, (this.mBombTime / 0.3f) * 11f);
            }
            if (this.mBombTime >= 0.3f)
            {
                base.overDistance();
            }
        }
    }
}

