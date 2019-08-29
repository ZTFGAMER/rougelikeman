using Dxx.Util;
using System;
using UnityEngine;

public class CameraStartCtrl
{
    public Action OnEnd;
    public Camera camera;
    private float endsize;
    private float startsize;
    private bool bInit;
    private float time = 0.35f;
    private float starttime;

    public void Begin()
    {
        if (!this.bInit)
        {
            this.bInit = true;
            this.starttime = Updater.AliveTime;
            Updater.AddUpdate("CameraStartCtrl", new Action<float>(this.OnUpdate), false);
        }
    }

    public void DeInit()
    {
        if (this.bInit)
        {
            this.bInit = false;
            Updater.RemoveUpdate("CameraStartCtrl", new Action<float>(this.OnUpdate));
        }
    }

    private void OnUpdate(float delta)
    {
        float num = (Updater.AliveTime - this.starttime) / this.time;
        if (num < 1f)
        {
            this.camera.orthographicSize = ((this.endsize - this.startsize) * num) + this.startsize;
        }
        else
        {
            this.camera.orthographicSize = this.endsize;
            this.DeInit();
            if (this.OnEnd != null)
            {
                this.OnEnd();
            }
        }
    }

    public void SetCamera(Camera camera)
    {
        this.camera = camera;
        float num = (((float) GameLogic.DesignWidth) / ((float) GameLogic.DesignHeight)) / (((float) GameLogic.Width) / ((float) GameLogic.Height));
        this.startsize = 6f * num;
        this.endsize = 10.5f * num;
    }
}

