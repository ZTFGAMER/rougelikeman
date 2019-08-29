namespace TableTool
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class Goods_food : LocalBean
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <GoodID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Notes>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <DropSound>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <GetSound>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string[] <Values>k__BackingField;
        private List<Goods_goods.GoodData> list = new List<Goods_goods.GoodData>();

        public Goods_food Copy() => 
            new Goods_food { 
                GoodID = this.GoodID,
                Notes = this.Notes,
                DropSound = this.DropSound,
                GetSound = this.GetSound,
                Values = this.Values
            };

        private void DeadGoods(EntityBase entity)
        {
            for (int i = 0; i < this.list.Count; i++)
            {
                Goods_goods.GoodData data = this.list[i];
                GetAttribute(entity, data);
            }
        }

        private void DealGoodsData()
        {
            if (this.list.Count == 0)
            {
                for (int i = 0; i < this.Values.Length; i++)
                {
                    this.list.Add(Goods_goods.GetGoodData(this.Values[i]));
                }
            }
        }

        public static void GetAttribute(EntityBase entity, string str)
        {
            Goods_goods.GoodData goodData = Goods_goods.GetGoodData(str);
            GetAttribute(entity, goodData);
        }

        public static void GetAttribute(EntityBase entity, Goods_goods.GoodData data)
        {
            entity.m_EntityData.ExcuteAttributes(data);
        }

        public void GetGoods(EntityBase entity)
        {
            if (this.list.Count == 0)
            {
                this.DealGoodsData();
            }
            this.DeadGoods(entity);
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

        protected override bool ReadImpl()
        {
            this.GoodID = base.readInt();
            this.Notes = base.readLocalString();
            this.DropSound = base.readLocalString();
            this.GetSound = base.readInt();
            this.Values = base.readArraystring();
            return true;
        }

        public int GoodID { get; private set; }

        public string Notes { get; private set; }

        public string DropSound { get; private set; }

        public int GetSound { get; private set; }

        public string[] Values { get; private set; }
    }
}

