using DG.Tweening;
using Dxx.Util;
using System;

public class Bullet5064 : BulletBase
{
    private float angle;
    private float time = 1.25f;
    private float starttime;
    private float updatetime;
    private float alltime = 3.9f;
    private bool bStartCreate;

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
        this.bStartCreate = false;
        TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.Append(base.mSeqPool.Get(), ShortcutExtensions.DOMoveY(base.mTransform, 0.8f, 1f, false)), new TweenCallback(this, this.<OnInit>m__0));
    }

    protected override void UpdateProcess()
    {
        if (this.bStartCreate)
        {
            base.UpdateProcess();
            if ((Updater.AliveTime - this.updatetime) > this.time)
            {
                this.updatetime += this.time;
                this.CreateBullets();
            }
            if ((Updater.AliveTime - this.starttime) > this.alltime)
            {
                this.overDistance();
            }
        }
    }
}

