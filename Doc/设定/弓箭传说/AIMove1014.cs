using Dxx.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AIMove1014 : AIMoveBase
{
    private EntityBase target;
    private List<Grid.NodeItem> findpath;
    private Vector3 nextpos;
    private float startTime;
    private float time;

    public AIMove1014(EntityBase entity, int time) : base(entity)
    {
        this.time = ((float) time) / 1000f;
    }

    private void AIMoveStart()
    {
        this.UpdateMoveData();
        this.m_MoveData.action = "Skill";
        GameLogic.Hold.Sound.PlayMonsterSkill(0x4dd1e1, base.m_Entity.position);
        base.m_Entity.m_MoveCtrl.AIMoveStart(base.m_MoveData);
    }

    private void AIMoving()
    {
        base.m_Entity.m_MoveCtrl.AIMoving(base.m_MoveData);
    }

    private void Find()
    {
        if (this.target != null)
        {
            this.findpath = GameLogic.Release.Path.FindingPath(base.m_Entity.position, this.target.position);
            if (this.findpath.Count > 0)
            {
                Grid.NodeItem item = this.findpath[0];
                this.nextpos = GameLogic.Release.MapCreatorCtrl.GetWorldPosition(item.x, item.y);
                this.UpdateDirection();
            }
        }
    }

    private void MoveNormal()
    {
        this.UpdateFindPath();
        this.AIMoving();
    }

    protected override void OnEnd()
    {
        base.m_Entity.m_MoveCtrl.AIMoveEnd(base.m_MoveData);
    }

    protected override void OnInitBase()
    {
        this.target = GameLogic.Self;
        this.Find();
        this.startTime = Updater.AliveTime;
        this.AIMoveStart();
    }

    protected override void OnUpdate()
    {
        this.MoveNormal();
    }

    private void UpdateDirection()
    {
        float x = this.nextpos.x - base.m_Entity.position.x;
        float z = this.nextpos.z - base.m_Entity.position.z;
        Vector3 vector3 = new Vector3(x, 0f, z);
        vector3 = vector3.normalized * 5f;
        this.m_MoveData.angle = Utils.getAngle(x, z);
        this.m_MoveData.direction = vector3;
        base.m_Entity.m_AttackCtrl.RotateHero(this.m_MoveData.angle);
    }

    private void UpdateFindPath()
    {
        if ((Updater.AliveTime - this.startTime) > this.time)
        {
            base.End();
        }
    }

    private void UpdateMoveData()
    {
        if (this.target != null)
        {
            if (this.findpath.Count > 0)
            {
                this.UpdateDirection();
                Vector3 vector = this.nextpos - base.m_Entity.position;
                if (vector.magnitude < 0.2f)
                {
                    this.findpath.RemoveAt(0);
                    if (this.findpath.Count > 0)
                    {
                        Grid.NodeItem item = this.findpath[0];
                        this.nextpos = GameLogic.Release.MapCreatorCtrl.GetWorldPosition(item.x, item.y);
                        this.UpdateDirection();
                    }
                }
            }
            else
            {
                this.nextpos = this.target.position;
                this.UpdateDirection();
            }
        }
    }
}

