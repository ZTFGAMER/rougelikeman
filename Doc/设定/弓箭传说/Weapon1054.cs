using DG.Tweening;
using Dxx.Util;
using System;
using UnityEngine;

public class Weapon1054 : WeaponBase
{
    private Sequence seq;

    private void CreateBulletByAngle(float angle)
    {
        float x = MathDxx.Sin(angle);
        float z = MathDxx.Cos(angle);
        base.CreateBulletOverride(new Vector3(x, 0f, z), angle);
    }

    private void KillSequence()
    {
        if (this.seq != null)
        {
            TweenExtensions.Kill(this.seq, false);
            this.seq = null;
        }
    }

    protected override void OnAttack(object[] args)
    {
        this.seq = DOTween.Sequence();
        TweenSettingsExtensions.AppendCallback(this.seq, new TweenCallback(this, this.<OnAttack>m__0));
        TweenSettingsExtensions.AppendInterval(this.seq, 0.05f);
        TweenSettingsExtensions.AppendCallback(this.seq, new TweenCallback(this, this.<OnAttack>m__1));
        TweenSettingsExtensions.AppendInterval(this.seq, 0.03f);
        TweenSettingsExtensions.AppendCallback(this.seq, new TweenCallback(this, this.<OnAttack>m__2));
    }

    protected override void OnInstall()
    {
    }

    protected override void OnUnInstall()
    {
        this.KillSequence();
    }
}

