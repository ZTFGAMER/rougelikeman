namespace TableTool
{
    using Dxx.Util;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class Goods_goods : LocalBean
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <GoodID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Notes>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <GoodsType>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Ground>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <DropSound>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <GetSound>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <SizeX>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <SizeY>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <OffsetX>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <OffsetY>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string[] <Args>k__BackingField;
        private List<GoodData> list = new List<GoodData>();

        public Goods_goods Copy() => 
            new Goods_goods { 
                GoodID = this.GoodID,
                Notes = this.Notes,
                GoodsType = this.GoodsType,
                Ground = this.Ground,
                DropSound = this.DropSound,
                GetSound = this.GetSound,
                SizeX = this.SizeX,
                SizeY = this.SizeY,
                OffsetX = this.OffsetX,
                OffsetY = this.OffsetY,
                Args = this.Args
            };

        private void DeadGoods(EntityBase entity)
        {
            for (int i = 0; i < this.list.Count; i++)
            {
                GoodData data = this.list[i];
                GetAttribute(entity, data);
            }
        }

        private void DealGoodsData()
        {
            if (this.list.Count == 0)
            {
                for (int i = 0; i < this.Args.Length; i++)
                {
                    this.list.Add(GetGoodData(this.Args[i]));
                }
            }
        }

        public static void GetAttribute(EntityBase entity, string str)
        {
            GoodData goodData = GetGoodData(str);
            GetAttribute(entity, goodData);
        }

        public static void GetAttribute(EntityBase entity, GoodData data)
        {
            entity.m_EntityData.ExcuteAttributes(data);
        }

        public static GoodData GetGoodData(string str)
        {
            char[] separator = new char[] { ' ' };
            string[] strArray = str.Split(separator);
            if (strArray.Length != 3)
            {
                object[] args = new object[] { str };
                SdkManager.Bugly_Report("Goods_goods_Extra", Utils.FormatString("GetGoodData(string str)[{0}] is invalid, ���ܶ���ո���ٸ��ո�.", args));
            }
            GoodData data = new GoodData();
            if (str != string.Empty)
            {
                data.goodType = strArray[0];
                data.percent = strArray[0].Contains("%");
                if (data.percent)
                {
                    data.value = GetSymbol(strArray[1]) * ((long) (float.Parse(strArray[2]) * 100f));
                    return data;
                }
                float num = float.Parse(strArray[2]);
                data.value = GetSymbol(strArray[1]) * ((long) num);
            }
            return data;
        }

        public void GetGoods(EntityBase entity)
        {
            if (this.list.Count == 0)
            {
                this.DealGoodsData();
            }
            this.DeadGoods(entity);
        }

        public static GoodShowData GetGoodShowData(string value) => 
            GetGoodShowData(GetGoodData(value));

        public static GoodShowData GetGoodShowData(GoodData data)
        {
            GoodShowData data2 = new GoodShowData();
            object[] args = new object[] { data.goodType };
            data2.iconname = Utils.FormatString("Attr_{0}", args);
            string languageByTID = GameLogic.Hold.Language.GetLanguageByTID(data2.iconname, Array.Empty<object>());
            string str2 = (data.value <= 0L) ? "-" : "+";
            string str3 = !data.percent ? string.Empty : "%";
            data2.goodType = languageByTID;
            data2.symbol = str2;
            if (data.percent)
            {
                object[] objArray2 = new object[] { (data.value <= 0L) ? (((float) -data.value) / 100f) : (((float) data.value) / 100f), str3 };
                data2.value = Utils.FormatString("{0}{1}", objArray2);
                return data2;
            }
            object[] objArray3 = new object[] { data.value };
            data2.value = Utils.FormatString("{0}", objArray3);
            return data2;
        }

        public static int GetSymbol(string s)
        {
            if (s != null)
            {
                if (s == "+")
                {
                    return 1;
                }
                if (s == "-")
                {
                    return -1;
                }
            }
            return 0;
        }

        public static string GoodDataToString(GoodData data)
        {
            object[] args = new object[] { data.goodType, (data.value <= 0L) ? "-" : "+", Mathf.Abs((float) data.value) };
            return Utils.FormatString("{0} {1} {2}", args);
        }

        protected override bool ReadImpl()
        {
            this.GoodID = base.readInt();
            this.Notes = base.readLocalString();
            this.GoodsType = base.readInt();
            this.Ground = base.readInt();
            this.DropSound = base.readLocalString();
            this.GetSound = base.readInt();
            this.SizeX = base.readInt();
            this.SizeY = base.readInt();
            this.OffsetX = base.readFloat();
            this.OffsetY = base.readFloat();
            this.Args = base.readArraystring();
            return true;
        }

        public int GoodID { get; private set; }

        public string Notes { get; private set; }

        public int GoodsType { get; private set; }

        public int Ground { get; private set; }

        public string DropSound { get; private set; }

        public int GetSound { get; private set; }

        public int SizeX { get; private set; }

        public int SizeY { get; private set; }

        public float OffsetX { get; private set; }

        public float OffsetY { get; private set; }

        public string[] Args { get; private set; }

        public class GoodData
        {
            public string goodType;
            public long value;
            public bool percent;

            public string GetSymbolString() => 
                ((this.value < 0L) ? "-" : "+");

            public override string ToString()
            {
                object[] args = new object[] { this.goodType, this.value };
                return Utils.FormatString("GoodData:{0} {1}", args);
            }
        }

        public class GoodShowData
        {
            public string goodType;
            public string iconname;
            public string symbol;
            public string value;

            public override string ToString()
            {
                object[] args = new object[] { this.goodType, this.symbol, this.value };
                return Utils.FormatString("{0} {1} {2}", args);
            }
        }
    }
}

