using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class TipsUICtrl : MonoBehaviour
{
    private const float time = 1.5f;
    private static Vector3 endpos = new Vector3(0f, 100f, 0f);
    private Sequence seq;
    private Text text1;
    private CanvasGroup canvasgroup;

    private void Awake()
    {
        this.text1 = base.transform.Find("Image_BG/Text1").GetComponent<Text>();
        this.canvasgroup = base.GetComponent<CanvasGroup>();
    }

    public void Init()
    {
        base.transform.localPosition = Vector3.zero;
        this.canvasgroup.alpha = 1f;
        if (this.seq != null)
        {
            TweenExtensions.Kill(this.seq, false);
        }
        Tweener tweener = TweenSettingsExtensions.SetUpdate<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalMove(base.transform, endpos, 1.5f, false), 15), true);
        Sequence sequence = DOTween.Sequence();
        TweenSettingsExtensions.SetUpdate<Sequence>(sequence, true);
        TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOFade(this.canvasgroup, 0.6f, 0.9f));
        TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOFade(this.canvasgroup, 0f, 0.6f));
        base.transform.localScale = Vector3.one * 0.5f;
        Tweener tweener2 = ShortcutExtensions.DOScale(base.transform, 1f, 0.45f);
        this.seq = DOTween.Sequence();
        TweenSettingsExtensions.SetUpdate<Sequence>(this.seq, true);
        TweenSettingsExtensions.Append(this.seq, tweener);
        TweenSettingsExtensions.Join(this.seq, sequence);
        TweenSettingsExtensions.Join(this.seq, tweener2);
        TweenSettingsExtensions.AppendCallback(this.seq, new TweenCallback(this, this.<Init>m__1));
    }

    public void Init(string value)
    {
        this.text1.text = value;
        this.Init();
    }

    public void Init(string value, Color color)
    {
        this.text1.set_color(color);
        this.Init(value);
    }

    public void InitNotAni(string value)
    {
        this.text1.text = value;
        if (this.seq != null)
        {
            TweenExtensions.Kill(this.seq, false);
        }
        this.seq = DOTween.Sequence();
        TweenSettingsExtensions.SetUpdate<Sequence>(this.seq, true);
        TweenSettingsExtensions.AppendInterval(this.seq, 1f);
        TweenSettingsExtensions.AppendCallback(this.seq, new TweenCallback(this, this.<InitNotAni>m__0));
    }
}

