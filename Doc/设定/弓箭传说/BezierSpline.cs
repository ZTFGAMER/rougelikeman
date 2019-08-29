using System;
using UnityEngine;

public class BezierSpline : MonoBehaviour
{
    [SerializeField]
    private Vector3[] points;
    [SerializeField]
    private BezierControlPointMode[] modes;
    [SerializeField]
    private bool loop;

    public void AddCurve()
    {
        Vector3 vector = this.points[this.points.Length - 1];
        Array.Resize<Vector3>(ref this.points, this.points.Length + 3);
        vector.x++;
        this.points[this.points.Length - 3] = vector;
        vector.x++;
        this.points[this.points.Length - 2] = vector;
        vector.x++;
        this.points[this.points.Length - 1] = vector;
        Array.Resize<BezierControlPointMode>(ref this.modes, this.modes.Length + 1);
        this.modes[this.modes.Length - 1] = this.modes[this.modes.Length - 2];
        this.EnforceMode(this.points.Length - 4);
        if (this.loop)
        {
            this.points[this.points.Length - 1] = this.points[0];
            this.modes[this.modes.Length - 1] = this.modes[0];
            this.EnforceMode(0);
        }
    }

    private void EnforceMode(int index)
    {
        int num = (index + 1) / 3;
        BezierControlPointMode mode = this.modes[num];
        if ((mode != BezierControlPointMode.Free) && (this.loop || ((num != 0) && (num != (this.modes.Length - 1)))))
        {
            int num3;
            int num4;
            int num2 = num * 3;
            if (index <= num2)
            {
                num3 = num2 - 1;
                if (num3 < 0)
                {
                    num3 = this.points.Length - 2;
                }
                num4 = num2 + 1;
                if (num4 >= this.points.Length)
                {
                    num4 = 1;
                }
            }
            else
            {
                num3 = num2 + 1;
                if (num3 >= this.points.Length)
                {
                    num3 = 1;
                }
                num4 = num2 - 1;
                if (num4 < 0)
                {
                    num4 = this.points.Length - 2;
                }
            }
            Vector3 a = this.points[num2];
            Vector3 vector2 = a - this.points[num3];
            if (mode == BezierControlPointMode.Aligned)
            {
                vector2 = vector2.normalized * Vector3.Distance(a, this.points[num4]);
            }
            this.points[num4] = a + vector2;
        }
    }

    public Vector3 GetControlPoint(int index) => 
        this.points[index];

    public BezierControlPointMode GetControlPointMode(int index) => 
        this.modes[(index + 1) / 3];

    public Vector3 GetDirection(float t) => 
        this.GetVelocity(t).normalized;

    public Vector3 GetPoint(float t)
    {
        int num;
        if (t >= 1f)
        {
            t = 1f;
            num = this.points.Length - 4;
        }
        else
        {
            t = Mathf.Clamp01(t) * this.CurveCount;
            num = (int) t;
            t -= num;
            num *= 3;
        }
        return base.transform.TransformPoint(Bezier.GetPoint(this.points[num], this.points[num + 1], this.points[num + 2], this.points[num + 3], t));
    }

    public Vector3 GetVelocity(float t)
    {
        int num;
        if (t >= 1f)
        {
            t = 1f;
            num = this.points.Length - 4;
        }
        else
        {
            t = Mathf.Clamp01(t) * this.CurveCount;
            num = (int) t;
            t -= num;
            num *= 3;
        }
        return (base.transform.TransformPoint(Bezier.GetFirstDerivative(this.points[num], this.points[num + 1], this.points[num + 2], this.points[num + 3], t)) - base.transform.position);
    }

    public void Reset()
    {
        this.points = new Vector3[] { new Vector3(1f, 0f, 0f), new Vector3(2f, 0f, 0f), new Vector3(3f, 0f, 0f), new Vector3(4f, 0f, 0f) };
        this.modes = new BezierControlPointMode[2];
    }

    public void SetControlPoint(int index, Vector3 point)
    {
        if ((index % 3) == 0)
        {
            Vector3 vector = point - this.points[index];
            if (this.loop)
            {
                if (index == 0)
                {
                    this.points[1] += vector;
                    this.points[this.points.Length - 2] += vector;
                    this.points[this.points.Length - 1] = point;
                }
                else if (index == (this.points.Length - 1))
                {
                    this.points[0] = point;
                    this.points[1] += vector;
                    this.points[index - 1] += vector;
                }
                else
                {
                    this.points[index - 1] += vector;
                    this.points[index + 1] += vector;
                }
            }
            else
            {
                if (index > 0)
                {
                    this.points[index - 1] += vector;
                }
                if ((index + 1) < this.points.Length)
                {
                    this.points[index + 1] += vector;
                }
            }
        }
        this.points[index] = point;
        this.EnforceMode(index);
    }

    public void SetControlPointMode(int index, BezierControlPointMode mode)
    {
        int num = (index + 1) / 3;
        this.modes[num] = mode;
        if (this.loop)
        {
            if (num == 0)
            {
                this.modes[this.modes.Length - 1] = mode;
            }
            else if (num == (this.modes.Length - 1))
            {
                this.modes[0] = mode;
            }
        }
        this.EnforceMode(index);
    }

    public bool Loop
    {
        get => 
            this.loop;
        set
        {
            this.loop = value;
            if (value)
            {
                this.modes[this.modes.Length - 1] = this.modes[0];
                this.SetControlPoint(0, this.points[0]);
            }
        }
    }

    public int ControlPointCount =>
        this.points.Length;

    public int CurveCount =>
        ((this.points.Length - 1) / 3);
}

