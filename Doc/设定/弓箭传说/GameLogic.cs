using Dxx.Net;
using Dxx.Util;
using GameProtocol;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using TableTool;
using UnityEngine;

public class GameLogic
{
    private static int bPause = 0;
    private static EGameState GameState = EGameState.Main;
    public const float RoomScaleZ = 1.23f;
    public static int DesignWidth = 720;
    public static int DesignHeight = 0x500;
    public static int ScreenWidth = Screen.width;
    public static int ScreenHeight = Screen.height;
    public static float ScreenRatio = (((float) ScreenHeight) / ((float) ScreenWidth));
    public static int Width = Screen.width;
    public static int Height = Screen.height;
    public static float WidthScale = (((float) Width) / ((float) DesignWidth));
    public static float HeightScale = (((float) Height) / ((float) DesignHeight));
    public static Vector2 ScreenSize;
    public static float WidthScaleAll = ((WidthScale >= HeightScale) ? 1f : (WidthScale / HeightScale));
    private static float _WidthReal = 0f;
    private static Vector3 GetCanHit_mePos;
    private static Vector3 GetCanHit_dir;
    private static RaycastHit[] GetCanHit_rayhits;
    private static int GetCanHit_RayLength;
    private static Quaternion GetCanHit_ChildRotate;
    private static float GetCanHit_Angle;
    private static List<Vector2Int> RandomItem_list;
    private static HoldManager _Hold = null;
    private static ReleaseManager _Release = null;
    private static bool mInGame = false;
    private static SelfAttributeData _SelfAttribute = null;
    private static int BulletID = 0;
    private static Dictionary<HitType, AnimationData> mAnimationList = new Dictionary<HitType, AnimationData>();
    public const string QualityString = "SettingQuality";
    public static Dictionary<int, int> mQualitys;
    private static int mBeforeWidth;
    [CompilerGenerated]
    private static Action <>f__am$cache0;
    [CompilerGenerated]
    private static Action<NetResponse> <>f__am$cache1;

    static GameLogic()
    {
        Dictionary<int, int> dictionary = new Dictionary<int, int> {
            { 
                1,
                480
            },
            { 
                2,
                720
            },
            { 
                3,
                0x438
            }
        };
        mQualitys = dictionary;
        mBeforeWidth = -1;
    }

    public static void BulletCache(int bulletID, GameObject o)
    {
        if (Release != null)
        {
            Release.Bullet.Cache(bulletID, o);
        }
    }

    public static GameObject BulletGet(int bulletID)
    {
        if (Release != null)
        {
            return Release.Bullet.Get(bulletID);
        }
        return null;
    }

    public static bool CheckLine(EntityBase self, EntityBase other)
    {
        Vector3 vector = other.position - self.position;
        Vector3 normalized = vector.normalized;
        float num = vector.magnitude - 0.5f;
        RaycastHit[] hitArray = Physics.SphereCastAll(self.position + (normalized * 0.01f), self.GetCollidersSize(), normalized, num - 0.01f);
        int index = 0;
        int length = hitArray.Length;
        while (index < length)
        {
            RaycastHit hit = hitArray[index];
            if ((hit.collider.gameObject.layer == LayerManager.Stone) || (hit.collider.gameObject.layer == LayerManager.Waters))
            {
                return true;
            }
            index++;
        }
        return false;
    }

    public static void CreateHPChanger(EntityBase from, EntityBase to, HitStruct hs)
    {
        GameObject child = EffectGet("Game/UI/HPChanger");
        child.SetParentNormal(GameNode.m_HP);
        child.GetComponent<HPChanger>().Init(to, hs);
        if ((((from != null) && from.IsSelf) && (hs.type == HitType.Crit)) && ((hs.sourcetype == HitSourceType.eBullet) || (hs.sourcetype == HitSourceType.eBody)))
        {
            GameNode.CameraShake(CameraShakeType.Crit);
        }
    }

    public static void EffectCache(GameObject o)
    {
        if (Release != null)
        {
            if (Release.MapEffect.check_is_map_effect(o))
            {
                Release.MapEffect.Cache(o);
            }
            else
            {
                Release.Effect.Cache(o);
            }
        }
        else if (o != null)
        {
            Object.Destroy(o);
        }
    }

    public static GameObject EffectGet(string key)
    {
        if (Release != null)
        {
            return Release.Effect.Get(key);
        }
        return null;
    }

    public static void EntityCache(GameObject o, int maxcount)
    {
        if (Release != null)
        {
            Release.EntityCache.Cache(o, maxcount);
        }
    }

    public static GameObject EntityGet(string key)
    {
        if (Release != null)
        {
            return Release.EntityCache.Get(key);
        }
        return null;
    }

    public static EntityBase FindTarget(EntityBase self) => 
        Release.Entity.GetNearTarget(self);

    private static HitStruct GetBodyHitStruct(EntityBase source, long beforehit)
    {
        int soundid = 0x3e8fa1;
        return GetBodyHitStruct(source, beforehit, soundid);
    }

    private static HitStruct GetBodyHitStruct(EntityBase source, long beforehit, int soundid) => 
        GetHitStruct(source, beforehit, HitType.Normal, null, HitSourceType.eBody, EElementType.eNone, 0, soundid);

    private static HitStruct GetBuffHitStruct(EntityBase source, long beforehit, EElementType element, int buffid)
    {
        int soundid = 0;
        return GetHitStruct(source, beforehit, HitType.Normal, null, HitSourceType.eBuff, element, buffid, soundid);
    }

    private static HitStruct GetBulletHitStruct(EntityBase source, long beforehit, HitType hittype, HitBulletStruct bulletdata)
    {
        int soundid = 0;
        if ((null != source) && (source.m_Data != null))
        {
            soundid = source.m_Data.HittedEffectID;
        }
        return GetBulletHitStruct(source, beforehit, hittype, bulletdata, soundid);
    }

    private static HitStruct GetBulletHitStruct(EntityBase source, long beforehit, HitType hittype, HitBulletStruct bulletdata, int soundid) => 
        GetHitStruct(source, beforehit, hittype, bulletdata, HitSourceType.eBullet, EElementType.eNone, 0, soundid);

    public static int GetBulletID()
    {
        int bulletID = BulletID;
        BulletID++;
        return bulletID;
    }

    public static bool GetCanHit(EntityBase me, EntityBase other)
    {
        if (me.Child == null)
        {
            return false;
        }
        if (other == null)
        {
            return false;
        }
        if (!other.GetMeshShow())
        {
            return false;
        }
        if (other.position.y < -1f)
        {
            return false;
        }
        GetCanHit_ChildRotate = me.Child.transform.localRotation;
        GetCanHit_Angle = Utils.getAngle(other.position.x - me.position.x, other.position.z - me.position.z);
        me.Child.transform.localRotation = Quaternion.Euler(0f, GetCanHit_Angle, 0f);
        GetCanHit_dir = other.position - me.m_Body.LeftBullet.transform.position;
        me.m_Body.LeftBullet.transform.rotation = Quaternion.Euler(0f, Utils.getAngle(GetCanHit_dir), 0f);
        GetCanHit_dir.y = 0f;
        GetCanHit_mePos = me.m_Body.LeftBullet.transform.position;
        GetCanHit_mePos.y = 0f;
        GetCanHit_rayhits = Physics.RaycastAll(GetCanHit_mePos, GetCanHit_dir, GetCanHit_dir.magnitude, (((int) 1) << LayerManager.Bullet2Map) | (((int) 1) << LayerManager.MapOutWall));
        me.Child.transform.localRotation = GetCanHit_ChildRotate;
        me.m_Body.LeftBullet.transform.localRotation = Quaternion.identity;
        int index = 0;
        int length = GetCanHit_rayhits.Length;
        while (index < length)
        {
            if ((GetCanHit_rayhits[index].collider.gameObject.layer == LayerManager.Bullet2Map) || (GetCanHit_rayhits[index].collider.gameObject.layer == LayerManager.MapOutWall))
            {
                other.SetCanHit(false);
                return false;
            }
            index++;
        }
        other.SetCanHit(true);
        return true;
    }

    public static EElementType GetElement(string value)
    {
        if (value != null)
        {
            if (value == "Att_Fire")
            {
                return EElementType.eFire;
            }
            if (value == "Att_Poison")
            {
                return EElementType.ePoison;
            }
            if (value == "Att_Thunder")
            {
                return EElementType.eThunder;
            }
            if (value == "Att_Ice")
            {
                return EElementType.eIce;
            }
        }
        return EElementType.eNone;
    }

    public static List<BattleDropData> GetExpList(int exp)
    {
        List<BattleDropData> list = new List<BattleDropData>();
        int num = exp / 10;
        exp = exp % 10;
        int num2 = exp;
        for (int i = 0; i < num; i++)
        {
            list.Add(new BattleDropData(FoodType.eExp, FoodOneType.eExp02, 0));
        }
        for (int j = 0; j < num2; j++)
        {
            list.Add(new BattleDropData(FoodType.eExp, FoodOneType.eExp01, 0));
        }
        return list;
    }

    private static HitStruct GetHitStruct(EntityBase source, long beforehit, HitType hittype, HitBulletStruct bulletdata, HitSourceType sourcetype, EElementType element, int buffid, int soundid) => 
        new HitStruct { 
            source = source,
            before_hit = beforehit,
            type = (beforehit > 0L) ? HitType.Add : hittype,
            bulletdata = bulletdata,
            sourcetype = sourcetype,
            element = element,
            buffid = buffid,
            soundid = soundid
        };

    public static AnimationCurve GetHPChangerAnimation(HitType type, int curve)
    {
        if (!mAnimationList.TryGetValue(type, out AnimationData data))
        {
            if (type != HitType.Crit)
            {
                if (type == HitType.HeadShot)
                {
                    data = new AnimationData(0x186b1, 0x186ae, 0x186af);
                }
                else
                {
                    data = new AnimationData(0x186ad, 0x186ae, 0x186af);
                }
            }
            else
            {
                data = new AnimationData(0x186b0, 0x186ae, 0x186af);
            }
            mAnimationList.Add(type, data);
        }
        return data.curves[curve];
    }

    public static long GetMaxHP(int entityid)
    {
        EntityAttributeBase base2 = new EntityAttributeBase(entityid);
        string[] monsterTmxAttributes = Hold.BattleData.mModeData.GetMonsterTmxAttributes();
        if ((monsterTmxAttributes != null) && (monsterTmxAttributes.Length > 0))
        {
            int index = 0;
            int length = monsterTmxAttributes.Length;
            while (index < length)
            {
                Goods_goods.GoodData goodData = Goods_goods.GetGoodData(monsterTmxAttributes[index]);
                base2.Excute(goodData);
                index++;
            }
        }
        return base2.HPValue.Value;
    }

    private static HitStruct GetReboundHitStruct(EntityBase source, HitStruct hs)
    {
        int soundid = 0;
        return GetHitStruct(source, hs.before_hit, HitType.Rebound, null, hs.sourcetype, EElementType.eNone, 0, soundid);
    }

    private static HitStruct GetRecoverStruct(long value) => 
        GetHitStruct(null, value, HitType.Add, null, HitSourceType.eRecover, EElementType.eNone, 0, 0);

    private static HitStruct GetSkillHitStruct(EntityBase entity, long beforehit)
    {
        int soundid = 0;
        if ((entity != null) && (entity.m_Data != null))
        {
            soundid = entity.m_Data.HittedEffectID;
        }
        return GetSkillHitStruct(beforehit, soundid);
    }

    private static HitStruct GetSkillHitStruct(long beforehit, int soundid) => 
        GetHitStruct(null, beforehit, HitType.Normal, null, HitSourceType.eSkill, EElementType.eNone, 0, soundid);

    private static int GetTeam(EntityBase entity)
    {
        if (entity == null)
        {
            return 0;
        }
        if (entity.Type == EntityType.Hero)
        {
            return 1;
        }
        if (entity.Type == EntityType.Baby)
        {
            if ((entity as EntityBabyBase).GetParent().Type == EntityType.Hero)
            {
                return 1;
            }
        }
        else if ((entity.Type == EntityType.PartBody) && ((entity as EntityPartBodyBase).GetParent().Type == EntityType.Hero))
        {
            return 1;
        }
        return 2;
    }

    private static HitStruct GetTrapHitStruct(EntityBase entity, long beforehit)
    {
        int soundid = 0;
        if ((entity != null) && (entity.m_Data != null))
        {
            soundid = entity.m_Data.HittedEffectID;
        }
        return GetTrapHitStruct(beforehit, soundid);
    }

    private static HitStruct GetTrapHitStruct(long beforehit, int soundid) => 
        GetHitStruct(null, beforehit, HitType.Normal, null, HitSourceType.eTrap, EElementType.eNone, 0, soundid);

    private static HitStruct GetTrapHitStruct(EntityBase entity, long beforehit, int soundid) => 
        GetTrapHitStruct(beforehit, soundid);

    public static void HoldCache(GameObject o)
    {
        Hold.Pool.Cache(o);
    }

    public static GameObject HoldGet(string key) => 
        Hold.Pool.Get(key);

    public static bool IsSameTeam(EntityBase me, EntityBase other) => 
        (GetTeam(me) == GetTeam(other));

    public static void PlayBattle_Main()
    {
        int modeLevelKey = GameConfig.GetModeLevelKey();
        LocalSave.Instance.Modify_Key((long) -modeLevelKey, true);
        Hold.Sound.PlayUI(0xf4243);
        Hold.BattleData.SetMode(GameMode.eLevel, BattleSource.eWorld);
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = () => WindowUI.ShowWindow(WindowID.WindowID_Battle);
        }
        WindowUI.ShowLoading(<>f__am$cache0, null, null, BattleLoadProxy.LoadingType.eFirstBattle);
        CLifeTransPacket packet = new CLifeTransPacket {
            m_nTransID = LocalSave.Instance.SaveExtra.GetTransID(),
            m_nType = 1,
            m_nMaterial = (ushort) modeLevelKey
        };
        if (<>f__am$cache1 == null)
        {
            <>f__am$cache1 = delegate (NetResponse response) {
                if (response.IsSuccess)
                {
                }
            };
        }
        NetManager.SendInternal<CLifeTransPacket>(packet, SendType.eCache, <>f__am$cache1);
    }

    public static void PlayEffect(int fxId, Vector3 position)
    {
        Fx_fx beanById = LocalModelManager.Instance.Fx_fx.GetBeanById(fxId);
        Release.MapEffect.Get(beanById.Path).transform.position = position;
    }

    public static void PlayEffect(string path, Transform parent)
    {
        GameObject obj2 = EffectGet(path);
        obj2.transform.SetParent(parent);
        obj2.transform.localPosition = Vector3.zero;
        obj2.transform.localScale = Vector3.one;
    }

    public static int Random(int min, int max) => 
        UnityEngine.Random.Range(min, max);

    public static long Random(long min, long max) => 
        ((long) UnityEngine.Random.Range((int) min, (int) max));

    public static float Random(float min, float max) => 
        UnityEngine.Random.Range(min, max);

    public static void RandomItem(EntityBase entity, int range, out float endx, out float endz)
    {
        RandomItem_list = Release.MapCreatorCtrl.GetRoundEmpty(entity.position, range);
        if (RandomItem_list.Count == 0)
        {
            endx = entity.position.x;
            endz = entity.position.z;
        }
        else
        {
            int num = Random(0, RandomItem_list.Count);
            Vector3 worldPosition = Release.MapCreatorCtrl.GetWorldPosition(RandomItem_list[num]);
            endx = worldPosition.x;
            endz = worldPosition.z;
        }
    }

    public static void ResetMaxResolution()
    {
        SetResolution(0x438);
    }

    public static void ResetRectTransform(RectTransform tran)
    {
        tran.offsetMin = Vector2.zero;
        tran.offsetMax = Vector2.zero;
        tran.sizeDelta = Vector2.zero;
        tran.localPosition = Vector3.zero;
        tran.localScale = Vector3.one;
    }

    public static void ResetRectTransform(Transform t)
    {
        ResetRectTransform(t as RectTransform);
    }

    public static void Send_Recover(EntityBase target, long value)
    {
        if (target != null)
        {
            target.ExcuteCommend(EBattleAction.EBattle_Action_Hitted_Once, GetRecoverStruct(value));
        }
    }

    public static void SendBuff(EntityBase target, int buffid, params float[] args)
    {
        if (target != null)
        {
            BattleStruct.BuffStruct data = new BattleStruct.BuffStruct {
                buffId = buffid,
                args = args
            };
            target.ExcuteCommend(EBattleAction.EBattle_Action_Add_Buff, data);
        }
    }

    public static void SendBuff(EntityBase target, EntityBase source, int buffid, params float[] args)
    {
        SendBuffInternal(target, source, buffid, args);
    }

    public static void SendBuffInternal(EntityBase target, EntityBase source, int buffid, params float[] args)
    {
        if (target != null)
        {
            BattleStruct.BuffStruct data = new BattleStruct.BuffStruct {
                entity = source,
                buffId = buffid,
                args = args
            };
            target.ExcuteCommend(EBattleAction.EBattle_Action_Add_Buff, data);
        }
    }

    public static void SendHit_Body(EntityBase target, EntityBase source, long beforehit)
    {
        SendHit_Body(target, source, beforehit, 0x3e8fa1);
    }

    public static void SendHit_Body(EntityBase target, EntityBase source, long beforehit, int soundid)
    {
        if ((target != null) && (beforehit < 0L))
        {
            target.ExcuteCommend(EBattleAction.EBattle_Action_Hitted_Once, GetBodyHitStruct(source, beforehit, soundid));
        }
    }

    public static void SendHit_Buff(EntityBase target, EntityBase source, long beforehit, EElementType element, int buffid)
    {
        if (target != null)
        {
            target.ExcuteCommend(EBattleAction.EBattle_Action_Hitted_Once, GetBuffHitStruct(source, beforehit, element, buffid));
        }
    }

    public static void SendHit_Bullet(EntityBase target, EntityBase source, long beforehit, HitType hittype, HitBulletStruct bulletdata)
    {
        SendHit_Bullet(target, source, beforehit, hittype, bulletdata, target.m_Data.HittedEffectID);
    }

    public static void SendHit_Bullet(EntityBase target, EntityBase source, long beforehit, HitType hittype, HitBulletStruct bulletdata, int soundid)
    {
        if (target != null)
        {
            target.ExcuteCommend(EBattleAction.EBattle_Action_Hitted_Once, GetBulletHitStruct(source, beforehit, hittype, bulletdata, soundid));
        }
    }

    public static void SendHit_Rebound(EntityBase target, EntityBase source, HitStruct hs)
    {
        if (target != null)
        {
            target.ExcuteCommend(EBattleAction.EBattle_Action_Hitted_Once, GetReboundHitStruct(source, hs));
        }
    }

    public static void SendHit_Skill(EntityBase target, long beforehit)
    {
        if (target != null)
        {
            target.ExcuteCommend(EBattleAction.EBattle_Action_Hitted_Once, GetSkillHitStruct(target, beforehit));
        }
    }

    public static void SendHit_Skill(EntityBase target, long beforehit, int soundid)
    {
        if (target != null)
        {
            target.ExcuteCommend(EBattleAction.EBattle_Action_Hitted_Once, GetSkillHitStruct(beforehit, soundid));
        }
    }

    public static void SendHit_Trap(EntityBase target, long beforehit)
    {
        if (target != null)
        {
            target.ExcuteCommend(EBattleAction.EBattle_Action_Hitted_Once, GetTrapHitStruct(target, beforehit));
        }
    }

    public static void SendHit_Trap(EntityBase target, long beforehit, int soundid)
    {
        if (target != null)
        {
            target.ExcuteCommend(EBattleAction.EBattle_Action_Hitted_Once, GetTrapHitStruct(target, beforehit, soundid));
        }
    }

    public static void SetGameState(EGameState state)
    {
        GameState = state;
        if (state != EGameState.Gaming)
        {
            if (state == EGameState.Over)
            {
                Release.Game.EndGame();
            }
        }
        else
        {
            Release.Game.StartGame();
        }
    }

    public static void SetHold(HoldManager hold)
    {
        _Hold = hold;
    }

    public static void SetInGame(bool gaming)
    {
        mInGame = gaming;
        GameNode.m_Camera.gameObject.SetActive(mInGame);
        GameNode.m_Light.SetActive(mInGame);
    }

    public static void SetPause(bool pause)
    {
        bPause += !pause ? -1 : 1;
        Time.timeScale = (bPause <= 0) ? 1f : 0f;
    }

    public static void SetRelease(ReleaseManager release)
    {
        _Release = release;
    }

    private static bool SetResolution(int res)
    {
        int num = res;
        int screenHeight = ScreenHeight;
        int screenWidth = ScreenWidth;
        screenHeight = (int) ((screenHeight * num) / ((float) screenWidth));
        screenWidth = num;
        if ((ScreenWidth >= screenWidth) && (ScreenHeight >= screenHeight))
        {
            SetResolution(screenWidth, screenHeight);
            return true;
        }
        SetResolution(ScreenWidth, ScreenHeight);
        return false;
    }

    private static void SetResolution(int width, int height)
    {
        if (mBeforeWidth != width)
        {
            mBeforeWidth = width;
            Debugger.Log(string.Concat(new object[] { "SetResolution w ", width, " h ", height }));
            Screen.SetResolution(width, height, true);
        }
    }

    public static void ShowHPMaxChange(long change)
    {
        HitStruct hs = new HitStruct {
            type = HitType.HPMaxChange,
            real_hit = change
        };
        CreateHPChanger(null, Self, hs);
    }

    public static void ShowPowerUpdate(int before, int after)
    {
        GameObject child = Object.Instantiate<GameObject>(ResourceManager.Load<GameObject>("UIPanel/MainUI/PowerUpdate"));
        child.SetParentNormal(GameNode.m_FrontEvent);
        RectTransform transform = child.transform as RectTransform;
        transform.anchoredPosition = new Vector2(0f, Height * -0.1f);
        child.GetComponent<PowerUpdateCtrl>().Init(Random(100, 200), Random(100, 200));
    }

    public static void UpdateResolution()
    {
        SetResolution(mQualitys[QualityID]);
    }

    public static bool Paused =>
        (bPause > 0);

    public static float WidthReal
    {
        get
        {
            if (_WidthReal == 0f)
            {
                float num = (((float) Width) / ((float) Height)) * DesignHeight;
                _WidthReal = MathDxx.Clamp(num, num, 720f);
            }
            return _WidthReal;
        }
        set => 
            (_WidthReal = value);
    }

    public static HoldManager Hold =>
        _Hold;

    public static ReleaseManager Release =>
        _Release;

    public static bool InGame =>
        mInGame;

    public static EntityHero Self =>
        Release.Entity.Self;

    public static SelfAttributeData SelfAttribute
    {
        get
        {
            if (_SelfAttribute == null)
            {
                _SelfAttribute = new SelfAttributeData();
            }
            return _SelfAttribute;
        }
    }

    public static SelfAttributeData SelfAttributeShow
    {
        get
        {
            SelfAttributeData data = new SelfAttributeData();
            data.Init();
            return data;
        }
    }

    public static GameMode AdventureMode
    {
        get => 
            ((GameMode) PlayerPrefs.GetInt("GameLogic.AdventureMode", 0x3e8));
        set => 
            PlayerPrefs.SetInt("GameLogic.AdventureMode", (int) value);
    }

    public static int Main_Stage
    {
        get => 
            PlayerPrefs.GetInt("GameLogic.Main_Stage", 0);
        set => 
            PlayerPrefs.SetInt("GameLogic.Main_Stage", value);
    }

    public static int QualityID
    {
        get
        {
            if (!PlayerPrefsEncrypt.HasKey("SettingQuality"))
            {
                if (PlatformHelper.GetFlagShip())
                {
                    QualityID = 3;
                }
                else
                {
                    QualityID = 2;
                }
            }
            return PlayerPrefsEncrypt.GetInt("SettingQuality", 2);
        }
        set => 
            PlayerPrefsEncrypt.SetInt("SettingQuality", value);
    }

    private class AnimationData
    {
        public AnimationCurve[] curves;

        public AnimationData(int id1, int id2, int id3)
        {
            this.curves = new AnimationCurve[] { LocalModelManager.Instance.Curve_curve.GetCurve(id1), LocalModelManager.Instance.Curve_curve.GetCurve(id2), LocalModelManager.Instance.Curve_curve.GetCurve(id3) };
        }
    }

    public enum EGameState
    {
        Main,
        Gaming,
        Pause,
        Over
    }
}

