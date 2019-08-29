using DG.Tweening;
using Dxx.Util;
using System;

public class AI3098 : AIBase
{
    private SequencePool mSeqPool = new SequencePool();
    private int index;
    private float startangle;

    private bool Conditions() => 
        (base.GetIsAlive() && (base.m_Entity.m_HatredTarget != null));

    private void KillSequence()
    {
        this.mSeqPool.Clear();
    }

    protected override void OnAIDeInit()
    {
        this.KillSequence();
    }

    protected override void OnInit()
    {
    }

    protected override void OnInitOnce()
    {
        GameLogic.EffectGet("Effect/Monster/3097_blue").SetParentNormal(base.m_Entity.m_Body.Body);
        CInstance<BattleResourceCreator>.Instance.Get3097Base(base.m_Entity.m_Body.EffectMask.transform.parent).SetTexture("3097_blue");
        this.startangle = GameLogic.Random(0, 8) * 45f;
        this.KillSequence();
        TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(this.mSeqPool.Get(), GameLogic.Random((float) 1.5f, (float) 2f)), new TweenCallback(this, this.<OnInitOnce>m__0));
    }
}

