using DG.Tweening;
using System;
using UnityEngine;

public class BoxOpenBoxCtrl : MonoBehaviour
{
    public BoxOpenBoxAniCtrl mBoxCtrl;
    public BoxOpenScrollCtrl mScrollCtrl;
    private Sequence seq;

    public void Init()
    {
        this.mBoxCtrl.Init();
        this.mScrollCtrl.Init();
    }

    public Sequence PlayScrollShow(bool value)
    {
        if (this.mScrollCtrl == null)
        {
            return null;
        }
        this.seq = DOTween.Sequence();
        if (value)
        {
            this.mScrollCtrl.transform.localScale = Vector3.zero;
            TweenSettingsExtensions.Append(this.seq, ShortcutExtensions.DOScale(this.mScrollCtrl.transform, 1f, 0.3f));
        }
        else
        {
            this.mScrollCtrl.transform.localScale = Vector3.zero;
        }
        return this.seq;
    }
}

