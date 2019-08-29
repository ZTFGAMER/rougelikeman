using Dxx.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AI5043 : AIBase
{
    private WeightRandomCount mWeightRandom = new WeightRandomCount(2);
    private int callid = 0xbd9;
    private int maxcount = 8;
    private List<Vector3> list = new List<Vector3>();

    private ActionBasic.ActionBase GetCall1()
    {
        AIBase.ActionSequence sequence = new AIBase.ActionSequence {
            ConditionBase1Data = this.callid,
            ConditionBase1 = new Func<object, bool>(this.GetCanCall)
        };
        sequence.AddAction(base.GetActionCall(this.callid));
        sequence.AddAction(base.GetActionWait("actionwait", 500));
        return sequence;
    }

    private ActionBasic.ActionBase GetCall2()
    {
        AIBase.ActionSequence sequence = new AIBase.ActionSequence {
            ConditionBase1Data = this.callid,
            ConditionBase1 = new Func<object, bool>(this.GetCanCall)
        };
        sequence.AddAction(base.GetActionDelegate(delegate {
            base.m_Entity.m_AniCtrl.SendEvent("Call", false);
        }));
        sequence.AddAction(base.GetActionWait(string.Empty, 600));
        sequence.AddAction(base.GetActionDelegate(delegate {
            int aliveCount = base.GetAliveCount(this.callid, false);
            int num2 = this.maxcount - aliveCount;
            num2 = MathDxx.Clamp(num2, 0, this.list.Count);
            this.list.RandomSort<Vector3>();
            for (int i = 0; i < num2; i++)
            {
                base.CallOneInternal(this.callid, this.list[i], true);
                base.AddCallCount(this.callid);
            }
        }));
        sequence.AddAction(base.GetActionWait("actionwait", 0x4b0));
        return sequence;
    }

    protected override void OnAIDeInit()
    {
    }

    protected override void OnInit()
    {
        switch (this.mWeightRandom.GetRandom())
        {
            case 0:
                base.AddAction(this.GetCall1());
                break;

            case 1:
                base.AddAction(this.GetCall2());
                break;

            case 2:
                base.AddActionDelegate(() => base.m_Entity.m_EntityData.ExcuteAttributes("RotateSpeed%", 0x15f90L));
                base.AddAction(base.GetActionAttack(string.Empty, 0x13fb, true));
                base.AddActionDelegate(() => base.m_Entity.m_EntityData.ExcuteAttributes("RotateSpeed%", -90000L));
                break;

            case 3:
                base.AddActionDelegate(() => base.m_Entity.m_EntityData.ExcuteAttributes("RotateSpeed%", 0x15f90L));
                base.AddAction(base.GetActionAttack(string.Empty, 0x13fc, true));
                base.AddActionDelegate(() => base.m_Entity.m_EntityData.ExcuteAttributes("RotateSpeed%", -90000L));
                break;
        }
        AIMove1031 action = new AIMove1031(base.m_Entity, GameLogic.Random((float) 1f, (float) 2f)) {
            peradd = 0.3f
        };
        base.AddAction(action);
        base.bReRandom = true;
    }

    protected override void OnInitOnce()
    {
        int width = GameLogic.Release.MapCreatorCtrl.width;
        int height = GameLogic.Release.MapCreatorCtrl.height;
        this.list.Add(GameLogic.Release.MapCreatorCtrl.GetWorldPosition(new Vector2Int(0, 0)));
        this.list.Add(GameLogic.Release.MapCreatorCtrl.GetWorldPosition(new Vector2Int(0, height - 1)));
        this.list.Add(GameLogic.Release.MapCreatorCtrl.GetWorldPosition(new Vector2Int(width - 1, 0)));
        this.list.Add(GameLogic.Release.MapCreatorCtrl.GetWorldPosition(new Vector2Int(width - 1, height - 1)));
        this.mWeightRandom.Add(1, 20);
        this.mWeightRandom.Add(2, 10);
        this.mWeightRandom.Add(3, 10);
        base.InitCallData(this.callid, this.maxcount, 0x7fffffff, 3, 1, 2);
    }
}

