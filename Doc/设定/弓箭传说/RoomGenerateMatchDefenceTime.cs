using DG.Tweening;
using Dxx.Util;
using System;
using TableTool;

public class RoomGenerateMatchDefenceTime : RoomGenerateBase
{
    private Sequence seq;
    private Stage_Level_activitylevel activitydata;

    public override bool CanOpenDoor()
    {
        if (base.CanOpenDoor())
        {
            this.CreateWave();
        }
        return false;
    }

    private void ClearCurrentRoom()
    {
        base.roomCtrl.ClearGoods();
    }

    private void CreateWave()
    {
        this.ClearCurrentRoom();
        base.currentRoomID++;
        RoomGenerateBase.Room room = new RoomGenerateBase.Room();
        room.SetRoomID(base.currentRoomID);
        string[] tmxids = null;
        int num = (int) GameLogic.Hold.BattleData.Challenge_GetEvent("MatchDefenceTime_get_random_roomid_row");
        switch (num)
        {
            case 0:
                tmxids = GameLogic.Hold.BattleData.GetActiveLevelData(base.currentRoomID).RoomIDs;
                break;

            case 1:
                tmxids = GameLogic.Hold.BattleData.GetActiveLevelData(base.currentRoomID).RoomIDs1;
                break;

            case 2:
                tmxids = GameLogic.Hold.BattleData.GetActiveLevelData(base.currentRoomID).RoomIDs2;
                break;

            default:
            {
                object[] args = new object[] { num };
                SdkManager.Bugly_Report("RoomGenerateMatchDefenceTime.CreateWave", Utils.FormatString("(NotifyConst.MatchDefenceTime.get_random_roomid_row[{0}] is invalid.", args));
                break;
            }
        }
        XRandom random = (XRandom) GameLogic.Hold.BattleData.Challenge_GetEvent("MatchDefenceTime_get_xrandom");
        string tmxid = base.RandomTmx(tmxids, random);
        room.SetTmx(tmxid);
        MapCreator.Transfer t = new MapCreator.Transfer {
            roomctrl = base.roomCtrl,
            roomid = base.currentRoomID,
            resourcesid = room.ResourcesID,
            tmxid = room.TMXID,
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
        base.currentRoomID = 0;
        base.maxRoomID = LocalModelManager.Instance.Stage_Level_activitylevel.GetMaxLayer();
        this.StartWave();
    }

    protected override void OnStartGameEnd()
    {
        base.roomCtrl.OpenDoor(false);
    }

    private void StartWave()
    {
        this.activitydata = GameLogic.Hold.BattleData.GetActiveLevelData(base.currentRoomID);
        this.seq = DOTween.Sequence();
        TweenSettingsExtensions.AppendInterval(this.seq, float.Parse(this.activitydata.Args[0]));
        TweenSettingsExtensions.AppendCallback(this.seq, new TweenCallback(this, this.CreateWave));
    }
}

