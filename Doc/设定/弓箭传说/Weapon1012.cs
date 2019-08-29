﻿using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Weapon1012 : WeaponBase
{
    protected override void OnAttack(object[] args)
    {
        for (int i = 0; i < 8; i++)
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
                waitTime = 0.037f
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
        internal Weapon1012 $this;

        internal void <>m__0()
        {
            this.$this.CreateBulletOverride(Vector3.zero, (float) (-45 * this.index));
        }
    }
}

