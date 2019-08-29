using Dxx.Util;
using System;
using TableTool;
using UnityEngine;

public class AIMove1035 : AIMoveBase
{
    private EntityBase target;
    private AnimationCurve curve;
    private int range;
    private float playerrange;
    private float ratio;
    private Vector3 startpos;
    private Vector3 endpos;
    private float alltime;
    private float height;
    private float currenttime;

    public AIMove1035(EntityBase entity, int range, float playerrange, float ratio) : base(entity)
    {
        this.height = 5f;
        this.target = GameLogic.Self;
        this.range = range;
        this.playerrange = playerrange;
        this.ratio = ratio;
        this.curve = LocalModelManager.Instance.Curve_curve.GetCurve(0x186b7);
    }

    private void CreateBullet()
    {
        if (base.m_Entity.IsElite)
        {
            float num = GameLogic.Random(0, 360);
            int num2 = 6;
            for (int i = 0; i < num2; i++)
            {
                GameLogic.Release.Bullet.CreateBullet(base.m_Entity, 0x13c5, base.m_Entity.position + new Vector3(0f, 0.5f, 0f), num + (i * 60f));
            }
        }
    }

    protected override void OnEnd()
    {
        base.m_Entity.m_AniCtrl.SetString("Skill", "Jump3011");
        base.m_Entity.m_AniCtrl.SendEvent("Skill", false);
    }

    protected override void OnInitBase()
    {
        this.currenttime = 0f;
        Vector3 vector = base.m_Entity.position - this.target.position;
        float magnitude = vector.magnitude;
        float num2 = GameLogic.Random((float) 0f, (float) 1f);
        if ((magnitude < this.playerrange) && (num2 < this.ratio))
        {
            this.endpos = this.target.position;
        }
        else
        {
            GameLogic.Release.MapCreatorCtrl.RandomItemSide(base.m_Entity, this.range, out float num3, out float num4);
            this.endpos = new Vector3(num3, 0f, num4);
        }
        this.UpdateDirection();
        this.startpos = base.m_Entity.position;
        Vector3 vector2 = this.endpos - this.startpos;
        this.alltime = vector2.magnitude / 7f;
        GameLogic.Hold.Sound.PlayMonsterSkill(0x4dd1e7, base.m_Entity.position);
    }

    protected override void OnUpdate()
    {
        this.currenttime += Updater.delta;
        if (this.currenttime > this.alltime)
        {
            this.currenttime = this.alltime;
        }
        float y = this.curve.Evaluate(this.currenttime / this.alltime) * this.height;
        float num2 = this.currenttime / this.alltime;
        base.m_Entity.SetPosition((((this.endpos - this.startpos) * num2) + this.startpos) + new Vector3(0f, y, 0f));
        if (this.currenttime == this.alltime)
        {
            this.CreateBullet();
            base.End();
        }
    }

    private void UpdateDirection()
    {
        float x = this.endpos.x - base.m_Entity.position.x;
        float y = this.endpos.z - base.m_Entity.position.z;
        this.m_MoveData.angle = Utils.getAngle(x, y);
        Vector3 vector3 = new Vector3(x, 0f, y);
        this.m_MoveData.direction = vector3.normalized;
        base.m_Entity.m_AttackCtrl.RotateHero(this.m_MoveData.angle);
    }
}

