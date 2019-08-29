using Dxx.Util;
using System;

public class RoomGenerateChallenge101 : RoomGenerateBase
{
    private void ClearCurrentRoom()
    {
        base.roomCtrl.Clear();
    }

    protected override void OnDeInit()
    {
    }

    protected override void OnEnterDoorAfter()
    {
        base.roomCtrl.OpenDoor(false);
    }

    protected override void OnEnterDoorBefore()
    {
    }

    protected override void OnEventClose(RoomGenerateBase.EventCloseTransfer data)
    {
    }

    protected override string OnGetTmxID(int roomid)
    {
        string[] tmxIds = GameLogic.Hold.BattleData.mModeData.GetTmxIds(roomid, 0);
        return base.RandomTmx(tmxIds);
    }

    protected override void OnInit()
    {
    }

    protected override void OnOpenDoor()
    {
    }

    protected override void OnReceiveEvent(string eventName, object data)
    {
        if (eventName != null)
        {
        }
        object[] args = new object[] { base.GetType().ToString(), eventName };
        throw new Exception(Utils.FormatString("{0}.OnReceiveEvent Receive [{1}] is not expected!", args));
    }

    protected override void OnStartGame()
    {
    }

    protected override void OnStartGameEnd()
    {
    }
}

