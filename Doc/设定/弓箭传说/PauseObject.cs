using System;
using UnityEngine;

public abstract class PauseObject : MonoBehaviour
{
    protected bool useLateUpdate;
    private int DeltaTime;

    protected PauseObject()
    {
    }

    private void Update()
    {
        if (!GameLogic.Paused && (Time.timeScale > 0f))
        {
            this.UpdateProcess();
        }
    }

    protected virtual void UpdateProcess()
    {
    }
}

