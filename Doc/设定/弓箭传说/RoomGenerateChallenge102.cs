using DG.Tweening;
using Dxx.Util;
using System;
using TableTool;

public class RoomGenerateChallenge102 : RoomGenerateBase
{
    private Sequence seq;
    private Stage_Level_activitylevel activitydata;
    private int waveid;

    public override bool CanOpenDoor() => 
        false;

    private void ClearCurrentRoom()
    {
        base.roomCtrl.Clear();
    }

    private void CreateWave()
    {
        this.waveid++;
        RoomGenerateBase.Room room = new RoomGenerateBase.Room();
        room.SetRoomID(this.waveid);
        string[] roomIDs = GameLogic.Hold.BattleData.GetActiveLevelData(this.waveid).RoomIDs;
        string tmxid = base.RandomTmx(roomIDs);
        room.SetTmx(tmxid);
        MapCreator.Transfer t = new MapCreator.Transfer {
            roomctrl = base.roomCtrl,
            roomid = base.currentRoomID,
            resourcesid = room.ResourcesID,
            tmxid = room.TMXID,
            delay = true
        };
        GameLogic.Release.MapCreatorCtrl.CreateMap(t);
        this.StartWave();
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
        if (this.seq != null)
        {
            TweenExtensions.Kill(this.seq, false);
        }
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
        if ((eventName == null) || (eventName != "Mode_Adventure_CreateNextWave"))
        {
            object[] args = new object[] { base.GetType().ToString(), eventName };
            throw new Exception(Utils.FormatString("{0}.OnReceiveEvent Receive [{1}] is not expected!", args));
        }
    }

    protected override void OnStartGame()
    {
        this.waveid = 0;
        this.StartWave();
    }

    protected override void OnStartGameEnd()
    {
        base.roomCtrl.OpenDoor(false);
    }

    private void StartWave()
    {
        this.activitydata = GameLogic.Hold.BattleData.GetActiveLevelData(this.waveid);
        this.seq = DOTween.Sequence();
        TweenSettingsExtensions.AppendInterval(this.seq, float.Parse(this.activitydata.Args[0]));
        TweenSettingsExtensions.AppendCallback(this.seq, new TweenCallback(this, this.CreateWave));
    }
}

