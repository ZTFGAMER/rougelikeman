using Dxx.Util;
using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class AIMove1031 : AIMoveBase
{
    private EntityBase target;
    public float peradd;
    private float maxadd;
    private float offsetangle;
    private int state;
    private float flytime;
    private float startflytime;

    public AIMove1031(EntityBase entity, float flytime = -1f) : base(entity)
    {
        this.peradd = 1f;
        this.maxadd = 50f;
        this.flytime = flytime;
    }

    protected override void OnEnd()
    {
        base.m_Entity.m_MoveCtrl.AIMoveEnd(base.m_MoveData);
    }

    protected override void OnInitBase()
    {
        this.state = GameLogic.Random(0, 2);
        this.startflytime = Updater.AliveTime;
        this.target = GameLogic.Self;
        this.UpdateDirection();
        base.m_Entity.m_MoveCtrl.AIMoveStart(base.m_MoveData);
    }

    protected override void OnUpdate()
    {
        this.UpdateDirection();
        base.m_Entity.m_MoveCtrl.AIMoving(base.m_MoveData);
        if ((this.flytime > 0f) && ((Updater.AliveTime - this.startflytime) > this.flytime))
        {
            base.End();
        }
    }

    private void UpdateDirection()
    {
        float x = this.target.position.x - base.m_Entity.position.x;
        float y = this.target.position.z - base.m_Entity.position.z;
        float num3 = Utils.getAngle(x, y);
        if (this.state == 0)
        {
            this.offsetangle += this.peradd;
            if (MathDxx.Abs(this.offsetangle) >= this.maxadd)
            {
                this.state = 1;
            }
        }
        else
        {
            this.offsetangle -= this.peradd;
            if (MathDxx.Abs(this.offsetangle) >= this.maxadd)
            {
                this.state = 0;
            }
        }
        this.m_MoveData.angle = num3 + this.offsetangle;
        this.m_MoveData.direction = new Vector3(MathDxx.Sin(this.m_MoveData.angle), 0f, MathDxx.Cos(this.m_MoveData.angle));
        base.m_Entity.m_AttackCtrl.RotateHero(this.m_MoveData.angle);
    }
}

