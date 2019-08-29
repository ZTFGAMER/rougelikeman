using DG.Tweening;
using Dxx.Util;
using System;
using UnityEngine;

public class BulletRedLineCtrl : MonoBehaviour
{
    public SpriteRenderer line1;
    public SpriteRenderer line2;
    private float line1height;
    private float line2height;

    private void Awake()
    {
        if (this.line1 != null)
        {
            this.line1height = this.line1.size.y;
        }
        if (this.line2 != null)
        {
            this.line2height = this.line2.size.y;
        }
    }

    public void PlayLineWidth()
    {
        this.PlayLineWidth(0f, 1f, 0.3f);
    }

    public void PlayLineWidth(float start, float end, float time)
    {
        if (this.line1 != null)
        {
            this.line1.transform.localScale = new Vector3(start, 1f, 1f);
            TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScaleX(this.line1.transform, end, time), 1);
        }
        if (this.line2 != null)
        {
            this.line2.transform.localScale = new Vector3(start, 1f, 1f);
            TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScaleX(this.line2.transform, end, time), 1);
        }
    }

    public void SetLine(bool islast, float length)
    {
        if (islast)
        {
            if (this.line1 != null)
            {
                this.line1.gameObject.SetActive(true);
                this.line1.size = new Vector2(length, this.line1height);
            }
            if (this.line2 != null)
            {
                this.line2.gameObject.SetActive(false);
            }
        }
        else
        {
            if (this.line1 != null)
            {
                this.line1.gameObject.SetActive(false);
            }
            if (this.line2 != null)
            {
                this.line2.gameObject.SetActive(true);
                this.line2.size = new Vector2(length, this.line2height);
            }
        }
    }

    public void UpdateLine(bool throughinsidewall, float width)
    {
        Vector3 vector = (new Vector3(MathDxx.Sin(base.transform.eulerAngles.y + 90f), 0f, MathDxx.Cos(base.transform.eulerAngles.y + 90f)) * width) / 2f;
        RayCastManager.CastMinDistance(base.transform.position + vector, base.transform.eulerAngles.y, throughinsidewall, out float num);
        RayCastManager.CastMinDistance(base.transform.position - vector, base.transform.eulerAngles.y, throughinsidewall, out float num2);
        float length = (num >= num2) ? num2 : num;
        this.SetLine(true, length);
    }
}

