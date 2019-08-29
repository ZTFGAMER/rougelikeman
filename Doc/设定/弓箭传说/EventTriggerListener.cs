using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventTriggerListener : EventTrigger
{
    public VoidDelegate onClick;
    public VoidDelegate onDown;
    public VoidDelegate onEnter;
    public VoidDelegate onExit;
    public VoidDelegate onUp;
    public VoidDelegate onSelect;
    public VoidDelegate onUpdateSelect;

    public static EventTriggerListener Get(GameObject go)
    {
        EventTriggerListener component = go.GetComponent<EventTriggerListener>();
        if (component == null)
        {
            component = go.AddComponent<EventTriggerListener>();
        }
        return component;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (this.onClick != null)
        {
            this.onClick(base.gameObject);
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (this.onDown != null)
        {
            this.onDown(base.gameObject);
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (this.onEnter != null)
        {
            this.onEnter(base.gameObject);
        }
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if (this.onExit != null)
        {
            this.onExit(base.gameObject);
        }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (this.onUp != null)
        {
            this.onUp(base.gameObject);
        }
    }

    public override void OnSelect(BaseEventData eventData)
    {
        if (this.onSelect != null)
        {
            this.onSelect(base.gameObject);
        }
    }

    public override void OnUpdateSelected(BaseEventData eventData)
    {
        if (this.onUpdateSelect != null)
        {
            this.onUpdateSelect(base.gameObject);
        }
    }

    public delegate void VoidDelegate(GameObject go);
}

