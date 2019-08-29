using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Weapon5040 : WeaponBase
{
    private float allangle = 90f;
    private int count = 3;
    private float perangle;

    protected override void OnAttack(object[] args)
    {
        for (int i = 0; i < 4; i++)
        {
            <OnAttack>c__AnonStorey0 storey = new <OnAttack>c__AnonStorey0 {
                $this = this,
                index = i
            };
            base.action.AddActionDelegate(new Action(storey.<>m__0));
            base.action.AddActionWait(0.1f);
        }
        for (int j = 0; j < 4; j++)
        {
            <OnAttack>c__AnonStorey1 storey2 = new <OnAttack>c__AnonStorey1 {
                $this = this,
                index = j
            };
            base.action.AddActionDelegate(new Action(storey2.<>m__0));
            base.action.AddActionWait(0.1f);
        }
    }

    protected override void OnInstall()
    {
        this.perangle = this.allangle / ((float) (this.count - 1));
    }

    protected override void OnUnInstall()
    {
    }

    [CompilerGenerated]
    private sealed class <OnAttack>c__AnonStorey0
    {
        internal int index;
        internal Weapon5040 $this;

        internal void <>m__0()
        {
            this.$this.CreateBulletOverride(Vector3.zero, (this.$this.perangle * this.index) - (this.$this.allangle / 2f));
        }
    }

    [CompilerGenerated]
    private sealed class <OnAttack>c__AnonStorey1
    {
        internal int index;
        internal Weapon5040 $this;

        internal void <>m__0()
        {
            this.$this.CreateBulletOverride(Vector3.zero, (-this.$this.perangle * this.index) + (this.$this.allangle / 2f));
        }
    }
}

