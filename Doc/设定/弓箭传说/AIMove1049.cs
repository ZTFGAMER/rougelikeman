using Dxx.Util;
using System;
using TableTool;
using UnityEngine;

public class AIMove1049 : AIMoveBase
{
    private EntityBase target;
    private float startTime;
    private float delaytime;
    private float jumptime;
    private float endtime;
    private bool bjumpend;
    private AnimationCurve curve;
    private Vector3 startpos;
    private Vector3 endpos;
    private float height;
    private float skillspeed;
    private GameObject obj;

    public AIMove1049(EntityBase entity) : base(entity)
    {
        this.height = 5f;
        this.skillspeed = 0.2f;
        this.curve = LocalModelManager.Instance.Curve_curve.GetCurve(0x186b4);
    }

    private void AIMoveStart()
    {
        float angle = Utils.getAngle(this.target.position - base.m_Entity.position);
        base.m_Entity.m_AttackCtrl.SetRotate(angle);
        base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("Skill", -this.skillspeed);
        base.m_Entity.m_AniCtrl.SendEvent("Skill", false);
        base.m_Entity.SetTrigger(true);
    }

    private void AIMoving()
    {
        float num = ((Updater.AliveTime - this.startTime) - this.delaytime) / this.jumptime;
        num = MathDxx.Clamp01(num);
        Vector3 vector = ((this.endpos - this.startpos) * num) + this.startpos;
        base.m_Entity.SetPosition(vector + new Vector3(0f, this.curve.Evaluate(num) * this.height, 0f));
        if (num == 1f)
        {
            this.obj = GameLogic.EffectGet("Effect/Boss/BossJumpHit5028");
            this.obj.transform.position = base.m_Entity.position;
            float[] args = new float[] { 1f };
            SkillAloneAttrGoodBase.Add(base.m_Entity, this.obj, true, args);
            this.bjumpend = true;
        }
    }

    private void MoveNormal()
    {
        if (this.bjumpend && ((Updater.AliveTime - this.startTime) > this.endtime))
        {
            base.End();
        }
        else if (!this.bjumpend && ((Updater.AliveTime - this.startTime) > this.delaytime))
        {
            this.AIMoving();
        }
    }

    protected override void OnEnd()
    {
        SkillAloneAttrGoodBase.Remove(this.obj);
        base.m_Entity.SetTrigger(false);
        base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("Skill", this.skillspeed);
    }

    protected override void OnInitBase()
    {
        this.target = GameLogic.Self;
        this.startpos = base.m_Entity.position;
        this.endpos = this.target.position;
        this.startTime = Updater.AliveTime;
        this.delaytime = 0.1f / (1f - this.skillspeed);
        this.jumptime = 0.55f / (1f - this.skillspeed);
        this.endtime = 1.1f / (1f - this.skillspeed);
        this.bjumpend = false;
        this.AIMoveStart();
    }

    protected override void OnUpdate()
    {
        this.MoveNormal();
    }
}

