using System;

public class LayerManager
{
    public static int UI = LayerMask.NameToLayer("UI");
    public static int Player = LayerMask.NameToLayer("Player");
    public static int Map = LayerMask.NameToLayer("Map");
    public static int Goods = LayerMask.NameToLayer("Goods");
    public static int Fly = LayerMask.NameToLayer("Fly");
    public static int MapOutWall = LayerMask.NameToLayer("MapOutWall");
    public static int Bullet = LayerMask.NameToLayer("Bullet");
    public static int Bullet2Map = LayerMask.NameToLayer("Bullet2Map");
    public static int PlayerAbsorb = LayerMask.NameToLayer("PlayerAbsorb");
    public static int PlayerAbsorbImme = LayerMask.NameToLayer("PlayerAbsorbImme");
    public static int Entity2MapOutWall = LayerMask.NameToLayer("Entity2MapOutWall");
    public static int Entity2Stone = LayerMask.NameToLayer("Entity2Stone");
    public static int Entity2Water = LayerMask.NameToLayer("Entity2Water");
    public static int Stone = LayerMask.NameToLayer("Stone");
    public static int Waters = LayerMask.NameToLayer("Waters");
    public static int BattleHits = LayerMask.NameToLayer("BattleHits");
    public static int BulletResist = LayerMask.NameToLayer("BulletResist");
    public static int Hide = LayerMask.NameToLayer("Hide");
    public static int[] BulletTriggers = new int[] { ((((((int) 1) << Player) | (((int) 1) << MapOutWall)) | (((int) 1) << Bullet2Map)) | (((int) 1) << BulletResist)), (((((int) 1) << Player) | (((int) 1) << MapOutWall)) | (((int) 1) << BulletResist)), ((((int) 1) << Player) | (((int) 1) << BulletResist)) };
    public static int HitEntity = (((int) 1) << Player);
    public static int MapAllInt = (((((int) 1) << Stone) | (((int) 1) << Waters)) | (((int) 1) << MapOutWall));
    public static int Move_Fly = 0;
    public static int Move_Ground = ((((int) 1) << Stone) | (((int) 1) << Waters));
    public const int RenderQueue_Fly = 0xbb8;
    public const int RenderQueue_Default = 0x7d0;

    public static int GetBullet(BulletLayer type) => 
        BulletTriggers[(int) type];

    public static bool IsCollisionMap(int layer) => 
        (((layer == Stone) || (layer == Waters)) || (layer == MapOutWall));

    public enum BulletLayer
    {
        eAll,
        eOnlyOut,
        eNone
    }
}

