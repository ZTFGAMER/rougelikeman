using DG.Tweening;
using Dxx.Util;
using System;
using UnityEngine;

public class BulletWaitFly2Self : BulletBase
{
    private bool bStart;
    [Header("等待时间")]
    public float waitTime = 0.5f;

    protected override void OnDeInit()
    {
        base.OnDeInit();
    }

    protected override void OnInit()
    {
        base.OnInit();
        this.bStart = false;
        TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(base.mSeqPool.Get(), this.waitTime), new TweenCallback(this, this.<OnInit>m__0));
    }

    protected override void UpdateProcess()
    {
        if (this.bStart)
        {
            base.UpdateProcess();
            if (base.m_Entity == null)
            {
                this.overDistance();
            }
            else
            {
                base.bulletAngle = Utils.getAngle(base.m_Entity.position - base.transform.position);
                base.UpdateMoveDirection();
                if (Vector3.Distance(new Vector3(base.transform.position.x, 0f, base.transform.position.z), new Vector3(base.m_Entity.position.x, 0f, base.m_Entity.position.z)) < 0.5f)
                {
                    this.overDistance();
                }
            }
        }
    }
}

