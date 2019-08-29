using System;
using UnityEngine;

public class SkillAlone1015 : SkillAloneBase
{
    private GameObject obj;

    protected override void OnInstall()
    {
        this.obj = GameLogic.EffectGet("Game/SkillPrefab/SkillAlone1015");
        this.obj.transform.SetParent(base.m_Entity.transform);
        this.obj.transform.localPosition = Vector3.zero;
        EntityParentBase[] componentsInChildren = this.obj.GetComponentsInChildren<EntityParentBase>(true);
        int index = 0;
        int length = componentsInChildren.Length;
        while (index < length)
        {
            componentsInChildren[index].SetEntityParent(base.m_Entity);
            index++;
        }
        base.m_Entity.AddNewRotateShield(this.obj);
    }

    protected override void OnUninstall()
    {
        base.m_Entity.RemoveRotateShield(this.obj);
    }
}

