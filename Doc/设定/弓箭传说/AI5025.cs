using System;

public class AI5025 : AIBase
{
    private int callid = 0xbbd;

    private bool Conditions() => 
        (base.GetIsAlive() && (base.m_Entity.m_HatredTarget != null));

    private ActionBasic.ActionBase GetActionAttackOne()
    {
        int waittime = 400;
        AIBase.ActionSequence sequence = new AIBase.ActionSequence {
            name = "actionseq",
            m_Entity = base.m_Entity
        };
        int num2 = GameLogic.Random(3, 6);
        for (int i = 0; i < num2; i++)
        {
            sequence.AddAction(base.GetActionAttackWait(0x13c4, waittime, waittime));
        }
        sequence.AddAction(base.GetActionWaitRandom(string.Empty, 300, 500));
        sequence.AddAction(this.GetActionMoveOne());
        return sequence;
    }

    private ActionBasic.ActionBase GetActionMoveOne()
    {
        AIBase.ActionSequence sequence = new AIBase.ActionSequence {
            name = "actionseq",
            m_Entity = base.m_Entity
        };
        sequence.AddAction(new AIMove1002(base.m_Entity, 300, 600));
        sequence.AddAction(base.GetActionWait("actionwait1", 200));
        return sequence;
    }

    protected override void OnAIDeInit()
    {
    }

    protected override void OnInit()
    {
        if ((GameLogic.Random(0, 100) < 40) && base.GetCanCall(this.callid))
        {
            base.AddAction(base.GetActionWait(string.Empty, 500));
            base.AddActionAddCall(this.callid, 0x1195);
        }
        else
        {
            base.AddAction(this.GetActionAttackOne());
        }
        base.AddAction(this.GetActionMoveOne());
        base.bReRandom = true;
    }

    protected override void OnInitOnce()
    {
        base.InitCallData(this.callid, 2, 0x7fffffff, 1, 2, 8);
    }
}

