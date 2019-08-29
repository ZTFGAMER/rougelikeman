using DG.Tweening;
using System;
using UnityEngine;

public class Bullet1083Ctrl : MonoBehaviour
{
    public Transform lightning;
    public GameObject hit;
    private Vector3 startscale = new Vector3(1f, 0f, 1f);
    private Sequence seq;

    private void OnEnable()
    {
        if (this.seq != null)
        {
            TweenExtensions.Kill(this.seq, false);
            this.seq = null;
        }
        this.seq = DOTween.Sequence();
        this.hit.SetActive(false);
        this.lightning.localScale = this.startscale;
        TweenSettingsExtensions.Append(this.seq, ShortcutExtensions.DOScaleY(this.lightning, 1f, 0.1f));
        TweenSettingsExtensions.AppendCallback(this.seq, new TweenCallback(this, this.<OnEnable>m__0));
        TweenSettingsExtensions.AppendInterval(this.seq, 0.2f);
        TweenSettingsExtensions.Append(this.seq, ShortcutExtensions.DOScaleX(this.lightning, 0f, 0.2f));
    }
}

