using DG.Tweening;
using System;
using UnityEngine;

public class BulletWaitBase : BulletBase
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
        }
    }
}

