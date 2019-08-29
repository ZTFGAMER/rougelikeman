namespace TableTool
{
    using Dxx.Util;
    using System;
    using System.Collections.Generic;

    public class Equip_equipModel : LocalModel<Equip_equip, int>
    {
        private const string _Filename = "Equip_equip";
        private Dictionary<int, List<int>> mQualities = new Dictionary<int, List<int>>();
        private List<int> mEquipExps = new List<int>();

        private void add_quality(int id, int quality)
        {
            if (this.mQualities.TryGetValue(quality, out List<int> list))
            {
                list.Add(id);
            }
            else
            {
                list = new List<int> {
                    id
                };
                this.mQualities.Add(quality, list);
            }
        }

        public List<int> GetAdditionSkills_ui(LocalSave.EquipOne one)
        {
            List<int> list = new List<int>();
            Equip_equip beanById = base.GetBeanById(one.EquipID);
            int index = 0;
            int length = beanById.AdditionSkills.Length;
            while (index < length)
            {
                if (int.TryParse(beanById.AdditionSkills[index], out int num))
                {
                    list.Add(num);
                }
                index++;
            }
            return list;
        }

        public int GetAttributeAllCount(int equipid)
        {
            Equip_equip beanById = base.GetBeanById(equipid);
            if (beanById == null)
            {
                return 0;
            }
            int length = beanById.Attributes.Length;
            int num2 = beanById.Skills.Length;
            return (length + num2);
        }

        protected override int GetBeanKey(Equip_equip bean) => 
            bean.Id;

        public List<string> GetEquipAddAttributes(LocalSave.EquipOne one)
        {
            List<string> list = new List<string>();
            Equip_equip data = one.data;
            int index = 0;
            int length = data.AdditionSkills.Length;
            while (index < length)
            {
                string s = data.AdditionSkills[index];
                if (!int.TryParse(s, out _))
                {
                    list.Add(data.AdditionSkills[index]);
                }
                index++;
            }
            return list;
        }

        public List<Goods_goods.GoodData> GetEquipAttributes(LocalSave.EquipOne one)
        {
            List<Goods_goods.GoodData> list = new List<Goods_goods.GoodData>();
            Equip_equip beanById = base.GetBeanById(one.EquipID);
            int level = one.Level;
            int index = 0;
            int length = beanById.Attributes.Length;
            while (index < length)
            {
                Goods_goods.GoodData goodData = Goods_goods.GetGoodData(beanById.Attributes[index]);
                if (goodData.percent)
                {
                    goodData.value += ((level - 1) * beanById.AttributesUp[index]) * 100;
                }
                else
                {
                    goodData.value += (level - 1) * beanById.AttributesUp[index];
                }
                list.Add(goodData);
                index++;
            }
            return list;
        }

        public List<string> GetEquipAttributesNext(LocalSave.EquipOne one)
        {
            LocalSave.EquipOne one2 = new LocalSave.EquipOne {
                EquipID = one.EquipID,
                Level = one.Level + 1
            };
            List<Goods_goods.GoodData> equipAttributes = this.GetEquipAttributes(one);
            List<Goods_goods.GoodData> list2 = this.GetEquipAttributes(one2);
            List<string> list3 = new List<string>();
            Equip_equip beanById = base.GetBeanById(one.EquipID);
            int index = 0;
            int length = beanById.Attributes.Length;
            while (index < length)
            {
                Goods_goods.GoodData goodData = Goods_goods.GetGoodData(beanById.Attributes[index]);
                string item = string.Empty;
                if (goodData.percent)
                {
                    object[] args = new object[] { beanById.AttributesUp[index] };
                    item = item + Utils.FormatString("+ {0}%", args);
                }
                else
                {
                    equipAttributes[index].value = (long) (equipAttributes[index].value * (1f + GameLogic.SelfAttributeShow.GetUpPercent(one.Position)));
                    list2[index].value = (long) (list2[index].value * (1f + GameLogic.SelfAttributeShow.GetUpPercent(one.Position)));
                    object[] args = new object[] { list2[index].value - equipAttributes[index].value };
                    item = item + Utils.FormatString("+ {0}", args);
                }
                list3.Add(item);
                index++;
            }
            return list3;
        }

        public List<string> GetEquipShowAddAttributes(LocalSave.EquipOne one)
        {
            List<string> list = new List<string>();
            Equip_equip beanById = base.GetBeanById(one.EquipID);
            int index = 0;
            int length = beanById.AdditionSkills.Length;
            while (index < length)
            {
                string item = string.Empty;
                if (!int.TryParse(beanById.AdditionSkills[index], out int num3))
                {
                    string str2 = beanById.AdditionSkills[index];
                    Goods_goods.GoodShowData goodShowData = Goods_goods.GetGoodShowData(str2);
                    if (one.IsBaby && !str2.Contains("EquipBaby:"))
                    {
                        string languageByTID = GameLogic.Hold.Language.GetLanguageByTID("Attr_BabyParentContent", Array.Empty<object>());
                        object[] args = new object[] { languageByTID, goodShowData.goodType };
                        goodShowData.goodType = Utils.FormatString("{0}{1}", args);
                    }
                    item = goodShowData.ToString();
                }
                else
                {
                    object[] args = new object[] { num3 };
                    item = GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("技能描述{0}", args), Array.Empty<object>());
                }
                list.Add(item);
                index++;
            }
            return list;
        }

        public List<Goods_goods.GoodShowData> GetEquipShowAttrs(LocalSave.EquipOne one)
        {
            List<Goods_goods.GoodShowData> list = new List<Goods_goods.GoodShowData>();
            int level = one.Level;
            Equip_equip beanById = base.GetBeanById(one.EquipID);
            int index = 0;
            int length = beanById.Attributes.Length;
            while (index < length)
            {
                Goods_goods.GoodData goodData = Goods_goods.GetGoodData(beanById.Attributes[index]);
                level = MathDxx.Clamp(level, 0, one.CurrentMaxLevel);
                if (goodData.percent)
                {
                    goodData.value += ((level - 1) * beanById.AttributesUp[index]) * 100;
                }
                else
                {
                    goodData.value += (level - 1) * beanById.AttributesUp[index];
                }
                goodData.value = (long) (goodData.value * (1f + GameLogic.SelfAttributeShow.GetUpPercent(one.Position)));
                Goods_goods.GoodShowData goodShowData = Goods_goods.GetGoodShowData(goodData);
                if (one.IsBaby && !goodData.goodType.Contains("EquipBaby:"))
                {
                    string languageByTID = GameLogic.Hold.Language.GetLanguageByTID("Attr_BabyParentContent", Array.Empty<object>());
                    object[] args = new object[] { languageByTID, goodShowData.goodType };
                    goodShowData.goodType = Utils.FormatString("{0}{1}", args);
                }
                list.Add(goodShowData);
                index++;
            }
            return list;
        }

        public List<int> GetListByPosition(int position)
        {
            List<int> list = new List<int>();
            IList<Equip_equip> allBeans = base.GetAllBeans();
            int num = 0;
            int count = allBeans.Count;
            while (num < count)
            {
                Equip_equip _equip = allBeans[num];
                if (_equip.Position == position)
                {
                    list.Add(_equip.Id);
                }
                num++;
            }
            return list;
        }

        public List<int> GetQuality(int quality)
        {
            if (this.mQualities.TryGetValue(quality, out List<int> list))
            {
                return list;
            }
            return null;
        }

        public List<int> GetSkills(LocalSave.EquipOne one)
        {
            List<int> list = new List<int>();
            Equip_equip beanById = base.GetBeanById(one.EquipID);
            int index = 0;
            int length = beanById.AdditionSkills.Length;
            while (index < length)
            {
                if (int.TryParse(beanById.AdditionSkills[index], out int num))
                {
                    list.Add(num);
                }
                index++;
            }
            return list;
        }

        public void Init()
        {
            IList<Equip_equip> allBeans = base.GetAllBeans();
            int num = 0;
            int count = allBeans.Count;
            while (num < count)
            {
                Equip_equip _equip = allBeans[num];
                if (_equip.Overlying == 0)
                {
                    this.add_quality(_equip.Id, _equip.Quality);
                }
                else
                {
                    this.mEquipExps.Add(_equip.Id);
                }
                num++;
            }
        }

        public int RandomByQuality(int quality)
        {
            List<int> list = this.GetQuality(quality);
            if (list == null)
            {
                return 0;
            }
            int num = GameLogic.Random(0, list.Count);
            return list[num];
        }

        public int RandomEquipExp()
        {
            int num = GameLogic.Random(0, this.mEquipExps.Count);
            return this.mEquipExps[num];
        }

        protected override string Filename =>
            "Equip_equip";
    }
}

