using System;

public class RoomGenerateFlyDodge : RoomGenerateChallenge101
{
    protected override void OnStartGameEnd()
    {
        base.OnStartGameEnd();
        base.roomCtrl.OpenDoor(false);
    }
}

