using System;

public class Bullet5062 : BulletBase
{
    private float angle;
    private float createdis;
    private float perdis = 3.5f;

    private void CreateBullets()
    {
        this.angle = GameLogic.Random((float) 0f, (float) 360f);
        for (int i = 0; i < 4; i++)
        {
            GameLogic.Release.Bullet.CreateBullet(base.m_Entity, 0x13c7, base.mTransform.position, (this.angle + base.bulletAngle) + (i * 90));
        }
    }

    protected override void OnDeInit()
    {
        base.OnDeInit();
    }

    protected override void OnInit()
    {
        base.OnInit();
        this.createdis = this.perdis;
    }

    protected override void UpdateProcess()
    {
        base.UpdateProcess();
        if (base.CurrentDistance > this.createdis)
        {
            this.CreateBullets();
            this.createdis += this.perdis;
        }
    }
}

