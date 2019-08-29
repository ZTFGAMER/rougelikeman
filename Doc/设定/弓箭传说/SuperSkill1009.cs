using System;
using UnityEngine;

public class SuperSkill1009 : SuperSkillBase
{
    protected override void OnDeInit()
    {
    }

    protected override void OnInit()
    {
    }

    protected override void OnUseSkill()
    {
        int bulletID = 0x22c6;
        int num2 = 4;
        float num3 = 360f / ((float) num2);
        Vector3 pos = base.m_Entity.position + new Vector3(0f, 1f, 0f);
        for (int i = 0; i < num2; i++)
        {
            BulletBase base2 = GameLogic.Release.Bullet.CreateBullet(base.m_Entity, bulletID, pos, (i * num3) + 45f);
            base2.mBulletTransmit.attribute.ReboundWall = new EntityAttributeBase.ValueRange(1, 6, 6);
            int[] buffs = new int[] { 0x405 };
            base2.mBulletTransmit.AddDebuffs(buffs);
            base2.UpdateBulletAttribute();
        }
    }
}

