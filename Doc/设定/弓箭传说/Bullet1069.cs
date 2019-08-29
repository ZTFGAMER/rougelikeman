using DG.Tweening;
using Dxx.Util;
using System;
using TableTool;
using UnityEngine;

public class Bullet1069 : BulletBase
{
    private float time = 1f;
    private float updatetime;
    private float percent;
    private LineRenderer line;
    private Color color;
    private float colora;
    private AnimationCurve curve;

    protected override void OnDeInit()
    {
        base.OnDeInit();
    }

    protected override void OnInit()
    {
        base.OnInit();
        if ((this.line == null) && (base.mBulletModel != null))
        {
            this.line = base.mBulletModel.GetComponentInChildren<LineRenderer>();
            if (this.line != null)
            {
                this.color = this.line.material.GetColor("_TintColor");
                this.colora = this.color.a;
                this.curve = LocalModelManager.Instance.Curve_curve.GetCurve(0x186b8);
            }
        }
        this.updatetime = Updater.AliveTime;
        this.BoxEnable(false);
        TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(base.mSeqPool.Get(), this.time * 0.2f), new TweenCallback(this, this.<OnInit>m__0));
    }

    protected override void OnUpdate()
    {
        if (this.line != null)
        {
            this.percent = (Updater.AliveTime - this.updatetime) / this.time;
            this.color = new Color(this.color.r, this.color.g, this.color.b, this.colora * this.curve.Evaluate(this.percent));
            this.line.material.SetColor("_TintColor", this.color);
        }
    }
}

