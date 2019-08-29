using System;

public class EntitySoldier3041 : EntityMonsterBase
{
    protected override void OnInit()
    {
        base.OnInit();
        base.m_AttackCtrl.SetRotate(0f);
    }
}

