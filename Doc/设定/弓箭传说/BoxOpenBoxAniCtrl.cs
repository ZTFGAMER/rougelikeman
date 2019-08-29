using DG.Tweening;
using Dxx.Util;
using System;
using UnityEngine;
using UnityEngine.UI;

public class BoxOpenBoxAniCtrl : MonoBehaviour
{
    public const string BoxAni_Open = "BoxOpenOpen";
    public const string BoxAni_Stand = "BoxOpenStand";
    public const string BoxAni_Show = "BoxOpenShow";
    public const string BoxAni_Shock = "shock";
    public Image Image_Down;
    public Animator Ani_Box;
    public GameObject effect_light;
    public GameObject effect_open;
    public GameObject boxshowing;
    public GameObject boxshowone;
    public RectTransform child_box;
    public RectTransform child_box2d;
    private Vector2 boxstartpos;
    private Vector2 child_boxpos;
    private Vector2 child_box2dpos;
    private RectTransform mRectTransform;

    private void Awake()
    {
        this.mRectTransform = base.transform as RectTransform;
        this.boxstartpos = this.mRectTransform.anchoredPosition;
        this.child_boxpos = this.child_box.anchoredPosition;
        this.child_box2dpos = this.child_box2d.anchoredPosition;
    }

    public void Init()
    {
        this.mRectTransform.anchoredPosition = this.boxstartpos;
        this.child_box.anchoredPosition = this.child_boxpos;
        this.child_box2d.anchoredPosition = this.child_box2dpos;
    }

    public void Play(string str)
    {
        this.Ani_Box.Play(str);
    }

    public void Play(BoxState state, LocalSave.TimeBoxType type)
    {
        string str = state.ToString();
        string str2 = string.Empty;
        if (type == LocalSave.TimeBoxType.BoxChoose_DiamondNormal)
        {
            str2 = "eNormal";
            this.Image_Down.set_sprite(SpriteManager.GetUICommon("UICommon_Box02_Down"));
        }
        else if (type == LocalSave.TimeBoxType.BoxChoose_DiamondLarge)
        {
            str2 = "eLarge";
            this.Image_Down.set_sprite(SpriteManager.GetUICommon("UICommon_Box01_Down"));
        }
        object[] args = new object[] { str, str2 };
        string stateName = Utils.FormatString("{0}_{1}", args);
        this.Ani_Box.Play(stateName);
    }

    public void ShowBoxOneEffect(bool value)
    {
        if (this.boxshowone != null)
        {
            this.boxshowone.SetActive(value);
            if (value)
            {
                TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(DOTween.Sequence(), 0.2f), new TweenCallback(this, this.<ShowBoxOneEffect>m__0));
            }
        }
    }

    public void ShowBoxOpeningEffect(bool value)
    {
        if (this.boxshowing != null)
        {
            this.boxshowing.SetActive(value);
        }
    }

    public void ShowOpenEffect(bool value)
    {
        this.effect_open.SetActive(value);
    }

    public enum BoxState
    {
        BoxOpenOpen = 0x65,
        BoxOpenStand = 0x66,
        BoxOpenShow = 0x67
    }
}

