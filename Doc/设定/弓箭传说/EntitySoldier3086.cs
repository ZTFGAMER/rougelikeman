using System;

public class EntitySoldier3086 : EntityMonsterBase
{
    public override bool SetHitted(HittedData data)
    {
        bool flag = base.SetHitted(data);
        if (flag)
        {
            int num = 2;
            for (int i = 0; i < num; i++)
            {
                GameLogic.Release.Bullet.CreateBullet(this, base.m_Data.WeaponID, base.m_Body.EffectMask.transform.position, GameLogic.Random((float) (i * 180f), (float) ((i * 180f) + 180f)));
            }
        }
        return flag;
    }
}

