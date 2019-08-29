using Dxx.Util;
using System;
using UnityEngine;

public class SkillAlone1032 : SkillAloneBase
{
    private void DeadAction(EntityBase entity)
    {
        if (entity != null)
        {
            object[] args = new object[] { "Game/SkillPrefab/", base.ClassName };
            GameObject o = GameLogic.EffectGet(Utils.GetString(args));
            o.transform.position = entity.position;
            SkillAloneAttrGoodBase.Add(base.m_Entity, o, true, Array.Empty<float>());
        }
    }

    protected override void OnInstall()
    {
        base.m_Entity.OnMonsterDeadAction = (Action<EntityBase>) Delegate.Combine(base.m_Entity.OnMonsterDeadAction, new Action<EntityBase>(this.DeadAction));
    }

    protected override void OnUninstall()
    {
        base.m_Entity.OnMonsterDeadAction = (Action<EntityBase>) Delegate.Remove(base.m_Entity.OnMonsterDeadAction, new Action<EntityBase>(this.DeadAction));
    }
}

