using Dxx.Util;
using System;
using TableTool;

public class RoomGenerateGold1 : RoomGenerateBase
{
    private RoomGenerateBase.WaveData mWave;
    private Stage_Level_activitylevel activitydata;

    public override bool CanOpenDoor() => 
        (((this.mWave != null) && this.mWave.IsEnd) && base.CanOpenDoor());

    private void ClearCurrentRoom()
    {
        base.roomCtrl.Clear();
    }

    public void OnCreateWave()
    {
        object[] args = new object[] { base.currentRoomID };
        string str = Utils.FormatString("{0:D2}", args);
        MapCreator.Transfer t = new MapCreator.Transfer {
            roomctrl = base.roomCtrl,
            roomid = base.currentRoomID,
            resourcesid = base.roomList[base.currentRoomID].ResourcesID,
            tmxid = str,
            delay = true
        };
        GameLogic.Release.MapCreatorCtrl.CreateMap(t);
    }

    protected override void OnDeInit()
    {
        if (this.mWave != null)
        {
            this.mWave.Stop();
        }
    }

    protected override void OnEnterDoorAfter()
    {
        base.roomCtrl.OpenDoor(false);
        this.StartCurrentRoomWave();
    }

    protected override void OnEnterDoorBefore()
    {
    }

    protected override void OnEventClose(RoomGenerateBase.EventCloseTransfer data)
    {
    }

    protected override string OnGetTmxID(int roomid)
    {
        if (roomid == 0)
        {
            return "emptyroom";
        }
        Stage_Level_activitylevel activeLevelData = GameLogic.Hold.BattleData.GetActiveLevelData(roomid);
        int index = GameLogic.Random(0, activeLevelData.RoomIDs.Length);
        return activeLevelData.RoomIDs[index];
    }

    protected override void OnInit()
    {
    }

    protected override void OnOpenDoor()
    {
    }

    protected override void OnReceiveEvent(string eventName, object data)
    {
        if ((eventName == null) || (eventName != "Mode_Adventure_CreateNextWave"))
        {
            object[] args = new object[] { base.GetType().ToString(), eventName };
            throw new Exception(Utils.FormatString("{0}.OnReceiveEvent Receive [{1}] is not expected!", args));
        }
    }

    protected override void OnStartGame()
    {
        base.maxRoomID = LocalModelManager.Instance.Stage_Level_activitylevel.GetMaxLayer();
    }

    protected override void OnStartGameEnd()
    {
    }

    private void StartCurrentRoomWave()
    {
        this.activitydata = GameLogic.Hold.BattleData.GetActiveLevelData(base.currentRoomID);
        this.mWave = new RoomGenerateBase.WaveData(0f, this.activitydata.Args);
        this.mWave.OnCreateWave = new Action(this.OnCreateWave);
        this.mWave.Start();
    }
}

