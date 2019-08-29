using Dxx.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AIMove1023 : AIMoveBase
{
    private EntityBase target;
    private List<Grid.NodeItem> findpath;
    private Vector3 nextpos;
    private Vector3 endpos;
    private int range;

    public AIMove1023(EntityBase entity, int range) : base(entity)
    {
        this.range = range;
    }

    private void AIMoveStart()
    {
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
            this.findpath = GameLogic.Release.Path.FindingPath(base.m_Entity.position, this.endpos);
            if ((this.findpath != null) && (this.findpath.Count > 0))
            {
                Grid.NodeItem item = this.findpath[0];
                this.nextpos = GameLogic.Release.MapCreatorCtrl.GetWorldPosition(item.x, item.y);
                this.UpdateDirection();
            }
        }
    }

    private void MoveNormal()
    {
        this.UpdateMoveData();
        this.AIMoving();
    }

    protected override void OnEnd()
    {
        base.m_Entity.m_MoveCtrl.AIMoveEnd(base.m_MoveData);
    }

    protected override void OnInitBase()
    {
        GameLogic.Release.MapCreatorCtrl.RandomItem(base.m_Entity, this.range, out float num, out float num2);
        this.endpos = new Vector3(num, 0f, num2);
        this.target = GameLogic.Self;
        if (this.target == null)
        {
            base.End();
        }
        else
        {
            this.Find();
            if (this.findpath == null)
            {
                base.End();
            }
            else
            {
                this.AIMoveStart();
            }
        }
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

    private void UpdateMoveData()
    {
        if (this.target == null)
        {
            base.End();
        }
        else if ((this.findpath != null) && (this.findpath.Count > 0))
        {
            this.UpdateDirection();
            Vector3 vector = this.nextpos - base.m_Entity.position;
            if (vector.magnitude < 0.5f)
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
            base.End();
        }
    }
}

