using Dxx.Util;
using System;

public class Bullet1010501 : BulletBase
{
    private float mDelayTime;
    private const float DelayTime = 0.4f;

    protected override void OnInit()
    {
        base.OnInit();
        this.mDelayTime = Updater.AliveTime;
        base.OnMove(0.7f);
    }

    protected override void OnUpdate()
    {
        if ((Updater.AliveTime - this.mDelayTime) > 0.4f)
        {
            base.OnUpdate();
        }
    }
}

