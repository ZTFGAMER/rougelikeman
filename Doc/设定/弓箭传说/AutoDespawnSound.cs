using System;
using UnityEngine;

public class AutoDespawnSound : MonoBehaviour
{
    public float DespawnTime = 1f;
    public SoundManager.SoundData sounddata;
    public Action<string, SoundManager.SoundData> callback;
    private float pDespawnTime;
    private bool bStart;

    private void OnEnable()
    {
        this.pDespawnTime = this.DespawnTime;
        this.bStart = true;
    }

    public void SetDespawnTime(float time)
    {
        this.DespawnTime = time;
        this.OnEnable();
    }

    private void Update()
    {
        if (this.bStart)
        {
            this.pDespawnTime -= Time.unscaledDeltaTime;
            if (this.pDespawnTime <= 0f)
            {
                this.bStart = false;
                if (this.callback != null)
                {
                    this.callback(base.name, this.sounddata);
                }
                else
                {
                    Object.Destroy(base.gameObject);
                }
            }
        }
    }
}

