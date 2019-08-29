using System;

public class EntityTowerBase : EntityCallBase
{
    protected override long GetBossHP() => 
        base.m_EntityData.MaxHP;

    protected override void OnCreateModel()
    {
        base.OnCreateModel();
        base.ShowHP(false);
    }

    protected override void OnDeInitLogic()
    {
        base.OnDeInitLogic();
    }

    protected override void OnInit()
    {
        base.OnInit();
        base.m_MoveCtrl = new MoveControl();
        base.m_AttackCtrl = new AttackControl();
        base.m_MoveCtrl.Init(this);
        base.m_AttackCtrl.Init(this);
        GameLogic.Release.Entity.AddTower(this);
        this.SetCollider(false);
        base.m_AttackCtrl.SetRotate(0f);
    }

    protected override void StartInit()
    {
        base.StartInit();
    }
}

