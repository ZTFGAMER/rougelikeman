using Dxx.Util;
using System;
using UnityEngine;

public class Bullet1076 : BulletBase
{
    private Transform line;
    private Transform ball_left;
    private Transform ball_right;
    private float boxx;
    private float maxtime = 1f;
    private float currenttime;
    public float maxWidth = 2f;

    protected override void OnInit()
    {
        base.OnInit();
        this.boxx = 0f;
        this.SetBoxX(this.boxx);
        if (this.line == null)
        {
            this.line = base.mBulletModel.Find("child/line");
        }
        if (this.ball_left == null)
        {
            this.ball_left = base.mBulletModel.Find("child/left");
        }
        if (this.ball_right == null)
        {
            this.ball_right = base.mBulletModel.Find("child/right");
        }
        this.currenttime = Updater.AliveTime;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        this.SetBoxX(((Updater.AliveTime - this.currenttime) / this.maxtime) * this.maxWidth);
    }

    private void SetBoxX(float x)
    {
        x = MathDxx.Clamp(x, x, this.maxWidth);
        if (base.boxList.Length > 0)
        {
            base.boxList[0].size = new Vector3(x + 0.4f, base.boxList[0].size.y, base.boxList[0].size.z);
        }
        if (this.line != null)
        {
            this.line.localScale = new Vector3(this.line.localScale.x, x / 2f, this.line.localScale.z);
        }
        if (this.ball_left != null)
        {
            this.ball_left.localPosition = new Vector3(-x / 2f, 0f, 0f);
            this.ball_left.rotation = Quaternion.Euler(-35f, 0f, 0f);
        }
        if (this.ball_right != null)
        {
            this.ball_right.localPosition = new Vector3(x / 2f, 0f, 0f);
            this.ball_right.rotation = Quaternion.Euler(-35f, 0f, 0f);
        }
    }
}

