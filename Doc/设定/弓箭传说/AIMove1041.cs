using Dxx.Util;
using System;
using UnityEngine;

public class AIMove1041 : AIMoveBase
{
    private string ContinuousName;
    private int count;
    private ActionBattle action;

    public AIMove1041(EntityBase entity) : base(entity)
    {
        this.count = 30;
        this.action = new ActionBattle();
    }

    private void Attack()
    {
        for (int i = 0; i < 8; i++)
        {
            GameLogic.Release.Bullet.CreateBullet(base.m_Entity, 0x13b6, base.m_Entity.position + new Vector3(0f, 1f, 0f), i * 45f);
        }
    }

    protected override void OnEnd()
    {
        this.action.DeInit();
        base.m_Entity.m_AniCtrl.SetString("Continuous", this.ContinuousName);
        base.m_Entity.m_AniCtrl.SendEvent("Idle", true);
    }

    protected override void OnInitBase()
    {
        this.action.Init(base.m_Entity);
        this.ContinuousName = base.m_Entity.m_AniCtrl.GetString("Continuous");
        base.m_Entity.m_AniCtrl.SetString("Continuous", "Recharging");
        base.m_Entity.m_AniCtrl.SendEvent("Continuous", false);
        for (int i = 0; i < this.count; i++)
        {
            this.action.AddActionDelegate(delegate {
                float num = Utils.getAngle(base.m_Entity.m_HatredTarget.position - base.m_Entity.position);
                GameLogic.Release.Bullet.CreateBullet(base.m_Entity, 0x13b6, base.m_Entity.position + new Vector3(0f, 2f, 0f), GameLogic.Random((float) (num - 90f), (float) (num + 90f)));
            });
            if (i < (this.count - 1))
            {
                this.action.AddActionWait(0.1f);
            }
        }
        this.action.AddActionDelegate(new Action(this.End));
    }

    protected override void OnUpdate()
    {
        base.m_Entity.m_AttackCtrl.RotateHero(base.m_Entity.m_HatredTarget);
    }
}

