using Dxx.Util;
using System;
using UnityEngine;

public class BulletDivideRangeBase : BulletBase
{
    [Header("分裂石头ID")]
    public int DivideID;
    [Header("分裂石头数量")]
    public int DivideCount;
    [Header("角度min")]
    public int angle_min;
    [Header("角度max")]
    public int angle_max;
    [Header("角度随机")]
    public int angle_random;
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
            SdkManager.Bugly_Report("BulletDivideRandomBase", Utils.FormatString("{0} OnOverDistance DivideID == 0", args));
        }
        else if (this.DivideCount == 0)
        {
            object[] args = new object[] { base.mTransform.name };
            SdkManager.Bugly_Report("BulletDivideRandomBase", Utils.FormatString("{0} OnOverDistance DivideCount == 0", args));
        }
        else
        {
            float num = GameLogic.Random((float) (((float) -this.angle_random) / 2f), (float) (((float) this.angle_random) / 2f));
            for (int i = 0; i < this.DivideCount; i++)
            {
                GameLogic.Release.Bullet.CreateBulletInternal(base.m_Entity, this.DivideID, new Vector3(base.mTransform.position.x, 1f, base.mTransform.position.z), (Utils.GetBulletAngle(i, this.DivideCount, (float) (this.angle_max - this.angle_min)) + (((float) (this.angle_max + this.angle_min)) / 2f)) + num, this.ClearAttribute);
            }
        }
    }
}

