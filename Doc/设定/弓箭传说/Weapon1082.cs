using Dxx.Util;
using System;
using TableTool;
using UnityEngine;

public class Weapon1082 : WeaponBase
{
    protected float distance = 4f;
    protected float delaytime = 0.4f;
    protected float updatetime = 0.2f;
    private bool bStart;
    private float starttime;
    private float percent;
    private float x;
    private float y;
    private Vector3 startpos;
    private Vector3 endpos;
    private float percentbefore;
    private float percentchange;
    private AnimationCurve curve;

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
        if (base.m_Entity.IsElite)
        {
            this.distance = 5f;
        }
        Updater.AddUpdate("Weapon1082", new Action<float>(this.OnUpdate), false);
        base.OnAttackStartStartAction = new Action(this.OnAttackStartStart);
        base.OnAttackStartEndAction = new Action(this.OnAttackStartEnd);
        this.curve = LocalModelManager.Instance.Curve_curve.GetCurve(0x186b9);
        base.OnInstall();
    }

    protected override void OnUnInstall()
    {
        Updater.RemoveUpdate("Weapon1082", new Action<float>(this.OnUpdate));
        base.OnAttackStartStartAction = null;
        base.OnAttackStartEndAction = null;
        base.OnUnInstall();
    }

    private void OnUpdate(float delta)
    {
        if (this.bStart && (Updater.AliveTime >= this.starttime))
        {
            if (this.percentbefore == 0f)
            {
                GameLogic.Hold.Sound.PlayBattleSpecial(0x4dd1e4, base.m_Entity.position);
            }
            this.percent = (Updater.AliveTime - this.starttime) / this.updatetime;
            this.percent = MathDxx.Clamp01(this.percent);
            this.percentchange = this.percent - this.percentbefore;
            this.percentbefore = this.percent;
            base.m_Entity.SetPositionBy((this.endpos - this.startpos) * this.percentchange);
            base.m_Entity.SetPosition(new Vector3(base.m_Entity.position.x, this.curve.Evaluate(this.percent) * 3f, base.m_Entity.position.z));
            if (this.percent == 1f)
            {
                this.bStart = false;
            }
        }
    }
}

