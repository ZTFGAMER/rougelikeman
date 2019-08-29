using System;

public class Bullet8002 : BulletBase
{
    protected override void AwakeInit()
    {
    }

    protected override void BoxEnable(bool enable)
    {
        base.BoxEnable(enable);
    }

    protected override void OnHitHero(EntityBase entity)
    {
    }

    protected override void OnHitWall()
    {
    }

    protected override void OnInit()
    {
        base.OnInit();
    }

    protected override void OnOverDistance()
    {
    }

    protected override void OnThroughTrailShow(bool show)
    {
        if (base.mTrailCtrl != null)
        {
            if (show)
            {
                base.mTrailCtrl.SetTrailTime(2f);
            }
            else
            {
                base.mTrailCtrl.SetTrailTime(1f);
            }
        }
    }
}

