using System;

public class AIHeroAttackControl : AttackControl
{
    private int attackState;

    private void AutoAttackUpdate()
    {
        if (((GameLogic.Release.Game.RoomState == RoomState.Runing) && !base.m_EntityHero.GetIsDead()) && !base.m_EntityHero.m_MoveCtrl.GetMoving())
        {
            if (this.attackState == 0)
            {
                if (base.m_EntityHero.m_HatredTarget != null)
                {
                    base.OnMoveStart(base.m_JoyData);
                    this.attackState = 1;
                }
            }
            else if ((this.attackState == 1) && base.RotateOver())
            {
                base.OnMoveEnd(base.m_JoyData);
                this.attackState = 2;
            }
            base.RotateUpdate(base.m_EntityHero.m_HatredTarget);
        }
    }

    protected override void OnDestroys()
    {
    }

    protected override void OnStart()
    {
    }

    public override void UpdateProgress()
    {
        base.UpdateProgress();
        this.AutoAttackUpdate();
    }
}

