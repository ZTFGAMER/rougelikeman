using System;

public class AI3089 : AIBase
{
    public const int DivideCount = 2;

    protected override void OnAIDeInit()
    {
    }

    protected override void OnDeadBefore()
    {
        if (base.m_Entity.IsElite)
        {
            base.Divide(0xc1f, 3);
        }
        else
        {
            base.Divide(0xc1f, 2);
        }
    }

    protected override void OnInit()
    {
        base.AddAction(new AIMove1007(base.m_Entity));
    }

    protected override void OnInitOnce()
    {
        base.OnInitOnce();
        if (base.m_Entity.IsElite)
        {
            base.m_Entity.m_EntityData.ExcuteAttributes("MoveSpeed%", 0x1b58L);
        }
    }
}

