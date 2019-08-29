using System;
using UnityEngine;

public class SkillAlone1034 : SkillAlonePartBase
{
    protected override void OnDeadAction(EntityBase deadentity, EntityPartBodyBase partbody)
    {
        Vector3 pos = base.m_Entity.position + new Vector3(GameLogic.Random((float) -2f, (float) 2f), 0f, GameLogic.Random((float) -2f, (float) 2f));
        if (partbody != null)
        {
            partbody.SetPosition(pos);
            base.m_Entity.AddRotateFollow(partbody);
        }
    }

    protected override void OnInstall()
    {
        base.OnInstall();
    }

    protected override void OnUninstall()
    {
        base.OnUninstall();
    }
}

