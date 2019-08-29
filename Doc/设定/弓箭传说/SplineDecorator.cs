using System;
using UnityEngine;

public class SplineDecorator : MonoBehaviour
{
    public BezierSpline spline;
    public int frequency;
    public bool lookForward;
    public Transform[] items;

    private void Awake()
    {
        if (((this.frequency > 0) && (this.items != null)) && (this.items.Length != 0))
        {
            float num = this.frequency * this.items.Length;
            if (this.spline.Loop || (num == 1f))
            {
                num = 1f / num;
            }
            else
            {
                num = 1f / (num - 1f);
            }
            int num2 = 0;
            for (int i = 0; i < this.frequency; i++)
            {
                int index = 0;
                while (index < this.items.Length)
                {
                    Transform transform = Object.Instantiate<Transform>(this.items[index]);
                    Vector3 point = this.spline.GetPoint(num2 * num);
                    transform.transform.localPosition = point;
                    if (this.lookForward)
                    {
                        transform.transform.LookAt(point + this.spline.GetDirection(num2 * num));
                    }
                    transform.transform.parent = base.transform;
                    index++;
                    num2++;
                }
            }
        }
    }
}

