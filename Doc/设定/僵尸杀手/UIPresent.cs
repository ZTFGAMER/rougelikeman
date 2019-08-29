using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIPresent : MonoBehaviour
{
    protected UIPresent()
    {
    }

    public abstract string GetSkipEventName();
    public void GoToGameOver()
    {
        DataLoader.gui.ChangeAnimationState("GameOver");
    }

    public virtual void OnEscape()
    {
        this.Skip();
    }

    public abstract void SetContent(int money);
    public void Skip()
    {
        this.GoToGameOver();
        AnalyticsManager.instance.LogEvent(this.GetSkipEventName(), new Dictionary<string, string>());
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && base.gameObject.activeInHierarchy)
        {
            this.OnEscape();
        }
    }
}

