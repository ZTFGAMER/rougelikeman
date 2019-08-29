using System;
using UnityEngine;

public class Bullet1047 : BulletBase
{
    protected override void AwakeInit()
    {
    }

    protected override void OnInit()
    {
        base.OnInit();
        base.Speed *= GameLogic.Random((float) 1f, (float) 1.5f);
        base.Parabola_MaxHeight = GameLogic.Random((float) 2f, (float) 5f);
        EntityBase base2 = GameLogic.FindTarget(base.m_Entity);
        base.PosFromStart2Target = GameLogic.Random((float) -3f, (float) 3f);
        if (base2 != null)
        {
            Vector3 vector = base2.position - base.m_Entity.position;
            base.PosFromStart2Target += vector.magnitude;
        }
    }
}

