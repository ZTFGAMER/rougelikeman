using System;

public class Bullet1029 : BulletBase
{
    private float perdis = 4f;
    private float curdis;

    private void Create3005()
    {
        this.CreateOne3005(90f);
        this.CreateOne3005(-90f);
    }

    private void CreateOne3005(float angle)
    {
        GameLogic.Release.Bullet.CreateBullet(base.m_Entity, 0xbbd, base.mTransform.position, base.bulletAngle + angle);
    }

    protected override void OnInit()
    {
        base.OnInit();
        this.curdis = this.perdis;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (base.CurrentDistance >= this.curdis)
        {
            this.curdis += this.perdis;
            this.Create3005();
        }
    }
}

