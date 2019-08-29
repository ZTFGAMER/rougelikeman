using System;
using UnityEngine;

public class SplineWalker : MonoBehaviour
{
    public BezierSpline spline;
    public float duration;
    public bool lookForward;
    public SplineWalkerMode mode;
    private float progress;
    private bool goingForward = true;

    private void Update()
    {
        if (this.goingForward)
        {
            this.progress += Time.deltaTime / this.duration;
            if (this.progress > 1f)
            {
                if (this.mode == SplineWalkerMode.Once)
                {
                    this.progress = 1f;
                }
                else if (this.mode == SplineWalkerMode.Loop)
                {
                    this.progress--;
                }
                else
                {
                    this.progress = 2f - this.progress;
                    this.goingForward = false;
                }
            }
        }
        else
        {
            this.progress -= Time.deltaTime / this.duration;
            if (this.progress < 0f)
            {
                this.progress = -this.progress;
                this.goingForward = true;
            }
        }
        Vector3 vector = base.transform.parent.InverseTransformPoint(this.spline.GetPoint(this.progress));
        base.transform.localPosition = vector;
        if (this.lookForward)
        {
            base.transform.LookAt(vector + this.spline.GetDirection(this.progress));
        }
    }
}

