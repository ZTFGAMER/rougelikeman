using DG.Tweening;
using Dxx.Util;
using System;
using UnityEngine;

public class Bullet1024 : BulletBase
{
    private Transform mStart;
    private Transform mEnd;
    private LineRenderer line;
    private const float textureLengthScale = 3f;
    private const float textureScrollSpeed = 8f;
    private int layerMask;
    private float startwidth;
    private float starttime;
    private float line_update_time = 0.2f;
    private bool bNearEnd;
    private SequencePool mPool = new SequencePool();

    public void CheckBulletLength()
    {
        float x = MathDxx.Sin(base.bulletAngle);
        float z = MathDxx.Cos(base.bulletAngle);
        Vector3 direction = new Vector3(x, 0f, z);
        RaycastHit[] hitArray = Physics.RaycastAll(base.mTransform.position, direction, 100f, this.layerMask);
        float distance = 100f;
        int index = 0;
        int length = hitArray.Length;
        while (index < length)
        {
            RaycastHit hit = hitArray[index];
            if ((((hit.collider.gameObject.layer == LayerManager.Stone) && !base.m_Data.bThroughWall) || (hit.collider.gameObject.layer == LayerManager.MapOutWall)) && (distance > hit.distance))
            {
                distance = hit.distance;
            }
            index++;
        }
        if (this.mStart != null)
        {
            this.mStart.position = base.mTransform.position;
        }
        Vector3 position = base.mTransform.position + (direction * distance);
        if (this.mEnd != null)
        {
            this.mEnd.position = position;
        }
        if (this.line != null)
        {
            this.line.positionCount = 2;
            this.line.SetPosition(0, base.mTransform.position);
            this.line.SetPosition(1, position);
            float num6 = Vector3.Distance(base.mTransform.position, position);
            this.line.material.mainTextureScale = new Vector2(num6 / 3f, 1f);
            Material material = this.line.material;
            material.mainTextureOffset -= new Vector2(Time.deltaTime * 8f, 0f);
        }
        base.boxList[0].center = new Vector3(0f, 0f, distance / 2f);
        base.boxList[0].size = new Vector3(base.boxList[0].size.x, base.boxList[0].size.y, distance);
    }

    protected override void OnDeInit()
    {
        base.OnDeInit();
        this.mPool.Clear();
    }

    protected override void OnInit()
    {
        base.OnInit();
        this.mStart = base.mBulletModel.Find("FireBeamStart");
        this.mEnd = base.mBulletModel.Find("FireBeamEnd");
        Transform transform = base.mBulletModel.Find("Fire Beam");
        if (transform != null)
        {
            this.line = transform.GetComponent<LineRenderer>();
            this.line.sortingLayerName = "Hit";
            this.startwidth = this.line.widthMultiplier;
        }
        if (base.m_Data.bThroughInsideWall)
        {
            this.layerMask = ((int) 1) << LayerManager.MapOutWall;
        }
        else
        {
            this.layerMask = (((int) 1) << LayerManager.Stone) | (((int) 1) << LayerManager.MapOutWall);
        }
        this.CheckBulletLength();
        this.starttime = 0f;
        this.bNearEnd = false;
        this.UpdateLineWidth();
        this.mPool.Clear();
        Sequence sequence = this.mPool.Get();
        float num = (((float) base.m_Data.AliveTime) / 1000f) - this.line_update_time;
        TweenSettingsExtensions.AppendInterval(sequence, num);
        TweenSettingsExtensions.AppendCallback(sequence, new TweenCallback(this, this.<OnInit>m__0));
    }

    protected override void OnOverDistance()
    {
        base.OnOverDistance();
        this.line.widthMultiplier = this.startwidth;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        this.UpdateLineWidth();
    }

    private void UpdateLineWidth()
    {
        if (this.line != null)
        {
            if (!this.bNearEnd && (this.starttime < (this.line_update_time + 0.1f)))
            {
                this.starttime += Updater.delta;
                this.starttime = MathDxx.Clamp(this.starttime, 0f, this.line_update_time);
                this.line.widthMultiplier = (this.starttime / this.line_update_time) * this.startwidth;
            }
            if (this.bNearEnd)
            {
                this.starttime += Updater.delta;
                this.starttime = MathDxx.Clamp(this.starttime, 0f, this.line_update_time);
                this.line.widthMultiplier = (1f - (this.starttime / this.line_update_time)) * this.startwidth;
            }
        }
    }
}

