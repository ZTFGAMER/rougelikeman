using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainUIScrollRectInsideCtrl : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, IEventSystemHandler
{
    public ScrollRectBase anotherScrollRect;
    public bool thisIsUpAndDown = true;
    public Action Event_OnClick;
    private ScrollRectBase thisScrollRect;
    private bool bFirstDrag = true;

    private void Awake()
    {
        this.thisScrollRect = base.GetComponent<ScrollRectBase>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (this.anotherScrollRect != null)
        {
            this.anotherScrollRect.enabled = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (this.anotherScrollRect != null)
        {
            if (this.bFirstDrag)
            {
                float num = Vector2.Angle(eventData.get_delta(), Vector2.up);
                if ((num > 45f) && (num < 135f))
                {
                    this.thisScrollRect.enabled = !this.thisIsUpAndDown;
                    this.anotherScrollRect.enabled = this.thisIsUpAndDown;
                }
                else
                {
                    this.anotherScrollRect.enabled = !this.thisIsUpAndDown;
                    this.thisScrollRect.enabled = this.thisIsUpAndDown;
                }
            }
            if (this.thisScrollRect.enabled)
            {
                if (this.bFirstDrag)
                {
                    this.thisScrollRect.OnBeginDragInternal(eventData);
                    this.bFirstDrag = false;
                }
                this.thisScrollRect.OnDragInternal(eventData);
            }
            else
            {
                if (this.bFirstDrag)
                {
                    this.anotherScrollRect.OnBeginDragInternal(eventData);
                    this.bFirstDrag = false;
                }
                this.anotherScrollRect.OnDragInternal(eventData);
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (this.anotherScrollRect != null)
        {
            if (this.thisScrollRect.enabled)
            {
                this.thisScrollRect.OnEndDragInternal(eventData);
            }
            else
            {
                this.anotherScrollRect.OnEndDragInternal(eventData);
            }
            this.anotherScrollRect.enabled = true;
            this.thisScrollRect.enabled = true;
            this.bFirstDrag = true;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (this.Event_OnClick != null)
        {
            this.Event_OnClick();
        }
    }
}

