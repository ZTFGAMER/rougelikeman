using Dxx.Util;
using System;
using UnityEngine;

public class AutoDisable : MonoBehaviour
{
    public float DisableTime = 1f;
    private float pDisableTime;
    private bool bStart;
    private SphereCollider mSphere;
    private BoxCollider mBox;
    private CapsuleCollider mCapsule;

    private void ColliderEnable(bool enable)
    {
        if (this.mSphere == null)
        {
            this.mSphere = base.GetComponent<SphereCollider>();
        }
        if (this.mSphere != null)
        {
            this.mSphere.enabled = enable;
        }
    }

    private void OnEnable()
    {
        this.pDisableTime = this.DisableTime;
        this.bStart = true;
        this.ColliderEnable(true);
    }

    private void Update()
    {
        if (this.bStart)
        {
            this.pDisableTime -= Updater.delta;
            if (this.pDisableTime <= 0f)
            {
                this.bStart = false;
                this.ColliderEnable(false);
            }
        }
    }
}

