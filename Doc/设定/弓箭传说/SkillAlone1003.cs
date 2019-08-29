using Dxx.Util;
using System;
using System.Collections.Generic;

public class SkillAlone1003 : SkillAloneBase
{
    private List<SkillCreateBase> mClouds = new List<SkillCreateBase>();

    private void CloudTimeOver(SkillCreateBase cloud)
    {
        cloud.Deinit();
        this.mClouds.Remove(cloud);
    }

    private void DeadAction(EntityBase entity)
    {
        if (entity != null)
        {
            SkillCreateBase cloud = this.GetCloud();
            cloud.transform.position = entity.position;
            cloud.Init(base.m_Entity, base.m_SkillData.Args);
            cloud.SetTimeCallback(new Action<SkillCreateBase>(this.CloudTimeOver));
            this.mClouds.Add(cloud);
        }
    }

    private SkillCreateBase GetCloud()
    {
        object[] args = new object[] { base.ClassName };
        return GameLogic.EffectGet(Utils.FormatString("Game/SkillPrefab/{0}", args)).GetComponent<SkillCreateBase>();
    }

    private void OnGotoNextRoom(RoomGenerateBase.Room room)
    {
        int num = 0;
        int count = this.mClouds.Count;
        while (num < count)
        {
            this.mClouds[num].Deinit();
            num++;
        }
        this.mClouds.Clear();
    }

    protected override void OnInstall()
    {
        base.m_Entity.OnMonsterDeadAction = (Action<EntityBase>) Delegate.Combine(base.m_Entity.OnMonsterDeadAction, new Action<EntityBase>(this.DeadAction));
        ReleaseModeManager mode = GameLogic.Release.Mode;
        mode.OnGotoNextRoom = (Action<RoomGenerateBase.Room>) Delegate.Combine(mode.OnGotoNextRoom, new Action<RoomGenerateBase.Room>(this.OnGotoNextRoom));
    }

    protected override void OnUninstall()
    {
        base.m_Entity.OnMonsterDeadAction = (Action<EntityBase>) Delegate.Remove(base.m_Entity.OnMonsterDeadAction, new Action<EntityBase>(this.DeadAction));
        ReleaseModeManager mode = GameLogic.Release.Mode;
        mode.OnGotoNextRoom = (Action<RoomGenerateBase.Room>) Delegate.Remove(mode.OnGotoNextRoom, new Action<RoomGenerateBase.Room>(this.OnGotoNextRoom));
        this.OnGotoNextRoom(null);
    }
}

