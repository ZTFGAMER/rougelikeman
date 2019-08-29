using Dxx.Util;
using System;
using UnityEngine;

public class AI3052 : AIBase
{
    private ActionBattle action = new ActionBattle();
    private int bulletid;
    private float startangle;

    private void CreateBullets()
    {
        this.CreateBulletsByStartAngle(this.startangle);
        this.CreateBulletsByStartAngle(this.startangle + 120f);
        this.CreateBulletsByStartAngle(this.startangle + 240f);
        this.startangle += 30f;
    }

    private void CreateBulletsByStartAngle(float angle)
    {
        int count = 3;
        for (int i = 0; i < count; i++)
        {
            float num3 = Utils.GetBulletAngle(i, count, 60f) + angle;
            float x = MathDxx.Sin(num3);
            float z = MathDxx.Cos(num3);
            Vector3 vector = new Vector3(x, 0f, z) * 1f;
            GameLogic.Release.Bullet.CreateBullet(base.m_Entity, this.bulletid, base.m_Entity.m_Body.LeftBullet.transform.position + vector, num3);
        }
    }

    protected override void OnAIDeInit()
    {
        this.action.DeInit();
    }

    protected override void OnInit()
    {
        this.bulletid = base.m_Entity.m_Data.WeaponID;
        this.action.Init(base.m_Entity);
        base.AddAction(base.GetActionWait(string.Empty, 0x7d0));
        base.AddAction(base.GetActionDelegate(() => this.CreateBullets()));
        base.AddAction(base.GetActionWait(string.Empty, 0x7d0));
    }
}

