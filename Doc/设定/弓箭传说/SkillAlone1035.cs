using System;
using UnityEngine;

public class SkillAlone1035 : SkillAlonePartBase
{
    protected override void OnDeadAction(EntityBase deadentity, EntityPartBodyBase partbody)
    {
        if ((partbody != null) && (deadentity != null))
        {
            Vector3 position = deadentity.position;
            partbody.SetPosition(position);
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

