using Dxx.Util;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Weapon5081 : WeaponBase
{
    private void AddAttack4()
    {
        <AddAttack4>c__AnonStorey1 storey = new <AddAttack4>c__AnonStorey1 {
            $this = this,
            count = 4
        };
        for (int i = 0; i < storey.count; i++)
        {
            <AddAttack4>c__AnonStorey0 storey2 = new <AddAttack4>c__AnonStorey0 {
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
        this.AddAttack4();
    }

    protected override void OnInstall()
    {
    }

    protected override void OnUnInstall()
    {
    }

    [CompilerGenerated]
    private sealed class <AddAttack4>c__AnonStorey0
    {
        internal int index;
        internal Weapon5081.<AddAttack4>c__AnonStorey1 <>f__ref$1;

        internal void <>m__0()
        {
            this.<>f__ref$1.$this.CreateBulletOverride(Vector3.zero, Utils.GetBulletAngle(this.index, this.<>f__ref$1.count, 120f));
        }
    }

    [CompilerGenerated]
    private sealed class <AddAttack4>c__AnonStorey1
    {
        internal int count;
        internal Weapon5081 $this;
    }
}

