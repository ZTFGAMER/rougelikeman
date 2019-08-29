using System;
using TableTool;
using UnityEngine;

public class AIMove1038 : AIMoveBase
{
    private Weapon_weapon weapondata;
    private int attackcount;
    private ActionBattle action;

    public AIMove1038(EntityBase entity) : base(entity)
    {
        this.attackcount = 8;
        this.action = new ActionBattle();
        this.weapondata = LocalModelManager.Instance.Weapon_weapon.GetBeanById(0x13af);
    }

    private void CreateFire()
    {
        this.action.AddActionWait(0.3f);
        for (int i = 0; i < this.attackcount; i++)
        {
            this.action.AddActionDelegate(delegate {
                Vector3 vector = GameLogic.Release.MapCreatorCtrl.RandomPosition();
                GameLogic.Release.Bullet.CreateBullet(base.m_Entity, 0x13af, vector + new Vector3(0f, 40f, 0f), 0f);
            });
            if (i < (this.attackcount - 1))
            {
                this.action.AddActionWait(0.3f);
            }
        }
        this.action.AddActionDelegate(new Action(this.End));
    }

    protected override void OnEnd()
    {
        this.action.DeInit();
    }

    protected override void OnInitBase()
    {
        this.action.Init(base.m_Entity);
        base.m_Entity.m_AniCtrl.SendEvent("Skill", false);
        this.CreateFire();
    }
}

