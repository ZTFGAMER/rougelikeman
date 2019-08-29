using System;

public class AI3084 : AIBase
{
    protected override void OnInit()
    {
        base.AddAction(base.GetActionWaitRandom(string.Empty, 800, 0x640));
        base.AddAction(new AIMove1035(base.m_Entity, 4, 7f, 0.2f));
        base.AddAction(base.GetActionWaitRandom(string.Empty, 600, 0x3e8));
    }
}

