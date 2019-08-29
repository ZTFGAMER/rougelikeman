using Dxx.Util;
using System;
using System.Collections.Generic;
using TableTool;
using UnityEngine;

public class AIMoveBomberman : AIMoveBase
{
    private EntityBase target;
    private List<Grid.NodeItem> findpath;
    private Vector3 nextpos;
    private Vector3 endpos;
    private Weapon_weapon weaponData;
    private int range;
    private Vector2Int checkpos;

    public AIMoveBomberman(EntityBase entity, int range) : base(entity)
    {
        this.checkpos = new Vector2Int(-1, -1);
        this.range = range;
        this.weaponData = LocalModelManager.Instance.Weapon_weapon.GetBeanById(0xbd9);
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
            Vector2Int num = GameLogic.Release.MapCreatorCtrl.Bomberman_get_safe_near(base.m_Entity.position);
            this.findpath = GameLogic.Release.Path.FindingPath(base.m_Entity.position, GameLogic.Release.MapCreatorCtrl.GetWorldPosition(num));
            if (this.findpath.Count == 0)
            {
                GameLogic.Release.MapCreatorCtrl.RandomItemSide(base.m_Entity, this.range, out float num2, out float num3);
                this.endpos = new Vector3(num2, 0f, num3);
                this.findpath = GameLogic.Release.MapCreatorCtrl.Bomberman_find_path(base.m_Entity.position, this.endpos);
            }
            if (this.findpath.Count > 0)
            {
                Grid.NodeItem item = this.findpath[0];
                this.checkpos = new Vector2Int(item.x, item.y);
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

    private bool issamepos(Grid.NodeItem item) => 
        ((item.x == this.checkpos.x) && (item.y == this.checkpos.y));

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
        if (((this.findpath.Count > 0) && !this.issamepos(this.findpath[0])) && GameLogic.Release.MapCreatorCtrl.Bomberman_is_danger(this.findpath[0]))
        {
            this.Find();
        }
        if (this.findpath.Count > 0)
        {
            this.UpdateDirection();
            Vector3 vector = this.nextpos - base.m_Entity.position;
            if (vector.magnitude < 0.1f)
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

