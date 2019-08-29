using DG.Tweening;
using System;

public class AIBeeBase : AIBase
{
    private Sequence seq;

    private void KillSequence()
    {
        if (this.seq != null)
        {
            TweenExtensions.Kill(this.seq, false);
            this.seq = null;
        }
    }

    protected override void OnAIDeInit()
    {
        this.KillSequence();
    }

    protected override void OnInitOnce()
    {
        this.seq = DOTween.Sequence();
        TweenSettingsExtensions.SetLoops<Sequence>(TweenSettingsExtensions.AppendInterval(TweenSettingsExtensions.AppendCallback(this.seq, new TweenCallback(this, this.<OnInitOnce>m__0)), 2.2f), -1);
    }
}

