using DG.Tweening;
using Dxx.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AIMove1048 : AIMoveBase
{
    private EntityBase target;
    private List<Grid.NodeItem> findpath;
    private Vector3 nextpos;
    private float startTime;
    private Sequence seq;
    private bool bStartMove;
    private float time;

    public AIMove1048(EntityBase entity, int time) : base(entity)
    {
        this.time = ((float) time) / 1000f;
    }

    private void AIMoveStart()
    {
        base.m_Entity.SetSuperArmor(true);
        this.m_MoveData.action = "Skill";
        GameLogic.Hold.Sound.PlayMonsterSkill(0x4dd1e1, base.m_Entity.position);
        this.m_MoveData.UpdateDirectionByAngle(base.m_Entity.eulerAngles.y);
        this.m_MoveData.direction *= 6f;
        base.m_Entity.m_MoveCtrl.AIMoveStart(base.m_MoveData);
    }

    private void AIMoving()
    {
        base.m_Entity.m_MoveCtrl.AIMoving(base.m_MoveData);
    }

    private void KillSequence()
    {
        if (this.seq != null)
        {
            TweenExtensions.Kill(this.seq, false);
            this.seq = null;
        }
    }

    private void MoveNormal()
    {
        this.AIMoving();
        if ((Updater.AliveTime - this.startTime) > this.time)
        {
            base.End();
        }
    }

    protected override void OnEnd()
    {
        this.KillSequence();
        base.m_Entity.SetSuperArmor(false);
        base.m_Entity.m_MoveCtrl.AIMoveEnd(base.m_MoveData);
    }

    protected override void OnInitBase()
    {
        this.KillSequence();
        this.bStartMove = true;
        this.target = GameLogic.Self;
        this.startTime = Updater.AliveTime;
        this.AIMoveStart();
    }

    protected override void OnUpdate()
    {
        if (this.bStartMove)
        {
            this.MoveNormal();
        }
    }
}

