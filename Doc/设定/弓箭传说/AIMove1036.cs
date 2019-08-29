using Dxx.Util;
using System;
using UnityEngine;

public class AIMove1036 : AIMove1008
{
    private float perattacktime;
    private float attackcurrenttime;

    public AIMove1036(EntityBase entity, float move2playerratio, int time) : base(entity, move2playerratio, time)
    {
        this.perattacktime = 0.3f;
    }

    private void CreateBullet()
    {
        GameLogic.Release.Bullet.CreateBullet(base.m_Entity, 0x13a6, base.m_Entity.position + new Vector3(0f, 1f, 0f), (float) GameLogic.Random(0, 360));
    }

    protected override void OnInitBase()
    {
        base.OnInitBase();
        this.attackcurrenttime = 0f;
    }

    protected override void OnUpdate()
    {
        this.attackcurrenttime += Updater.delta;
        if (this.attackcurrenttime >= this.perattacktime)
        {
            this.attackcurrenttime -= this.perattacktime;
            this.CreateBullet();
        }
        base.OnUpdate();
    }
}

