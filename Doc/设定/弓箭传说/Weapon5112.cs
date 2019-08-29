using DG.Tweening;
using Dxx.Util;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Weapon5112 : WeaponBase
{
    protected override void OnAttack(object[] args)
    {
        <OnAttack>c__AnonStorey1 storey = new <OnAttack>c__AnonStorey1 {
            $this = this
        };
        Sequence sequence = base.mSeqPool.Get();
        bool flag = MathDxx.RandomBool();
        storey.count = 7;
        for (int i = 0; i < storey.count; i++)
        {
            <OnAttack>c__AnonStorey0 storey2 = new <OnAttack>c__AnonStorey0 {
                <>f__ref$1 = storey,
                index = i
            };
            if (flag)
            {
                storey2.index = (storey.count - i) - 1;
            }
            TweenSettingsExtensions.AppendCallback(sequence, new TweenCallback(storey2, this.<>m__0));
            TweenSettingsExtensions.AppendInterval(sequence, 0.07f);
        }
    }

    protected override void OnInstall()
    {
    }

    protected override void OnUnInstall()
    {
    }

    [CompilerGenerated]
    private sealed class <OnAttack>c__AnonStorey0
    {
        internal int index;
        internal Weapon5112.<OnAttack>c__AnonStorey1 <>f__ref$1;

        internal void <>m__0()
        {
            float angle = Utils.GetBulletAngle(this.index, this.<>f__ref$1.count, 150f);
            float x = MathDxx.Sin(angle);
            float z = MathDxx.Cos(angle);
            this.<>f__ref$1.$this.CreateBulletOverride(new Vector3(x, 0f, z), angle);
        }
    }

    [CompilerGenerated]
    private sealed class <OnAttack>c__AnonStorey1
    {
        internal int count;
        internal Weapon5112 $this;
    }
}

