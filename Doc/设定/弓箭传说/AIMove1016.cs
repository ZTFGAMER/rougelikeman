using Dxx.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AIMove1016 : AIMoveBase
{
    private EntityBase target;
    private List<Grid.NodeItem> findpath;
    private Vector3 nextpos;

    public AIMove1016(EntityBase entity) : base(entity)
    {
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
        if (this.target != null)
        {
            List<Vector2Int> list = new List<Vector2Int>();
            Vector2Int roomXY = GameLogic.Release.MapCreatorCtrl.GetRoomXY(this.target.position);
            int[,] findPathRect = GameLogic.Release.MapCreatorCtrl.GetFindPathRect();
            int width = GameLogic.Release.MapCreatorCtrl.width;
            int height = GameLogic.Release.MapCreatorCtrl.height;
            if ((roomXY.x <= (((float) width) / 2f)) && (roomXY.y <= (((float) height) / 2f)))
            {
                int x = (width / 2) + 1;
                int num5 = width;
                while (x < num5)
                {
                    int y = (height / 2) + 1;
                    int num7 = height;
                    while (y < num7)
                    {
                        if (findPathRect[x, y] == 0)
                        {
                            list.Add(new Vector2Int(x, y));
                        }
                        y++;
                    }
                    x++;
                }
            }
            else if ((roomXY.x <= (((float) width) / 2f)) && (roomXY.y > (((float) height) / 2f)))
            {
                int x = (width / 2) + 1;
                int num9 = width;
                while (x < num9)
                {
                    int y = 0;
                    int num11 = height / 2;
                    while (y < num11)
                    {
                        if (findPathRect[x, y] == 0)
                        {
                            list.Add(new Vector2Int(x, y));
                        }
                        y++;
                    }
                    x++;
                }
            }
            else if ((roomXY.x > (((float) width) / 2f)) && (roomXY.y <= (((float) height) / 2f)))
            {
                int x = 0;
                int num13 = width / 2;
                while (x < num13)
                {
                    int y = (height / 2) + 1;
                    int num15 = height;
                    while (y < num15)
                    {
                        if (findPathRect[x, y] == 0)
                        {
                            list.Add(new Vector2Int(x, y));
                        }
                        y++;
                    }
                    x++;
                }
            }
            else
            {
                int x = 0;
                int num17 = width / 2;
                while (x < num17)
                {
                    int y = 0;
                    int num19 = height / 2;
                    while (y < num19)
                    {
                        if (findPathRect[x, y] == 0)
                        {
                            list.Add(new Vector2Int(x, y));
                        }
                        y++;
                    }
                    x++;
                }
            }
            int num20 = GameLogic.Random(0, list.Count);
            Vector2Int num21 = list[num20];
            this.nextpos = GameLogic.Release.MapCreatorCtrl.GetWorldPosition(num21.x, num21.y);
            this.findpath = GameLogic.Release.Path.FindingPath(base.m_Entity.position, this.nextpos);
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
        this.target = GameLogic.Self;
        this.Find();
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
        Vector3 normalized = new Vector3(x, 0f, z);
        normalized = normalized.normalized;
        this.m_MoveData.angle = Utils.getAngle(x, z);
        this.m_MoveData.direction = normalized;
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
                this.nextpos = this.target.position;
                this.UpdateDirection();
            }
        }
    }
}

