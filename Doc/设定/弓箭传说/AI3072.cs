using System;

public class AI3072 : AIBase
{
    private float angle;

    private bool Conditions() => 
        (base.GetIsAlive() && (base.m_Entity.m_HatredTarget != null));

    protected override void OnAIDeInit()
    {
    }

    protected override void OnInit()
    {
        AIBase.ActionSequence action = new AIBase.ActionSequence {
            name = "actionseq",
            ConditionBase = new Func<bool>(this.Conditions)
        };
        if (base.m_Entity.IsElite)
        {
            action.AddAction(base.GetActionWaitRandom("actionwaitr1", 200, 400));
            action.AddAction(base.GetActionWaitRandom("actionwaitr1", 100, 300));
            action.AddAction(base.GetActionDelegate(delegate {
                base.m_Entity.m_AttackCtrl.RotateHero(this.angle);
                this.angle += 30f;
                this.angle = this.angle % 360f;
            }));
            action.AddAction(base.GetActionAttack("actionattack", base.m_Entity.m_Data.WeaponID, false));
            action.AddAction(base.GetActionWaitRandom("actionwaitr2", 100, 200));
        }
        else
        {
            action.AddAction(base.GetActionWaitRandom("actionwaitr1", 300, 600));
            action.AddAction(base.GetActionWaitRandom("actionwaitr1", 300, 600));
            action.AddAction(base.GetActionAttack("actionattack", base.m_Entity.m_Data.WeaponID, true));
            action.AddAction(base.GetActionWaitRandom("actionwaitr2", 100, 500));
        }
        base.AddAction(action);
    }
}

