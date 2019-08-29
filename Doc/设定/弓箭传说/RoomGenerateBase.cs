using DG.Tweening;
using Dxx.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class RoomGenerateBase
{
    public const string FirstRoomTmx = "firstroom";
    public const string EmptyRoomTmx = "emptyroom";
    protected Action mGuidEndAction;
    protected List<string> mUsedMaps = new List<string>();
    protected Dictionary<string, GameObject> mapList = new Dictionary<string, GameObject>();
    protected Dictionary<int, Room> roomList = new Dictionary<int, Room>();
    private int _currentRoomID;
    private Sequence s_absorb;
    protected int maxRoomID;
    private int opendoorIndex;
    protected RoomControlBase roomCtrl;
    protected GameObject currentMap;
    protected float mCurrentRoomTime;
    private SequencePool mSequencePool = new SequencePool();
    private const int DemonInitRatio = 100;
    private int mDemonRatio = 100;

    public void AddGuildToMap(GameObject o)
    {
        o.transform.SetParent(this.currentMap.transform);
        o.transform.localPosition = Vector3.zero;
        o.transform.localScale = Vector3.one;
        o.transform.localRotation = Quaternion.identity;
    }

    private void AddUsedTmx(string tmxid)
    {
        this.mUsedMaps.Add(tmxid);
    }

    protected static void CacheMap(GameObject o)
    {
        if (o != null)
        {
            o.GetComponent<RoomControlBase>().Clear();
            o.transform.SetParent(GameNode.MapCacheNode.transform);
            o.SetActive(false);
        }
    }

    public virtual bool CanOpenDoor() => 
        (GameLogic.Release.Entity.GetActiveEntityCount() == 0);

    private bool CanRandomTmx(string tmxid) => 
        !this.mUsedMaps.Contains(tmxid);

    public void CheckOpenDoor()
    {
        if (this.CanOpenDoor())
        {
            this.OpenDoor();
            if (GameLogic.Self != null)
            {
                GameLogic.Self.SetAbsorb(true);
            }
        }
    }

    public void Clear()
    {
    }

    protected GameObject CreateMapObject(int resourcesid)
    {
        object[] args = new object[] { SpriteManager.GetStylePrefix(), resourcesid };
        string key = Utils.FormatString("Game/Map/Map{0}{1:D2}", args);
        if (this.mapList.TryGetValue(key, out GameObject obj2))
        {
            obj2.gameObject.SetActive(true);
            obj2.transform.SetParent(GameNode.m_Room.transform);
            obj2.name = key;
            return obj2;
        }
        Object original = ResourceManager.Load<GameObject>(key);
        if (original == null)
        {
            object[] objArray2 = new object[] { key };
            SdkManager.Bugly_Report("RoomGenerateBase", Utils.FormatString("CreateMapObject ResourceManager.Load[{0}] is invalid!!!", objArray2));
        }
        obj2 = Object.Instantiate(original) as GameObject;
        if (obj2 == null)
        {
            object[] objArray3 = new object[] { key };
            SdkManager.Bugly_Report("RoomGenerateBase", Utils.FormatString("CreateMapObject GameObject {0}] is invalid!!!", objArray3));
        }
        obj2.transform.SetParent(GameNode.m_Room.transform);
        obj2.name = key;
        this.mapList.Add(key, obj2);
        return obj2;
    }

    public void DeInit()
    {
        CacheMap(this.currentMap);
        this.mSequencePool.Clear();
        if (this.s_absorb != null)
        {
            TweenExtensions.Kill(this.s_absorb, false);
        }
        this.OnDeInit();
    }

    public void EnterDoor()
    {
        this.mDemonRatio = 100;
        this.OnEnterDoorBefore();
        GameNode.DestroyMonsterNode();
        if (this.roomCtrl != null)
        {
            this.roomCtrl.Clear();
        }
        CacheMap(this.currentMap);
        GameLogic.Release.Entity.MonstersClear();
        this.currentRoomID++;
        this.mCurrentRoomTime = Updater.AliveTime;
        if (this.currentRoomID <= this.maxRoomID)
        {
            this.GotoNextDoor();
        }
        this.OnEnterDoorAfter();
    }

    public void EventClose(EventCloseTransfer data)
    {
        this.OnEventClose(data);
    }

    public int GetCurrentRoomID() => 
        this.currentRoomID;

    public float GetCurrentRoomTime() => 
        this.mCurrentRoomTime;

    public object GetEvent(string eventName, object data = null) => 
        this.OnGetEvent(eventName, data);

    private void GotoNextDoor()
    {
        this.RandomNextRoom();
        Room room = this.roomList[this.currentRoomID];
        GameObject obj2 = this.CreateMapObject(room.ResourcesID);
        this.currentMap = obj2;
        this.roomCtrl = obj2.GetComponent<RoomControlBase>();
        object[] args = new object[] { room.RoomID, room.ResourcesID, room.TMXID };
        obj2.name = obj2.name + Utils.FormatString("_{0}_{1}_{2}", args);
        obj2.transform.position = new Vector3(0f, 0f, 0f);
        obj2.transform.localScale = new Vector3(1f, 1f, 1.23f);
        Room room2 = null;
        this.roomList.TryGetValue(this.currentRoomID + 1, out room2);
        GameLogic.Release.Mode.SetGoodsParent(this.roomCtrl.GetGoodsDropParent());
        MapCreator.Transfer t = new MapCreator.Transfer {
            roomctrl = this.roomCtrl,
            roomid = this.currentRoomID,
            resourcesid = room.ResourcesID,
            tmxid = room.TMXID
        };
        GameLogic.Release.MapCreatorCtrl.CreateMap(t);
        RoomControlBase.Mode_LevelData data = new RoomControlBase.Mode_LevelData {
            room = room,
            nextroom = room2
        };
        this.roomCtrl.Init(data);
        if (this.gotonextdoor_canopen())
        {
            this.OpenDoorDelay(0f);
        }
        else
        {
            GameLogic.Self.SetAbsorb(false);
        }
        if (this._currentRoomID > 0)
        {
            GameConfig.MapGood.Init();
            if ((this.roomList[this.currentRoomID].IsBossRoom && (GameLogic.Self != null)) && (GameLogic.Self.OnInBossRoom != null))
            {
                GameLogic.Self.OnInBossRoom();
            }
            if (LocalSave.Instance.BattleIn_GetIn())
            {
                GameLogic.Self.SetPosition(new Vector3(0f, 0f, -this.roomList[this.currentRoomID].playery));
            }
            else
            {
                GameLogic.Self.SetPosition(new Vector3(0f, 0f, this.roomList[this.currentRoomID].playery));
            }
            if (GameLogic.Release.Mode.OnGotoNextRoom != null)
            {
                GameLogic.Release.Mode.OnGotoNextRoom(this.roomList[this.currentRoomID]);
            }
            GameLogic.Self.OnGotoNextRoom();
            GameLogic.Release.Bullet.CacheAll();
            GameLogic.Release.MapEffect.Clear();
            CameraControlM.Instance.ResetCameraPosition();
            GameLogic.Self.SetAbsorbRangeMax(false);
            GameLogic.Hold.PreLoad(this.currentRoomID);
            ResourceManager.UnloadUnused();
        }
    }

    protected virtual bool gotonextdoor_canopen() => 
        (GameLogic.Release.Entity.GetActiveEntityCount() == 0);

    public void Init()
    {
        this.mCurrentRoomTime = Updater.AliveTime;
        LocalSave.Instance.BattleIn_Init();
        this.OnInit();
    }

    public bool IsBattleLoad() => 
        this.OnIsBattleLoad();

    public bool IsDoorOpen() => 
        this.roomCtrl.IsDoorOpen();

    public virtual bool IsLastRoom() => 
        true;

    public void MonsterDead(EntityBase entity)
    {
        this.OnMonsterDead(entity);
    }

    protected virtual void OnDeInit()
    {
    }

    protected virtual void OnEnd()
    {
        LocalSave.Instance.BattleIn_DeInit();
        WindowUI.ShowWindow(WindowID.WindowID_GameOver);
    }

    protected virtual void OnEnterDoorAfter()
    {
    }

    protected virtual void OnEnterDoorBefore()
    {
    }

    protected virtual void OnEventClose(EventCloseTransfer data)
    {
    }

    protected virtual object OnGetEvent(string eventName, object data = null) => 
        null;

    protected virtual string OnGetFirstRoomTMX() => 
        "firstroom";

    protected virtual string OnGetTmxID(int roomid)
    {
        object[] args = new object[] { base.GetType().ToString(), roomid };
        SdkManager.Bugly_Report("RoomGenerateBase.OnGetTmxID", Utils.FormatString("{0} {1} is not found.", args));
        return string.Empty;
    }

    protected virtual void OnInit()
    {
    }

    protected bool OnIsBattleLoad()
    {
        GameLogic.Hold.BattleData.SetLayer(this.currentRoomID);
        if (this.currentRoomID >= this.maxRoomID)
        {
            this.OnEnd();
            return false;
        }
        return true;
    }

    protected virtual void OnMonsterDead(EntityBase entity)
    {
    }

    protected virtual void OnOpenDoor()
    {
    }

    protected virtual void OnPlayerHitted(long changehp)
    {
    }

    protected virtual void OnReceiveEvent(string eventName, object data)
    {
    }

    protected virtual void OnStartGame()
    {
    }

    protected virtual void OnStartGameEnd()
    {
    }

    public void OpenDoor()
    {
        this.OpenDoorDelay(1.5f);
    }

    private void OpenDoorDelay(float delay)
    {
        if (this.opendoorIndex != this.currentRoomID)
        {
            GameLogic.Self.SetAbsorbRangeMax(true);
            this.opendoorIndex = this.currentRoomID;
            if ((GameLogic.Self != null) && this.roomList[this.currentRoomID].IsBossRoom)
            {
                GameLogic.SendBuff(GameLogic.Self, 0x44c, Array.Empty<float>());
            }
            this.UpdateBattleIn();
            if (delay <= 0f)
            {
                this.OpenDoorDelayCallBack();
            }
            else
            {
                Sequence seq = TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(DOTween.Sequence(), delay), new TweenCallback(this, this.<OpenDoorDelay>m__0));
                this.mSequencePool.Add(seq);
            }
        }
    }

    private void OpenDoorDelayCallBack()
    {
        GameLogic.Hold.Sound.PlayUI(0x4c4b48);
        this.roomCtrl.OpenDoor(true);
        this.ShowBossDeadEvent();
        this.OnOpenDoor();
    }

    public virtual void PlayerDead()
    {
    }

    public void PlayerHitted(long changehp)
    {
        if (changehp < 0L)
        {
            this.mDemonRatio -= GameConfig.GetDemonPerHit();
            int demonMin = GameConfig.GetDemonMin();
            if (this.mDemonRatio < demonMin)
            {
                this.mDemonRatio = demonMin;
            }
        }
        this.OnPlayerHitted(changehp);
    }

    public void PlayerMove()
    {
        if ((Updater.AliveTime - this.mCurrentRoomTime) < 1f)
        {
            this.mCurrentRoomTime--;
        }
    }

    public static void PreloadMap(int id)
    {
    }

    private void RandomNextRoom()
    {
        int currentRoomID = this.currentRoomID;
        if ((currentRoomID <= this.maxRoomID) && !this.roomList.ContainsKey(currentRoomID))
        {
            Room room = new Room();
            room.SetRoomID(currentRoomID);
            if (currentRoomID == 0)
            {
                room.SetTmx("emptyroom");
            }
            else
            {
                room.SetTmx(this.OnGetTmxID(currentRoomID));
            }
            this.roomList.Add(currentRoomID, room);
        }
        int key = this.currentRoomID + 1;
        if (this.maxRoomID == 0)
        {
            SdkManager.Bugly_Report(base.GetType().ToString(), "maxRoomID is 0");
        }
        else if ((key <= this.maxRoomID) && !this.roomList.ContainsKey(key))
        {
            Room room2 = new Room();
            room2.SetRoomID(key);
            room2.SetTmx(this.OnGetTmxID(key));
            this.roomList.Add(key, room2);
        }
    }

    protected string RandomTmx(string[] tmxids) => 
        this.RandomTmx(tmxids, null);

    protected string RandomTmx(string[] tmxids, XRandom random)
    {
        int num3;
        List<string> list = new List<string>();
        int index = 0;
        int length = tmxids.Length;
        while (index < length)
        {
            string str = tmxids[index];
            if (this.CanRandomTmx(str))
            {
                list.Add(str);
            }
            index++;
        }
        if (random != null)
        {
            num3 = random.nextInt(0, list.Count);
        }
        else
        {
            num3 = GameLogic.Random(0, list.Count);
        }
        string tmxid = list[num3];
        this.AddUsedTmx(tmxid);
        if (!GameLogic.Release.MapCreatorCtrl.HaveTmx(tmxid))
        {
            object[] args = new object[] { GameLogic.Hold.BattleData.Level_CurrentStage, tmxid };
            SdkManager.Bugly_Report("RoomGenerateBase", Utils.FormatString("stage:{0} RandomTmx[{1}] is dont have!!!", args));
            return this.RandomTmx(tmxids);
        }
        return tmxid;
    }

    public void SendEvent(string eventName, object data = null)
    {
        this.OnReceiveEvent(eventName, data);
    }

    public void SetGuideEndAction(Action callback)
    {
        this.mGuidEndAction = callback;
    }

    private void ShowBossDeadEvent()
    {
        if (this.roomList[this.currentRoomID].IsBossRoom && !this.IsLastRoom())
        {
            if (GameLogic.Self != null)
            {
                long change = GameLogic.Self.m_EntityData.attribute.KillBossShield.Value;
                GameLogic.Self.m_EntityData.UpdateShieldValueChange(change);
                float num2 = GameLogic.Self.m_EntityData.attribute.KillBossShieldPercent.Value;
                long num3 = MathDxx.CeilToInt(GameLogic.Self.m_EntityData.attribute.GetHPBase() * num2);
                GameLogic.Self.m_EntityData.UpdateShieldValueChange(num3);
            }
            int num4 = GameLogic.Random(0, 100);
            int goodsID = 0x2329;
            if ((num4 < this.mDemonRatio) || GameLogic.Self.m_EntityData.GetOnlyDemon())
            {
                goodsID = 0x2330;
            }
            GameLogic.Release.MapCreatorCtrl.CreateOneGoods(goodsID, 5, 3);
        }
    }

    public void StartGame()
    {
        this.roomList.Clear();
        this.opendoorIndex = -1;
        if (LocalSave.Instance.BattleIn_GetIn())
        {
            this.currentRoomID = LocalSave.Instance.BattleIn_GetRoomID();
            Room room = new Room();
            room.SetRoomID(this.currentRoomID);
            room.SetTmx(LocalSave.Instance.BattleIn_GetTmxID());
            this.roomList.Add(this.currentRoomID, room);
        }
        else
        {
            this.currentRoomID = 0;
            Room room2 = new Room();
            room2.SetRoomID(this.currentRoomID);
            room2.SetTmx(this.OnGetFirstRoomTMX());
            this.roomList.Add(this.currentRoomID, room2);
            this.UpdateBattleIn();
        }
        this.maxRoomID = GameLogic.Hold.BattleData.mModeData.GetMaxLayer();
        if (!LocalSave.Instance.BattleIn_GetIn())
        {
            CInstance<PlayerPrefsMgr>.Instance.gametime.set_value(0);
            CInstance<PlayerPrefsMgr>.Instance.gametime.flush();
        }
        this.OnStartGame();
        this.GotoNextDoor();
        this.OnStartGameEnd();
    }

    private void UpdateBattleIn()
    {
        LocalSave.Instance.BattleIn_UpdateRoomID(this.currentRoomID);
        LocalSave.Instance.BattleIn_UpdateTmxID(this.roomList[this.currentRoomID].TMXID);
        LocalSave.Instance.BattleIn_UpdateResourcesID(this.roomList[this.currentRoomID].ResourcesID);
        LocalSave.Instance.BattleIn_SetHaveBattle(true);
        float gold = GameLogic.Hold.BattleData.GetGold();
        LocalSave.Instance.BattleIn_UpdateGold(gold);
        LocalSave.Instance.BattleIn_UpdateExp(GameLogic.Self.m_EntityData.GetCurrentExp());
        LocalSave.Instance.BattleIn_UpdateLevel(GameLogic.Self.m_EntityData.GetLevel());
    }

    protected int currentRoomID
    {
        get => 
            this._currentRoomID;
        set
        {
            this._currentRoomID = value;
            GameLogic.Hold.BattleData.BossHPClear();
        }
    }

    public class EventCloseTransfer
    {
        public WindowID windowid;
        public object data;
    }

    public class Room
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <RoomID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <ResourcesID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <RoomHeight>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <TMXID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private RoomGenerateBase.RoomType <RoomType>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <playery>k__BackingField;

        private void ExcuteTmxName()
        {
            this.RoomType = GameLogic.Release.MapCreatorCtrl.CheckTmxID(this.TMXID);
        }

        private void InitPlayerY()
        {
            this.playery = (((float) -this.RoomHeight) / 2f) - 1f;
        }

        private void InitResource()
        {
            this.ResourcesID = GameLogic.Release.MapCreatorCtrl.GetRoomResourceID(this.TMXID);
            this.RoomHeight = GameLogic.Release.MapCreatorCtrl.GetRoomHeight("InitResource", this.TMXID);
        }

        public void SetRoomID(int id)
        {
            this.RoomType = RoomGenerateBase.RoomType.eNormal;
            this.RoomID = id;
            this.ResourcesID = 1;
            this.RoomHeight = 0x15;
        }

        public void SetTmx(string tmxid)
        {
            this.TMXID = tmxid;
            this.ExcuteTmxName();
            this.InitResource();
            this.InitPlayerY();
        }

        public int RoomID { get; private set; }

        public int ResourcesID { get; private set; }

        public int RoomHeight { get; private set; }

        public string TMXID { get; private set; }

        public bool IsBossRoom =>
            (this.RoomType == RoomGenerateBase.RoomType.eBoss);

        public RoomGenerateBase.RoomType RoomType { get; private set; }

        public float playery { get; private set; }
    }

    public enum RoomType
    {
        eInvalid,
        eNormal,
        eEvent,
        eBoss
    }

    public class WaveData
    {
        public Action OnCreateWave;
        private float firstWaveTime;
        private int maxWave;
        private float waveTime;
        private int currentWave;
        private float wavealivetime;
        private Sequence s_wave;

        public WaveData(float firstWaveTime, string[] args)
        {
            this.firstWaveTime = firstWaveTime;
            if (args.Length == 2)
            {
                this.maxWave = int.Parse(args[0]);
                this.waveTime = float.Parse(args[1]);
            }
            else
            {
                this.maxWave = 1;
                this.waveTime = 0f;
            }
        }

        public void Start()
        {
            this.currentWave = 0;
            this.UpdateActiveData();
        }

        public void Stop()
        {
            TweenExtensions.Kill(this.s_wave, false);
        }

        private void UpdateActiveData()
        {
            if (this.currentWave < this.maxWave)
            {
                this.s_wave = DOTween.Sequence();
                if (this.currentWave == 0)
                {
                    TweenSettingsExtensions.AppendInterval(this.s_wave, this.firstWaveTime);
                }
                else
                {
                    TweenSettingsExtensions.AppendInterval(this.s_wave, this.waveTime);
                }
                TweenSettingsExtensions.AppendCallback(this.s_wave, new TweenCallback(this, this.<UpdateActiveData>m__0));
            }
        }

        public bool IsEnd =>
            ((this.currentWave >= this.maxWave) && ((Updater.AliveTime - this.wavealivetime) > 1f));
    }
}

