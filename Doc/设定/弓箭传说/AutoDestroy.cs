using Dxx.Util;
using System;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float DestroyTime = 1f;
    private float pDestroyTime;
    private bool bStart;

    private void OnEnable()
    {
        this.pDestroyTime = this.DestroyTime;
        this.bStart = true;
    }

    public void SetDestroyTime(float time)
    {
        this.DestroyTime = time;
        this.OnEnable();
    }

    private void Update()
    {
        if (this.bStart)
        {
            this.pDestroyTime -= Updater.delta;
            if (this.pDestroyTime <= 0f)
            {
                this.bStart = false;
                Object.Destroy(base.gameObject);
            }
        }
    }
}

