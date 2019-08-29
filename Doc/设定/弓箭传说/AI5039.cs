using Dxx.Util;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AI5039 : AIBase
{
    private WeightRandomCount mWeightRandom = new WeightRandomCount(2, 3);
    private List<GameObject> mPrevs = new List<GameObject>();
    private List<Vector3> poslist = new List<Vector3>();
    private int count = 30;

    private void AddAttack(int attackid, float slowspeed)
    {
        <AddAttack>c__AnonStorey0 storey = new <AddAttack>c__AnonStorey0 {
            slowspeed = slowspeed,
            $this = this
        };
        base.AddAction(base.GetActionDelegate(new Action(storey.<>m__0)));
        base.AddAction(base.GetActionAttack(string.Empty, attackid, false));
        base.AddAction(base.GetActionDelegate(new Action(storey.<>m__1)));
    }

    private void CachePrevs()
    {
        int num = 0;
        int count = this.mPrevs.Count;
        while (num < count)
        {
            GameLogic.EffectCache(this.mPrevs[num]);
            num++;
        }
        this.mPrevs.Clear();
    }

    protected override void OnAIDeInit()
    {
        this.CachePrevs();
    }

    protected override void OnInit()
    {
        switch (this.mWeightRandom.GetRandom())
        {
            case 0:
                base.AddAction(base.GetActionDelegate(delegate {
                    this.CachePrevs();
                    this.poslist.Clear();
                    for (int i = 0; i < this.count; i++)
                    {
                        GameObject item = GameLogic.EffectGet("Game/PrevEffect/prev_circle");
                        Vector3 vector = GameLogic.Release.MapCreatorCtrl.RandomPosition();
                        item.transform.position = vector;
                        this.poslist.Add(vector);
                        this.mPrevs.Add(item);
                    }
                }));
                base.AddAction(base.GetActionWait(string.Empty, 500));
                base.AddAction(base.GetActionDelegate(delegate {
                    int num = 0;
                    int count = this.poslist.Count;
                    while (num < count)
                    {
                        (GameLogic.Release.Bullet.CreateBullet(base.m_Entity, 0x13f5, this.poslist[num] + new Vector3(0f, 20f, 0f), 0f) as BulletSlopeBase).SetEndPos(this.poslist[num]);
                        num++;
                    }
                }));
                break;

            case 1:
                this.AddAttack(0x13f4, -0.6f);
                break;

            case 2:
                base.AddAction(base.GetActionAttack(string.Empty, 0x13f6, false));
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
    private sealed class <AddAttack>c__AnonStorey0
    {
        internal float slowspeed;
        internal AI5039 $this;

        internal void <>m__0()
        {
            this.$this.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", this.slowspeed);
        }

        internal void <>m__1()
        {
            this.$this.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", -this.slowspeed);
        }
    }
}

