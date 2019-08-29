using Dxx.Util;
using System;
using UnityEngine;

public class BulletDivideBase : BulletBase
{
    [Header("分裂石头ID")]
    public int DivideID;
    [Header("分裂石头数量")]
    public int DivideCount;
    [Header("分裂石头初始角度偏移")]
    public int AngelOffset;
    [Header("清除子弹父亲属性")]
    public bool ClearAttribute = true;

    protected override void OnHitWall()
    {
        this.OnOverDistance();
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
                GameLogic.Release.Bullet.CreateBulletInternal(base.m_Entity, this.DivideID, new Vector3(base.mTransform.position.x, 1f, base.mTransform.position.z), (i * num) + this.AngelOffset, this.ClearAttribute);
            }
        }
    }
}

