using Dxx.Util;
using System;
using UnityEngine;

public class AIMove1028 : AIMoveBase
{
    private const string COLLIDER_RESOURCE = "Game/SkillPrefab/CollisionCtrl1007";
    private bool bRotateOver;
    private int reboundcount;
    private const int ReboundMaxCount = 3;
    protected float Move_NextX;
    protected float Move_NextY;
    private GameObject lastwall;
    private GameObject mCollision;
    private float mEndStartTime;
    private float mEndTime;

    public AIMove1028(EntityBase entity) : base(entity)
    {
        this.mEndTime = 1f;
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
        if (((o.gameObject.layer == LayerManager.MapOutWall) || (o.gameObject.layer == LayerManager.Stone)) && (this.lastwall != o.gameObject))
        {
            this.lastwall = o.gameObject;
            if (this.reboundcount < 3)
            {
                this.reboundcount++;
                float angle = Utils.ExcuteReboundWallSkill(base.m_Entity.eulerAngles.y, o.contacts[0].point, base.m_Entity.GetComponent<SphereCollider>(), o.collider);
                this.Move_NextX = MathDxx.Sin(angle);
                this.Move_NextY = MathDxx.Cos(angle);
                this.UpdateMoveData();
                if (this.reboundcount == 3)
                {
                    this.mEndStartTime = Updater.AliveTime + this.mEndTime;
                }
            }
            else
            {
                base.End();
            }
        }
    }

    private void CreateCollisionCtrl()
    {
        GameObject obj2 = GameObjectPool.Instantiate("Game/SkillPrefab/CollisionCtrl1007");
        obj2.transform.SetParent(base.m_Entity.transform);
        obj2.transform.localPosition = Vector3.zero;
        obj2.transform.localRotation = Quaternion.identity;
        obj2.transform.localScale = Vector3.one;
        obj2.GetComponent<CapsuleCollider>().radius = base.m_Entity.GetCollidersSize();
        this.mCollision = obj2;
        obj2.GetComponent<EntityMove1007Ctrl>().CollisionEnterAction = new Action<Collision>(this.CollisionEnter);
    }

    protected override void OnEnd()
    {
        base.m_Entity.m_AniCtrl.SetString("Skill", "Skill");
        Object.Destroy(this.mCollision);
        base.m_Entity.m_MoveCtrl.AIMoveEnd(base.m_MoveData);
    }

    protected override void OnInitBase()
    {
        this.bRotateOver = false;
        this.lastwall = null;
        this.reboundcount = 0;
        this.Move_NextX = base.m_Entity.m_HatredTarget.position.x - base.m_Entity.position.x;
        this.Move_NextY = base.m_Entity.m_HatredTarget.position.z - base.m_Entity.position.z;
        float angle = Utils.getAngle(this.Move_NextX, this.Move_NextY);
        base.m_Entity.m_AttackCtrl.RotateHero(angle);
        this.CreateCollisionCtrl();
    }

    protected override void OnUpdate()
    {
        if (!this.bRotateOver)
        {
            if (base.m_Entity.m_AttackCtrl.RotateOver())
            {
                this.bRotateOver = true;
                this.Move_NextX = MathDxx.Sin(base.m_Entity.eulerAngles.y);
                this.Move_NextY = MathDxx.Cos(base.m_Entity.eulerAngles.y);
                this.AIMoveStart();
                base.m_Entity.m_AniCtrl.SetString("Skill", "Hitted");
                base.m_Entity.m_AniCtrl.SendEvent("Skill", false);
            }
        }
        else
        {
            this.AIMoving();
        }
        if ((this.reboundcount == 3) && (Updater.AliveTime > this.mEndStartTime))
        {
            base.End();
        }
    }

    private void UpdateMoveData()
    {
        this.m_MoveData.angle = Utils.getAngle(this.Move_NextX, this.Move_NextY);
        this.m_MoveData.direction = new Vector3(this.Move_NextX, 0f, this.Move_NextY) * (3f - (this.reboundcount * 0.4f));
        base.m_Entity.m_AttackCtrl.SetRotate(this.m_MoveData.angle);
    }
}

