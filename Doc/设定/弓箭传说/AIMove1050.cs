using DG.Tweening;
using System;
using UnityEngine;

public class AIMove1050 : AIMoveBase
{
    protected EntityBase target;
    protected Vector3 nextpos;
    protected Vector3 endpos;
    private Sequence seq;
    private Animation ani;
    private int skillendcount;
    private bool bDizzy;

    public AIMove1050(EntityBase entity) : base(entity)
    {
        base.name = "AIMove1050";
    }

    private void DeInitSeq()
    {
        if (this.seq != null)
        {
            TweenExtensions.Kill(this.seq, false);
            this.seq = null;
        }
    }

    protected override void OnEnd()
    {
        this.DeInitSeq();
        base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("Skill", -1.5f);
        base.m_Entity.OnSkillActionEnd = (Action) Delegate.Remove(base.m_Entity.OnSkillActionEnd, new Action(this.OnSkillEnd));
        base.m_Entity.m_AniCtrl.SetString("SkillEnd", string.Empty);
    }

    protected override void OnInitBase()
    {
        this.skillendcount = 0;
        this.target = GameLogic.Self;
        this.endpos = GameLogic.Release.MapCreatorCtrl.RandomPosition();
        this.SetAnimation();
    }

    private void OnSkillEnd()
    {
        this.skillendcount++;
        if (this.skillendcount == 1)
        {
            base.m_Entity.SetPosition(new Vector3(base.m_Entity.position.x, -100f, base.m_Entity.position.z));
        }
        else if (this.skillendcount == 2)
        {
            int num = 0x10;
            float num2 = 360f / ((float) num);
            Vector3 pos = base.m_Entity.position + new Vector3(0f, 1f, 0f);
            for (int i = 0; i < num; i++)
            {
                GameLogic.Release.Bullet.CreateBullet(base.m_Entity, 0x13cf, pos, i * num2);
            }
            this.seq = DOTween.Sequence();
            TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(this.seq, 0.5f), new TweenCallback(this, this.End));
        }
    }

    private void SetAnimation()
    {
        base.m_Entity.m_AniCtrl.SetString("Skill", "Spawn");
        base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("Skill", 1.5f);
        base.m_Entity.mAniCtrlBase.SetAnimationRevert("Skill", true);
        base.m_Entity.m_AniCtrl.SendEvent("Skill", false);
        base.m_Entity.m_AniCtrl.SetString("SkillEnd", string.Empty);
        base.m_Entity.ShowHP(false);
        base.m_Entity.SetCollider(false);
        base.m_Entity.OnSkillActionEnd = (Action) Delegate.Combine(base.m_Entity.OnSkillActionEnd, new Action(this.OnSkillEnd));
        float animationTime = base.m_Entity.m_AniCtrl.GetAnimationTime("Skill");
        this.seq = DOTween.Sequence();
        TweenSettingsExtensions.AppendInterval(this.seq, animationTime + 0.5f);
        TweenSettingsExtensions.AppendCallback(this.seq, new TweenCallback(this, this.<SetAnimation>m__0));
        TweenSettingsExtensions.AppendInterval(this.seq, 0.2f);
        TweenSettingsExtensions.AppendCallback(this.seq, new TweenCallback(this, this.<SetAnimation>m__1));
    }
}

