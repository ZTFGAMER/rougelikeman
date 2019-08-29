using Dxx.Util;
using System;
using UnityEngine;

public class AIMove1051 : AIMoveBase
{
    private EntityBase target;
    private float redshowtime;
    private float delaytime;
    private float updatetime;
    private RedLineCtrl mLineCtrl;
    private GameObject effect;
    private bool bCreateBullet;

    public AIMove1051(EntityBase entity, float redshowtime, float delaytime) : base(entity)
    {
        base.name = "AIMove1051";
        this.redshowtime = redshowtime;
        this.delaytime = delaytime;
    }

    private void EffectCache()
    {
        if (this.effect != null)
        {
            GameLogic.EffectCache(this.effect);
            this.effect = null;
        }
    }

    private void LineCache()
    {
        if (this.mLineCtrl != null)
        {
            this.mLineCtrl.DeInit();
            this.mLineCtrl = null;
        }
    }

    protected override void OnEnd()
    {
        this.EffectCache();
        this.LineCache();
    }

    protected override void OnInitBase()
    {
        this.bCreateBullet = false;
        this.target = GameLogic.FindTarget(base.m_Entity);
        this.updatetime = 0f;
        object[] args = new object[] { base.m_Entity.m_Data.WeaponID };
        this.effect = GameLogic.EffectGet(Utils.FormatString("Game/WeaponHand/WeaponHand{0}Effect", args));
        this.effect.SetParentNormal(base.m_Entity.m_Body.LeftWeapon);
    }

    protected override void OnUpdate()
    {
        if ((this.target != null) && (base.m_Entity != null))
        {
            this.updatetime += Updater.delta;
            if (this.updatetime < this.redshowtime)
            {
                base.m_Entity.m_AttackCtrl.RotateHero(Utils.getAngle(this.target.position - base.m_Entity.position));
                if (this.updatetime > 0.2f)
                {
                    if (this.mLineCtrl == null)
                    {
                        this.mLineCtrl = new RedLineCtrl();
                        this.mLineCtrl.Init(base.m_Entity, true, 0, 0f);
                    }
                    if (this.mLineCtrl != null)
                    {
                        this.mLineCtrl.Update();
                    }
                }
            }
            else if (this.updatetime > (this.redshowtime + this.delaytime))
            {
                if (!this.bCreateBullet)
                {
                    this.bCreateBullet = true;
                    this.EffectCache();
                    this.LineCache();
                    GameLogic.Release.Bullet.CreateBullet(base.m_Entity, 0x432, base.m_Entity.m_Body.LeftBullet.transform.position, base.m_Entity.eulerAngles.y);
                }
                if (this.updatetime > ((this.redshowtime + this.delaytime) + 0.4f))
                {
                    base.End();
                }
            }
        }
    }
}

