using Dxx.Util;
using System;
using UnityEngine;

public class BulletSlopeCreateBase : BulletSlopeBase
{
    [Header("分裂石头ID")]
    public int DivideID;
    [Header("分裂石头数量")]
    public int DivideCount;
    [Header("分裂石头初始角度偏移")]
    public int AngelOffset;
    [Header("分裂石头高度")]
    public float Height = 0.3f;
    [Header("分裂石头初始前进距离")]
    public float ForwardLength;
    private Vector3 shadowScaleInit = (Vector3.one * -1f);
    private float height;

    protected override void OnInit()
    {
        base.OnInit();
        if ((this.shadowScaleInit.x < 0f) && (base.shadow != null))
        {
            this.shadowScaleInit = base.shadow.localScale;
        }
        this.height = base.transform.position.y;
    }

    protected override void OnOverDistance()
    {
        if (this.DivideID == 0)
        {
            object[] args = new object[] { base.mTransform.name };
            SdkManager.Bugly_Report("BulletDivideBase", Utils.FormatString("{0} OnOverDistance DivideID == 0", args));
        }
        else if (this.DivideCount == 0)
        {
            object[] args = new object[] { base.mTransform.name };
            SdkManager.Bugly_Report("BulletDivideBase", Utils.FormatString("{0} OnOverDistance DivideCount == 0", args));
        }
        else
        {
            float num = 360f / ((float) this.DivideCount);
            for (int i = 0; i < this.DivideCount; i++)
            {
                float angle = (i * num) + this.AngelOffset;
                float num4 = MathDxx.Sin(angle) * this.ForwardLength;
                float num5 = MathDxx.Cos(angle) * this.ForwardLength;
                GameLogic.Release.Bullet.CreateBullet(base.m_Entity, this.DivideID, new Vector3(base.mTransform.position.x + num4, this.Height, base.mTransform.position.z + num5), angle);
            }
        }
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        float num = base.transform.position.y / this.height;
        if (base.shadow != null)
        {
            base.shadow.localScale = ((this.shadowScaleInit * (1f - num)) * 0.7f) + (this.shadowScaleInit * 0.3f);
        }
    }
}

