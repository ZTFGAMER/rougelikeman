using DG.Tweening;
using System;
using UnityEngine;

public class ShopUI_Discount_StarCtrl : MonoBehaviour
{
    public float delaytime_min;
    public float delaytime_max;
    private Sequence seq;
    private float delaytime;

    private void OnDestroy()
    {
        if (this.seq != null)
        {
            TweenExtensions.Kill(this.seq, false);
            this.seq = null;
        }
    }

    private void Start()
    {
        base.transform.localScale = Vector3.zero;
        if (this.delaytime_min < 0f)
        {
            this.delaytime_min = 0f;
        }
        if (this.delaytime_max < this.delaytime_min)
        {
            this.delaytime_max = this.delaytime_min;
        }
        this.delaytime = GameLogic.Random(this.delaytime_min, this.delaytime_max);
    }

    private void Update()
    {
        if (this.seq == null)
        {
            this.delaytime -= Time.deltaTime;
            if (this.delaytime <= 0f)
            {
                this.seq = DOTween.Sequence();
                float num = 1f;
                TweenSettingsExtensions.Append(this.seq, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(base.transform, 1f, num), 1));
                TweenSettingsExtensions.Join(this.seq, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DORotate(base.transform, new Vector3(0f, 0f, 180f), num, 0), 1));
                TweenSettingsExtensions.Append(this.seq, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(base.transform, 0f, num), 1));
                TweenSettingsExtensions.Join(this.seq, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DORotate(base.transform, new Vector3(0f, 0f, 360f), num, 0), 1));
                TweenSettingsExtensions.AppendInterval(this.seq, 0.33f);
                TweenSettingsExtensions.SetLoops<Sequence>(this.seq, -1);
            }
        }
    }
}

