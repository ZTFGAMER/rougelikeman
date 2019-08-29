using Dxx.Util;
using System;
using UnityEngine;

public class BulletFlyDivideBase : BulletBase
{
    [Header("分裂石头ID")]
    public int DivideID;
    [Header("分裂石头数量")]
    public int DivideCount;
    [Header("分裂石头初始角度偏移")]
    public int AngelOffset;
    [Header("分裂石头初始角度是否根据父子弹角度")]
    public bool DependBulletAngle;
    [Header("创建分裂石头时间间隔")]
    public float DivideTime;
    [Header("清除子弹父亲属性")]
    public bool ClearAttribute = true;
    private float updatetime;

    private void CreateDivideBullet()
    {
        if (this.DivideID == 0)
        {
            object[] args = new object[] { base.mTransform.name };
            SdkManager.Bugly_Report("BulletFlyDivideBase", Utils.FormatString("{0} CreateDivideBullet DivideID == 0", args));
        }
        else if (this.DivideCount == 0)
        {
            object[] args = new object[] { base.mTransform.name };
            SdkManager.Bugly_Report("BulletFlyDivideBase", Utils.FormatString("{0} CreateDivideBullet DivideCount == 0", args));
        }
        else
        {
            float num = 360f / ((float) this.DivideCount);
            for (int i = 0; i < this.DivideCount; i++)
            {
                float bulletAngle = 0f;
                if (this.DependBulletAngle)
                {
                    bulletAngle = base.bulletAngle;
                }
                GameLogic.Release.Bullet.CreateBulletInternal(base.m_Entity, this.DivideID, new Vector3(base.mTransform.position.x, 1f, base.mTransform.position.z), ((i * num) + this.AngelOffset) + bulletAngle, this.ClearAttribute);
            }
        }
    }

    protected override void OnInit()
    {
        base.OnInit();
        this.updatetime = Updater.AliveTime;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        if ((this.DivideTime > 0f) && ((Updater.AliveTime - this.updatetime) > this.DivideTime))
        {
            this.updatetime += this.DivideTime;
            this.CreateDivideBullet();
        }
    }
}

