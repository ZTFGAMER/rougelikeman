using Dxx.Util;
using System;
using UnityEngine;

public class WaveControl : MonoBehaviour
{
    private Transform child;
    private float speed = 5f;
    private float wavetime = 2f;
    private float currenttime;

    private void Start()
    {
        this.child = base.transform.Find("Plane");
    }

    private void Update()
    {
        this.currenttime += Updater.delta;
        this.child.localScale = new Vector3(this.currenttime * this.speed, 1f, this.currenttime * this.speed);
        if (this.currenttime >= this.wavetime)
        {
            Object.Destroy(base.gameObject);
        }
    }
}

