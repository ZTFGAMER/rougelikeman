using System;
using UnityEngine;

public class SkillAlone3003 : SkillAloneBase
{
    private long percent;
    private bool bUse;
    private GameObject obj;

    private void AddDefence()
    {
        if (!this.bUse)
        {
            this.bUse = true;
            base.m_Entity.m_EntityData.ExcuteAttributes("DamageResist%", this.percent);
            this.obj = base.m_Entity.PlayEffect(0x2f4d6d);
        }
    }

    protected override void OnInstall()
    {
        base.m_Entity.OnMoveEvent = (Action<bool>) Delegate.Combine(base.m_Entity.OnMoveEvent, new Action<bool>(this.OnMoveEvent));
        this.percent = long.Parse(base.m_SkillData.Args[0]);
    }

    private void OnMoveEvent(bool move)
    {
        if (move)
        {
            this.RemoveDefence();
        }
        else
        {
            this.AddDefence();
        }
    }

    protected override void OnUninstall()
    {
        base.m_Entity.OnMoveEvent = (Action<bool>) Delegate.Remove(base.m_Entity.OnMoveEvent, new Action<bool>(this.OnMoveEvent));
        this.RemoveDefence();
    }

    private void RemoveDefence()
    {
        if (this.bUse)
        {
            this.bUse = false;
            base.m_Entity.m_EntityData.ExcuteAttributes("DamageResist%", -this.percent);
            GameLogic.EffectCache(this.obj);
            this.obj = null;
        }
    }
}

