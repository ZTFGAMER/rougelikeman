using Dxx.Util;
using System;
using UnityEngine;

public class Bullet3014 : BulletBase
{
    private const float FLY_TIME = 1f;
    private SpriteRenderer sprite_sword;
    private Color mColor = new Color(1f, 1f, 1f, 1f);
    private float flytime;

    protected override void OnInit()
    {
        base.OnInit();
        this.sprite_sword = base.GetComponentInChildren<SpriteRenderer>();
        this.flytime = 1f;
    }

    protected override void OnUpdate()
    {
        if (this.flytime > 0f)
        {
            this.flytime -= Updater.delta;
            this.mColor.a = 1f - (this.flytime / 1f);
            this.mColor.a = MathDxx.Clamp01(this.mColor.a);
            this.sprite_sword.color = this.mColor;
        }
        else
        {
            base.OnUpdate();
        }
    }
}

