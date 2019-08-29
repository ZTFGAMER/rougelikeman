using Dxx.Util;
using System;

public class SkillAlone1072 : SkillAloneBase
{
    private float hitratio;

    protected override void OnInstall()
    {
        this.hitratio = float.Parse(base.m_SkillData.Args[0]);
        base.m_Entity.m_EntityData.Modify_Light45(1);
        base.m_Entity.OnLight45 = (Action<EntityBase>) Delegate.Combine(base.m_Entity.OnLight45, new Action<EntityBase>(this.OnLight45));
    }

    private void OnLight45(EntityBase target)
    {
        if (target != null)
        {
            BulletBase base2 = GameLogic.Release.Bullet.CreateBullet(base.m_Entity, 0xbc5, target.m_Body.EffectMask.transform.position, 0f);
            base2.AddCantHit(target);
            base2.mBulletTransmit.SetAttack((long) MathDxx.CeilToInt(this.hitratio * base.m_Entity.m_EntityData.GetAttackBase()));
        }
    }

    protected override void OnUninstall()
    {
        base.m_Entity.m_EntityData.Modify_Light45(-1);
        base.m_Entity.OnLight45 = (Action<EntityBase>) Delegate.Remove(base.m_Entity.OnLight45, new Action<EntityBase>(this.OnLight45));
    }
}

