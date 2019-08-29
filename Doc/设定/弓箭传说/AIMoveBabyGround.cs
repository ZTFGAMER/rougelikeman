using Dxx.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AIMoveBabyGround : AIMoveBase
{
    private EntityBase mParent;
    private List<Grid.NodeItem> findpath;
    private Vector3 nextpos;
    private float findTime;
    private float findDelay;
    private bool bUpdateTime;
    private float updatetime;
    private float starttime;
    private float range;
    private float randomrange;
    private float movespeed;
    private int groundindex;

    public AIMoveBabyGround(EntityBase entity, int groundindex, float movespeed, float range) : base(entity)
    {
        this.bUpdateTime = true;
        this.groundindex = groundindex;
        this.range = range;
        this.movespeed = movespeed;
        if (entity is EntityCallBase)
        {
            EntityCallBase base2 = entity as EntityCallBase;
            if (base2 != null)
            {
                this.mParent = base2.GetParent();
            }
        }
    }

    private void AIMoveEnd()
    {
        base.End();
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
        if (this.mParent != null)
        {
            this.findpath = GameLogic.Release.Path.FindingPath(base.m_Entity.position, this.mParent.position + this.mParent.GetBabyGroundPos(this.groundindex));
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
        this.UpdateMoveData();
        this.AIMoving();
    }

    protected override void OnEnd()
    {
        base.m_Entity.m_MoveCtrl.AIMoveEnd(base.m_MoveData);
    }

    protected override void OnInitBase()
    {
        this.randomrange = GameLogic.Random(this.range * 0.8f, this.range);
        if ((this.mParent == null) || (Vector3.Distance(base.m_Entity.position, this.mParent.position) < this.randomrange))
        {
            base.End();
        }
        else
        {
            this.findDelay = GameLogic.Random((float) 0.5f, (float) 0.7f);
            this.Find();
            this.findTime = Updater.AliveTime;
            this.starttime = Updater.AliveTime;
            this.AIMoveStart();
        }
    }

    protected override void OnUpdate()
    {
        this.MoveNormal();
        if (Vector3.Distance(base.m_Entity.position, this.mParent.position) < this.randomrange)
        {
            base.End();
        }
    }

    private void UpdateDirection()
    {
        float x = this.nextpos.x - base.m_Entity.position.x;
        float z = this.nextpos.z - base.m_Entity.position.z;
        Vector3 normalized = new Vector3(x, 0f, z);
        normalized = normalized.normalized;
        this.m_MoveData.angle = Utils.getAngle(x, z);
        this.m_MoveData.direction = normalized * this.movespeed;
        base.m_Entity.m_AttackCtrl.RotateHero(this.m_MoveData.angle);
    }

    private void UpdateFindPath()
    {
        if ((Updater.AliveTime - this.findTime) > this.findDelay)
        {
            this.findTime = Updater.AliveTime;
            this.Find();
        }
    }

    private void UpdateMoveData()
    {
        if (this.mParent != null)
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
                base.End();
            }
        }
    }
}

