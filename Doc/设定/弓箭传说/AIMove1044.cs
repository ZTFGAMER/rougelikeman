using Dxx.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AIMove1044 : AIMoveBase
{
    private EntityBase target;
    private List<Grid.NodeItem> findpath;
    private Vector3 nextpos;
    private float starttime;
    private const float MOVEADD = 3f;
    private const float MOVEADD_TIME = 1f;
    private float mMoveAdd;
    private Vector2Int mMoveDir;
    private bool bSameLine;

    public AIMove1044(EntityBase entity) : base(entity)
    {
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
            Vector3 e = GameLogic.Release.MapCreatorCtrl.RandomPosition();
            this.findpath = GameLogic.Release.Path.FindingPath(base.m_Entity.position, e);
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
        base.m_Entity.m_AniCtrl.SendEvent("Idle", true);
    }

    protected override void OnInitBase()
    {
        this.target = GameLogic.Self;
        this.bSameLine = false;
        this.mMoveAdd = 1f;
        this.Find();
        this.AIMoveStart();
    }

    protected override void OnUpdate()
    {
        if (!this.bSameLine)
        {
            Vector2Int roomXY = GameLogic.Release.MapCreatorCtrl.GetRoomXY(base.m_Entity.position);
            Vector2Int to = GameLogic.Release.MapCreatorCtrl.GetRoomXY(this.target.position);
            if (GameLogic.Release.MapCreatorCtrl.ExcuteRelativeDirection(roomXY, to, out this.mMoveDir, base.m_Entity.GetFlying()))
            {
                this.bSameLine = true;
                this.UpdateSprint(this.mMoveDir);
            }
            else
            {
                this.MoveNormal();
            }
        }
        else if ((Updater.AliveTime - this.starttime) < 1f)
        {
            this.AIMoving();
            Vector2Int roomXY = GameLogic.Release.MapCreatorCtrl.GetRoomXY(base.m_Entity.position);
            if (!base.m_Entity.GetFlying())
            {
                if (!GameLogic.Release.MapCreatorCtrl.IsEmpty(roomXY + this.mMoveDir))
                {
                    base.End();
                }
            }
            else
            {
                Vector2Int v = roomXY + this.mMoveDir;
                if (!GameLogic.Release.MapCreatorCtrl.IsValid(v))
                {
                    base.End();
                }
            }
        }
        else
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
        this.m_MoveData.direction = normalized * this.mMoveAdd;
        base.m_Entity.m_AttackCtrl.RotateHero(this.m_MoveData.angle);
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
                this.Find();
            }
        }
    }

    private void UpdateSprint(Vector2Int dir)
    {
        this.starttime = Updater.AliveTime;
        base.m_Entity.m_AniCtrl.SendEvent("Continuous", false);
        this.mMoveAdd = 3f;
        Vector3 vector = new Vector3((float) dir.x, 0f, (float) dir.y);
        this.m_MoveData.angle = Utils.getAngle((float) dir.x, (float) dir.y);
        this.m_MoveData.direction = vector * this.mMoveAdd;
        base.m_Entity.m_AttackCtrl.RotateHero(this.m_MoveData.angle);
    }
}

