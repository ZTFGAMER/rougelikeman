using DG.Tweening;
using DG.Tweening.Core;
using Dxx.Util;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine.UI;

public class ProgressTextCtrl : ProgressCtrl
{
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private Text <text>k__BackingField;
    private int _cur;
    private int _max = 10;
    public Action<int, int> OnValueChanged;

    protected override void OnAwake()
    {
        this.text = base.transform.Find("Slider/Text").GetComponent<Text>();
    }

    public Sequence PlayCount(int after, float alltime)
    {
        int num = MathDxx.Abs((int) (after - this.current));
        float num2 = alltime;
        if (num != 0)
        {
            num2 = alltime / ((float) num);
        }
        num2 = MathDxx.Clamp(num2, 0f, 0.15f);
        this.PlayTextScale(1.4f, 0.15f);
        TweenSettingsExtensions.SetUpdate<Tweener>(DOTween.To(new DOGetter<int>(this, this.get_current), new DOSetter<int>(this, this.<PlayCount>m__0), after, num2 * num), true);
        Sequence sequence = DOTween.Sequence();
        TweenSettingsExtensions.SetUpdate<Sequence>(TweenSettingsExtensions.AppendInterval(TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(sequence, num2 * (num - 1)), new TweenCallback(this, this.<PlayCount>m__1)), num2), true);
        return sequence;
    }

    public void PlayPercent(float after, float time)
    {
        TweenSettingsExtensions.SetUpdate<TweenerCore<float, float, FloatOptions>>(DOTween.To(new DOGetter<float>(this, this.get_Value), new DOSetter<float>(this, this.<PlayPercent>m__2), after, time), true);
    }

    public void PlayTextScale(float scale, float time)
    {
        if (this.text != null)
        {
            Sequence sequence = DOTween.Sequence();
            TweenSettingsExtensions.SetUpdate<Sequence>(sequence, true);
            TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.text.transform, scale, time));
        }
    }

    public void SetText(string value)
    {
        if (this.text != null)
        {
            this.text.text = value;
        }
    }

    private void UpdateValue()
    {
        if (this._max > 0)
        {
            base.Value = ((float) MathDxx.Clamp(this._cur, 0, this._cur)) / ((float) this._max);
            if (this.text != null)
            {
                object[] args = new object[] { this._cur, this._max };
                this.text.text = Utils.FormatString("{0}/{1}", args);
            }
        }
    }

    public Text text { get; private set; }

    public int current
    {
        get => 
            this._cur;
        set
        {
            this._cur = value;
            this.UpdateValue();
            if (this.OnValueChanged != null)
            {
                this.OnValueChanged(this.current, this.max);
            }
        }
    }

    public int currentcount
    {
        get => 
            this._cur;
        set
        {
            if (value >= 0)
            {
                this._cur = value;
                if ((this._max > 0) && (this.text != null))
                {
                    object[] args = new object[] { this._cur, this._max };
                    this.text.text = Utils.FormatString("{0}/{1}", args);
                }
            }
        }
    }

    public int max
    {
        get => 
            this._max;
        set
        {
            if (value > 0)
            {
                this._max = value;
                this.UpdateValue();
            }
        }
    }
}

