using System;

public class SkillAlone1057 : SkillAloneBase
{
    private void OnHitted(EntityBase entity, long hp)
    {
        if ((entity != null) && !entity.GetIsDead())
        {
            GameLogic.SendBuff(entity, base.m_Entity, 0x3fe, Array.Empty<float>());
        }
    }

    protected override void OnInstall()
    {
        base.m_Entity.OnHitted = (Action<EntityBase, long>) Delegate.Combine(base.m_Entity.OnHitted, new Action<EntityBase, long>(this.OnHitted));
    }

    protected override void OnUninstall()
    {
        base.m_Entity.OnHitted = (Action<EntityBase, long>) Delegate.Remove(base.m_Entity.OnHitted, new Action<EntityBase, long>(this.OnHitted));
    }
}

