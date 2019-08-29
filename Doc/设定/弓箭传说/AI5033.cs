using DG.Tweening;
using Dxx.Util;
using System;

public class AI5033 : AIBase
{
    private WeightRandomCount weight = new WeightRandomCount(2, 4);
    private float attackadd = 0.3f;
    private Sequence seq;

    protected override void OnAIDeInit()
    {
        if (this.seq != null)
        {
            TweenExtensions.Kill(this.seq, false);
            this.seq = null;
        }
    }

    protected override void OnInit()
    {
        AIBase.ActionSequence sequence2;
        switch (this.weight.GetRandom())
        {
            case 0:
            {
                sequence2 = new AIBase.ActionSequence {
                    name = "actionseq",
                    m_Entity = base.m_Entity
                };
                AIBase.ActionSequence action = sequence2;
                action.AddAction(base.GetActionAttack(string.Empty, 0x13db, true));
                action.AddAction(base.GetActionWaitRandom("actionwait", 500, 0x3e8));
                base.AddAction(action);
                break;
            }
            case 1:
            {
                sequence2 = new AIBase.ActionSequence {
                    name = "actionseq",
                    m_Entity = base.m_Entity
                };
                AIBase.ActionSequence action = sequence2;
                action.AddAction(base.GetActionAttack(string.Empty, 0x13dd, true));
                action.AddAction(base.GetActionWaitRandom("actionwait", 600, 0x3e8));
                base.AddAction(action);
                break;
            }
            case 2:
            {
                sequence2 = new AIBase.ActionSequence {
                    name = "actionseq",
                    m_Entity = base.m_Entity
                };
                AIBase.ActionSequence action = sequence2;
                action.AddAction(base.GetActionDelegate(delegate {
                    base.m_Entity.m_AniCtrl.SendEvent("Continuous", false);
                    this.seq = DOTween.Sequence();
                    int num = 30;
                    for (int i = 0; i < num; i++)
                    {
                        TweenSettingsExtensions.AppendCallback(this.seq, new TweenCallback(this, this.<OnInit>m__1));
                        TweenSettingsExtensions.AppendInterval(this.seq, 0.06f);
                    }
                    TweenSettingsExtensions.AppendCallback(this.seq, new TweenCallback(this, this.<OnInit>m__2));
                }));
                action.AddAction(base.GetActionWaitRandom("actionwait", 0x9c4, 0xbb8));
                base.AddAction(action);
                break;
            }
            case 3:
                base.AddAction(new AIMove1002(base.m_Entity, 500, 0x3e8));
                break;
        }
        base.bReRandom = true;
    }
}

