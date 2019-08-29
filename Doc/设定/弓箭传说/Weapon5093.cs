using DG.Tweening;
using System;

public class Weapon5093 : WeaponBase
{
    private Sequence seq;

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
        for (int i = 0; i < 2; i++)
        {
            TweenSettingsExtensions.AppendCallback(this.seq, new TweenCallback(this, this.<OnAttack>m__0));
            TweenSettingsExtensions.AppendInterval(this.seq, 0.4f);
        }
    }

    protected override void OnInstall()
    {
        this.KillSequence();
    }

    protected override void OnUnInstall()
    {
        this.KillSequence();
    }
}

