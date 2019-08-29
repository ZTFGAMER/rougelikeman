using Dxx.Util;
using System;
using TableTool;

public class RoomGenerateLevel : RoomGenerateBase
{
    private int opendoorIndex;
    private bool bShowMysticShop;
    private int bossdeadecentid = -1;

    public override bool CanOpenDoor()
    {
        if (!LocalModelManager.Instance.Stage_Level_stagechapter.is_wave_room())
        {
            return base.CanOpenDoor();
        }
        if (base.CanOpenDoor())
        {
            GameLogic.Release.MapCreatorCtrl.waveroom_currentwave_clear();
        }
        return (GameLogic.Release.MapCreatorCtrl.waveroom_is_clear() && base.CanOpenDoor());
    }

    protected override bool gotonextdoor_canopen()
    {
        if (!LocalModelManager.Instance.Stage_Level_stagechapter.is_wave_room())
        {
            return base.gotonextdoor_canopen();
        }
        return ((base.currentRoomID == 0) || GameLogic.Release.MapCreatorCtrl.waveroom_is_clear());
    }

    private bool IsBossRoom(int roomid) => 
        LocalModelManager.Instance.Stage_Level_stagechapter.IsMaxLevel(GameLogic.Hold.BattleData.Level_CurrentStage, roomid);

    public override bool IsLastRoom() => 
        LocalModelManager.Instance.Stage_Level_stagechapter.IsMaxLevel(GameLogic.Hold.BattleData.Level_CurrentStage, base.currentRoomID);

    protected override void OnDeInit()
    {
        RoomGenerateBase.CacheMap(base.currentMap);
    }

    protected override void OnEnd()
    {
        base.OnEnd();
    }

    protected override void OnEnterDoorAfter()
    {
        this.room_update();
    }

    protected override void OnEnterDoorBefore()
    {
        if (LocalModelManager.Instance.Stage_Level_stagechapter.is_wave_room())
        {
            GameLogic.Release.MapCreatorCtrl.waveroom_killseq();
        }
    }

    protected override void OnEventClose(RoomGenerateBase.EventCloseTransfer data)
    {
    }

    protected override string OnGetTmxID(int roomid)
    {
        int stage = GameLogic.Hold.BattleData.Level_CurrentStage;
        string[] tmxIds = GameLogic.Hold.BattleData.mModeData.GetTmxIds(roomid, LocalSave.Instance.SaveExtra.GetLayerCount(stage, roomid));
        return base.RandomTmx(tmxIds);
    }

    protected override void OnInit()
    {
    }

    protected override void OnMonsterDead(EntityBase entity)
    {
    }

    protected override void OnOpenDoor()
    {
        if (this.bossdeadecentid != base.currentRoomID)
        {
            this.bossdeadecentid = base.currentRoomID;
            if (((base.currentRoomID % 5) != 0) && (base.currentRoomID > 0))
            {
                if (!this.bShowMysticShop)
                {
                    if (LocalModelManager.Instance.Shop_MysticShop.RandomShop(GameLogic.Hold.BattleData.Level_CurrentStage, base.currentRoomID, base.roomList[base.currentRoomID].RoomType))
                    {
                        this.bShowMysticShop = true;
                        GameLogic.Release.MapCreatorCtrl.CreateOneGoods(0x232c, 5, 1);
                    }
                }
                else
                {
                    LocalModelManager.Instance.Shop_MysticShop.AddRatio(GameLogic.Hold.BattleData.Level_CurrentStage);
                }
            }
        }
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
        this.opendoorIndex = -1;
        base.maxRoomID = LocalModelManager.Instance.Stage_Level_stagechapter.GetCurrentMaxLevel(GameLogic.Hold.BattleData.Level_CurrentStage);
        if (LocalModelManager.Instance.Stage_Level_stagechapter.is_wave_room())
        {
            GameLogic.Release.MapCreatorCtrl.waveroom_battlecache_init();
        }
    }

    protected override void OnStartGameEnd()
    {
        this.room_update();
        if (((GameLogic.Hold.BattleData.GetMode() == GameMode.eLevel) && (base.currentRoomID == 0)) && ((GameLogic.Hold.BattleData.Level_CurrentStage > 1) && !LocalSave.Instance.BattleIn_GetGoldTurn()))
        {
            GameLogic.Release.MapCreatorCtrl.CreateGoodExtra(0x2335, 5, 1);
        }
    }

    public override void PlayerDead()
    {
    }

    private void room_update()
    {
        if (base.currentRoomID == 0)
        {
            base.roomCtrl.SetText(string.Empty);
        }
        else
        {
            base.roomCtrl.SetText(base.currentRoomID.ToString());
        }
    }

    private void ShowEvent()
    {
        if (!GameLogic.Hold.Guide.GetNeedGuide() && (GameLogic.Release.Entity.GetActiveEntityCount() == 0))
        {
            int goodsID = 0x2331;
            if (this.IsBossRoom(base.currentRoomID + 1))
            {
                goodsID = 0x232b;
            }
            GameLogic.Release.MapCreatorCtrl.CreateOneGoods(goodsID, 5, 3);
        }
    }
}

