using Dxx.Util;
using PureMVC.Patterns;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TableTool;
using UnityEngine;

public class EntityData
{
    public int CharID;
    private EntityBase m_Entity;
    public int mDeadRecover;
    private long mHP2AttackSpeed;
    private long mHP2Miss;
    private int mHitCreate2;
    public float mHitCreate2Percent;
    private int mFlyStoneCount;
    private int mFlyWaterCount;
    private int mBulletThroughCount;
    private int DizzyCount;
    private float mDizzyTime;
    [NonSerialized]
    public int ExtraSkillCount;
    public long CurrentHP;
    [Header("最大血量")]
    public long MaxHP;
    private HitStruct Attack = new HitStruct();
    public int InvincibleCount;
    [NonSerialized]
    public float BulletSpeed = 1f;
    private int MissHP_Count;
    private WeightRandom<AttackCallData> mAttackMeteorite = new WeightRandom<AttackCallData>();
    public EntityAttributeBase attribute;
    private float mHP2AttackRatio;
    private int mThroughEnemy;
    private float mThroughRatio = 1f;
    private int mBulletLine;
    public BulletBase mLastBullet;
    private int mBulletSputter;
    private int mBulletSpeedHittedCount;
    private float mBulletSpeedHitted = 1f;
    private float mBulletSpeedHittedTime;
    private float mBulletSpeed1Ratio = 1f;
    private float mBulletSpeed1Range;
    private float mBulletSpeed = 1f;
    public float HittedInterval;
    public int TurnTableCount;
    private int mBulletScaleCount;
    private int mOnlyDemonCount;
    private int mBabyResistBulletCount;
    private int mFrontShieldCount;
    private int mLight45;
    private StaticAddData mStaticReduce;
    private MoveAddData mMoveAdd;
    public Dictionary<EElementType, BuffAttrData> mBuffAttrList;
    public static Dictionary<EElementType, ElementDataClass> ElementData;
    public EElementType ArrowTrailType;
    public EElementType ArrowHeadType;
    private List<EntityBabyBase> mBabies;
    public List<string> mBabyAttributes;
    public List<string> mSelfAttributes;
    public List<int> mBabySkillIds;
    public List<int> mSelfSkillIds;
    private WeightRandom<DeadCallData> mCallWeight;
    public int MaxLevel;
    private int Level;
    private float Exp;
    private ProgressAniManager exp_data;
    private Dictionary<int, float> explist;
    private bool bInitHeadShot;
    private bool bHeadShot;
    private long Shield_Count;
    private long Shield_CurrentCount;
    public long Shield_CurrentHitValue;
    private GameObject mShieldObj;
    private float hittedSoundTime;
    private float mRebornStartTime;
    private AnimationCurve mRebornCurve;
    private const float mRebornAllTime = 1.5f;
    private float mTrapHitTime;
    [CompilerGenerated]
    private static Dictionary<string, int> <>f__switch$map4;

    static EntityData()
    {
        Dictionary<EElementType, ElementDataClass> dictionary = new Dictionary<EElementType, ElementDataClass> {
            { 
                EElementType.eNone,
                new ElementDataClass()
            }
        };
        ElementDataClass class2 = new ElementDataClass {
            TrailPriority = 0,
            TrailPath = string.Empty,
            HeadPriority = 20,
            HeadPath = "Effect/Attributes/ThunderHead",
            color = new Color(1f, 1f, 0.1921569f)
        };
        dictionary.Add(EElementType.eThunder, class2);
        class2 = new ElementDataClass {
            TrailPriority = 0,
            TrailPath = string.Empty,
            HeadPriority = 10,
            HeadPath = "Effect/Attributes/FireHead",
            color = new Color(1f, 0.3529412f, 0f)
        };
        dictionary.Add(EElementType.eFire, class2);
        class2 = new ElementDataClass {
            TrailPriority = 10,
            TrailPath = "Effect/Attributes/IceTrail",
            HeadPriority = 0,
            HeadPath = string.Empty,
            color = new Color(0f, 0.9568627f, 1f)
        };
        dictionary.Add(EElementType.eIce, class2);
        class2 = new ElementDataClass {
            TrailPriority = 20,
            TrailPath = "Effect/Attributes/PoisonTrail",
            HeadPriority = 0,
            HeadPath = string.Empty,
            color = new Color(0.6470588f, 0f, 0.7882353f)
        };
        dictionary.Add(EElementType.ePoison, class2);
        class2 = new ElementDataClass {
            TrailPriority = 0,
            TrailPath = string.Empty,
            HeadPriority = 20,
            HeadPath = "Effect/Attributes/ThunderFireHead",
            color = new Color(1f, 1f, 0.1921569f)
        };
        dictionary.Add(EElementType.eThunderFire, class2);
        ElementData = dictionary;
    }

    public EntityData()
    {
        Dictionary<EElementType, BuffAttrData> dictionary = new Dictionary<EElementType, BuffAttrData> {
            { 
                EElementType.eNone,
                new BuffAttrData()
            },
            { 
                EElementType.eThunder,
                new BuffAttrData()
            },
            { 
                EElementType.eFire,
                new BuffAttrData()
            },
            { 
                EElementType.eIce,
                new BuffAttrData()
            },
            { 
                EElementType.ePoison,
                new BuffAttrData()
            }
        };
        this.mBuffAttrList = dictionary;
        this.mBabies = new List<EntityBabyBase>();
        this.mBabyAttributes = new List<string>();
        this.mSelfAttributes = new List<string>();
        this.mBabySkillIds = new List<int>();
        this.mSelfSkillIds = new List<int>();
        this.mCallWeight = new WeightRandom<DeadCallData>();
        this.Level = 1;
        this.exp_data = new ProgressAniManager();
        this.explist = new Dictionary<int, float>();
    }

    public void AddAttackMeteorite(AttackCallData data)
    {
        this.mAttackMeteorite.Add(data, data.weight);
    }

    public void AddBaby(EntityBabyBase entity)
    {
        this.mBabies.Add(entity);
    }

    public void AddBabyAttribute(string value)
    {
        this.mBabyAttributes.Add(value);
        this.BabyUpdateAttributes();
    }

    public void AddBabyLearnSkillId(int skillid)
    {
        this.mBabySkillIds.Add(skillid);
        this.BabyUpdateSkillIds();
    }

    public void AddDeadCall(DeadCallData data)
    {
        this.mCallWeight.Add(data, data.weight);
    }

    public void AddDeBuff(EElementType element)
    {
    }

    public void AddElement(EElementType type)
    {
        BuffAttrData local1 = this.mBuffAttrList[type];
        local1.count++;
        this.ArrowTrailType = this.GetTrailType();
        this.ArrowHeadType = this.GetHeadType();
    }

    public void AddExp(float exp)
    {
        if (this.Level != this.MaxLevel)
        {
            this.exp_data.Play(exp);
            this.Level = this.exp_data.currentlevel;
            this.Exp = this.exp_data.currentvalue;
        }
    }

    public void AddShieldCount(long count)
    {
        this.Shield_Count += count;
        this.Shield_CurrentCount += count;
        this.UpdateShieldCount();
    }

    public void AddShieldCountAction(Action<long> action)
    {
        this.m_Entity.Shield_CountAction = (Action<long>) Delegate.Combine(this.m_Entity.Shield_CountAction, action);
    }

    public void Attribute_HP(long value)
    {
        long num = (long) (((((value * (1f + GameConfig.MapGood.GetHPAddPercent())) + this.attribute.HPAdd.Value) + MathDxx.CeilBig(this.attribute.GetHPBase() * 0.05f)) * (1f + this.attribute.HPAddPercent.Value)) * (1f + this.GetHP2HPAddPercent()));
        if ((num < 0L) && ((this.CurrentHP + num) <= 0L))
        {
            num = -(this.CurrentHP - 1L);
        }
        GameLogic.Send_Recover(this.m_Entity, num);
    }

    public void Attribute_HPBasePercent(long value)
    {
        long num = (long) ((((((float) (this.attribute.GetHPBase() * value)) / 10000f) * (1f + GameConfig.MapGood.GetHPAddPercent())) + this.attribute.HPAdd.Value) * (1f + this.attribute.HPAddPercent.Value));
        if ((num < 0L) && ((this.CurrentHP + num) <= 0L))
        {
            num = -(this.CurrentHP - 1L);
        }
        GameLogic.Send_Recover(this.m_Entity, num);
    }

    public void Attribute_HPPercent(long value)
    {
        long num = (long) (((((((float) (this.attribute.HPValue.Value * value)) / 10000f) * (1f + GameConfig.MapGood.GetHPAddPercent())) + this.attribute.HPAdd.Value) * (1f + this.attribute.HPAddPercent.Value)) * (1f + this.GetHP2HPAddPercent()));
        if ((num < 0L) && ((this.CurrentHP + num) <= 0L))
        {
            num = -(this.CurrentHP - 1L);
        }
        GameLogic.Send_Recover(this.m_Entity, num);
    }

    public void BabyResistBullet(bool value)
    {
        int num = 0;
        int count = this.mBabies.Count;
        while (num < count)
        {
            this.mBabies[num].SetCollider(value);
            num++;
        }
    }

    public void BabyUpdateAttributes()
    {
        int num = 0;
        int count = this.mBabies.Count;
        while (num < count)
        {
            this.mBabies[num].UpdateAttributes();
            num++;
        }
    }

    public void BabyUpdateSkillIds()
    {
        int num = 0;
        int count = this.mBabies.Count;
        while (num < count)
        {
            this.mBabies[num].UpdateSkillIds();
            num++;
        }
    }

    public long ChangeHP(EntityBase entity, long HP)
    {
        if (this.m_Entity.Type == EntityType.Baby)
        {
            HP = 0L;
        }
        if (this.m_Entity.IsSelf)
        {
            if (!GameLogic.Hold.BattleData.Challenge_RecoverHP() && (HP > 0L))
            {
                HP = 0L;
            }
            if ((((this.CurrentHP + HP) <= 0L) && (this.mDeadRecover > 0)) && (MathDxx.Abs((float) (((float) (this.CurrentHP + HP)) / ((float) this.MaxHP))) < 0.3f))
            {
                if (GameConfig.GetFirstDeadRecover())
                {
                    HP = GameLogic.Random(1, 50) - this.CurrentHP;
                }
                this.UseDeadRecover();
            }
        }
        long num2 = HP;
        this.CurrentHP += HP;
        if (this.CurrentHP > this.MaxHP)
        {
            this.CurrentHP = this.MaxHP;
        }
        else if (this.CurrentHP < 0L)
        {
            num2 -= this.CurrentHP;
            this.CurrentHP = 0L;
        }
        if ((this.CurrentHP == 0L) && (this.m_Entity.OnWillDead != null))
        {
            this.m_Entity.OnWillDead();
        }
        if ((this.CurrentHP == 0L) && this.attribute.GetCanReborn())
        {
            LocalSave.Instance.BattleIn_AddRebornSkill();
            long hP = this.attribute.RebornHP.Value + MathDxx.CeilToInt(this.attribute.RebornHPPercent.Value * this.MaxHP);
            this.ChangeHP(entity, hP);
            this.CurrentHP = hP;
            this.RebornUpdate();
        }
        if (HP > 0L)
        {
            this.m_Entity.PlayEffect(0x2f4d6a);
            if (this.m_Entity.IsSelf)
            {
                GameLogic.Hold.Sound.PlayBattleSpecial(0x4c4b46, this.m_Entity.position);
            }
        }
        if ((this.m_Entity.m_HPSlider != null) && this.m_Entity.m_HPSlider.gameObject.activeInHierarchy)
        {
            this.m_Entity.m_HPSlider.UpdateHP();
        }
        if (this.m_Entity.GetIsDead())
        {
            BattleStruct.DeadStruct data = new BattleStruct.DeadStruct {
                entity = this.m_Entity
            };
            this.m_Entity.ExcuteCommend(EBattleAction.EBattle_Action_Dead_Before, data);
            BattleStruct.DeadStruct struct3 = new BattleStruct.DeadStruct {
                entity = this.m_Entity
            };
            this.m_Entity.ExcuteCommend(EBattleAction.EBattle_Action_Dead, struct3);
            if ((this.m_Entity.m_Data.Divide == 0) && (entity != null))
            {
                this.DoDeadCommand(entity);
            }
        }
        if (this.m_Entity.OnChangeHPAction != null)
        {
            this.m_Entity.OnChangeHPAction(this.CurrentHP, this.MaxHP, this.GetHPPercent(), num2);
        }
        if ((this.GetHPPercent() == 1f) && (this.m_Entity.OnFullHP != null))
        {
            this.m_Entity.OnFullHP();
        }
        this.m_Entity.CurrentHPUpdate();
        if (this.mHP2AttackSpeed > 0L)
        {
            this.ExcuteAttributes("AttackSpeed%", -this.mHP2AttackSpeed);
        }
        this.mHP2AttackSpeed = (long) ((this.attribute.HP2AttackSpeed.Value * 10000f) * (1f - this.GetHPPercent()));
        if (this.mHP2AttackSpeed > 0L)
        {
            this.ExcuteAttributes("AttackSpeed%", this.mHP2AttackSpeed);
        }
        if (this.mHP2Miss > 0L)
        {
            this.ExcuteAttributes("MissRate%", -this.mHP2Miss);
        }
        this.mHP2Miss = (long) ((this.attribute.HP2Miss.Value * 10000f) * (1f - this.GetHPPercent()));
        if (this.mHP2Miss > 0L)
        {
            this.ExcuteAttributes("MissRate%", this.mHP2Miss);
        }
        return num2;
    }

    public void DeInit()
    {
        this.m_Entity.OnCrit = (Action<long>) Delegate.Remove(this.m_Entity.OnCrit, new Action<long>(this.OnCritEvent));
        Updater.RemoveUpdate("EntityData_Update.RebornUpdate", new Action<float>(this.OnRebornUpdate));
    }

    public void DeinitExp()
    {
        this.exp_data.Deinit();
    }

    private void DoDeadCommand(EntityBase entity)
    {
        if (entity.OnMonsterDeadAction != null)
        {
            entity.OnMonsterDeadAction(this.m_Entity);
        }
        if (entity.IsSelf)
        {
            GameLogic.Hold.BattleData.Challenge_MonsterDead();
        }
        switch (entity)
        {
            case (EntityBabyBase _):
            {
                EntityBase parent = (entity as EntityBabyBase).GetParent();
                if ((parent != null) && (parent.OnMonsterDeadAction != null))
                {
                    parent.OnMonsterDeadAction(this.m_Entity);
                }
                break;
            }
            default:
                if (entity is EntityPartBodyBase)
                {
                    EntityBase parent = (entity as EntityPartBodyBase).GetParent();
                    if ((parent != null) && (parent.OnMonsterDeadAction != null))
                    {
                        parent.OnMonsterDeadAction(this.m_Entity);
                    }
                }
                break;
        }
        if (entity.OnKillAction != null)
        {
            entity.OnKillAction(this.m_Entity, this.m_Entity.HittedDirection);
        }
        if (entity is EntityBabyBase)
        {
            EntityBase parent = (entity as EntityBabyBase).GetParent();
            if ((parent != null) && (parent.OnKillAction != null))
            {
                parent.OnKillAction(this.m_Entity, this.m_Entity.HittedDirection);
            }
        }
        else if (entity is EntityPartBodyBase)
        {
            EntityBase parent = (entity as EntityPartBodyBase).GetParent();
            if ((parent != null) && (parent.OnKillAction != null))
            {
                parent.OnKillAction(this.m_Entity, this.m_Entity.HittedDirection);
            }
        }
    }

    public void ExcuteAttributes(string str)
    {
        Goods_goods.GoodData goodData = Goods_goods.GetGoodData(str);
        this.ExcuteAttributes(goodData.goodType, goodData.value);
    }

    public void ExcuteAttributes(Goods_goods.GoodData data)
    {
        this.ExcuteAttributes(data.goodType, data.value);
    }

    public void ExcuteAttributes(string name, long value)
    {
        if ((name.Length > "Baby:".Length) && (name.Substring(0, "Baby:".Length) == "Baby:"))
        {
            if (name.Contains("%"))
            {
                value /= 100L;
            }
            this.ExcuteBabyAttributes(name.Substring("Baby:".Length, name.Length - "Baby:".Length), value);
        }
        else if ((name.Length <= "EquipBaby:".Length) || (name.Substring(0, "EquipBaby:".Length) != "EquipBaby:"))
        {
            if (((name.Length > "LevelUp:".Length) && (name.Substring(0, "LevelUp:".Length) == "LevelUp:")) && (this.m_Entity is EntityHero))
            {
                (this.m_Entity as EntityHero).ExcuteLevelUpAttributes(name.Substring("LevelUp:".Length, name.Length - "LevelUp:".Length), value);
            }
            else
            {
                bool flag = this.attribute.Excute(name, value);
                if (!flag && (value > 0L))
                {
                    flag = GameLogic.SelfAttribute.Excute(name, value);
                }
                if (name != null)
                {
                    if (name != "BodyScale%")
                    {
                        if (name == "AttackSpeed%")
                        {
                            if (this.m_Entity.m_AniCtrl == null)
                            {
                                Debugger.Log("m_Entity.m_AniCtrl is null");
                            }
                            else if (this.m_Entity.mAniCtrlBase == null)
                            {
                                Debugger.Log("m_Entity.m_AniCtrl。mAniCtrlBase is null");
                            }
                            this.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackPrev", ((float) value) / 10000f);
                            this.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", ((float) value) / 10000f);
                        }
                        else if (name == "AttackSpeed%_Buff")
                        {
                            this.m_Entity.mAniCtrlBase.UpdateWeaponSpeed(((float) value) / 10000f);
                        }
                        else if (name == "BabyCountAttack%")
                        {
                            int count = this.mBabies.Count;
                            if (count > 0)
                            {
                                this.ExcuteAttributes("Attack%", value * count);
                            }
                        }
                        else if (name == "BabyCountAttackSpeed%")
                        {
                            int count = this.mBabies.Count;
                            if (count > 0)
                            {
                                this.ExcuteAttributes("AttackSpeed%", value * count);
                            }
                        }
                    }
                    else
                    {
                        this.UpdateBodyScale();
                    }
                }
                if (!flag && (name != null))
                {
                    if (<>f__switch$map4 == null)
                    {
                        Dictionary<string, int> dictionary = new Dictionary<string, int>(14) {
                            { 
                                "Exp",
                                0
                            },
                            { 
                                "Gold",
                                1
                            },
                            { 
                                "HPRecoverFixed",
                                2
                            },
                            { 
                                "HPRecoverFixed%",
                                3
                            },
                            { 
                                "HPRecover",
                                4
                            },
                            { 
                                "HPRecover%",
                                5
                            },
                            { 
                                "HPRecoverBase%",
                                6
                            },
                            { 
                                "AllSpeed%",
                                7
                            },
                            { 
                                "BulletSpeed%",
                                8
                            },
                            { 
                                "MaxLevel",
                                9
                            },
                            { 
                                "BabyArgs",
                                10
                            },
                            { 
                                "Invincible",
                                11
                            },
                            { 
                                "AttackParentAttack%",
                                12
                            },
                            { 
                                "BodyHitParentAttack%",
                                13
                            }
                        };
                        <>f__switch$map4 = dictionary;
                    }
                    if (<>f__switch$map4.TryGetValue(name, out int num3))
                    {
                        switch (num3)
                        {
                            case 0:
                            {
                                float num4 = value * (1f + GameLogic.SelfAttribute.InGameExp.Value);
                                if (GameLogic.Hold.BattleData.GetMode() == GameMode.eLevel)
                                {
                                    num4 *= LocalModelManager.Instance.Stage_Level_stagechapter.GetScoreRate();
                                }
                                GameLogic.Hold.BattleData.AddGold(num4);
                                float exp = value * (1f + this.attribute.ExpGet.Value);
                                this.AddExp(exp);
                                break;
                            }
                            case 2:
                                GameLogic.Send_Recover(this.m_Entity, value);
                                break;

                            case 3:
                            {
                                long num7 = MathDxx.CeilToInt(((float) (this.attribute.GetHPBase() * value)) / 10000f);
                                GameLogic.Send_Recover(this.m_Entity, num7);
                                break;
                            }
                            case 4:
                                this.Attribute_HP(value);
                                break;

                            case 5:
                                this.Attribute_HPPercent(value);
                                break;

                            case 6:
                                this.Attribute_HPBasePercent(value);
                                break;

                            case 7:
                                this.Modify_AllSpeed(value);
                                break;

                            case 8:
                                this.Modify_BulletSpeed((float) value);
                                break;

                            case 9:
                                this.MaxLevel += (int) value;
                                break;

                            case 10:
                                this.m_Entity.SetBabyArgs(value);
                                break;

                            case 11:
                                this.Modify_Invincible(value > 0L);
                                break;

                            case 12:
                                if (this.m_Entity is EntityCallBase)
                                {
                                    EntityCallBase entity = this.m_Entity as EntityCallBase;
                                    if (entity != null)
                                    {
                                        EntityBase parent = entity.GetParent();
                                        if (parent != null)
                                        {
                                            long count = (long) (((float) (parent.m_EntityData.attribute.AttackValue.Value * value)) / 10000f);
                                            this.attribute.AttackValue.InitValueCount(count);
                                        }
                                    }
                                }
                                break;

                            case 13:
                                if (this.m_Entity is EntityCallBase)
                                {
                                    EntityCallBase entity = this.m_Entity as EntityCallBase;
                                    if (entity != null)
                                    {
                                        EntityBase parent = entity.GetParent();
                                        if (parent != null)
                                        {
                                            long valueCount = parent.m_EntityData.attribute.AttackValue.ValueCount;
                                            this.attribute.BodyHit.InitValueCount((long) (((float) (valueCount * value)) / 10000f));
                                        }
                                    }
                                }
                                break;
                        }
                    }
                }
            }
        }
    }

    public void ExcuteBabyAttributes(string name, long value)
    {
        object[] args = new object[] { name, value };
        this.AddBabyAttribute(Utils.FormatString("{0} + {1}", args));
    }

    public void ExcuteBuffs(EntityBase source, int buffid, string name, float value)
    {
        value = MathDxx.CeilBig(value);
        int num = (int) this.GetBuffValueInternal(source, name, value);
        GameLogic.SendHit_Buff(this.m_Entity, source, (long) num, GameLogic.GetElement(name), buffid);
    }

    public void ExcuteHitAdd()
    {
        if (this.attribute.HitVampireResult > 0L)
        {
            GameLogic.Send_Recover(this.m_Entity, this.attribute.HitVampireResult);
        }
    }

    public void ExcuteKillAdd()
    {
        if (this.attribute.KillVampireResult > 0L)
        {
            GameLogic.Send_Recover(this.m_Entity, this.attribute.KillVampireResult);
        }
    }

    public long GetAttack(int attack)
    {
        float num = 1f;
        num *= 1f + (this.HP2AttackRatio * (1f - this.GetHPPercent()));
        num *= this.attribute.AttackModify.Value;
        return (long) (this.GetAttackBase(attack) * num);
    }

    public long GetAttackBase() => 
        this.GetAttackBase(0);

    public long GetAttackBase(int attack)
    {
        float num = 0f;
        if (this.m_Entity.IsSelf)
        {
            this.attribute.AttackValue.UpdateValueCount((long) attack);
            num = this.attribute.AttackValue.Value;
            this.attribute.AttackValue.UpdateValueCount((long) -attack);
        }
        else
        {
            this.attribute.AttackValue.UpdateValueCount((long) attack);
            num = this.attribute.AttackValue.Value;
            this.attribute.AttackValue.UpdateValueCount((long) -attack);
        }
        num *= 1f + this.attribute.Attack_Value.Value;
        return (long) num;
    }

    public bool GetBabyResistBullet() => 
        (this.mBabyResistBulletCount > 0);

    public int GetBodyHit() => 
        ((int) this.attribute.BodyHit.Value);

    private float GetBuffBulletValue(EntityBase source, BulletBase bullet, float value)
    {
        if (bullet.m_Data.DebuffID != 0)
        {
            string attribute = LocalModelManager.Instance.Buff_alone.GetBeanById(bullet.m_Data.DebuffID).Attribute;
            return this.GetBuffValueInternal(source, attribute, value);
        }
        return value;
    }

    private float GetBuffValueInternal(EntityBase source, string name, float value)
    {
        if (name != null)
        {
            if (name != "Att_Fire")
            {
                if (name != "Att_Thunder")
                {
                    if (name == "Att_Ice")
                    {
                        return value;
                    }
                    if (name != "Att_Poison")
                    {
                        return value;
                    }
                    value += this.attribute.Att_Poison_Resist.Value;
                    value *= 1f - this.attribute.Att_Poison_ResistPercent.Value;
                }
                return value;
            }
            value += this.attribute.Att_Fire_Resist.Value;
            value *= 1f - this.attribute.Att_Fire_ResistPercent.Value;
        }
        return value;
    }

    public bool GetBulletLine() => 
        (this.mBulletLine > 0);

    public bool GetBulletScale() => 
        (this.mBulletScaleCount > 0);

    public float GetBulletSpeedRatio(BulletBase bullet)
    {
        this.mBulletSpeed = 1f;
        if (this.mBulletSpeed1Range > 0f)
        {
            Vector3 vector = bullet.transform.position - this.m_Entity.position;
            if (vector.magnitude < this.mBulletSpeed1Range)
            {
                this.mBulletSpeed *= this.mBulletSpeed1Ratio;
            }
        }
        if ((this.mBulletSpeedHittedCount > 0) && ((Updater.AliveTime - this.m_Entity.HittedLastTime) < this.mBulletSpeedHittedTime))
        {
            this.mBulletSpeed *= this.mBulletSpeedHitted;
        }
        return this.mBulletSpeed;
    }

    public bool GetCanDizzy()
    {
        if ((Updater.AliveTime - this.mDizzyTime) >= this.attribute.Monster_DizzyDelay.Value)
        {
            this.mDizzyTime = Updater.AliveTime;
            return true;
        }
        return false;
    }

    public bool GetCanShieldCount()
    {
        if (this.Shield_CurrentCount > 0L)
        {
            this.Shield_CurrentCount -= 1L;
            this.UpdateShieldCount();
            return true;
        }
        return false;
    }

    public bool GetCanTrapHit()
    {
        if ((Updater.AliveTime - this.mTrapHitTime) > 0.5f)
        {
            this.mTrapHitTime = Updater.AliveTime;
            return true;
        }
        return false;
    }

    public float GetCurrentExp() => 
        this.Exp;

    public int GetElementCount()
    {
        int num = 0;
        Dictionary<EElementType, BuffAttrData>.Enumerator enumerator = this.mBuffAttrList.GetEnumerator();
        while (enumerator.MoveNext())
        {
            KeyValuePair<EElementType, BuffAttrData> current = enumerator.Current;
            if (current.Value.count > 0)
            {
                num++;
            }
        }
        return num;
    }

    public bool GetFrontShield() => 
        (this.mFrontShieldCount > 0);

    public bool GetHeadShot()
    {
        if (this.m_Entity.Type != EntityType.Soldier)
        {
            return false;
        }
        if (GameLogic.Self == null)
        {
            return false;
        }
        if (!this.bInitHeadShot)
        {
            this.bInitHeadShot = true;
            this.bHeadShot = GameLogic.Random((float) 0f, (float) 1f) < GameLogic.Self.m_EntityData.attribute.HeadShot.Value;
        }
        return this.bHeadShot;
    }

    private EElementType GetHeadType()
    {
        EElementType eNone = EElementType.eNone;
        int num = 0;
        int num2 = 0;
        Dictionary<EElementType, BuffAttrData>.Enumerator enumerator = this.mBuffAttrList.GetEnumerator();
        while (enumerator.MoveNext())
        {
            KeyValuePair<EElementType, BuffAttrData> current = enumerator.Current;
            EElementType key = current.Key;
            ElementDataClass class2 = ElementData[key];
            int headPriority = 0;
            KeyValuePair<EElementType, BuffAttrData> pair2 = enumerator.Current;
            if (pair2.Value.count > 0)
            {
                headPriority = class2.HeadPriority;
                if ((headPriority > num2) && (class2.HeadPath != string.Empty))
                {
                    eNone = key;
                    num2 = headPriority;
                }
                if (key == EElementType.eFire)
                {
                    num |= 1;
                }
                if (key == EElementType.eThunder)
                {
                    num |= 2;
                }
            }
        }
        if (num == 3)
        {
            Debug.Log("雷火");
            eNone = EElementType.eThunderFire;
        }
        return eNone;
    }

    public float GetHP2HPAddPercent() => 
        (this.attribute.HP2HPAddPercent.Value * (1f - this.GetHPPercent()));

    public float GetHPPercent()
    {
        if (this.MaxHP == 0L)
        {
            return 1f;
        }
        return (((float) this.CurrentHP) / ((float) this.MaxHP));
    }

    public HitStruct GetHurt(HitStruct otherhs)
    {
        float num = otherhs.before_hit;
        if (otherhs.sourcetype == HitSourceType.eBullet)
        {
            if (otherhs.source != null)
            {
                float num2 = 1f;
                float num3 = otherhs.source.m_EntityData.attribute.AttackModify.Value;
                if (this.m_Entity.GetFlying())
                {
                    num -= otherhs.source.m_EntityData.attribute.HitToFly.Value * num3;
                    num2 *= 1f + otherhs.source.m_EntityData.attribute.HitToFlyPercent.Value;
                }
                else
                {
                    num -= otherhs.source.m_EntityData.attribute.HitToGround.Value * num3;
                    num2 *= 1f + otherhs.source.m_EntityData.attribute.HitToGroundPercent.Value;
                }
                if (this.m_Entity.m_Data.Attackrangetype == 1)
                {
                    num -= otherhs.source.m_EntityData.attribute.HitToNear.Value * num3;
                    num2 *= 1f + otherhs.source.m_EntityData.attribute.HitToNearPercent.Value;
                }
                else if (this.m_Entity.m_Data.Attackrangetype == 2)
                {
                    num -= otherhs.source.m_EntityData.attribute.HitToFar.Value * num3;
                    num2 *= 1f + otherhs.source.m_EntityData.attribute.HitToFarPercent.Value;
                }
                if (this.m_Entity.Type == EntityType.Boss)
                {
                    num -= otherhs.source.m_EntityData.attribute.HitToBoss.Value * num3;
                    num2 *= 1f + otherhs.source.m_EntityData.attribute.HitToBossPercent.Value;
                }
                num *= num2;
            }
            if (((otherhs.source != null) && (otherhs.bulletdata != null)) && (otherhs.bulletdata.bullet != null))
            {
                num = this.GetBuffBulletValue(otherhs.source, otherhs.bulletdata.bullet, num);
                if (otherhs.bulletdata.bullet.m_Data.DebuffID != 0)
                {
                    GameLogic.SendBuff(this.m_Entity, otherhs.source, otherhs.bulletdata.bullet.m_Data.DebuffID, otherhs.bulletdata.bullet.GetBuffArgs());
                }
            }
        }
        if ((otherhs.sourcetype == HitSourceType.eBullet) || (otherhs.sourcetype == HitSourceType.eBody))
        {
            if (this.GetMiss(otherhs.source))
            {
                otherhs.real_hit = 0L;
                otherhs.type = HitType.Miss;
                if (this.m_Entity.OnMiss != null)
                {
                    this.m_Entity.OnMiss();
                }
                return otherhs;
            }
            if (otherhs.sourcetype == HitSourceType.eBody)
            {
                num += this.attribute.BodyHittedCount.Value;
                num = MathDxx.Clamp(num, num, -1f) * (1f - this.attribute.BodyHitted.Value);
            }
            else
            {
                num *= 1f - this.attribute.BulletDef.Value;
            }
            if (GameLogic.Random((float) 0f, (float) 1f) < this.attribute.BlockRate.Value)
            {
                num *= 0.5f;
                otherhs.type = HitType.Block;
            }
        }
        if ((otherhs.sourcetype == HitSourceType.eBullet) || (otherhs.sourcetype == HitSourceType.eBody))
        {
            if (otherhs.source != null)
            {
                float critSuperRate = 0f;
                if ((otherhs.sourcetype == HitSourceType.eBullet) && (otherhs.bulletdata != null))
                {
                    critSuperRate = otherhs.bulletdata.bullet.mBulletTransmit.CritSuperRate;
                }
                else
                {
                    critSuperRate = otherhs.source.m_EntityData.attribute.CritSuperRate.Value;
                }
                if (GameLogic.Random((float) 0f, (float) 1f) < critSuperRate)
                {
                    otherhs.type = HitType.Crit;
                    num *= otherhs.source.m_EntityData.attribute.CritSuperValue.Value;
                }
                else
                {
                    if ((otherhs.sourcetype == HitSourceType.eBullet) && (otherhs.bulletdata != null))
                    {
                        critSuperRate = otherhs.bulletdata.bullet.mBulletTransmit.CritRate - this.attribute.CritDefRate.Value;
                    }
                    else
                    {
                        critSuperRate = otherhs.source.m_EntityData.attribute.CritRate.Value - this.attribute.CritDefRate.Value;
                    }
                    if (GameLogic.Random((float) 0f, (float) 1f) < critSuperRate)
                    {
                        otherhs.type = HitType.Crit;
                        num *= otherhs.source.m_EntityData.attribute.CritValue.Value;
                    }
                }
            }
            if ((otherhs.source != null) && (otherhs.source.Type == EntityType.Boss))
            {
                num *= 1f - this.attribute.HitFromBoss.Value;
            }
            if (this.m_Entity.Type == EntityType.Boss)
            {
                num *= 1f + this.attribute.HitToBoss.Value;
            }
            if ((otherhs.source != null) && otherhs.source.GetFlying())
            {
                num *= 1f - this.attribute.HitFromFly.Value;
            }
            if (this.m_Entity.IsSelf)
            {
                num *= 1f - Formula.GetDefence(this.attribute.DefenceValue.Value);
            }
            num *= 1f - this.attribute.Damage_Resistance.Value;
        }
        else if (otherhs.sourcetype == HitSourceType.eTrap)
        {
            num += this.attribute.TrapDefCount.Value;
            num = MathDxx.Clamp(num, num, -1f) * (1f - this.attribute.TrapDef.Value);
        }
        otherhs.real_hit = MathDxx.CeilBig(num);
        if (otherhs.real_hit > -1L)
        {
            otherhs.real_hit = -1L;
        }
        return otherhs;
    }

    public bool GetInvincible() => 
        (this.InvincibleCount > 0);

    public int GetLevel() => 
        this.Level;

    public bool GetLight45() => 
        (this.mLight45 > 0);

    private bool GetMiss(EntityBase source)
    {
        if (source == null)
        {
            return false;
        }
        float num = source.m_EntityData.attribute.HitRate.Value - this.attribute.MissRate.Value;
        num = MathDxx.Clamp(num, 0.05f, 1f);
        return (GameLogic.Random((float) 0f, (float) 1f) >= num);
    }

    public bool GetMissHP() => 
        (this.MissHP_Count > 0);

    public bool GetOnlyDemon() => 
        (this.mOnlyDemonCount > 0);

    public bool GetPlayHittedSound()
    {
        if ((Updater.AliveTime - this.hittedSoundTime) > 0.2f)
        {
            this.hittedSoundTime = Updater.AliveTime;
            return true;
        }
        return false;
    }

    public long GetShieldHitValue(long value)
    {
        if (this.Shield_CurrentHitValue >= value)
        {
            this.Shield_CurrentHitValue -= value;
            this.UpdateShieldValue();
            return value;
        }
        if (this.Shield_CurrentHitValue > 0L)
        {
            long num = this.Shield_CurrentHitValue;
            this.Shield_CurrentHitValue = 0L;
            this.UpdateShieldValue();
            return num;
        }
        return 0L;
    }

    public float GetSpeed() => 
        (((float) this.attribute.MoveSpeed.Value) / 100f);

    private EElementType GetTrailType()
    {
        EElementType eNone = EElementType.eNone;
        int num = 0;
        Dictionary<EElementType, BuffAttrData>.Enumerator enumerator = this.mBuffAttrList.GetEnumerator();
        while (enumerator.MoveNext())
        {
            KeyValuePair<EElementType, BuffAttrData> current = enumerator.Current;
            EElementType key = current.Key;
            ElementDataClass class2 = ElementData[key];
            int trailPriority = 0;
            KeyValuePair<EElementType, BuffAttrData> pair2 = enumerator.Current;
            if (pair2.Value.count > 0)
            {
                trailPriority = class2.TrailPriority;
                if ((trailPriority > num) && (class2.TrailPath != string.Empty))
                {
                    eNone = key;
                    num = trailPriority;
                }
            }
        }
        return eNone;
    }

    public void Init(EntityBase entity, int CharID)
    {
        this.m_Entity = entity;
        this.CharID = CharID;
        this.mDeadRecover = 1;
        this.InitAttribute();
        this.MaxHP = this.attribute.HPValue.Value;
        this.CurrentHP = this.MaxHP;
        this.MaxLevel = 11;
        if (GameLogic.Hold.BattleData.GetMode() == GameMode.eMatchDefenceTime)
        {
            this.MaxLevel = 20;
        }
        this.attribute.Reborn_Refresh_Count(LocalSave.Instance.BattleIn_GetRebornSkill());
        this.m_Entity.OnCrit = (Action<long>) Delegate.Combine(this.m_Entity.OnCrit, new Action<long>(this.OnCritEvent));
        this.m_Entity.OnMonsterDeadAction = (Action<EntityBase>) Delegate.Combine(this.m_Entity.OnMonsterDeadAction, new Action<EntityBase>(this.OnMonsterDead));
        this.m_Entity.Event_OnAttack = (Action) Delegate.Combine(this.m_Entity.Event_OnAttack, new Action(this.OnAttackCreate));
        this.m_Entity.OnMoveEvent = (Action<bool>) Delegate.Combine(this.m_Entity.OnMoveEvent, new Action<bool>(this.OnMoveEvent));
        this.OnMoveEvent(false);
    }

    public void InitAfter()
    {
        this.attribute.Shield_ValueAction = (Action<long>) Delegate.Combine(this.attribute.Shield_ValueAction, new Action<long>(this.UpdateShieldValueChange));
        this.Shield_CurrentHitValue = this.attribute.Shield.Value;
        this.UpdateShieldValue();
    }

    public void InitAttribute()
    {
        if (this.m_Entity.IsSelf)
        {
            this.attribute = new EntityAttributeBase();
            GameLogic.SelfAttribute.attribute.AttributeTo(this.attribute);
            GameLogic.SelfAttribute.Attribute2LevelUp(this);
        }
        else
        {
            this.attribute = new EntityAttributeBase(this.m_Entity.m_Data.CharID);
        }
        this.attribute.OnHPUpdate = new Action<long>(this.OnHPUpdate);
    }

    public void InitExp()
    {
        this.Exp = 0f;
        this.explist.Clear();
        IEnumerator<Exp_exp> enumerator = LocalModelManager.Instance.Exp_exp.GetAllBeans().GetEnumerator();
        while (enumerator.MoveNext())
        {
            this.explist.Add(enumerator.Current.LevelID, (float) enumerator.Current.Exp);
        }
        this.UpdateExp();
    }

    public bool IsDizzy() => 
        (this.DizzyCount > 0);

    public bool IsFlyStone() => 
        (this.mFlyStoneCount > 0);

    public bool IsFlyWater() => 
        (this.mFlyWaterCount > 0);

    public bool IsMaxLevel() => 
        (this.Level >= this.MaxLevel);

    public void Modify_AllSpeed(long value)
    {
        this.m_Entity.mAniCtrlBase.SetAllSpeed(((float) value) / 10000f);
        this.attribute.MoveSpeed.UpdateValuePercent(value);
    }

    public void Modify_BabyResistBullet(int count)
    {
        this.mBabyResistBulletCount += count;
        if ((this.mBabyResistBulletCount == 0) && (count < 0))
        {
            this.BabyResistBullet(false);
        }
        else if ((this.mBabyResistBulletCount == count) && (count > 0))
        {
            this.BabyResistBullet(true);
        }
    }

    public void Modify_BulletLineCount(int count)
    {
        this.mBulletLine += count;
    }

    public void Modify_BulletScale(int count)
    {
        this.mBulletScaleCount += count;
    }

    public void Modify_BulletSpeed(float value)
    {
        this.BulletSpeed += value;
    }

    public void Modify_BulletSpeedHitted(int value, float ratio, float time)
    {
        this.mBulletSpeedHittedCount += value;
        this.mBulletSpeedHitted += ratio;
        this.mBulletSpeedHittedTime += time;
    }

    public void Modify_BulletSpeedRatio(float value, float range)
    {
        this.mBulletSpeed1Ratio = value;
        this.mBulletSpeed1Range = range;
    }

    public void Modify_BulletThroughCount(int count)
    {
        this.mBulletThroughCount += count;
    }

    public void Modify_ButtetSputter(int count)
    {
        this.mBulletSputter += count;
    }

    public void Modify_FlyStone(int count)
    {
        this.mFlyStoneCount += count;
        this.m_Entity.SetFlyStone(this.IsFlyStone());
    }

    public void Modify_FlyWater(int count)
    {
        this.mFlyWaterCount += count;
        this.m_Entity.SetFlyWater(this.IsFlyWater());
    }

    public void Modify_FrontShield(int count)
    {
        this.mFrontShieldCount += count;
    }

    public void Modify_HitCreate2(int count, float percent)
    {
        this.mHitCreate2 += count;
        this.mHitCreate2Percent = percent;
    }

    public void Modify_HittedInterval(float value)
    {
        this.HittedInterval += value;
    }

    public void Modify_HP2Attack(float value)
    {
        this.mHP2AttackRatio += value;
    }

    public void Modify_Invincible(bool value)
    {
        if (value)
        {
            this.InvincibleCount++;
        }
        else
        {
            this.InvincibleCount--;
        }
    }

    public void Modify_Light45(int count)
    {
        this.mLight45 += count;
    }

    public void Modify_MissHP(bool value)
    {
        if (value)
        {
            this.MissHP_Count++;
        }
        else
        {
            this.MissHP_Count--;
        }
        if (this.m_Entity.m_HPSlider != null)
        {
            this.m_Entity.m_HPSlider.ShowHP(this.MissHP_Count <= 0);
        }
    }

    public void Modify_OnlyDemon(int count)
    {
        this.mOnlyDemonCount += count;
    }

    public void Modify_ThroughEnemy(int count, float ratio)
    {
        this.mThroughEnemy += count;
        if ((count < 0) && (this.mThroughEnemy == 0))
        {
            this.mThroughRatio = 1f;
        }
        else if ((count > 0) && (this.mThroughEnemy == 1))
        {
            this.mThroughRatio = ratio;
        }
    }

    public void Modify_TurnTableCount(int value)
    {
        this.TurnTableCount += value;
    }

    private void OnAttackCreate()
    {
        this.OnAttackMeteorite();
    }

    private void OnAttackMeteorite()
    {
        int allWeight = this.mAttackMeteorite.GetAllWeight();
        if ((allWeight != 0) && (GameLogic.Random((float) 0f, (float) 100f) <= allWeight))
        {
            AttackCallData random = this.mAttackMeteorite.GetRandom();
            Vector3 endpos = GameLogic.Release.MapCreatorCtrl.RandomPositionRange(this.m_Entity, 7);
            GameLogic.Release.Bullet.CreateSlopeBullet(this.m_Entity, random.id, this.m_Entity.position + new Vector3(0f, 21f, 0f), endpos).mBulletTransmit.SetAttack((long) MathDxx.CeilToInt(random.hitratio * this.m_Entity.m_EntityData.GetAttackBase()));
        }
    }

    private void OnCritEvent(long value)
    {
        if (this.attribute.CritAddHP.Value > 0f)
        {
            long num = (long) (this.attribute.CritAddHP.Value * this.MaxHP);
            GameLogic.Send_Recover(this.m_Entity, num);
        }
    }

    private void OnHPUpdate(long beforemaxhp)
    {
        this.MaxHP = this.attribute.HPValue.Value;
        if (this.MaxHP <= 0L)
        {
            this.MaxHP = 1L;
        }
        long num = this.MaxHP - beforemaxhp;
        if (num > 0L)
        {
            this.CurrentHP += num;
        }
        else if (this.CurrentHP > this.MaxHP)
        {
            this.CurrentHP = this.MaxHP;
        }
        if (this.m_Entity.OnMaxHpUpdate != null)
        {
            this.m_Entity.OnMaxHpUpdate(beforemaxhp, this.MaxHP);
        }
    }

    private void OnMonsterDead(EntityBase entity)
    {
        this.OnMonsterDeadCall(entity);
    }

    private void OnMonsterDeadCall(EntityBase entity)
    {
        int allWeight = this.mCallWeight.GetAllWeight();
        if (GameLogic.Random(0, 100) < allWeight)
        {
            DeadCallData random = this.mCallWeight.GetRandom();
            if (random.OnDead != null)
            {
                random.OnDead(entity);
            }
        }
    }

    private void OnMoveEvent(bool value)
    {
        if (this.mStaticReduce == null)
        {
            StaticAddData data = new StaticAddData {
                goodType = "DamageResist%",
                value = this.attribute.StaticReducePercent.ValuePercent
            };
            this.mStaticReduce = data;
        }
        this.mStaticReduce.Update(this.m_Entity, value, this.attribute.StaticReducePercent.ValuePercent);
    }

    private void OnRebornUpdate(float delta)
    {
        if ((Updater.unscaleAliveTime - this.mRebornStartTime) < 1.5f)
        {
            Time.timeScale = this.mRebornCurve.Evaluate((Updater.unscaleAliveTime - this.mRebornStartTime) / 1.5f);
        }
        else
        {
            Time.timeScale = 1f;
            Updater.RemoveUpdate("EntityData_Update.RebornUpdate", new Action<float>(this.OnRebornUpdate));
        }
    }

    private void OnShieldObjUpdate()
    {
        if ((this.Shield_CurrentHitValue > 0L) && (this.mShieldObj == null))
        {
            this.mShieldObj = GameLogic.EffectGet("Effect/Battle/Shield");
            this.mShieldObj.transform.SetParent(this.m_Entity.m_Body.EffectMask.transform);
            this.mShieldObj.transform.localPosition = Vector3.zero;
            this.mShieldObj.transform.localRotation = Quaternion.identity;
            this.mShieldObj.transform.localScale = Vector3.one;
        }
        else if ((this.Shield_CurrentHitValue == 0L) && (this.mShieldObj != null))
        {
            GameLogic.EffectCache(this.mShieldObj);
            this.mShieldObj = null;
        }
    }

    public void Reborn()
    {
        this.CurrentHP = this.MaxHP;
    }

    private void RebornUpdate()
    {
        this.m_Entity.PlayEffect(0x2f4d6c);
        this.mRebornCurve = LocalModelManager.Instance.Curve_curve.GetCurve(0x186b2);
        this.mRebornStartTime = Updater.unscaleAliveTime;
        Updater.AddUpdate("EntityData_Update.RebornUpdate", new Action<float>(this.OnRebornUpdate), true);
    }

    public void RemoveBaby(EntityBabyBase entity)
    {
        this.mBabies.Remove(entity);
    }

    public void RemoveBabyAttribute(string value)
    {
        this.mBabyAttributes.Remove(value);
        this.BabyUpdateAttributes();
    }

    public void RemoveElement(EElementType type)
    {
        BuffAttrData local1 = this.mBuffAttrList[type];
        local1.count--;
        this.ArrowTrailType = this.GetTrailType();
        this.ArrowHeadType = this.GetHeadType();
    }

    public void RemoveShieldCountAction(Action<long> action)
    {
        this.m_Entity.Shield_CountAction = (Action<long>) Delegate.Remove(this.m_Entity.Shield_CountAction, action);
    }

    public void ResetShieldCount()
    {
        this.Shield_CurrentCount = this.Shield_Count;
        this.UpdateShieldCount();
    }

    public void ResetShieldHitValue()
    {
        this.Shield_CurrentHitValue = this.attribute.Shield.Value;
        this.UpdateShieldValue();
    }

    public void SetCurrentExpLevel(float exp, int level)
    {
        this.Exp = exp;
        this.Level = level;
        this.UpdateExp();
    }

    public void UpdateBodyScale()
    {
        this.m_Entity.SetBodyScale(this.attribute.BodyScale.Value);
    }

    public void UpdateDizzy(int count)
    {
        if ((this.DizzyCount == 0) && (count == 1))
        {
            this.m_Entity.m_AniCtrl.SendEvent("Dizzy", false);
            if (this.m_Entity.OnDizzy != null)
            {
                this.m_Entity.OnDizzy(true);
            }
        }
        else if ((this.DizzyCount == 1) && (count == -1))
        {
            this.m_Entity.mAniCtrlBase.DizzyEnd();
            if (this.m_Entity.OnDizzy != null)
            {
                this.m_Entity.OnDizzy(false);
            }
        }
        this.DizzyCount += count;
    }

    private void UpdateExp()
    {
        this.exp_data.Init(this.Level, this.Exp, this.explist);
        Facade.Instance.SendNotification("BATTLE_EXP_UP", this.exp_data);
    }

    private void UpdateShieldCount()
    {
        if (this.m_Entity.Shield_CountAction != null)
        {
            this.m_Entity.Shield_CountAction(this.Shield_CurrentCount);
        }
    }

    private void UpdateShieldValue()
    {
        if ((this.m_Entity != null) && (this.m_Entity.m_HPSlider != null))
        {
            this.m_Entity.m_HPSlider.UpdateHP();
        }
        this.OnShieldObjUpdate();
    }

    public void UpdateShieldValueChange(long change)
    {
        this.Shield_CurrentHitValue += change;
        this.UpdateShieldValue();
    }

    public void UseDeadRecover()
    {
        this.mDeadRecover--;
    }

    public int HitCreate2 =>
        this.mHitCreate2;

    public int BulletThroughCount =>
        this.mBulletThroughCount;

    public float HP2AttackRatio =>
        this.mHP2AttackRatio;

    public int ThroughEnemy =>
        this.mThroughEnemy;

    public float ThroughRatio =>
        this.mThroughRatio;

    public int BulletLineCount =>
        this.mBulletLine;

    public int BulletSputter =>
        this.mBulletSputter;

    public class BuffAttrData
    {
        public int count;
        public float attack = 1f;
        public float resistance;
    }

    private class MoveAddData
    {
        public string goodType;
        public long value;
        public bool bAdded;

        public void Update(EntityBase entity, bool move, long value)
        {
            if (this.value == value)
            {
                if (!move && this.bAdded)
                {
                    this.bAdded = false;
                    entity.m_EntityData.ExcuteAttributes(this.goodType, -value);
                }
                else if (move && !this.bAdded)
                {
                    this.bAdded = true;
                    entity.m_EntityData.ExcuteAttributes(this.goodType, value);
                }
            }
            else
            {
                if (this.bAdded)
                {
                    this.bAdded = false;
                    entity.m_EntityData.ExcuteAttributes(this.goodType, -this.value);
                }
                this.value = value;
                this.Update(entity, move, value);
            }
        }
    }

    private class StaticAddData
    {
        public string goodType;
        public long value;
        public bool bAdded;

        public void Update(EntityBase entity, bool move, long value)
        {
            if (this.value == value)
            {
                if (move && this.bAdded)
                {
                    this.bAdded = false;
                    entity.m_EntityData.ExcuteAttributes(this.goodType, -value);
                }
                else if (!move && !this.bAdded)
                {
                    this.bAdded = true;
                    entity.m_EntityData.ExcuteAttributes(this.goodType, value);
                }
            }
            else
            {
                if (this.bAdded)
                {
                    this.bAdded = false;
                    entity.m_EntityData.ExcuteAttributes(this.goodType, -this.value);
                }
                this.value = value;
                this.Update(entity, move, value);
            }
        }
    }
}

