using System;
using TableTool;
using UnityEngine;

public class Bullet1063 : Bullet5046
{
    protected override void OnInit()
    {
        base.OnInit();
        base.bFlyRotate = false;
        base.mTransform.localRotation = Quaternion.identity;
        base.onetime = 2f;
        base.movedis = 2f;
        base.curveId = 0x186ab;
        base.curve = LocalModelManager.Instance.Curve_curve.GetCurve(base.curveId);
    }
}

