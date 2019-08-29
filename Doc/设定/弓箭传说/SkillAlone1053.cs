using System;
using UnityEngine;

public class SkillAlone1053 : SkillAloneBase
{
    private float time;

    private void OnHitted(EntityBase entity, long value)
    {
        GameObject obj2 = GameLogic.EffectGet("Game/SkillPrefab/SkillAlone1053");
        obj2.transform.SetParent(base.m_Entity.m_Body.EffectMask.transform);
        obj2.transform.localPosition = Vector3.zero;
        obj2.transform.localScale = Vector3.one;
        obj2.transform.localRotation = Quaternion.identity;
        obj2.GetComponent<AutoDespawn>().SetDespawnTime(base.m_Entity.m_EntityData.HittedInterval);
    }

    protected override void OnInstall()
    {
        this.time = float.Parse(base.m_SkillData.Args[0]);
        base.m_Entity.OnHitted = (Action<EntityBase, long>) Delegate.Combine(base.m_Entity.OnHitted, new Action<EntityBase, long>(this.OnHitted));
        base.m_Entity.m_EntityData.Modify_HittedInterval(this.time);
    }

    protected override void OnUninstall()
    {
        base.m_Entity.OnHitted = (Action<EntityBase, long>) Delegate.Remove(base.m_Entity.OnHitted, new Action<EntityBase, long>(this.OnHitted));
        base.m_Entity.m_EntityData.Modify_HittedInterval(-this.time);
    }
}

