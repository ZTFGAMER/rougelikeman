using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Weapon1030 : WeaponBase
{
    protected override void OnAttack(object[] args)
    {
        <OnAttack>c__AnonStorey0 storey = new <OnAttack>c__AnonStorey0 {
            $this = this
        };
        int num = 0x24;
        storey.per = 360f / ((float) num);
        for (int i = 0; i < num; i++)
        {
            <OnAttack>c__AnonStorey1 storey2 = new <OnAttack>c__AnonStorey1 {
                <>f__ref$0 = storey,
                index = i
            };
            base.action.AddActionWaitDelegate(0.01f, new Action(storey2.<>m__0));
        }
    }

    protected override void OnInstall()
    {
        base.OnInstall();
    }

    protected override void OnUnInstall()
    {
        base.OnUnInstall();
    }

    [CompilerGenerated]
    private sealed class <OnAttack>c__AnonStorey0
    {
        internal float per;
        internal Weapon1030 $this;
    }

    [CompilerGenerated]
    private sealed class <OnAttack>c__AnonStorey1
    {
        internal int index;
        internal Weapon1030.<OnAttack>c__AnonStorey0 <>f__ref$0;

        internal void <>m__0()
        {
            for (int i = 0; i < 4; i++)
            {
                this.<>f__ref$0.$this.CreateBulletOverride(Vector3.zero, (i * 90f) + (this.<>f__ref$0.per * this.index));
            }
        }
    }
}

