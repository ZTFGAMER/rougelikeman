using System;
using UnityEngine;

public class AdCamera : MonoBehaviour
{
    public SimpleTouchPad touchPad;
    public float rotationSpeed = 10f;
    public float speed = 10f;
    public GameObject target;
    private float currentTranslation;
    private Vector3 point;

    private void FixedUpdate()
    {
        Vector2 direction = this.touchPad.GetDirection();
        float num = direction.y * this.speed;
        if (this.currentTranslation != num)
        {
            this.currentTranslation = num;
            base.transform.RotateAround(this.point, new Vector3(0f, 1f, 0f), 5f * ((direction.y < 0f) ? -1f : 1f));
        }
    }

    private void Start()
    {
        this.point = this.target.transform.position;
        base.transform.LookAt(this.point);
    }
}

