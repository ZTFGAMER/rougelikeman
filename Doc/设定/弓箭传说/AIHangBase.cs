using System;

public class AIHangBase : AIBase
{
    protected override void OnAIDeInit()
    {
    }

    protected override void OnInit()
    {
        base.mRoomTime = -1f;
        base.AddAction(new AIMove1001(base.m_Entity));
    }
}

