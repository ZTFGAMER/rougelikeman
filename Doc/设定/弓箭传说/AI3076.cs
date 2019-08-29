using System;

public class AI3076 : AIBase
{
    protected override void OnAIDeInit()
    {
    }

    protected override void OnDeadBefore()
    {
        base.OnDeadBefore();
        if (base.m_Entity.IsElite)
        {
            base.Divide(0xbbe, 2);
        }
    }

    protected override void OnInit()
    {
        base.AddAction(new AIMove1056(base.m_Entity, 0x3e8, 0f, 0.86f));
        base.AddAction(base.GetActionWaitRandom(string.Empty, 800, 800));
    }
}

