using Dxx.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AI5029 : AIBase
{
    private WeightRandomCount mWeight = new WeightRandomCount(1);

    protected override void OnInit()
    {
        switch (this.mWeight.GetRandom())
        {
            case 1:
                base.AddAction(base.GetActionAttack(string.Empty, 0x13c6, true));
                base.AddAction(base.GetActionWaitRandom(string.Empty, 750, 0x47e));
                break;

            case 2:
                base.AddAction(base.GetActionAttack(string.Empty, 0x13d6, true));
                base.AddAction(base.GetActionWaitRandom(string.Empty, 750, 0x47e));
                break;

            case 3:
                base.AddAction(base.GetActionDelegate(() => base.m_Entity.m_AniCtrl.SendEvent("Call", false)));
                base.AddAction(base.GetActionWait(string.Empty, 750));
                base.AddAction(base.GetActionDelegate(delegate {
                    List<Vector3> list = GameLogic.Release.MapCreatorCtrl.GetRoundNotSame(base.m_Entity.position, 4, 2);
                    for (int i = 0; i < list.Count; i++)
                    {
                        Vector3 vector = list[i];
                        Vector3 vector2 = list[i];
                        GameLogic.Release.Bullet.CreateBullet(base.m_Entity, 0x13c8, new Vector3(vector.x, -1f, vector2.z), 0f);
                    }
                }));
                base.AddAction(base.GetActionWait(string.Empty, 0x546));
                break;
        }
        base.bReRandom = true;
    }

    protected override void OnInitOnce()
    {
        this.mWeight.Add(1, 10);
        this.mWeight.Add(2, 15);
        this.mWeight.Add(3, 10);
    }
}

