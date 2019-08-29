using Dxx.Util;
using System;
using UnityEngine;

public class Bullet1001 : BulletBase
{
    private Animation ani;
    private GameObject dilie;
    private Transform wave;
    private Vector3 startPos;
    private Quaternion startrota;

    protected override void AwakeInit()
    {
        this.ani = base.GetComponentInChildren<Animation>();
        this.wave = base.mTransform.Find("wave");
        Transform transform = base.mTransform.Find("dilie");
        if (transform != null)
        {
            this.dilie = transform.gameObject;
        }
        if (this.ani != null)
        {
            this.startPos = this.ani.transform.localPosition;
            this.startrota = this.ani.transform.localRotation;
        }
    }

    protected override void BoxEnable(bool enable)
    {
        base.BoxEnable(enable);
        this.WaveActive(enable);
    }

    protected override void OnDeInit()
    {
        if (this.ani != null)
        {
            this.ani.enabled = false;
        }
        base.OnDeInit();
    }

    protected override void OnHitHero(EntityBase entity)
    {
        this.PHitWallAnimation();
    }

    protected override void OnHitWall()
    {
        this.PHitWallAnimation();
    }

    protected override void OnInit()
    {
        base.OnInit();
        if (this.dilie != null)
        {
            this.dilie.SetActive(false);
        }
        if (this.ani != null)
        {
            this.ani.transform.localPosition = this.startPos;
            this.ani.transform.localRotation = this.startrota;
            this.ani.enabled = false;
        }
    }

    protected override void OnOverDistance()
    {
        if (this.ani != null)
        {
            this.ani.enabled = true;
            this.ani.Play(base.ClassName);
        }
        if (this.dilie != null)
        {
            this.dilie.SetActive(true);
        }
    }

    protected override void OnThroughTrailShow(bool show)
    {
    }

    private void PHitWallAnimation()
    {
        if (this.ani != null)
        {
            this.ani.enabled = true;
            object[] args = new object[] { base.ClassName, "_Wall" };
            this.ani.Play(Utils.GetString(args));
        }
    }

    private void WaveActive(bool active)
    {
        if (this.wave != null)
        {
            this.wave.gameObject.SetActive(active);
        }
    }
}

