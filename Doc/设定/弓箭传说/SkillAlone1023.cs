using Dxx.Util;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SkillAlone1023 : SkillAloneBase
{
    private float hitratio;
    private ActionBasic action = new ActionBasic();

    private void DeadAction(EntityBase entity)
    {
        <DeadAction>c__AnonStorey0 storey = new <DeadAction>c__AnonStorey0 {
            entity = entity,
            $this = this
        };
        if (storey.entity != null)
        {
            this.action.AddActionWaitDelegate(0.1f, new Action(storey.<>m__0));
        }
    }

    protected override void OnInstall()
    {
        base.m_Entity.OnMonsterDeadAction = (Action<EntityBase>) Delegate.Combine(base.m_Entity.OnMonsterDeadAction, new Action<EntityBase>(this.DeadAction));
        this.hitratio = float.Parse(base.m_SkillData.Args[0]);
        this.action.Init(false);
    }

    protected override void OnUninstall()
    {
        base.m_Entity.OnMonsterDeadAction = (Action<EntityBase>) Delegate.Remove(base.m_Entity.OnMonsterDeadAction, new Action<EntityBase>(this.DeadAction));
        this.action.DeInit();
    }

    [CompilerGenerated]
    private sealed class <DeadAction>c__AnonStorey0
    {
        internal EntityBase entity;
        internal SkillAlone1023 $this;

        internal void <>m__0()
        {
            object[] args = new object[] { "Game/SkillPrefab/", this.$this.ClassName };
            GameObject o = GameLogic.EffectGet(Utils.GetString(args));
            o.transform.position = this.entity.position;
            float[] singleArray1 = new float[] { this.$this.hitratio };
            SkillAloneAttrGoodBase.Add(this.$this.m_Entity, o, true, singleArray1);
        }
    }
}

