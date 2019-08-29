using System;
using UnityEngine;

public class AIMove1056 : AIMove1008
{
    private float createdis;
    private float currentdis;
    private float angle;

    public AIMove1056(EntityBase entity, int time, float move2playerratio, float createdis) : base(entity, move2playerratio, time)
    {
        this.createdis = createdis;
    }

    private void CreateBullet()
    {
        GameLogic.Release.Bullet.CreateBullet(base.m_Entity, 0x422, base.m_Entity.m_Body.EffectMask.transform.position, this.angle);
    }

    protected override void OnEnd()
    {
        base.OnEnd();
        base.m_Entity.Event_PositionBy -= new Action<Vector3>(this.OnMoveBy);
    }

    protected override void OnInitBase()
    {
        base.OnInitBase();
        this.currentdis = 0f;
        this.angle = GameLogic.Random((float) 0f, (float) 360f);
        base.m_Entity.Event_PositionBy += new Action<Vector3>(this.OnMoveBy);
    }

    private void OnMoveBy(Vector3 move)
    {
        this.currentdis += move.magnitude;
        if (this.currentdis >= this.createdis)
        {
            this.currentdis -= this.createdis;
            this.CreateBullet();
            this.angle += GameLogic.Random((float) 70f, (float) 110f);
        }
    }
}

