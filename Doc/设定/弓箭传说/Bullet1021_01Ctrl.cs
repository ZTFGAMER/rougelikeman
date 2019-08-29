using Dxx.Util;
using System;
using UnityEngine;

public class Bullet1021_01Ctrl : MonoBehaviour
{
    public GameObject child;
    private float alltime = 1.4f;
    private float downtime = 1.2f;
    private float time;
    private int state;

    private void OnEnable()
    {
        this.state = 0;
        this.time = 0f;
        this.child.SetActive(true);
        this.child.transform.localPosition = new Vector3(0f, 0f, 15f);
        this.child.transform.localScale = Vector3.zero;
    }

    private void Start()
    {
    }

    private void Update()
    {
        this.time += Time.deltaTime;
        if (this.state == 0)
        {
            float num = MathDxx.Clamp(this.time, 0f, 0.3f) / 0.3f;
            this.child.transform.localScale = Vector3.one * num;
            if (this.time > this.downtime)
            {
                this.state = 1;
            }
        }
        else if (this.state == 1)
        {
            if (this.time > this.alltime)
            {
                this.time = this.alltime;
            }
            float num2 = (this.time - this.downtime) / (this.alltime - this.downtime);
            this.child.transform.localPosition = new Vector3(0f, 0f, 15f * (1f - num2));
            if (this.time >= this.alltime)
            {
                this.child.SetActive(false);
            }
        }
    }
}

