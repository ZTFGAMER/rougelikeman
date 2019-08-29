using System;
using UnityEngine;

public class Bullet1093 : Bullet1076
{
    protected override void OnUpdate()
    {
        base.OnUpdate();
        base.transform.rotation = Quaternion.Euler(0f, base.transform.eulerAngles.y + 5f, 0f);
    }
}

