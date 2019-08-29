using System;
using System.Runtime.CompilerServices;

public class AI5005 : AIBase
{
    [CompilerGenerated]
    private static Func<bool> <>f__am$cache0;
    [CompilerGenerated]
    private static Func<bool> <>f__am$cache1;
    [CompilerGenerated]
    private static Func<bool> <>f__am$cache2;

    private ActionBasic.ActionBase GetActionMoveOne(int waittime, int waitmaxtime)
    {
        AIBase.ActionSequence sequence = new AIBase.ActionSequence {
            name = "actionseq",
            m_Entity = base.m_Entity
        };
        sequence.AddAction(new AIMove1019(base.m_Entity));
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
        AIBase.ActionChooseRandom action = new AIBase.ActionChooseRandom {
            name = "actionchooser",
            m_Entity = base.m_Entity
        };
        action.ConditionBase = new Func<bool>(this.GetIsAlive);
        AIBase.ActionChooseIf @if = new AIBase.ActionChooseIf {
            m_Entity = base.m_Entity
        };
        ActionBasic.ActionBase actionMoves = this.GetActionMoves(1);
        actionMoves.ConditionBase = new Func<bool>(this.GetHPMore40);
        @if.AddAction(actionMoves);
        ActionBasic.ActionBase base3 = this.GetActionMoves(3);
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = () => GameLogic.Random(0, 100) < 0x22;
        }
        base3.ConditionBase = <>f__am$cache0;
        @if.AddAction(base3);
        ActionBasic.ActionBase base4 = this.GetActionMoves(4);
        if (<>f__am$cache1 == null)
        {
            <>f__am$cache1 = () => GameLogic.Random(0, 100) < 50;
        }
        base4.ConditionBase = <>f__am$cache1;
        @if.AddAction(base4);
        ActionBasic.ActionBase base5 = this.GetActionMoves(5);
        if (<>f__am$cache2 == null)
        {
            <>f__am$cache2 = () => true;
        }
        base5.ConditionBase = <>f__am$cache2;
        @if.AddAction(base5);
        action.AddAction(5, @if);
        action.AddAction(5, base.GetActionAttackWait(0x1393, 0xbb8, 0xfa0));
        base.AddAction(action);
    }
}

