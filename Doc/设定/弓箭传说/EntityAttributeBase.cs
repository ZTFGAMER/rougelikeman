using Dxx.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using TableTool;

public class EntityAttributeBase
{
    public Action<long> OnHPUpdate;
    public Action OnMoveSpeedUpdate;
    public Action<long> Shield_ValueAction;
    public Action<long> OnAttackUpdate;
    public ValueBase Bullet_Forward;
    public ValueBase Bullet_Backward;
    public ValueBase Bullet_Side;
    public ValueBase Bullet_ForSide;
    public ValueBase Bullet_Continue;
    public ValueBase HPValue;
    public ValueBase HPAdd;
    public ValueFloatBase HPAddPercent;
    public ValueBase AttackValue;
    public ValueBase DefenceValue;
    public ValueBase MoveSpeed;
    public ValueFloatBase Attack_Value;
    public ValueFloatBase Damage_Resistance;
    public ValueFloatBase HitRate;
    public ValueReduce MissRate;
    public ValueFloatBase CritRate;
    public ValueFloatBase CritDefRate;
    public ValueFloatBase BlockRate;
    public ValueFloatBase CritValue;
    public ValueFloatBase AttackSpeed;
    public ValueBase HitVampire;
    public ValueFloatBase HitVampirePercent;
    public ValueFloatBase HitVampireAddPercent;
    public long HitVampireResult;
    public ValueBase KillVampire;
    public ValueFloatBase KillVampirePercent;
    public ValueFloatBase KillVampireAddPercent;
    public long KillVampireResult;
    public ValueBase TrapDefCount;
    public ValueFloatBase TrapDef;
    public ValueFloatBase BulletDef;
    public ValueFloatBase HitFromFly;
    public ValueFloatBase HitToFlyPercent;
    public ValueBase HitToFly;
    public ValueBase HitToGround;
    public ValueFloatBase HitToGroundPercent;
    public ValueBase HitToNear;
    public ValueFloatBase HitToNearPercent;
    public ValueBase HitToFar;
    public ValueFloatBase HitToFarPercent;
    public ValueFloatBase HitFromBoss;
    public ValueFloatBase HitToBossPercent;
    public ValueBase HitToBoss;
    public ValueBase BodyHittedCount;
    public ValueFloatBase BodyHitted;
    public ValueFloatBase HeadShot;
    public ValueBase ReboundHit;
    public ValueFloatBase ReboundTargetPercent;
    public ValueMult AttackModify;
    public ValueBase ExtraSkill;
    public ValueFloatBase ExpGet;
    public ValueBase RebornCount;
    public ValueBase RebornHP;
    public ValueFloatBase RebornHPPercent;
    public ValueBase BodyHit;
    public ValueFloatBase HitBack;
    public ValueFloatBase BodyScale;
    public ValueBase RotateSpeed;
    public ValueRange ArrowEject;
    public ValueBase ArrowTrack;
    public ValueRange ReboundWall;
    public ValueFloatBase CritAddHP;
    public ValueFloatBase CritSuperRate;
    public ValueFloatBase CritSuperValue;
    public ValueFloatBase AngelR2Rate;
    public ValueFloatBase HP2AttackSpeed;
    public ValueFloatBase HP2HPAddPercent;
    public ValueFloatBase HP2Miss;
    public ValueFloatBase BabyCountAttack;
    public ValueFloatBase BabyCountAttackSpeed;
    public ValueFloatBase StaticReducePercent;
    public ValueBase KillBossShield;
    public ValueFloatBase KillBossShieldPercent;
    public ValueFloatBase KillMonsterLessHP;
    public ValueFloatBase KillMonsterLessHPRatio;
    public ValueBase DistanceAttackValueDis;
    public ValueFloatBase DistanceAttackValuePercent;
    public ValueFloatBase WeaponRoundBackAttackPercent;
    public ValueBase Shield;
    public ValueFloatBase EquipDrop;
    public ValueBase Att_Fire_Add;
    public ValueFloatBase Att_Fire_AddPercent;
    public ValueBase Att_Fire_Resist;
    public ValueFloatBase Att_Fire_ResistPercent;
    public ValueBase Att_Poison_Add;
    public ValueFloatBase Att_Poison_AddPercent;
    public ValueBase Att_Poison_Resist;
    public ValueFloatBase Att_Poison_ResistPercent;
    public ValueFloatBase Monster_ExpPercent;
    public ValueTime Monster_DizzyDelay;
    public ValueFloatBase Monster_HPDrop;
    public ValueFloatBase Monster_GoldDrop;
    [CompilerGenerated]
    private static Dictionary<string, int> <>f__switch$map3;

    public EntityAttributeBase()
    {
        this.Bullet_Forward = new ValueBase(1L);
        this.Bullet_Backward = new ValueBase();
        this.Bullet_Side = new ValueBase();
        this.Bullet_ForSide = new ValueBase();
        this.Bullet_Continue = new ValueBase(1L);
        this.HPValue = new ValueBase();
        this.HPAdd = new ValueBase();
        this.HPAddPercent = new ValueFloatBase();
        this.AttackValue = new ValueBase();
        this.DefenceValue = new ValueBase();
        this.MoveSpeed = new ValueBase();
        this.Attack_Value = new ValueFloatBase();
        this.Damage_Resistance = new ValueFloatBase();
        this.HitRate = new ValueFloatBase(0L, 0x2710L);
        this.MissRate = new ValueReduce();
        this.CritRate = new ValueFloatBase();
        this.CritDefRate = new ValueFloatBase();
        this.BlockRate = new ValueFloatBase();
        this.CritValue = new ValueFloatBase(0L, 0x4e20L);
        this.AttackSpeed = new ValueFloatBase();
        this.HitVampire = new ValueBase();
        this.HitVampirePercent = new ValueFloatBase();
        this.HitVampireAddPercent = new ValueFloatBase();
        this.KillVampire = new ValueBase();
        this.KillVampirePercent = new ValueFloatBase();
        this.KillVampireAddPercent = new ValueFloatBase();
        this.TrapDefCount = new ValueBase();
        this.TrapDef = new ValueFloatBase();
        this.BulletDef = new ValueFloatBase();
        this.HitFromFly = new ValueFloatBase();
        this.HitToFlyPercent = new ValueFloatBase();
        this.HitToFly = new ValueBase();
        this.HitToGround = new ValueBase();
        this.HitToGroundPercent = new ValueFloatBase();
        this.HitToNear = new ValueBase();
        this.HitToNearPercent = new ValueFloatBase();
        this.HitToFar = new ValueBase();
        this.HitToFarPercent = new ValueFloatBase();
        this.HitFromBoss = new ValueFloatBase();
        this.HitToBossPercent = new ValueFloatBase();
        this.HitToBoss = new ValueBase();
        this.BodyHittedCount = new ValueBase();
        this.BodyHitted = new ValueFloatBase();
        this.HeadShot = new ValueFloatBase();
        this.ReboundHit = new ValueBase();
        this.ReboundTargetPercent = new ValueFloatBase();
        this.AttackModify = new ValueMult(0x2710L);
        this.ExtraSkill = new ValueBase();
        this.ExpGet = new ValueFloatBase();
        this.RebornCount = new ValueBase();
        this.RebornHP = new ValueBase();
        this.RebornHPPercent = new ValueFloatBase();
        this.BodyHit = new ValueBase();
        this.HitBack = new ValueFloatBase();
        this.BodyScale = new ValueFloatBase(0L, 0x2710L);
        this.RotateSpeed = new ValueBase();
        this.ArrowEject = new ValueRange();
        this.ArrowTrack = new ValueBase();
        this.ReboundWall = new ValueRange();
        this.CritAddHP = new ValueFloatBase();
        this.CritSuperRate = new ValueFloatBase();
        this.CritSuperValue = new ValueFloatBase(0L, 0x2710L);
        this.AngelR2Rate = new ValueFloatBase();
        this.HP2AttackSpeed = new ValueFloatBase();
        this.HP2HPAddPercent = new ValueFloatBase();
        this.HP2Miss = new ValueFloatBase();
        this.BabyCountAttack = new ValueFloatBase();
        this.BabyCountAttackSpeed = new ValueFloatBase();
        this.StaticReducePercent = new ValueFloatBase();
        this.KillBossShield = new ValueBase();
        this.KillBossShieldPercent = new ValueFloatBase();
        this.KillMonsterLessHP = new ValueFloatBase();
        this.KillMonsterLessHPRatio = new ValueFloatBase();
        this.DistanceAttackValueDis = new ValueBase();
        this.DistanceAttackValuePercent = new ValueFloatBase();
        this.WeaponRoundBackAttackPercent = new ValueFloatBase();
        this.Shield = new ValueBase();
        this.EquipDrop = new ValueFloatBase();
        this.Att_Fire_Add = new ValueBase();
        this.Att_Fire_AddPercent = new ValueFloatBase();
        this.Att_Fire_Resist = new ValueBase();
        this.Att_Fire_ResistPercent = new ValueFloatBase();
        this.Att_Poison_Add = new ValueBase();
        this.Att_Poison_AddPercent = new ValueFloatBase();
        this.Att_Poison_Resist = new ValueBase();
        this.Att_Poison_ResistPercent = new ValueFloatBase();
        this.Monster_ExpPercent = new ValueFloatBase();
        this.Monster_DizzyDelay = new ValueTime(0xbb8L);
        this.Monster_HPDrop = new ValueFloatBase();
        this.Monster_GoldDrop = new ValueFloatBase();
    }

    public EntityAttributeBase(int CharID)
    {
        this.Bullet_Forward = new ValueBase(1L);
        this.Bullet_Backward = new ValueBase();
        this.Bullet_Side = new ValueBase();
        this.Bullet_ForSide = new ValueBase();
        this.Bullet_Continue = new ValueBase(1L);
        this.HPValue = new ValueBase();
        this.HPAdd = new ValueBase();
        this.HPAddPercent = new ValueFloatBase();
        this.AttackValue = new ValueBase();
        this.DefenceValue = new ValueBase();
        this.MoveSpeed = new ValueBase();
        this.Attack_Value = new ValueFloatBase();
        this.Damage_Resistance = new ValueFloatBase();
        this.HitRate = new ValueFloatBase(0L, 0x2710L);
        this.MissRate = new ValueReduce();
        this.CritRate = new ValueFloatBase();
        this.CritDefRate = new ValueFloatBase();
        this.BlockRate = new ValueFloatBase();
        this.CritValue = new ValueFloatBase(0L, 0x4e20L);
        this.AttackSpeed = new ValueFloatBase();
        this.HitVampire = new ValueBase();
        this.HitVampirePercent = new ValueFloatBase();
        this.HitVampireAddPercent = new ValueFloatBase();
        this.KillVampire = new ValueBase();
        this.KillVampirePercent = new ValueFloatBase();
        this.KillVampireAddPercent = new ValueFloatBase();
        this.TrapDefCount = new ValueBase();
        this.TrapDef = new ValueFloatBase();
        this.BulletDef = new ValueFloatBase();
        this.HitFromFly = new ValueFloatBase();
        this.HitToFlyPercent = new ValueFloatBase();
        this.HitToFly = new ValueBase();
        this.HitToGround = new ValueBase();
        this.HitToGroundPercent = new ValueFloatBase();
        this.HitToNear = new ValueBase();
        this.HitToNearPercent = new ValueFloatBase();
        this.HitToFar = new ValueBase();
        this.HitToFarPercent = new ValueFloatBase();
        this.HitFromBoss = new ValueFloatBase();
        this.HitToBossPercent = new ValueFloatBase();
        this.HitToBoss = new ValueBase();
        this.BodyHittedCount = new ValueBase();
        this.BodyHitted = new ValueFloatBase();
        this.HeadShot = new ValueFloatBase();
        this.ReboundHit = new ValueBase();
        this.ReboundTargetPercent = new ValueFloatBase();
        this.AttackModify = new ValueMult(0x2710L);
        this.ExtraSkill = new ValueBase();
        this.ExpGet = new ValueFloatBase();
        this.RebornCount = new ValueBase();
        this.RebornHP = new ValueBase();
        this.RebornHPPercent = new ValueFloatBase();
        this.BodyHit = new ValueBase();
        this.HitBack = new ValueFloatBase();
        this.BodyScale = new ValueFloatBase(0L, 0x2710L);
        this.RotateSpeed = new ValueBase();
        this.ArrowEject = new ValueRange();
        this.ArrowTrack = new ValueBase();
        this.ReboundWall = new ValueRange();
        this.CritAddHP = new ValueFloatBase();
        this.CritSuperRate = new ValueFloatBase();
        this.CritSuperValue = new ValueFloatBase(0L, 0x2710L);
        this.AngelR2Rate = new ValueFloatBase();
        this.HP2AttackSpeed = new ValueFloatBase();
        this.HP2HPAddPercent = new ValueFloatBase();
        this.HP2Miss = new ValueFloatBase();
        this.BabyCountAttack = new ValueFloatBase();
        this.BabyCountAttackSpeed = new ValueFloatBase();
        this.StaticReducePercent = new ValueFloatBase();
        this.KillBossShield = new ValueBase();
        this.KillBossShieldPercent = new ValueFloatBase();
        this.KillMonsterLessHP = new ValueFloatBase();
        this.KillMonsterLessHPRatio = new ValueFloatBase();
        this.DistanceAttackValueDis = new ValueBase();
        this.DistanceAttackValuePercent = new ValueFloatBase();
        this.WeaponRoundBackAttackPercent = new ValueFloatBase();
        this.Shield = new ValueBase();
        this.EquipDrop = new ValueFloatBase();
        this.Att_Fire_Add = new ValueBase();
        this.Att_Fire_AddPercent = new ValueFloatBase();
        this.Att_Fire_Resist = new ValueBase();
        this.Att_Fire_ResistPercent = new ValueFloatBase();
        this.Att_Poison_Add = new ValueBase();
        this.Att_Poison_AddPercent = new ValueFloatBase();
        this.Att_Poison_Resist = new ValueBase();
        this.Att_Poison_ResistPercent = new ValueFloatBase();
        this.Monster_ExpPercent = new ValueFloatBase();
        this.Monster_DizzyDelay = new ValueTime(0xbb8L);
        this.Monster_HPDrop = new ValueFloatBase();
        this.Monster_GoldDrop = new ValueFloatBase();
        Character_Char beanById = LocalModelManager.Instance.Character_Char.GetBeanById(CharID);
        this.Excute("MoveSpeed", (long) beanById.Speed);
        this.Excute("HPMax", (long) beanById.HP);
        this.Excute("RotateSpeed", (long) beanById.RotateSpeed);
        this.Excute("BodyHit", (long) beanById.BodyAttack);
    }

    public void AttributeTo(EntityAttributeBase attribute)
    {
        attribute.HPValue.InitValueCount(this.HPValue.ValueLong);
        long count = (long) (this.HPAdd.ValueLong * (1f + this.HPAddPercent.Value));
        attribute.HPAdd.InitValueCount(count);
        attribute.AttackValue.InitValueCount(this.AttackValue.ValueLong);
        attribute.DefenceValue.InitValueCount(this.DefenceValue.ValueLong);
        attribute.MoveSpeed.InitValueCount(this.MoveSpeed.ValueLong);
        attribute.Attack_Value.InitValuePercent(this.Attack_Value.ValueLong);
        attribute.Damage_Resistance.InitValueCount(this.Damage_Resistance.ValueLong);
        attribute.HitRate.InitValuePercent(this.HitRate.ValueLong);
        attribute.MissRate.InitValue(this.MissRate.mList);
        attribute.CritRate.InitValuePercent(this.CritRate.ValueLong);
        attribute.CritDefRate.InitValuePercent(this.CritDefRate.ValueLong);
        attribute.BlockRate.InitValuePercent(this.BlockRate.ValueLong);
        attribute.CritValue.InitValuePercent(this.CritValue.ValueLong);
        attribute.AttackSpeed.InitValuePercent(this.AttackSpeed.ValueLong);
        attribute.HitVampire.InitValueCount(this.HitVampireResult);
        attribute.UpdateHitVampireResult();
        attribute.KillVampire.InitValueCount(this.KillVampireResult);
        attribute.UpdateKillVampireResult();
        attribute.TrapDefCount.InitValueCount(this.TrapDefCount.ValueLong);
        attribute.TrapDef.InitValueCount(this.TrapDef.ValueLong);
        attribute.BulletDef.InitValueCount(this.BulletDef.ValueLong);
        attribute.HitFromFly.InitValueCount(this.HitFromFly.ValueLong);
        attribute.HitToFlyPercent.InitValuePercent(this.HitToFlyPercent.ValueLong);
        attribute.HitToFly.InitValueCount(this.HitToFly.ValueLong);
        attribute.HitToGround.InitValueCount(this.HitToGround.ValueLong);
        attribute.HitToGroundPercent.InitValuePercent(this.HitToGroundPercent.ValueLong);
        attribute.HitToNear.InitValueCount(this.HitToNear.ValueLong);
        attribute.HitToNearPercent.InitValuePercent(this.HitToNearPercent.ValueLong);
        attribute.HitToFar.InitValueCount(this.HitToFar.ValueLong);
        attribute.HitToFarPercent.InitValuePercent(this.HitToFarPercent.ValueLong);
        attribute.HitFromBoss.InitValueCount(this.HitFromBoss.ValueLong);
        attribute.HitToBossPercent.InitValuePercent(this.HitToBossPercent.ValueLong);
        attribute.HitToBoss.InitValueCount(this.HitToBoss.ValueLong);
        attribute.BodyHittedCount.InitValueCount(this.BodyHittedCount.ValueLong);
        attribute.BodyHitted.InitValueCount(this.BodyHitted.ValueLong);
        attribute.HeadShot.InitValuePercent(this.HeadShot.ValueLong);
        attribute.ReboundHit.InitValueCount(this.ReboundHit.ValueLong);
        attribute.ReboundTargetPercent.InitValuePercent(this.ReboundTargetPercent.ValueLong);
        attribute.AttackModify.InitValue(this.AttackModify.ValueLong);
        attribute.ExtraSkill.InitValueCount(this.ExtraSkill.ValueLong);
        attribute.ExpGet.InitValuePercent(this.ExpGet.ValueLong);
        attribute.RebornCount.InitValueCount(this.RebornCount.ValueLong);
        attribute.BodyHit.InitValueCount(this.BodyHit.ValueLong);
        attribute.HitBack.InitValuePercent(this.HitBack.ValueLong);
        attribute.BodyScale.InitValuePercent(this.BodyScale.ValueLong);
        attribute.RotateSpeed.InitValueCount(this.RotateSpeed.ValueLong);
        attribute.ArrowEject.InitValue(this.ArrowEject);
        attribute.ArrowTrack.InitValueCount(this.ArrowTrack.ValueLong);
        attribute.ReboundWall.InitValue(this.ReboundWall);
        attribute.CritAddHP.InitValue(this.CritAddHP.ValueLong, this.CritAddHP.ValuePercent);
        attribute.CritSuperRate.InitValuePercent(this.CritSuperRate.ValueLong);
        attribute.CritSuperValue.InitValuePercent(this.CritSuperValue.ValueLong);
        attribute.AngelR2Rate.InitValuePercent(this.AngelR2Rate.ValueLong);
        attribute.HP2AttackSpeed.InitValuePercent(this.HP2AttackSpeed.ValueLong);
        attribute.HP2HPAddPercent.InitValuePercent(this.HP2HPAddPercent.ValueLong);
        attribute.HP2Miss.InitValuePercent(this.HP2Miss.ValueLong);
        attribute.BabyCountAttack.InitValuePercent(this.BabyCountAttack.ValueLong);
        attribute.BabyCountAttackSpeed.InitValuePercent(this.BabyCountAttackSpeed.ValueLong);
        attribute.StaticReducePercent.InitValuePercent(this.StaticReducePercent.ValueLong);
        attribute.KillMonsterLessHP.InitValuePercent(this.KillMonsterLessHP.ValueLong);
        attribute.KillMonsterLessHPRatio.InitValuePercent(this.KillMonsterLessHPRatio.ValueLong);
        attribute.Shield.InitValueCount(this.Shield.ValueLong);
        attribute.KillBossShield.InitValueCount(this.KillBossShield.ValueLong);
        attribute.KillBossShieldPercent.InitValuePercent(this.KillBossShieldPercent.ValueLong);
        attribute.EquipDrop.InitValuePercent(this.EquipDrop.ValueLong);
        attribute.Att_Fire_Add.InitValueCount(this.Att_Fire_Add.ValueLong);
        attribute.Att_Fire_AddPercent.InitValuePercent(this.Att_Fire_AddPercent.ValueLong);
        attribute.Att_Fire_Resist.InitValueCount(this.Att_Fire_Resist.ValueLong);
        attribute.Att_Fire_ResistPercent.InitValuePercent(this.Att_Fire_ResistPercent.ValueLong);
        attribute.Att_Poison_Add.InitValueCount(this.Att_Poison_Add.ValueLong);
        attribute.Att_Poison_AddPercent.InitValuePercent(this.Att_Poison_AddPercent.ValueLong);
        attribute.Att_Poison_Resist.InitValueCount(this.Att_Poison_Resist.ValueLong);
        attribute.Att_Poison_ResistPercent.InitValuePercent(this.Att_Poison_ResistPercent.ValueLong);
        attribute.Monster_ExpPercent.InitValuePercent(this.Monster_ExpPercent.ValueLong);
        attribute.Monster_HPDrop.InitValuePercent(this.Monster_HPDrop.ValueLong);
        attribute.Monster_ExpPercent.InitValuePercent(this.Monster_ExpPercent.ValueLong);
        attribute.Monster_GoldDrop.InitValuePercent(this.Monster_GoldDrop.ValueLong);
    }

    public void DebugValue()
    {
        Debugger.Log("生命值 : " + this.HPValue.Value);
        Debugger.Log("生命回复增加值：" + this.HPAdd.Value);
        Debugger.Log("生命回复% : " + this.HPAddPercent.Value);
        Debugger.Log("攻击力 : " + this.AttackValue.Value);
        Debugger.Log("防御力 : " + this.DefenceValue.Value);
        Debugger.Log("移动速度 : " + this.MoveSpeed.Value);
        Debugger.Log("伤害加成% : " + this.Attack_Value.Value);
        Debugger.Log("伤害减免% : " + this.Damage_Resistance.Value);
        Debugger.Log("命中率 : " + this.HitRate.Value);
        Debugger.Log("闪避率 : " + this.MissRate.Value);
        Debugger.Log("暴击率 : " + this.CritRate.Value);
        Debugger.Log("抗暴击率 : " + this.CritDefRate.Value);
        Debugger.Log("格挡率 : " + this.BlockRate.Value);
        Debugger.Log("暴击伤害% : " + this.CritValue.Value);
        Debugger.Log("攻击速度% : " + this.AttackSpeed.Value);
        Debugger.Log("命中吸血 : " + this.HitVampireResult);
        Debugger.Log("击杀吸血 : " + this.KillVampireResult);
        Debugger.Log("陷阱伤害减免% : " + this.TrapDef.Value);
        Debugger.Log("飞行单位伤害减免% : " + this.HitFromFly.Value);
        Debugger.Log("对飞行单位伤害% : " + this.HitToFlyPercent.Value);
        Debugger.Log("对飞行单位伤害数值 : " + this.HitToFly.Value);
        Debugger.Log("Boss伤害减免% : " + this.HitFromBoss.Value);
        Debugger.Log("对Boss伤害加成% : " + this.HitToBossPercent.Value);
        Debugger.Log("对Boss伤害加成数值 : " + this.HitToBoss.Value);
        Debugger.Log("碰撞减伤% : " + this.BodyHitted.Value);
        Debugger.Log("秒杀小怪% : " + this.HeadShot.Value);
        Debugger.Log("反弹伤害% : " + this.ReboundHit.Value);
        Debugger.Log("反弹伤害敌方% : " + this.ReboundTargetPercent.Value);
        Debugger.Log("攻击修正% : " + this.AttackModify.Value);
        Debugger.Log("额外技能 : " + this.ExtraSkill.Value);
        Debugger.Log("经验获取% : " + this.ExpGet.Value);
        Debugger.Log("复活次数 : " + this.RebornCount.Value);
        Debugger.Log("击退系数% : " + this.HitBack.Value);
        Debugger.Log("碰撞伤害 : " + this.BodyHit.Value);
        Debugger.Log("体积% : " + this.BodyScale.Value);
        Debugger.Log("转头速率 : " + this.RotateSpeed.Value);
        Debugger.Log("超级暴击率% : " + this.CritSuperRate.Value);
        Debugger.Log("超级暴击伤害% : " + this.CritSuperValue.Value);
    }

    public bool Excute(string str)
    {
        Goods_goods.GoodData goodData = Goods_goods.GetGoodData(str);
        return this.Excute(goodData);
    }

    public bool Excute(Goods_goods.GoodData data) => 
        this.Excute(data.goodType, data.value);

    public bool Excute(string type, long value)
    {
        bool flag = true;
        if (type != null)
        {
            if (<>f__switch$map3 == null)
            {
                Dictionary<string, int> dictionary = new Dictionary<string, int>(0x66) {
                    { 
                        "BulletForward",
                        0
                    },
                    { 
                        "BulletBackward",
                        1
                    },
                    { 
                        "BulletSide",
                        2
                    },
                    { 
                        "BulletForSide",
                        3
                    },
                    { 
                        "BulletContinue",
                        4
                    },
                    { 
                        "HPMax",
                        5
                    },
                    { 
                        "HPMax%",
                        6
                    },
                    { 
                        "HPAdd",
                        7
                    },
                    { 
                        "HPAdd%",
                        8
                    },
                    { 
                        "Attack",
                        9
                    },
                    { 
                        "Attack%",
                        10
                    },
                    { 
                        "Defence",
                        11
                    },
                    { 
                        "Defence%",
                        12
                    },
                    { 
                        "MoveSpeed",
                        13
                    },
                    { 
                        "MoveSpeed%",
                        14
                    },
                    { 
                        "AttackValue%",
                        15
                    },
                    { 
                        "DamageResist%",
                        0x10
                    },
                    { 
                        "HitRate%",
                        0x11
                    },
                    { 
                        "MissRate%",
                        0x12
                    },
                    { 
                        "CritRate%",
                        0x13
                    },
                    { 
                        "CritDefRate%",
                        20
                    },
                    { 
                        "BlockRate%",
                        0x15
                    },
                    { 
                        "CritValue%",
                        0x16
                    },
                    { 
                        "AttackSpeed%",
                        0x17
                    },
                    { 
                        "HitVampire",
                        0x18
                    },
                    { 
                        "HitVampire%",
                        0x19
                    },
                    { 
                        "HitVampireAdd%",
                        0x1a
                    },
                    { 
                        "KillVampire",
                        0x1b
                    },
                    { 
                        "KillVampire%",
                        0x1c
                    },
                    { 
                        "KillVampireAdd%",
                        0x1d
                    },
                    { 
                        "TrapHittedReduce",
                        30
                    },
                    { 
                        "TrapHittedReduce%",
                        0x1f
                    },
                    { 
                        "BulletReduce%",
                        0x20
                    },
                    { 
                        "HitFromFly%",
                        0x21
                    },
                    { 
                        "HitToFly%",
                        0x22
                    },
                    { 
                        "HitToFly",
                        0x23
                    },
                    { 
                        "HitToGround",
                        0x24
                    },
                    { 
                        "HitToGround%",
                        0x25
                    },
                    { 
                        "HitToNear",
                        0x26
                    },
                    { 
                        "HitToNear%",
                        0x27
                    },
                    { 
                        "HitToFar",
                        40
                    },
                    { 
                        "HitToFar%",
                        0x29
                    },
                    { 
                        "HitFromBoss%",
                        0x2a
                    },
                    { 
                        "HitToBoss%",
                        0x2b
                    },
                    { 
                        "HitToBoss",
                        0x2c
                    },
                    { 
                        "BodyHittedReduce",
                        0x2d
                    },
                    { 
                        "BodyHittedReduce%",
                        0x2e
                    },
                    { 
                        "HeadShot%",
                        0x2f
                    },
                    { 
                        "ReboundHit",
                        0x30
                    },
                    { 
                        "ReboundHit%",
                        0x31
                    },
                    { 
                        "ReboundTarget%",
                        50
                    },
                    { 
                        "AttackModify%",
                        0x33
                    },
                    { 
                        "ExtraSkill",
                        0x34
                    },
                    { 
                        "ExpGet%",
                        0x35
                    },
                    { 
                        "RebornCount",
                        0x36
                    },
                    { 
                        "RebornHP",
                        0x37
                    },
                    { 
                        "RebornHP%",
                        0x38
                    },
                    { 
                        "BodyHit",
                        0x39
                    },
                    { 
                        "BodyHit%",
                        0x3a
                    },
                    { 
                        "HitBack%",
                        0x3b
                    },
                    { 
                        "BodyScale%",
                        60
                    },
                    { 
                        "RotateSpeed",
                        0x3d
                    },
                    { 
                        "RotateSpeed%",
                        0x3e
                    },
                    { 
                        "ArrowEjectCount",
                        0x3f
                    },
                    { 
                        "ArrowEjectMin",
                        0x40
                    },
                    { 
                        "ArrowEjectMax",
                        0x41
                    },
                    { 
                        "ArrowTrack",
                        0x42
                    },
                    { 
                        "ReboundWall",
                        0x43
                    },
                    { 
                        "ReboundWallMin",
                        0x44
                    },
                    { 
                        "ReboundWallMax",
                        0x45
                    },
                    { 
                        "CritAddHP%",
                        70
                    },
                    { 
                        "CritSuperRate%",
                        0x47
                    },
                    { 
                        "CritSuperValue%",
                        0x48
                    },
                    { 
                        "AngelRecover2Rate%",
                        0x49
                    },
                    { 
                        "HP2AttackSpeed%",
                        0x4a
                    },
                    { 
                        "HP2HPAdd%",
                        0x4b
                    },
                    { 
                        "HP2Miss%",
                        0x4c
                    },
                    { 
                        "BabyCountAttack%",
                        0x4d
                    },
                    { 
                        "BabyCountAttackSpeed%",
                        0x4e
                    },
                    { 
                        "StaticReduce%",
                        0x4f
                    },
                    { 
                        "KillBossShield",
                        80
                    },
                    { 
                        "KillBossShield%",
                        0x51
                    },
                    { 
                        "KillMonsterLessHP%",
                        0x52
                    },
                    { 
                        "KillMonsterLessHPRatio%",
                        0x53
                    },
                    { 
                        "DistanceAttackValueDis",
                        0x54
                    },
                    { 
                        "DistanceAttackValue%",
                        0x55
                    },
                    { 
                        "WeaponRoundBackAttack%",
                        0x56
                    },
                    { 
                        "AddShieldValue",
                        0x57
                    },
                    { 
                        "EquipDrop%",
                        0x58
                    },
                    { 
                        "Att_Fire_Add",
                        0x59
                    },
                    { 
                        "Att_Fire_Add%",
                        90
                    },
                    { 
                        "Att_Fire_Resist",
                        0x5b
                    },
                    { 
                        "Att_Fire_Resist%",
                        0x5c
                    },
                    { 
                        "Att_Poison_Add",
                        0x5d
                    },
                    { 
                        "Att_Poison_Add%",
                        0x5e
                    },
                    { 
                        "Att_Poison_Resist",
                        0x5f
                    },
                    { 
                        "Att_Poison_Resist%",
                        0x60
                    },
                    { 
                        "MonsterExp%",
                        0x61
                    },
                    { 
                        "DizzyDelay",
                        0x62
                    },
                    { 
                        "HPDrop%",
                        0x63
                    },
                    { 
                        "GoldDrop%",
                        100
                    },
                    { 
                        "TrapHit%",
                        0x65
                    }
                };
                <>f__switch$map3 = dictionary;
            }
            if (<>f__switch$map3.TryGetValue(type, out int num))
            {
                switch (num)
                {
                    case 0:
                        this.Bullet_Forward.UpdateValueCount(value);
                        return flag;

                    case 1:
                        this.Bullet_Backward.UpdateValueCount(value);
                        return flag;

                    case 2:
                        this.Bullet_Side.UpdateValueCount(value);
                        return flag;

                    case 3:
                        this.Bullet_ForSide.UpdateValueCount(value);
                        return flag;

                    case 4:
                        this.Bullet_Continue.UpdateValueCount(value);
                        return flag;

                    case 5:
                    {
                        long num2 = this.HPValue.Value;
                        this.HPValue.UpdateValueCount(value);
                        this.UpdateHitVampireResult();
                        this.UpdateKillVampireResult();
                        if (this.OnHPUpdate != null)
                        {
                            this.OnHPUpdate(num2);
                        }
                        return flag;
                    }
                    case 6:
                    {
                        long num3 = this.HPValue.Value;
                        this.HPValue.UpdateValuePercent(value);
                        this.UpdateHitVampireResult();
                        this.UpdateKillVampireResult();
                        if (this.OnHPUpdate != null)
                        {
                            this.OnHPUpdate(num3);
                        }
                        return flag;
                    }
                    case 7:
                        this.HPAdd.UpdateValueCount(value);
                        return flag;

                    case 8:
                        this.HPAddPercent.UpdateValuePercent(value);
                        return flag;

                    case 9:
                        this.AttackValue.UpdateValueCount(value);
                        return flag;

                    case 10:
                        this.AttackValue.UpdateValuePercent(value);
                        if (this.OnAttackUpdate != null)
                        {
                            this.OnAttackUpdate(this.AttackValue.Value);
                        }
                        return flag;

                    case 11:
                        this.DefenceValue.UpdateValueCount(value);
                        return flag;

                    case 12:
                        this.DefenceValue.UpdateValuePercent(value);
                        return flag;

                    case 13:
                        this.MoveSpeed.UpdateValueCount(value);
                        if (this.OnMoveSpeedUpdate != null)
                        {
                            this.OnMoveSpeedUpdate();
                        }
                        return flag;

                    case 14:
                        this.MoveSpeed.UpdateValuePercent(value);
                        if (this.OnMoveSpeedUpdate != null)
                        {
                            this.OnMoveSpeedUpdate();
                        }
                        return flag;

                    case 15:
                        this.Attack_Value.UpdateValuePercent(value);
                        return flag;

                    case 0x10:
                        this.Damage_Resistance.UpdateValuePercent(value);
                        return flag;

                    case 0x11:
                        this.HitRate.UpdateValuePercent(value);
                        return flag;

                    case 0x12:
                        this.MissRate.UpdateValue(value);
                        return flag;

                    case 0x13:
                        this.CritRate.UpdateValuePercent(value);
                        return flag;

                    case 20:
                        this.CritDefRate.UpdateValuePercent(value);
                        return flag;

                    case 0x15:
                        this.BlockRate.UpdateValuePercent(value);
                        return flag;

                    case 0x16:
                        this.CritValue.UpdateValuePercent(value);
                        return flag;

                    case 0x17:
                        this.AttackSpeed.UpdateValuePercent(value);
                        return flag;

                    case 0x18:
                        this.HitVampire.UpdateValueCount(value);
                        this.UpdateHitVampireResult();
                        return flag;

                    case 0x19:
                        this.HitVampirePercent.UpdateValueCount(value);
                        this.UpdateHitVampireResult();
                        return flag;

                    case 0x1a:
                        this.HitVampireAddPercent.UpdateValueCount(value);
                        this.UpdateHitVampireResult();
                        return flag;

                    case 0x1b:
                        this.KillVampire.UpdateValueCount(value);
                        this.UpdateKillVampireResult();
                        return flag;

                    case 0x1c:
                        this.KillVampirePercent.UpdateValueCount(value);
                        this.UpdateKillVampireResult();
                        return flag;

                    case 0x1d:
                        this.KillVampireAddPercent.UpdateValueCount(value);
                        this.UpdateKillVampireResult();
                        return flag;

                    case 30:
                        this.TrapDefCount.UpdateValueCount(value);
                        return flag;

                    case 0x1f:
                        this.TrapDef.UpdateValuePercent(value);
                        return flag;

                    case 0x20:
                        this.BulletDef.UpdateValuePercent(value);
                        return flag;

                    case 0x21:
                        this.HitFromFly.UpdateValuePercent(value);
                        return flag;

                    case 0x22:
                        this.HitToFlyPercent.UpdateValuePercent(value);
                        return flag;

                    case 0x23:
                        this.HitToFly.UpdateValueCount(value);
                        return flag;

                    case 0x24:
                        this.HitToGround.UpdateValueCount(value);
                        return flag;

                    case 0x25:
                        this.HitToGroundPercent.UpdateValuePercent(value);
                        return flag;

                    case 0x26:
                        this.HitToNear.UpdateValueCount(value);
                        return flag;

                    case 0x27:
                        this.HitToNearPercent.UpdateValuePercent(value);
                        return flag;

                    case 40:
                        this.HitToFar.UpdateValueCount(value);
                        return flag;

                    case 0x29:
                        this.HitToFarPercent.UpdateValuePercent(value);
                        return flag;

                    case 0x2a:
                        this.HitFromBoss.UpdateValuePercent(value);
                        return flag;

                    case 0x2b:
                        this.HitToBossPercent.UpdateValuePercent(value);
                        return flag;

                    case 0x2c:
                        this.HitToBoss.UpdateValueCount(value);
                        return flag;

                    case 0x2d:
                        this.BodyHittedCount.UpdateValueCount(value);
                        return flag;

                    case 0x2e:
                        this.BodyHitted.UpdateValuePercent(value);
                        return flag;

                    case 0x2f:
                        this.HeadShot.UpdateValuePercent(value);
                        return flag;

                    case 0x30:
                        this.ReboundHit.UpdateValueCount(value);
                        return flag;

                    case 0x31:
                        this.ReboundHit.UpdateValuePercent(value);
                        return flag;

                    case 50:
                        this.ReboundTargetPercent.UpdateValuePercent(value);
                        return flag;

                    case 0x33:
                        this.AttackModify.UpdateValue(value);
                        return flag;

                    case 0x34:
                        this.ExtraSkill.UpdateValueCount(value);
                        return flag;

                    case 0x35:
                        this.ExpGet.UpdateValuePercent(value);
                        return flag;

                    case 0x36:
                        this.RebornCount.UpdateValueCount(value);
                        return flag;

                    case 0x37:
                        this.RebornHP.UpdateValueCount(value);
                        return flag;

                    case 0x38:
                        this.RebornHPPercent.UpdateValuePercent(value);
                        return flag;

                    case 0x39:
                        this.BodyHit.UpdateValueCount(value);
                        return flag;

                    case 0x3a:
                        this.BodyHit.UpdateValuePercent(value);
                        return flag;

                    case 0x3b:
                        this.HitBack.UpdateValuePercent(value);
                        return flag;

                    case 60:
                        this.BodyScale.UpdateValuePercent(value);
                        return flag;

                    case 0x3d:
                        this.RotateSpeed.UpdateValueCount(value);
                        return flag;

                    case 0x3e:
                        this.RotateSpeed.UpdateValuePercent(value);
                        return flag;

                    case 0x3f:
                        this.ArrowEject.UpdateCount((int) value);
                        return flag;

                    case 0x40:
                        this.ArrowEject.UpdateMin((int) value);
                        return flag;

                    case 0x41:
                        this.ArrowEject.UpdateMax((int) value);
                        return flag;

                    case 0x42:
                        this.ArrowTrack.UpdateValueCount(value);
                        return flag;

                    case 0x43:
                        this.ReboundWall.UpdateCount((int) value);
                        return flag;

                    case 0x44:
                        this.ReboundWall.UpdateMin((int) value);
                        return flag;

                    case 0x45:
                        this.ReboundWall.UpdateMax((int) value);
                        return flag;

                    case 70:
                        this.CritAddHP.UpdateValuePercent(value);
                        return flag;

                    case 0x47:
                        this.CritSuperRate.UpdateValuePercent(value);
                        return flag;

                    case 0x48:
                        this.CritSuperValue.UpdateValuePercent(value);
                        return flag;

                    case 0x49:
                        this.AngelR2Rate.UpdateValuePercent(value);
                        return flag;

                    case 0x4a:
                        this.HP2AttackSpeed.UpdateValuePercent(value);
                        return flag;

                    case 0x4b:
                        this.HP2HPAddPercent.UpdateValuePercent(value);
                        return flag;

                    case 0x4c:
                        this.HP2Miss.UpdateValuePercent(value);
                        return flag;

                    case 0x4d:
                        this.BabyCountAttack.UpdateValuePercent(value);
                        return flag;

                    case 0x4e:
                        this.BabyCountAttackSpeed.UpdateValuePercent(value);
                        return flag;

                    case 0x4f:
                        this.StaticReducePercent.UpdateValuePercent(value);
                        return flag;

                    case 80:
                        this.KillBossShield.UpdateValueCount(value);
                        return flag;

                    case 0x51:
                        this.KillBossShieldPercent.UpdateValuePercent(value);
                        return flag;

                    case 0x52:
                        if (this.KillMonsterLessHP.ValuePercent < value)
                        {
                            this.KillMonsterLessHP.InitValuePercent(value);
                        }
                        return flag;

                    case 0x53:
                        if (this.KillMonsterLessHPRatio.ValuePercent < value)
                        {
                            this.KillMonsterLessHPRatio.InitValuePercent(value);
                        }
                        return flag;

                    case 0x54:
                        this.DistanceAttackValueDis.UpdateValueCount(value);
                        return flag;

                    case 0x55:
                        this.DistanceAttackValuePercent.UpdateValuePercent(value);
                        return flag;

                    case 0x56:
                        this.WeaponRoundBackAttackPercent.UpdateValuePercent(value);
                        return flag;

                    case 0x57:
                        this.Shield.UpdateValueCount(value);
                        if (this.Shield_ValueAction != null)
                        {
                            this.Shield_ValueAction(value);
                        }
                        return flag;

                    case 0x58:
                        this.EquipDrop.UpdateValuePercent(value);
                        return flag;

                    case 0x59:
                        this.Att_Fire_Add.UpdateValueCount(value);
                        return flag;

                    case 90:
                        this.Att_Fire_AddPercent.UpdateValuePercent(value);
                        return flag;

                    case 0x5b:
                        this.Att_Fire_Resist.UpdateValueCount(value);
                        return flag;

                    case 0x5c:
                        this.Att_Fire_ResistPercent.UpdateValuePercent(value);
                        return flag;

                    case 0x5d:
                        this.Att_Poison_Add.UpdateValueCount(value);
                        return flag;

                    case 0x5e:
                        this.Att_Poison_AddPercent.UpdateValuePercent(value);
                        return flag;

                    case 0x5f:
                        this.Att_Poison_Resist.UpdateValueCount(value);
                        return flag;

                    case 0x60:
                        this.Att_Poison_ResistPercent.UpdateValuePercent(value);
                        return flag;

                    case 0x61:
                        this.Monster_ExpPercent.UpdateValuePercent(value);
                        return flag;

                    case 0x62:
                        this.Monster_DizzyDelay.UpdateValueCount(value);
                        return flag;

                    case 0x63:
                        this.Monster_HPDrop.UpdateValuePercent(value);
                        return flag;

                    case 100:
                        this.Monster_GoldDrop.UpdateValuePercent(value);
                        return flag;

                    case 0x65:
                        return flag;
                }
            }
        }
        return false;
    }

    public bool GetCanReborn()
    {
        if (this.RebornCount.Value > 0L)
        {
            this.RebornCount.UpdateValueCount(-1L);
            return true;
        }
        return false;
    }

    public long GetHPBase() => 
        this.HPValue.ValueCount;

    public void Reborn_Refresh_Count(int usecount)
    {
        this.RebornCount.UpdateValueCount((long) -usecount);
    }

    public void UpdateHitVampireResult()
    {
        this.HitVampireResult = MathDxx.FloorToInt((this.HitVampire.Value + (this.HitVampirePercent.Value * this.GetHPBase())) * (1f + this.HitVampireAddPercent.Value));
    }

    public void UpdateKillVampireResult()
    {
        this.KillVampireResult = MathDxx.FloorToInt((this.KillVampire.Value + (this.KillVampirePercent.Value * this.GetHPBase())) * (1f + this.KillVampireAddPercent.Value));
    }

    public class ValueBase
    {
        private long mValueCount;
        private long mValuePercent;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private long <Value>k__BackingField;

        public ValueBase()
        {
        }

        public ValueBase(long count)
        {
            this.mValueCount = count;
            this.mValuePercent = 0L;
            this.UpdateValue();
        }

        public ValueBase(long count, long percent)
        {
            this.mValueCount = count;
            this.mValuePercent = percent;
            this.UpdateValue();
        }

        public void InitValueCount(long count)
        {
            this.mValueCount = count;
            this.mValuePercent = 0L;
            this.UpdateValue();
        }

        protected virtual void OnUpdateValue()
        {
        }

        private void UpdateValue()
        {
            this.Value = (long) (((float) (this.mValueCount * (0x2710L + this.mValuePercent))) / 10000f);
            this.OnUpdateValue();
        }

        public void UpdateValueCount(long count)
        {
            this.mValueCount += count;
            this.UpdateValue();
        }

        public void UpdateValuePercent(long percent)
        {
            this.mValuePercent += percent;
            this.UpdateValue();
        }

        public long ValueLong =>
            this.Value;

        public long ValueCount =>
            this.mValueCount;

        public long ValuePercent =>
            this.mValuePercent;

        public long Value { get; private set; }

        public bool Enable =>
            (this.ValueLong > 0L);
    }

    public class ValueFloatBase
    {
        private long mValueCountInit;
        private long mValueCount;
        private long mValuePercent;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <Value>k__BackingField;

        public ValueFloatBase()
        {
            this.InitValueCount(0L);
        }

        public ValueFloatBase(long count)
        {
            this.InitValueCount(count);
        }

        public ValueFloatBase(long count, long percent)
        {
            this.mValueCount = count;
            this.mValuePercent = percent;
            this.mValueCountInit = this.mValueCount;
            this.UpdateValue();
        }

        public void InitValue(long count, long percent)
        {
            this.mValueCount = count;
            this.mValuePercent = percent;
            this.mValueCountInit = this.mValueCount;
            this.UpdateValue();
        }

        public void InitValueCount(long count)
        {
            this.InitValue(count, 0L);
        }

        public void InitValuePercent(long percent)
        {
            this.InitValue(0L, percent);
        }

        private void UpdateValue()
        {
            if ((this.mValueCountInit == 0L) && (this.mValueCount == 0L))
            {
                this.Value = ((float) this.mValuePercent) / 10000f;
            }
            else
            {
                this.Value = ((float) (this.mValueCount * (0x2710L + this.mValuePercent))) / 1E+08f;
            }
        }

        public void UpdateValueCount(long count)
        {
            this.mValueCount += count;
            this.UpdateValue();
        }

        public void UpdateValuePercent(long percent)
        {
            this.mValuePercent += percent;
            this.UpdateValue();
        }

        public long ValueLong
        {
            get
            {
                if ((this.mValueCountInit == 0L) && (this.mValueCount == 0L))
                {
                    return this.mValuePercent;
                }
                return ((this.mValueCount * (0x2710L + this.mValuePercent)) / 0x2710L);
            }
        }

        public long ValueCount =>
            this.mValueCount;

        public long ValuePercent =>
            this.mValuePercent;

        public float Value { get; private set; }
    }

    public class ValueMult
    {
        private long mValue;

        public ValueMult()
        {
            this.mValue = 0x2710L;
        }

        public ValueMult(long value)
        {
            this.InitValue(value);
        }

        public void InitValue(long value)
        {
            this.mValue = value;
        }

        public void UpdateValue(long value)
        {
            if (value > 0L)
            {
                this.mValue = (long) (((float) (this.mValue * (0x2710L + value))) / 10000f);
            }
            else
            {
                this.mValue = (long) (((float) this.mValue) / (((float) (0x2710L - value)) / 10000f));
            }
        }

        public float Value =>
            (((float) this.mValue) / 10000f);

        public long ValueLong =>
            this.mValue;
    }

    public class ValueRange
    {
        private int count;
        private int min;
        private int max;

        public ValueRange()
        {
            this.count = 0;
            this.min = 0;
            this.max = 0;
        }

        public ValueRange(int count, int min, int max)
        {
            this.count = count;
            this.min = min;
            if (max < min)
            {
                max = min;
            }
            this.max = max;
        }

        public void InitValue(EntityAttributeBase.ValueRange data)
        {
            this.count = data.Count;
            this.min = data.Min;
            this.max = data.Max;
        }

        public void UpdateCount(int count)
        {
            this.count += count;
        }

        public void UpdateMax(int max)
        {
            this.max += max;
        }

        public void UpdateMin(int min)
        {
            this.min += min;
        }

        public int Count =>
            this.count;

        public int Min =>
            this.min;

        public int Max =>
            this.max;

        public bool Enable =>
            (this.count > 0);

        public int Value
        {
            get
            {
                if (this.Enable)
                {
                    return GameLogic.Random(this.min, this.max + 1);
                }
                return 0;
            }
        }
    }

    public class ValueReduce
    {
        public List<long> mList;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <Value>k__BackingField;

        public ValueReduce()
        {
            this.mList = new List<long>();
            this.UpdateValue();
        }

        public ValueReduce(long value)
        {
            this.mList = new List<long>();
            this.UpdateValue(value);
        }

        public void InitValue(List<long> list)
        {
            this.mList = list;
            this.UpdateValue();
        }

        private void UpdateValue()
        {
            if (this.mList.Count == 0)
            {
                this.Value = 0f;
            }
            else
            {
                this.Value = 1f;
                int num = 0;
                int count = this.mList.Count;
                while (num < count)
                {
                    this.Value *= 1f - (((float) this.mList[num]) / 10000f);
                    num++;
                }
                this.Value = 1f - this.Value;
            }
        }

        public void UpdateValue(long value)
        {
            if (value > 0L)
            {
                this.mList.Add(value);
            }
            else
            {
                value *= -1L;
                if (this.mList.Contains(value))
                {
                    this.mList.Remove(value);
                }
            }
            this.UpdateValue();
        }

        public float Value { get; private set; }
    }

    public class ValueTime : EntityAttributeBase.ValueBase
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <Value>k__BackingField;

        public ValueTime()
        {
        }

        public ValueTime(long count) : base(count)
        {
        }

        public ValueTime(long count, long percent) : base(count, percent)
        {
        }

        protected override void OnUpdateValue()
        {
            this.Value = ((float) base.Value) / 1000f;
        }

        public float Value { get; private set; }
    }
}

