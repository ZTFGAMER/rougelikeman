using System;
using UnityEngine;

public class Bullet5106 : BulletBase
{
    private ConditionBase mCondition1;
    private ConditionBase mCondition2;
    protected int DelayTimeEnable = 100;

    private void create_bullet()
    {
        int num = 3;
        float num2 = GameLogic.Random((float) 0f, (float) 360f);
        for (int i = 0; i < num; i++)
        {
            GameLogic.Release.Bullet.CreateBullet(base.m_Entity, 0x13f3, new Vector3(base.transform.position.x, 0.5f, base.transform.position.z), num2 + ((i * 360f) / ((float) num)));
        }
    }

    protected override void OnInit()
    {
        base.OnInit();
        this.BoxEnable(false);
        this.mCondition1 = AIMoveBase.GetConditionTime(this.DelayTimeEnable);
        this.mCondition2 = AIMoveBase.GetConditionTime(this.DelayTimeEnable + 100);
    }

    protected override void OnUpdate()
    {
        if ((this.mCondition1 != null) && this.mCondition1.IsEnd())
        {
            this.BoxEnable(true);
            this.mCondition1 = null;
            this.create_bullet();
        }
        if ((this.mCondition2 != null) && this.mCondition2.IsEnd())
        {
            this.BoxEnable(false);
            this.mCondition2 = null;
        }
    }
}

