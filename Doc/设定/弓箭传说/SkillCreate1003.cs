using DG.Tweening;
using System;
using UnityEngine;

public class SkillCreate1003 : SkillCreateBase
{
    private Transform mCloud;
    private float hitratio;
    private float range;
    private Sequence seqcloud;
    private GameObject line;
    private LightingLineCtrl linectrl;

    protected override void OnAwake()
    {
        this.mCloud = base.transform.Find("child/cloud");
    }

    protected override void OnDeinit()
    {
        if (this.seqcloud != null)
        {
            TweenExtensions.Kill(this.seqcloud, false);
        }
    }

    protected override void OnInit(string[] args)
    {
        base.time = float.Parse(args[0]);
        float num = float.Parse(args[1]);
        this.hitratio = float.Parse(args[2]);
        this.range = float.Parse(args[3]);
        this.seqcloud = TweenSettingsExtensions.SetLoops<Sequence>(TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(DOTween.Sequence(), num), new TweenCallback(this, this.<OnInit>m__0)), -1);
    }

    private void OnStepUpdate()
    {
        EntityBase target = GameLogic.Release.Entity.GetNearEntity(base.m_Entity, this.range, false);
        if (target != null)
        {
            long num = (long) (base.m_Entity.m_EntityData.GetAttackBase() * this.hitratio);
            GameLogic.SendHit_Skill(target, -num);
            this.line = GameLogic.EffectGet("Game/SkillPrefab/SkillAlone1042_One");
            this.linectrl = this.line.GetComponent<LightingLineCtrl>();
            this.linectrl.Init(this.mCloud, target);
        }
    }
}

