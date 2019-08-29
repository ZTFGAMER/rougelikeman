using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Weapon1006 : WeaponBase
{
    protected override void OnAttack(object[] args)
    {
        for (int i = 0; i < 4; i++)
        {
            <OnAttack>c__AnonStorey0 storey = new <OnAttack>c__AnonStorey0 {
                $this = this,
                index = i
            };
            ActionBasic.ActionDelegate action = new ActionBasic.ActionDelegate {
                action = new Action(storey.<>m__0)
            };
            base.action.AddAction(action);
            ActionBasic.ActionWait wait = new ActionBasic.ActionWait {
                waitTime = 0.04f
            };
            base.action.AddAction(wait);
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
        internal Weapon1006 $this;

        internal void <>m__0()
        {
            this.$this.CreateBulletOverride(Vector3.zero, (float) (-90 * this.index));
        }
    }
}

