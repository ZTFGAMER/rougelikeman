using Dxx.Util;
using System;
using UnityEngine;

public class SkillAlone1073 : SkillAloneBase
{
    private GameObject obj;

    protected override void OnInstall()
    {
        string str = base.m_SkillData.Args[0];
        float num = float.Parse(base.m_SkillData.Args[1]);
        int num2 = int.Parse(base.m_SkillData.Args[2]);
        object[] args = new object[] { "Game/SkillPrefab/SkillAlone", str };
        this.obj = GameLogic.EffectGet(Utils.GetString(args));
        float[] singleArray1 = new float[] { num, (float) num2 };
        SkillAloneAttrGoodBase.Add(base.m_Entity, this.obj, false, singleArray1);
        base.m_Entity.AddNewRotateSword(this.obj);
    }

    protected override void OnUninstall()
    {
        SkillAloneAttrGoodBase.Remove(this.obj);
        base.m_Entity.RemoveRotateSword(this.obj);
    }
}

