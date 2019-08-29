using DG.Tweening;
using Dxx.Util;
using System;
using UnityEngine;

public class SkillCreateBase : MonoBehaviour
{
    protected EntityBase m_Entity;
    protected Action<SkillCreateBase> mCallback;
    protected float time;
    private Sequence seq;

    private void Awake()
    {
        this.OnAwake();
    }

    public void Deinit()
    {
        if (this.seq != null)
        {
            TweenExtensions.Kill(this.seq, false);
            this.seq = null;
        }
        this.OnDeinit();
        GameLogic.EffectCache(base.gameObject);
    }

    public void Init(EntityBase entity, string[] args)
    {
        this.m_Entity = entity;
        this.OnInit(args);
        if (this.time == 0f)
        {
            object[] objArray1 = new object[] { base.GetType().ToString() };
            SdkManager.Bugly_Report("SkillCreateBase.cs", Utils.FormatString("Init {0} time is 0", objArray1));
        }
        this.seq = TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(DOTween.Sequence(), this.time), new TweenCallback(this, this.<Init>m__0));
    }

    protected virtual void OnAwake()
    {
    }

    protected virtual void OnDeinit()
    {
    }

    protected virtual void OnInit(string[] args)
    {
    }

    public void SetTimeCallback(Action<SkillCreateBase> callback)
    {
        this.mCallback = callback;
    }
}

