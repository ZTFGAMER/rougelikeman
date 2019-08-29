using DG.Tweening;
using System;
using UnityEngine;

public class BulletWaitMove4 : BulletBase
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
        Sequence sequence = base.mSeqPool.Get();
        this.BoxEnable(false);
        TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(sequence, this.waitTime), new TweenCallback(this, this.<OnInit>m__0));
    }

    protected override void OnUpdate()
    {
        if (this.bStart)
        {
            base.OnUpdate();
        }
    }
}

