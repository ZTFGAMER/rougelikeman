using System;

public class SkillAlone1039 : SkillAloneShieldValueBase
{
    private void OnGotoNextRoom(RoomGenerateBase.Room room)
    {
        base.ResetShieldHitValue();
    }

    protected override void OnInstall()
    {
        base.OnInstall();
        ReleaseModeManager mode = GameLogic.Release.Mode;
        mode.OnGotoNextRoom = (Action<RoomGenerateBase.Room>) Delegate.Combine(mode.OnGotoNextRoom, new Action<RoomGenerateBase.Room>(this.OnGotoNextRoom));
    }

    protected override void OnUninstall()
    {
        base.OnUninstall();
        ReleaseModeManager mode = GameLogic.Release.Mode;
        mode.OnGotoNextRoom = (Action<RoomGenerateBase.Room>) Delegate.Remove(mode.OnGotoNextRoom, new Action<RoomGenerateBase.Room>(this.OnGotoNextRoom));
    }
}

