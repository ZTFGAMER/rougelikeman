using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DeadGoodMgr
{
    private List<DeadGoodCtrl> mList = new List<DeadGoodCtrl>();

    public void DeInit()
    {
        int num = 0;
        int count = this.mList.Count;
        while (num < count)
        {
            this.mList[num].DeInit();
            num++;
        }
        this.mList.Clear();
    }

    public void Init()
    {
    }

    public void StartDrop(Vector3 pos, List<BattleDropData> goodslist, int radius, Transform MapGoodsDrop)
    {
        <StartDrop>c__AnonStorey0 storey = new <StartDrop>c__AnonStorey0 {
            $this = this,
            ctrl = new DeadGoodCtrl()
        };
        storey.ctrl.Init();
        storey.ctrl.StartDrop(pos, goodslist, radius, MapGoodsDrop, new Action(storey.<>m__0));
        this.mList.Add(storey.ctrl);
    }

    [CompilerGenerated]
    private sealed class <StartDrop>c__AnonStorey0
    {
        internal DeadGoodCtrl ctrl;
        internal DeadGoodMgr $this;

        internal void <>m__0()
        {
            this.ctrl.DeInit();
            if (this.$this.mList.Contains(this.ctrl))
            {
                this.$this.mList.Remove(this.ctrl);
            }
        }
    }
}

