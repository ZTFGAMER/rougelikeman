using System;
using UnityEngine;

public class SkillAloneShieldCountBase : SkillAloneBase
{
    private long shieldcount;
    private GameObject mShieldObj;

    protected override void OnInstall()
    {
        this.shieldcount = long.Parse(base.m_SkillData.Args[0]);
        base.m_Entity.m_EntityData.AddShieldCountAction(new Action<long>(this.OnShieldCount));
        base.m_Entity.m_EntityData.AddShieldCount(this.shieldcount);
    }

    private void OnShieldCount(long value)
    {
        if ((value > 0L) && (this.mShieldObj == null))
        {
            this.mShieldObj = GameLogic.EffectGet("Game/SkillPrefab/SkillAlone1038");
            this.mShieldObj.transform.SetParent(base.m_Entity.m_Body.EffectMask.transform);
            this.mShieldObj.transform.localPosition = Vector3.zero;
            this.mShieldObj.transform.localRotation = Quaternion.identity;
            this.mShieldObj.transform.localScale = Vector3.one;
        }
        else if ((value == 0L) && (this.mShieldObj != null))
        {
            GameLogic.EffectCache(this.mShieldObj);
            this.mShieldObj = null;
        }
    }

    protected override void OnUninstall()
    {
        base.m_Entity.m_EntityData.AddShieldCount(-this.shieldcount);
        base.m_Entity.m_EntityData.RemoveShieldCountAction(new Action<long>(this.OnShieldCount));
        this.OnShieldCount(0L);
    }

    protected void ResetShieldCount()
    {
        base.m_Entity.m_EntityData.ResetShieldCount();
    }
}

