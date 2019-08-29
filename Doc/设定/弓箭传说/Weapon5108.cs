using DG.Tweening;
using Dxx.Util;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Weapon5108 : WeaponBase
{
    private SequencePool mPool = new SequencePool();

    protected override void OnAttack(object[] args)
    {
        <OnAttack>c__AnonStorey1 storey = new <OnAttack>c__AnonStorey1 {
            $this = this
        };
        GameLogic.Hold.Sound.PlayBulletCreate(0x1e9809, base.m_Entity.position);
        storey.count = 3;
        for (int i = 0; i < storey.count; i++)
        {
            <OnAttack>c__AnonStorey0 storey2 = new <OnAttack>c__AnonStorey0 {
                <>f__ref$1 = storey,
                index = i
            };
            Sequence sequence = this.mPool.Get();
            storey2.offset = GameLogic.Random((float) -20f, (float) 20f);
            for (int j = 0; j < 4; j++)
            {
                TweenSettingsExtensions.AppendCallback(sequence, new TweenCallback(storey2, this.<>m__0));
                TweenSettingsExtensions.AppendInterval(sequence, 0.2f);
            }
        }
    }

    protected override void OnInstall()
    {
        this.mPool.Clear();
    }

    protected override void OnUnInstall()
    {
        this.mPool.Clear();
    }

    [CompilerGenerated]
    private sealed class <OnAttack>c__AnonStorey0
    {
        internal int index;
        internal float offset;
        internal Weapon5108.<OnAttack>c__AnonStorey1 <>f__ref$1;

        internal void <>m__0()
        {
            float angle = Utils.GetBulletAngle(this.index, this.<>f__ref$1.count, 90f) + this.offset;
            Vector3 offsetpos = new Vector3(MathDxx.Sin(angle), 0f, MathDxx.Cos(angle)) * 1f;
            BulletBase base2 = this.<>f__ref$1.$this.CreateBulletOverride(offsetpos, angle);
            base2.mBulletTransmit.attribute.ReboundWall = new EntityAttributeBase.ValueRange(1, 2, 2);
            base2.UpdateBulletAttribute();
        }
    }

    [CompilerGenerated]
    private sealed class <OnAttack>c__AnonStorey1
    {
        internal int count;
        internal Weapon5108 $this;
    }
}

