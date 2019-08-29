using DG.Tweening;
using System;

public class AI3099 : AIBase
{
    protected Sequence seq;
    private int bulletid;

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
        base.AddAction(base.GetActionWait("actionwaitr1", 0x7d0));
        base.AddAction(base.GetActionDelegate(delegate {
            float num = GameLogic.Random((float) 0f, (float) 360f);
            for (int i = 0; i < 9; i++)
            {
                int num3 = i;
                GameLogic.Release.Bullet.CreateBullet(base.m_Entity, this.bulletid, base.m_Entity.m_Body.LeftBullet.transform.position, (((float) (num3 * 360)) / 9f) + num);
            }
        }));
        base.AddAction(base.GetActionWait("actionwaitr1", 0x3e8));
    }

    protected override void OnInitOnce()
    {
        base.OnInitOnce();
        GameLogic.EffectGet("Effect/Monster/3097_red").SetParentNormal(base.m_Entity.m_Body.Body);
        CInstance<BattleResourceCreator>.Instance.Get3097Base(base.m_Entity.m_Body.EffectMask.transform.parent).SetTexture("3097_red");
        this.bulletid = base.m_Entity.m_Data.WeaponID;
        if (base.m_Entity.IsElite)
        {
            this.bulletid = 0x446;
        }
    }
}

