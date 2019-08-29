using Dxx.Util;
using System;
using UnityEngine;

public class AIMove1034 : AIMoveBase
{
    private EntityBase target;
    private int offsetx;
    private int offsety;
    private int move2player;
    private Vector3 endpos;
    private float movedis;
    private float alldis;

    public AIMove1034(EntityBase entity) : base(entity)
    {
        this.move2player = 50;
        this.target = GameLogic.Self;
    }

    private bool GetMove2Player() => 
        (GameLogic.Random(0, 100) < this.move2player);

    protected override void OnEnd()
    {
        base.m_Entity.m_MoveCtrl.AIMoveEnd(base.m_MoveData);
    }

    protected override void OnInitBase()
    {
        this.movedis = 0f;
        Vector2Int num = GameLogic.Release.MapCreatorCtrl.RandomItemSide(base.m_Entity);
        if (this.GetMove2Player())
        {
            num = this.UpdateMove2Player();
        }
        Vector2Int roomXY = GameLogic.Release.MapCreatorCtrl.GetRoomXY(base.m_Entity.position);
        this.offsetx = num.x - roomXY.x;
        this.offsety = num.y - roomXY.y;
        for (int i = 0; i < 2; i++)
        {
            if (!GameLogic.Release.MapCreatorCtrl.IsEmpty(new Vector2Int(num.x + this.offsetx, num.y + this.offsety)))
            {
                break;
            }
            num.x += this.offsetx;
            num.y += this.offsety;
        }
        this.endpos = GameLogic.Release.MapCreatorCtrl.GetWorldPosition(num);
        this.UpdateDirection();
        Vector3 vector = this.endpos - base.m_Entity.position;
        this.alldis = vector.magnitude;
        base.m_Entity.m_MoveCtrl.AIMoveStart(base.m_MoveData);
    }

    protected override void OnUpdate()
    {
        float num = base.m_Entity.m_EntityData.GetSpeed() * Updater.delta;
        if ((this.movedis + num) > this.alldis)
        {
            num = this.alldis - this.movedis;
        }
        this.movedis += num;
        base.m_Entity.m_MoveCtrl.AIMoving(base.m_MoveData);
        if (this.movedis >= this.alldis)
        {
            base.End();
        }
    }

    private void UpdateDirection()
    {
        float x = this.endpos.x - base.m_Entity.position.x;
        float y = this.endpos.z - base.m_Entity.position.z;
        this.m_MoveData.angle = Utils.getAngle(x, y);
        Vector3 vector3 = new Vector3(x, 0f, y);
        this.m_MoveData.direction = vector3.normalized;
        base.m_Entity.m_AttackCtrl.RotateHero(this.m_MoveData.angle);
    }

    private Vector2Int UpdateMove2Player()
    {
        Vector2Int roomXY = GameLogic.Release.MapCreatorCtrl.GetRoomXY(base.m_Entity.position);
        float num2 = Utils.getAngle(this.target.position - base.m_Entity.position);
        if (((num2 >= 337.5f) || (num2 <= 22.5f)) && GameLogic.Release.MapCreatorCtrl.IsEmpty(roomXY + new Vector2Int(0, -1)))
        {
            return (roomXY + new Vector2Int(0, -1));
        }
        if (((num2 >= 22.5f) && (num2 <= 67.5f)) && GameLogic.Release.MapCreatorCtrl.IsEmpty(roomXY + new Vector2Int(1, -1)))
        {
            return (roomXY + new Vector2Int(1, -1));
        }
        if (((num2 >= 67.5f) && (num2 <= 112.5f)) && GameLogic.Release.MapCreatorCtrl.IsEmpty(roomXY + new Vector2Int(1, 0)))
        {
            return (roomXY + new Vector2Int(1, 0));
        }
        if (((num2 >= 112.5f) && (num2 <= 157.5f)) && GameLogic.Release.MapCreatorCtrl.IsEmpty(roomXY + new Vector2Int(1, 1)))
        {
            return (roomXY + new Vector2Int(1, 1));
        }
        if (((num2 >= 157.5f) && (num2 <= 202.5f)) && GameLogic.Release.MapCreatorCtrl.IsEmpty(roomXY + new Vector2Int(0, 1)))
        {
            return (roomXY + new Vector2Int(0, 1));
        }
        if (((num2 >= 202.5f) && (num2 <= 247.5f)) && GameLogic.Release.MapCreatorCtrl.IsEmpty(roomXY + new Vector2Int(-1, 1)))
        {
            return (roomXY + new Vector2Int(-1, 1));
        }
        if (((num2 >= 247.5f) && (num2 <= 292.5f)) && GameLogic.Release.MapCreatorCtrl.IsEmpty(roomXY + new Vector2Int(-1, 0)))
        {
            return (roomXY + new Vector2Int(-1, 0));
        }
        if (((num2 >= 292.5f) && (num2 <= 337.5f)) && GameLogic.Release.MapCreatorCtrl.IsEmpty(roomXY + new Vector2Int(-1, -1)))
        {
            return (roomXY + new Vector2Int(-1, -1));
        }
        return GameLogic.Release.MapCreatorCtrl.RandomItemSide(base.m_Entity);
    }
}

