using DG.Tweening;
using System;
using UnityEngine;

public class EventNormalWindowCtrl : MonoBehaviour
{
    private string MissAction = "Event_Angel_Miss";
    public WindowID windowID;
    private GoodsEventEmojiCtrl eMojiCtrl;
    private Animation ani;

    private void Awake()
    {
        this.eMojiCtrl = base.transform.Find("child/child/body/Emotion_BG").GetComponent<GoodsEventEmojiCtrl>();
        this.ani = base.transform.Find("child").GetComponent<Animation>();
        this.ani[this.MissAction].time = this.ani[this.MissAction].clip.length;
        this.ani[this.MissAction].speed = -1f;
        this.ani.Play(this.MissAction);
    }

    private void OnEnter()
    {
        this.eMojiCtrl.Near();
        TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(DOTween.Sequence(), 0.3f), new TweenCallback(this, this.<OnEnter>m__0));
    }

    private void OnExit()
    {
        this.eMojiCtrl.Far();
    }

    private void OnTriggerEnter(Collider o)
    {
        if (GameLogic.Release.Entity.IsSelfObject(o.gameObject))
        {
            this.OnEnter();
        }
    }

    private void OnTriggerExit(Collider o)
    {
        if (GameLogic.Release.Entity.IsSelfObject(o.gameObject))
        {
            this.OnExit();
        }
    }
}

