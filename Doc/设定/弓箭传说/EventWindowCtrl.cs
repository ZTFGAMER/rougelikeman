using DG.Tweening;
using Dxx.Util;
using System;
using UnityEngine;

public class EventWindowCtrl : MonoBehaviour
{
    public WindowID windowID;
    private GameObject shadow;
    private GoodsEventEmojiCtrl eMojiCtrl;
    private SphereCollider mSphere;
    private Sequence seq;
    private Animation ani;
    [SerializeField]
    private bool bEvent;
    private bool bDelay;
    private bool bOpenUI;
    private float delaystarttime;
    private float starttime;
    private float anispeed = 1f;

    private void Awake()
    {
        this.mSphere = base.GetComponent<SphereCollider>();
        this.eMojiCtrl = base.transform.Find("child/child/body/Emotion_BG").GetComponent<GoodsEventEmojiCtrl>();
        this.shadow = base.transform.Find("child/shadow").gameObject;
        this.ani = base.transform.Find("child").GetComponent<Animation>();
        this.starttime = Updater.AliveTime;
        if (this.windowID == WindowID.WindowID_EventBlackShop)
        {
            this.anispeed = 1.5f;
            SdkManager.send_event_mysteries("APPEAR", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, string.Empty, string.Empty);
        }
    }

    private void Enter(GameObject o)
    {
        if (!this.bEvent && GameLogic.Release.Entity.IsSelfObject(o))
        {
            this.bEvent = true;
            this.Miss();
            this.OnEnter();
        }
    }

    private void KillSequence()
    {
        if (this.seq != null)
        {
            TweenExtensions.Kill(this.seq, false);
            this.seq = null;
        }
    }

    private void Miss()
    {
        this.bDelay = true;
        this.delaystarttime = Updater.AliveTime;
    }

    private void OnCollisionEnter(Collision o)
    {
        this.Enter(o.gameObject);
    }

    private void OnDestroy()
    {
        this.KillSequence();
    }

    private void OnDisable()
    {
        if (this.ani != null)
        {
            this.ani.enabled = false;
        }
    }

    private void OnEnable()
    {
        if (this.ani != null)
        {
            this.ani.enabled = true;
            this.shadow.SetActive(true);
            this.ani[this.MissAction].time = this.ani[this.MissAction].clip.length;
            this.ani[this.MissAction].speed = -this.anispeed;
            this.ani.Play(this.MissAction);
        }
    }

    private void OnEnter()
    {
        this.eMojiCtrl.Near();
        this.KillSequence();
        this.seq = TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(DOTween.Sequence(), 0.3f), new TweenCallback(this, this.<OnEnter>m__0));
    }

    private void OnTriggerEnter(Collider o)
    {
        this.Enter(o.gameObject);
    }

    private void Update()
    {
        if (this.bDelay && ((Updater.AliveTime - this.delaystarttime) > 1f))
        {
            this.bDelay = false;
            Object.Destroy(base.gameObject);
        }
        if ((!this.bEvent && ((Updater.AliveTime - this.starttime) > 1.5f)) && ((this.mSphere != null) && !this.mSphere.enabled))
        {
            this.mSphere.enabled = true;
        }
    }

    protected virtual string MissAction =>
        string.Empty;
}

