using Dxx.Util;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Weapon5045 : WeaponBase
{
    private int count = 7;
    private Vector3 dir;
    private float attackprevtime;
    private float starttime;
    private bool bAttackPrevEnd;

    protected override void OnAttack(object[] args)
    {
        this.bAttackPrevEnd = true;
        this.dir = Utils.GetDirection(base.m_Entity.eulerAngles.y);
        for (int i = 0; i < 5; i++)
        {
            <OnAttack>c__AnonStorey0 storey = new <OnAttack>c__AnonStorey0 {
                $this = this,
                index = i
            };
            base.action.AddActionDelegate(new Action(storey.<>m__0));
            if (i < (this.count - 1))
            {
                base.action.AddActionWait(0.07f);
            }
        }
    }

    protected override void OnInit()
    {
        this.bAttackPrevEnd = false;
        base.OnInit();
        this.starttime = Updater.AliveTime;
        this.attackprevtime = 0.5f;
        Updater.AddUpdate("Weapon5045", new Action<float>(this.OnUpdate), false);
    }

    protected override void OnUnInstall()
    {
        Updater.RemoveUpdate("Weapon5045", new Action<float>(this.OnUpdate));
        base.OnUnInstall();
    }

    private void OnUpdate(float delta)
    {
        if ((Updater.AliveTime - this.starttime) >= this.attackprevtime)
        {
            Updater.RemoveUpdate("Weapon5045", new Action<float>(this.OnUpdate));
        }
        else
        {
            base.m_Entity.m_AttackCtrl.RotateHero(base.m_Entity.m_HatredTarget);
        }
    }

    [CompilerGenerated]
    private sealed class <OnAttack>c__AnonStorey0
    {
        internal int index;
        internal Weapon5045 $this;

        internal void <>m__0()
        {
            Transform transform = this.$this.CreateBullet(Vector3.zero, 0f);
            transform.transform.position = this.$this.m_Entity.position + ((this.$this.dir * (this.index + 1)) * 3.5f);
            BulletBase component = transform.GetComponent<BulletBase>();
            component.SetBulletAttribute(new BulletTransmit(this.$this.m_Entity, this.$this.BulletID, false));
            component.SetTarget(this.$this.Target, 1);
        }
    }
}

