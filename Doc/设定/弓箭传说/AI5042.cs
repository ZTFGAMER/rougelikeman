using DG.Tweening;
using Dxx.Util;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AI5042 : AIBase
{
    private List<int> callids;
    private int calldelay1;
    private int calldelay2;
    private float prev_scale;
    private SequencePool mSeqPool;
    private SequencePool mSeqPool2;
    private float delay;
    private List<Vector3> poslist;
    private List<GameObject> prevs;
    private List<BulletRedLineCtrl> mLines;
    private float startangle;
    private static float[] angles = new float[] { 40f, 140f, 220f, 320f };

    public AI5042()
    {
        List<int> list = new List<int> { 
            0xc23,
            0xc25,
            0xc27
        };
        this.callids = list;
        this.calldelay2 = 0x2710;
        this.prev_scale = 1f;
        this.mSeqPool = new SequencePool();
        this.mSeqPool2 = new SequencePool();
        this.delay = 0.25f;
        this.poslist = new List<Vector3>();
        this.prevs = new List<GameObject>();
        this.mLines = new List<BulletRedLineCtrl>();
    }

    private void ClearLines()
    {
        for (int i = 0; i < this.mLines.Count; i++)
        {
            BulletRedLineCtrl ctrl = this.mLines[i];
            if (ctrl != null)
            {
                Object.Destroy(ctrl.gameObject);
            }
        }
        this.mLines.Clear();
    }

    private void CreateBullets()
    {
        for (int i = 0; i < 4; i++)
        {
            float rota = 0f;
            if (this.startangle == 0f)
            {
                rota = (i * 90f) + this.startangle;
            }
            else
            {
                rota = angles[i];
            }
            GameLogic.Release.Bullet.CreateBullet(base.m_Entity, 0x44e, base.m_Entity.m_Body.LeftBullet.transform.position, rota);
        }
    }

    protected override void OnAIDeInit()
    {
        this.mSeqPool.Clear();
        this.mSeqPool2.Clear();
        int num = 0;
        int count = this.prevs.Count;
        while (num < count)
        {
            ShortcutExtensions.DOKill(this.prevs[num].transform, false);
            GameLogic.EffectCache(this.prevs[num]);
            num++;
        }
        this.prevs.Clear();
        this.ClearLines();
    }

    protected override void OnInit()
    {
        base.AddAction(base.GetActionWait("actionwaitr1", this.calldelay1));
        base.AddAction(base.GetActionDelegate(delegate {
            this.mSeqPool.Clear();
            this.poslist.Clear();
            this.prevs.Clear();
            for (int i = 0; i < this.callids.Count; i++)
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
        base.AddAction(base.GetActionWait("actionwaitr1", this.calldelay2));
    }

    protected override void OnInitOnce()
    {
        base.OnInitOnce();
        TweenSettingsExtensions.SetLoops<Sequence>(TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(this.mSeqPool2.Get(), 2f), new TweenCallback(this, this.<OnInitOnce>m__0)), 0.6f), new TweenCallback(this, this.<OnInitOnce>m__1)), -1);
    }

    private void showlines(bool show)
    {
        if (this.mLines.Count == 0)
        {
            for (int i = 0; i < 4; i++)
            {
                GameObject child = Object.Instantiate<GameObject>(ResourceManager.Load<GameObject>("Game/Bullet/Bullet1102_RedLine"));
                child.SetParentNormal(base.m_Entity.m_Body.LeftBullet.transform);
                BulletRedLineCtrl component = child.GetComponent<BulletRedLineCtrl>();
                this.mLines.Add(component);
            }
        }
        if (show)
        {
            int index = 0;
            int count = this.mLines.Count;
            while (index < count)
            {
                BulletRedLineCtrl ctrl2 = this.mLines[index];
                ctrl2.gameObject.SetActive(true);
                if (this.startangle == 0f)
                {
                    ctrl2.transform.rotation = Quaternion.Euler(0f, (index * 90f) + this.startangle, 0f);
                }
                else
                {
                    ctrl2.transform.rotation = Quaternion.Euler(0f, angles[index], 0f);
                }
                ctrl2.UpdateLine(false, 0.5f);
                ctrl2.PlayLineWidth();
                index++;
            }
        }
        else
        {
            int num4 = 0;
            int count = this.mLines.Count;
            while (num4 < count)
            {
                this.mLines[num4].gameObject.SetActive(false);
                num4++;
            }
        }
    }

    [CompilerGenerated]
    private sealed class <OnInit>c__AnonStorey0
    {
        internal int index;
        internal AI5042 $this;

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
            this.$this.CallOneInternal(this.$this.callids[this.index], this.$this.poslist[this.index], true);
        }
    }
}

