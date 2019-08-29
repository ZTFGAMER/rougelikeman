using DG.Tweening;
using System;
using UnityEngine;

public class BulletWaitOverBase : BulletBase
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
        base.SetBoxEnableOnce(0.3f);
        TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(sequence, this.waitTime), new TweenCallback(this, this.<OnInit>m__0));
    }

    protected override void UpdateProcess()
    {
        base.UpdateProcess();
    }
}

