using Dxx.Util;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AI5038 : AIBase
{
    private WeightRandomCount mWeightRandom = new WeightRandomCount(2, 3);
    private ThunderContinueMgr.ThunderContinueReceive receive;

    private ActionBasic.ActionBase get_thunder(int row)
    {
        <get_thunder>c__AnonStorey0 storey = new <get_thunder>c__AnonStorey0 {
            row = row,
            $this = this
        };
        AIBase.ActionSequence sequence = new AIBase.ActionSequence {
            m_Entity = base.m_Entity
        };
        int width = GameLogic.Release.MapCreatorCtrl.width;
        int height = GameLogic.Release.MapCreatorCtrl.height;
        for (int i = 0; i < height; i++)
        {
            <get_thunder>c__AnonStorey1 storey2 = new <get_thunder>c__AnonStorey1 {
                <>f__ref$0 = storey,
                index = i
            };
            sequence.AddAction(base.GetActionDelegate(new Action(storey2.<>m__0)));
            sequence.AddAction(base.GetActionWait(string.Empty, 80));
        }
        return sequence;
    }

    protected override void OnAIDeInit()
    {
        if (this.receive != null)
        {
            this.receive.Deinit();
        }
    }

    protected override void OnInit()
    {
        switch (this.mWeightRandom.GetRandom())
        {
            case 0:
            {
                base.AddAction(base.GetActionDelegate(() => base.m_Entity.m_AniCtrl.SendEvent("Skill", false)));
                base.AddAction(base.GetActionWait(string.Empty, 0x3e8));
                ActionBasic.ActionParallel action = new ActionBasic.ActionParallel {
                    m_Entity = base.m_Entity
                };
                int width = GameLogic.Release.MapCreatorCtrl.width;
                List<bool> list = new List<bool>();
                int num3 = 7;
                for (int i = 0; i < num3; i++)
                {
                    list.Add(true);
                }
                for (int j = 0; j < (width - num3); j++)
                {
                    list.Add(false);
                }
                list.RandomSort<bool>();
                for (int k = 0; k < width; k++)
                {
                    if (list[k])
                    {
                        action.Add(this.get_thunder(k));
                    }
                }
                base.AddAction(action);
                break;
            }
            case 1:
                base.AddAction(base.GetActionAttack(string.Empty, 0x13f1, false));
                break;

            case 2:
                base.AddAction(base.GetActionDelegate(() => base.m_Entity.m_AniCtrl.SendEvent("Skill", false)));
                base.AddAction(base.GetActionWait(string.Empty, 0x3e8));
                base.AddAction(base.GetActionDelegate(delegate {
                    if (this.receive != null)
                    {
                        this.receive.Deinit();
                    }
                    ThunderContinueMgr.ThunderContinueData data = new ThunderContinueMgr.ThunderContinueData {
                        entity = base.m_Entity,
                        bulletid = 0x13f2,
                        count = 4,
                        delay = 0.15f
                    };
                    this.receive = ThunderContinueMgr.GetThunderContinue(data);
                }));
                break;
        }
        base.AddAction(base.GetActionWaitRandom(string.Empty, 500, 0x4b0));
        base.bReRandom = true;
    }

    protected override void OnInitOnce()
    {
        base.OnInitOnce();
    }

    [CompilerGenerated]
    private sealed class <get_thunder>c__AnonStorey0
    {
        internal int row;
        internal AI5038 $this;
    }

    [CompilerGenerated]
    private sealed class <get_thunder>c__AnonStorey1
    {
        internal int index;
        internal AI5038.<get_thunder>c__AnonStorey0 <>f__ref$0;

        internal void <>m__0()
        {
            Vector3 worldPosition = GameLogic.Release.MapCreatorCtrl.GetWorldPosition(this.<>f__ref$0.row, this.index);
            GameLogic.Release.Bullet.CreateBullet(this.<>f__ref$0.$this.m_Entity, 0x43b, worldPosition, 0f);
        }
    }
}

