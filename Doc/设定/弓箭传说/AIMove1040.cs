using Dxx.Util;
using System;
using TableTool;
using UnityEngine;

public class AIMove1040 : AIMoveBase
{
    private const float Height = 6f;
    private EntityBase target;
    private Vector3 startpos;
    private Vector3 endpos;
    private int range;
    private float jumptime;
    private float alltime;
    private float starttime;
    private float percent;
    private AnimationCurve curve;
    private bool bJumpEnd;

    public AIMove1040(EntityBase entity, int range) : base(entity)
    {
        this.jumptime = 0.65f;
        this.range = range;
        this.curve = LocalModelManager.Instance.Curve_curve.GetCurve(0x186aa);
    }

    private void Attack()
    {
        for (int i = 0; i < 8; i++)
        {
            GameLogic.Release.Bullet.CreateBullet(base.m_Entity, 0x13b3, base.m_Entity.position + new Vector3(0f, 1f, 0f), i * 45f);
        }
    }

    protected override void OnEnd()
    {
        base.m_Entity.OnSkillActionEnd = (Action) Delegate.Remove(base.m_Entity.OnSkillActionEnd, new Action(this.End));
    }

    protected override void OnInitBase()
    {
        this.bJumpEnd = false;
        this.starttime = 0f;
        GameLogic.Release.MapCreatorCtrl.RandomItemSide(GameLogic.Self, this.range, out float num, out float num2);
        this.endpos = new Vector3(num, 0f, num2);
        this.target = GameLogic.Self;
        this.starttime = Updater.AliveTime;
        this.startpos = base.m_Entity.position;
        base.m_Entity.m_AniCtrl.SendEvent("Skill", false);
        this.alltime = base.m_Entity.m_AniCtrl.GetAnimationTime("Skill");
        base.m_Entity.m_AttackCtrl.RotateHero(Utils.getAngle(this.endpos - this.startpos));
        base.m_Entity.OnSkillActionEnd = (Action) Delegate.Combine(base.m_Entity.OnSkillActionEnd, new Action(this.End));
    }

    protected override void OnUpdate()
    {
        if ((this.starttime == 0f) && base.m_Entity.m_AttackCtrl.RotateOver())
        {
            this.starttime = Updater.AliveTime;
        }
        if ((this.starttime > 0f) && !this.bJumpEnd)
        {
            this.percent = (Updater.AliveTime - this.starttime) / this.jumptime;
            this.percent = MathDxx.Clamp01(this.percent);
            base.m_Entity.SetPosition((this.startpos + ((this.endpos - this.startpos) * this.percent)) + new Vector3(0f, this.curve.Evaluate(this.percent) * 6f, 0f));
            if (this.percent == 1f)
            {
                this.bJumpEnd = true;
                this.Attack();
            }
        }
    }
}

