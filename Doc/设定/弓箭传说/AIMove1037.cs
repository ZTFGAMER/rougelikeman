using System;
using TableTool;
using UnityEngine;

public class AIMove1037 : AIMoveBase
{
    private string skillname;
    private Weapon_weapon weapondata;

    public AIMove1037(EntityBase entity) : base(entity)
    {
        this.weapondata = LocalModelManager.Instance.Weapon_weapon.GetBeanById(0x13ad);
    }

    private void CreateFire()
    {
        for (int i = 0; i < 3; i++)
        {
            Vector3 pos = GameLogic.Release.MapCreatorCtrl.RandomPosition();
            float rota = (GameLogic.Random(0, 2) != 0) ? ((float) 90) : ((float) 0);
            GameLogic.Release.Bullet.CreateBullet(base.m_Entity, 0x13ad, pos, rota);
        }
    }

    protected override void OnEnd()
    {
        base.m_Entity.OnSkillActionEnd = (Action) Delegate.Remove(base.m_Entity.OnSkillActionEnd, new Action(this.OnSkillEnd));
        base.m_Entity.OnSkillActionEnd = (Action) Delegate.Remove(base.m_Entity.OnSkillActionEnd, new Action(this.OnSkillAfterEnd));
    }

    protected override void OnInitBase()
    {
        this.skillname = base.m_Entity.m_AniCtrl.GetString("Skill");
        base.m_Entity.m_AniCtrl.SetString("Skill", "FireAttackPrev1");
        base.m_Entity.m_AniCtrl.SendEvent("Skill", false);
        base.m_Entity.OnSkillActionEnd = (Action) Delegate.Combine(base.m_Entity.OnSkillActionEnd, new Action(this.OnSkillEnd));
    }

    private void OnSkillAfterEnd()
    {
        base.m_Entity.OnSkillActionEnd = (Action) Delegate.Remove(base.m_Entity.OnSkillActionEnd, new Action(this.OnSkillAfterEnd));
        base.m_Entity.m_AniCtrl.SetString("Skill", this.skillname);
        base.End();
    }

    private void OnSkillEnd()
    {
        base.m_Entity.OnSkillActionEnd = (Action) Delegate.Remove(base.m_Entity.OnSkillActionEnd, new Action(this.OnSkillEnd));
        base.m_Entity.OnSkillActionEnd = (Action) Delegate.Combine(base.m_Entity.OnSkillActionEnd, new Action(this.OnSkillAfterEnd));
        base.m_Entity.m_AniCtrl.SetString("Skill", "FireAttackEnd1");
        base.m_Entity.m_AniCtrl.SendEvent("Skill", false);
        this.CreateFire();
    }
}

