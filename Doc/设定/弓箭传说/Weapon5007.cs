using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Weapon5007 : WeaponBase
{
    protected override void OnAttack(object[] args)
    {
        int num = 5;
        for (int i = 0; i < num; i++)
        {
            <OnAttack>c__AnonStorey0 storey = new <OnAttack>c__AnonStorey0 {
                $this = this,
                cc = i
            };
            ActionBasic.ActionDelegate action = new ActionBasic.ActionDelegate {
                action = new Action(storey.<>m__0)
            };
            base.action.AddAction(action);
            ActionBasic.ActionWait wait = new ActionBasic.ActionWait {
                waitTime = 0.15f
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
        internal int cc;
        internal Weapon5007 $this;

        internal void <>m__0()
        {
            GameLogic.Hold.Sound.PlayBulletCreate(0x1e903c, this.$this.m_Entity.position);
            if ((this.cc % 2) == 0)
            {
                for (int i = 0; i < 5; i++)
                {
                    this.$this.CreateBulletOverride(new Vector3((i * 0.2f) - 0.4f, 0f, 0f), (float) ((i * 20) - 40));
                }
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    this.$this.CreateBulletOverride(new Vector3((i * 0.4f) - 0.8f, 0f, 0f), (float) ((i * 20) - 40));
                }
            }
        }
    }
}

