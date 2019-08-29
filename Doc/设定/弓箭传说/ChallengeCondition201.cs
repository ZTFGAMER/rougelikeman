using System;

public class ChallengeCondition201 : ChallengeConditionBase
{
    protected override void OnDeInit()
    {
        if (GameLogic.Self != null)
        {
            EntityHero self = GameLogic.Self;
            self.OnHitted = (Action<EntityBase, long>) Delegate.Remove(self.OnHitted, new Action<EntityBase, long>(this.OnHitted));
        }
    }

    private void OnHitted(EntityBase entity, long hit)
    {
        base.OnFailure();
    }

    protected override void OnInit()
    {
    }

    protected override void OnStart()
    {
        if (GameLogic.Self != null)
        {
            EntityHero self = GameLogic.Self;
            self.OnHitted = (Action<EntityBase, long>) Delegate.Combine(self.OnHitted, new Action<EntityBase, long>(this.OnHitted));
        }
    }
}

