using DG.Tweening;
using System;
using UnityEngine;

public class BulletUpWaitBase : BulletBase
{
    private bool bStart;
    [Header("升起时间")]
    public float upTime = 1f;
    [Header("升起高度")]
    public float upHeight = 1f;
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
        TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(TweenSettingsExtensions.Append(base.mSeqPool.Get(), ShortcutExtensions.DOMoveY(base.mTransform, this.upHeight, this.upTime, false)), this.waitTime), new TweenCallback(this, this.<OnInit>m__0));
    }

    protected override void UpdateProcess()
    {
        if (this.bStart)
        {
            base.UpdateProcess();
        }
    }
}

