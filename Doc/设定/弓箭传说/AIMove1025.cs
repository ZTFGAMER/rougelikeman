using Dxx.Util;
using System;
using System.Collections.Generic;
using TableTool;
using UnityEngine;

public class AIMove1025 : AIMoveBase
{
    private EntityBase target;
    private List<Grid.NodeItem> findpath;
    private Vector3 nextpos;
    private Vector3 endpos;
    private Weapon_weapon weaponData;
    private int range;

    public AIMove1025(EntityBase entity, int range) : base(entity)
    {
        this.range = range;
        this.weaponData = LocalModelManager.Instance.Weapon_weapon.GetBeanById(0x1397);
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

    private void CreateBullet(Vector3 pos)
    {
        BulletBase base2 = GameLogic.Release.Bullet.CreateBullet(base.m_Entity, 0x1397, pos + new Vector3(0f, 0.4f, 0f), 0f);
        if (base2 != null)
        {
            base2.transform.rotation = Quaternion.identity;
            base2.SetTarget(GameLogic.Self, 1);
        }
    }

    private void Find()
    {
        if (this.target != null)
        {
            this.findpath = GameLogic.Release.Path.FindingPath(base.m_Entity.position, this.endpos);
            if (this.findpath.Count > 0)
            {
                Grid.NodeItem item = this.findpath[0];
                this.nextpos = GameLogic.Release.MapCreatorCtrl.GetWorldPosition(item.x, item.y);
                this.UpdateDirection();
            }
        }
    }

    private void InitPos()
    {
        GameLogic.Release.MapCreatorCtrl.RandomItemSide(base.m_Entity, this.range, out float num, out float num2);
        this.endpos = new Vector3(num, 0f, num2);
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
        this.InitPos();
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
        if (this.findpath.Count > 0)
        {
            this.UpdateDirection();
            Vector3 vector = this.nextpos - base.m_Entity.position;
            if (vector.magnitude < 0.5f)
            {
                this.CreateBullet(GameLogic.Release.MapCreatorCtrl.GetWorldPosition(this.findpath[0].x, this.findpath[0].y));
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

