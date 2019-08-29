using System;
using UnityEngine;

public class SkillCreate1022 : SkillCreateBase
{
    private Transform mCloud;

    protected override void OnAwake()
    {
        this.mCloud = base.transform.Find("child/cloud");
    }

    protected override void OnDeinit()
    {
    }

    protected override void OnInit(string[] args)
    {
        base.time = float.Parse(args[0]);
    }

    private void OnTriggerEnter(Collider o)
    {
        if (o.gameObject.layer == LayerManager.Player)
        {
            EntityBase component = o.GetComponent<EntityBase>();
            if (!GameLogic.IsSameTeam(base.m_Entity, component))
            {
                GameLogic.SendBuff(component, base.m_Entity, 0x407, Array.Empty<float>());
            }
        }
    }
}

