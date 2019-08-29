using Dxx.Util;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Weapon5080 : WeaponBase
{
    private void AddAttack3()
    {
        <AddAttack3>c__AnonStorey1 storey = new <AddAttack3>c__AnonStorey1 {
            $this = this,
            count = 3
        };
        for (int i = 0; i < storey.count; i++)
        {
            <AddAttack3>c__AnonStorey0 storey2 = new <AddAttack3>c__AnonStorey0 {
                <>f__ref$1 = storey,
                index = i
            };
            ActionBasic.ActionDelegate action = new ActionBasic.ActionDelegate {
                action = new Action(storey2.<>m__0)
            };
            base.action.AddAction(action);
        }
    }

    protected override void OnAttack(object[] args)
    {
        this.AddAttack3();
    }

    protected override void OnInstall()
    {
    }

    protected override void OnUnInstall()
    {
    }

    [CompilerGenerated]
    private sealed class <AddAttack3>c__AnonStorey0
    {
        internal int index;
        internal Weapon5080.<AddAttack3>c__AnonStorey1 <>f__ref$1;

        internal void <>m__0()
        {
            this.<>f__ref$1.$this.CreateBulletOverride(Vector3.zero, Utils.GetBulletAngle(this.index, this.<>f__ref$1.count, 90f));
        }
    }

    [CompilerGenerated]
    private sealed class <AddAttack3>c__AnonStorey1
    {
        internal int count;
        internal Weapon5080 $this;
    }
}

