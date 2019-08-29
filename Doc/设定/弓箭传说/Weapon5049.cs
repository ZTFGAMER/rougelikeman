using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Weapon5049 : WeaponBase
{
    private int count = 2;

    protected override void OnAttack(object[] args)
    {
        for (int i = 0; i < this.count; i++)
        {
            <OnAttack>c__AnonStorey0 storey = new <OnAttack>c__AnonStorey0 {
                $this = this,
                index = i
            };
            base.action.AddActionDelegate(new Action(storey.<>m__0));
            if (i < (this.count - 1))
            {
                base.action.AddActionWait(0.15f);
            }
        }
    }

    protected override void OnInit()
    {
        base.OnInit();
    }

    protected override void OnUnInstall()
    {
        base.OnUnInstall();
    }

    [CompilerGenerated]
    private sealed class <OnAttack>c__AnonStorey0
    {
        internal int index;
        internal Weapon5049 $this;

        internal void <>m__0()
        {
            for (int i = 0; i < 4; i++)
            {
                float[] args = new float[] { (float) (0x186ab + (((this.index % 2) != 0) ? 1 : 0)) };
                this.$this.CreateBulletOverride(Vector3.zero, (i * 30) - 45f).SetArgs(args);
            }
        }
    }
}

