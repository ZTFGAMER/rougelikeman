using Dxx.Util;
using System;
using UnityEngine;

public class WeaponSprintBase : WeaponBase
{
    protected float distance = 2f;
    protected float delaytime = 0.4f;
    protected float updatetime = 0.1f;
    private bool bStart;
    private float starttime;
    private float percent;
    private float x;
    private float y;
    private Vector3 startpos;
    private Vector3 endpos;
    private Vector3 currentmove;
    private float percentbefore;
    private float percentchange;

    private void OnAttackStartEnd()
    {
        this.bStart = false;
    }

    private void OnAttackStartStart()
    {
        this.bStart = true;
        this.percentbefore = 0f;
        this.starttime = Updater.AliveTime + this.delaytime;
        this.x = MathDxx.Sin(base.m_Entity.eulerAngles.y);
        this.y = MathDxx.Cos(base.m_Entity.eulerAngles.y);
        this.startpos = base.m_Entity.position;
        Vector3 vector3 = new Vector3(this.x, 0f, this.y);
        this.endpos = (vector3.normalized * this.distance) + base.m_Entity.position;
    }

    protected override void OnInstall()
    {
        Updater.AddUpdate("WeaponSprintBase", new Action<float>(this.OnUpdate), false);
        base.OnAttackStartStartAction = new Action(this.OnAttackStartStart);
        base.OnAttackStartEndAction = new Action(this.OnAttackStartEnd);
        base.OnInstall();
    }

    protected override void OnUnInstall()
    {
        Updater.RemoveUpdate("WeaponSprintBase", new Action<float>(this.OnUpdate));
        base.OnAttackStartStartAction = null;
        base.OnAttackStartEndAction = null;
        base.OnUnInstall();
    }

    private void OnUpdate(float delta)
    {
        if (this.bStart && (Updater.AliveTime >= this.starttime))
        {
            this.percent = (Updater.AliveTime - this.starttime) / this.updatetime;
            this.percent = MathDxx.Clamp01(this.percent);
            this.percentchange = this.percent - this.percentbefore;
            this.percentbefore = this.percent;
            this.currentmove = (this.endpos - this.startpos) * this.percentchange;
            base.m_Entity.SetPositionBy(this.currentmove);
            this.OnUpdateMove(this.currentmove.magnitude);
            if (this.percent == 1f)
            {
                this.bStart = false;
            }
        }
    }

    protected virtual void OnUpdateMove(float currentdis)
    {
    }
}

