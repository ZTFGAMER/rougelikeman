using Dxx.Util;
using System;
using UnityEngine;

public class AIMove1007 : AIMoveBase
{
    private const string COLLIDER_RESOURCE = "Game/SkillPrefab/CollisionCtrl1007";
    protected float Move_NextX;
    protected float Move_NextY;
    private GameObject lastwall;
    private GameObject mCollision;

    public AIMove1007(EntityBase entity) : base(entity)
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

    private void CollisionEnter(Collision o)
    {
        if ((!base.m_Entity.GetFlying() || ((o.gameObject.layer != LayerManager.Stone) && (o.gameObject.layer != LayerManager.Waters))) && (this.lastwall != o.gameObject))
        {
            this.lastwall = o.gameObject;
            float angle = Utils.ExcuteReboundWallSkill(base.m_Entity.eulerAngles.y, base.m_Entity.position, base.m_Entity.GetComponent<SphereCollider>(), o.collider);
            this.Move_NextX = MathDxx.Sin(angle);
            this.Move_NextY = MathDxx.Cos(angle);
            this.UpdateMoveData();
        }
    }

    private void CreateCollisionCtrl()
    {
        if (base.m_Entity != null)
        {
            GameObject child = GameObjectPool.Instantiate("Game/SkillPrefab/CollisionCtrl1007");
            child.SetParentNormal(base.m_Entity.transform);
            child.GetComponent<CapsuleCollider>().radius = base.m_Entity.GetCollidersSize();
            this.mCollision = child;
            child.GetComponent<EntityMove1007Ctrl>().CollisionEnterAction = new Action<Collision>(this.CollisionEnter);
        }
    }

    private void MoveNormal()
    {
        this.UpdateMoveData();
        this.AIMoving();
    }

    protected override void OnEnd()
    {
        if (this.mCollision != null)
        {
            Object.Destroy(this.mCollision);
        }
        base.m_Entity.m_MoveCtrl.AIMoveEnd(base.m_MoveData);
    }

    protected override void OnInitBase()
    {
        this.lastwall = null;
        this.CreateCollisionCtrl();
        this.Move_NextX = (GameLogic.Random(0, 2) != 0) ? ((float) (-1)) : ((float) 1);
        this.Move_NextY = (GameLogic.Random(0, 2) != 0) ? ((float) (-1)) : ((float) 1);
        this.AIMoveStart();
    }

    protected override void OnUpdate()
    {
        this.MoveNormal();
    }

    private void UpdateMoveData()
    {
        this.m_MoveData.angle = Utils.getAngle(this.Move_NextX, this.Move_NextY);
        this.m_MoveData.direction = new Vector3(this.Move_NextX, 0f, this.Move_NextY);
        base.m_Entity.m_AttackCtrl.SetRotate(this.m_MoveData.angle);
    }
}

