using System;
using UnityEngine;

public class BezierCurve : MonoBehaviour
{
    public Vector3[] points;

    public Vector3 GetDirection(float t) => 
        this.GetVelocity(t).normalized;

    public Vector3 GetPoint(float t) => 
        base.transform.TransformPoint(Bezier.GetPoint(this.points[0], this.points[1], this.points[2], this.points[3], t));

    public Vector3 GetVelocity(float t) => 
        (base.transform.TransformPoint(Bezier.GetFirstDerivative(this.points[0], this.points[1], this.points[2], this.points[3], t)) - base.transform.position);

    public void Reset()
    {
        this.points = new Vector3[] { new Vector3(1f, 0f, 0f), new Vector3(2f, 0f, 0f), new Vector3(3f, 0f, 0f), new Vector3(4f, 0f, 0f) };
    }
}

