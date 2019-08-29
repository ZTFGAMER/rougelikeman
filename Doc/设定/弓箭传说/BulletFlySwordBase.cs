using DG.Tweening;
using System;
using UnityEngine;

public class BulletFlySwordBase : BulletBase
{
    private bool bStart;
    [Header("缩放时间")]
    public float scaleTime = 0.5f;
    [Header("等待时间")]
    public float waitTime = 0.6f;

    protected override void OnDeInit()
    {
        base.OnDeInit();
    }

    protected override void OnInit()
    {
        base.OnInit();
        this.bStart = false;
        base.mTransform.localScale = new Vector3(0f, 1f, 1f);
        TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(TweenSettingsExtensions.Append(base.mSeqPool.Get(), ShortcutExtensions.DOScale(base.mTransform, 1f, this.scaleTime)), this.waitTime), new TweenCallback(this, this.<OnInit>m__0));
    }

    protected override void OnSetBulletAttribute()
    {
        base.OnSetBulletAttribute();
        base.TrailShow(false);
    }

    protected override void UpdateProcess()
    {
        if (this.bStart)
        {
            base.UpdateProcess();
        }
    }
}

