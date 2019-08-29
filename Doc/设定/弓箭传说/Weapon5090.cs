using System;

public class Weapon5090 : WeaponSprintBase
{
    private float onedis = 2.4f;
    private float alldis;

    protected override void OnInit()
    {
        base.distance = 9f;
        base.delaytime = 0.6f;
        this.alldis = 0f;
        base.OnInit();
    }

    protected override void OnUpdateMove(float currentdis)
    {
        this.alldis += currentdis;
        if (this.alldis >= this.onedis)
        {
            this.alldis -= this.onedis;
            if (base.m_Entity.m_HatredTarget != null)
            {
                GameLogic.Release.Bullet.CreateBullet(base.m_Entity, 0x13e1, base.m_Entity.m_Body.EffectMask.transform.position, (base.m_Entity.eulerAngles.y + GameLogic.Random(-15, 15)) + 90f).SetPosFromTarget(10f);
                GameLogic.Release.Bullet.CreateBullet(base.m_Entity, 0x13e1, base.m_Entity.m_Body.EffectMask.transform.position, (base.m_Entity.eulerAngles.y - GameLogic.Random(-15, 15)) - 90f).SetPosFromTarget(10f);
            }
        }
    }
}

