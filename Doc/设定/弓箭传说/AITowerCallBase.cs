using DG.Tweening;
using Dxx.Util;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AITowerCallBase : AIBase
{
    protected int callid;
    protected int callcount;
    protected int calldelay = 0x1770;
    protected float prev_scale = 1f;
    private SequencePool mSeqPool = new SequencePool();
    private float delay = 0.25f;
    private List<Vector3> poslist = new List<Vector3>();
    private List<GameObject> prevs = new List<GameObject>();

    protected override void OnAIDeInit()
    {
        this.mSeqPool.Clear();
        int num = 0;
        int count = this.prevs.Count;
        while (num < count)
        {
            ShortcutExtensions.DOKill(this.prevs[num].transform, false);
            GameLogic.EffectCache(this.prevs[num]);
            num++;
        }
        this.prevs.Clear();
    }

    protected override void OnInit()
    {
        int waitTime = GameLogic.Random(500, 0x5dc);
        base.AddAction(base.GetActionWait("actionwaitr1", waitTime));
        base.AddAction(base.GetActionDelegate(delegate {
            this.mSeqPool.Clear();
            this.poslist.Clear();
            this.prevs.Clear();
            for (int i = 0; i < this.callcount; i++)
            {
                <OnInit>c__AnonStorey0 storey = new <OnInit>c__AnonStorey0 {
                    $this = this,
                    index = i
                };
                Sequence sequence = this.mSeqPool.Get();
                TweenSettingsExtensions.AppendInterval(sequence, storey.index * this.delay);
                TweenSettingsExtensions.AppendCallback(sequence, new TweenCallback(storey, this.<>m__0));
                TweenSettingsExtensions.AppendInterval(sequence, 1f);
                TweenSettingsExtensions.AppendCallback(sequence, new TweenCallback(storey, this.<>m__1));
            }
        }));
        base.AddAction(base.GetActionWait("actionwaitr1", this.calldelay - waitTime));
    }

    protected override void OnInitOnce()
    {
        base.OnInitOnce();
        if (this.callid == 0)
        {
            object[] args = new object[] { base.GetType().ToString() };
            SdkManager.Bugly_Report("AITowerCallBase", Utils.FormatString("{0}.callid == 0", args));
        }
        if (this.callcount == 0)
        {
            object[] args = new object[] { base.GetType().ToString() };
            SdkManager.Bugly_Report("AITowerCallBase", Utils.FormatString("{0}.callcount == 0", args));
        }
    }

    [CompilerGenerated]
    private sealed class <OnInit>c__AnonStorey0
    {
        internal int index;
        internal AITowerCallBase $this;

        internal void <>m__0()
        {
            Vector3 item = GameLogic.Release.MapCreatorCtrl.RandomPosition(this.index % 4);
            for (int i = 0; i < 20; i++)
            {
                Vector3 vector2 = item - this.$this.m_Entity.position;
                if (vector2.magnitude < 2f)
                {
                    item = GameLogic.Release.MapCreatorCtrl.RandomPosition(this.index % 4);
                }
            }
            this.$this.poslist.Add(item);
            GameObject obj2 = GameLogic.EffectGet("Game/PrevEffect/TowerPrev_3057");
            obj2.transform.position = this.$this.m_Entity.position;
            obj2.transform.localScale = Vector3.one * this.$this.prev_scale;
            this.$this.prevs.Add(obj2);
            ShortcutExtensions.DOMove(obj2.transform, this.$this.poslist[this.index], this.$this.delay, false);
        }

        internal void <>m__1()
        {
            this.$this.CallOneInternal(this.$this.callid, this.$this.poslist[this.index], true);
        }
    }
}

