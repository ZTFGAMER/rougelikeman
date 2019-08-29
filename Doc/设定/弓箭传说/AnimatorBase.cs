using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class AnimatorBase
{
    public const int AttackPrev = 20;
    public const int AttackNext = 0x15;
    public const int Dead = 100;
    public const int Hitted = 200;
    public EntityBase m_Entity;
    private Animation m_AnimationBase;

    public void ClearString(string name)
    {
        if (this.m_Entity.mAniCtrlBase != null)
        {
            this.m_Entity.mAniCtrlBase.SetAnimationClear(name);
        }
    }

    public void DeadDown()
    {
        this.m_Entity.m_Body.DeadDown();
    }

    public void DeInit()
    {
        if (this.m_Entity.mAniCtrlBase != null)
        {
            this.m_Entity.mAniCtrlBase.DeInit();
        }
    }

    public float GetAnimationTime(string eventName) => 
        this.m_Entity.mAniCtrlBase?.GetAnimationTime(eventName);

    public string GetAnimationValue(string eventName) => 
        this.m_Entity.mAniCtrlBase?.GetAnimationValue(eventName);

    public string GetString(string name) => 
        this.m_Entity.mAniCtrlBase?.GetAnimationValue(name);

    public virtual void Init(EntityBase entity)
    {
        this.m_Entity = entity;
        if (this.m_Entity.mAniCtrlBase != null)
        {
            this.m_Entity.mAniCtrlBase.DeInit();
            this.m_Entity.mAniCtrlBase = null;
        }
        EntityType type = entity.Type;
        if (type == EntityType.Hero)
        {
            this.m_Entity.mAniCtrlBase = new AnimationCtrlHero();
        }
        else if (type == EntityType.Soldier)
        {
            this.m_Entity.mAniCtrlBase = new AnimationCtrlMonster();
        }
        else if (type == EntityType.Boss)
        {
            this.m_Entity.mAniCtrlBase = new AnimationCtrlBoss();
        }
        else
        {
            this.m_Entity.mAniCtrlBase = new AnimationCtrlHero();
        }
        if (this.m_Entity.m_Body != null)
        {
            this.m_AnimationBase = this.m_Entity.m_Body.AnimatorBodyObj.GetComponent<Animation>();
        }
        this.m_Entity.mAniCtrlBase.OnStart();
        this.m_Entity.mAniCtrlBase.SetAnimatorBase(this);
        this.m_Entity.mAniCtrlBase.SetAnimation(this.m_AnimationBase);
        this.m_Entity.mAniCtrlBase.SetHittedCallBack(new Action(this.m_Entity.m_HitEdit.HittedAnimationCallBack));
        this.m_Entity.mAniCtrlBase.SetHeroPlayMakerColtrol(this.m_Entity.m_Body.mHeroPlayMakerCtrl);
        this.m_AnimationBase.enabled = true;
        this.SendEvent("Idle", true);
    }

    public void Reborn()
    {
        if (this.m_Entity.mAniCtrlBase != null)
        {
            this.m_Entity.mAniCtrlBase.Reborn();
        }
    }

    public void SendEvent(string eventName, bool force = false)
    {
        if (this.m_Entity.mAniCtrlBase != null)
        {
            this.m_Entity.mAniCtrlBase.SendEvent(eventName, force);
        }
    }

    public void SetBool(string name, bool value)
    {
        if (this.m_Entity.mAniCtrlBase != null)
        {
            this.m_Entity.mAniCtrlBase.SetBool(name, value);
        }
    }

    public void SetString(string name, string value = "")
    {
        if (this.m_Entity.mAniCtrlBase != null)
        {
            this.m_Entity.mAniCtrlBase.SetAnimationValue(name, value);
        }
    }

    public void SetTouchMoveJoy(bool value)
    {
        this.SetBool("TouchMoveJoy", value);
    }
}

