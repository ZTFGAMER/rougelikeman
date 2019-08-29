using Dxx.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AIMove1022 : AIMoveBase
{
    private EntityBase target;
    private List<Grid.NodeItem> findpath;
    private Vector3 nextpos;
    private float findTime;
    private float findDelay;
    private Vector3 mDirection;
    private float addspeed;
    private float maxspeed;
    private float movetime;
    private bool bNear;
    private float neardelaytime;
    private float NearPos;
    private Vector3 dir;

    public AIMove1022(EntityBase entity, float nearpos) : base(entity)
    {
        this.addspeed = 0.3f;
        this.maxspeed = 2f;
        this.NearPos = nearpos;
    }

    private void AIMoveEnd()
    {
        base.m_Entity.m_MoveCtrl.AIMoveEnd(base.m_MoveData);
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

    private bool IsNear()
    {
        Vector3 vector;
        this.dir = this.target.position - base.m_Entity.position;
        bool flag = Physics.Raycast(base.m_Entity.position, this.dir, this.dir.magnitude, (int) ((((int) 1) << LayerManager.Stone) | (((int) 1) << LayerManager.Waters)));
        if (this.target != null)
        {
            vector = this.target.position - base.m_Entity.position;
        }
        return ((vector.magnitude < this.NearPos) && !flag);
    }

    private void MoveNormal()
    {
        this.movetime += Updater.delta;
        this.UpdateFindPath();
        this.UpdateMoveData();
        this.UpdateMoveSpeed();
        this.AIMoving();
    }

    protected override void OnEnd()
    {
    }

    protected override void OnInitBase()
    {
        this.target = GameLogic.Self;
        this.bNear = false;
        this.movetime = 0f;
        this.neardelaytime = 0.2f;
        this.findDelay = GameLogic.Random((float) 0.5f, (float) 0.7f);
        this.Find();
        this.findTime = Updater.AliveTime;
        this.AIMoveStart();
    }

    protected override void OnUpdate()
    {
        if (this.bNear)
        {
            this.neardelaytime -= Updater.delta;
            if (this.neardelaytime <= 0f)
            {
                base.End();
            }
        }
        else if (this.IsNear())
        {
            this.AIMoveEnd();
            this.bNear = true;
        }
        else
        {
            this.MoveNormal();
        }
    }

    private void UpdateDirection()
    {
        float x = this.nextpos.x - base.m_Entity.position.x;
        float z = this.nextpos.z - base.m_Entity.position.z;
        Vector3 normalized = new Vector3(x, 0f, z);
        normalized = normalized.normalized;
        this.m_MoveData.angle = Utils.getAngle(x, z);
        this.m_MoveData.direction = normalized;
        this.m_MoveData._moveDirection = normalized;
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
        if (this.target != null)
        {
            if (this.findpath.Count > 0)
            {
                Grid.NodeItem item = this.findpath[0];
                this.nextpos = GameLogic.Release.MapCreatorCtrl.GetWorldPosition(item.x, item.y);
                this.UpdateDirection();
                Vector3 vector = this.nextpos - base.m_Entity.position;
                if (vector.magnitude < 0.5f)
                {
                    this.findpath.RemoveAt(0);
                }
            }
            else
            {
                this.nextpos = this.target.position;
                this.UpdateDirection();
            }
        }
    }

    private void UpdateMoveSpeed()
    {
        float num = (this.movetime * this.addspeed) + 1f;
        num = MathDxx.Clamp(num, num, this.maxspeed);
        this.m_MoveData._moveDirection = this.m_MoveData._moveDirection.normalized * num;
    }
}

