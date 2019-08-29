namespace TableTool
{
    using Dxx.Util;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;

    public class Drop_DropModel : LocalModel<Drop_Drop, int>
    {
        private const string _Filename = "Drop_Drop";
        private Dictionary<int, DropOneIDData> list = new Dictionary<int, DropOneIDData>();
        private int golddroproom = -1;
        private float golddroppercent = 1f;

        public void ClearGoldDrop()
        {
            this.golddroproom = -1;
        }

        protected override int GetBeanKey(Drop_Drop bean) => 
            bean.DropID;

        private void GetDiamondBox_ExcuteHave(List<DropData> list, List<DropData> giftlist)
        {
            int num = 0;
            int count = giftlist.Count;
            while (num < count)
            {
                bool flag = false;
                int num3 = 0;
                int num4 = list.Count;
                while (num3 < num4)
                {
                    if (list[num3].Equals(giftlist[num]))
                    {
                        DropData local1 = list[num3];
                        local1.count += giftlist[num].count;
                        flag = true;
                        break;
                    }
                    num3++;
                }
                if (!flag)
                {
                    list.Add(giftlist[num]);
                }
                num++;
            }
        }

        private void GetDiamondBox_ExcuteOne(List<DropData> list, int singleid, int giftid)
        {
            List<DropData> dropList = LocalModelManager.Instance.Drop_Drop.GetDropList(singleid);
            int num = 0;
            int count = dropList.Count;
            while (num < count)
            {
                list.Add(dropList[num]);
                num++;
            }
            List<DropData> giftlist = LocalModelManager.Instance.Drop_Drop.GetDropList(giftid);
            this.GetDiamondBox_ExcuteHave(list, giftlist);
        }

        public List<DropData> GetDiamondBoxLarge()
        {
            bool flag = true;
            object[] args = new object[] { LocalSave.Instance.GetServerUserID() };
            string key = Utils.FormatString("GetDiamondBox1_FirstGet_{0}", args);
            if (PlayerPrefsEncrypt.HasKey(key))
            {
                flag = false;
            }
            else
            {
                PlayerPrefsEncrypt.SetInt(key, 0);
            }
            int num = LocalSave.Instance.Stage_GetStage();
            Box_SilverBox beanById = LocalModelManager.Instance.Box_SilverBox.GetBeanById(num);
            List<DropData> dropList = LocalModelManager.Instance.Drop_Drop.GetDropList(beanById.SingleDrop);
            for (int i = LocalModelManager.Instance.Equip_equip.GetBeanById(dropList[0].id).Position; flag && (i == 1); i = LocalModelManager.Instance.Equip_equip.GetBeanById(dropList[0].id).Position)
            {
                dropList = LocalModelManager.Instance.Drop_Drop.GetDropList(beanById.SingleDrop);
            }
            return dropList;
        }

        public List<DropData> GetDiamondBoxNormal()
        {
            bool flag = true;
            object[] args = new object[] { LocalSave.Instance.GetServerUserID() };
            string key = Utils.FormatString("GetDiamondBox1_FirstGet_{0}", args);
            if (PlayerPrefsEncrypt.HasKey(key))
            {
                flag = false;
            }
            else
            {
                PlayerPrefsEncrypt.SetInt(key, 0);
            }
            int num = LocalSave.Instance.Stage_GetStage();
            Box_SilverNormalBox beanById = LocalModelManager.Instance.Box_SilverNormalBox.GetBeanById(num);
            List<DropData> dropList = LocalModelManager.Instance.Drop_Drop.GetDropList(beanById.SingleDrop);
            for (int i = LocalModelManager.Instance.Equip_equip.GetBeanById(dropList[0].id).Position; flag && (i == 1); i = LocalModelManager.Instance.Equip_equip.GetBeanById(dropList[0].id).Position)
            {
                dropList = LocalModelManager.Instance.Drop_Drop.GetDropList(beanById.SingleDrop);
            }
            return dropList;
        }

        public static DropData GetDropData(string str)
        {
            DropData data = new DropData();
            char[] separator = new char[] { ',' };
            string[] strArray = str.Split(separator);
            int.TryParse(strArray[0], out int num);
            data.type = (PropType) num;
            int.TryParse(strArray[1], out data.id);
            int.TryParse(strArray[2], out data.count);
            return data;
        }

        public static List<DropData> GetDropDatas(string[] strs)
        {
            List<DropData> list = new List<DropData>();
            int index = 0;
            int length = strs.Length;
            while (index < length)
            {
                list.Add(GetDropData(strs[index]));
                index++;
            }
            return list;
        }

        public int GetDropDiamond(List<DropData> list)
        {
            int num = 0;
            if (list != null)
            {
                int num2 = 0;
                int count = list.Count;
                while (num2 < count)
                {
                    if ((list[num2].type == PropType.eCurrency) && (list[num2].id == 2))
                    {
                        num += list[num2].count;
                    }
                    num2++;
                }
            }
            return num;
        }

        public int GetDropDiamondBoxLarge(List<DropData> list)
        {
            int num = 0;
            if (list != null)
            {
                int num2 = 0;
                int count = list.Count;
                while (num2 < count)
                {
                    if ((list[num2].type == PropType.eCurrency) && (list[num2].id == 0x16))
                    {
                        num += list[num2].count;
                    }
                    num2++;
                }
            }
            return num;
        }

        public int GetDropDiamondBoxNormal(List<DropData> list)
        {
            int num = 0;
            if (list != null)
            {
                int num2 = 0;
                int count = list.Count;
                while (num2 < count)
                {
                    if ((list[num2].type == PropType.eCurrency) && (list[num2].id == 0x15))
                    {
                        num += list[num2].count;
                    }
                    num2++;
                }
            }
            return num;
        }

        public List<DropData> GetDropEquips(List<DropData> list)
        {
            List<DropData> list2 = new List<DropData>();
            if (list != null)
            {
                int num = 0;
                int count = list.Count;
                while (num < count)
                {
                    if (list[num].type == PropType.eEquip)
                    {
                        list2.Add(list[num]);
                    }
                    num++;
                }
            }
            return list2;
        }

        public int GetDropExp(List<DropData> list)
        {
            int num = 0;
            if (list != null)
            {
                int num2 = 0;
                int count = list.Count;
                while (num2 < count)
                {
                    if ((list[num2].type == PropType.eCurrency) && (list[num2].id == 0x3e9))
                    {
                        num += list[num2].count;
                    }
                    num2++;
                }
            }
            return num;
        }

        public int GetDropGold(List<DropData> list)
        {
            int num = 0;
            if (list != null)
            {
                int num2 = 0;
                int count = list.Count;
                while (num2 < count)
                {
                    if ((list[num2].type == PropType.eCurrency) && (list[num2].id == 1))
                    {
                        num += list[num2].count;
                    }
                    num2++;
                }
            }
            return num;
        }

        public int GetDropKey(List<DropData> list)
        {
            int num = 0;
            if (list != null)
            {
                int num2 = 0;
                int count = list.Count;
                while (num2 < count)
                {
                    if ((list[num2].type == PropType.eCurrency) && (list[num2].id == 3))
                    {
                        num += list[num2].count;
                    }
                    num2++;
                }
            }
            return num;
        }

        public List<DropData> GetDropList(int dropid)
        {
            if (!this.list.TryGetValue(dropid, out DropOneIDData data))
            {
                data = new DropOneIDData(base.GetBeanById(dropid));
            }
            return data.GetRandomDrop();
        }

        public static DropSaveOneData GetDropOne(string str)
        {
            DropSaveOneData data = new DropSaveOneData();
            data.Init(str);
            return data;
        }

        public float GetGoldDropPercent()
        {
            if (((GameLogic.Release != null) && (GameLogic.Release.Mode != null)) && (GameLogic.Release.Mode.RoomGenerate != null))
            {
                int currentRoomID = GameLogic.Release.Mode.RoomGenerate.GetCurrentRoomID();
                if (this.golddroproom < currentRoomID)
                {
                    this.golddroproom = currentRoomID;
                    this.golddroppercent = LocalModelManager.Instance.Stage_Level_stagechapter.GetGoldDropPercent(currentRoomID);
                }
            }
            else
            {
                this.golddroppercent = 1f;
            }
            return this.golddroppercent;
        }

        private void RandomList(List<DropData> list)
        {
            List<DropData> list2 = new List<DropData>();
            for (int i = list.Count - 1; (i >= 0) && (i < list.Count); i--)
            {
                if (list[i].type != PropType.eEquip)
                {
                    list2.Add(list[i]);
                    list.RemoveAt(i);
                }
            }
            list.RandomSort<DropData>();
            int num2 = 0;
            int count = list2.Count;
            while (num2 < count)
            {
                list.Add(list2[num2]);
                num2++;
            }
        }

        protected override string Filename =>
            "Drop_Drop";

        [Serializable]
        public class DropData
        {
            public PropType type;
            public int id;
            public int count;
            public ulong uniqueid;
            [JsonIgnore]
            public Action OnClose;

            public DropData()
            {
            }

            public DropData(PropType type, int id, int count)
            {
                this.type = type;
                this.id = id;
                this.count = count;
            }

            public bool Equals(Drop_DropModel.DropData data) => 
                (((data != null) && (this.type == data.type)) && (this.id == data.id));

            public override int GetHashCode() => 
                base.GetHashCode();

            public override string ToString()
            {
                object[] args = new object[] { this.id, this.count };
                return Utils.FormatString("{0}:{1}!", args);
            }

            [JsonIgnore]
            public bool is_base_currency
            {
                get
                {
                    if ((this.type != PropType.eCurrency) || ((((this.id != 1) && (this.id != 2)) && ((this.id != 3) && (this.id != 4))) && ((this.id != 0x15) && (this.id != 0x16))))
                    {
                        return false;
                    }
                    return true;
                }
            }

            [JsonIgnore]
            public bool can_fly
            {
                get
                {
                    if ((this.type != PropType.eCurrency) || (((this.id != 1) && (this.id != 2)) && (this.id != 3)))
                    {
                        return false;
                    }
                    return true;
                }
            }

            [JsonIgnore]
            public bool is_equipexp =>
                (((this.type == PropType.eEquip) && (this.id >= 0x7595)) && (this.id <= 0x7598));
        }

        private class DropOneIDData
        {
            private Drop_Drop mDropData;
            private Drop_DropModel.DropRandOne mFixedData = new Drop_DropModel.DropRandOne();
            private List<Drop_DropModel.DropRandOne> list = new List<Drop_DropModel.DropRandOne>();

            public DropOneIDData(Drop_Drop data)
            {
                this.mDropData = data;
                if (this.DropType == 1)
                {
                    int length = this.mDropData.Prob.Length;
                    if (length >= 1)
                    {
                        Drop_DropModel.DropRandOne item = new Drop_DropModel.DropRandOne {
                            RandomPercent = this.GetPercent(this.mDropData.Prob[0])
                        };
                        item.AddOne(this.mDropData.Rand1);
                        this.list.Add(item);
                    }
                    if (length >= 2)
                    {
                        Drop_DropModel.DropRandOne item = new Drop_DropModel.DropRandOne {
                            RandomPercent = this.GetPercent(this.mDropData.Prob[1])
                        };
                        item.AddOne(this.mDropData.Rand2);
                        this.list.Add(item);
                    }
                    if (length >= 3)
                    {
                        Drop_DropModel.DropRandOne item = new Drop_DropModel.DropRandOne {
                            RandomPercent = this.GetPercent(this.mDropData.Prob[2])
                        };
                        item.AddOne(this.mDropData.Rand3);
                        this.list.Add(item);
                    }
                    if (length >= 4)
                    {
                        Drop_DropModel.DropRandOne item = new Drop_DropModel.DropRandOne {
                            RandomPercent = this.GetPercent(this.mDropData.Prob[3])
                        };
                        item.AddOne(this.mDropData.Rand4);
                        this.list.Add(item);
                    }
                    if (length >= 5)
                    {
                        Drop_DropModel.DropRandOne item = new Drop_DropModel.DropRandOne {
                            RandomPercent = this.GetPercent(this.mDropData.Prob[4])
                        };
                        item.AddOne(this.mDropData.Rand5);
                        this.list.Add(item);
                    }
                }
                else if (this.DropType == 2)
                {
                    this.mFixedData.AddOne(this.mDropData.Fixed);
                }
            }

            private int GetPercent(string value)
            {
                if (!int.TryParse(value, out int num) && value.Contains("%"))
                {
                    value = value.Substring(0, value.Length - 1);
                    int.TryParse(value, out num);
                }
                return num;
            }

            public List<Drop_DropModel.DropData> GetRandomDrop()
            {
                if (this.DropType == 1)
                {
                    List<Drop_DropModel.DropData> list = new List<Drop_DropModel.DropData>();
                    int num = 0;
                    int count = this.list.Count;
                    while (num < count)
                    {
                        Drop_DropModel.DropData randomDrop = this.list[num].GetRandomDrop();
                        if (randomDrop != null)
                        {
                            list.Add(randomDrop);
                        }
                        num++;
                    }
                    return list;
                }
                if (this.DropType == 2)
                {
                    return this.mFixedData.GetAllDrop();
                }
                object[] args = new object[] { this.DropType };
                SdkManager.Bugly_Report("Drop_DropModel_Extra.cs", Utils.FormatString("DropOneIDData.GetRandomDrop DropType:{0} is invalid!", args));
                return new List<Drop_DropModel.DropData>();
            }

            public int DropType =>
                this.mDropData.DropType;

            public int DropID =>
                this.mDropData.DropID;
        }

        private class DropRandOne
        {
            public int RandomPercent;
            private int weight;
            private List<Drop_DropModel.DropSaveOneData> list = new List<Drop_DropModel.DropSaveOneData>();

            public void AddOne(string[] value)
            {
                int index = 0;
                int length = value.Length;
                while (index < length)
                {
                    Drop_DropModel.DropSaveOneData item = new Drop_DropModel.DropSaveOneData();
                    item.Init(value[index]);
                    this.weight += item.weight;
                    this.list.Add(item);
                    index++;
                }
            }

            public List<Drop_DropModel.DropData> GetAllDrop()
            {
                List<Drop_DropModel.DropData> list = new List<Drop_DropModel.DropData>();
                int num = 0;
                int count = this.list.Count;
                while (num < count)
                {
                    Drop_DropModel.DropSaveOneData data = this.list[num];
                    Drop_DropModel.DropData item = new Drop_DropModel.DropData((PropType) data.type, data.id, data.RandomCount());
                    list.Add(item);
                    num++;
                }
                return list;
            }

            public Drop_DropModel.DropData GetRandomDrop()
            {
                if (this.IsDrop())
                {
                    int num = GameLogic.Random(0, this.weight);
                    int num2 = 0;
                    int count = this.list.Count;
                    while (num2 < count)
                    {
                        Drop_DropModel.DropSaveOneData data = this.list[num2];
                        if (num < data.weight)
                        {
                            return new Drop_DropModel.DropData((PropType) data.type, data.id, data.RandomCount());
                        }
                        num -= data.weight;
                        num2++;
                    }
                }
                return null;
            }

            private bool IsDrop() => 
                (GameLogic.Random(0, 0x2710) < this.RandomPercent);
        }

        public class DropSaveOneData
        {
            public int type;
            public int id;
            public int min;
            public int max;
            public int weight;

            public void Init(string value)
            {
                char[] separator = new char[] { ',' };
                string[] strArray = value.Split(separator);
                if (strArray.Length == 5)
                {
                    int.TryParse(strArray[0], out this.type);
                    int.TryParse(strArray[1], out this.id);
                    int.TryParse(strArray[2], out this.min);
                    int.TryParse(strArray[3], out this.max);
                    int.TryParse(strArray[4], out this.weight);
                }
                else if (strArray.Length == 3)
                {
                    int.TryParse(strArray[0], out this.type);
                    int.TryParse(strArray[1], out this.id);
                    int.TryParse(strArray[2], out this.min);
                    this.max = this.min;
                }
            }

            public int RandomCount()
            {
                if ((this.type == 1) && (this.id == 0x7d1))
                {
                    float goldDropPercent = LocalModelManager.Instance.Drop_Drop.GetGoldDropPercent();
                    int min = (int) (this.min * goldDropPercent);
                    int num3 = (int) (this.max * goldDropPercent);
                    return GameLogic.Random(min, num3 + 1);
                }
                return GameLogic.Random(this.min, this.max + 1);
            }

            public int count
            {
                get
                {
                    if (this.min == this.max)
                    {
                        return this.min;
                    }
                    return this.RandomCount();
                }
            }
        }
    }
}

