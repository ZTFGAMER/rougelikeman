using System;
using UnityEngine;

public class Bullet1046 : BulletBase
{
    protected override void AwakeInit()
    {
    }

    protected override void OnInit()
    {
        base.OnInit();
        base.Speed *= GameLogic.Random((float) 0.5f, (float) 1.2f);
        base.Parabola_MaxHeight = GameLogic.Random((float) 3f, (float) 8f);
        base.PosFromStart2Target = GameLogic.Random((float) 3f, (float) 7f);
        base.mTransform.localScale = Vector3.one * GameLogic.Random((float) 0.6f, (float) 1f);
        base.mTrailCtrl.UpdateTrailWidthScale(base.mTransform.localScale.x);
    }
}

