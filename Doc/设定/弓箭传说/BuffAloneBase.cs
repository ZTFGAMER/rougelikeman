using Dxx.Util;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using TableTool;
using UnityEngine;

public class BuffAloneBase
{
    public const string Hit_Fixed = "FixedDamage";
    public const string Hit_AttackPercent = "Attack%";
    public const string Hit_AttackBasePercent = "AttackBase%";
    public const string Hit_HPMaxFrom = "SourceHPMax%";
    public const string Hit_HPFrom = "SourceHP%";
    public const string Hit_HPMaxTo = "TargetHPMax%";
    public const string Hit_HPTo = "TargetHP%";
    public const string RecoverHPPercent = "HPRecover%";
    public const string BodyHitPercent = "BodyHit%";
    public const string RecoverHPBasePercent = "HPRecoverBase%";
    public const string HPRecover = "HPRecover";
    private int _BuffID;
    protected List<Goods_goods.GoodData> attrList = new List<Goods_goods.GoodData>();
    protected Buff_alone buff_data;
    protected EntityBase m_Entity;
    protected EntityBase m_Target;
    private float startTime;
    private float endTime;
    private bool bForever;
    private bool isEnd;
    private bool bDizzyTrue;
    private bool bCanDizzy;
    private EElementType changeElement;
    protected float[] args;
    public Action<int> RemoveAction;
    private GameObject effect;
    protected List<BuffData> mEffects = new List<BuffData>();
    public const string SelfAttackPercent = "SelfAttack%";
    public const string SelfFixedDamage = "SelfFixedDamage";
    public const string OtherAttackPercent = "OtherAttack%";
    public const string OtherFixedDamage = "OtherFixedDamage";
    public const string ThunderRange = "Range";
    private float thunder_range;
    private float thunder_otherhit;
    private float thunder_selfhit;
    [CompilerGenerated]
    private static Dictionary<string, int> <>f__switch$map1;

    private void CreateEffect()
    {
        if (this.effect == null)
        {
            Fx_fx beanById = LocalModelManager.Instance.Fx_fx.GetBeanById(this.buff_data.FxId);
            if (beanById != null)
            {
                this.effect = GameLogic.EffectGet(beanById.Path);
                this.effect.transform.SetParent(this.m_Entity.GetKetNode(beanById.Node));
                if (beanById.Node != 7)
                {
                    this.effect.transform.localPosition = Vector3.zero;
                }
                else
                {
                    RectTransform transform = this.effect.transform as RectTransform;
                    transform.anchoredPosition = Vector3.zero;
                }
                this.effect.transform.localRotation = Quaternion.identity;
                this.effect.transform.localScale = Vector3.one;
            }
        }
    }

    private void DealBuffHit()
    {
        int num = 0;
        int count = this.mEffects.Count;
        while (num < count)
        {
            BuffData data = this.mEffects[num];
            if ((Updater.AliveTime - data.current_time) >= data.updatetime)
            {
                this.ExcuteBuff(data);
                data.current_time += data.updatetime;
                data.hurtCount++;
            }
            num++;
        }
    }

    private void Excute_Thunder()
    {
        this.thunder_range = 0f;
        this.thunder_otherhit = 0f;
        this.thunder_selfhit = 0f;
        int index = 0;
        int length = this.buff_data.FirstEffects.Length;
        while (index < length)
        {
            Goods_goods.GoodData goodData = Goods_goods.GetGoodData(this.buff_data.FirstEffects[index]);
            string goodType = goodData.goodType;
            if (goodType != null)
            {
                if (goodType != "SelfAttack%")
                {
                    if (goodType == "OtherAttack%")
                    {
                        goto Label_00E6;
                    }
                    if (goodType == "SelfFixedDamage")
                    {
                        goto Label_0121;
                    }
                    if (goodType == "OtherFixedDamage")
                    {
                        goto Label_0133;
                    }
                    if (goodType == "Range")
                    {
                        goto Label_0145;
                    }
                }
                else if (this.m_Target != null)
                {
                    this.thunder_selfhit = (((float) goodData.value) / 10000f) * this.m_Target.m_EntityData.GetAttackBase();
                }
            }
            goto Label_015E;
        Label_00E6:
            if (this.m_Target != null)
            {
                this.thunder_otherhit = (((float) goodData.value) / 10000f) * this.m_Target.m_EntityData.GetAttackBase();
            }
            goto Label_015E;
        Label_0121:
            this.thunder_selfhit = goodData.value;
            goto Label_015E;
        Label_0133:
            this.thunder_otherhit = goodData.value;
            goto Label_015E;
        Label_0145:
            this.thunder_range += goodData.value;
        Label_015E:
            index++;
        }
        float num3 = 1f;
        if (this.args.Length > 0f)
        {
            num3 *= this.args[0];
        }
        this.thunder_selfhit *= num3;
        this.thunder_otherhit *= num3;
        if (this.thunder_selfhit != 0f)
        {
            this.m_Entity.m_EntityData.ExcuteBuffs(this.m_Target, this.buff_data.BuffID, this.buff_data.Attribute, this.thunder_selfhit);
        }
        List<EntityBase> list = GameLogic.Release.Entity.GetRoundEntities(this.m_Entity, this.thunder_range, false);
        int num4 = 0;
        int count = list.Count;
        while (num4 < count)
        {
            EntityBase to = list[num4];
            to.m_EntityData.ExcuteBuffs(this.m_Target, this.buff_data.BuffID, this.buff_data.Attribute, this.thunder_otherhit);
            GameLogic.EffectGet("Effect/Attributes/ThunderLine").GetComponent<ThunderLineCtrl>().UpdateEntity(this.m_Entity, to);
            num4++;
        }
        if (list.Count > 0)
        {
            GameLogic.Hold.Sound.PlayBattleSpecial(0x4c4b4a, this.m_Entity.position);
        }
    }

    private void ExcuteAttribute()
    {
        if (this.buff_data.Attribute == "Att_Fire")
        {
            this.changeElement = EElementType.eFire;
            this.m_Entity.m_Body.AddElement(this.changeElement);
        }
        else if (this.buff_data.Attribute == "Att_Ice")
        {
            this.changeElement = EElementType.eIce;
            this.m_Entity.m_Body.AddElement(this.changeElement);
        }
    }

    private void ExcuteAttributes()
    {
        string[] attributes = this.buff_data.Attributes;
        int index = 0;
        int length = attributes.Length;
        while (index < length)
        {
            string str = attributes[index];
            this.attrList.Add(Goods_goods.GetGoodData(str));
            Goods_goods.GetAttribute(this.m_Entity, str);
            index++;
        }
    }

    protected virtual void ExcuteBuff(BuffData data)
    {
        if (data.hittype != string.Empty)
        {
            float num = 0f;
            string hittype = data.hittype;
            if (hittype != null)
            {
                if (<>f__switch$map1 == null)
                {
                    Dictionary<string, int> dictionary = new Dictionary<string, int>(11) {
                        { 
                            "FixedDamage",
                            0
                        },
                        { 
                            "Attack%",
                            1
                        },
                        { 
                            "AttackBase%",
                            2
                        },
                        { 
                            "SourceHPMax%",
                            3
                        },
                        { 
                            "SourceHP%",
                            4
                        },
                        { 
                            "TargetHPMax%",
                            5
                        },
                        { 
                            "TargetHP%",
                            6
                        },
                        { 
                            "HPRecover%",
                            7
                        },
                        { 
                            "HPRecover",
                            8
                        },
                        { 
                            "BodyHit%",
                            9
                        },
                        { 
                            "HPRecoverBase%",
                            10
                        }
                    };
                    <>f__switch$map1 = dictionary;
                }
                if (<>f__switch$map1.TryGetValue(hittype, out int num2))
                {
                    switch (num2)
                    {
                        case 0:
                            num = data.value;
                            break;

                        case 1:
                            if (this.m_Target != null)
                            {
                                int attack = 0;
                                if ((this.m_Target != null) && (this.m_Target.m_Weapon != null))
                                {
                                    attack = this.m_Target.m_Weapon.m_Data.Attack;
                                }
                                num = this.m_Target.m_EntityData.GetAttackBase(attack) * data.value;
                                if (num == 0f)
                                {
                                    num = this.m_Target.m_EntityData.GetBodyHit() * data.value;
                                }
                            }
                            break;

                        case 2:
                            if (this.m_Target != null)
                            {
                                num = this.m_Target.m_EntityData.attribute.AttackValue.ValueCount * data.value;
                            }
                            break;

                        case 3:
                            if (this.m_Target != null)
                            {
                                num = this.m_Target.m_EntityData.MaxHP * data.value;
                            }
                            break;

                        case 4:
                            if (this.m_Target != null)
                            {
                                num = this.m_Target.m_EntityData.CurrentHP * data.value;
                            }
                            break;

                        case 5:
                            num = this.m_Entity.m_EntityData.MaxHP * data.value;
                            break;

                        case 6:
                            num = this.m_Entity.m_EntityData.CurrentHP * data.value;
                            break;

                        case 7:
                            num = this.m_Entity.m_EntityData.MaxHP * data.value;
                            break;

                        case 8:
                            num = data.value;
                            break;

                        case 9:
                            if (this.m_Target != null)
                            {
                                num = this.m_Target.m_EntityData.GetBodyHit() * data.value;
                            }
                            break;

                        case 10:
                            num = this.m_Entity.m_EntityData.attribute.HPValue.ValueCount * data.value;
                            break;
                    }
                }
            }
            num = this.OnValue(num);
            num = this.ExcuteElement(data, num);
            if ((MathDxx.Abs(num) >= 1f) && (this.m_Entity != null))
            {
                this.m_Entity.m_EntityData.ExcuteBuffs(this.m_Target, this.buff_data.BuffID, this.buff_data.Attribute, num);
            }
        }
    }

    private void ExcuteEffects()
    {
        string[] effects = this.buff_data.Effects;
        int index = 0;
        int length = effects.Length;
        while (index < length)
        {
            string str = effects[index];
            this.mEffects.Add(this.GetGoodDataEvery(str));
            index++;
        }
    }

    private float ExcuteElement(BuffData data, float value)
    {
        string attribute = this.buff_data.Attribute;
        if (attribute != null)
        {
            if (attribute != "Att_Fire")
            {
                if ((attribute == "Att_Poison") && (this.m_Target != null))
                {
                    value -= this.m_Target.m_EntityData.attribute.Att_Poison_Add.Value;
                    value *= 1f + this.m_Target.m_EntityData.attribute.Att_Poison_AddPercent.Value;
                }
                return value;
            }
            if (this.m_Target != null)
            {
                value -= this.m_Target.m_EntityData.attribute.Att_Fire_Add.Value;
                value *= 1f + this.m_Target.m_EntityData.attribute.Att_Fire_AddPercent.Value;
            }
        }
        return value;
    }

    private void ExcuteFirstEffects()
    {
        if (this.buff_data.Attribute == "Att_Thunder")
        {
            this.Excute_Thunder();
        }
        else
        {
            string[] firstEffects = this.buff_data.FirstEffects;
            int index = 0;
            int length = firstEffects.Length;
            while (index < length)
            {
                string str = firstEffects[index];
                this.ExcuteBuff(this.GetGoodDataFirst(str));
                index++;
            }
        }
    }

    private BuffData GetGoodDataEvery(string str)
    {
        char[] separator = new char[] { ' ' };
        string[] strArray = str.Split(separator);
        BuffData data = new BuffData {
            hittype = strArray[0],
            symbol = Goods_goods.GetSymbol(strArray[1])
        };
        char[] chArray2 = new char[] { '/' };
        string[] strArray2 = strArray[2].Split(chArray2);
        data.updatetime = ((float) int.Parse(strArray2[0])) / 1000f;
        data.value = float.Parse(strArray2[1]) * data.symbol;
        if (strArray[0].Contains("%"))
        {
            data.value /= 100f;
        }
        data.current_time = Updater.AliveTime;
        return data;
    }

    private BuffData GetGoodDataFirst(string str)
    {
        char[] separator = new char[] { ' ' };
        string[] strArray = str.Split(separator);
        BuffData data = new BuffData {
            hittype = strArray[0],
            symbol = Goods_goods.GetSymbol(strArray[1])
        };
        data.value = float.Parse(strArray[2]) * data.symbol;
        if (strArray[0].Contains("%"))
        {
            data.value /= 100f;
        }
        return data;
    }

    public void Init(EntityBase entity, BattleStruct.BuffStruct data, Buff_alone buff_data)
    {
        this.buff_data = buff_data;
        this._BuffID = buff_data.BuffID;
        this.m_Entity = entity;
        this.m_Target = data.entity;
        this.args = data.args;
        this.bForever = buff_data.Time == 0;
        this.ResetBuffTime(true);
        this.InitEffects();
        this.OnStart();
    }

    private void InitEffects()
    {
        bool flag = false;
        if (this.buff_data.BuffType == 1)
        {
            if (GameLogic.Random(0, 100) < this.buff_data.DizzyChance)
            {
                this.CreateEffect();
                this.bDizzyTrue = true;
                this.bCanDizzy = this.m_Entity.m_EntityData.GetCanDizzy();
                if (this.bCanDizzy)
                {
                    flag = true;
                    this.m_Entity.m_EntityData.UpdateDizzy(1);
                }
            }
        }
        else
        {
            this.CreateEffect();
        }
        this.ExcuteFirstEffects();
        if (flag || (this.buff_data.BuffType != 1))
        {
            this.ExcuteAttribute();
            this.ExcuteAttributes();
            this.ExcuteEffects();
        }
    }

    public bool IsEnd() => 
        (!this.bForever && (Updater.AliveTime >= this.endTime));

    protected virtual void OnRemove()
    {
    }

    protected virtual void OnResetBuffTime()
    {
    }

    protected virtual void OnStart()
    {
    }

    public void OnUpdate(float delta)
    {
        if ((Updater.AliveTime >= this.startTime) && !this.isEnd)
        {
            this.DealBuffHit();
            if (!this.bForever && (Updater.AliveTime >= this.endTime))
            {
                this.isEnd = true;
            }
        }
    }

    protected virtual float OnValue(float value) => 
        value;

    public void Remove()
    {
        this.RemoveAttributes();
        if (this.bDizzyTrue && this.bCanDizzy)
        {
            this.m_Entity.m_EntityData.UpdateDizzy(-1);
        }
        this.RemoveEffect();
        if (this.changeElement != EElementType.eNone)
        {
            this.m_Entity.m_Body.RemoveElement(this.changeElement);
        }
        this.OnRemove();
    }

    private void RemoveAttributes()
    {
        int num = 0;
        int count = this.attrList.Count;
        while (num < count)
        {
            Goods_goods.GoodData data = this.attrList[num];
            data.value = -data.value;
            Goods_goods.GetAttribute(this.m_Entity, data);
            num++;
        }
    }

    private void RemoveEffect()
    {
        if (this.effect != null)
        {
            GameLogic.EffectCache(this.effect);
            this.effect = null;
        }
    }

    public void ResetBuffTime(bool force = false)
    {
        if ((this.buff_data.BuffType != 1) || force)
        {
            this.OnResetBuffTime();
            this.startTime = Updater.AliveTime + this.buff_data.Delay_time;
            if ((this.buff_data.Attribute == "Att_Ice") && (this.m_Entity.Type == EntityType.Boss))
            {
                this.endTime = ((((float) this.buff_data.Time) / 1000f) / 2f) + this.startTime;
            }
            else
            {
                this.endTime = (((float) this.buff_data.Time) / 1000f) + this.startTime;
            }
            int num = 0;
            int count = this.mEffects.Count;
            while (num < count)
            {
                BuffData data = this.mEffects[num];
                data.hurtCount = 0;
                num++;
            }
        }
    }

    public int BuffID =>
        this._BuffID;

    public class BuffData
    {
        public int symbol;
        public float updatetime;
        public float value;
        public int hurtCount;
        public string hittype;
        public float current_time;
    }
}

