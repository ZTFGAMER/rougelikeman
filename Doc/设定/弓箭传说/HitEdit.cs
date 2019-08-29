using Dxx.Util;
using System;
using UnityEngine;

public class HitEdit : MonoBehaviour
{
    private EntityBase m_Entity;
    [Header("受击变白时间")]
    public float HittedWhiteTime = 0.3f;
    [Header("受击变白曲线")]
    public AnimationCurve HittedWhiteCurve;
    [Header("受击变白最大值")]
    public float HittedWhiteMax = 1f;
    private Animator m_AnimatorJelly;
    [Header("受击变形动画")]
    public EHittedScaleAnimation HittedScaleAnimation;
    private string mHittedScaleAnimation;
    private Animator m_AnimatorPosition;
    private EHittedPositionAnimation HittedPositionAnimation;
    private string mHittedPositionAnimation;
    private bool bPlayJelly;
    private int mPlayJellyFrame;
    private bool bPlayPosition;
    private int mPlayPositionFrame;

    public void DeInit()
    {
        this.DeInitHittedAnimation();
        object[] args = new object[] { this.m_Entity.m_EntityData.CharID };
        Updater.RemoveUpdate(Utils.FormatString("{0}.HitEdit", args), new Action<float>(this.OnUpdate));
    }

    private void DeInitHittedAnimation()
    {
        if (this.m_AnimatorJelly != null)
        {
            Object.Destroy(this.m_AnimatorJelly);
        }
        if (this.m_AnimatorPosition != null)
        {
            Object.Destroy(this.m_AnimatorPosition);
        }
    }

    public float GetHittedWhiteByTime(float time)
    {
        if (time < 0f)
        {
            return 1f;
        }
        if (time > this.HittedWhiteTime)
        {
            return 0f;
        }
        return (this.HittedWhiteCurve.Evaluate(time) * this.HittedWhiteMax);
    }

    public void HittedAnimationCallBack()
    {
        if ((this.m_AnimatorJelly != null) && (this.HittedScaleAnimation != EHittedScaleAnimation.eNone))
        {
            if (this.m_AnimatorJelly.GetCurrentAnimatorStateInfo(0).IsName(this.mHittedScaleAnimation))
            {
                this.bPlayJelly = true;
                this.mPlayJellyFrame = 0;
            }
            else
            {
                this.m_AnimatorJelly.Play(this.mHittedScaleAnimation);
            }
        }
    }

    public void HittedPosAni()
    {
        if ((this.m_AnimatorPosition != null) && (this.HittedPositionAnimation != EHittedPositionAnimation.eNone))
        {
            if (this.m_AnimatorPosition.GetCurrentAnimatorStateInfo(0).IsName(this.mHittedPositionAnimation))
            {
                this.bPlayPosition = true;
                this.mPlayPositionFrame = 0;
            }
            else
            {
                this.m_AnimatorPosition.Play(this.mHittedPositionAnimation);
            }
        }
    }

    public void Init(EntityBase entity)
    {
        this.m_Entity = entity;
        this.InitHittedWhiteCurve();
        this.InitHittedAnimation();
        object[] args = new object[] { this.m_Entity.m_EntityData.CharID };
        Updater.AddUpdate(Utils.FormatString("{0}.HitEdit", args), new Action<float>(this.OnUpdate), false);
    }

    private void InitHittedAnimation()
    {
        if (!this.m_Entity.IsSelf)
        {
            this.m_AnimatorJelly = this.m_Entity.m_Body.gameObject.GetComponent<Animator>();
            if (this.m_AnimatorJelly == null)
            {
                this.m_AnimatorJelly = this.m_Entity.m_Body.gameObject.AddComponent<Animator>();
                this.m_AnimatorJelly.runtimeAnimatorController = ResourceManager.Load<RuntimeAnimatorController>("Game/Animator/Jelly");
            }
            this.mHittedScaleAnimation = this.HittedScaleAnimation.ToString();
            this.m_AnimatorPosition = this.m_Entity.m_Body.RotateMask.GetComponent<Animator>();
            if (this.m_AnimatorPosition == null)
            {
                this.m_AnimatorPosition = this.m_Entity.m_Body.RotateMask.AddComponent<Animator>();
                this.m_AnimatorPosition.runtimeAnimatorController = ResourceManager.Load<RuntimeAnimatorController>("Game/Animator/PosAni");
            }
            this.mHittedPositionAnimation = this.HittedPositionAnimation.ToString();
        }
    }

    private void InitHittedWhiteCurve()
    {
        int length = this.HittedWhiteCurve.length;
        object[] args = new object[] { this.m_Entity.m_EntityData.CharID };
        SdkManager.Bugly_Report(length > 0, "HitEdit.cs", Utils.FormatString("EntityID:{0}, HitEdit.HittedWhiteCurve.length <= 0", args));
        Keyframe keyframe = this.HittedWhiteCurve[length - 1];
        float time = keyframe.time;
        float scale = this.HittedWhiteTime / time;
        if (scale > 1f)
        {
            for (int i = length - 1; i >= 0; i--)
            {
                this.MoveKey(i, scale, this.HittedWhiteCurve[i]);
            }
        }
        else if (scale < 1f)
        {
            for (int i = 0; i < length; i++)
            {
                this.MoveKey(i, scale, this.HittedWhiteCurve[i]);
            }
        }
    }

    public bool IsHittedWhiteEnd(float time) => 
        (time >= this.HittedWhiteTime);

    private void MoveKey(int index, float scale, Keyframe keyframe)
    {
        Keyframe key = new Keyframe {
            time = keyframe.time * scale,
            value = keyframe.value,
            inTangent = keyframe.inTangent,
            outTangent = keyframe.outTangent,
            tangentMode = keyframe.tangentMode
        };
        this.HittedWhiteCurve.MoveKey(index, key);
    }

    private void OnDisable()
    {
        if (this.m_AnimatorJelly != null)
        {
            this.m_AnimatorJelly.enabled = false;
        }
        if (this.m_AnimatorPosition != null)
        {
            this.m_AnimatorPosition.enabled = false;
        }
    }

    private void OnEnable()
    {
        if (this.m_AnimatorJelly != null)
        {
            this.m_AnimatorJelly.enabled = true;
        }
        if (this.m_AnimatorPosition != null)
        {
            this.m_AnimatorPosition.enabled = true;
        }
    }

    private void OnUpdate(float delta)
    {
        if (this.bPlayJelly && (this.m_AnimatorJelly != null))
        {
            if (this.mPlayJellyFrame == 0)
            {
                this.m_AnimatorJelly.Play("Normal");
            }
            else if (this.mPlayJellyFrame == 2)
            {
                this.m_AnimatorJelly.Play(this.mHittedScaleAnimation);
                this.bPlayJelly = false;
            }
            this.mPlayJellyFrame++;
        }
        if (this.bPlayPosition && (this.m_AnimatorPosition != null))
        {
            if (this.mPlayPositionFrame == 0)
            {
                this.m_AnimatorPosition.Play("Normal");
            }
            else if (this.mPlayPositionFrame == 2)
            {
                this.m_AnimatorPosition.Play(this.mHittedPositionAnimation);
                this.bPlayPosition = false;
            }
            this.mPlayPositionFrame++;
        }
    }

    public enum EHittedPositionAnimation
    {
        eNone,
        pos_likestone
    }

    public enum EHittedScaleAnimation
    {
        eNone,
        jelly_likebat,
        jelly_likeflower,
        jelly_likestone,
        jelly_likeghost,
        jelly_likestonesmall1,
        jelly_likestonesmall2
    }
}

