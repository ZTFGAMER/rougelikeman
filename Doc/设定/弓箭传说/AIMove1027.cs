using System;
using System.Collections.Generic;
using UnityEngine;

public class AIMove1027 : AIMoveBase
{
    private int range;
    protected EntityBase target;
    protected List<Grid.NodeItem> findpath;
    protected Vector3 nextpos;
    protected Vector3 endpos;
    private ActionBattle action;
    private Animation ani;
    private bool bDizzy;

    public AIMove1027(EntityBase entity, int range) : base(entity)
    {
        this.action = new ActionBattle();
        if (range < 1)
        {
            range = 1;
        }
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
        if ((base.m_Entity != null) && (base.m_Entity.m_Body != null))
        {
            Transform transform = base.m_Entity.m_Body.AnimatorBodyObj.transform.Find("GroundBreak/scale/sprite");
            if (transform != null)
            {
                this.ani = transform.GetComponent<Animation>();
            }
        }
        this.target = GameLogic.Self;
        this.SetAnimation();
    }

    private void SetAnimation()
    {
        base.m_Entity.m_AniCtrl.SetString("Skill", string.Empty);
        base.m_Entity.mAniCtrlBase.SetAnimationRevert("Skill", true);
        base.m_Entity.m_AniCtrl.SendEvent("Skill", false);
        this.action.DeInit();
        base.m_Entity.ShowHP(false);
        this.action.Init(base.m_Entity);
        float animationTime = base.m_Entity.m_AniCtrl.GetAnimationTime("Skill");
        this.action.AddActionWaitDelegate(animationTime - 0.2f, delegate {
            if (this.ani != null)
            {
                this.ani.Play("3028_GroundBreak_Miss");
            }
        });
        this.action.AddActionWaitDelegate(0.1f, delegate {
            base.m_Entity.SetCollider(false);
            base.m_Entity.SetPosition(new Vector3(base.m_Entity.position.x, -5f, base.m_Entity.position.z));
        });
        this.action.AddActionWaitDelegate(1.5f, delegate {
            base.m_Entity.m_MoveCtrl.AIMoveEnd(base.m_MoveData);
            GameLogic.Release.MapCreatorCtrl.RandomItem(GameLogic.Self, this.range, out float num, out float num2);
            this.endpos = new Vector3(num, 0f, num2);
            base.m_Entity.SetPosition(new Vector3(this.endpos.x, 0f, this.endpos.z));
            if (this.ani != null)
            {
                this.ani.Play("3028_GroundBreak_Show");
            }
            base.m_Entity.mAniCtrlBase.SetAnimationRevert("Skill", false);
            base.m_Entity.m_AniCtrl.SetString("Skill", string.Empty);
            base.m_Entity.m_AniCtrl.SendEvent("Skill", false);
            base.m_Entity.ShowHP(true);
        });
        this.action.AddActionWaitDelegate(0.3f, () => base.m_Entity.SetCollider(true));
        this.action.AddActionWaitDelegate(0.2f, () => base.End());
    }
}

