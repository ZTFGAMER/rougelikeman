using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SimpleTouchPad : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler, IEventSystemHandler
{
    public float smoothing;
    private Vector2 origin;
    private Vector2 direction;
    private Vector2 smoothDirection;
    private bool touched;
    private int pointerId;

    private void Awake()
    {
        this.direction = Vector2.zero;
        this.touched = false;
    }

    public Vector2 GetDirection()
    {
        this.smoothDirection = Vector2.MoveTowards(this.smoothDirection, this.direction, this.smoothing);
        return this.smoothDirection;
    }

    public void OnDrag(PointerEventData data)
    {
        if (this.pointerId == data.pointerId)
        {
            Vector2 vector2 = data.get_position() - this.origin;
            this.direction = vector2.normalized;
            Debug.Log(this.direction);
        }
    }

    public void OnPointerDown(PointerEventData data)
    {
        if (!this.touched)
        {
            this.touched = true;
            this.pointerId = data.pointerId;
            this.origin = data.get_position();
        }
    }

    public void OnPointerUp(PointerEventData data)
    {
        if (this.pointerId == data.pointerId)
        {
            this.direction = Vector2.zero;
            this.touched = false;
        }
    }
}

