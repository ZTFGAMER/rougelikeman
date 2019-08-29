using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemCtrl : MonoBehaviour
{
    public static EventSystemCtrl Instance;
    private EventSystem mEventSystem;
    private int defaultDragThreshold;
    private bool bEnable = true;

    public void SetDragEnable(bool value)
    {
        if (this.bEnable != value)
        {
            this.bEnable = value;
            if (value)
            {
                this.mEventSystem.pixelDragThreshold = this.defaultDragThreshold;
            }
            else
            {
                this.mEventSystem.pixelDragThreshold = 0x7fffffff;
            }
        }
    }

    private void Start()
    {
        this.mEventSystem = EventSystem.current;
        this.mEventSystem.pixelDragThreshold = (int) (this.mEventSystem.pixelDragThreshold * GameLogic.WidthScale);
        Instance = this;
        this.defaultDragThreshold = this.mEventSystem.pixelDragThreshold;
    }
}

