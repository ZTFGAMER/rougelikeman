using System;
using System.Runtime.InteropServices;

public class Bullet1002 : BulletBase
{
    protected override void AwakeInit()
    {
        base.Parabola_MaxHeight = 3f;
    }

    protected override void OnHitHero(EntityBase entity)
    {
    }

    protected override void OnInit()
    {
        base.OnInit();
    }

    public override void SetTarget(EntityBase entity, int size = 1)
    {
        base.SetTarget(entity, size);
        if (base.PosFromStart2Target < 8f)
        {
            base.PosFromStart2Target = 8f;
        }
    }
}

