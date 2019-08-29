using System;
using System.Collections.Generic;
using UnityEngine;

public class AIMove1026 : AIMoveBase
{
    public Action onDown;
    public Action onUp;
    private AIGroundBase m_AIGroiund;
    private int range;
    protected EntityBase target;
    protected List<Grid.NodeItem> findpath;
    protected Vector3 nextpos;
    protected Vector3 endpos;
    private ActionBattle action;
    private bool bDizzy;
    private bool bMissHP;

    public AIMove1026(EntityBase entity, int range) : base(entity)
    {
        this.action = new ActionBattle();
        if (entity != null)
        {
            EntityMonsterBase base2 = entity as EntityMonsterBase;
            if (base2 != null)
            {
                AIBase aI = base2.GetAI();
                if (aI != null)
                {
                    this.m_AIGroiund = aI as AIGroundBase;
                }
            }
        }
        base.name = "1026move";
        this.range = range;
    }

    private void OnDizzy(bool dizzy)
    {
        this.bDizzy = dizzy;
        if (dizzy)
        {
            this.target = null;
            this.action.DeInit();
            base.m_Entity.SetPosition(new Vector3(base.m_Entity.position.x, 0f, base.m_Entity.position.z));
        }
        else
        {
            this.target = GameLogic.Self;
            this.SetAnimation();
        }
    }

    protected override void OnEnd()
    {
        base.m_Entity.SetPosition(new Vector3(base.m_Entity.position.x, 0f, base.m_Entity.position.z));
        base.m_Entity.SetCollider(true);
        this.action.DeInit();
    }

    protected override void OnInitBase()
    {
        if (this.m_AIGroiund == null)
        {
            base.End();
        }
        else
        {
            this.bMissHP = false;
            this.target = GameLogic.Self;
            GameLogic.Release.MapCreatorCtrl.RandomItemSide(base.m_Entity, this.range, out float num, out float num2);
            this.endpos = new Vector3(num, 0f, num2);
            this.SetAnimation();
        }
    }

    private void SetAnimation()
    {
        base.m_Entity.m_AniCtrl.SetString("Skill", string.Empty);
        base.m_Entity.mAniCtrlBase.SetAnimationRevert("Skill", true);
        base.m_Entity.m_AniCtrl.SendEvent("Skill", false);
        this.action.DeInit();
        if (!this.bMissHP)
        {
            base.m_Entity.ShowHP(false);
            this.bMissHP = true;
        }
        this.action.Init(base.m_Entity);
        float animationTime = base.m_Entity.m_AniCtrl.GetAnimationTime("Skill");
        this.action.AddActionWaitDelegate(animationTime - 0.2f, delegate {
            this.m_AIGroiund.GroundShow(false);
            if (this.onDown != null)
            {
                this.onDown();
            }
        });
        this.action.AddActionWaitDelegate(0.1f, delegate {
            base.m_Entity.SetCollider(false);
            base.m_Entity.SetPosition(new Vector3(base.m_Entity.position.x, -5f, base.m_Entity.position.z));
        });
        this.action.AddActionWaitDelegate(1.5f, delegate {
            base.m_Entity.m_MoveCtrl.AIMoveEnd(base.m_MoveData);
            base.m_Entity.SetPosition(new Vector3(this.endpos.x, 0f, this.endpos.z));
            this.m_AIGroiund.GroundShow(true);
            base.m_Entity.mAniCtrlBase.SetAnimationRevert("Skill", false);
            base.m_Entity.m_AniCtrl.SetString("Skill", string.Empty);
            base.m_Entity.m_AniCtrl.SendEvent("Skill", false);
            base.m_Entity.ShowHP(true);
            this.bMissHP = false;
            GameLogic.Hold.Sound.PlayMonsterSkill(0x4dd1e6, base.m_Entity.position);
        });
        this.action.AddActionWaitDelegate(0.3f, delegate {
            if (this.onUp != null)
            {
                this.onUp();
            }
        });
        this.action.AddActionWaitDelegate(0.3f, delegate {
            base.m_Entity.SetCollider(true);
            base.End();
        });
    }
}

