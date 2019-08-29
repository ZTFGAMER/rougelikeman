using Dxx.Util;
using System;

public class RoomGenerateBombDodge : RoomGenerateChallenge101
{
    protected override string OnGetFirstRoomTMX()
    {
        int num = GameLogic.Random(1, 3);
        object[] args = new object[] { num };
        return Utils.FormatString("bombdodge{0:D2}", args);
    }

    protected override void OnStartGameEnd()
    {
        base.OnStartGameEnd();
        base.roomCtrl.OpenDoor(false);
    }
}

