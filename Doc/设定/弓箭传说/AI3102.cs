using System;

public class AI3102 : AIBase
{
    private int bulletid;
    private float prev_scale;
    private int count = 8;
    private float delay = 0.15f;
    private ThunderContinueMgr.ThunderContinueReceive receive;

    private bool Conditions() => 
        (base.GetIsAlive() && (base.m_Entity.m_HatredTarget != null));

    protected override void OnAIDeInit()
    {
        if (this.receive != null)
        {
            this.receive.Deinit();
        }
    }

    protected override void OnInit()
    {
        int waitTime = GameLogic.Random(500, 0x5dc);
        base.AddAction(base.GetActionWait("actionwaitr1", waitTime));
        base.AddAction(base.GetActionDelegate(delegate {
            ThunderContinueMgr.ThunderContinueData data = new ThunderContinueMgr.ThunderContinueData {
                entity = base.m_Entity,
                bulletid = this.bulletid,
                count = this.count,
                delay = this.delay,
                prev_scale = this.prev_scale
            };
            this.receive = ThunderContinueMgr.GetThunderContinue(data);
        }));
        base.AddAction(base.GetActionWait("actionwaitr1", 0xfa0 - waitTime));
    }

    protected override void OnInitOnce()
    {
        base.OnInitOnce();
        GameLogic.EffectGet("Effect/Monster/3097_yellow").SetParentNormal(base.m_Entity.m_Body.Body);
        CInstance<BattleResourceCreator>.Instance.Get3097Base(base.m_Entity.m_Body.EffectMask.transform.parent).SetTexture("3097_yellow");
        this.bulletid = base.m_Entity.m_Data.WeaponID;
        this.prev_scale = 1f;
        if (base.m_Entity.IsElite)
        {
            this.bulletid = 0x447;
            this.prev_scale = 1.5f;
        }
    }
}

