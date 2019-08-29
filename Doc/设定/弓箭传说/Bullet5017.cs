using System;
using UnityEngine;

public class Bullet5017 : Bullet8001
{
    private float angle;

    protected override void OnRotate()
    {
        this.angle += base.m_Data.RotateSpeed;
        base.childMesh.localRotation = Quaternion.Euler(this.angle, 0f, 0f);
    }
}

