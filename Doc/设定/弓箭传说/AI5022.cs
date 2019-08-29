using System;

public class AI5022 : AIBase
{
    private void AddAttack5052()
    {
        base.AddAction(base.GetActionDelegate(() => base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", -0.6f)));
        base.AddAction(base.GetActionAttack(string.Empty, 0x13bc, true));
        base.AddAction(base.GetActionDelegate(() => base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", 0.6f)));
    }

    protected override void OnAIDeInit()
    {
    }

    protected override void OnInit()
    {
        int num = GameLogic.Random(0, 3);
        for (int i = 0; i < num; i++)
        {
            base.AddAction(new AIMove1002(base.m_Entity, GameLogic.Random(500, 0x3e8), -1));
        }
        switch (GameLogic.Random(0, 2))
        {
            case 0:
                base.AddAction(base.GetActionAttack(string.Empty, 0x13ba, true));
                break;

            case 1:
                this.AddAttack5052();
                break;
        }
        base.bReRandom = true;
    }
}

