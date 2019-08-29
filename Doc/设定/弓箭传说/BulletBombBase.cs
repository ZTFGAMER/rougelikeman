using Dxx.Util;
using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class BulletBombBase : BulletBase
{
    [Header("延迟时间")]
    public float DelayTime = 1f;
    [Header("爆炸冲击时间")]
    public float BombTime = 0.5f;
    private float showCircleTime = 0.5f;
    private const float MaxColliderSize = 11f;
    private float mDelaytime;
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

    public override void SetTarget(EntityBase entity, int size = 1)
    {
        base.SetTarget(entity, size);
        if (base.PosFromStart2Target < 3f)
        {
            base.PosFromStart2Target = 3f;
        }
        this.mDelaytime = this.DelayTime;
        this.mBombTime = 0f;
        this.bColliderUpdate = false;
        this.BoxEnable(false);
        base.childMesh.gameObject.SetActive(true);
    }

    protected override void UpdateProcess()
    {
        base.UpdateProcess();
        this.mDelaytime -= Updater.delta;
        if ((this.mDelaytime <= 0f) && !this.bColliderUpdate)
        {
            this.bColliderUpdate = true;
            this.mDelaytime = 0f;
            GameLogic.PlayEffect(0xf55d7, base.mTransform.position);
            base.childMesh.gameObject.SetActive(false);
            if (base.shadowGameObject != null)
            {
                base.shadowGameObject.SetActive(false);
            }
            this.BoxEnable(true);
        }
        if (this.bColliderUpdate)
        {
            this.mBombTime += Updater.delta;
            if (base.boxListCount == 2)
            {
                base.boxList[0].size = new Vector3((this.mBombTime / this.BombTime) * 11f, 1f, 1f);
                base.boxList[1].size = new Vector3(1f, 1f, (this.mBombTime / this.BombTime) * 11f);
            }
            if (this.mBombTime >= this.BombTime)
            {
                this.overDistance();
            }
        }
    }
}

