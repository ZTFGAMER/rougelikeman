using System;

public class ConstString
{
    public const string MoveJoy = "MoveJoy";
    public const string AttackJoy = "AttackJoy";
    public const string WeaponClass = "Weapon";
    public const string BulletClass = "Bullet";
    public const string BossMoveClass = "EntityMove";
    public const string BabyAttribute = "Baby:";
    public const string EquipBabyAttribute = "EquipBaby:";
    public const string LevelUpAttribute = "LevelUp:";

    public class Effect
    {
        public const int CallID = 0x2f4d68;
        public const int DivideID = 0x2f4d69;
        public const int DefenceWave = 0x2f4d77;
    }

    public class GoodsType
    {
        public const string Bullet_Forward = "BulletForward";
        public const string Bullet_Backward = "BulletBackward";
        public const string Bullet_Side = "BulletSide";
        public const string Bullet_ForSide = "BulletForSide";
        public const string Bullet_Continue = "BulletContinue";
        public const string HPMax = "HPMax";
        public const string HPMaxPercent = "HPMax%";
        public const string HPAdd = "HPAdd";
        public const string HPAddPercent = "HPAdd%";
        public const string Attack = "Attack";
        public const string AttackPercent = "Attack%";
        public const string Defence = "Defence";
        public const string DefencePercent = "Defence%";
        public const string MoveSpeed = "MoveSpeed";
        public const string MoveSpeedPercent = "MoveSpeed%";
        public const string Attack_Value = "AttackValue%";
        public const string DamageResistPercent = "DamageResist%";
        public const string HitRate = "HitRate%";
        public const string MissRate = "MissRate%";
        public const string CritRate = "CritRate%";
        public const string CritDefRate = "CritDefRate%";
        public const string BlockRate = "BlockRate%";
        public const string CritValue = "CritValue%";
        public const string AttackSpeedPercent = "AttackSpeed%";
        public const string HitVampire = "HitVampire";
        public const string HitVampirePercent = "HitVampire%";
        public const string HitVampireAddPercent = "HitVampireAdd%";
        public const string KillVampire = "KillVampire";
        public const string KillVampirePercent = "KillVampire%";
        public const string KillVampireAddPercent = "KillVampireAdd%";
        public const string TrapHittedReduce = "TrapHittedReduce";
        public const string TrapHittedPercent = "TrapHittedReduce%";
        public const string BulletReducePercent = "BulletReduce%";
        public const string HitFromFly = "HitFromFly%";
        public const string HitToFlyPercent = "HitToFly%";
        public const string HitToFly = "HitToFly";
        public const string HitToGround = "HitToGround";
        public const string HitToGroundPercent = "HitToGround%";
        public const string HitToNear = "HitToNear";
        public const string HitToNearPercent = "HitToNear%";
        public const string HitToFar = "HitToFar";
        public const string HitToFarPercent = "HitToFar%";
        public const string HitFromBoss = "HitFromBoss%";
        public const string HitToBossPercent = "HitToBoss%";
        public const string HitToBoss = "HitToBoss";
        public const string BodyHittedReduce = "BodyHittedReduce";
        public const string BodyHittedPercent = "BodyHittedReduce%";
        public const string HeadShot = "HeadShot%";
        public const string ReboundHit = "ReboundHit";
        public const string ReboundHitPercent = "ReboundHit%";
        public const string ReboundTargetPercent = "ReboundTarget%";
        public const string AttackModify = "AttackModify%";
        public const string ExtraSkill = "ExtraSkill";
        public const string ExpGet = "ExpGet%";
        public const string RebornCount = "RebornCount";
        public const string RebornHP = "RebornHP";
        public const string RebornHPPercent = "RebornHP%";
        public const string BodyHit = "BodyHit";
        public const string BodyHitPercent = "BodyHit%";
        public const string HitBack = "HitBack%";
        public const string BodyScale = "BodyScale%";
        public const string RotateSpeed = "RotateSpeed";
        public const string RotateSpeedPercent = "RotateSpeed%";
        public const string Invincible = "Invincible";
        public const string BabyArgs = "BabyArgs";
        public const string ArrowEjectCount = "ArrowEjectCount";
        public const string ArrowEjectMin = "ArrowEjectMin";
        public const string ArrowEjectMax = "ArrowEjectMax";
        public const string ArrowTrack = "ArrowTrack";
        public const string ReboundWall = "ReboundWall";
        public const string ReboundWallMin = "ReboundWallMin";
        public const string ReboundWallMax = "ReboundWallMax";
        public const string CritAddHPPercent = "CritAddHP%";
        public const string CritSuperRate = "CritSuperRate%";
        public const string CritSuperValue = "CritSuperValue%";
        public const string AngelRecover2Rate = "AngelRecover2Rate%";
        public const string HPRecoverFixed = "HPRecoverFixed";
        public const string HPRecoverFixedPercent = "HPRecoverFixed%";
        public const string AttackParentAttackPercent = "AttackParentAttack%";
        public const string BodyHitParentAttackPercent = "BodyHitParentAttack%";
        public const string HP2AttackSpeedPercent = "HP2AttackSpeed%";
        public const string HP2HPAddPercent = "HP2HPAdd%";
        public const string HP2MissPercent = "HP2Miss%";
        public const string BabyCountAttackPercent = "BabyCountAttack%";
        public const string BabyCountAttackSpeedPercent = "BabyCountAttackSpeed%";
        public const string StaticReducePercent = "StaticReduce%";
        public const string KillBossShield = "KillBossShield";
        public const string KillBossShieldPercent = "KillBossShield%";
        public const string KillMonsterLessHPPercent = "KillMonsterLessHP%";
        public const string KillMonsterLessHPRatioPercent = "KillMonsterLessHPRatio%";
        public const string DistanceAttackValueDis = "DistanceAttackValueDis";
        public const string DistanceAttackValuePercent = "DistanceAttackValue%";
        public const string WeaponRoundBackAttackPercent = "WeaponRoundBackAttack%";
        public const string Map_TrapHitPercent = "TrapHit%";
        public const string Monster_ExpPercent = "MonsterExp%";
        public const string Monster_DizzyDelay = "DizzyDelay";
        public const string Monster_HPDrop = "HPDrop%";
        public const string Monster_GoldDrop = "GoldDrop%";
        public const string Att_Thunder = "Att_Thunder";
        public const string Att_Fire = "Att_Fire";
        public const string Att_Ice = "Att_Ice";
        public const string Att_Poison = "Att_Poison";
        public const string Att_Fire_Add = "Att_Fire_Add";
        public const string Att_Fire_AddPercent = "Att_Fire_Add%";
        public const string Att_Fire_Resist = "Att_Fire_Resist";
        public const string Att_Fire_ResistPercent = "Att_Fire_Resist%";
        public const string Att_Poison_Add = "Att_Poison_Add";
        public const string Att_Poison_AddPercent = "Att_Poison_Add%";
        public const string Att_Poison_Resist = "Att_Poison_Resist";
        public const string Att_Poison_ResistPercent = "Att_Poison_Resist%";
        public const string Exp = "Exp";
        public const string Gold = "Gold";
        public const string HPRecover = "HPRecover";
        public const string HPRecoverPercent = "HPRecover%";
        public const string AllSpeedPercent = "AllSpeed%";
        public const string BulletSpeedPercent = "BulletSpeed%";
        public const string MaxLevel = "MaxLevel";
        public const string HPRecoverBasePercent = "HPRecoverBase%";
        public const string AddShieldValue = "AddShieldValue";
        public const string EquipDropPercent = "EquipDrop%";
        public const string AttackSpeed_Buff = "AttackSpeed%_Buff";
        public const string Global_HarvestLevel = "Global_HarvestLevel";
        public const string Global_InGameGold = "Global_InGameGold%";
        public const string Global_InGameExp = "Global_InGameExp%";
        public const string Global_UP_Weapon = "Global_UP_Weapon%";
        public const string Global_UP_Hero = "Global_UP_Hero%";
        public const string Global_UP_Armor = "Global_UP_Armor%";
        public const string Global_UP_Pet = "Global_UP_Pet%";
        public const string Global_UP_Ornament = "Global_UP_Ornament%";
        public const string Global_UP_EquipAll = "Global_UP_EquipAll%";
    }

    public class Tags
    {
        public const string Map_Door = "Map_Door";
    }
}

