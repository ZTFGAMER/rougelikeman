using Dxx.Util;
using System;
using UnityEngine;

public class Weapon1066 : Weapon1024
{
    private GameObject redline;
    private BulletRedLineCtrl ctrl;
    private float time;
    private float alltime = 0.5f;
    private float mindis;

    private void DeinitRedLine()
    {
        if (this.redline != null)
        {
            Object.Destroy(this.redline);
        }
    }

    protected override void OnAttack(object[] args)
    {
        this.DeinitRedLine();
        base.OnAttack(args);
    }

    protected override void OnInstall()
    {
        base.AttackEffect = "WeaponHand1066Effect";
        this.redline = Object.Instantiate<GameObject>(ResourceManager.Load<GameObject>("Game/Bullet/Bullet1066_RedLine"));
        this.redline.SetParentNormal(base.m_Entity.m_Body.transform);
        this.ctrl = this.redline.GetComponent<BulletRedLineCtrl>();
        this.ctrl.SetLine(true, 0f);
        this.time = 0f;
        Vector3 vector = new Vector3(MathDxx.Sin(base.m_Entity.eulerAngles.y + 90f), 0f, MathDxx.Cos(base.m_Entity.eulerAngles.y + 90f)) * 0.5f;
        RayCastManager.CastMinDistance(base.m_Entity.m_Body.transform.position + vector, base.m_Entity.eulerAngles.y, false, out float num);
        RayCastManager.CastMinDistance(base.m_Entity.m_Body.transform.position - vector, base.m_Entity.eulerAngles.y, false, out float num2);
        this.mindis = (num >= num2) ? num2 : num;
        Updater.AddUpdate("Weapon1066", new Action<float>(this.OnUpdate), false);
        base.OnInstall();
    }

    protected override void OnUnInstall()
    {
        this.DeinitRedLine();
        Updater.RemoveUpdate("Weapon1066", new Action<float>(this.OnUpdate));
        base.OnUnInstall();
    }

    private void OnUpdate(float delta)
    {
        this.time += delta;
        this.time = MathDxx.Clamp(this.time, 0f, this.alltime);
        this.ctrl.SetLine(true, this.mindis * (this.time / this.alltime));
    }
}

