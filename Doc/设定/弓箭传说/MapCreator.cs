using DG.Tweening;
using Dxx.Util;
using PureMVC.Patterns;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using TableTool;
using UnityEngine;

public class MapCreator
{
    public Action Event_Button1101;
    private static Dictionary<MapGoodType, string> mResList;
    private Dictionary<string, string> mMapStrings = new Dictionary<string, string>();
    private const int TrapUpStart = 0x7d1;
    private const int TrapUpEnd = 0x7d2;
    private const int TrapDownStart = 0x7d3;
    private const int TrapDownEnd = 0x7d4;
    private const int TrapLeftStart = 0x7d5;
    private const int TrapLeftEnd = 0x7d6;
    private const int TrapRightStart = 0x7d7;
    private const int TrapRightEnd = 0x7d8;
    private const int WaterID = 0x3ee;
    private SequencePool mSequencePool = new SequencePool();
    private string MapID;
    private RoomGenerateBase.RoomType mRoomType;
    private RoomControlBase mRoomCtrl;
    public int width;
    public int height;
    private Vector2 CombineOffset;
    private Dictionary<int, Goods_goods> GoodsList = new Dictionary<int, Goods_goods>();
    private Dictionary<int, int> TileMap2Goods = new Dictionary<int, int>();
    private int[,] tiledata;
    private int[,] RoomRealRect;
    private int[,] findpathRect;
    private TMXGoodsData[,] TmxGoodsList;
    private int roomid;
    private List<GameObject> mGoodsList = new List<GameObject>();
    private Transfer mTransfer;
    private static int count;
    public FindPath Bomberman_path;
    private int[,] bomberman_rect;
    private int[,] bomberman_danger;
    private int[,] mCallRect;
    public const int Good_StoneID = 0x3e9;
    public const int Good_GrassID = 0x3ef;
    public const int Good_WoodID = 0x3f1;
    public const int Good_FenceID = 0x3f0;
    private Dictionary<int, Dictionary<int, WeightRandom>> mElementData = new Dictionary<int, Dictionary<int, WeightRandom>>();
    private Dictionary<int, string> mWeightStrings;
    private Dictionary<Vector2Int, HeroModeData> elitelist;
    private Vector2Int GetRoundEmpty_v;
    private List<Vector2Int> GetRoundEmpty_list;
    private List<Vector2Int> sides_resultlist;
    private List<Vector2Int> sides_list;
    private List<Vector2Int> sides_listtemp;
    private List<Vector2Int> line_list;
    private List<Vector2Int> line_listtemp;
    private bool[,] waterchecks;
    private Dictionary<RoomGenerateBase.RoomType, int> waveroom_maxwave;
    private Dictionary<RoomGenerateBase.RoomType, int> waveroom_time;
    private int waveroom_currentwave;
    private bool waveroom_currentwave_createend;
    private bool waveroom_startwave;
    private List<XmlNode> waveroom_nodelist;
    private SequencePool waveroom_pool;
    private BattleLevelWaveData mWaveData;

    static MapCreator()
    {
        Dictionary<MapGoodType, string> dictionary = new Dictionary<MapGoodType, string> {
            { 
                MapGoodType.Soldier,
                "Game/Soldier/SoldierNode"
            },
            { 
                MapGoodType.Boss,
                "Game/Boss/BossNode"
            },
            { 
                MapGoodType.Tower,
                "Game/Tower/TowerNode"
            }
        };
        mResList = dictionary;
        count = 0;
    }

    public MapCreator()
    {
        Dictionary<int, string> dictionary = new Dictionary<int, string> {
            { 
                1,
                "1,7;2,1|1,7;2,1|1,1;2,1|1,1"
            },
            { 
                2,
                "1,15;2,1;3,1|1,9;2,1|1,1|1,1"
            },
            { 
                3,
                "1,7;2,1|1,7;2,1|1,1|1,1"
            },
            { 
                4,
                "1,7;2,1|1,7;2,1|1,1|1,1"
            },
            { 
                5,
                "1,15;2,1|1,9;2,1|1,1|1,1"
            },
            { 
                6,
                "1,7;2,1|1,7;2,1|1,1|1,1"
            },
            { 
                7,
                "1,7;2,1|1,7;2,1|1,1;2,1|1,1"
            },
            { 
                8,
                "1,15;2,1;3,1|1,9;2,1|1,1|1,1"
            },
            { 
                9,
                "1,7;2,1|1,7;2,1|1,1|1,1"
            },
            { 
                10,
                "1,7;2,1|1,7;2,1|1,1;2,1|1,1"
            },
            { 
                11,
                "1,15;2,1|1,9;2,1|1,1|1,1"
            },
            { 
                12,
                "1,7;2,1|1,7;2,1|1,1|1,1"
            }
        };
        this.mWeightStrings = dictionary;
        this.elitelist = new Dictionary<Vector2Int, HeroModeData>();
        this.GetRoundEmpty_list = new List<Vector2Int>();
        this.sides_resultlist = new List<Vector2Int>();
        this.sides_list = new List<Vector2Int>();
        this.line_list = new List<Vector2Int>();
        this.waterchecks = new bool[3, 3];
        this.waveroom_maxwave = new Dictionary<RoomGenerateBase.RoomType, int>();
        this.waveroom_time = new Dictionary<RoomGenerateBase.RoomType, int>();
        this.waveroom_nodelist = new List<XmlNode>();
        this.waveroom_pool = new SequencePool();
        this.mWaveData = new BattleLevelWaveData();
    }

    public void Bomberman_Cache(Vector3 pos)
    {
        Vector2Int roomXY = this.GetRoomXY(pos);
        this.bomberman_rect[roomXY.x, roomXY.y] = 0;
    }

    private void Bomberman_danger_reset()
    {
        for (int i = 0; i < this.width; i++)
        {
            for (int j = 0; j < this.height; j++)
            {
                if (this.bomberman_danger[i, j] <= 10)
                {
                    this.bomberman_danger[i, j] = 0;
                }
            }
        }
    }

    public List<Grid.NodeItem> Bomberman_find_path(Vector3 startpos, Vector3 endpos) => 
        this.Bomberman_path.FindingPath(startpos, endpos);

    public Vector2Int Bomberman_get_safe_near(Vector3 pos)
    {
        <Bomberman_get_safe_near>c__AnonStorey0 storey = new <Bomberman_get_safe_near>c__AnonStorey0 {
            selfpos = this.GetRoomXY(pos)
        };
        this.Bomberman_update_danger();
        List<Vector2Int> list = new List<Vector2Int>();
        for (int i = 0; i < this.width; i++)
        {
            for (int j = 0; j < this.height; j++)
            {
                if (this.bomberman_danger[i, j] == 0)
                {
                    list.Add(new Vector2Int(i, j));
                }
            }
        }
        list.Sort(new Comparison<Vector2Int>(storey.<>m__0));
        if (list.Count > 0)
        {
            int max = MathDxx.Clamp(list.Count, 1, 3);
            int num4 = GameLogic.Random(0, max);
            return list[max];
        }
        return this.RandomRoomXY();
    }

    public int[,] Bomberman_GetDanger() => 
        this.bomberman_danger;

    public int[,] Bomberman_GetRect() => 
        this.bomberman_rect;

    private void Bomberman_Init()
    {
        this.bomberman_rect = (int[,]) this.tiledata.Clone();
        this.bomberman_danger = (int[,]) this.tiledata.Clone();
        this.Bomberman_path = new FindPath();
        this.Bomberman_danger_reset();
    }

    public bool Bomberman_is_danger(Grid.NodeItem item) => 
        this.Bomberman_is_danger(item.x, item.y);

    public bool Bomberman_is_danger(Vector2Int v) => 
        this.Bomberman_is_danger(v.x, v.y);

    public bool Bomberman_is_danger(int x, int y)
    {
        this.Bomberman_update_danger();
        return (this.bomberman_danger[x, y] > 0);
    }

    public bool Bomberman_is_empty(Vector3 pos)
    {
        Vector2Int roomXY = this.GetRoomXY(pos);
        return (this.bomberman_rect[roomXY.x, roomXY.y] == 0);
    }

    private void Bomberman_set_danger(int length, int x, int y)
    {
        for (int i = x - length; i <= (x + length); i++)
        {
            if ((i >= 0) && (i < this.width))
            {
                this.bomberman_danger[i, y] = 1;
            }
        }
        for (int j = y - length; j <= (y + length); j++)
        {
            if ((j >= 0) && (j < this.height))
            {
                this.bomberman_danger[x, j] = 1;
            }
        }
    }

    private void Bomberman_update_danger()
    {
        this.Bomberman_danger_reset();
        for (int i = 0; i < this.width; i++)
        {
            for (int j = 0; j < this.height; j++)
            {
                if (this.bomberman_rect[i, j] > 0)
                {
                    this.Bomberman_set_danger(this.bomberman_rect[i, j], i, j);
                }
            }
        }
        this.Bomberman_path.Init(this.bomberman_danger);
    }

    public void Bomberman_Use(Vector3 pos, int length)
    {
        Vector2Int roomXY = this.GetRoomXY(pos);
        this.bomberman_rect[roomXY.x, roomXY.y] = length;
    }

    public void CallPositionRecover(Vector3 pos)
    {
        Vector2Int roomXY = this.GetRoomXY(pos);
        this.mCallRect[roomXY.x, roomXY.y] = 0;
    }

    public RoomGenerateBase.RoomType CheckTmxID(string TmxID)
    {
        RoomGenerateBase.RoomType eNormal = RoomGenerateBase.RoomType.eNormal;
        if (TmxID.Length > 6)
        {
            switch (TmxID.Substring(6, 1))
            {
                case "B":
                    return RoomGenerateBase.RoomType.eBoss;

                case "E":
                    eNormal = RoomGenerateBase.RoomType.eEvent;
                    break;
            }
        }
        return eNormal;
    }

    private int checkwaterid(string s)
    {
        IList<Goods_water> allBeans = LocalModelManager.Instance.Goods_water.GetAllBeans();
        bool flag = false;
        int num = 0;
        int count = allBeans.Count;
        while (num < count)
        {
            string checkID = allBeans[num].CheckID;
            flag = true;
            int num3 = 0;
            int length = checkID.Length;
            while (num3 < length)
            {
                if ((checkID[num3] != '_') && (checkID[num3] != s[num3]))
                {
                    flag = false;
                    break;
                }
                num3++;
            }
            if (flag)
            {
                int index = GameLogic.Random(0, allBeans[num].WaterID.Length);
                return allBeans[num].WaterID[index];
            }
            num++;
        }
        object[] args = new object[] { s };
        SdkManager.Bugly_Report("MapCreator_Water.checkwaterid", Utils.FormatString("{0} is check error.", args));
        return 0x5dd;
    }

    public void ClearGoods()
    {
        for (int i = this.mGoodsList.Count - 1; i >= 0; i--)
        {
            Object.Destroy(this.mGoodsList[i]);
        }
        this.mGoodsList.Clear();
    }

    private void CreateAllGoods()
    {
        for (int i = 0; i < this.width; i++)
        {
            for (int j = 0; j < this.height; j++)
            {
                if (this.tiledata[i, j] > 0)
                {
                    int goodID = this.GetGoodID(i, j);
                    float num2 = i + this.CombineOffset.x;
                    float num3 = -j + this.CombineOffset.y;
                    this.CreateOneGoods(goodID, i, j);
                }
            }
        }
        this.DealTrap();
        this.DealWater();
        this.heromode_end();
    }

    public EntityBase CreateDivideEntity(EntityBase parent, int entityid, float x, float y)
    {
        CreateData data = new CreateData {
            entityid = entityid,
            x = x,
            y = y,
            v = this.GetRoomXY(x, y),
            bDivide = true
        };
        EntityBase base2 = this.CreateEntityOneWorld(data);
        base2.SetEntityDivide(this.mRoomType);
        return base2;
    }

    public EntityBase CreateEntity(CreateData data)
    {
        GameObject obj2 = Object.Instantiate<GameObject>(ResourceManager.Load<GameObject>(data.path));
        obj2.transform.SetParent(GameNode.m_Monster.transform);
        obj2.transform.position = new Vector3(data.x, 0f, data.y);
        obj2.transform.localScale = Vector3.one;
        obj2.transform.localRotation = Quaternion.identity;
        EntityBase component = obj2.GetComponent<EntityBase>();
        component.SetElite(data.m_bElite);
        component.bCall = data.bCall;
        component.bDivide = data.bDivide;
        component.SetRoomType(this.mRoomType);
        component.Init(data.entityid);
        return component;
    }

    public EntityMonsterBase CreateEntityCall(int entityid, float x, float y)
    {
        CreateData data = new CreateData {
            entityid = entityid,
            x = x,
            y = y,
            v = this.GetRoomXY(x, y),
            bCall = true
        };
        EntityMonsterBase base3 = this.CreateEntity(data) as EntityMonsterBase;
        if (base3 != null)
        {
            base3.StartCall();
        }
        return base3;
    }

    public EntityBase CreateEntityOneWorld(CreateData data)
    {
        EntityBase base2 = this.CreateEntity(data);
        base2.SetPosition(new Vector3(data.x, 0f, data.y));
        return base2;
    }

    private GameObject CreateGood(int GoodsID, int x, int y)
    {
        object[] args = new object[] { "Game/Goods/", GoodsID };
        GameObject original = ResourceManager.Load<GameObject>(Utils.GetString(args));
        if (original == null)
        {
            return null;
        }
        GameObject item = Object.Instantiate<GameObject>(original);
        this.mGoodsList.Add(item);
        item.transform.parent = this.mRoomCtrl.GoodsParent;
        item.transform.localPosition = this.GetWorldPositionUnscale(x, y);
        item.transform.localScale = Vector3.one;
        item.transform.localRotation = Quaternion.identity;
        Transform transform = item.transform.Find("child/good");
        if ((transform != null) && (LocalModelManager.Instance.Goods_goods.GetBeanById(GoodsID).Ground == 0))
        {
            int num = (int) -item.transform.position.z;
            transform.GetComponent<SpriteRenderer>().sortingOrder = num;
        }
        return item;
    }

    public GameObject CreateGoodExtra(int GoodsID, int x, int y) => 
        this.CreateGoodNotTrap(GoodsID, x, y);

    private GameObject CreateGoodNotTrap(int GoodsID, int x, int y)
    {
        if ((GoodsID >= 0x7d1) && (GoodsID <= 0x7d8))
        {
            return null;
        }
        if (GoodsID == 0x3ee)
        {
            return null;
        }
        return this.CreateGood(GoodsID, x, y);
    }

    public void CreateMap(Transfer t)
    {
        this.mTransfer = t;
        this.mGoodsList.Clear();
        this.Event_Button1101 = null;
        this.mRoomCtrl = this.mTransfer.roomctrl;
        this.roomid = this.mTransfer.roomid;
        int resourcesid = this.mTransfer.resourcesid;
        this.MapID = this.mTransfer.tmxid;
        this.mRoomType = this.mTransfer.roomtype;
        if (this.mRoomType == RoomGenerateBase.RoomType.eInvalid)
        {
            this.mRoomType = this.CheckTmxID(this.mTransfer.tmxid);
        }
        Facade.Instance.SendNotification("BATTLE_ROOM_TYPE", this.mRoomType);
        float x = LocalModelManager.Instance.Room_room.GetBeanById(resourcesid).GoodsOffset[0];
        float y = LocalModelManager.Instance.Room_room.GetBeanById(resourcesid).GoodsOffset[1];
        this.CombineOffset = new Vector2(x, y);
        this.InitTiledMap2Goods();
        bool flag = LocalModelManager.Instance.Stage_Level_stagechapter.is_wave_room();
        if (flag && (this.mRoomType != RoomGenerateBase.RoomType.eEvent))
        {
            this.waveroom_init();
        }
        else
        {
            this.readTileMap();
        }
        this.InitHeroMode();
        if (flag)
        {
            this.waveroom_create_good();
        }
        else if (this.mTransfer.delay)
        {
            Sequence seq = TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(TweenSettingsExtensions.AppendCallback(DOTween.Sequence(), new TweenCallback(this, this.<CreateMap>m__0)), 0.9f), new TweenCallback(this, this.CreateAllGoods));
            this.mSequencePool.Add(seq);
        }
        else
        {
            this.CreateAllGoods();
        }
    }

    public void CreateOneGoods(int GoodsID, int x, int y)
    {
        switch (GetGoodType(GoodsID))
        {
            case MapGoodType.Goods:
                this.CreateGoodNotTrap(GoodsID, x, y);
                break;

            case MapGoodType.Soldier:
            case MapGoodType.Boss:
            case MapGoodType.Tower:
                if (!LocalSave.Instance.BattleIn_GetIn())
                {
                    CreateData data = new CreateData {
                        m_bElite = this.heromode_is_elite(x, y),
                        entityid = GoodsID
                    };
                    Vector3 worldPosition = this.GetWorldPosition(x, y);
                    data.x = worldPosition.x;
                    data.y = worldPosition.z;
                    data.v = new Vector2Int(x, y);
                    this.CreateEntity(data);
                    break;
                }
                return;

            case MapGoodType.Event:
                if (!LocalSave.Instance.BattleIn_GetIn())
                {
                    this.CreateGoodNotTrap(GoodsID, x, y);
                    break;
                }
                return;
        }
    }

    private void CreateWater(int x, int y)
    {
        object[] args = new object[] { "Game/Goods/1006" };
        GameObject item = Object.Instantiate<GameObject>(ResourceManager.Load<GameObject>(Utils.GetString(args)));
        this.mGoodsList.Add(item);
        item.transform.parent = this.mRoomCtrl.GoodsParent;
        float num = x + this.CombineOffset.x;
        float z = -y + this.CombineOffset.y;
        item.transform.localPosition = new Vector3(num, 0f, z);
        item.transform.localScale = Vector3.one;
        item.transform.localRotation = Quaternion.identity;
        Transform transform = item.transform.Find("child/good");
        if (transform != null)
        {
            SpriteRenderer component = transform.GetComponent<SpriteRenderer>();
            if (component != null)
            {
                int num3 = this.water_checkround(x, y);
                object[] objArray2 = new object[] { num3 };
                component.sprite = SpriteManager.GetMap(Utils.FormatString("water{0}", objArray2));
            }
        }
    }

    private void DealTrap()
    {
        for (int i = 0; i < this.width; i++)
        {
            for (int j = 0; j < this.height; j++)
            {
                if (this.tiledata[i, j] <= 0)
                {
                    continue;
                }
                int goodID = this.GetGoodID(i, j);
                if (((goodID == 0x7d1) || (goodID == 0x7d3)) || ((goodID == 0x7d5) || (goodID == 0x7d7)))
                {
                    GameObject obj2 = this.CreateGood(goodID, i, j);
                    int x = i;
                    int y = j;
                    switch (goodID)
                    {
                        case 0x7d1:
                            y = this.FindTrapUpEnd(i, j);
                            this.CreateGood(0x7dc, i, j);
                            this.CreateGood(0x7db, i, y);
                            break;

                        case 0x7d3:
                            y = this.FindTrapDownEnd(i, j);
                            this.CreateGood(0x7db, i, j);
                            this.CreateGood(0x7dc, i, y);
                            break;

                        case 0x7d5:
                            x = this.FindTrapLeftEnd(i, j);
                            this.CreateGood(0x7de, i, j);
                            this.CreateGood(0x7dd, x, j);
                            break;

                        case 0x7d7:
                            x = this.FindTrapRightEnd(i, j);
                            this.CreateGood(0x7dd, i, j);
                            this.CreateGood(0x7de, x, j);
                            break;
                    }
                    obj2.GetComponent<Goods2001>().SetEndPosition(this.GetWorldPositionUnscale(x, y));
                }
            }
        }
    }

    private void DealWater()
    {
        for (int i = 0; i < this.width; i++)
        {
            for (int j = 0; j < this.height; j++)
            {
                if (this.IsWater(i, j))
                {
                    this.CreateWater(i, j);
                }
            }
        }
    }

    public void Deinit()
    {
        this.mMapStrings.Clear();
        this.mSequencePool.Clear();
        this.waveroom_deinit();
    }

    public bool ExcuteRelativeDirection(Vector2Int from, Vector2Int to, out Vector2Int dir, bool fly = false)
    {
        if (from.x == to.x)
        {
            int num = (from.y <= to.y) ? from.y : to.y;
            int num2 = (from.y >= to.y) ? from.y : to.y;
            if ((this.IsValid(from) && this.IsValid(to)) && !fly)
            {
                for (int i = num + 1; i < num2; i++)
                {
                    if (this.findpathRect[from.x, i] != 0)
                    {
                        dir = new Vector2Int();
                        return false;
                    }
                }
            }
            if (from.y >= to.y)
            {
                dir = new Vector2Int(0, 1);
            }
            else
            {
                dir = new Vector2Int(0, -1);
            }
            return true;
        }
        if (from.y == to.y)
        {
            int num4 = (from.x <= to.x) ? from.x : to.x;
            int num5 = (from.x >= to.x) ? from.x : to.x;
            if ((this.IsValid(from) && this.IsValid(to)) && !fly)
            {
                for (int i = num4 + 1; i < num5; i++)
                {
                    if (this.findpathRect[i, from.y] != 0)
                    {
                        dir = new Vector2Int();
                        return false;
                    }
                }
            }
            if (from.x >= to.x)
            {
                dir = new Vector2Int(-1, 0);
            }
            else
            {
                dir = new Vector2Int(1, 0);
            }
            return true;
        }
        dir = new Vector2Int();
        return false;
    }

    private int FindTrapDownEnd(int x, int y)
    {
        for (int i = y + 1; i < this.height; i++)
        {
            int num2 = this.tiledata[x, i];
            if (num2 > 0)
            {
                int num3 = this.TileMap2Goods[(num2 % 0x2710) - 1];
                if (num3 == 0x7d4)
                {
                    return i;
                }
            }
        }
        return -1;
    }

    private int FindTrapLeftEnd(int x, int y)
    {
        for (int i = x - 1; i >= 0; i--)
        {
            int num2 = this.tiledata[i, y];
            if (num2 > 0)
            {
                int num3 = this.TileMap2Goods[(num2 % 0x2710) - 1];
                if (num3 == 0x7d6)
                {
                    return i;
                }
            }
        }
        return -1;
    }

    private int FindTrapRightEnd(int x, int y)
    {
        for (int i = x + 1; i < this.width; i++)
        {
            int num2 = this.tiledata[i, y];
            if (num2 > 0)
            {
                int num3 = this.TileMap2Goods[(num2 % 0x2710) - 1];
                if (num3 == 0x7d8)
                {
                    return i;
                }
            }
        }
        return -1;
    }

    private int FindTrapUpEnd(int x, int y)
    {
        for (int i = y - 1; i >= 0; i--)
        {
            int num2 = this.tiledata[x, i];
            if (num2 > 0)
            {
                int num3 = this.TileMap2Goods[(num2 % 0x2710) - 1];
                if (num3 == 0x7d2)
                {
                    return i;
                }
            }
        }
        return -1;
    }

    private int get_max_wave()
    {
        if (this.waveroom_maxwave.TryGetValue(this.mRoomType, out int num))
        {
            return num;
        }
        return -1;
    }

    private XmlNode get_node(XmlNodeList nodes)
    {
        XmlNode node = null;
        if (node == null)
        {
            int num = GameLogic.Random(0, nodes.Count);
            node = nodes[num];
        }
        return node;
    }

    private int get_time()
    {
        if (this.waveroom_time.TryGetValue(this.mRoomType, out int num))
        {
            return num;
        }
        return -1;
    }

    public bool GetCanCall(EntityBase entity, int radiusmin, int radiusmax)
    {
        for (int i = radiusmin; i <= radiusmax; i++)
        {
            if (this.GetRoundSideEmpty(this.mCallRect, entity.position, i).Count > 0)
            {
                return true;
            }
        }
        return false;
    }

    private List<Vector2Int> GetCreatePositionList()
    {
        List<Vector2Int> list = new List<Vector2Int>();
        for (int i = 0; i < this.width; i++)
        {
            for (int j = 0; j < this.height; j++)
            {
                if (this.tiledata[i, j] > 0)
                {
                    int goodID = this.GetGoodID(i, j);
                    if (this.IsMonster(goodID))
                    {
                        list.Add(new Vector2Int(i, j));
                    }
                }
            }
        }
        return list;
    }

    public Sprite GetElementShadow(int elementid)
    {
        object[] args = new object[] { elementid };
        return SpriteManager.GetMap(Utils.FormatString("elementshadow{0:D2}", args));
    }

    public int[,] GetFindPathRect() => 
        this.findpathRect;

    private int GetGoodID(int i, int j)
    {
        int key = (this.tiledata[i, j] % 0x2710) - 1;
        int num2 = -1;
        this.TileMap2Goods.TryGetValue(key, out num2);
        return num2;
    }

    private Goods_goods GetGoods(int GoodsID)
    {
        if (!this.GoodsList.ContainsKey(GoodsID))
        {
            this.GoodsList.Add(GoodsID, LocalModelManager.Instance.Goods_goods.GetBeanById(GoodsID));
        }
        if (this.GoodsList.ContainsKey(GoodsID))
        {
            return this.GoodsList[GoodsID];
        }
        return null;
    }

    public static MapGoodType GetGoodType(int goodid)
    {
        if ((goodid > 0) && (goodid < 0x7d0))
        {
            return MapGoodType.Goods;
        }
        if ((goodid >= 0x7d9) && (goodid <= 0x7df))
        {
            return MapGoodType.Goods;
        }
        if ((goodid >= 0xbb8) && (goodid < 0x1388))
        {
            return MapGoodType.Soldier;
        }
        if ((goodid >= 0x1388) && (goodid < 0x1770))
        {
            return MapGoodType.Boss;
        }
        if ((goodid >= 0x1f40) && (goodid < 0x2328))
        {
            return MapGoodType.Tower;
        }
        if (goodid > 0x2328)
        {
            return MapGoodType.Event;
        }
        return MapGoodType.Empty;
    }

    private List<Vector2Int> GetHorizontalEmpty(EntityBase entity)
    {
        List<Vector2Int> list = new List<Vector2Int>();
        Vector2Int roomXY = this.GetRoomXY(entity.position);
        for (int i = 0; i < this.width; i++)
        {
            if (this.findpathRect[i, roomXY.y] == 0)
            {
                list.Add(new Vector2Int(i, roomXY.y));
            }
        }
        return list;
    }

    public List<Vector2Int> GetNearEmptyList(Vector3 pos, ref Vector2Int GoldsCenter, int radius)
    {
        List<Vector2Int> list = new List<Vector2Int>();
        Vector2Int roomXY = this.GetRoomXY(pos);
        GoldsCenter = this.GetNearestEmpty(roomXY);
        for (int i = GoldsCenter.x - radius; i <= (GoldsCenter.x + radius); i++)
        {
            for (int j = GoldsCenter.y - radius; j <= (GoldsCenter.y + radius); j++)
            {
                if ((((i >= 0) && (i < this.width)) && ((j >= 0) && (j < this.height))) && this.IsEmpty(i, j))
                {
                    list.Add(new Vector2Int(i, j));
                }
            }
        }
        if (list.Count == 0)
        {
            return this.GetNearEmptyList(pos, ref GoldsCenter, radius + 1);
        }
        return list;
    }

    private Vector2Int GetNearestEmpty(Vector2Int vInt)
    {
        Vector2Int v = new Vector2Int();
        for (int i = 0; i < 0x15; i++)
        {
            if (this.GetNearestEmpty(vInt, i, ref v))
            {
                return v;
            }
        }
        return v;
    }

    private bool GetNearestEmpty(Vector2Int vInt, int count, ref Vector2Int v)
    {
        v = new Vector2Int(vInt.x, vInt.y);
        if ((count != 0) || !this.IsEmpty(v))
        {
            List<Vector2Int> list = new List<Vector2Int>();
            int x = vInt.x - count;
            if (x < 0)
            {
                x = 0;
            }
            else if (x >= this.width)
            {
                return false;
            }
            int width = vInt.x + count;
            if (width > this.width)
            {
                width = this.width;
            }
            int y = vInt.y - count;
            if (y < 0)
            {
                y = 0;
            }
            else if (y >= this.height)
            {
                return false;
            }
            int height = vInt.y + count;
            if (height > this.height)
            {
                height = this.height;
            }
            while (x <= width)
            {
                while (y <= height)
                {
                    if (((MathDxx.Abs((int) (x - vInt.x)) >= count) || (MathDxx.Abs((int) (y - vInt.y)) >= count)) && this.IsEmpty(x, y))
                    {
                        list.Add(new Vector2Int(x, y));
                    }
                    y++;
                }
                x++;
            }
            if (list.Count <= 0)
            {
                return false;
            }
            int num5 = 0x7fffffff;
            int num7 = 0;
            for (x = 0; x < list.Count; x++)
            {
                Vector2Int num8 = list[x];
                Vector2Int num9 = list[x];
                int num6 = MathDxx.Abs((int) (num8.x - vInt.x)) + MathDxx.Abs((int) (num9.y - vInt.y));
                if (num6 < num5)
                {
                    num5 = num6;
                    num7 = x;
                }
            }
            v = list[num7];
        }
        return true;
    }

    public Sprite GetRandomElement(int elementid)
    {
        int randomElementID = this.GetRandomElementID(elementid);
        object[] args = new object[] { elementid, randomElementID };
        return SpriteManager.GetMap(Utils.FormatString("element{0:D2}{1:D2}", args));
    }

    private int GetRandomElementID(int elementid)
    {
        this.InitElementData(elementid);
        int chapter = GameLogic.Hold.BattleData.Level_CurrentStage;
        int tiledID = LocalModelManager.Instance.Stage_Level_stagechapter.GetTiledID(chapter);
        return this.mElementData[tiledID][elementid].GetRandom();
    }

    public int[,] GetRects() => 
        this.tiledata;

    public int GetRoomHeight(string source, string tmxid)
    {
        XmlDocument document = new XmlDocument();
        string tmxString = this.GetTmxString(tmxid);
        if (string.IsNullOrEmpty(tmxString))
        {
            object[] args = new object[] { source, tmxid };
            SdkManager.Bugly_Report("GetRoomHeight", Utils.FormatString("source : [{0}] the tmxpath:[{1}] is not found!", args));
            return 11;
        }
        int num = 11;
        try
        {
            document.LoadXml(tmxString);
            num = int.Parse(document.SelectSingleNode("map").Attributes["height"].Value);
        }
        catch
        {
            object[] args = new object[] { source, tmxid, GameLogic.Hold.BattleData.Level_CurrentStage };
            SdkManager.Bugly_Report("GetRoomHeight", Utils.FormatString("source : {0} GetTmxString try the tmxpath:[{1}] stage:{2} is error!", args));
            string tmxPath = this.GetTmxPath(tmxid);
            if (this.mMapStrings.ContainsKey(tmxPath))
            {
                this.mMapStrings.Remove(tmxPath);
                return this.GetRoomHeight("GetRoomHeight", tmxid);
            }
        }
        return num;
    }

    public int GetRoomResourceID(string tmxid)
    {
        switch (this.GetRoomHeight("GetRoomResourceID", tmxid))
        {
            case 11:
                return 3;

            case 15:
                return 2;

            case 0x15:
                return 1;
        }
        return 1;
    }

    public Vector2Int GetRoomXY(Vector3 pos)
    {
        Vector2Int num = new Vector2Int();
        float num2 = pos.x - this.CombineOffset.x;
        int num3 = ((int) (num2 * 10f)) % 10;
        if (num3 < 5)
        {
            num.x = (int) num2;
        }
        else
        {
            num.x = ((int) num2) + 1;
        }
        float num4 = this.CombineOffset.y - (pos.z / 1.23f);
        int num5 = ((int) (num4 * 10f)) % 10;
        if (num5 < 5)
        {
            num.y = (int) num4;
        }
        else
        {
            num.y = ((int) num4) + 1;
        }
        num.x = MathDxx.Clamp(num.x, 0, this.width - 1);
        num.y = MathDxx.Clamp(num.y, 0, this.height - 1);
        return num;
    }

    public Vector2Int GetRoomXY(float x, float z) => 
        this.GetRoomXY(new Vector3(x, 0f, z));

    public Vector2Int GetRoomXYInside(Vector3 pos)
    {
        Vector2Int roomXY = this.GetRoomXY(pos);
        roomXY.x = MathDxx.Clamp(roomXY.x, 1, this.width - 1);
        roomXY.y = MathDxx.Clamp(roomXY.y, 1, this.height - 1);
        return roomXY;
    }

    public List<Vector2Int> GetRoundEmpty(Vector3 pos, int size)
    {
        this.GetRoundEmpty_v = this.GetRoomXY(pos);
        this.GetRoundEmpty_list.Clear();
        int x = this.GetRoundEmpty_v.x - size;
        int num2 = (this.GetRoundEmpty_v.x + size) + 1;
        while (x < num2)
        {
            if ((x >= 0) && (x < this.width))
            {
                int y = this.GetRoundEmpty_v.y - size;
                int num4 = (this.GetRoundEmpty_v.y + size) + 1;
                while (y < num4)
                {
                    if (((y >= 0) && (y < this.height)) && (this.findpathRect[x, y] == 0))
                    {
                        this.GetRoundEmpty_list.Add(new Vector2Int(x, y));
                    }
                    y++;
                }
            }
            x++;
        }
        return this.GetRoundEmpty_list;
    }

    public List<Vector2Int> GetRoundFly(Vector3 pos, int size)
    {
        this.GetRoundEmpty_v = this.GetRoomXY(pos);
        this.GetRoundEmpty_list.Clear();
        int x = this.GetRoundEmpty_v.x - size;
        int num2 = (this.GetRoundEmpty_v.x + size) + 1;
        while (x < num2)
        {
            if ((x >= 0) && (x < this.width))
            {
                int y = this.GetRoundEmpty_v.y - size;
                int num4 = (this.GetRoundEmpty_v.y + size) + 1;
                while (y < num4)
                {
                    if ((((y >= 0) && (y < this.height)) && ((this.findpathRect[x, y] != 0x3e9) && (this.findpathRect[x, y] != 0x3ef))) && ((this.findpathRect[x, y] != 0x3f1) && (this.findpathRect[x, y] != 0x3f0)))
                    {
                        this.GetRoundEmpty_list.Add(new Vector2Int(x, y));
                    }
                    y++;
                }
            }
            x++;
        }
        return this.GetRoundEmpty_list;
    }

    public List<Vector3> GetRoundNotSame(Vector3 selfwordpos, int range, int count)
    {
        List<Vector3> list = new List<Vector3>();
        Vector2Int roomXY = this.GetRoomXY(selfwordpos);
        Vector3 worldPosition = GameLogic.Release.MapCreatorCtrl.GetWorldPosition(roomXY);
        list.Add(worldPosition);
        for (int i = 0; i < count; i++)
        {
            this.RandomItem(selfwordpos, range, out float num3, out float num4);
            while (this.GetRoundNotSame_CheckSame(list, num3, num4))
            {
                this.RandomItem(selfwordpos, range, out num3, out num4);
            }
            list.Add(new Vector3(num3, 0f, num4));
        }
        list.Remove(worldPosition);
        return list;
    }

    private bool GetRoundNotSame_CheckSame(List<Vector3> list, float endx, float endz)
    {
        int num = 0;
        int count = list.Count;
        while (num < count)
        {
            Vector3 vector = list[num];
            if (vector.x == endx)
            {
                Vector3 vector2 = list[num];
                if (vector2.z == endz)
                {
                    return true;
                }
            }
            num++;
        }
        return false;
    }

    public List<Vector2Int> GetRoundSideEmpty(int[,] rects, Vector3 pos, int size)
    {
        Vector2Int roomXY = this.GetRoomXY(pos);
        this.sides_resultlist.Clear();
        int num2 = Mathf.Clamp(roomXY.x - size, 0, this.width);
        int num3 = Mathf.Clamp((roomXY.x + size) + 1, 0, this.width);
        int num4 = Mathf.Clamp(roomXY.y - size, 0, this.height);
        int num5 = Mathf.Clamp((roomXY.y + size) + 1, 0, this.height);
        int num6 = roomXY.x - size;
        int num7 = roomXY.x + size;
        int num8 = roomXY.y - size;
        int num9 = roomXY.y + size;
        for (int i = num2; i < num3; i++)
        {
            for (int j = num4; j < num5; j++)
            {
                if ((((i == num6) || (i == num7)) || ((j == num8) || (j == num9))) && (rects[i, j] == 0))
                {
                    this.sides_resultlist.Add(new Vector2Int(i, j));
                }
            }
        }
        return this.sides_resultlist;
    }

    public bool GetStage3MiddleWater()
    {
        int num = this.width / 2;
        int num2 = num - 1;
        int num3 = num + 1;
        while (num2 <= num3)
        {
            for (int i = 0; i < this.height; i++)
            {
                if (this.findpathRect[num2, i] == 0x3ee)
                {
                    return true;
                }
            }
            num2++;
        }
        return false;
    }

    private int[,] GetTileData(string tmxid)
    {
        XmlDocument document = new XmlDocument();
        string tmxString = this.GetTmxString(this.MapID);
        document.LoadXml(tmxString);
        XmlNode node = document.SelectSingleNode("map/tileset");
        XmlAttributeCollection attributes = document.SelectSingleNode("map").Attributes;
        int num = int.Parse(attributes["width"].Value);
        int num2 = int.Parse(attributes["height"].Value);
        int[,] numArray = new int[num, num2];
        char[] separator = new char[] { '\n' };
        string[] strArray = document.SelectSingleNode("map/layer/data").InnerText.Split(separator);
        for (int i = 1; i < (strArray.Length - 1); i++)
        {
            char[] chArray2 = new char[] { ',' };
            string[] strArray2 = strArray[i].Split(chArray2);
            for (int j = 0; j < num; j++)
            {
                numArray[j, i - 1] = int.Parse(strArray2[j]);
            }
        }
        return numArray;
    }

    private string GetTmxPath(string tmxid)
    {
        if (tmxid == string.Empty)
        {
            object[] objArray1 = new object[] { GameLogic.Hold.BattleData.Level_CurrentStage, GameLogic.Release.Mode.GetMode().ToString(), GameLogic.Release.Mode.RoomGenerate.GetCurrentRoomID() };
            SdkManager.Bugly_Report("MapCreator.GetTmxPath", Utils.FormatString("stage:{0} mode:{1} roomid:{2} random a empty tmxid!!!", objArray1));
        }
        if ((tmxid == "firstroom") || tmxid.Contains("emptyroom"))
        {
            object[] objArray2 = new object[] { tmxid };
            return Utils.FormatString("Game/Map/Tiled/{0}", objArray2);
        }
        GameMode mode = GameLogic.Release.Mode.GetMode();
        if (mode != GameMode.eLevel)
        {
            if ((mode == GameMode.eGold1) || (mode == GameMode.eChest1))
            {
                int difficult = LocalModelManager.Instance.Stage_Level_activity.GetBeanById(GameLogic.Hold.BattleData.ActiveID).Difficult;
                object[] objArray4 = new object[] { GameLogic.Release.Mode.GetMode().ToString(), difficult, tmxid };
                return Utils.FormatString("Game/Map/Tiled/{0}/{1}/{2}", objArray4);
            }
            if (GameLogic.Hold.BattleData.Challenge_ismainchallenge())
            {
                int num3 = LocalModelManager.Instance.Stage_Level_stagechapter.GetTiledID(GameLogic.Hold.BattleData.Level_CurrentStage);
                object[] objArray5 = new object[] { GameMode.eLevel.ToString(), num3, tmxid };
                return Utils.FormatString("Game/Map/Tiled/{0}/Main{1}/{2}", objArray5);
            }
            object[] objArray6 = new object[] { GameLogic.Release.Mode.GetMode().ToString(), 1, tmxid };
            return Utils.FormatString("Game/Map/Tiled/{0}/{1}/{2}", objArray6);
        }
        int tiledID = LocalModelManager.Instance.Stage_Level_stagechapter.GetTiledID(GameLogic.Hold.BattleData.Level_CurrentStage);
        object[] args = new object[] { GameLogic.Release.Mode.GetMode().ToString(), tiledID, tmxid };
        return Utils.FormatString("Game/Map/Tiled/{0}/Main{1}/{2}", args);
    }

    private string GetTmxString(string tmxid)
    {
        string tmxPath = this.GetTmxPath(tmxid);
        string text = string.Empty;
        if (this.mMapStrings.TryGetValue(tmxPath, out text))
        {
            if (!string.IsNullOrEmpty(text))
            {
                return text;
            }
            object[] objArray1 = new object[] { tmxPath };
            SdkManager.Bugly_Report("MapCreator_GetTmxString", Utils.FormatString("mMapStrings.Try first [{0}] is null!", objArray1));
        }
        object[] args = new object[] { "data/tiledmap", GameLogic.Release.Mode.GetMode().ToString(), GameLogic.Hold.BattleData.Level_CurrentStage };
        string dir = Utils.FormatString("{0}/{1}/Main{2}", args);
        object[] objArray3 = new object[] { dir, tmxid };
        string str4 = Utils.FormatString("{0}/{1}.txt", objArray3);
        object[] objArray4 = new object[] { tmxid };
        byte[] fileBytes = FileUtils.GetFileBytes(dir, Utils.FormatString("{0}.txt", objArray4));
        if (fileBytes != null)
        {
            text = Encoding.Default.GetString(fileBytes);
            if (!string.IsNullOrEmpty(text))
            {
                if (!this.mMapStrings.ContainsKey(tmxPath))
                {
                    this.mMapStrings.Add(tmxPath, text);
                }
                return text;
            }
        }
        TextAsset asset = ResourceManager.Load<TextAsset>(tmxPath);
        if (asset != null)
        {
            text = asset.text;
            if (!this.mMapStrings.ContainsKey(tmxPath))
            {
                this.mMapStrings.Add(tmxPath, text);
            }
            return text;
        }
        object[] objArray5 = new object[] { tmxPath };
        SdkManager.Bugly_Report("MapCreator_GetTmxString", Utils.FormatString("ResourceManager.Load[{0}] is null!", objArray5));
        return string.Empty;
    }

    private List<Vector2Int> GetVerticalEmpty(EntityBase entity)
    {
        List<Vector2Int> list = new List<Vector2Int>();
        Vector2Int roomXY = this.GetRoomXY(entity.position);
        for (int i = 0; i < this.width; i++)
        {
            if (this.findpathRect[roomXY.x, i] == 0)
            {
                list.Add(new Vector2Int(roomXY.x, i));
            }
        }
        return list;
    }

    private int GetWaterID(bool[,] checks)
    {
        string key = string.Empty;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (checks[j, i])
                {
                    key = key + "1";
                }
                else
                {
                    key = key + "0";
                }
            }
        }
        Goods_water beanById = LocalModelManager.Instance.Goods_water.GetBeanById(key);
        if (beanById != null)
        {
            int index = GameLogic.Random(0, beanById.WaterID.Length);
            return beanById.WaterID[index];
        }
        return this.checkwaterid(key);
    }

    public Vector3 GetWorldPosition(Vector2Int v2) => 
        this.GetWorldPosition(v2.x, v2.y);

    public Vector3 GetWorldPosition(int x, int y)
    {
        Vector3 worldPositionUnscale = this.GetWorldPositionUnscale(x, y);
        return new Vector3(worldPositionUnscale.x, 0f, worldPositionUnscale.z * 1.23f);
    }

    public Vector3 GetWorldPositionUnscale(Vector2Int v) => 
        this.GetWorldPositionUnscale(v.x, v.y);

    public Vector3 GetWorldPositionUnscale(int x, int y) => 
        new Vector3(x + this.CombineOffset.x, 0f, this.CombineOffset.y - y);

    public bool HaveTmx(string tmxid) => 
        !this.GetTmxString(tmxid).Equals(string.Empty);

    private bool heromode_can_add(int entityid)
    {
        if (entityid == 0xbc8)
        {
            return false;
        }
        return true;
    }

    private void heromode_end()
    {
        this.elitelist.Clear();
    }

    private bool heromode_is_elite(int x, int y)
    {
        Vector2Int key = new Vector2Int(x, y);
        return (this.elitelist.TryGetValue(key, out HeroModeData data) && data.m_bElite);
    }

    private void InitElementData(int id)
    {
        int chapter = GameLogic.Hold.BattleData.Level_CurrentStage;
        int tiledID = LocalModelManager.Instance.Stage_Level_stagechapter.GetTiledID(chapter);
        if (!this.mElementData.ContainsKey(tiledID))
        {
            Dictionary<int, WeightRandom> dictionary = new Dictionary<int, WeightRandom>();
            char[] separator = new char[] { '|' };
            string[] strArray = this.mWeightStrings[tiledID].Split(separator);
            int index = 0;
            int length = strArray.Length;
            while (index < length)
            {
                WeightRandom random = new WeightRandom();
                char[] chArray2 = new char[] { ';' };
                string[] strArray2 = strArray[index].Split(chArray2);
                int num5 = 0;
                int num6 = strArray2.Length;
                while (num5 < num6)
                {
                    char[] chArray3 = new char[] { ',' };
                    string[] strArray3 = strArray2[num5].Split(chArray3);
                    int num7 = int.Parse(strArray3[0]);
                    int weight = int.Parse(strArray3[1]);
                    random.Add(num7, weight);
                    num5++;
                }
                dictionary.Add(index + 1, random);
                index++;
            }
            this.mElementData.Add(tiledID, dictionary);
        }
    }

    private void InitHeroMode()
    {
        if (GameLogic.Hold.BattleData.IsHeroMode())
        {
            this.elitelist.Clear();
            List<bool> list = new List<bool>();
            for (int i = 0; i < this.width; i++)
            {
                for (int m = 0; m < this.height; m++)
                {
                    if (this.tiledata[i, m] > 0)
                    {
                        int goodID = this.GetGoodID(i, m);
                        if ((GetGoodType(goodID) == MapGoodType.Soldier) && this.heromode_can_add(goodID))
                        {
                            Vector2Int key = new Vector2Int(i, m);
                            HeroModeData data = new HeroModeData {
                                roompos = key,
                                entityid = goodID
                            };
                            this.elitelist.Add(key, data);
                            list.Add(false);
                        }
                    }
                }
            }
            int num5 = MathDxx.Clamp(GameLogic.Random(1, 3), 0, this.elitelist.Count);
            for (int j = 0; j < num5; j++)
            {
                list[j] = true;
            }
            list.RandomSort<bool>();
            Dictionary<Vector2Int, HeroModeData>.Enumerator enumerator = this.elitelist.GetEnumerator();
            for (int k = 0; enumerator.MoveNext(); k++)
            {
                KeyValuePair<Vector2Int, HeroModeData> current = enumerator.Current;
                current.Value.m_bElite = list[k];
            }
        }
    }

    private void initRoomRealRectGoods()
    {
        this.RoomRealRect = (int[,]) this.tiledata.Clone();
        this.TmxGoodsList = new TMXGoodsData[this.width, this.height];
        for (int i = 0; i < this.height; i++)
        {
            for (int k = this.width - 1; k >= 0; k--)
            {
                this.TmxGoodsList[k, i] = new TMXGoodsData();
                if (this.RoomRealRect[k, i] > 0)
                {
                    int key = (this.RoomRealRect[k, i] % 0x2710) - 1;
                    if (this.TileMap2Goods.TryGetValue(key, out int num))
                    {
                        this.TmxGoodsList[k, i].SetGoodsId(num);
                        if (num != 0)
                        {
                            switch (GetGoodType(num))
                            {
                                case MapGoodType.Goods:
                                case MapGoodType.Event:
                                {
                                    Goods_goods goods = this.GetGoods(num);
                                    if (goods == null)
                                    {
                                        object[] args = new object[] { num };
                                        SdkManager.Bugly_Report("MapCreator_Rect.initRoomRealRectGoods", Utils.FormatString("goods is null {0}", args));
                                    }
                                    this.TmxGoodsList[k, i].Init(goods.GoodsType);
                                    for (int m = 0; m > -goods.SizeY; m--)
                                    {
                                        for (int n = 0; n < goods.SizeX; n++)
                                        {
                                            this.RoomRealRect[k + n, i + m] = this.RoomRealRect[k, i];
                                            this.TmxGoodsList[k + n, i + m].Init(goods.GoodsType);
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
        this.findpathRect = (int[,]) this.RoomRealRect.Clone();
        for (int j = 0; j < this.height; j++)
        {
            for (int k = this.width - 1; k >= 0; k--)
            {
                if (this.findpathRect[k, j] > 0)
                {
                    int key = (this.RoomRealRect[k, j] % 0x2710) - 1;
                    if (this.TileMap2Goods.TryGetValue(key, out num))
                    {
                        if ((num > 0x3e8) && (num < 0x7d0))
                        {
                            this.findpathRect[k, j] = num;
                        }
                        else
                        {
                            this.findpathRect[k, j] = 0;
                        }
                    }
                }
            }
        }
        this.mCallRect = (int[,]) this.findpathRect.Clone();
    }

    private void InitTiledMap2Goods()
    {
        if (this.TileMap2Goods.Count <= 1)
        {
            this.ReadTiledMapTSX("map_boss");
            this.ReadTiledMapTSX("map_monster");
            this.ReadTiledMapTSX("map_good");
        }
    }

    public bool IsEmpty(Vector2Int v) => 
        this.IsEmpty(v.x, v.y);

    public bool IsEmpty(int x, int y) => 
        ((((x >= 0) && (x < this.width)) && ((y >= 0) && (y < this.height))) && this.TmxGoodsList[x, y].IsEmpty());

    public bool IsEmpty(bool flystone, bool flywater, int x, int y)
    {
        if ((x < 0) || (x >= this.width))
        {
            return false;
        }
        if ((y < 0) || (y >= this.height))
        {
            return false;
        }
        if (!flystone && (this.TmxGoodsList[x, y].ParentType == TMXGoodsParentType.Obstacle_GroundUp))
        {
            return false;
        }
        if (!flywater && (this.TmxGoodsList[x, y].ParentType == TMXGoodsParentType.Obstacle_GroundDown))
        {
            return false;
        }
        return true;
    }

    private bool ishave(List<Vector3> list, Vector3 pos)
    {
        int num = 0;
        int count = list.Count;
        while (num < count)
        {
            Vector3 vector = list[num];
            if (vector.x == pos.x)
            {
                Vector3 vector2 = list[num];
                if (vector2.z == pos.z)
                {
                    return true;
                }
            }
            num++;
        }
        return false;
    }

    private bool IsMonster(int goodsid)
    {
        MapGoodType goodType = GetGoodType(goodsid);
        return (((goodType == MapGoodType.Soldier) || (goodType == MapGoodType.Boss)) || (goodType == MapGoodType.Tower));
    }

    public bool IsValid(Vector2Int v) => 
        this.IsValid(v.x, v.y);

    public bool IsValid(int x, int y) => 
        ((((x >= 0) && (x < this.width)) && (y >= 0)) && (y < this.height));

    private bool IsWater(int x, int y) => 
        (this.IsValid(x, y) && (this.GetGoodID(x, y) == 0x3ee));

    public bool IsXMiddle(int x) => 
        (x == ((this.width - 1) / 2));

    public bool IsYMiddle(int y) => 
        (y == ((this.height - 1) / 2));

    public bool RandomCallSide(EntityBase entity, int range, out float endx, out float endz)
    {
        List<Vector2Int> list = this.GetRoundSideEmpty(this.mCallRect, entity.position, range);
        if (list.Count == 0)
        {
            this.RandomItem(entity, range, out endx, out endz);
            return false;
        }
        int num = GameLogic.Random(0, list.Count);
        Vector3 worldPosition = this.GetWorldPosition(list[num]);
        endx = worldPosition.x;
        endz = worldPosition.z;
        Vector2Int roomXY = this.GetRoomXY(worldPosition);
        this.mCallRect[roomXY.x, roomXY.y] = 1;
        return true;
    }

    public bool RandomCallSide(EntityBase entity, int radiusmin, int radiusmax, out float endx, out float endz)
    {
        for (int i = radiusmin; i <= radiusmax; i++)
        {
            if (this.RandomCallSide(entity, i, out endx, out endz))
            {
                return true;
            }
        }
        endx = 0f;
        endz = 0f;
        return true;
    }

    public void RandomFly(EntityBase entity, int range, out float endx, out float endz)
    {
        if (entity == null)
        {
            Vector3 vector = this.RandomPosition();
            endx = vector.x;
            endz = vector.z;
        }
        else
        {
            List<Vector2Int> roundFly = this.GetRoundFly(entity.position, range);
            int num = 0;
            while ((roundFly.Count == 0) && (num < 10))
            {
                range++;
                num++;
                roundFly = this.GetRoundFly(entity.position, range);
            }
            if (roundFly.Count == 0)
            {
                endx = entity.position.x;
                endz = entity.position.z;
            }
            else
            {
                int num2 = GameLogic.Random(0, roundFly.Count);
                Vector3 worldPosition = this.GetWorldPosition(roundFly[num2]);
                endx = worldPosition.x;
                endz = worldPosition.z;
            }
        }
    }

    public void RandomItem(EntityBase entity, int range, out float endx, out float endz)
    {
        if (entity == null)
        {
            Vector3 vector = this.RandomPosition();
            endx = vector.x;
            endz = vector.z;
        }
        else
        {
            this.RandomItem(entity.position, range, out endx, out endz);
        }
    }

    public void RandomItem(Vector3 pos, int range, out float endx, out float endz)
    {
        List<Vector2Int> roundEmpty = this.GetRoundEmpty(pos, range);
        int num = 0;
        while ((roundEmpty.Count == 0) && (num < 10))
        {
            range++;
            num++;
            roundEmpty = this.GetRoundEmpty(pos, range);
        }
        if (roundEmpty.Count == 0)
        {
            endx = pos.x;
            endz = pos.z;
        }
        else
        {
            int num2 = GameLogic.Random(0, roundEmpty.Count);
            Vector3 worldPosition = this.GetWorldPosition(roundEmpty[num2]);
            endx = worldPosition.x;
            endz = worldPosition.z;
        }
    }

    public bool RandomItemLine(EntityBase entity, bool dir, int rangemin, int rangemax, out float endx, out float endz)
    {
        List<Vector2Int> horizontalEmpty;
        Vector2Int roomXY = this.GetRoomXY(entity.position);
        if (dir)
        {
            horizontalEmpty = this.GetHorizontalEmpty(entity);
        }
        else
        {
            horizontalEmpty = this.GetVerticalEmpty(entity);
        }
        if (horizontalEmpty.Count > 0)
        {
            for (int i = horizontalEmpty.Count - 1; i >= 0; i--)
            {
                Vector2Int num4 = horizontalEmpty[i];
                int introduced8 = MathDxx.Abs((int) (roomXY.x - num4.x));
                Vector2Int num5 = horizontalEmpty[i];
                int num3 = introduced8 + MathDxx.Abs((int) (roomXY.y - num5.y));
                if ((num3 < rangemin) || (num3 > rangemax))
                {
                    horizontalEmpty.RemoveAt(i);
                }
            }
        }
        else
        {
            endx = 0f;
            endz = 0f;
            return false;
        }
        int num6 = GameLogic.Random(0, horizontalEmpty.Count);
        Vector3 worldPosition = this.GetWorldPosition(horizontalEmpty[num6]);
        endx = worldPosition.x;
        endz = worldPosition.z;
        return true;
    }

    public Vector2Int RandomItemSide(EntityBase entity)
    {
        List<Vector2Int> list = this.GetRoundSideEmpty(this.findpathRect, entity.position, 1);
        if (list.Count == 0)
        {
            return this.GetRoomXY(entity.position);
        }
        int num = GameLogic.Random(0, list.Count);
        return list[num];
    }

    public void RandomItemSide(EntityBase entity, int range, out float endx, out float endz)
    {
        List<Vector2Int> list = this.GetRoundSideEmpty(this.findpathRect, entity.position, range);
        if (list.Count == 0)
        {
            endx = entity.position.x;
            endz = entity.position.z;
        }
        else
        {
            int num = GameLogic.Random(0, list.Count);
            Vector3 worldPosition = this.GetWorldPosition(list[num]);
            endx = worldPosition.x;
            endz = worldPosition.z;
        }
    }

    public void RandomItemSides(EntityBase entity, int rangemin, int rangemax, out float endx, out float endz)
    {
        for (int i = rangemin; i <= rangemax; i++)
        {
            this.sides_listtemp = this.GetRoundSideEmpty(this.findpathRect, entity.position, i);
            int num2 = 0;
            int count = this.sides_listtemp.Count;
            while (num2 < count)
            {
                this.sides_list.Add(this.sides_listtemp[num2]);
                num2++;
            }
        }
        int num4 = 0;
        while ((this.sides_list.Count == 0) && (num4 < 10))
        {
            num4++;
            rangemax++;
            this.sides_listtemp = this.GetRoundSideEmpty(this.findpathRect, entity.position, rangemax);
            int num5 = 0;
            int count = this.sides_listtemp.Count;
            while (num5 < count)
            {
                this.sides_list.Add(this.sides_listtemp[num5]);
                num5++;
            }
        }
        if (this.sides_list.Count == 0)
        {
            Vector3 vector = this.RandomPosition();
            endx = vector.x;
            endz = vector.z;
        }
        else
        {
            int num7 = GameLogic.Random(0, this.sides_list.Count);
            Vector3 worldPosition = this.GetWorldPosition(this.sides_list[num7]);
            endx = worldPosition.x;
            endz = worldPosition.z;
        }
    }

    public Vector3 RandomOutPosition()
    {
        int num = GameLogic.Random(0, 4);
        int x = 0;
        int y = 0;
        switch (num)
        {
            case 0:
                x = 0;
                y = GameLogic.Random(0, this.height);
                break;

            case 1:
                x = this.width - 1;
                y = GameLogic.Random(0, this.height);
                break;

            case 2:
                x = GameLogic.Random(0, this.width);
                y = 0;
                break;

            case 3:
                x = GameLogic.Random(0, this.width);
                y = this.height - 1;
                break;
        }
        return this.GetWorldPosition(x, y);
    }

    public List<Vector3> RandomOutPositions(int count)
    {
        List<Vector3> list = new List<Vector3>();
        for (int i = 0; i < 7; i++)
        {
            Vector3 pos = this.RandomOutPosition();
            while (this.ishave(list, pos))
            {
                pos = this.RandomOutPosition();
            }
            list.Add(pos);
        }
        return list;
    }

    public Vector3 RandomPosition()
    {
        int x = GameLogic.Random(0, this.width);
        int y = GameLogic.Random(0, this.height);
        while (!this.IsEmpty(x, y))
        {
            x = GameLogic.Random(0, this.width);
            y = GameLogic.Random(0, this.height);
        }
        return this.GetWorldPosition(x, y);
    }

    public Vector3 RandomPosition(int area)
    {
        if (area == 0)
        {
            int x = GameLogic.Random(0, this.width / 2);
            int y = GameLogic.Random(0, this.height / 2);
            return this.GetWorldPosition(x, y);
        }
        if (area == 1)
        {
            int x = GameLogic.Random(this.width / 2, this.width);
            int y = GameLogic.Random(0, this.height / 2);
            return this.GetWorldPosition(x, y);
        }
        if (area == 2)
        {
            int x = GameLogic.Random(0, this.width / 2);
            int y = GameLogic.Random(this.height / 2, this.height);
            return this.GetWorldPosition(x, y);
        }
        if (area == 3)
        {
            int x = GameLogic.Random(this.width / 2, this.width);
            int y = GameLogic.Random(this.height / 2, this.height);
            return this.GetWorldPosition(x, y);
        }
        return this.RandomPosition();
    }

    public Vector3 RandomPositionRange(EntityBase entity, int range)
    {
        if (entity == null)
        {
            return Vector3.zero;
        }
        Vector2Int roomXY = this.GetRoomXY(entity.position);
        int num2 = GameLogic.Random((int) (roomXY.x - range), (int) (roomXY.x + range));
        int num3 = GameLogic.Random((int) (roomXY.y - range), (int) (roomXY.y + range));
        num2 = MathDxx.Clamp(num2, 2, this.width - 2);
        num3 = MathDxx.Clamp(num3, 2, this.height - 2);
        return this.GetWorldPosition(num2, num3);
    }

    public Vector2Int RandomRoomXY()
    {
        int x = GameLogic.Random(0, this.width);
        return new Vector2Int(x, GameLogic.Random(0, this.height));
    }

    private void ReadTiledMapTSX(string name)
    {
        XmlDocument document = new XmlDocument();
        object[] args = new object[] { name };
        string xml = ResourceManager.Load<TextAsset>(Utils.FormatString("Game/Map/Tiled/{0}", args)).ToString();
        document.LoadXml(xml);
        IEnumerator enumerator = document.SelectSingleNode("tileset/terraintypes").SelectNodes("terrain").GetEnumerator();
        while (enumerator.MoveNext())
        {
            XmlNode current = (XmlNode) enumerator.Current;
            int num = int.Parse(current.Attributes["name"].Value);
            int key = int.Parse(current.Attributes["tile"].Value);
            if (!this.TileMap2Goods.ContainsKey(key))
            {
                this.TileMap2Goods.Add(key, num);
            }
        }
    }

    private void readTileMap()
    {
        XmlDocument document = new XmlDocument();
        string tmxString = this.GetTmxString(this.MapID);
        if (tmxString.Equals(string.Empty))
        {
            object[] args = new object[] { this.MapID, GameLogic.Hold.BattleData.GetMode().ToString(), GameLogic.Hold.BattleData.Level_CurrentStage };
            SdkManager.Bugly_Report("MapCreator_Init", Utils.FormatString("readTileMap [{0}] mode:{1} stage:{2} is not found!!!", args));
            this.MapID = "firstroom";
            tmxString = this.GetTmxString(this.MapID);
        }
        document.LoadXml(tmxString);
        XmlNode node = document.SelectSingleNode("map/tileset");
        XmlAttributeCollection attributes = document.SelectSingleNode("map").Attributes;
        this.width = int.Parse(attributes["width"].Value);
        this.height = int.Parse(attributes["height"].Value);
        this.tiledata = new int[this.width, this.height];
        XmlNodeList nodes = document.SelectNodes("map/layer");
        char[] separator = new char[] { '\n' };
        string[] strArray = this.get_node(nodes).SelectSingleNode("data").InnerText.Split(separator);
        for (int i = 1; i < (strArray.Length - 1); i++)
        {
            char[] chArray2 = new char[] { ',' };
            string[] strArray2 = strArray[i].Split(chArray2);
            for (int j = 0; j < this.width; j++)
            {
                this.tiledata[j, i - 1] = int.Parse(strArray2[j]);
            }
        }
        this.initRoomRealRectGoods();
        this.Bomberman_Init();
    }

    private void send_ui(int time)
    {
        this.mWaveData.maxwave = this.get_max_wave() + 1;
        this.mWaveData.currentwave = this.waveroom_currentwave + 1;
        this.mWaveData.showui = (this.mWaveData.maxwave > 1) && (this.mWaveData.currentwave < this.mWaveData.maxwave);
        this.mWaveData.lasttime = time;
        Facade.Instance.SendNotification("BattleUI_level_wave_update", this.mWaveData);
    }

    private int water_checkround(int x, int y)
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                this.waterchecks[i, j] = this.IsWater((i + x) - 1, (j + y) - 1);
            }
        }
        return this.GetWaterID(this.waterchecks);
    }

    private void water_test()
    {
        bool[,] checks = new bool[3, 3];
        this.water_test(checks, 0);
    }

    private void water_test(bool[,] checks, int index)
    {
        int num = index % 3;
        int num2 = index / 3;
        checks[num, num2] = true;
        if (index == 8)
        {
            if (checks[1, 1])
            {
                this.GetWaterID(checks);
            }
        }
        else
        {
            this.water_test(checks, index + 1);
        }
        checks[num, num2] = false;
        if (index == 8)
        {
            if (checks[1, 1])
            {
                this.GetWaterID(checks);
            }
        }
        else
        {
            this.water_test(checks, index + 1);
        }
    }

    private bool water_test_iswater(bool[,] checks, int x, int y) => 
        ((((x >= 0) && (x < 3)) && ((y >= 0) && (y < 3))) && checks[x, y]);

    public void waveroom_battlecache_init()
    {
        if (LocalSave.Instance.BattleIn_GetIn())
        {
            this.waveroom_currentwave = 0x7fffffff;
            this.waveroom_currentwave_createend = true;
        }
    }

    private void waveroom_create_good()
    {
        this.waveroom_nodelist.Clear();
        XmlDocument document = new XmlDocument();
        string tmxString = this.GetTmxString(this.MapID);
        if (tmxString.Equals(string.Empty))
        {
            object[] args = new object[] { this.MapID, GameLogic.Hold.BattleData.GetMode().ToString(), GameLogic.Hold.BattleData.Level_CurrentStage };
            SdkManager.Bugly_Report("MapCreator_Init", Utils.FormatString("readTileMap [{0}] mode:{1} stage:{2} is not found!!!", args));
            this.MapID = "firstroom";
            tmxString = this.GetTmxString(this.MapID);
        }
        document.LoadXml(tmxString);
        XmlNode node = document.SelectSingleNode("map/tileset");
        XmlAttributeCollection attributes = document.SelectSingleNode("map").Attributes;
        this.width = int.Parse(attributes["width"].Value);
        this.height = int.Parse(attributes["height"].Value);
        this.tiledata = new int[this.width, this.height];
        XmlNodeList list = document.SelectNodes("map/layer");
        bool flag = false;
        if (this.waveroom_nodelist.Count == 0)
        {
            int num = 0;
            for (int i = 0; i < list.Count; i++)
            {
                XmlNode node2 = list[i];
                if (node2.Attributes["name"].Value == "layer1")
                {
                    flag = true;
                    this.waveroom_createmap(node2);
                }
                else
                {
                    this.waveroom_nodelist.Add(node2);
                    num++;
                }
            }
        }
        if (!flag)
        {
            this.waveroom_createmap(list[0]);
        }
    }

    private void waveroom_createmap(XmlNode node)
    {
        char[] separator = new char[] { '\n' };
        string[] strArray = node.SelectSingleNode("data").InnerText.Split(separator);
        for (int i = 1; i < (strArray.Length - 1); i++)
        {
            char[] chArray2 = new char[] { ',' };
            string[] strArray2 = strArray[i].Split(chArray2);
            for (int j = 0; j < this.width; j++)
            {
                this.tiledata[j, i - 1] = int.Parse(strArray2[j]);
            }
        }
        this.initRoomRealRectGoods();
        this.CreateAllGoods();
    }

    private void waveroom_createnext_wave()
    {
        if (this.waveroom_nodelist.Count == 0)
        {
            this.waveroom_currentwave_createend = true;
            this.waveroom_currentwave = this.get_max_wave();
        }
        else
        {
            char[] separator = new char[] { '\n' };
            string[] strArray = this.waveroom_random_node().SelectSingleNode("data").InnerText.Split(separator);
            for (int i = 1; i < (strArray.Length - 1); i++)
            {
                char[] chArray2 = new char[] { ',' };
                string[] strArray2 = strArray[i].Split(chArray2);
                for (int j = 0; j < this.width; j++)
                {
                    this.tiledata[j, i - 1] = int.Parse(strArray2[j]);
                }
            }
            this.waveroom_currentwave++;
            this.send_ui(this.get_time());
            this.waveroom_currentwave_createend = false;
            Sequence sequence = this.waveroom_pool.Get();
            TweenSettingsExtensions.AppendCallback(sequence, new TweenCallback(this, this.<waveroom_createnext_wave>m__1));
            TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(sequence, 0.9f), new TweenCallback(this, this.<waveroom_createnext_wave>m__2));
            if (this.waveroom_currentwave < this.get_max_wave())
            {
                TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(this.waveroom_pool.Get(), (float) this.get_time()), new TweenCallback(this, this.<waveroom_createnext_wave>m__3));
            }
        }
    }

    public void waveroom_currentwave_clear()
    {
        if ((this.waveroom_currentwave < this.get_max_wave()) && this.waveroom_currentwave_createend)
        {
            this.waveroom_killseq();
            this.waveroom_createnext_wave();
        }
    }

    private void waveroom_deinit()
    {
        if (GameLogic.Self != null)
        {
            GameLogic.Self.Event_PositionBy -= new Action<Vector3>(this.waveroom_playermove);
        }
        this.waveroom_currentwave_createend = false;
        this.waveroom_killseq();
    }

    private void waveroom_init()
    {
        if ((this.mRoomType != RoomGenerateBase.RoomType.eBoss) && (this.mRoomType != RoomGenerateBase.RoomType.eNormal))
        {
            object[] args = new object[] { this.mRoomType };
            SdkManager.Bugly_Report("MapCreator_WaveRoom", Utils.FormatString("RoomType:{0} in wave type  is not a valid type.", args));
        }
        else
        {
            GameLogic.Self.Event_PositionBy += new Action<Vector3>(this.waveroom_playermove);
            this.waveroom_maxwave.Clear();
            this.waveroom_time.Clear();
            this.waveroom_maxwave.Add(RoomGenerateBase.RoomType.eBoss, LocalModelManager.Instance.Stage_Level_stagechapter.waveroom_get_bosswave() - 1);
            this.waveroom_maxwave.Add(RoomGenerateBase.RoomType.eNormal, LocalModelManager.Instance.Stage_Level_stagechapter.waveroom_get_monsterwave() - 1);
            this.waveroom_time.Add(RoomGenerateBase.RoomType.eBoss, LocalModelManager.Instance.Stage_Level_stagechapter.waveroom_get_bosswave_time());
            this.waveroom_time.Add(RoomGenerateBase.RoomType.eNormal, LocalModelManager.Instance.Stage_Level_stagechapter.waveroom_get_monsterwave_time());
            this.waveroom_currentwave = -1;
            this.waveroom_startwave = false;
            this.waveroom_battlecache_init();
        }
    }

    public bool waveroom_is_clear()
    {
        if (!this.waveroom_currentwave_createend)
        {
            return false;
        }
        return (this.waveroom_currentwave >= this.get_max_wave());
    }

    public void waveroom_killseq()
    {
        this.waveroom_pool.Clear();
    }

    private void waveroom_playermove(Vector3 moveby)
    {
        if (((GameLogic.Self != null) && !this.waveroom_startwave) && (GameLogic.Self.position.z >= 0f))
        {
            this.waveroom_startwave = true;
            if (this.waveroom_currentwave < this.get_max_wave())
            {
                this.waveroom_createnext_wave();
            }
        }
    }

    private XmlNode waveroom_random_node()
    {
        int index = GameLogic.Random(0, this.waveroom_nodelist.Count);
        XmlNode node = this.waveroom_nodelist[index];
        this.waveroom_nodelist.RemoveAt(index);
        return node;
    }

    public int RoomID =>
        this.roomid;

    [CompilerGenerated]
    private sealed class <Bomberman_get_safe_near>c__AnonStorey0
    {
        internal Vector2Int selfpos;

        internal int <>m__0(Vector2Int a, Vector2Int b)
        {
            if ((MathDxx.Pow((float) (a.x - this.selfpos.x), 2f) + MathDxx.Pow((float) (a.y - this.selfpos.y), 2f)) < (MathDxx.Pow((float) (b.x - this.selfpos.x), 2f) + MathDxx.Pow((float) (b.y - this.selfpos.y), 2f)))
            {
                return -1;
            }
            return 1;
        }
    }

    public class CreateData
    {
        public EntityBase parent;
        public int entityid;
        public Vector2Int v;
        public float x;
        public float y;
        public bool m_bElite;
        public bool bDivide;
        public bool bCall;

        public string path
        {
            get
            {
                object[] args = new object[] { MapCreator.mResList[MapCreator.GetGoodType(this.entityid)], this.entityid };
                return Utils.GetString(args);
            }
        }
    }

    private class HeroModeData
    {
        public Vector2Int roompos;
        public int entityid;
        public bool m_bElite;
    }

    public enum MapGoodType
    {
        Empty,
        Goods,
        Soldier,
        Boss,
        Event,
        Tower
    }

    public class Transfer
    {
        public RoomControlBase roomctrl;
        public int roomid;
        public int resourcesid;
        public string tmxid;
        public bool delay;
        public RoomGenerateBase.RoomType roomtype;

        public override string ToString()
        {
            object[] args = new object[] { this.roomid, this.resourcesid, this.tmxid, this.delay, this.roomtype };
            return Utils.FormatString("RoomID:{0}, Res:{1}, TmxID:{2}, Delay:{3}, RoomType:{4}", args);
        }
    }
}

