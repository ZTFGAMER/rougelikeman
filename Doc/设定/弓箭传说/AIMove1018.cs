using Dxx.Util;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class AIMove1018 : AIMoveBase
{
    private EntityBase target;
    private List<Grid.NodeItem> findpath;
    private Vector3 nextpos;
    private float findTime;
    private float findDelay;
    private bool bUpdateTime;
    private int min;
    private int max;

    public AIMove1018(EntityBase entity, int min = 0, int max = 0) : base(entity)
    {
        this.bUpdateTime = min > 0;
        if (this.bUpdateTime)
        {
            this.min = min;
            this.max = (max != 0) ? max : min;
        }
    }

    private void AIMoveStart()
    {
        this.UpdateMoveData();
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
        this.findDelay = 0.4f;
        this.Find();
        this.findTime = Updater.AliveTime;
        this.AIMoveStart();
        ConditionBase conditionRandomTime = AIMoveBase.GetConditionRandomTime(this.min, this.max);
        base.ConditionUpdate = new Func<bool>(conditionRandomTime.IsEnd);
    }

    protected override void OnUpdate()
    {
        this.MoveNormal();
    }

    private void UpdateDirection()
    {
        float x = this.nextpos.x - base.m_Entity.position.x;
        float z = this.nextpos.z - base.m_Entity.position.z;
        Vector3 normalized = new Vector3(x, 0f, z);
        normalized = normalized.normalized;
        this.m_MoveData.angle = Utils.getAngle(x, z);
        this.m_MoveData.direction = normalized;
        base.m_Entity.m_AttackCtrl.RotateHero(this.m_MoveData.angle);
    }

    private void UpdateFindPath()
    {
        if ((Updater.AliveTime - this.findTime) > this.findDelay)
        {
            this.Find();
            this.findTime = Updater.AliveTime;
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

