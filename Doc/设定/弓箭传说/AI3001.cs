using System;
using UnityEngine;

public class AI3001 : AIBase
{
    private bool GetCanMoveToHatred() => 
        ((base.m_Entity.m_HatredTarget != null) && (Vector3.Distance(base.m_Entity.m_HatredTarget.position, base.m_Entity.position) > 0.5f));

    protected override void OnAIDeInit()
    {
    }

    protected override void OnInit()
    {
        AIBase.ActionChoose action = new AIBase.ActionChoose {
            name = "actionchoose",
            m_Entity = base.m_Entity,
            Condition = new Func<bool>(this.GetCanMoveToHatred)
        };
        AIBase.ActionSequence sequence = new AIBase.ActionSequence {
            m_Entity = base.m_Entity,
            name = "actionseqtrue"
        };
        if (base.m_Entity.IsElite)
        {
            AIBase.ActionChooseRandom random = new AIBase.ActionChooseRandom {
                m_Entity = base.m_Entity
            };
            random.AddAction(10, new AIMove1009(base.m_Entity));
            random.AddAction(10, new AIMove1057(base.m_Entity));
            sequence.AddAction(random);
        }
        else
        {
            sequence.AddAction(new AIMove1009(base.m_Entity));
        }
        sequence.AddAction(new AIMove1006(base.m_Entity, 0x708, 0x9c4));
        action.ResultTrue = sequence;
        AIBase.ActionSequence sequence2 = new AIBase.ActionSequence {
            m_Entity = base.m_Entity,
            name = "actionseq"
        };
        sequence2.AddAction(base.GetActionWaitRandom("actionwaitrandom1", 300, 600));
        sequence2.AddAction(new AIMove1002(base.m_Entity, 800, 0x3e8));
        action.ResultFalse = sequence2;
        AIBase.ActionSequence sequence3 = new AIBase.ActionSequence {
            m_Entity = base.m_Entity,
            name = "actionseq1"
        };
        sequence3.AddAction(action);
        base.AddAction(sequence3);
    }
}

