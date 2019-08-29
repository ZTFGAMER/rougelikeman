using DG.Tweening;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Weapon1071 : WeaponBase
{
    private Sequence seq;

    protected override void OnAttack(object[] args)
    {
        this.seq = DOTween.Sequence();
        for (int i = 0; i < 3; i++)
        {
            <OnAttack>c__AnonStorey0 storey = new <OnAttack>c__AnonStorey0 {
                $this = this,
                index = i
            };
            TweenSettingsExtensions.AppendCallback(this.seq, new TweenCallback(storey, this.<>m__0));
            TweenSettingsExtensions.AppendInterval(this.seq, 0.33f);
        }
    }

    protected override void OnInstall()
    {
    }

    protected override void OnUnInstall()
    {
        if (this.seq != null)
        {
            TweenExtensions.Kill(this.seq, false);
            this.seq = null;
        }
    }

    [CompilerGenerated]
    private sealed class <OnAttack>c__AnonStorey0
    {
        internal int index;
        internal Weapon1071 $this;

        internal void <>m__0()
        {
            float rota = 0f;
            if (this.index == 0)
            {
                rota = GameLogic.Random((float) -30f, (float) 0f);
            }
            else if (this.index == 1)
            {
                rota = GameLogic.Random((float) 0f, (float) 30f);
            }
            else
            {
                rota = GameLogic.Random((float) -15f, (float) 15f);
            }
            this.$this.CreateBulletOverride(Vector3.zero, rota);
        }
    }
}

