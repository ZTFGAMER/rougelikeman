using DG.Tweening;
using Dxx.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using TableTool;
using UnityEngine;

public class EntityBase : MonoBehaviour
{
    public Action Event_DeInit;
    public Action Event_OnAttack;
    [NonSerialized]
    public string ClassName;
    [NonSerialized]
    public int ClassID;
    protected string HPSliderName = "HPSlider";
    [NonSerialized]
    public Character_Char m_Data;
    public EntityData m_EntityData;
    public float HPOffsetY = 100f;
    protected GameObject child;
    [NonSerialized]
    public AnimatorBase m_AniCtrl;
    [NonSerialized]
    public MoveControl m_MoveCtrl;
    [NonSerialized]
    public AttackControl m_AttackCtrl;
    public WeaponBase m_Weapon;
    public AnimationCtrlBase mAniCtrlBase;
    public BodyMask m_Body;
    public HitEdit m_HitEdit;
    protected SphereCollider m_SphereCollider;
    protected CapsuleCollider m_CapsuleCollider;
    protected BoxCollider m_BoxCollider;
    protected Dictionary<string, BoxCollider> m_ChildsBoxCollider = new Dictionary<string, BoxCollider>();
    protected Dictionary<string, SphereCollider> m_ChildsSphereCollider = new Dictionary<string, SphereCollider>();
    protected Dictionary<string, CapsuleCollider> m_ChildsCapsuleCollider = new Dictionary<string, CapsuleCollider>();
    protected const string Entity2MapOutWall = "Entity2MapOutWall";
    protected const string Entity2Stone = "Entity2Stone";
    protected const string Entity2Water = "Entity2Water";
    public HpSlider m_HPSlider;
    public float AliveTime;
    [NonSerialized]
    private bool m_bElite;
    protected float HittedX;
    protected float HittedY;
    public Vector3 HittedDirection = Vector3.zero;
    public float HittedAngle;
    public float HittedV;
    public Vector3 hittedoffset;
    [SerializeField]
    protected EntityState m_State;
    [SerializeField, Tooltip("当前生命值")]
    private string HPPercent;
    private int showhpcount = 1;
    private int showmeshcount = 1;
    private Transform[] childs;
    private bool showchallenge = true;
    private EntityBase m_HatredTargetP;
    private bool bInit;
    private bool Dead_bPlay;
    private int Dead_PlayCount = 20;
    private int Dead_CurrentCount;
    private float Dead_StartAngle;
    private float Dead_PerAngle;
    private bool bFlyWater;
    private bool bFlyStone;
    public Action<bool> OnPlayHittedAction;
    public Action<EntityBase, Vector3> OnKillAction;
    public Action<EntityBase, Vector3> OnHitAction;
    public Action OnSkillActionEnd;
    public Action OnWillDead;
    public Action<long, long, float, long> OnChangeHPAction;
    public Action<long, long> OnMaxHpUpdate;
    public Action<EntityBase> OnMonsterDeadAction;
    public Action<int> OnLevelUp;
    public Action<long> Shield_CountAction;
    public Action<long> Shield_ValueAction;
    public Action<bool> OnMoveEvent;
    public Action OnMissAngel;
    public Action OnMissDemon;
    public Action OnMissShop;
    public Action OnInBossRoom;
    public Action<EntityBase, long> OnHitted;
    public Action<long> OnCrit;
    public Action OnFullHP;
    public Action<bool> OnDizzy;
    public Action OnMiss;
    public Action<EntityBase> OnLight45;
    private List<int> mBabySkillIds = new List<int>();
    private List<long> mBabyArgs = new List<long>();
    private List<Vector3> mBabyGroundPos;
    private int mBabyGroundIndex;
    private List<int> DebuffList;
    private int mCallID;
    private Vector3 mCallEndPos;
    private EntityHitCtrl mHitCtrl;
    public int collidercount;
    private List<EntityCtrlBase> ctrlsList;
    public float HittedLastTime;
    private bool bCanHit;
    private GameObject CantHitTarget;
    private GameObject HitTarget;
    protected Sequence mDeadSeq;
    public bool bCall;
    protected ActionBasic mAction;
    private int divide_frame;
    private int divide_maxframe;
    private Vector3 dividemove;
    private Vector3 _position;
    private Vector3 _eulerAngles;
    private Rigidbody _rigid;
    private Vector3 SetPositionBy_P;
    private int move_layermask;
    private float move_offset;
    private RaycastHit[] move_hits;
    private RaycastHit move_hit;
    private float move_dis;
    private Vector3 move_vec;
    private int mSuperArmor;
    private Dictionary<int, PartBodyData> mPartBodyList;
    private int PartBody_AliveCount;
    private int PartBody_MaxCount;
    private Dictionary<int, RotateFollowData> mRotateFollowList;
    private Dictionary<int, int> mRotateIndexList;
    private RotateBallClass mRotateAttribute;
    private RotateBallClass mRotateSword;
    private RotateClass mRotateShield;
    private Dictionary<int, SkillBase> skillsList;
    private Dictionary<int, SkillBase> skillsAutoList;
    private List<SkillBase> skillsAttributeList;
    private List<int> skillidList;
    private List<SkillBase> skillsOverlyingList;
    [SerializeField]
    private EntityType m_Type;
    private string _namep;
    public bool bDivide;
    private string mDivideID;
    public GameObject m_WeaponHand;

    [field: CompilerGenerated, DebuggerBrowsable(0)]
    public event Action<Vector3> Event_PositionBy;

    public EntityBase()
    {
        List<Vector3> list = new List<Vector3> {
            new Vector3(-2f, 0f, 0f),
            new Vector3(-1.5f, 0f, -1.5f),
            new Vector3(0f, 0f, -1.5f),
            new Vector3(1.5f, 0f, -1.5f),
            new Vector3(2f, 0f, 0f)
        };
        this.mBabyGroundPos = list;
        this.DebuffList = new List<int>();
        this.collidercount = 1;
        this.ctrlsList = new List<EntityCtrlBase>();
        this.bCanHit = true;
        this.mAction = new ActionBasic();
        this.divide_maxframe = 5;
        this.move_vec = new Vector3(0f, 1f, 0f);
        Dictionary<int, PartBodyData> dictionary = new Dictionary<int, PartBodyData>();
        PartBodyData data = new PartBodyData {
            alivecount = 0,
            maxcount = 10
        };
        dictionary.Add(0x709, data);
        data = new PartBodyData {
            alivecount = 0,
            maxcount = 8
        };
        dictionary.Add(0x70a, data);
        data = new PartBodyData {
            alivecount = 0,
            maxcount = 8
        };
        dictionary.Add(0x70b, data);
        data = new PartBodyData {
            alivecount = 0,
            maxcount = 8
        };
        dictionary.Add(0x70c, data);
        data = new PartBodyData {
            alivecount = 0,
            maxcount = 8
        };
        dictionary.Add(0x70d, data);
        data = new PartBodyData {
            alivecount = 0,
            maxcount = 8
        };
        dictionary.Add(0x70e, data);
        data = new PartBodyData {
            alivecount = 0,
            maxcount = 8
        };
        dictionary.Add(0x70f, data);
        this.mPartBodyList = dictionary;
        this.PartBody_MaxCount = 10;
        this.mRotateFollowList = new Dictionary<int, RotateFollowData>();
        this.mRotateIndexList = new Dictionary<int, int>();
        this.skillsList = new Dictionary<int, SkillBase>();
        this.skillsAutoList = new Dictionary<int, SkillBase>();
        this.skillsAttributeList = new List<SkillBase>();
        this.skillidList = new List<int>();
        this.skillsOverlyingList = new List<SkillBase>();
        this._namep = string.Empty;
        this.mDivideID = string.Empty;
    }

    public void AddBabySkillID(int id)
    {
        this.mBabySkillIds.Add(id);
    }

    public void AddController<T>() where T: EntityCtrlBase, new()
    {
        T item = Activator.CreateInstance<T>();
        item.OnStart(item.mActionsList);
        item.SetEntity(this);
        if (item.UseUpdate)
        {
            object[] args = new object[] { this.m_EntityData.CharID, item.GetType().ToString() };
            T local1 = item;
            Updater.AddUpdate(Utils.FormatString("{0}.AddController<{1}>", args), new Action<float>(local1.OnUpdate), false);
        }
        this.ctrlsList.Add(item);
    }

    public void AddDebuff(int debuffid)
    {
        this.DebuffList.Add(debuffid);
    }

    public void AddHatredTarget(EntityBase entity)
    {
    }

    private void AddInitSkills()
    {
        int index = 0;
        int length = this.m_Data.Skills.Length;
        while (index < length)
        {
            this.AddSkillAuto(this.m_Data.Skills[index], Array.Empty<object>());
            index++;
        }
    }

    public void AddNewRotateAttribute(GameObject o)
    {
        if (this.mRotateAttribute == null)
        {
            this.mRotateAttribute = new RotateBallClass();
            this.mRotateAttribute.SetRadius(3.5f);
            this.mRotateAttribute.Init(this, "RotateAttribute", -5f, 180f);
        }
        this.mRotateAttribute.AddNewRotateAttribute(o);
    }

    public void AddNewRotateShield(GameObject o)
    {
        if (this.mRotateShield == null)
        {
            this.mRotateShield = new RotateClass();
            this.mRotateShield.Init(this, "RotateShield", 1f, 360f);
        }
        this.mRotateShield.AddNewRotateAttribute(o);
    }

    public void AddNewRotateSword(GameObject o)
    {
        if (this.mRotateSword == null)
        {
            this.mRotateSword = new RotateBallClass();
            this.mRotateSword.SetRadius(0.3f);
            this.mRotateSword.Init(this, "RotateSword", 4f, 180f);
        }
        this.mRotateSword.AddNewRotateAttribute(o);
    }

    public void AddRotateFollow(EntityBase entity)
    {
        int charID = entity.m_Data.CharID;
        if (!this.mRotateFollowList.TryGetValue(charID, out RotateFollowData data))
        {
            data = new RotateFollowData(charID);
            data.Init(this, 3f, 3f);
            this.mRotateFollowList.Add(charID, data);
        }
        data.Add(entity);
    }

    public void AddSkill(int skillId, params object[] args)
    {
        this.AddSkillInternal(skillId, args);
        if (this.IsSelf)
        {
            LocalSave.Instance.BattleIn_UpdateSkill(skillId);
        }
    }

    public void AddSkillAttribute(int skillId, params object[] args)
    {
        Skill_skill beanById = LocalModelManager.Instance.Skill_skill.GetBeanById(skillId);
        if (beanById != null)
        {
            SkillBase item = new SkillBase();
            item.Install(this, beanById, args);
            this.skillsAttributeList.Add(item);
        }
    }

    public void AddSkillAuto(int skillId, params object[] args)
    {
        if (!this.skillsAutoList.ContainsKey(skillId))
        {
            Skill_skill beanById = LocalModelManager.Instance.Skill_skill.GetBeanById(skillId);
            if (beanById != null)
            {
                SkillBase base2 = new SkillBase();
                base2.Install(this, beanById, args);
                this.skillsAutoList.Add(skillId, base2);
            }
        }
    }

    public void AddSkillBaby(int skillId, params object[] args)
    {
        Skill_skill beanById = LocalModelManager.Instance.Skill_skill.GetBeanById(skillId);
        if (beanById != null)
        {
            SkillBase item = new SkillBase();
            item.Install(this, beanById, args);
            this.skillsOverlyingList.Add(item);
        }
    }

    protected void AddSkillInternal(int skillId, params object[] args)
    {
        if (!this.skillsList.ContainsKey(skillId))
        {
            Skill_skill beanById = LocalModelManager.Instance.Skill_skill.GetBeanById(skillId);
            if (beanById != null)
            {
                SkillBase base2 = new SkillBase();
                base2.Install(this, beanById, args);
                this.skillsList.Add(skillId, base2);
                this.skillidList.Add(skillId);
            }
        }
    }

    public void AddSkillOverLying(int skillId, params object[] args)
    {
        Skill_skill beanById = LocalModelManager.Instance.Skill_skill.GetBeanById(skillId);
        if (beanById != null)
        {
            SkillBase item = new SkillBase();
            item.Install(this, beanById, args);
            this.skillsOverlyingList.Add(item);
        }
    }

    public void AddSkillTest(int skillId)
    {
        Skill_skill beanById = LocalModelManager.Instance.Skill_skill.GetBeanById(skillId);
        if (beanById != null)
        {
            SkillBase item = new SkillBase();
            item.Install(this, beanById, Array.Empty<object>());
            this.skillsOverlyingList.Add(item);
            this.skillidList.Add(skillId);
        }
    }

    public void AddThunder2Round(EntityBase target, int debuffid, float ThunderRatio)
    {
        this.AddThunder2Round(target, debuffid, 6f, ThunderRatio);
    }

    public void AddThunder2Round(EntityBase target, int debuffid, float range, float ThunderRatio)
    {
        List<EntityBase> list = GameLogic.Release.Entity.GetRoundEntities(target, range, false);
        int num = 0;
        int count = list.Count;
        while (num < count)
        {
            EntityBase base2 = list[num];
            float[] args = new float[] { ThunderRatio };
            GameLogic.SendBuffInternal(base2, this, debuffid, args);
            GameLogic.EffectGet("Effect/Attributes/ThunderLine").GetComponent<ThunderLineCtrl>().UpdateEntity(target, base2);
            num++;
        }
        if (list.Count > 0)
        {
            GameLogic.Hold.Sound.PlayBattleSpecial(0x4c4b4a, target.position);
        }
    }

    public void BabiesClone()
    {
        int num = 0;
        int count = this.mBabySkillIds.Count;
        while (num < count)
        {
            this.AddSkillOverLying(this.mBabySkillIds[num], Array.Empty<object>());
            num++;
        }
    }

    public void BattleInGetGoods(int goodid)
    {
        this.GetGoodsInternal(goodid);
    }

    public void BattleInInitSkill(int skillId)
    {
        this.AddSkillInternal(skillId, Array.Empty<object>());
    }

    private void CantTitTargetShow(bool show)
    {
        if (this.CantHitTarget != null)
        {
            this.CantHitTarget.SetActive(show);
        }
        if (this.HitTarget != null)
        {
            this.HitTarget.SetActive(!show);
        }
    }

    public void ChangeHP(EntityBase entity, long HP)
    {
        if (!this.GetIsDead())
        {
            this.ChangeHPMust(entity, HP);
            if (HP < 0f)
            {
                this.UpdateHittedTime();
            }
        }
    }

    public void ChangeHPMust(EntityBase entity, long HP)
    {
        long hp = this.m_EntityData.ChangeHP(entity, HP);
        if (this.Type == EntityType.Boss)
        {
            GameLogic.Hold.BattleData.BossChangeHP(hp);
        }
        if (this.GetIsDead())
        {
            this.DeadCallBack();
        }
        this.OnChangeHP(entity, hp);
        if ((entity != null) && (entity.OnHitAction != null))
        {
            entity.OnHitAction(this, this.HittedDirection);
        }
        if (this.GetIsDead())
        {
            if (this.m_Data.DeadSoundID != 0)
            {
                GameLogic.Hold.Sound.PlayEntityDead(this.m_Data.DeadSoundID, base.transform.position);
            }
            this.SetCollider(false);
            if (entity != null)
            {
                entity.m_EntityData.ExcuteKillAdd();
            }
        }
    }

    public void ChangeWeapon(int WeaponID)
    {
        if (this.m_Weapon != null)
        {
            this.m_Weapon.UnInstall();
            this.m_Weapon = null;
        }
        this.InitWeapon(WeaponID);
    }

    protected virtual void CollisionEnterExtra(Collision o)
    {
    }

    protected virtual void CollisionExitExtra(Collision o)
    {
    }

    protected void CreateHP()
    {
        object[] args = new object[] { "Game/UI/", this.HPSliderName };
        GameObject child = GameLogic.EffectGet(Utils.GetString(args));
        child.SetParentNormal(GameNode.m_HP);
        HpSlider component = child.GetComponent<HpSlider>();
        this.m_HPSlider = component;
    }

    protected void CreateModel()
    {
        string modelID = this.m_Data.ModelID;
        GameObject t = null;
        if (this.IsSelf)
        {
            int num = LocalSave.Instance.Equip_GetCloth();
            if ((num <= 0) || !ResourceManager.TryLoad<GameObject>(this.GetBodyString(num.ToString()), out t))
            {
                t = ResourceManager.Load<GameObject>(this.GetBodyString(modelID));
            }
        }
        else
        {
            t = ResourceManager.Load<GameObject>(this.GetBodyString(modelID));
        }
        GameObject obj3 = Object.Instantiate<GameObject>(t);
        obj3.transform.parent = base.transform;
        obj3.transform.localPosition = new Vector3(0f, 0f, 0f);
        obj3.transform.localRotation = Quaternion.identity;
        obj3.transform.localScale = Vector3.one;
        this.child = obj3;
        this.m_Body = obj3.GetComponent<BodyMask>();
        this.CreateHP();
        if (this.m_AniCtrl != null)
        {
            this.m_AniCtrl.Init(this);
        }
        this.OnCreateModel();
    }

    private EntityPartBodyBase CreatePartBody(int partBodyID, Vector3 pos)
    {
        object[] args = new object[] { "Game/PartBody/PartBodyNode", partBodyID };
        GameObject obj2 = Object.Instantiate<GameObject>(ResourceManager.Load<GameObject>(Utils.GetString(args)));
        obj2.transform.parent = GameNode.m_Battle.transform;
        obj2.transform.localPosition = pos;
        obj2.transform.localScale = Vector3.one;
        obj2.transform.localRotation = Quaternion.identity;
        return obj2.GetComponent<EntityPartBodyBase>();
    }

    public EntityPartBodyBase CreatePartBody(int partbodyid, Vector3 pos, float time)
    {
        PartBodyData data = this.mPartBodyList[partbodyid];
        if (!data.CanAdd())
        {
            return null;
        }
        data.Add();
        EntityPartBodyBase base2 = this.CreatePartBody(partbodyid, pos);
        base2.SetParent(this);
        base2.Init(partbodyid);
        if (time > 0f)
        {
            base2.SetAliveTime(time);
        }
        base2.OnRemoveEvent = new Action<int>(this.OnPartBodyRemove);
        return base2;
    }

    public void CurrentHPUpdate()
    {
    }

    public void DeadBefore()
    {
        this.OnDeadBefore();
    }

    public virtual void DeadCallBack()
    {
        if (this.m_HPSlider != null)
        {
            this.m_HPSlider.DeInit();
        }
        this.m_MoveCtrl.ResetRigidBody();
        this.m_AniCtrl.SendEvent("Dead", false);
        if ((GameLogic.Release.Mode != null) && (GameLogic.Release.Mode.RoomGenerate != null))
        {
            GameLogic.Release.Mode.RoomGenerate.MonsterDead(this);
        }
        this.DeInitLogic();
        this.m_AniCtrl.DeadDown();
        float animationTime = this.m_AniCtrl.GetAnimationTime("Dead");
        this.mDeadSeq = TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(DOTween.Sequence(), animationTime * 0.8f), new TweenCallback(this, this.<DeadCallBack>m__0));
    }

    public void DeInit()
    {
        this.DeInitLogic();
        this.DeInitMesh(false);
        this.OnDeInit();
    }

    protected void DeInitLogic()
    {
        if (this.Event_DeInit != null)
        {
            this.Event_DeInit();
        }
        GameLogic.Release.Entity.RemoveLogic(this);
        this.m_AttackCtrl.DeInit();
        this.m_MoveCtrl.DeInit();
        this.m_HitEdit.DeInit();
        this.mAction.DeInit();
        this.m_Body.DeInit();
        if (this.m_HPSlider != null)
        {
            this.m_HPSlider.DeInit();
        }
        if (this.m_Weapon != null)
        {
            this.m_Weapon.UnInstall();
            this.m_Weapon = null;
        }
        this.m_EntityData.DeInit();
        this.m_MoveCtrl.ResetRigidBody();
        if (this.mRotateAttribute != null)
        {
            this.mRotateAttribute.DeInit();
        }
        if (this.mRotateShield != null)
        {
            this.mRotateShield.DeInit();
        }
        this.SetCollider(false);
        this.RemoveColliders();
        this.RemoveControllers();
        this.UnInstallAllSkills();
        this.RemoveDivideUpdate();
        this.OnDeInitLogic();
    }

    protected void DeInitMesh(bool showeffect)
    {
        if (showeffect && (this.m_Body != null))
        {
            GameObject obj2 = GameLogic.Release.MapEffect.Get("Effect/Battle/eff_die");
            if (obj2 != null)
            {
                obj2.transform.position = this.m_Body.DeadNode.transform.position;
            }
        }
        if (this.m_AniCtrl != null)
        {
            this.m_AniCtrl.DeInit();
        }
        if (this.mDeadSeq != null)
        {
            TweenExtensions.Kill(this.mDeadSeq, false);
            this.mDeadSeq = null;
        }
        if (this.m_Body != null)
        {
            this.m_Body.CacheEffect();
        }
        if ((this.child != null) && (this.m_Data != null))
        {
            GameLogic.EntityCache(this.child, this.m_Data.Cache);
            this.child = null;
        }
        if ((this != null) && (base.gameObject != null))
        {
            Object.Destroy(base.gameObject);
        }
    }

    public void DivideAction(float x, float z)
    {
        this.dividemove = new Vector3(x, 0f, z);
        Vector2Int roomXYInside = GameLogic.Release.MapCreatorCtrl.GetRoomXYInside(base.transform.position + this.dividemove);
        Vector3 worldPosition = GameLogic.Release.MapCreatorCtrl.GetWorldPosition(roomXYInside);
        this.dividemove = worldPosition - base.transform.position;
        object[] args = new object[] { this.m_EntityData.CharID };
        Updater.AddUpdate(Utils.FormatString("{0}.DivideAction", args), new Action<float>(this.OnDivideUpdate), false);
    }

    public void ExcuteCommend(EBattleAction action, object data)
    {
        int num = 0;
        int count = this.ctrlsList.Count;
        while (num < count)
        {
            if (this.ctrlsList.Count > num)
            {
                this.ctrlsList[num].ExcuteCommend(action, data);
            }
            num++;
        }
    }

    private void FixedUpdate()
    {
        if (!GameLogic.Paused)
        {
            this.UpdateFixed();
        }
    }

    public long GetBabyArgs(int index)
    {
        if ((index >= 0) && (index < this.mBabyArgs.Count))
        {
            return this.mBabyArgs[index];
        }
        object[] args = new object[] { index };
        SdkManager.Bugly_Report("EntityBase_Baby", Utils.FormatString("GetBabyArgs[{0}] is out of range.", args));
        return 0L;
    }

    public int GetBabyGroundIndex()
    {
        int mBabyGroundIndex = this.mBabyGroundIndex;
        this.mBabyGroundIndex++;
        return mBabyGroundIndex;
    }

    public Vector3 GetBabyGroundPos(int index)
    {
        if (index < 0)
        {
            index = 0;
        }
        index = index % this.mBabyGroundPos.Count;
        return this.mBabyGroundPos[index];
    }

    private string GetBodyString(string value)
    {
        object[] args = new object[] { value };
        return Utils.FormatString("Game/Models/{0}", args);
    }

    protected virtual long GetBossHP()
    {
        object[] args = new object[] { this.m_Data.CharID };
        throw new Exception(Utils.FormatString("Entity {0} not achieve [GetBossHP]", args));
    }

    public Transform GetBulletCreateNode(int index)
    {
        if (index >= 10)
        {
            return this.m_Body.BulletList[index - 10].transform;
        }
        if (index != 1)
        {
            if (index == 2)
            {
                return base.transform;
            }
            return this.m_Body.LeftBullet.transform;
        }
        return this.m_Body.FootMask.transform;
    }

    private bool GetCanHitted() => 
        (((Updater.AliveTime - this.HittedLastTime) >= this.m_EntityData.HittedInterval) && !this.m_EntityData.GetInvincible());

    protected virtual bool GetCanPositionBy() => 
        true;

    public bool GetColliderEnable() => 
        this.mHitCtrl.GetColliderEnable();

    public float GetColliderHeight() => 
        this.mHitCtrl.GetColliderHeight();

    public float GetCollidersSize() => 
        this.mHitCtrl.GetCollidersSize();

    protected bool GetColliderTrigger() => 
        this.mHitCtrl.GetColliderTrigger();

    public List<int> GetDebuffList() => 
        this.DebuffList;

    public GameObject GetEffect(int fxId) => 
        this.PlayEffect(fxId);

    public bool GetFlying() => 
        this.bFlyStone;

    public void GetGoods(int goodid)
    {
        this.GetGoodsInternal(goodid);
        LocalSave.Instance.BattleIn_UpdateGood(goodid);
    }

    private void GetGoodsInternal(int goodid)
    {
        LocalModelManager.Instance.Goods_food.GetBeanById(goodid).GetGoods(this);
    }

    public HittedData GetHittedData(BulletBase bullet) => 
        this.GetHittedData(bullet.m_Data.bThroughWall, bullet.transform.eulerAngles.y);

    public HittedData GetHittedData(bool bulletthrough, float bulletangle)
    {
        HittedData data = new HittedData {
            type = EHittedType.eNormal
        };
        if (!this.GetCanHitted())
        {
            data.type = EHittedType.eInvincible;
            return data;
        }
        return this.OnHittedData(data, bulletthrough, bulletangle);
    }

    public Vector3 GetHittedDirection() => 
        this.HittedDirection;

    public virtual Transform GetHittedMask()
    {
        object[] args = new object[] { "EntityBase ", this.m_Type, " don't achieve GetHittedTransform" };
        SdkManager.Bugly_Report("EntityBase.cs", Utils.GetString(args));
        return null;
    }

    public bool GetIsDead()
    {
        if (this.m_EntityData != null)
        {
            return (this.m_EntityData.CurrentHP <= 0L);
        }
        return true;
    }

    public bool GetIsInCamera() => 
        this.m_Body.GetIsInCamera();

    public Transform GetKetNode(int index)
    {
        switch (index)
        {
            case 1:
                return this.m_Body.Body.transform;

            case 2:
            case 8:
                return this.m_Body.EffectMask.transform;

            case 3:
                return this.m_Body.HPMask.transform;

            case 4:
                return this.m_Body.FootMask.transform;

            case 5:
                return this.m_Body.HeadMask.transform;

            case 6:
                return this.m_Body.BulletHitMask.transform;

            case 7:
                return this.m_HPSlider?.transform;

            case 9:
                return base.transform;
        }
        return null;
    }

    public bool GetMeshShow() => 
        (this.showmeshcount > 0);

    private Vector3 GetMoveDistance(Vector3 pos)
    {
        if (!this.IsSelf)
        {
            this.move_layermask = !this.GetFlying() ? LayerManager.Move_Ground : LayerManager.Move_Fly;
            this.move_offset = 0.1f;
            this.move_dis = pos.magnitude + this.move_offset;
            this.move_hits = Physics.CapsuleCastAll((this.position + ((this.move_vec * (this.mHitCtrl.m_CapsuleCollider.height - 1f)) / 2f)) - (pos.normalized * this.move_offset), (this.position - ((this.move_vec * (this.mHitCtrl.m_CapsuleCollider.height - 1f)) / 2f)) - (pos.normalized * this.move_offset), ((base.transform.localScale.x * this.mHitCtrl.m_CapsuleCollider.radius) - this.move_offset) - 0.02f, pos, this.move_dis, this.move_layermask);
            int index = 0;
            int length = this.move_hits.Length;
            while (index < length)
            {
                this.move_hit = this.move_hits[index];
                if (this.State == EntityState.Hitted)
                {
                    return (pos.normalized * (this.move_hit.distance - this.move_offset));
                }
                float introduced7 = MathDxx.Abs(this.move_hit.normal.x);
                if (introduced7 > MathDxx.Abs(this.move_hit.normal.z))
                {
                    pos.x = 0f;
                }
                else
                {
                    float introduced8 = MathDxx.Abs(this.move_hit.normal.x);
                    if (introduced8 < MathDxx.Abs(this.move_hit.normal.z))
                    {
                        pos.z = 0f;
                    }
                    else if (MathDxx.Abs(pos.x) > MathDxx.Abs(pos.z))
                    {
                        pos.z = 0f;
                    }
                    else
                    {
                        pos.x = 0f;
                    }
                }
                index++;
            }
        }
        return pos;
    }

    public int GetRotateFollowIndex(int key)
    {
        int num = 0;
        if (this.mRotateIndexList.TryGetValue(key, out num))
        {
            return num;
        }
        return num;
    }

    public Vector3 GetRotateFollowPosition(EntityBase entity)
    {
        int charID = entity.m_Data.CharID;
        if (this.mRotateFollowList.TryGetValue(charID, out RotateFollowData data))
        {
            return data.GetPosition(entity);
        }
        return this.position;
    }

    public List<int> GetSkillList() => 
        this.skillidList;

    public bool GetSuperArmor() => 
        (this.mSuperArmor > 0);

    public bool GetTrigger() => 
        this.mHitCtrl.GetTrigger();

    public void Init(int id)
    {
        this.ClassID = id;
        this.m_Data = LocalModelManager.Instance.Character_Char.GetBeanById(this.ClassID);
        this.UpdateName();
        if (this.m_Data.Speed == 0)
        {
            this.rigid.constraints = RigidbodyConstraints.FreezeAll;
        }
        if (this.m_Type == EntityType.Invalid)
        {
            this.SetEntityType((EntityType) this.m_Data.TypeID);
        }
        this.OnInitBefore();
        this.m_HitEdit = base.GetComponent<HitEdit>();
        SdkManager.Bugly_Report(this.m_HitEdit != null, this.ClassName, " dont have HitEdit!!!");
        this.m_EntityData = new EntityData();
        this.m_AniCtrl = new AnimatorBase();
        this.mHitCtrl = new EntityHitCtrl();
        this.mHitCtrl.Init(this);
        this.AddController<EntityLifeCtrl>();
        this.AddController<BuffCtrl>();
        this.mAction.Init(false);
        this.SetPosition(base.transform.position);
        this.InitCharacter();
        this.CreateModel();
        this.OnInit();
        this.OnInitAfter();
        this.MissBossHP();
    }

    private void InitBossHP()
    {
        if (((this.Type == EntityType.Boss) && !this.bDivide) && !this.bCall)
        {
            long bossHP = this.GetBossHP();
            GameLogic.Hold.BattleData.AddBossMaxHP(bossHP);
        }
    }

    protected virtual void InitCharacter()
    {
        this.m_EntityData.Init(this, this.ClassID);
    }

    public virtual void InitWeapon(int WeaponID)
    {
        string typeName = "Weapon" + WeaponID;
        System.Type type = System.Type.GetType(typeName);
        WeaponBase base2 = null;
        if (type == null)
        {
            base2 = new WeaponBase();
        }
        else
        {
            base2 = type.Assembly.CreateInstance(typeName) as WeaponBase;
        }
        this.m_Weapon = base2;
        base2.Init(this, WeaponID);
    }

    private void MissBossHP()
    {
        if ((((this.Type == EntityType.Boss) && (this.m_Data != null)) && ((this.m_Data.Divide == 0) && (this.DivideID == string.Empty))) && !this.bDivide)
        {
            this.ShowHP(false);
        }
    }

    protected virtual void OnChangeHP(EntityBase entity, long HP)
    {
    }

    private void OnCollisionEnter(Collision o)
    {
        this.CollisionEnterExtra(o);
    }

    private void OnCollisionExit(Collision o)
    {
        this.CollisionExitExtra(o);
    }

    protected virtual void OnCreateModel()
    {
    }

    protected virtual void OnDeadBefore()
    {
        this.RemoveController<BuffCtrl>();
        this.RemoveMove();
    }

    protected virtual void OnDeInit()
    {
    }

    protected virtual void OnDeInitLogic()
    {
    }

    private void OnDivideUpdate(float delta)
    {
        this.SetPositionBy(this.dividemove / ((float) this.divide_maxframe));
        this.divide_frame++;
        if (this.divide_frame == this.divide_maxframe)
        {
            this.RemoveDivideUpdate();
        }
    }

    protected virtual List<BattleDropData> OnGetGoodList() => 
        new List<BattleDropData>();

    protected virtual HittedData OnHittedData(HittedData data, bool bulletthrough, float bulletangle) => 
        data;

    protected virtual void OnInit()
    {
    }

    private void OnInitAfter()
    {
        if (this.m_MoveCtrl != null)
        {
            this.m_MoveCtrl.Start();
        }
        if (this.m_AttackCtrl != null)
        {
            this.m_AttackCtrl.Start();
        }
        if (this.m_HitEdit != null)
        {
            this.m_HitEdit.Init(this);
        }
        if (this.m_HPSlider != null)
        {
            this.m_HPSlider.Init(this);
        }
        this.AddInitSkills();
        this.AliveTime = Updater.AliveTime;
        this.InitBossHP();
        this.m_Body.SetEntity(this);
        this.m_EntityData.InitAfter();
        this.StartInit();
    }

    protected virtual void OnInitBefore()
    {
    }

    private void OnPartBodyRemove(int partbodyid)
    {
        this.mPartBodyList[partbodyid].Remove();
    }

    protected virtual void OnSetFlying(bool fly)
    {
    }

    protected virtual void OnSetPositionBy(Vector3 pos)
    {
    }

    private void OnTriggerEnter(Collider o)
    {
        this.OnTriggerEnterExtra(o);
    }

    protected virtual void OnTriggerEnterExtra(Collider o)
    {
    }

    private void OnTriggerExit(Collider o)
    {
        this.OnTriggerExitExtra(o);
    }

    protected virtual void OnTriggerExitExtra(Collider o)
    {
    }

    public void PlayAttack()
    {
        this.m_AniCtrl.SendEvent("AttackEnd", false);
        if (this.m_Weapon != null)
        {
            this.m_Weapon.Attack(Array.Empty<object>());
        }
    }

    public GameObject PlayEffect(int fxId) => 
        this.PlayEffect(fxId, Vector3.zero, Quaternion.identity);

    public GameObject PlayEffect(int fxId, Vector3 pos) => 
        this.PlayEffect(fxId, pos, Quaternion.identity);

    public GameObject PlayEffect(int fxId, Vector3 pos, Quaternion rota)
    {
        Fx_fx beanById = LocalModelManager.Instance.Fx_fx.GetBeanById(fxId);
        if (beanById == null)
        {
            object[] args = new object[] { fxId };
            SdkManager.Bugly_Report("EntityBase_Ctrl", Utils.FormatString("PlayEffect[{0}] is null.", args));
        }
        Transform transform = GameLogic.Release.MapEffect.Get(beanById.Path).transform;
        Transform ketNode = this.GetKetNode(beanById.Node);
        if (ketNode == null)
        {
            transform.SetParent(GameNode.m_PoolParent);
            transform.position = pos;
        }
        else if (beanById.Node == 8)
        {
            transform.SetParent(this.m_Body.EffectMask.transform);
            transform.localScale = Vector3.one;
            transform.localPosition = Vector3.zero;
            transform.SetParent(GameNode.m_PoolParent);
        }
        else
        {
            transform.SetParent(ketNode);
            transform.localScale = Vector3.one;
            transform.localPosition = Vector3.zero;
        }
        transform.rotation = rota;
        return transform.gameObject;
    }

    public void PlayHittedSound()
    {
        int hittedEffectID = this.m_Data.HittedEffectID;
        if (hittedEffectID != 0)
        {
            GameLogic.Hold.Sound.PlayHitted(hittedEffectID, this.position, -1f);
        }
    }

    public void PlaySound(int soundid)
    {
        if (soundid != 0)
        {
            GameLogic.Hold.Sound.PlayHitted(soundid, this.position, -1f);
        }
    }

    public void RemoveBabySkillID(int id)
    {
        this.mBabySkillIds.Remove(id);
    }

    public void RemoveColliders()
    {
        this.mHitCtrl.RemoveColliders();
    }

    public void RemoveController<T>() where T: EntityCtrlBase
    {
        for (int i = this.ctrlsList.Count - 1; i >= 0; i--)
        {
            EntityCtrlBase base2 = this.ctrlsList[i];
            if (base2 is T)
            {
                if (base2.UseUpdate)
                {
                    object[] args = new object[] { this.m_EntityData.CharID, base2.GetType().ToString() };
                    Updater.RemoveUpdate(Utils.FormatString("{0}.AddController<{1}>", args), new Action<float>(base2.OnUpdate));
                }
                base2.OnRemove();
                this.ctrlsList.RemoveAt(i);
                break;
            }
        }
    }

    public void RemoveControllers()
    {
        for (int i = this.ctrlsList.Count - 1; i >= 0; i--)
        {
            EntityCtrlBase base2 = this.ctrlsList[i];
            if (base2.UseUpdate)
            {
                object[] args = new object[] { this.m_EntityData.CharID, base2.GetType().ToString() };
                Updater.RemoveUpdate(Utils.FormatString("{0}.AddController<{1}>", args), new Action<float>(base2.OnUpdate));
            }
            base2.OnRemove();
        }
        this.ctrlsList.Clear();
    }

    public void RemoveDebuff(int debuffid)
    {
        this.DebuffList.Remove(debuffid);
    }

    private void RemoveDivideUpdate()
    {
        object[] args = new object[] { this.m_EntityData.CharID };
        Updater.RemoveUpdate(Utils.FormatString("{0}.DivideAction", args), new Action<float>(this.OnDivideUpdate));
    }

    public virtual void RemoveMove()
    {
    }

    public void RemoveRotateAttribute(GameObject o)
    {
        if (this.mRotateAttribute != null)
        {
            this.mRotateAttribute.Remove(o);
        }
    }

    public void RemoveRotateFollow(EntityBase entity)
    {
        int charID = entity.m_Data.CharID;
        if (this.mRotateFollowList.TryGetValue(charID, out RotateFollowData data))
        {
            data.Remove(entity);
        }
    }

    public void RemoveRotateShield(GameObject o)
    {
        if (this.mRotateShield != null)
        {
            this.mRotateShield.Remove(o);
        }
    }

    public void RemoveRotateSword(GameObject o)
    {
        if (this.mRotateSword != null)
        {
            this.mRotateSword.Remove(o);
        }
    }

    public void RemoveSkill(int skillId)
    {
        if (this.skillsList.TryGetValue(skillId, out SkillBase base2))
        {
            base2.Uninstall();
            this.skillsList.Remove(skillId);
            this.skillidList.Remove(skillId);
        }
    }

    public void SelfMoveBy(Vector3 pos)
    {
        if (!this.m_EntityData.IsDizzy())
        {
            this.SetPositionBy(pos);
        }
    }

    public void SetBabyArgs(long value)
    {
        this.mBabyArgs.Add(value);
    }

    public void SetBodyScale(float value)
    {
        this.mHitCtrl.SetBodyScale(value);
        this.m_Body.SetBodyScale(value);
    }

    public void SetCallID(int callid, Vector3 endpos)
    {
        this.mCallID = callid;
        this.mCallEndPos = endpos;
    }

    public void SetCanHit(bool value)
    {
        if (this.bCanHit != value)
        {
            this.bCanHit = value;
            this.CantTitTargetShow(!this.bCanHit);
        }
    }

    public virtual void SetCollider(bool enable)
    {
        this.collidercount += !enable ? -1 : 1;
        this.mHitCtrl.SetCollider(this.collidercount > 0);
        this.SetFlyStone(this.bFlyStone);
        this.SetFlyWater(this.bFlyWater);
    }

    public void SetCollidersScale(float scale)
    {
        this.mHitCtrl.SetCollidersScale(scale);
    }

    public void SetElite(bool value)
    {
        this.m_bElite = value;
    }

    public void SetEntityDivide(RoomGenerateBase.RoomType type)
    {
        if (type == RoomGenerateBase.RoomType.eBoss)
        {
            this.SetEntityType(EntityType.Boss);
        }
    }

    public void SetEntityType(EntityType type)
    {
        this.m_Type = type;
        this.UpdateName();
    }

    public void SetEulerAngles(Vector3 e)
    {
        this._eulerAngles = e;
    }

    public void SetFlying(bool fly)
    {
        this.SetFlyWater(fly);
        this.SetFlyStone(fly);
        this.OnSetFlying(fly);
    }

    private void SetFlyOne(string layer, bool fly)
    {
        this.mHitCtrl.SetFlyOne(layer, fly);
    }

    public void SetFlyStone(bool fly)
    {
        this.bFlyStone = fly;
        this.SetFlyOne("Entity2Stone", fly);
        this.m_Body.SetFlyStone(fly);
        if (this.m_Weapon != null)
        {
            this.m_Weapon.SetFlying(fly);
        }
        this.OnSetFlying(fly);
    }

    public void SetFlyWater(bool fly)
    {
        this.bFlyWater = fly;
        this.SetFlyOne("Entity2Water", fly);
    }

    public virtual bool SetHitted(HittedData data)
    {
        if (this.GetSuperArmor())
        {
            return false;
        }
        this.HittedAngle = data.angle;
        this.HittedX = MathDxx.Sin(this.HittedAngle) * data.backtatio;
        this.HittedY = MathDxx.Cos(this.HittedAngle) * data.backtatio;
        this.HittedDirection = new Vector3(this.HittedX, 0f, this.HittedY);
        if (data.GetPlayHitted())
        {
            this.m_AniCtrl.SendEvent("Hitted", false);
            if (!this.IsSelf)
            {
                this.m_Body.Hitted(this.HittedDirection, data.hittype);
            }
        }
        return true;
    }

    public void SetObstacleCollider(bool value)
    {
        if (!value)
        {
            this.SetFlyStone(true);
            this.SetFlyWater(true);
        }
        else
        {
            this.SetFlyStone(this.bFlyStone);
            this.SetFlyWater(this.bFlyWater);
        }
    }

    public void SetPosition(Vector3 pos)
    {
        if ((this != null) && (base.transform != null))
        {
            base.transform.position = pos;
            this._position = pos;
        }
    }

    public void SetPositionBy(Vector3 pos)
    {
        if (((this != null) && (base.transform != null)) && this.GetCanPositionBy())
        {
            this.SetPositionBy_P = this.GetMoveDistance(pos);
            this.SetPositionByInternal(this.SetPositionBy_P);
        }
    }

    protected void SetPositionByInternal(Vector3 pos)
    {
        base.transform.Translate(pos);
        if (this.Event_PositionBy != null)
        {
            this.Event_PositionBy(pos);
        }
        this.OnSetPositionBy(pos);
        this._position = base.transform.position;
    }

    public void SetRoomType(RoomGenerateBase.RoomType type)
    {
        if ((type == RoomGenerateBase.RoomType.eBoss) && !this.bCall)
        {
            this.SetEntityType(EntityType.Boss);
        }
    }

    public void SetRotateFollowIndex(int key, int index)
    {
        if (!this.mRotateIndexList.ContainsKey(key))
        {
            this.mRotateIndexList.Add(key, index);
        }
        else
        {
            this.mRotateIndexList[key] = index;
        }
    }

    public void SetSuperArmor(bool value)
    {
        this.mSuperArmor += !value ? -1 : 1;
    }

    public void SetTrigger(bool value)
    {
        this.mHitCtrl.SetTrigger(value);
    }

    private void ShowChallengeEntity(bool show)
    {
        if (this.showchallenge != show)
        {
            this.showchallenge = show;
            this.ShowEntity(show);
        }
    }

    public void ShowEntity(bool show)
    {
        this.ShowHP(show);
        this.ShowMesh(show);
        this.SetCollider(show);
    }

    public void ShowHP(bool show)
    {
        this.showhpcount += !show ? -1 : 1;
        if (this.m_HPSlider != null)
        {
            this.m_HPSlider.ShowHP(this.showhpcount > 0);
        }
    }

    public void ShowMesh(bool show)
    {
        if (this.m_Body != null)
        {
            this.showmeshcount += !show ? -1 : 1;
            if (this.childs == null)
            {
                this.childs = this.m_Body.transform.GetComponentsInChildren<Transform>(true);
            }
            int layer = (this.showmeshcount <= 0) ? LayerManager.Hide : LayerManager.Player;
            this.m_Body.transform.ChangeChildLayer(layer);
        }
    }

    protected void StartDeadOffSet()
    {
        this.Dead_bPlay = true;
    }

    protected virtual void StartInit()
    {
    }

    private void UnInstallAllSkills()
    {
        Dictionary<int, SkillBase>.Enumerator enumerator = this.skillsList.GetEnumerator();
        while (enumerator.MoveNext())
        {
            KeyValuePair<int, SkillBase> current = enumerator.Current;
            current.Value.Uninstall();
        }
        Dictionary<int, SkillBase>.Enumerator enumerator2 = this.skillsAutoList.GetEnumerator();
        while (enumerator2.MoveNext())
        {
            KeyValuePair<int, SkillBase> current = enumerator2.Current;
            current.Value.Uninstall();
        }
        int num = 0;
        int count = this.skillsOverlyingList.Count;
        while (num < count)
        {
            this.skillsOverlyingList[num].Uninstall();
            num++;
        }
        int num3 = 0;
        int num4 = this.skillsAttributeList.Count;
        while (num3 < num4)
        {
            this.skillsAttributeList[num3].Uninstall();
            num3++;
        }
        this.skillsAttributeList.Clear();
        this.skillsOverlyingList.Clear();
        this.skillsList.Clear();
        this.skillidList.Clear();
        this.skillsAutoList.Clear();
    }

    private void Update()
    {
        if (!GameLogic.Paused)
        {
            this.UpdateProcess(Updater.delta);
        }
    }

    private void UpdateDead()
    {
        if (this.Dead_bPlay)
        {
            this.Dead_CurrentCount++;
            if (this.Dead_CurrentCount == this.Dead_PlayCount)
            {
                this.Dead_bPlay = false;
            }
            base.transform.Translate((new Vector3(this.HittedX, 0f, this.HittedY) * (this.Dead_PlayCount - this.Dead_CurrentCount)) * 0.02f);
        }
    }

    protected virtual void UpdateFixed()
    {
    }

    public virtual void UpdateHittedTime()
    {
        this.HittedLastTime = Updater.AliveTime;
    }

    private void UpdateName()
    {
        if (this.m_Data != null)
        {
            object[] args = new object[] { "Entity_", this.m_Type.ToString(), "_", this.m_Data.CharID, "_", GameLogic.Random(0x3e8, 0x270f) };
            base.name = Utils.GetString(args);
        }
        else
        {
            object[] args = new object[] { "Entity_", this.m_Type.ToString(), "_", this._name, "_", GameLogic.Random(0x3e8, 0x270f) };
            base.name = Utils.GetString(args);
        }
    }

    protected virtual void UpdateProcess(float delta)
    {
        if (this.m_EntityData != null)
        {
            if (!this.m_EntityData.IsDizzy())
            {
                this.AliveTime += Updater.delta;
            }
            if (((GameLogic.Self != null) && GameLogic.Hold.BattleData.Challenge_MonsterHide()) && !this.IsSelf)
            {
                this.ShowChallengeEntity(Vector3.Distance(this.position, GameLogic.Self.position) <= GameLogic.Hold.BattleData.Challenge_MonsterHideRange());
            }
            this.UpdateDead();
        }
    }

    public void WeaponHandShow(bool show)
    {
        if (this.m_WeaponHand != null)
        {
            this.m_WeaponHand.SetActive(show);
        }
    }

    public void WeaponHandUpdate()
    {
        if (this.m_WeaponHand != null)
        {
            Object.Destroy(this.m_WeaponHand);
        }
        if ((this.m_Weapon != null) && (this.m_Weapon.m_Data != null))
        {
            object[] args = new object[] { "Game/WeaponHand/WeaponHand", this.m_Weapon.m_Data.WeaponID };
            GameObject obj2 = GameLogic.EffectGet(Utils.GetString(args));
            if (obj2 != null)
            {
                obj2.transform.parent = WeaponBase.GetWeaponNode(this.m_Body, this.m_Weapon.m_Data.WeaponNode);
                obj2.transform.localPosition = Vector3.zero;
                obj2.transform.localScale = Vector3.one;
                obj2.transform.localRotation = Quaternion.identity;
                this.m_WeaponHand = obj2;
                MeshRenderer[] componentsInChildren = this.m_WeaponHand.GetComponentsInChildren<MeshRenderer>(true);
                this.m_WeaponHand.ChangeChildLayer(LayerManager.Player);
            }
        }
    }

    protected virtual string ModelPath =>
        "Game/Player/player";

    public GameObject Child =>
        this.child;

    public bool IsElite =>
        this.m_bElite;

    public EntityState State =>
        this.m_State;

    public EntityBase m_HatredTarget
    {
        get
        {
            if ((this.m_HatredTargetP == null) || ((this.m_HatredTargetP.GetIsDead() || this.m_HatredTargetP.IsSelf) && !this.m_HatredTargetP.IsSelf))
            {
                this.m_HatredTargetP = GameLogic.FindTarget(this);
            }
            return this.m_HatredTargetP;
        }
        set
        {
            if (this.IsSelf || ((value != null) && !value.GetIsDead()))
            {
                this.m_HatredTargetP = value;
            }
        }
    }

    public int CallID =>
        this.mCallID;

    public Vector3 CallEndPos =>
        this.mCallEndPos;

    public bool IsSelf =>
        (((GameLogic.Release != null) && (GameLogic.Release.Entity != null)) && (this == GameLogic.Self));

    public Vector3 position
    {
        get
        {
            if ((this != null) && (base.transform != null))
            {
                return base.transform.position;
            }
            return this._position;
        }
    }

    public Vector3 eulerAngles =>
        this._eulerAngles;

    private Rigidbody rigid
    {
        get
        {
            if (this._rigid == null)
            {
                this._rigid = base.GetComponent<Rigidbody>();
            }
            return this._rigid;
        }
    }

    private string _name
    {
        get
        {
            if (this._namep == string.Empty)
            {
                this._namep = base.gameObject.name;
            }
            return this._namep;
        }
    }

    public string DivideID
    {
        get => 
            this.mDivideID;
        set
        {
            if (this.mDivideID == string.Empty)
            {
                this.mDivideID = value;
                EntityManager.DivideTransfer transfer = new EntityManager.DivideTransfer {
                    divedeid = this.mDivideID,
                    charid = this.m_Data.CharID,
                    entitytype = this.Type
                };
                GameLogic.Release.Entity.AddDivide(this.mDivideID, transfer);
            }
        }
    }

    public EntityType Type
    {
        get
        {
            if (this.m_Type == EntityType.Invalid)
            {
                object[] args = new object[] { "EntityType Invalid ", base.name };
                SdkManager.Bugly_Report("EntityBase.cs", Utils.GetString(args));
            }
            return this.m_Type;
        }
    }

    public class PartBodyData
    {
        public int ID;
        public int alivecount;
        public int maxcount;

        public void Add()
        {
            this.alivecount++;
        }

        public bool CanAdd() => 
            (this.alivecount < this.maxcount);

        public void Remove()
        {
            this.alivecount--;
        }
    }

    protected class RotateBallClass : EntityBase.RotateClass
    {
        public float rangemin;
        public float rangemax;
        private Dictionary<Transform, List<Transform>> mList = new Dictionary<Transform, List<Transform>>();
        private float angle;
        private float radius;

        private Transform GetOne(Transform t, int index)
        {
            if (!this.mList.TryGetValue(t, out List<Transform> list))
            {
                list = new List<Transform>();
                this.mList.Add(t, list);
            }
            if (list.Count > index)
            {
                return list[index];
            }
            Transform item = t.Find(index.ToString());
            list.Add(item);
            return item;
        }

        protected override void OnAddorMove()
        {
            for (int i = 0; i < base.mRotateAttrList.Count; i++)
            {
                Transform t = base.mRotateAttrList[i];
                Transform one = this.GetOne(t, 0);
                one.localPosition = new Vector3(this.radius, one.localPosition.y, one.localPosition.z);
                Transform transform3 = this.GetOne(t, 1);
                transform3.localPosition = new Vector3(-this.radius, transform3.localPosition.y, transform3.localPosition.z);
            }
        }

        public void SetRadius(float radius)
        {
            this.radius = radius;
        }
    }

    protected class RotateClass
    {
        public EntityBase parent;
        public string name;
        public float rotate;
        public float allangle;
        protected float time;
        private Transform RotateAttribute;
        protected List<Transform> mRotateAttrList = new List<Transform>();

        public void AddNewRotateAttribute(GameObject o)
        {
            this.mRotateAttrList.Add(o.transform);
            o.transform.SetParent(this.RotateAttribute);
            o.transform.localScale = Vector3.one;
            o.transform.localPosition = Vector3.zero;
            this.RotateAttributeUpdatePosition();
        }

        public void DeInit()
        {
            object[] args = new object[] { this.parent.m_EntityData.CharID };
            Updater.RemoveUpdate(Utils.FormatString("{0}.RotateClass", args), new Action<float>(this.OnRotateAttributeUpdate));
        }

        public void Init(EntityBase parent, string name, float rotate, float allangle)
        {
            this.parent = parent;
            this.name = name;
            this.rotate = rotate;
            this.allangle = allangle;
            this.time = 0f;
            this.RotateAttribute = new GameObject(name).transform;
            this.RotateAttribute.SetParent(parent.transform);
            this.RotateAttribute.localPosition = Vector3.zero;
            this.RotateAttribute.localRotation = Quaternion.identity;
            object[] args = new object[] { parent.m_EntityData.CharID };
            Updater.AddUpdate(Utils.FormatString("{0}.RotateClass", args), new Action<float>(this.OnRotateAttributeUpdate), false);
        }

        protected virtual void OnAddorMove()
        {
        }

        private void OnRotateAttributeUpdate(float delta)
        {
            if (this.RotateAttribute != null)
            {
                this.RotateAttribute.localRotation = Quaternion.Euler(0f, this.RotateAttribute.localEulerAngles.y + this.rotate, 0f);
            }
        }

        public void Remove(GameObject o)
        {
            if (o != null)
            {
                this.mRotateAttrList.Remove(o.transform);
                GameLogic.EffectCache(o);
                this.RotateAttributeUpdatePosition();
            }
        }

        private void RotateAttributeUpdatePosition()
        {
            int count = this.mRotateAttrList.Count;
            if (count > 0)
            {
                float num2 = this.allangle / ((float) count);
                for (int i = 0; i < count; i++)
                {
                    this.mRotateAttrList[i].localRotation = Quaternion.Euler(0f, num2 * i, 0f);
                }
            }
            this.OnAddorMove();
        }
    }

    public class RotateFollowData
    {
        private int name;
        public EntityBase parent;
        public float rotate;
        private float currentrotate;
        private float range;
        private List<EntityBase> mList = new List<EntityBase>();
        private GameObject test;

        public RotateFollowData(int name)
        {
            this.name = name;
        }

        public void Add(EntityBase entity)
        {
            if (!this.mList.Contains(entity))
            {
                this.mList.Add(entity);
                this.UpdateEntities();
            }
        }

        public void DeInit()
        {
            object[] args = new object[] { this.parent.m_EntityData.CharID };
            Updater.RemoveUpdate(Utils.FormatString("{0}.RotateFollowData", args), new Action<float>(this.OnUpdate));
        }

        public Vector3 GetPosition(EntityBase entity)
        {
            float angle = ((360f / ((float) this.mList.Count)) % 360f) * entity.GetRotateFollowIndex(this.name);
            angle += this.currentrotate;
            float x = MathDxx.Sin(angle) * this.range;
            float z = MathDxx.Cos(angle) * this.range;
            return (this.parent.position + new Vector3(x, 0f, z));
        }

        public void Init(EntityBase parent, float rotate, float range)
        {
            this.parent = parent;
            this.rotate = rotate;
            this.range = range;
            this.currentrotate = 0f;
            object[] args = new object[] { parent.m_EntityData.CharID };
            Updater.AddUpdate(Utils.FormatString("{0}.RotateFollowData", args), new Action<float>(this.OnUpdate), false);
        }

        private void OnUpdate(float delta)
        {
            this.currentrotate += this.rotate;
            this.currentrotate = this.currentrotate % 360f;
        }

        public void Remove(EntityBase entity)
        {
            if (this.mList.Contains(entity))
            {
                this.mList.Remove(entity);
                this.UpdateEntities();
            }
        }

        private void UpdateEntities()
        {
            int index = 0;
            int count = this.mList.Count;
            while (index < count)
            {
                this.mList[index].SetRotateFollowIndex(this.name, index);
                index++;
            }
        }
    }
}

