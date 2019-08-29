using Dxx.Util;
using System;

public class AI5046 : AIBase
{
    private WeightRandomCount mWeight = new WeightRandomCount(1);
    private int movecount = 3;

    private ActionBasic.ActionBase GetActionMoveOne(int waittime, int waitmaxtime)
    {
        AIBase.ActionSequence sequence = new AIBase.ActionSequence {
            name = "actionseq",
            m_Entity = base.m_Entity
        };
        AIMove1019 action = new AIMove1019(base.m_Entity) {
            attackid = 0x1403
        };
        sequence.AddAction(action);
        sequence.AddAction(base.GetActionWaitRandom("actionwait1", waittime, waitmaxtime));
        return sequence;
    }

    private ActionBasic.ActionBase GetActionMoves(int count)
    {
        AIBase.ActionSequence sequence = new AIBase.ActionSequence {
            m_Entity = base.m_Entity
        };
        for (int i = 0; i < count; i++)
        {
            sequence.AddAction(this.GetActionMoveOne(0x3e8, 0x7d0));
        }
        return sequence;
    }

    private bool GetHPMore40() => 
        (base.m_Entity.m_EntityData.GetHPPercent() >= 0.4f);

    protected override void OnAIDeInit()
    {
    }

    protected override void OnInit()
    {
        switch (this.mWeight.GetRandom())
        {
            case 0:
                base.AddAction(base.GetActionAttack(string.Empty, 0x1404, true));
                base.AddActionWait(1f);
                break;

            case 1:
                for (int i = 0; i < this.movecount; i++)
                {
                    base.AddAction(this.GetActionMoveOne(200, 200));
                }
                base.AddActionWait(1f);
                if (MathDxx.RandomBool())
                {
                    base.AddActionWait(0.5f);
                    for (int j = 0; j < this.movecount; j++)
                    {
                        base.AddAction(this.GetActionMoveOne(200, 200));
                    }
                    base.AddActionWait(1f);
                }
                break;

            case 2:
                for (int i = 0; i < 3; i++)
                {
                    base.AddAction(base.GetActionAttackWait(0x1407, 100, 100));
                }
                base.AddActionWait(0.5f);
                break;
        }
        base.bReRandom = true;
    }

    protected override void OnInitOnce()
    {
        this.mWeight.Add(0, 10);
        this.mWeight.Add(1, 10);
        this.mWeight.Add(2, 10);
    }
}

