using Dxx.Util;
using System;
using System.Runtime.InteropServices;
using UnityEngine;

[StructLayout(LayoutKind.Sequential)]
public struct JoyData
{
    public string name;
    public Vector3 direction;
    public Vector3 _moveDirection;
    public float angle;
    public float length;
    public int type;
    public string action;
    public Vector3 MoveDirection =>
        (!(this._moveDirection == Vector3.zero) ? this._moveDirection : this.direction);
    public void Revert()
    {
        this.direction *= -1f;
        this.angle = (this.angle + 180f) % 360f;
    }

    public void UpdateDirectionByAngle(float angle)
    {
        this.angle = angle;
        this.direction.x = MathDxx.Sin(angle);
        this.direction.z = MathDxx.Cos(angle);
    }
}

