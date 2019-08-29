using System;

public class AI5024 : AIBase
{
    private bool call;
    private int count;
    private int range = 3;
    private int callid = 0xbbb;

    private ActionBasic.ActionBase GetActionSequence(int attackId, int waitTime)
    {
        AIBase.ActionSequence sequence = new AIBase.ActionSequence {
            m_Entity = base.m_Entity
        };
        sequence.AddAction(base.GetActionAttack("actionattack", attackId, true));
        sequence.AddAction(base.GetActionWait("actionwait", waitTime));
        return sequence;
    }

    protected override void OnAIDeInit()
    {
    }

    protected override void OnInit()
    {
        base.AddAction(base.GetActionWaitRandom("actionwait1", 600, 0x3e8));
        this.call = GameLogic.Random(0, 100) < 50;
        if (this.call && base.GetCanCall(this.callid))
        {
            base.AddActionAddCall(this.callid, 0x1195);
        }
        else
        {
            this.count = GameLogic.Random(2, 4);
            AIBase.ActionSequence action = new AIBase.ActionSequence {
                name = string.Empty,
                m_Entity = base.m_Entity
            };
            for (int i = 0; i < this.count; i++)
            {
                action.AddAction(this.GetActionSequence(0x13c2, 50));
            }
            base.AddAction(action);
        }
        base.bReRandom = true;
    }

    protected override void OnInitOnce()
    {
        base.InitCallData(this.callid, 3, 0x7fffffff, 1, 4, 10);
    }
}

