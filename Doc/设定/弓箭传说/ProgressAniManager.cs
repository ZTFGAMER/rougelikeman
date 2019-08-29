using DG.Tweening;
using DG.Tweening.Core;
using Dxx.Util;
using System;
using System.Collections.Generic;

public class ProgressAniManager
{
    public int currentlevel;
    public float currentvalue;
    public Dictionary<int, float> levellist;
    public float progresstime = 1f;
    private float addvalue;
    private float currentaddvalue;
    private ProgressTransfer mTransfer = new ProgressTransfer(1, 0f);
    private Sequence seq;
    private Action<ProgressTransfer> updatecallback;

    public void Deinit()
    {
        this.KillSequence();
    }

    private float getcurrentmax()
    {
        if (this.levellist.TryGetValue(this.currentlevel, out float num))
        {
            return (num * 100f);
        }
        return -1f;
    }

    public void Init(int currentlevel, float currentvalue, Dictionary<int, float> levellist)
    {
        this.currentlevel = currentlevel;
        this.currentvalue = currentvalue;
        this.levellist = levellist;
        this.mTransfer.currentvalue = this.currentvalue;
        this.mTransfer.currentlevel = this.currentlevel;
        this.mTransfer.levellist = this.levellist;
        this.mTransfer.init();
    }

    private void KillSequence()
    {
        if (this.seq != null)
        {
            TweenExtensions.Kill(this.seq, false);
            this.seq = null;
        }
    }

    public Sequence Play(float add)
    {
        add *= 100f;
        this.currentaddvalue -= this.mTransfer.changevalue;
        this.currentaddvalue += add;
        this.addvalue = this.currentaddvalue;
        this.currentlevel = this.mTransfer.currentlevel;
        this.currentvalue = this.mTransfer.currentvalue;
        this.mTransfer.clear();
        this.KillSequence();
        this.seq = DOTween.Sequence();
        float num = this.getcurrentmax();
        float num2 = 0f;
        float num3 = 0f;
        float num4 = 0f;
        float num5 = 0f;
        while ((this.currentvalue + this.addvalue) >= num)
        {
            num3 = 1f;
            num2 = this.currentvalue / num;
            this.currentlevel++;
            num5 = (num3 - num2) * this.progresstime;
            num4 += num5;
            if (this.seq != null)
            {
                int currentlevel = this.currentlevel;
                TweenSettingsExtensions.Append(this.seq, TweenSettingsExtensions.OnUpdate<TweenerCore<float, float, FloatOptions>>(DOTween.To(new DOGetter<float>(this, this.<Play>m__0), new DOSetter<float>(this, this.<Play>m__1), num, num5), new TweenCallback(this, this.<Play>m__2)));
                TweenSettingsExtensions.AppendCallback(this.seq, new TweenCallback(this, this.<Play>m__3));
                TweenSettingsExtensions.AppendCallback(this.seq, new TweenCallback(this, this.<Play>m__4));
            }
            this.addvalue -= num - this.currentvalue;
            this.currentvalue = 0f;
            num = this.getcurrentmax();
        }
        if (this.currentlevel >= GameLogic.Self.m_EntityData.MaxLevel)
        {
            TweenSettingsExtensions.AppendCallback(this.seq, new TweenCallback(this, this.<Play>m__5));
        }
        else
        {
            num2 = this.currentvalue / num;
            num3 = (this.currentvalue + this.addvalue) / num;
            num5 = (num3 - num2) * this.progresstime;
            num4 += num5;
            if (this.seq != null)
            {
                TweenSettingsExtensions.Append(this.seq, TweenSettingsExtensions.OnUpdate<TweenerCore<float, float, FloatOptions>>(DOTween.To(new DOGetter<float>(this, this.<Play>m__6), new DOSetter<float>(this, this.<Play>m__7), this.currentvalue + this.addvalue, num5), new TweenCallback(this, this.<Play>m__8)));
                TweenSettingsExtensions.AppendCallback(this.seq, new TweenCallback(this, this.<Play>m__9));
            }
        }
        return this.seq;
    }

    public void SetUpdate(Action<ProgressTransfer> updatecallback)
    {
        this.updatecallback = updatecallback;
    }

    public class ProgressTransfer
    {
        private float _currentvalue;
        public float changevalue;
        public float maxvalue;
        public int currentlevel;
        public bool islevelup;
        public bool isend;
        public Dictionary<int, float> levellist;

        public ProgressTransfer(int currentlevel, float currentvalue)
        {
            this.init(currentlevel, currentvalue);
        }

        public void clear()
        {
            this.changevalue = 0f;
        }

        private float getcurrentmax()
        {
            if (this.levellist.TryGetValue(this.currentlevel, out float num))
            {
                return (num * 100f);
            }
            return -1f;
        }

        public void init()
        {
            this.islevelup = false;
            this.isend = true;
            this.clear();
        }

        public void init(int currentlevel, float currentvalue)
        {
            this.currentlevel = currentlevel;
            this.currentvalue = currentvalue;
            this.init();
        }

        public void levelup()
        {
            this.currentlevel++;
        }

        public override string ToString()
        {
            object[] args = new object[] { this.currentlevel, this.currentvalue, this.percent, this.islevelup };
            return Utils.FormatString("level:{0} value:{1} percent:{2} islevelup:{3}", args);
        }

        public float currentvalue
        {
            get => 
                this._currentvalue;
            set
            {
                if (value != 0f)
                {
                    float num = value - this._currentvalue;
                    this.changevalue += num;
                }
                this._currentvalue = value;
            }
        }

        public float percent
        {
            get
            {
                float num = this.getcurrentmax();
                if (num > 0f)
                {
                    return (this.currentvalue / num);
                }
                return 1f;
            }
        }
    }
}

