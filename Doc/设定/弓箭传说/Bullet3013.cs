using DG.Tweening;
using System;

public class Bullet3013 : BulletBase
{
    private float enabletime = 0.03f;
    private Sequence seq_enableonce;

    protected override void OnInit()
    {
        base.bLight45 = true;
        base.OnInit();
        base.SetBoxEnableOnce(this.enabletime);
    }
}

