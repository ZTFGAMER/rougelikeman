using System;

public class SkillAlone3001 : SkillAloneBase
{
    private int level;
    private float speedratio = 0.5f;
    private bool bRemove = true;

    private void OnGotoNextRoom(RoomGenerateBase.Room room)
    {
        if (room.RoomID > this.level)
        {
            base.Uninstall();
        }
    }

    protected override void OnInstall()
    {
        this.level = int.Parse(base.m_SkillData.Args[0]);
        this.bRemove = false;
        ReleaseModeManager mode = GameLogic.Release.Mode;
        mode.OnGotoNextRoom = (Action<RoomGenerateBase.Room>) Delegate.Combine(mode.OnGotoNextRoom, new Action<RoomGenerateBase.Room>(this.OnGotoNextRoom));
    }

    protected override void OnUninstall()
    {
        ReleaseModeManager mode = GameLogic.Release.Mode;
        mode.OnGotoNextRoom = (Action<RoomGenerateBase.Room>) Delegate.Remove(mode.OnGotoNextRoom, new Action<RoomGenerateBase.Room>(this.OnGotoNextRoom));
        this.RemoveAttribute();
    }

    private void RemoveAttribute()
    {
        if (!this.bRemove)
        {
            this.bRemove = true;
        }
    }
}

