namespace TableTool
{
    using Dxx.Util;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class Stage_Level_stagechapterModel : LocalModel<Stage_Level_stagechapter, int>
    {
        private const string _Filename = "Stage_Level_stagechapter";
        private Dictionary<int, int> mMaxLayers = new Dictionary<int, int>();
        private int maxChapter;
        private int styleid;
        private EquipExpDropData mEquipExp = new EquipExpDropData();

        public int GetAllMaxLevel()
        {
            int chapterId = GameLogic.Hold.BattleData.Level_CurrentStage;
            if (chapterId < 1)
            {
                return 0;
            }
            return this.GetAllMaxLevel(chapterId);
        }

        public int GetAllMaxLevel(int chapterId)
        {
            int num = 0;
            if (!this.mMaxLayers.TryGetValue(chapterId, out num))
            {
                for (int i = 1; i <= chapterId; i++)
                {
                    int currentMaxLevel = this.GetCurrentMaxLevel(i);
                    num += currentMaxLevel;
                }
                this.mMaxLayers.Add(chapterId, num);
            }
            return num;
        }

        public Stage_Level_stagechapter GetBeanByChapter(int chapter) => 
            base.GetBeanById(chapter + 100);

        protected override int GetBeanKey(Stage_Level_stagechapter bean) => 
            bean.ID;

        public int GetBossDropID() => 
            0;

        public Color GetChapterColor(int stageid) => 
            (!GameLogic.Hold.BattleData.IsHeroMode(stageid) ? Color.white : new Color(1f, 0.2313726f, 0.3254902f));

        public string GetChapterFullName(int stageid)
        {
            if (!GameLogic.Hold.BattleData.IsHeroMode(stageid))
            {
                object[] objArray1 = new object[2];
                objArray1[0] = stageid;
                object[] objArray2 = new object[] { stageid };
                objArray1[1] = GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("ChapterName_{0}", objArray2), Array.Empty<object>());
                return Utils.FormatString("{0}.{1}", objArray1);
            }
            string languageByTID = GameLogic.Hold.Language.GetLanguageByTID("Chapter_HeroMode", Array.Empty<object>());
            object[] args = new object[3];
            args[0] = languageByTID;
            args[1] = stageid;
            object[] objArray4 = new object[] { stageid };
            args[2] = GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("ChapterName_{0}", objArray4), Array.Empty<object>());
            return Utils.FormatString("{0}{1}.{2}", args);
        }

        public int GetCurrentMaxLevel(int chapter) => 
            LocalModelManager.Instance.Stage_Level_chapter1.GetStageLevel_MaxLevel(chapter);

        public Stage_Level_stagechapter GetCurrentStageLevel() => 
            this.GetBeanByChapter(GameLogic.Hold.BattleData.Level_CurrentStage);

        public int GetEquipDropID(int chapterid) => 
            this.GetBeanByChapter(chapterid).EquipDropID;

        public int GetEquipDropRate(int chapterid) => 
            this.GetBeanByChapter(chapterid).EquipProb;

        public List<int> GetEquipExpDrop(EntityType type) => 
            this.mEquipExp.random(GameLogic.Hold.BattleData.Level_CurrentStage, type);

        public int GetExp()
        {
            int num = 0;
            if (GameLogic.Hold.BattleData.GetMode() == GameMode.eLevel)
            {
                Stage_Level_stagechapter beanByChapter = this.GetBeanByChapter(GameLogic.Hold.BattleData.Level_CurrentStage);
                int layer = GameLogic.Hold.BattleData.GetLayer();
                num = beanByChapter.ExpBase + (layer * beanByChapter.ExpAdd);
            }
            return num;
        }

        public float GetGoldDropPercent(int layer)
        {
            string[] stageLevelAttributes = this.GetStageLevelAttributes(layer);
            int index = 0;
            int length = stageLevelAttributes.Length;
            while (index < length)
            {
                Goods_goods.GoodData goodData = Goods_goods.GetGoodData(stageLevelAttributes[index]);
                if (goodData.goodType == "GoldDrop%")
                {
                    return (1f + (((float) goodData.value) / 100f));
                }
                index++;
            }
            return 1f;
        }

        public float GetGoldRate() => 
            this.GetBeanByChapter(GameLogic.Hold.BattleData.Level_CurrentStage).GoldRate;

        public int GetMaxChapter() => 
            this.maxChapter;

        public int GetMaxChapter_Hero()
        {
            if (LocalSave.Instance.Stage_GetStage() <= 10)
            {
                return 10;
            }
            return this.maxChapter;
        }

        public int GetMonsterDropID() => 
            0;

        public float GetScoreRate() => 
            this.GetBeanByChapter(GameLogic.Hold.BattleData.Level_CurrentStage).IntegralRate;

        public string[] GetStageLevelAttributes(int layer) => 
            this.GetStageLevelAttributes(GameLogic.Hold.BattleData.Level_CurrentStage, layer);

        public string[] GetStageLevelAttributes(int stage, int layer) => 
            LocalModelManager.Instance.Stage_Level_chapter1.GetStageLevel_Attributes(stage, layer);

        public string[] GetStageLevelMapAttributes(int stage, int layer) => 
            LocalModelManager.Instance.Stage_Level_chapter1.GetStageLevel_MapAttributes(stage, layer);

        public long GetStageLevelStandardDefence(int stage, int layer) => 
            LocalModelManager.Instance.Stage_Level_chapter1.GetStageLevel_Defence(stage, layer);

        public string[] GetStageLevelTmxIds(int layer) => 
            this.GetStageLevelTmxIds(GameLogic.Hold.BattleData.Level_CurrentStage, layer);

        public string[] GetStageLevelTmxIds(int stage, int layer) => 
            this.GetStageLevelTmxIds(stage, layer);

        public int GetStartLevel()
        {
            int chapterId = GameLogic.Hold.BattleData.Level_CurrentStage;
            return (this.GetAllMaxLevel(chapterId) + 1);
        }

        public string GetStartTmx() => 
            string.Empty;

        public int GetStyleID()
        {
            int result = 1;
            string styleString = "0101";
            if (((GameLogic.Release.Mode != null) && (GameLogic.Release.Mode.RoomGenerate != null)) && ((GameLogic.Hold != null) && (GameLogic.Hold.BattleData != null)))
            {
                int currentRoomID = GameLogic.Release.Mode.RoomGenerate.GetCurrentRoomID();
                styleString = this.GetStyleString(currentRoomID);
                int.TryParse(this.GetStyleString(currentRoomID).Substring(0, 2), out result);
            }
            return result;
        }

        public string GetStyleString(int roomid)
        {
            string[] seq = new string[0];
            if (GameLogic.Hold.BattleData.GetMode() == GameMode.eLevel)
            {
                seq = this.GetBeanByChapter(GameLogic.Hold.BattleData.Level_CurrentStage).StyleSequence;
            }
            else
            {
                seq = LocalModelManager.Instance.Stage_Level_activity.GetBeanById(GameLogic.Hold.BattleData.ActiveID).StyleSequence;
            }
            return this.GetStyleStringInternal(seq, roomid);
        }

        private string GetStyleStringInternal(string[] seq, int roomid)
        {
            int length = seq.Length;
            roomid--;
            if (roomid <= 0)
            {
                roomid = 1;
            }
            roomid = (roomid / 10) % length;
            return seq[roomid];
        }

        public int GetTiledID(int chapter) => 
            this.GetBeanByChapter(chapter).TiledID;

        public void Init()
        {
            this.maxChapter = 1;
            for (int i = 1; i < 100; i++)
            {
                if (this.GetBeanByChapter(i) == null)
                {
                    break;
                }
                this.maxChapter = i;
            }
            this.init_equipexp();
        }

        private void init_equipexp()
        {
            IList<Stage_Level_stagechapter> allBeans = base.GetAllBeans();
            int num = 0;
            int count = allBeans.Count;
            while (num < count)
            {
                Stage_Level_stagechapter _stagechapter = allBeans[num];
                this.mEquipExp.add(_stagechapter.ID - 100, _stagechapter.ScrollRate, _stagechapter.ScrollRateBoss);
                num++;
            }
        }

        public bool is_wave_room() => 
            this.is_wave_room(GameLogic.Hold.BattleData.Level_CurrentStage);

        public bool is_wave_room(int chapter) => 
            (this.GetBeanByChapter(chapter).GameType == 1);

        public bool IsMaxLevel(int roomid) => 
            this.IsMaxLevel(GameLogic.Hold.BattleData.Level_CurrentStage, roomid);

        public bool IsMaxLevel(int chapterId, int roomid)
        {
            int currentMaxLevel = this.GetCurrentMaxLevel(chapterId);
            return (roomid >= currentMaxLevel);
        }

        public int waveroom_get_bosswave()
        {
            int chapter = GameLogic.Hold.BattleData.Level_CurrentStage;
            return this.GetBeanByChapter(chapter).GameArgs[2];
        }

        public int waveroom_get_bosswave_time()
        {
            int chapter = GameLogic.Hold.BattleData.Level_CurrentStage;
            return this.GetBeanByChapter(chapter).GameArgs[3];
        }

        public int waveroom_get_monsterwave()
        {
            int chapter = GameLogic.Hold.BattleData.Level_CurrentStage;
            return this.GetBeanByChapter(chapter).GameArgs[0];
        }

        public int waveroom_get_monsterwave_time()
        {
            int chapter = GameLogic.Hold.BattleData.Level_CurrentStage;
            return this.GetBeanByChapter(chapter).GameArgs[1];
        }

        protected override string Filename =>
            "Stage_Level_stagechapter";

        public class EquipExpDropData
        {
            public Dictionary<int, WeightRandom<Stage_Level_stagechapterModel.EquipExpDropDataOne>> soldiers = new Dictionary<int, WeightRandom<Stage_Level_stagechapterModel.EquipExpDropDataOne>>();
            public Dictionary<int, WeightRandom<Stage_Level_stagechapterModel.EquipExpDropDataOne>> bosss = new Dictionary<int, WeightRandom<Stage_Level_stagechapterModel.EquipExpDropDataOne>>();
            private Stage_Level_stagechapterModel.EquipExpDropDataOne one;
            private List<int> list = new List<int>();

            public void add(int stage, string[] data_soldiers, string[] data_bosss)
            {
                WeightRandom<Stage_Level_stagechapterModel.EquipExpDropDataOne> random = new WeightRandom<Stage_Level_stagechapterModel.EquipExpDropDataOne>();
                int index = 0;
                int length = data_soldiers.Length;
                while (index < length)
                {
                    Stage_Level_stagechapterModel.EquipExpDropDataOne t = this.get(data_soldiers[index]);
                    random.Add(t, t.weight);
                    index++;
                }
                this.soldiers.Add(stage, random);
                WeightRandom<Stage_Level_stagechapterModel.EquipExpDropDataOne> random2 = new WeightRandom<Stage_Level_stagechapterModel.EquipExpDropDataOne>();
                int num3 = 0;
                int num4 = data_bosss.Length;
                while (num3 < num4)
                {
                    Stage_Level_stagechapterModel.EquipExpDropDataOne t = this.get(data_bosss[num3]);
                    random2.Add(t, t.weight);
                    num3++;
                }
                this.bosss.Add(stage, random2);
            }

            private Stage_Level_stagechapterModel.EquipExpDropDataOne get(string str)
            {
                char[] separator = new char[] { ',' };
                string[] strArray = str.Split(separator);
                if (strArray.Length != 4)
                {
                    SdkManager.Bugly_Report("Stage_Level_stagechapterModel_Extra", "some equipexp rate is invalid != 4 ! EquipExpDropData");
                }
                return new Stage_Level_stagechapterModel.EquipExpDropDataOne(int.Parse(strArray[0])) { 
                    weight = int.Parse(strArray[1]),
                    min = int.Parse(strArray[2]),
                    max = int.Parse(strArray[3])
                };
            }

            public List<int> random(int stage, EntityType type)
            {
                this.list.Clear();
                if (type == EntityType.Boss)
                {
                    this.one = this.bosss[stage].GetRandom();
                }
                else
                {
                    this.one = this.soldiers[stage].GetRandom();
                }
                this.list.AddRange(this.one.GetRandom());
                return this.list;
            }
        }

        public class EquipExpDropDataOne : WeightRandomDataBase
        {
            public int count;
            public int min;
            public int max;

            public EquipExpDropDataOne(int id) : base(id)
            {
                this.count = id;
            }

            public List<int> GetRandom()
            {
                List<int> list = new List<int>();
                for (int i = 0; i < this.count; i++)
                {
                    list.Add(GameLogic.Random(this.min, this.max + 1));
                }
                return list;
            }
        }
    }
}

