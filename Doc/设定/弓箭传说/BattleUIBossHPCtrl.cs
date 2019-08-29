using Dxx.Util;
using System;
using UnityEngine;

public class BattleUIBossHPCtrl
{
    private RectTransform t1;
    private RectTransform t2;
    private int width;
    private float speed = 70f;
    private bool bStart;
    private float starttime;
    private float mEndX;

    public void DeInit()
    {
        this.update_remove();
    }

    public void Init(RectTransform t1, RectTransform t2, int width)
    {
        this.t1 = t1;
        this.t2 = t2;
        this.width = width;
        this.update_add();
    }

    private void OnUpdate(float delta)
    {
        if ((this.bStart && ((Updater.AliveTime - this.starttime) >= 0.2f)) && ((this.t1 != null) && (this.t2 != null)))
        {
            float x = this.t1.sizeDelta.x - (Updater.delta * this.speed);
            if (x < this.mEndX)
            {
                x = this.mEndX;
                this.bStart = false;
            }
            this.t1.sizeDelta = new Vector2(x, this.t1.sizeDelta.y);
            this.t2.sizeDelta = this.t1.sizeDelta;
        }
    }

    public void Reduce(float endx)
    {
        if (!this.bStart)
        {
            this.starttime = Updater.AliveTime;
        }
        this.bStart = true;
        this.update_add();
        this.mEndX = endx;
    }

    private void update_add()
    {
        this.update_remove();
        Updater.AddUpdate("BattleUIBossHPCtrl", new Action<float>(this.OnUpdate), false);
    }

    private void update_remove()
    {
        Updater.RemoveUpdate("BattleUIBossHPCtrl", new Action<float>(this.OnUpdate));
    }
}

