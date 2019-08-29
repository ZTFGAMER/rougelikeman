using System;
using UnityEngine;

[Serializable, ExecuteInEditMode]
public class SpecialEffectElement : MonoBehaviour
{
    [HideInInspector, SerializeField]
    public float startTime;
    [HideInInspector, SerializeField]
    public bool isLoop = true;
    [HideInInspector, SerializeField]
    public ElementPlayStyle playStyle;
    [HideInInspector, SerializeField]
    public float playTime;
    [NonSerialized, HideInInspector]
    private bool canShow = true;
    protected float currPlayTime;
    [NonSerialized, HideInInspector]
    private bool isPlaying;
    private float speedScale = 1f;

    protected float _CalcLocalTime(float elapseTime) => 
        (elapseTime - this.startTime);

    public bool _CopyValues(SpecialEffectElement o) => 
        false;

    protected virtual void _CustomOperate(float elapseTime)
    {
    }

    protected virtual void _Init()
    {
    }

    protected virtual void _OnDisableElement()
    {
    }

    protected virtual void _OnEnableElement()
    {
    }

    protected virtual void _PauseImpl()
    {
    }

    protected virtual void _PlayImpl()
    {
    }

    protected virtual void _ResetImpl()
    {
    }

    protected virtual void _SetCurrPlayTime(float t)
    {
    }

    private void Awake()
    {
        this._Init();
    }

    public override bool Equals(object o)
    {
        if (o == null)
        {
            return false;
        }
        if (o != this)
        {
            if (base.GetType() != o.GetType())
            {
                return false;
            }
            SpecialEffectElement element = o as SpecialEffectElement;
            if (this.startTime != element.startTime)
            {
                return false;
            }
            if (this.playTime != element.playTime)
            {
                return false;
            }
            if (this.playStyle != element.playStyle)
            {
                return false;
            }
        }
        return true;
    }

    public override int GetHashCode() => 
        this.startTime.GetHashCode();

    public bool IsEnable() => 
        base.gameObject.activeSelf;

    protected bool IsInPlayTimeInterval(float elapseTime)
    {
        if (this.playTime <= 0f)
        {
            return false;
        }
        if ((this.playStyle == ElementPlayStyle.Loop) || (this.playStyle == ElementPlayStyle.Unreset))
        {
            return (elapseTime >= this.startTime);
        }
        return (((elapseTime - (this.startTime + this.playTime)) < Mathf.Epsilon) && (elapseTime >= this.startTime));
    }

    public bool IsPlaying() => 
        this.isPlaying;

    private void OnDisable()
    {
    }

    private void OnEnable()
    {
    }

    public void Pause()
    {
        this._PauseImpl();
        this.isPlaying = false;
    }

    public void Play()
    {
        this._PlayImpl();
        this.isPlaying = true;
    }

    public void Reset()
    {
        this._ResetImpl();
        if (this.isPlaying)
        {
            this.Play();
        }
        else
        {
            this.Pause();
        }
    }

    public void SetCurrPlayTime(float t)
    {
        this.currPlayTime = t;
        if (this.IsEnable())
        {
            float num = this._CalcLocalTime(t);
            if ((num > this.playTime) && (this.playStyle == ElementPlayStyle.Loop))
            {
                num = Mathf.Repeat(num, this.playTime);
            }
            this._SetCurrPlayTime(num);
        }
    }

    public void SetEnable(bool b)
    {
        if (this.canShow)
        {
            base.gameObject.SetActive(b);
        }
    }

    public virtual void SetSpeedScale(float scale)
    {
        this.speedScale = scale;
        if (this.IsPlaying())
        {
            this.UpdateSpeed();
        }
    }

    private void Start()
    {
    }

    public void Stop()
    {
        this._ResetImpl();
        this.Pause();
    }

    public void UpdatePlayingState(float elapseTime)
    {
        if (this.IsInPlayTimeInterval(elapseTime) && !this.IsPlaying())
        {
            this.Play();
            float speedScale = this.speedScale;
            this.SetSpeedScale(1f);
            this.SetCurrPlayTime(elapseTime);
            this.Play();
            this.SetSpeedScale(speedScale);
        }
    }

    public virtual void UpdateSpeed()
    {
    }

    public void UpdateState(float elapseTime)
    {
        if (this.IsInPlayTimeInterval(elapseTime))
        {
            if (!this.IsEnable() && this.canShow)
            {
                this.SetEnable(true);
                this._OnEnableElement();
            }
        }
        else if (this.IsEnable())
        {
            this.Stop();
            this._OnDisableElement();
            this.SetEnable(false);
        }
    }

    public bool CanShow
    {
        get => 
            this.canShow;
        set
        {
            this.canShow = value;
            if (!value)
            {
                base.gameObject.SetActive(false);
            }
            else
            {
                this.UpdateState(this.currPlayTime);
                this._CustomOperate(this.currPlayTime);
            }
        }
    }

    public float SpeedScale =>
        this.speedScale;

    public enum ElementPlayStyle
    {
        Once,
        Loop,
        Unreset
    }
}

