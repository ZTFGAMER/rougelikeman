using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class Bullet1003 : BulletBase
{
    protected override void AwakeInit()
    {
        base.Parabola_MaxHeight = 4f;
    }

    protected override void OnDeInit()
    {
        base.OnDeInit();
    }

    protected override void OnInit()
    {
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        base.Speed += 0.1f;
    }

    public override void SetTarget(EntityBase entity, int size = 1)
    {
        base.SetTarget(entity, size);
        if (base.PosFromStart2Target < 3f)
        {
            base.PosFromStart2Target = 3f;
        }
    }

    protected override void UpdateProcess()
    {
        base.UpdateProcess();
        if (base.childMesh != null)
        {
            base.childMesh.localRotation = Quaternion.Euler(0f, 0f, base.childMesh.localEulerAngles.z + 20f);
        }
    }

    protected override bool bFlyCantHit =>
        true;
}

