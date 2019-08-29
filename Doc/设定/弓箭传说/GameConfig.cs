using Dxx.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Runtime.InteropServices;
using TableTool;

public class GameConfig
{
    public const int HeroModeStage = 10;
    private static JObject config_game;
    private static int maxkeycount = -1;
    private static int adkeycount = -1;
    public const int HPItemCount = 4;
    private static MapGoodData _MapGood;

    public static int Get100AttackValue() => 
        LocalModelManager.Instance.Config_config.GetValue<int>(70);

    public static int Get100HPValue() => 
        LocalModelManager.Instance.Config_config.GetValue<int>(0x47);

    public static int GetAdKeyCount()
    {
        if (adkeycount > 0)
        {
            return adkeycount;
        }
        int num = LocalModelManager.Instance.Config_config.GetValue<int>(0x3ec);
        adkeycount = num;
        if (TryGet_Game<int>("ad_key_max", out int num2))
        {
            num = num2;
            adkeycount = num;
        }
        return num;
    }

    public static float GetArrowEject() => 
        LocalModelManager.Instance.Config_config.GetValue<float>(30);

    public static int GetBoxChooseGold(LocalSave.TimeBoxType type) => 
        LocalModelManager.Instance.Config_config.GetValue<int>((int) type);

    public static long GetBoxChooseTime(LocalSave.TimeBoxType type) => 
        LocalModelManager.Instance.Config_config.GetValue<long>(((int) type) + 10);

    public static int GetBoxDropGold() => 
        LocalModelManager.Instance.Config_config.GetValue<int>(0x3f3);

    public static bool GetCanOpenRateUI() => 
        (LocalSave.Instance.SaveExtra.overopencount == LocalModelManager.Instance.Config_config.GetValue<int>(10));

    public static float GetCoin1Wave() => 
        LocalModelManager.Instance.Config_config.GetValue<float>(80);

    public static int GetDemonMin() => 
        LocalModelManager.Instance.Config_config.GetValue<int>(60);

    public static int GetDemonPerHit() => 
        LocalModelManager.Instance.Config_config.GetValue<int>(0x3d);

    public static float GetEquipDropMaxRate() => 
        LocalModelManager.Instance.Config_config.GetValue<float>(130);

    public static int GetEquipExpUnlockTalentLevel() => 
        LocalModelManager.Instance.Config_config.GetValue<int>(0xbba);

    public static int GetEquipGuide_alllayer() => 
        LocalModelManager.Instance.Config_config.GetValue<int>(120);

    public static float GetEquipOneRatio() => 
        LocalModelManager.Instance.Config_config.GetValue<float>(0x412);

    public static int GetEquipUnlockTalentLevel() => 
        LocalModelManager.Instance.Config_config.GetValue<int>(0xbb9);

    public static float GetEquipUpgradeSellRatio() => 
        LocalModelManager.Instance.Config_config.GetValue<float>(0x411);

    public static bool GetFirstDeadRecover() => 
        (GameLogic.Random(0, 100) < LocalModelManager.Instance.Config_config.GetValue<int>(20));

    public static int GetFirstShopStartStage() => 
        LocalModelManager.Instance.Config_config.GetValue<int>(90);

    public static int GetHarvestMaxDay() => 
        LocalModelManager.Instance.Config_config.GetValue<int>(140);

    public static int GetKeyBuyDiamond() => 
        LocalModelManager.Instance.Config_config.GetValue<int>(0x413);

    public static int GetKeyRecoverTime()
    {
        int num = LocalModelManager.Instance.Config_config.GetValue<int>(0x3ea);
        if (TryGet_Game<int>("key_recover_second", out int num2))
        {
            num = num2;
        }
        return num;
    }

    public static short GetKeyTrustCount() => 
        LocalModelManager.Instance.Config_config.GetValue<short>(150);

    public static int GetMapStyleID(int RoomID)
    {
        RoomID--;
        return (((RoomID / 10) % 4) + 1);
    }

    public static int GetMaxKeyCount()
    {
        if (maxkeycount > 0)
        {
            return maxkeycount;
        }
        int num = LocalModelManager.Instance.Config_config.GetValue<int>(0x3e9);
        maxkeycount = num;
        if (TryGet_Game<int>("key_max", out int num2))
        {
            num = num2;
            maxkeycount = num;
        }
        return num;
    }

    public static int GetMaxKeyStartCount()
    {
        int num = LocalModelManager.Instance.Config_config.GetValue<int>(0x3e9) * 2;
        if (TryGet_Game<int>("key_max_start", out int num2))
        {
            num = num2;
        }
        return num;
    }

    public static int GetModeLevelKey() => 
        LocalModelManager.Instance.Config_config.GetValue<int>(0x3eb);

    public static int GetRebornCount() => 
        LocalModelManager.Instance.Config_config.GetValue<int>(0x5b);

    public static int GetRebornDiamond()
    {
        int rebornCount = GameLogic.Hold.BattleData.GetRebornCount();
        int num2 = GetRebornCount();
        return LocalModelManager.Instance.Config_config.GetValue<int>((0x5c + num2) - rebornCount);
    }

    public static float GetReboundHit() => 
        LocalModelManager.Instance.Config_config.GetValue<float>(40);

    public static float GetReboundSpeed() => 
        LocalModelManager.Instance.Config_config.GetValue<float>(0x29);

    public static int GetTimeBoxTime(LocalSave.TimeBoxType type) => 
        GetValue<int>(((int) type) + 10);

    public static int GetTrapHitBase() => 
        LocalModelManager.Instance.Config_config.GetValue<int>(0x7d1);

    public static T GetValue<T>(int id) => 
        LocalModelManager.Instance.Config_config.GetValue<T>(id);

    public static void Init()
    {
    }

    private static void Init_Game()
    {
        if (config_game == null)
        {
            string config = FileUtils.GetConfig("game_config.json");
            if (!string.IsNullOrEmpty(config))
            {
                try
                {
                    config_game = (JObject) JsonConvert.DeserializeObject(config);
                }
                catch
                {
                    config_game = null;
                    object[] args = new object[] { "data/config", "game_config.json" };
                    IAMazonS3Manager.Instance.ClearFileName(Utils.FormatString("{0}/{1}", args));
                }
            }
        }
    }

    private static bool TryGet_Game<T>(string name, out T result)
    {
        Init_Game();
        if (config_game != null)
        {
            JToken token = null;
            if (config_game.TryGetValue(name, ref token))
            {
                result = token.ToObject<T>();
                return true;
            }
        }
        result = default(T);
        return false;
    }

    public static int BattleAdUnlockTalentLevel =>
        LocalModelManager.Instance.Config_config.GetValue<int>(0xbbb);

    public static MapGoodData MapGood
    {
        get
        {
            if (_MapGood == null)
            {
                _MapGood = new MapGoodData();
            }
            return _MapGood;
        }
    }

    public class MapGoodData
    {
        private string[] attributes;
        private EntityAttributeBase.ValueFloatBase HPAddPercent = new EntityAttributeBase.ValueFloatBase();
        private EntityAttributeBase.ValueBase TrapHit = new EntityAttributeBase.ValueBase();
        private long StandardDefence;

        private void AddAttributes()
        {
            int index = 0;
            int length = this.attributes.Length;
            while (index < length)
            {
                Goods_goods.GoodData goodData = Goods_goods.GetGoodData(this.attributes[index]);
                switch (goodData.goodType)
                {
                    case "HPAdd%":
                        this.HPAddPercent.UpdateValuePercent(goodData.value);
                        break;

                    case "TrapHit%":
                        this.TrapHit.UpdateValuePercent(goodData.value);
                        break;
                }
                index++;
            }
        }

        public float GetHPAddPercent() => 
            this.HPAddPercent.Value;

        public long GetStandardDefence() => 
            this.StandardDefence;

        public long GetTrapHit() => 
            this.TrapHit.Value;

        public void Init()
        {
            this.TrapHit.InitValueCount((long) GameConfig.GetTrapHitBase());
            if (GameLogic.Release.Mode.RoomGenerate.GetCurrentRoomID() > 0)
            {
                this.attributes = GameLogic.Hold.BattleData.mModeData.GetMapAttributes();
                this.StandardDefence = GameLogic.Hold.BattleData.mModeData.GetMapStandardDefence();
            }
            else
            {
                this.attributes = new string[0];
                this.StandardDefence = 0L;
            }
            this.AddAttributes();
        }
    }
}

