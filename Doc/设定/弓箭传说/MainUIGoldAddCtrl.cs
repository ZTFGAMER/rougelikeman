using DG.Tweening;
using Dxx.Util;
using System;
using UnityEngine;
using UnityEngine.UI;

public class MainUIGoldAddCtrl : MonoBehaviour
{
    public Text text;
    public RectTransform imageRect;
    public Transform child;
    public CanvasGroup mCanvasGroup;
    public Action<MainUIGoldAddCtrl> OnFinish;
    private Sequence seq;

    private void KillSequence()
    {
        if (this.seq != null)
        {
            TweenExtensions.Kill(this.seq, false);
            this.seq = null;
        }
    }

    public void SetGold(long gold)
    {
        this.KillSequence();
        this.seq = DOTween.Sequence();
        this.child.localPosition = Vector3.zero;
        this.mCanvasGroup.alpha = 1f;
        TweenSettingsExtensions.Append(this.seq, ShortcutExtensions.DOLocalMoveY(this.child, 100f, 1f, false));
        TweenSettingsExtensions.Join(this.seq, TweenSettingsExtensions.Append(TweenSettingsExtensions.AppendInterval(DOTween.Sequence(), 0.5f), ShortcutExtensions46.DOFade(this.mCanvasGroup, 0f, 0.5f)));
        TweenSettingsExtensions.AppendCallback(this.seq, new TweenCallback(this, this.<SetGold>m__0));
        object[] args = new object[] { gold };
        this.text.text = Utils.FormatString("+{0}", args);
        float num = this.imageRect.sizeDelta.x + this.text.preferredWidth;
        float x = -num / 2f;
        float num3 = x + this.imageRect.sizeDelta.x;
        this.imageRect.anchoredPosition = new Vector2(x, this.imageRect.anchoredPosition.y);
        this.text.get_rectTransform().anchoredPosition = new Vector2(num3, this.text.get_rectTransform().anchoredPosition.y);
    }
}

