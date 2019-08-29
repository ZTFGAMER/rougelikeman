using System;

public class AnimationCtrlHero : AnimationCtrlBase
{
    protected override void AttackInterrupt()
    {
        base.AttackInterrupt();
        this.OnAttackEnd();
    }

    protected override void Event_AttackEndI(AnimationCtrlBase.AniClass a)
    {
        base.UpdateAnimationSpeed(a.name);
        float num = 1f;
        if (base.ani != null)
        {
            if (!a.revert)
            {
                base.ani[a.value].time = 0f;
            }
            else
            {
                base.ani[a.value].time = base.ani[a.value].clip.length;
            }
            base.ani.Play(a.value);
            num = base.ani[a.value].length / a.Speed;
        }
        if ((base.m_Entity.m_Weapon != null) && (base.m_Entity.m_Weapon.OnAttackEndStartAction != null))
        {
            base.m_Entity.m_Weapon.OnAttackEndStartAction();
        }
        ActionBasic.ActionWait action = new ActionBasic.ActionWait {
            waitTime = num
        };
        base.mActionList[a.name].AddAction(action);
        ActionBasic.ActionDelegate delegate2 = new ActionBasic.ActionDelegate {
            action = delegate {
                this.OnAttackEnd();
                base.UpdateTouch();
            }
        };
        base.mActionList[a.name].AddAction(delegate2);
    }

    protected override void Event_AttackPrevI(AnimationCtrlBase.AniClass a)
    {
        if ((base.m_Entity.m_Weapon != null) && (base.m_Entity.m_Weapon.OnAttackStartStartAction != null))
        {
            base.m_Entity.m_Weapon.OnAttackStartStartAction();
        }
        base.mActionList["AttackPrev"].ActionClear();
        base.UpdateAnimationSpeed(a.name);
        if (base.ani != null)
        {
            if (!a.revert)
            {
                base.ani[a.value].time = 0f;
            }
            else
            {
                base.ani[a.value].time = base.ani[a.value].clip.length;
            }
            base.ani.Play(a.value);
        }
        float num = 1f;
        if (base.ani != null)
        {
            num = base.ani[a.value].length / a.Speed;
        }
        ActionBasic.ActionWait action = new ActionBasic.ActionWait {
            waitTime = num
        };
        base.mActionList[a.name].AddAction(action);
        ActionBasic.ActionDelegate delegate2 = new ActionBasic.ActionDelegate {
            action = delegate {
                base.m_Entity.PlayAttack();
                if (((base.m_Entity != null) && (base.m_Entity.m_Weapon != null)) && (base.m_Entity.m_Weapon.OnAttackStartEndAction != null))
                {
                    base.m_Entity.m_Weapon.OnAttackStartEndAction();
                }
                if ((base.m_Entity != null) && (base.m_Entity.Event_OnAttack != null))
                {
                    base.m_Entity.Event_OnAttack();
                }
            }
        };
        base.mActionList[a.name].AddAction(delegate2);
    }

    protected override void Event_CallI(AnimationCtrlBase.AniClass a)
    {
        base.UpdateAnimationSpeed(a.name);
        float num = 1f;
        if (base.ani != null)
        {
            base.ani.Play(a.value);
            num = base.ani[a.value].length / a.Speed;
        }
        ActionBasic.ActionWait action = new ActionBasic.ActionWait {
            waitTime = num
        };
        base.mActionList[a.name].AddAction(action);
        ActionBasic.ActionDelegate delegate2 = new ActionBasic.ActionDelegate {
            action = () => base.UpdateTouch()
        };
        base.mActionList[a.name].AddAction(delegate2);
    }

    protected override void Event_ContinuousI(AnimationCtrlBase.AniClass a)
    {
        base.UpdateAnimationSpeed(a.name);
        if (base.ani != null)
        {
            if (!a.revert)
            {
                base.ani[a.value].time = 0f;
            }
            else
            {
                base.ani[a.value].time = base.ani[a.value].clip.length;
            }
            base.ani.Play(a.value);
        }
    }

    protected override void Event_DeadI(AnimationCtrlBase.AniClass a)
    {
        base.UpdateAnimationSpeed(a.name);
        if (base.ani != null)
        {
            base.ani.Play(a.value);
        }
    }

    protected override void Event_DizzyI(AnimationCtrlBase.AniClass a)
    {
        base.UpdateAnimationSpeed(a.name);
        if (base.ani != null)
        {
            base.ani.Play(a.value);
        }
    }

    protected override void Event_HittedZI(AnimationCtrlBase.AniClass a)
    {
        if (a.value != string.Empty)
        {
            base.PlayHittedAction(true);
            base.UpdateAnimationSpeed(a.name);
            float num = 1f;
            if (base.ani != null)
            {
                base.ani.Play(a.value);
                num = base.ani[a.value].length / a.Speed;
            }
            ActionBasic.ActionWait action = new ActionBasic.ActionWait {
                waitTime = num
            };
            base.mActionList[a.name].AddAction(action);
            ActionBasic.ActionDelegate delegate2 = new ActionBasic.ActionDelegate {
                action = delegate {
                    base.PlayHittedAction(false);
                    if (base.IsCurrentState("Hitted"))
                    {
                        base.UpdateTouch();
                    }
                }
            };
            base.mActionList[a.name].AddAction(delegate2);
        }
    }

    protected override void Event_IdleI(AnimationCtrlBase.AniClass a)
    {
        base.UpdateAnimationSpeed(a.name);
        if (base.ani != null)
        {
            base.ani.CrossFade(a.value, 0.2f);
        }
    }

    protected override void Event_RunI(AnimationCtrlBase.AniClass a)
    {
        if ((base.PrevState.name == "AttackEnd") || (base.PrevState.name == "AttackPrev"))
        {
            base.mAttackInterrupt = true;
            if (((base.m_Entity != null) && (base.m_Entity.m_Weapon != null)) && (base.m_Entity.m_Weapon.OnAttackInterruptAction != null))
            {
                base.m_Entity.m_Weapon.OnAttackInterruptAction();
            }
            base.mActionList["AttackPrev"].ActionClear();
            base.mActionList["AttackEnd"].ActionClear();
            this.AttackInterrupt();
        }
        base.UpdateAnimationSpeed(a.name);
        if (base.ani != null)
        {
            base.ani.CrossFade(a.value, 0.2f);
        }
    }

    protected override void Event_SkillEndI(AnimationCtrlBase.AniClass a)
    {
        base.UpdateAnimationSpeed(a.name);
        float num = 1f;
        if (base.ani != null)
        {
            base.ani.Play(a.value);
            num = base.ani[a.value].length / a.Speed;
        }
        ActionBasic.ActionWait action = new ActionBasic.ActionWait {
            waitTime = num
        };
        base.mActionList[a.name].AddAction(action);
        ActionBasic.ActionDelegate delegate2 = new ActionBasic.ActionDelegate {
            action = () => base.UpdateTouch()
        };
        base.mActionList[a.name].AddAction(delegate2);
    }

    protected override void Event_SkillI(AnimationCtrlBase.AniClass a)
    {
        if (base.ani[a.value] != null)
        {
            base.UpdateAnimationSpeed(a.name);
            float num = 1f;
            if (base.ani != null)
            {
                if (!a.revert)
                {
                    base.ani[a.value].time = 0f;
                }
                else
                {
                    base.ani[a.value].time = base.ani[a.value].clip.length;
                }
                base.ani.Play(a.value);
                num = base.ani[a.value].length / a.Speed;
            }
            ActionBasic.ActionWait action = new ActionBasic.ActionWait {
                waitTime = num
            };
            base.mActionList[a.name].AddAction(action);
            ActionBasic.ActionDelegate delegate2 = new ActionBasic.ActionDelegate {
                action = delegate {
                    if (base.mAniStringList["SkillEnd"].value != "SkillEnd")
                    {
                        if (base.m_Entity.OnSkillActionEnd != null)
                        {
                            base.m_Entity.OnSkillActionEnd();
                        }
                        base.m_Entity.m_AniCtrl.SendEvent("SkillEnd", true);
                    }
                    else
                    {
                        base.UpdateTouch();
                        if (base.m_Entity.OnSkillActionEnd != null)
                        {
                            base.m_Entity.OnSkillActionEnd();
                        }
                    }
                }
            };
            base.mActionList[a.name].AddAction(delegate2);
        }
    }

    private void OnAttackEnd()
    {
        if (((base.m_Entity != null) && (base.m_Entity.m_Weapon != null)) && (base.m_Entity.m_Weapon.OnAttackEndEndAction != null))
        {
            base.m_Entity.m_Weapon.OnAttackEndEndAction();
        }
    }
}

