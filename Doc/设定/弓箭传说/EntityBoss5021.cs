using System;

public class EntityBoss5021 : EntityBossBase
{
    protected override bool GetCanPositionBy() => 
        !base.mAniCtrlBase.GetPlayHittedCallback();

    protected override void StartInit()
    {
        base.StartInit();
        base.m_AniCtrl.ClearString("Hitted");
    }
}

