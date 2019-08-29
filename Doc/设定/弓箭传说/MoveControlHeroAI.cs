using System;

public class MoveControlHeroAI : MoveControl
{
    protected override void OnMoveSpeedUpdate()
    {
        base.MoveDirection = this.m_JoyData.MoveDirection * base.m_Entity.m_EntityData.GetSpeed();
    }
}

