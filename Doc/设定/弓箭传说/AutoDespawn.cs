using Dxx.Util;
using System;
using UnityEngine;

public class AutoDespawn : MonoBehaviour
{
    public float DespawnTime = 2f;
    private float pDespawnTime;
    private bool bStart;

    private void OnEnable()
    {
        this.pDespawnTime = this.DespawnTime;
        this.bStart = true;
    }

    public void SetDespawnTime(float value)
    {
        this.DespawnTime = value;
        this.pDespawnTime = this.DespawnTime;
    }

    private void Update()
    {
        if (this.bStart)
        {
            this.pDespawnTime -= Updater.delta;
            if (this.pDespawnTime <= 0f)
            {
                this.bStart = false;
                GameLogic.EffectCache(base.gameObject);
            }
        }
    }
}

