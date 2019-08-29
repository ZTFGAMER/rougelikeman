using Dxx.Util;
using System;
using UnityEngine;

public class BuffAlone1015 : BuffAloneBase
{
    private float range;

    protected override void ExcuteBuff(BuffAloneBase.BuffData data)
    {
        EntityBase entity = GameLogic.Release.Entity.GetNearEntity(base.m_Entity, this.range, false);
        if (entity != null)
        {
            int bulletID = 0xbba;
            BulletBase base3 = GameLogic.Release.Bullet.CreateBullet(base.m_Entity, bulletID, base.m_Entity.position + new Vector3(0f, 1f, 0f), 0f);
            if (base3 != null)
            {
                Vector3 dir = entity.position - base.m_Entity.position;
                float y = Utils.getAngle(dir);
                base3.transform.rotation = Quaternion.Euler(0f, y, 0f);
                base3.SetTarget(entity, 1);
            }
        }
    }

    protected override void OnRemove()
    {
    }

    protected override void OnStart()
    {
        this.range = base.buff_data.Args[0];
    }
}

