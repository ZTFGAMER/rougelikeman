using Dxx.Util;
using System;
using UnityEngine;

public class ColliderEnable : MonoBehaviour
{
    private float mTime;
    private float starttime;
    private bool bExcute;
    private BoxCollider mBox;
    private SphereCollider mSphere;
    private CapsuleCollider mCapsule;
    public float DelayTime;
    [Tooltip("延迟后可否碰撞")]
    public bool bDelayEnable;

    private void Awake()
    {
        this.mBox = base.GetComponent<BoxCollider>();
        this.mSphere = base.GetComponent<SphereCollider>();
        this.mCapsule = base.GetComponent<CapsuleCollider>();
    }

    private void OnEnable()
    {
        this.mTime = this.DelayTime;
        this.starttime = Updater.AliveTime;
        this.bExcute = false;
        this.SetColliderEnable(!this.bDelayEnable);
    }

    private void SetColliderEnable(bool enable)
    {
        if (this.mBox != null)
        {
            this.mBox.enabled = enable;
        }
        if (this.mSphere != null)
        {
            this.mSphere.enabled = enable;
        }
        if (this.mCapsule != null)
        {
            this.mCapsule.enabled = enable;
        }
    }

    private void Update()
    {
        if (!this.bExcute && ((Updater.AliveTime - this.starttime) >= this.mTime))
        {
            this.bExcute = true;
            this.SetColliderEnable(this.bDelayEnable);
        }
    }
}

