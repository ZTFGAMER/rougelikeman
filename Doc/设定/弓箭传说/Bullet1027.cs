using System;
using UnityEngine;

public class Bullet1027 : Bullet1024
{
    private float rotateangle = 0.5f;

    protected override void OnInit()
    {
        base.OnInit();
    }

    protected override void OnUpdate()
    {
        base.bulletAngle += this.rotateangle;
        base.transform.rotation = Quaternion.Euler(0f, base.bulletAngle, 0f);
        base.CheckBulletLength();
    }
}

