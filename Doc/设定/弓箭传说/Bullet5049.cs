using System;
using TableTool;

public class Bullet5049 : Bullet5046
{
    protected override void OnSetArgs()
    {
        base.onetime = 2.4f;
        base.movedis = 2.5f;
        base.curveId = (int) base.mArgs[0];
        base.curve = LocalModelManager.Instance.Curve_curve.GetCurve(base.curveId);
    }
}

