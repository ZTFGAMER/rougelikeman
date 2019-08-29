using DG.Tweening;
using DG.Tweening.Core;
using Dxx.Util;
using System;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpdateCtrl : MonoBehaviour
{
    public RectTransform child;
    public RectTransform image_bg;
    public Text Text_Value;
    public Text Text_Change;
    private float change_y_init;
    private int m_before;
    private int m_after;
    private int m_current;
    private static Color color_add = Color.green;
    private static Color color_reduce = Color.red;

    private void Awake()
    {
        this.change_y_init = this.Text_Change.get_rectTransform().anchoredPosition.y;
    }

    public void Init(int before, int after)
    {
        this.m_before = before;
        this.m_after = after;
        this.m_current = this.m_before;
        this.SetTextValue(this.m_before);
        this.child.localScale = new Vector3(0f, 1f, 1f);
        this.child.sizeDelta = new Vector2(this.Text_Value.preferredWidth, this.child.sizeDelta.y);
        this.image_bg.sizeDelta = new Vector2(this.Text_Value.preferredWidth + 300f, this.image_bg.sizeDelta.y);
        int num = after - before;
        bool flag = num >= 0;
        this.Text_Change.set_color(!flag ? color_reduce : color_add);
        string str = !flag ? "-" : "+";
        object[] args = new object[] { str, MathDxx.Abs(num) };
        this.Text_Change.text = Utils.FormatString("{0}{1}", args);
        this.Text_Change.get_rectTransform().anchoredPosition = new Vector2(this.Text_Value.preferredWidth + 10f, 0f);
        this.PlayAnimation();
    }

    private void PlayAnimation()
    {
        Sequence sequence = DOTween.Sequence();
        TweenSettingsExtensions.Append(sequence, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScaleX(this.child, 1f, 0.2f), 0x1b));
        TweenSettingsExtensions.AppendInterval(sequence, 0.3f);
        TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOFade(this.Text_Change, 0f, 0.5f));
        TweenSettingsExtensions.Join(sequence, TweenSettingsExtensions.OnUpdate<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(DOTween.To(new DOGetter<int>(this, this.<PlayAnimation>m__0), new DOSetter<int>(this, this.<PlayAnimation>m__1), this.m_after, 0.5f), 3), new TweenCallback(this, this.<PlayAnimation>m__2)));
        TweenSettingsExtensions.AppendInterval(sequence, 0.3f);
        TweenSettingsExtensions.Append(sequence, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScaleX(this.child, 0f, 0.15f), 5));
        TweenSettingsExtensions.AppendCallback(sequence, new TweenCallback(this, this.<PlayAnimation>m__3));
    }

    private void SetTextValue(int value)
    {
        string str = "战斗力";
        object[] args = new object[] { str, value };
        this.Text_Value.text = Utils.FormatString("{0} {1}", args);
    }
}

