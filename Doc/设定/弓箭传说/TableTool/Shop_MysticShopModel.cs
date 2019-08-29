namespace TableTool
{
    using Dxx.Util;
    using System;
    using System.Collections.Generic;

    public class Shop_MysticShopModel : LocalModel<Shop_MysticShop, int>
    {
        private const string _Filename = "Shop_MysticShop";
        public static Dictionary<int, int> mSellCounts;
        private MysticShopData mMysticShopData;
        private Dictionary<int, ShopData> mEquipList = new Dictionary<int, ShopData>();
        private Dictionary<int, WeightRandom> mCountList = new Dictionary<int, WeightRandom>();

        static Shop_MysticShopModel()
        {
            Dictionary<int, int> dictionary = new Dictionary<int, int> {
                { 
                    1,
                    1
                },
                { 
                    2,
                    2
                },
                { 
                    3,
                    2
                },
                { 
                    4,
                    3
                },
                { 
                    5,
                    3
                }
            };
            mSellCounts = dictionary;
        }

        public void AddRatio(int stage)
        {
            Shop_MysticShopShow beanById = LocalModelManager.Instance.Shop_MysticShopShow.GetBeanById(stage);
            this.mMysticShopData.AddRate(beanById.AddProb);
        }

        protected override int GetBeanKey(Shop_MysticShop bean) => 
            bean.ID;

        public List<Shop_MysticShop> GetListByStage(int stage, int shoptype)
        {
            if (!this.mEquipList.TryGetValue(stage, out ShopData data))
            {
                object[] args = new object[] { stage };
                SdkManager.Bugly_Report("Shop_MysticShopModel_Extra", Utils.FormatString("GetListByStage[{0}] is not in mList.", args));
                return new List<Shop_MysticShop>();
            }
            return data.GetList(shoptype);
        }

        public int GetRandomShopType()
        {
            int key = GameLogic.Hold.BattleData.Level_CurrentStage;
            WeightRandom random = null;
            if (this.mCountList.TryGetValue(key, out random))
            {
                return random.GetRandom();
            }
            object[] args = new object[] { key };
            SdkManager.Bugly_Report("Shop_MysticShopModel_Extra", Utils.FormatString("stage ��{0} is not in mCountList!", args));
            return 0;
        }

        public static int GetSellCount(int shoptype)
        {
            int num = 0;
            if (!mSellCounts.TryGetValue(shoptype, out num))
            {
                object[] args = new object[] { shoptype };
                SdkManager.Bugly_Report("Shop_MysticShopModel_Extra", Utils.FormatString("the shoptype:{0} is not in mSellCounts!", args));
            }
            return num;
        }

        public void Init()
        {
            this.init_show_prop_weight();
            this.mMysticShopData = LocalSave.Instance.mSaveData.mMysticShopData;
            IList<Shop_MysticShop> allBeans = base.GetAllBeans();
            int num = 0;
            int count = allBeans.Count;
            while (num < count)
            {
                Shop_MysticShop shop = allBeans[num];
                int index = 0;
                int length = shop.Stage.Length;
                while (index < length)
                {
                    if (!this.mEquipList.TryGetValue(shop.Stage[index], out ShopData data))
                    {
                        data = new ShopData {
                            stageid = shop.Stage[index]
                        };
                        this.mEquipList.Add(shop.Stage[index], data);
                    }
                    data.Add(shop);
                    index++;
                }
                num++;
            }
            IList<Shop_MysticShopShow> list2 = LocalModelManager.Instance.Shop_MysticShopShow.GetAllBeans();
            int num5 = 0;
            int num6 = list2.Count;
            while (num5 < num6)
            {
                Shop_MysticShopShow show = list2[num5];
                WeightRandom random = new WeightRandom();
                int index = 0;
                int length = show.ShopTypeProb.Length;
                while (index < length)
                {
                    random.Add(index + 1, show.ShopTypeProb[index]);
                    index++;
                }
                this.mCountList.Add(show.ID, random);
                num5++;
            }
        }

        private void init_show_prop_weight()
        {
            IEnumerator<Shop_MysticShopShow> enumerator = LocalModelManager.Instance.Shop_MysticShopShow.GetAllBeans().GetEnumerator();
            while (enumerator.MoveNext())
            {
                int[] shopTypeProb = enumerator.Current.ShopTypeProb;
                if (shopTypeProb.Length == 2)
                {
                    WeightRandom random = new WeightRandom();
                    for (int i = 0; i < shopTypeProb.Length; i++)
                    {
                        if (shopTypeProb[i] > 0)
                        {
                            random.Add(i, shopTypeProb[i]);
                        }
                    }
                }
            }
        }

        public bool RandomShop(int stage, int roomid, RoomGenerateBase.RoomType roomtype)
        {
            if (roomtype != RoomGenerateBase.RoomType.eBoss)
            {
                Shop_MysticShopShow beanById = LocalModelManager.Instance.Shop_MysticShopShow.GetBeanById(stage);
                if (beanById == null)
                {
                    object[] args = new object[] { stage };
                    SdkManager.Bugly_Report("Shop_MysticShopModel_Extra", Utils.FormatString("RandomShop stage:[{0}] is not in Shop_MysticShopShow.xls", args));
                    return false;
                }
                if (beanById.ShowRoom.Length != 2)
                {
                    object[] args = new object[] { stage, beanById.ShowRoom.Length };
                    SdkManager.Bugly_Report("Shop_MysticShopModel_Extra", Utils.FormatString("RandomShop stage:[{0}].ShowRoom.Length[{1}] != 2", args));
                    return false;
                }
                if (((roomid == 0) || (roomid > beanById.ShowRoom[1])) || (roomid < beanById.ShowRoom[0]))
                {
                    return false;
                }
                if (this.mMysticShopData.stage != stage)
                {
                    this.mMysticShopData.Reset(stage, beanById.ShowProb);
                }
                int rate = this.mMysticShopData.rate;
                if (GameLogic.Random(0, 0x2710) < rate)
                {
                    this.mMysticShopData.ResetRate(beanById.ShowProb);
                    return true;
                }
                this.mMysticShopData.AddRate(beanById.AddProb);
            }
            return false;
        }

        protected override string Filename =>
            "Shop_MysticShop";

        [Serializable]
        public class MysticShopData : LocalSaveBase
        {
            public int stage = 1;
            public int rate;

            public void AddRate(int rate)
            {
                this.rate += rate;
                base.Refresh();
            }

            protected override void OnRefresh()
            {
                FileUtils.WriteXml<Shop_MysticShopModel.MysticShopData>("File_MysticShop", this);
            }

            public void Reset(int stage, int rate)
            {
                this.stage = stage;
                this.rate = rate;
                base.Refresh();
            }

            public void ResetRate(int rate)
            {
                this.rate = rate;
                base.Refresh();
            }
        }

        public class ShopData
        {
            public int stageid;
            public Dictionary<int, Dictionary<int, WeightRandom>> mList = new Dictionary<int, Dictionary<int, WeightRandom>>();

            public void Add(Shop_MysticShop data)
            {
                Dictionary<int, WeightRandom> dictionary = null;
                WeightRandom random = null;
                int index = 0;
                int length = data.Position.Length;
                while (index < length)
                {
                    if (!this.mList.TryGetValue(data.Position[index], out dictionary))
                    {
                        dictionary = new Dictionary<int, WeightRandom>();
                        this.mList.Add(data.Position[index], dictionary);
                    }
                    if (!dictionary.TryGetValue(data.ShopType, out random))
                    {
                        random = new WeightRandom();
                        dictionary.Add(data.ShopType, random);
                    }
                    random.Add(data.ID, data.Weights);
                    index++;
                }
            }

            public List<Shop_MysticShop> GetList(int shoptype)
            {
                List<Shop_MysticShop> list = new List<Shop_MysticShop>();
                int sellCount = Shop_MysticShopModel.GetSellCount(shoptype);
                for (int i = 1; i <= sellCount; i++)
                {
                    Dictionary<int, WeightRandom> dictionary = this.mList[i];
                    int random = dictionary[shoptype].GetRandom();
                    list.Add(LocalModelManager.Instance.Shop_MysticShop.GetBeanById(random));
                }
                return list;
            }

            public override string ToString()
            {
                string str = string.Empty;
                for (int i = 1; i <= this.mList.Count; i++)
                {
                    string str2 = str;
                    object[] objArray1 = new object[] { str2, "Pos:", i, " : ", this.mList[i].ToString(), "\n" };
                    str = string.Concat(objArray1);
                }
                return str;
            }
        }
    }
}

