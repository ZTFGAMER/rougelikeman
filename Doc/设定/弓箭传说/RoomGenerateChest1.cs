using Dxx.Util;
using PureMVC.Patterns;
using System;
using System.Runtime.InteropServices;
using TableTool;

public class RoomGenerateChest1 : RoomGenerateBase
{
    public const string Event_TurnTable_Monster = "Event_TurnTable_Monster";
    public const string Event_TurnTable_Boss = "Event_TurnTable_Boss";
    public const string GetEvent_TurnTable_DropID = "GetEvent_TurnTable_DropID";
    private RoomGenerateBase.WaveData mWave;
    private Stage_Level_activitylevel activitydata;

    public override bool CanOpenDoor() => 
        base.CanOpenDoor();

    private bool IsBossRoom(int roomid) => 
        LocalModelManager.Instance.Stage_Level_stagechapter.IsMaxLevel(GameLogic.Hold.BattleData.Level_CurrentStage, roomid);

    public override bool IsLastRoom() => 
        LocalModelManager.Instance.Stage_Level_stagechapter.IsMaxLevel(GameLogic.Hold.BattleData.Level_CurrentStage, base.currentRoomID);

    public void OnCreateWave()
    {
        string str = base.RandomTmx(this.activitydata.RoomIDs);
        MapCreator.Transfer t = new MapCreator.Transfer {
            roomctrl = base.roomCtrl,
            roomid = base.currentRoomID,
            resourcesid = base.roomList[base.currentRoomID].ResourcesID,
            tmxid = str,
            delay = true,
            roomtype = RoomGenerateBase.RoomType.eBoss
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
        this.UpdateActivityData();
        Facade.Instance.RegisterProxy(new EventChest1Proxy(this.activitydata.Args));
    }

    protected override void OnEnterDoorBefore()
    {
    }

    protected override void OnEventClose(RoomGenerateBase.EventCloseTransfer data)
    {
        if (data.windowid == WindowID.WindowID_EventChect1)
        {
            TurnTableType type = (TurnTableType) data.data;
            if (type != TurnTableType.Boss)
            {
                base.roomCtrl.OpenDoor(true);
            }
        }
    }

    protected override object OnGetEvent(string eventName, object data = null)
    {
        if ((eventName != null) && (eventName == "GetEvent_TurnTable_DropID"))
        {
            return int.Parse(this.activitydata.Args[2]);
        }
        return null;
    }

    protected override string OnGetTmxID(int roomid) => 
        "0_0001";

    protected override void OnInit()
    {
    }

    protected override void OnMonsterDead(EntityBase entity)
    {
    }

    protected override void OnOpenDoor()
    {
        this.ShowBossDeadEvent();
    }

    protected override void OnPlayerHitted(long changehp)
    {
    }

    protected override void OnReceiveEvent(string eventName, object data)
    {
        if ((eventName != null) && ((eventName == "Event_TurnTable_Monster") || (eventName == "Event_TurnTable_Boss")))
        {
            this.StartWave();
        }
        else
        {
            object[] args = new object[] { base.GetType().ToString(), eventName };
            throw new Exception(Utils.FormatString("{0}.OnReceiveEvent Receive [{1}] is not expected!", args));
        }
    }

    protected override void OnStartGame()
    {
        this.UpdateActivityData();
        base.maxRoomID = LocalModelManager.Instance.Stage_Level_activitylevel.GetMaxLayer();
    }

    public override void PlayerDead()
    {
    }

    private void ShowBossDeadEvent()
    {
    }

    private void ShowEvent()
    {
    }

    private void StartWave()
    {
        this.mWave = new RoomGenerateBase.WaveData(0.5f, this.activitydata.Args);
        this.mWave.OnCreateWave = new Action(this.OnCreateWave);
        this.mWave.Start();
    }

    private void UpdateActivityData()
    {
        this.activitydata = GameLogic.Hold.BattleData.ActiveLevelData;
    }
}

