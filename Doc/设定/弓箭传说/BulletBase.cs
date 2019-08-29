using DG.Tweening;
using Dxx.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using TableTool;
using UnityEngine;

public class BulletBase : PauseObject
{
    public Action OnBulletCache;
    protected Transform mTransform;
    protected GameObject mGameObject;
    public const float g = 9.8f;
    private int BulletID;
    protected string ClassName;
    protected bool bInit;
    protected bool bbMoveEnable = true;
    protected bool bFlyRotate = true;
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private EntityBase <m_Entity>k__BackingField;
    public Weapon_weapon m_Data;
    public BulletTransmit mBulletTransmit;
    private bool bBoxEnable = true;
    protected BoxCollider[] boxList;
    protected int boxListCount;
    protected SphereCollider[] sphereList;
    protected int sphereListCount;
    protected CapsuleCollider[] capsuleList;
    protected int capsuleListCount;
    private int CurrentFrameCount;
    private GameObject AttackSoundObj;
    private GameObject trailattrobj;
    private GameObject headattrobj;
    protected Action OnHitSelf;
    protected Action<Collider> HitWallAction;
    private Sequence seq_flyhit;
    protected SequencePool mSeqPool = new SequencePool();
    protected float moveX;
    protected float moveY;
    protected float bulletAngle;
    protected Vector3 moveDirection;
    private Vector3 raycastPoint;
    protected Vector3 StartPosition;
    protected float StartPositionY;
    protected float PosFromStart2Target = 5f;
    protected Transform shadow;
    protected GameObject shadowGameObject;
    protected Vector3 shadow_initpos;
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private float <CurrentDistance>k__BackingField;
    private float mDistance;
    protected float LifeTime;
    protected float CreateTime;
    protected float RemoveTime;
    private float mSpeed;
    protected Transform childMesh;
    protected MeshRenderer childMeshRender;
    private Transform rotateTran;
    protected Vector3 childMesh_initpos;
    private TrailRenderer[] trails;
    private GameObject lastwall;
    private ActionBasic action = new ActionBasic();
    protected List<EntityBase> mHitList = new List<EntityBase>();
    protected GameObject mHitWall;
    protected Action<bool> OnTrailShowEvent;
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private int <bulletids>k__BackingField;
    private bool bDelayCache;
    protected ConditionBase mCondition;
    protected float[] mArgs;
    protected EntityBase Target;
    protected Vector3 TargetPosition;
    [NonSerialized]
    public BulletBase mLastBullet;
    private BulletLine mBulletLine;
    protected Transform mBulletModel;
    private int mArrowEjectCount;
    private int mArrowEjectMaxCount;
    private float currentHitRatio = 1f;
    private float catapult_x;
    private float catapult_z;
    private float catapult_alpha;
    private float catapult_scale;
    protected Action<float> meshAlphaAction;
    protected int mReboundWallCount;
    protected int mReboundWallMaxCount;
    private SphereCollider mReboundSphere;
    private BulletBase HitCreate2_Bullet;
    private GameObject HitSputter_o;
    private List<EntityBase> HitSputter_list;
    private float HitSputter_hitratio = 0.5f;
    private int HitSputter_i;
    private int HitSputter_imax;
    protected bool bLight45;
    protected float mSpeedRatio = 1f;
    protected TrailCtrl mTrailCtrl;
    private Dictionary<GameObject, TriggerData> mTriggerList = new Dictionary<GameObject, TriggerData>();
    private Dictionary<GameObject, TriggerData>.Enumerator mTriggerListIter;
    private int TriggerTest_Interval;
    private int TriggerTest_TriggerFrame;
    private int TriggerTest_Boxi;
    private int TriggerTest_Spherei;
    private int TriggerTest_Capsulei;
    private RaycastHit[] TriggerTest_Hits;
    private RaycastHit TriggerTest_Hit;
    private float TriggerTest_Min;
    private float TriggerTest_MoveDis;
    private Vector3 TriggerTest_CurrentPos;
    private float TriggerTest_BeforeHit;
    private Vector3 TriggerTest_vec = new Vector3(0f, 1f, 0f);
    private Collider minCollider;
    private float tempdis;
    private float tempmin;
    private float mindis;
    private int mInitFrameCount;
    private List<Collider> mColliders = new List<Collider>();
    private bool canhitted;
    private HitStruct target_hs;
    private int TriggerExtra_hit;
    private bool TriggerExtra_bEject;
    private bool TriggerExtra_bThroughEnemy;
    private RaycastHit HitWall_hit;
    private Vector3 HitWall_dir = new Vector3();
    private bool bShowBullet = true;
    private bool bGetTrackTarget;
    private EntityBase mTrackTarget;
    protected bool bExcuteReboundWall;
    private float BulletRayCast_cudris;
    protected Vector3 Parabola_position = new Vector3();
    private Vector3 OnMove_vec = new Vector3();
    protected float Parabola_MaxHeight = 2f;
    protected AnimationCurve Parabola_Curve;
    private Keyframe beforeframe;
    private Keyframe afterframe = new Keyframe();
    private AnimationCurve Horizontal_Curve;
    private Vector3 Horizontal_vec = new Vector3();
    private float bulletscale;

    public void AddCantHit(EntityBase entity)
    {
        this.mHitList.Add(entity);
    }

    private void Awake()
    {
        this.mTransform = base.transform;
        this.mGameObject = base.gameObject;
        this.boxList = base.GetComponents<BoxCollider>();
        this.boxListCount = this.boxList.Length;
        this.sphereList = base.GetComponents<SphereCollider>();
        this.sphereListCount = this.sphereList.Length;
        this.capsuleList = base.GetComponents<CapsuleCollider>();
        this.capsuleListCount = this.capsuleList.Length;
        this.AwakeInit();
    }

    protected virtual void AwakeInit()
    {
    }

    protected virtual void BoxEnable(bool enable)
    {
        this.bBoxEnable = enable;
        if (this.boxListCount > 0)
        {
            for (int i = 0; i < this.boxListCount; i++)
            {
                if (this.boxList[i] != null)
                {
                    this.boxList[i].enabled = enable;
                }
            }
        }
        if (this.sphereList.Length > 0)
        {
            for (int i = 0; i < this.sphereList.Length; i++)
            {
                if (this.sphereList[i] != null)
                {
                    this.sphereList[i].enabled = enable;
                }
            }
        }
        if (this.capsuleList.Length > 0)
        {
            for (int i = 0; i < this.capsuleList.Length; i++)
            {
                if (this.capsuleList[i] != null)
                {
                    this.capsuleList[i].enabled = enable;
                }
            }
        }
    }

    public void BulletCache()
    {
        this.Cache();
    }

    public void BulletDestroy()
    {
        this.overDistance();
    }

    private void BulletHorizontal()
    {
        if (this.bMoveEnable && !this.bExcuteReboundWall)
        {
            float frameDistance = this.FrameDistance;
            this.CurrentDistance += frameDistance;
            float time = this.CurrentDistance / this.PosFromStart2Target;
            this.Horizontal_vec.x = this.mTransform.position.x + (this.moveX * this.FrameDistance);
            this.Horizontal_vec.y = this.Horizontal_Curve.Evaluate(time) * this.StartPositionY;
            this.Horizontal_vec.z = this.mTransform.position.z + (this.moveY * this.FrameDistance);
            this.mTransform.position = this.Horizontal_vec;
            if (time >= 1f)
            {
                this.overDistance();
            }
        }
    }

    private void BulletHorizontalInit()
    {
        if (this.m_Data.Ballistic == 3)
        {
            this.Horizontal_Curve = LocalModelManager.Instance.Curve_curve.GetCurve(0x186a3);
        }
    }

    protected void BulletModelShow(bool value)
    {
        if (this.mBulletModel != null)
        {
            this.mBulletModel.gameObject.SetActive(value);
        }
    }

    private void BulletParabola()
    {
        if (this.bMoveEnable && !this.bExcuteReboundWall)
        {
            float frameDistance = this.FrameDistance;
            this.CurrentDistance += frameDistance;
            float num2 = this.CurrentDistance / this.PosFromStart2Target;
            this.Parabola_position.x = this.mTransform.position.x + (this.moveX * frameDistance);
            this.Parabola_position.y = this.Parabola_Curve.Evaluate(this.CurrentDistance / this.PosFromStart2Target) * this.Parabola_MaxHeight;
            this.Parabola_position.z = this.mTransform.position.z + (this.moveY * frameDistance);
            this.mTransform.position = this.Parabola_position;
            if (num2 >= 1f)
            {
                if (this.m_Data.DeadSoundID != 0)
                {
                    GameLogic.Hold.Sound.PlayBulletDead(this.m_Data.DeadSoundID, this.mTransform.position);
                }
                this.ParabolaOver();
            }
        }
    }

    private void BulletParabolaInit()
    {
        if (this.m_Data.Ballistic == 2)
        {
            this.Parabola_Curve = LocalModelManager.Instance.Curve_curve.GetCurve(0x186a2);
        }
    }

    private void BulletRayCast()
    {
        if (this.bMoveEnable && !this.bExcuteReboundWall)
        {
            this.BulletRayCast_cudris = this.FrameDistance;
            this.mTransform.position += this.moveDirection * this.BulletRayCast_cudris;
            this.CurrentDistance += this.BulletRayCast_cudris;
            if (this.CurrentDistance >= this.Distance)
            {
                this.overDistance();
            }
        }
    }

    protected void BulletStraight()
    {
        if (this.bMoveEnable && !this.bExcuteReboundWall)
        {
            this.OnMove();
            if (this.CurrentDistance >= this.Distance)
            {
                this.overDistance();
            }
        }
    }

    protected virtual void Cache()
    {
        if ((this != null) && (this.mGameObject != null))
        {
            this.DeInitData();
            if ((this.m_Entity != null) && this.m_Entity.IsSelf)
            {
                GameLogic.Release.PlayerBullet.Cache(this.m_Data.WeaponID, this.mGameObject);
            }
            else
            {
                GameLogic.BulletCache(this.m_Data.WeaponID, this.mGameObject);
            }
        }
    }

    private void CacheLater()
    {
        this.BulletCache();
    }

    protected void Catapult()
    {
        if (this.meshAlphaAction == null)
        {
            object[] args = new object[] { this.BulletID };
            SdkManager.Bugly_Report("BulletBase_Skill.Catapult", Utils.FormatString("子弹ID:{0} 弹飞效果 必须有meshAlphaAction", args));
        }
        float angle = Random.Range((float) (this.mTransform.eulerAngles.y + 120f), (float) (this.mTransform.eulerAngles.y + 240f));
        this.catapult_x = MathDxx.Sin(angle);
        this.catapult_z = MathDxx.Cos(angle);
        this.catapult_alpha = 1f;
        this.catapult_scale = 1f;
        this.bMoveEnable = false;
        this.BoxEnable(false);
        Updater.AddUpdate("BulletBase_Skill.Catapult", new Action<float>(this.OnCatapult), false);
    }

    private void CheckFar()
    {
        if ((GameLogic.Release.Game.RoomState == RoomState.Runing) && ((MathDxx.Abs(this.mTransform.position.x) > 30f) || (MathDxx.Abs(this.mTransform.position.z) > 30f)))
        {
            this.overDistance();
        }
    }

    private void CreateBulletEffect()
    {
        if (this.m_Data.CreatePath != string.Empty)
        {
            object[] args = new object[] { "Game/BulletCreate/BulletCreate_", this.m_Data.CreatePath };
            Transform transform = GameLogic.EffectGet(Utils.GetString(args)).transform;
            transform.parent = this.mTransform;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
            transform.parent = GameNode.m_PoolParent;
        }
    }

    private void CreateBulletLine()
    {
        if ((this.m_Entity.m_EntityData.GetBulletLine() && (this.mLastBullet != null)) && this.mLastBullet.GetInit())
        {
            this.mBulletLine = new BulletLine();
            this.mBulletLine.Init(this, this.mLastBullet);
        }
    }

    public void DeInit()
    {
        this.bInit = false;
        this.CacheLater();
    }

    private void DeInitData()
    {
        this.mCondition = null;
        this.mLastBullet = null;
        if (this.mBulletLine != null)
        {
            this.mBulletLine.DeInit();
            this.mBulletLine = null;
        }
        this.mHitWall = null;
        this.OnDeInit();
        this.action.DeInit();
        this.BoxEnable(false);
        this.bMoveEnable = false;
        this.TrailShow(false);
        this.ShadowShow(false);
        this.FlyOver();
        Updater.RemoveUpdate("BulletBase_Skill.Catapult", new Action<float>(this.OnCatapult));
    }

    private void DeInitDelay(float deaddelay)
    {
        this.TrailShow(false);
        if (deaddelay > 0f)
        {
            this.action.ActionClear();
            ActionBasic.ActionWait action = new ActionBasic.ActionWait {
                waitTime = deaddelay / 1000f
            };
            this.action.AddAction(action);
            ActionBasic.ActionDelegate delegate2 = new ActionBasic.ActionDelegate {
                action = new Action(this.DeInit)
            };
            this.action.AddAction(delegate2);
        }
        else
        {
            this.DeInit();
        }
    }

    public bool ExcuteArrowEject(EntityBase entity)
    {
        if (this.mArrowEjectCount < 1)
        {
            return false;
        }
        EntityBase nextentity = GameLogic.Release.Entity.FindArrowEject(entity);
        if (nextentity == null)
        {
            return false;
        }
        this.OnArrowEject(nextentity);
        this.currentHitRatio *= GameConfig.GetArrowEject();
        if (this.mArrowEjectCount == this.mArrowEjectMaxCount)
        {
            this.mSpeed *= GameConfig.GetArrowEject();
        }
        this.mBulletTransmit.ArrowEjectAction(GameConfig.GetArrowEject());
        this.mArrowEjectCount--;
        this.mTransform.position = new Vector3(entity.position.x, this.mTransform.position.y, entity.position.z);
        float x = nextentity.position.x - this.mTransform.position.x;
        float y = nextentity.position.z - this.mTransform.position.z;
        this.bulletAngle = Utils.getAngle(x, y);
        this.UpdateMoveDirection();
        return true;
    }

    protected void ExcuteReboundWall(Collider o)
    {
        if (o.gameObject != this.mHitWall)
        {
            if (this.mReboundWallCount < 1)
            {
                this.PlayHitWallSound();
                if (this.HitWallAction != null)
                {
                    this.HitWallAction(o);
                }
            }
            else
            {
                if (this.m_Entity.IsSelf)
                {
                    this.currentHitRatio *= GameConfig.GetReboundHit();
                }
                if (this.mReboundWallCount == this.mReboundWallMaxCount)
                {
                    this.mSpeed *= GameConfig.GetReboundSpeed();
                }
                this.mReboundWallCount--;
                this.mHitList.Clear();
                this.mHitWall = o.gameObject;
                this.mReboundSphere = null;
                if (this.sphereList.Length > 0)
                {
                    this.mReboundSphere = this.sphereList[0];
                }
                float num = Utils.ExcuteReboundWallSkill(this.bulletAngle, this.mTransform.position, this.mReboundSphere, o);
                this.bulletAngle = num;
                if (this.m_Data.Ballistic == 1)
                {
                    this.mTransform.position = this.raycastPoint;
                }
                this.bExcuteReboundWall = true;
                this.UpdateMoveDirection();
            }
        }
    }

    protected void FlyOver()
    {
    }

    public float[] GetBuffArgs() => 
        this.OnGetBuffArg();

    public EntityBase GetEntity() => 
        this.m_Entity;

    public bool GetInit() => 
        this.bInit;

    public bool GetLight45() => 
        this.bLight45;

    protected Vector3 GetMoveDirection(float angle)
    {
        float x = MathDxx.Sin(angle);
        float z = MathDxx.Cos(angle);
        if (this.bZScale)
        {
            z *= 1.23f;
        }
        return new Vector3(x, 0f, z);
    }

    protected virtual Transform GetTrailAttParent() => 
        this.mTransform;

    protected void HeadAttrShow(bool show)
    {
        if ((this.headattrobj != null) && !show)
        {
            GameLogic.EffectCache(this.headattrobj);
            this.headattrobj = null;
        }
        else if (((this.headattrobj == null) && show) && (this.mBulletTransmit.headType != EElementType.eNone))
        {
            this.headattrobj = GameLogic.EffectGet(EntityData.ElementData[this.mBulletTransmit.headType].HeadPath);
            this.headattrobj.transform.SetParent(this.mTransform);
            this.headattrobj.transform.localPosition = Vector3.zero;
            this.headattrobj.transform.localScale = Vector3.one;
            this.headattrobj.transform.localRotation = Quaternion.identity;
        }
    }

    private void HitCreate2(EntityBase entity, float hittedAngle)
    {
        if (this.mBulletTransmit.GetHitCreate2())
        {
            for (int i = 0; i < 2; i++)
            {
                Transform transform = GameLogic.BulletGet(0xbb9).transform;
                transform.SetParent(GameNode.m_PoolParent);
                transform.position = new Vector3(entity.position.x, 1f, entity.position.z);
                transform.localRotation = Quaternion.Euler(0f, (hittedAngle - 90f) + (i * 180f), 0f);
                transform.localScale = Vector3.one;
                transform.SetParent(GameNode.m_PoolParent);
                this.HitCreate2_Bullet = transform.GetComponent<BulletBase>();
                this.HitCreate2_Bullet.Init(this.m_Entity, 0xbb9);
                this.HitCreate2_Bullet.AddCantHit(entity);
                this.HitCreate2_Bullet.SetBulletAttribute(new BulletTransmit(this.m_Entity, 0xbb9, true));
                this.HitCreate2_Bullet.mBulletTransmit.AddAttackRatio(this.mBulletTransmit.mHitCreate2Percent);
            }
        }
    }

    protected void HitHero(EntityBase entity, Collider o)
    {
        if (this.childMesh != null)
        {
            this.childMesh.localPosition = this.childMesh_initpos;
        }
        this.OnHitHero(entity);
        if (entity != null)
        {
            entity.AddHatredTarget(this.m_Entity);
        }
        if (entity != null)
        {
            Transform ketNode = entity.GetKetNode(this.m_Data.DeadNode);
            if (ketNode != null)
            {
                this.mTransform.SetParent(ketNode);
                this.mTransform.localPosition = Vector3.zero;
            }
        }
        if (!this.m_Data.bThroughEntity || (entity == null))
        {
            this.BoxEnable(false);
            this.bMoveEnable = false;
            this.TrailShow(false);
            this.ShadowShow(false);
            this.FlyOver();
            this.DeInitDelay((float) this.m_Data.DeadDelay);
        }
    }

    private void HitSputter(EntityBase entity, float hittedAngle)
    {
        if (this.mBulletTransmit.GetHitSputter())
        {
            this.HitSputter_o = GameLogic.EffectGet("Effect/Attributes/Bullet_Crescent");
            this.HitSputter_o.transform.position = entity.position;
            this.HitSputter_o.transform.localRotation = Quaternion.Euler(0f, hittedAngle, 0f);
            this.mBulletTransmit.mHitSputter = 0;
            this.HitSputter_list = GameLogic.Release.Entity.GetSectorEntities(entity, 4f, hittedAngle, 90f, true);
            this.HitSputter_imax = this.HitSputter_list.Count;
            this.HitSputter_i = 0;
            while (this.HitSputter_i < this.HitSputter_imax)
            {
                long beforehit = MathDxx.CeilToInt(this.mBulletTransmit.GetAttackStruct().before_hit * this.HitSputter_hitratio);
                GameLogic.SendHit_Skill(this.HitSputter_list[this.HitSputter_i], beforehit);
                this.HitSputter_i++;
            }
        }
    }

    private void HitWall(Collider o)
    {
        if (this.bMoveEnable)
        {
            if (this.m_Data.bCache)
            {
                if (this.m_Data.Speed > 0f)
                {
                    object[] args = new object[] { this.m_Data.WeaponID };
                    SdkManager.Bugly_Report("BulletBase", Utils.FormatString("飞行子弹{0} 使用了 1击中不回收子弹！！！", args));
                }
            }
            else if (this.m_Data.Ballistic != 2)
            {
                this.bMoveEnable = false;
                this.ShowDeadEffect();
                this.OnHitWall();
                this.DeInitDelay((float) this.m_Data.DeadDelay);
            }
        }
    }

    public void Init(EntityBase entity, int BulletID)
    {
        this.BulletID = BulletID;
        this.m_Data = LocalModelManager.Instance.Weapon_weapon.GetBeanById(BulletID);
        this.Init_Model();
        Transform trail = null;
        if (this.mBulletModel != null)
        {
            this.shadow = this.mBulletModel.Find("shadow");
            if (this.shadow != null)
            {
                this.shadowGameObject = this.shadow.gameObject;
            }
            this.childMesh = this.mBulletModel.Find("child");
            if (this.childMesh != null)
            {
                this.rotateTran = this.childMesh.Find("rotate");
                this.childMesh_initpos = this.childMesh.localPosition;
                this.childMeshRender = this.childMesh.GetComponentInChildren<MeshRenderer>();
            }
            trail = this.mBulletModel.Find("trail");
        }
        this.bulletids = GameLogic.GetBulletID();
        this.mTrailCtrl = new TrailCtrl(trail);
        this.HitWallAction = new Action<Collider>(this.HitWall);
        this.TriggerTest_TriggerFrame = 0;
        this.mTriggerList.Clear();
        if ((this.m_Data.AliveTime > 0) && this.m_Data.bCache)
        {
            this.mCondition = AIMoveBase.GetConditionTime(this.m_Data.AliveTime);
        }
        this.mInitFrameCount = 0;
        this.bInit = true;
        this.currentHitRatio = 1f;
        this.mHitList.Clear();
        this.m_Entity = entity;
        this.bDelayCache = false;
        this.bGetTrackTarget = false;
        this.action.ActionClear();
        this.action.Init(false);
        this.Target = null;
        if (this.childMesh != null)
        {
            this.childMesh.gameObject.SetActive(true);
            this.childMesh.localPosition = this.childMesh_initpos;
        }
        this.CurrentDistance = 0f;
        this.bMoveEnable = true;
        this.BoxEnable(!this.bFlyCantHit);
        this.mSpeed = this.m_Data.Speed;
        if (this.m_Entity != null)
        {
            this.mSpeed *= this.m_Entity.m_EntityData.BulletSpeed;
        }
        this.ShadowShow(true);
        this.mDistance = this.m_Data.Distance;
        this.bulletAngle = this.mTransform.eulerAngles.y;
        this.UpdateMoveDirection();
        this.StartPosition = new Vector3(this.mTransform.position.x, 0f, this.mTransform.position.z);
        this.StartPositionY = this.mTransform.position.y;
        if (this.m_Data.CreateSoundID != 0)
        {
            this.AttackSoundObj = GameLogic.Hold.Sound.PlayBulletCreate(this.m_Data.CreateSoundID, this.mTransform.position);
        }
        this.BulletParabolaInit();
        this.BulletHorizontalInit();
        this.CreateBulletEffect();
        this.OnInit();
    }

    private void Init_Model()
    {
        if (this.m_Data.ModelID != string.Empty)
        {
            Transform transform = this.mTransform.Find(this.m_Data.ModelID);
            if (transform == null)
            {
                object[] args = new object[] { this.m_Data.ModelID };
                string key = Utils.FormatString("Game/BulletModels/{0}", args);
                GameObject obj2 = GameLogic.EffectGet(key);
                GameObject obj3 = null;
                if (obj2 == null)
                {
                    object[] objArray2 = new object[] { this.m_Data.WeaponID, key, this.m_Data.ModelID };
                    SdkManager.Bugly_Report("BulletBase_Model", Utils.FormatString("Init_Model BulletID:{0} path:{1} model:{2} is not found!!!", objArray2));
                    obj3 = Object.Instantiate<GameObject>(ResourceManager.Load<GameObject>("Game/BulletModels/Empty"));
                }
                else
                {
                    obj3 = obj2;
                }
                if (obj3 != null)
                {
                    this.mBulletModel = obj3.transform;
                    this.mBulletModel.name = this.m_Data.ModelID;
                    this.mBulletModel.SetParentNormal(this.mTransform);
                    this.mBulletModel.localScale = Vector3.one * this.m_Data.ModelScale;
                    this.Init_ModelScale();
                    Renderer[] componentsInChildren = this.mBulletModel.GetComponentsInChildren<Renderer>(true);
                    int index = 0;
                    int length = componentsInChildren.Length;
                    while (index < length)
                    {
                        componentsInChildren[index].sortingLayerName = "BulletEffect";
                        index++;
                    }
                }
            }
            else
            {
                this.mBulletModel = transform;
                this.mBulletModel.gameObject.SetActive(true);
            }
        }
    }

    private void Init_ModelScale()
    {
        TrailRenderer[] componentsInChildren = this.mBulletModel.GetComponentsInChildren<TrailRenderer>(true);
        if ((componentsInChildren != null) && (componentsInChildren.Length > 0))
        {
            int index = 0;
            int length = componentsInChildren.Length;
            while (index < length)
            {
                TrailRenderer renderer1 = componentsInChildren[index];
                renderer1.startWidth *= this.m_Data.ModelScale;
                TrailRenderer renderer2 = componentsInChildren[index];
                renderer2.endWidth *= this.m_Data.ModelScale;
                index++;
            }
        }
    }

    private void KillSequence()
    {
        this.mSeqPool.Clear();
    }

    protected virtual void OnArrowEject(EntityBase nextentity)
    {
    }

    protected void OnBulletTrack()
    {
        if ((((this.mBulletTransmit != null) && (this.mBulletTransmit.attribute != null)) && this.mBulletTransmit.attribute.ArrowTrack.Enable) && (this.m_Entity != null))
        {
            if (!this.bGetTrackTarget)
            {
                this.bGetTrackTarget = true;
                this.mTrackTarget = GameLogic.Release.Entity.FindTargetExclude(null);
            }
            if ((this.mTrackTarget != null) && !this.mTrackTarget.GetIsDead())
            {
                this.bulletAngle = Utils.getAngle(this.mTrackTarget.position - this.mTransform.position);
                this.UpdateMoveDirection();
            }
        }
    }

    private void OnCatapult(float delta)
    {
        if ((this == null) || (this.mTransform == null))
        {
            Updater.RemoveUpdate("BulletBase_Skill.Catapult", new Action<float>(this.OnCatapult));
        }
        else
        {
            this.mTransform.position += (new Vector3(this.catapult_x, 0f, this.catapult_z) * Updater.delta) * 6f;
            this.catapult_alpha -= Updater.delta * 4f;
            this.catapult_scale -= Updater.delta * 0.6f;
            this.mTransform.localScale = Vector3.one * this.catapult_scale;
            if (this.catapult_alpha < 0.4f)
            {
                this.catapult_alpha = 0f;
                this.DeInit();
            }
            if (this.meshAlphaAction != null)
            {
                this.meshAlphaAction(this.catapult_alpha);
            }
        }
    }

    protected virtual void OnDeInit()
    {
        this.KillSequence();
        if (this.OnBulletCache != null)
        {
            this.OnBulletCache();
        }
    }

    protected virtual float[] OnGetBuffArg() => 
        new float[0];

    private void OnHitEvent(EntityBase entity, float hittedAngle)
    {
        this.HitCreate2(entity, hittedAngle);
        this.HitSputter(entity, hittedAngle);
    }

    protected virtual void OnHitHero(EntityBase entity)
    {
    }

    protected virtual void OnHitWall()
    {
    }

    protected virtual void OnInit()
    {
    }

    protected virtual void OnMove()
    {
        float frameDistance = this.FrameDistance;
        this.OnMove(frameDistance);
    }

    protected void OnMove(float dis)
    {
        this.OnMove_vec.x = this.moveX;
        this.OnMove_vec.y = 0f;
        this.OnMove_vec.z = this.moveY;
        this.mTransform.position += this.OnMove_vec * dis;
        this.CurrentDistance += dis;
    }

    protected virtual void OnOverDistance()
    {
    }

    protected virtual void OnSetArgs()
    {
    }

    protected virtual void OnSetBulletAttribute()
    {
    }

    protected virtual void OnThroughTrailShow(bool show)
    {
    }

    protected virtual void OnUpdate()
    {
        switch (this.m_Data.Ballistic)
        {
            case 0:
                this.BulletStraight();
                break;

            case 1:
                this.BulletRayCast();
                break;

            case 2:
                this.BulletParabola();
                break;

            case 3:
                this.BulletHorizontal();
                break;
        }
    }

    protected virtual void overDistance()
    {
        if (this.m_Data.DeadSoundID != 0)
        {
            GameLogic.Hold.Sound.PlayBulletDead(this.m_Data.DeadSoundID, this.mTransform.position);
        }
        this.OnOverDistance();
        this.ShowDeadEffect();
        this.HitHero(null, null);
    }

    protected virtual void ParabolaOver()
    {
        if (this.bFlyCantHit)
        {
            this.BoxEnable(true);
            this.bMoveEnable = false;
            this.TrailShow(false);
            this.ShadowShow(false);
            this.FlyOver();
            this.ShowDeadEffect();
            if (this.childMesh != null)
            {
                this.childMesh.gameObject.SetActive(false);
            }
            TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(this.mSeqPool.Get(), 0.1f), new TweenCallback(this, this.<ParabolaOver>m__2));
        }
        else
        {
            this.overDistance();
        }
    }

    public GameObject PlayEffect(int fxId, Vector3 pos, Quaternion rota)
    {
        Fx_fx beanById = LocalModelManager.Instance.Fx_fx.GetBeanById(fxId);
        if (beanById == null)
        {
            object[] args = new object[] { fxId };
            SdkManager.Bugly_Report("BulletBase", Utils.FormatString("PlayEffect[{0}] is null.", args));
        }
        Transform transform = GameLogic.Release.MapEffect.Get(beanById.Path).transform;
        transform.SetParent(GameNode.m_PoolParent);
        transform.position = pos;
        transform.rotation = rota;
        return transform.gameObject;
    }

    private void PlayHitWallSound()
    {
        if (this.m_Data.HitWallSoundID != 0)
        {
            GameLogic.Hold.Sound.PlayBulletHitWall(this.m_Data.HitWallSoundID, this.mTransform.position);
        }
    }

    protected void RotateDeal()
    {
        if (this.bMoveEnable)
        {
            if ((this.m_Data.LookCamera != 0) && (this.childMesh != null))
            {
                this.childMesh.rotation = Quaternion.Euler(-35f, 0f, 0f);
            }
            if (this.m_Data.RotateSpeed > 0f)
            {
                if (this.rotateTran != null)
                {
                    this.rotateTran.localRotation = Quaternion.Euler(this.rotateTran.localEulerAngles.x, this.rotateTran.localEulerAngles.y + this.m_Data.RotateSpeed, this.rotateTran.localEulerAngles.z);
                }
                else if (this.childMesh != null)
                {
                    this.childMesh.localRotation = Quaternion.Euler(this.childMesh.localEulerAngles.x, this.childMesh.localEulerAngles.y + this.m_Data.RotateSpeed, this.childMesh.localEulerAngles.z);
                }
            }
        }
    }

    public void SetArgs(params float[] args)
    {
        this.mArgs = args;
        this.OnSetArgs();
    }

    protected void SetBoxEnableOnce(float starttime)
    {
        this.BoxEnable(false);
        TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(this.mSeqPool.Get(), starttime), new TweenCallback(this, this.<SetBoxEnableOnce>m__0)), 0.03f), new TweenCallback(this, this.<SetBoxEnableOnce>m__1));
    }

    public virtual void SetBulletAttribute(BulletTransmit bullet)
    {
        this.mBulletTransmit = bullet;
        int index = 0;
        int length = this.m_Data.Attributes.Length;
        while (index < length)
        {
            this.mBulletTransmit.attribute.Excute(this.m_Data.Attributes[index]);
            index++;
        }
        this.TrailShow(true);
        this.UpdateBulletAttribute();
        this.OnSetBulletAttribute();
    }

    public void SetLastBullet(BulletBase o)
    {
        this.mLastBullet = o;
        this.CreateBulletLine();
    }

    public void SetPosFromTarget(float dis)
    {
        this.PosFromStart2Target = dis;
        this.UpdateParabolaArgs();
    }

    public void SetRadius(float radius)
    {
        if (this.sphereList.Length > 0)
        {
            for (int i = 0; i < this.sphereList.Length; i++)
            {
                if (this.sphereList[i] != null)
                {
                    this.sphereList[i].radius = radius;
                }
            }
        }
        if (this.capsuleList.Length > 0)
        {
            for (int i = 0; i < this.capsuleList.Length; i++)
            {
                if (this.capsuleList[i] != null)
                {
                    this.capsuleList[i].radius = radius;
                }
            }
        }
    }

    public virtual void SetTarget(EntityBase entity, int size = 1)
    {
        if (entity != null)
        {
            this.Target = entity;
            List<Vector2Int> roundEmpty = GameLogic.Release.MapCreatorCtrl.GetRoundEmpty(this.Target.position, size);
            if (roundEmpty.Count == 0)
            {
                this.TargetPosition = this.Target.position;
            }
            else
            {
                int num = GameLogic.Random(0, roundEmpty.Count);
                this.TargetPosition = GameLogic.Release.MapCreatorCtrl.GetWorldPosition(roundEmpty[num]);
            }
            this.PosFromStart2Target = Vector3.Distance(this.StartPosition, this.TargetPosition);
            if (this.PosFromStart2Target < 0.01f)
            {
                this.PosFromStart2Target = 0.01f;
            }
        }
        this.UpdateParabolaArgs();
    }

    protected void ShadowDeal()
    {
        if (this.shadow != null)
        {
            this.shadow.localPosition = this.shadow_initpos;
            this.shadow.position = new Vector3(this.shadow.position.x, 0.1f, this.shadow.position.z);
            this.shadow.rotation = Quaternion.Euler(90f, this.mTransform.eulerAngles.y, 0f);
        }
    }

    private void ShadowShow(bool show)
    {
        if (this.shadowGameObject != null)
        {
            this.shadowGameObject.SetActive(show);
        }
    }

    protected void ShowDeadEffect()
    {
        if (this.m_Data.DeadEffectID != 0)
        {
            this.PlayEffect(this.m_Data.DeadEffectID, this.mTransform.position, this.mTransform.rotation);
        }
    }

    private void Start()
    {
    }

    protected virtual void StartInit()
    {
    }

    protected void ThroughTrailShow(bool show)
    {
        if (this.mBulletTransmit.GetThroughEnemy())
        {
            this.OnThroughTrailShow(show);
        }
    }

    protected void TrailAttrShow(bool show)
    {
        if ((this.trailattrobj != null) && !show)
        {
            GameLogic.EffectCache(this.trailattrobj);
            this.trailattrobj = null;
        }
        else if (((this.trailattrobj == null) && show) && (this.mBulletTransmit.trailType != EElementType.eNone))
        {
            this.trailattrobj = GameLogic.EffectGet(EntityData.ElementData[this.mBulletTransmit.trailType].TrailPath);
            this.trailattrobj.transform.SetParent(this.GetTrailAttParent());
            this.trailattrobj.transform.localPosition = Vector3.zero;
            this.trailattrobj.transform.localRotation = Quaternion.identity;
            Vector3 one = Vector3.one;
            int type = 0x65;
            Equip_equip beanById = LocalModelManager.Instance.Equip_equip.GetBeanById(this.m_Data.WeaponID);
            if (beanById != null)
            {
                type = beanById.Type;
            }
            switch (this.m_Data.WeaponID)
            {
                case 0x1f41:
                    type = 0x68;
                    break;

                case 0x1f42:
                    type = 0x67;
                    break;

                case 0x3e9:
                    type = 0x65;
                    break;
            }
            switch (type)
            {
                case 0x65:
                    one = new Vector3(1f, 1f, 1f);
                    break;

                case 0x67:
                    one = new Vector3(1.5f, 1f, 0.4f);
                    break;

                case 0x68:
                    one = new Vector3(2f, 1f, 0.4f);
                    break;
            }
            this.trailattrobj.transform.localScale = one;
        }
    }

    protected void TrailShow(bool show)
    {
        if (((this.mTrailCtrl != null) && (this.mBulletTransmit != null)) && (this.mTrailCtrl.bShow != show))
        {
            this.mTrailCtrl.bShow = show;
            this.ThroughTrailShow(show);
            this.TrailAttrShow(show);
            this.HeadAttrShow(show);
            if ((this.mBulletTransmit.trailType != EElementType.eNone) || (this.mBulletTransmit.headType != EElementType.eNone))
            {
                show = false;
            }
            this.mTrailCtrl.TrailShow(show);
            if (this.OnTrailShowEvent != null)
            {
                this.OnTrailShowEvent(show);
            }
        }
    }

    private void TriggerEnter1(Collider o)
    {
        GameObject gameObject = null;
        if (o != null)
        {
            gameObject = o.gameObject;
        }
        if ((gameObject != null) && ((gameObject.layer == LayerManager.Bullet2Map) || (gameObject.layer == LayerManager.MapOutWall)))
        {
            if (!this.m_Data.bThroughWall)
            {
                if (this.mBulletTransmit.attribute.ReboundWall.Enable)
                {
                    this.ExcuteReboundWall(o);
                }
                else
                {
                    this.PlayHitWallSound();
                    if (this.HitWallAction != null)
                    {
                        this.HitWallAction(o);
                    }
                }
            }
        }
        else
        {
            if ((gameObject.layer == LayerManager.BulletResist) && !this.m_Data.bThroughEntity)
            {
                EntityParentBase component = o.GetComponent<EntityParentBase>();
                if ((component != null) && !component.IsSelf(this.m_Entity))
                {
                    this.overDistance();
                }
            }
            this.TriggerExtra(o);
        }
    }

    protected virtual void TriggerExtra(Collider o)
    {
        EntityBase me = null;
        GameObject gameObject = null;
        if (o != null)
        {
            gameObject = o.gameObject;
        }
        if ((gameObject != null) && ((gameObject.layer == LayerManager.Player) || (gameObject.layer == LayerManager.Fly)))
        {
            me = GameLogic.Release.Entity.GetEntityByChild(gameObject);
            if (me == this.m_Entity)
            {
                if (this.OnHitSelf != null)
                {
                    this.OnHitSelf();
                }
                return;
            }
        }
        if (((me != null) && !GameLogic.IsSameTeam(me, this.m_Entity)) && !this.mHitList.Contains(me))
        {
            bool enable = this.mBulletTransmit.attribute.ArrowEject.Enable;
            if (enable)
            {
                this.mHitList.Clear();
            }
            if (!this.m_Data.bMoreHit)
            {
                this.mHitList.Add(me);
            }
            this.TriggerExtra_bThroughEnemy = this.mBulletTransmit.GetThroughEnemy();
            this.canhitted = me.GetHittedData(this).GetCanHitted();
            if ((!this.canhitted && !this.m_Data.bThroughEntity) && !this.TriggerExtra_bThroughEnemy)
            {
                this.HitHero(me, o);
            }
            if (this.canhitted)
            {
                this.target_hs = this.mBulletTransmit.GetAttackStruct();
                this.TriggerExtra_hit = (int) (this.target_hs.before_hit * this.currentHitRatio);
                this.target_hs.before_hit = MathDxx.Clamp(this.TriggerExtra_hit, this.TriggerExtra_hit, -1);
                this.mBulletTransmit.AddDebuffsToTarget(me);
                HitBulletStruct bulletdata = new HitBulletStruct {
                    bullet = this,
                    weapon = this.m_Data
                };
                GameLogic.SendHit_Bullet(me, this.m_Entity, this.target_hs.before_hit, this.target_hs.type, bulletdata);
                this.TriggerExtra_bEject = false;
                if (enable)
                {
                    this.TriggerExtra_bEject = this.ExcuteArrowEject(me);
                }
                if (this.TriggerExtra_bThroughEnemy)
                {
                    this.currentHitRatio *= this.mBulletTransmit.mThroughRatio;
                }
                else if (!this.TriggerExtra_bEject)
                {
                    this.HitHero(me, o);
                }
                this.OnHitEvent(me, this.bulletAngle);
            }
        }
    }

    private void TriggerListCheck()
    {
        this.mTriggerListIter = this.mTriggerList.GetEnumerator();
        while (this.mTriggerListIter.MoveNext())
        {
            if (this.mTriggerListIter.Current.Value.GetCanHit())
            {
                this.TriggerEnter1(this.mTriggerListIter.Current.Value.collider);
            }
        }
    }

    protected void TriggerTest()
    {
        if (this.FrameDistance > 0f)
        {
            this.TriggerTest_Interval = (int) (0.1f / this.FrameDistance);
        }
        else
        {
            this.TriggerTest_Interval = 0;
        }
        this.bExcuteReboundWall = false;
        if ((Time.frameCount - this.TriggerTest_TriggerFrame) > this.TriggerTest_Interval)
        {
            this.TriggerTest_TriggerFrame = Time.frameCount;
            this.TriggerTest_Base();
        }
        if (this.bMoveEnable && !this.bExcuteReboundWall)
        {
            this.OnUpdate();
            this.OnBulletTrack();
        }
    }

    protected void TriggerTest_Base()
    {
        this.TriggerTest_MoveDis = this.FrameDistance;
        this.TriggerTest_BeforeHit = this.TriggerTest_MoveDis;
        if (this.CurrentFrameCount != Time.frameCount)
        {
            this.mColliders.Clear();
            this.TriggerTest_CurrentPos = this.mTransform.position;
            if (this.mInitFrameCount == 0)
            {
                this.mInitFrameCount = 1;
                this.TriggerTest_BeforeHit = 0f;
            }
            else
            {
                this.TriggerTest_BeforeHit = this.TriggerTest_MoveDis;
            }
            if (this.m_Data.Ballistic == 1)
            {
                if (!this.bMoveEnable)
                {
                    return;
                }
                this.TriggerTest_BeforeHit = this.TriggerTest_MoveDis;
                this.TriggerTest_BeforeHit = MathDxx.Clamp(this.TriggerTest_BeforeHit, 0f, 0.8f);
                this.TriggerTest_Hits = Physics.RaycastAll(this.TriggerTest_CurrentPos - (this.moveDirection.normalized * this.TriggerTest_BeforeHit), this.moveDirection, this.FrameDistance + this.TriggerTest_BeforeHit, this.m_Data.GetLayer());
            }
            else if (this.boxListCount > 0)
            {
                if (!this.boxList[0].enabled)
                {
                    return;
                }
                int index = 0;
                int length = this.boxList.Length;
                while (index < length)
                {
                    RaycastHit[] hitArray = Physics.BoxCastAll(this.mTransform.TransformPoint((new Vector3(0f, 0f, -1f) * this.TriggerTest_BeforeHit) + this.boxList[index].center), (Vector3) ((this.mTransform.localScale.x * this.boxList[index].size) / 2f), this.moveDirection, this.mTransform.rotation, this.FrameDistance + this.TriggerTest_BeforeHit, this.m_Data.GetLayer());
                    if (index == 0)
                    {
                        this.TriggerTest_Hits = hitArray;
                    }
                    else
                    {
                        RaycastHit[] hitArray2 = new RaycastHit[this.TriggerTest_Hits.Length + hitArray.Length];
                        for (int i = 0; i < this.TriggerTest_Hits.Length; i++)
                        {
                            hitArray2[i] = this.TriggerTest_Hits[i];
                        }
                        for (int j = 0; j < hitArray.Length; j++)
                        {
                            hitArray2[this.TriggerTest_Hits.Length + j] = hitArray[j];
                        }
                        this.TriggerTest_Hits = hitArray2;
                    }
                    index++;
                }
            }
            else if (this.sphereListCount > 0)
            {
                if (!this.sphereList[0].enabled)
                {
                    return;
                }
                this.TriggerTest_Hits = Physics.SphereCastAll(this.TriggerTest_CurrentPos - (this.moveDirection * this.TriggerTest_BeforeHit), this.mTransform.localScale.x * this.sphereList[0].radius, this.moveDirection, this.FrameDistance + this.TriggerTest_BeforeHit, this.m_Data.GetLayer());
            }
            else if (this.capsuleListCount > 0)
            {
                if (!this.capsuleList[0].enabled)
                {
                    return;
                }
                this.TriggerTest_Hits = Physics.CapsuleCastAll((this.TriggerTest_CurrentPos + ((this.TriggerTest_vec * (this.capsuleList[0].height - 1f)) / 2f)) - (this.moveDirection * this.TriggerTest_BeforeHit), (this.TriggerTest_CurrentPos - ((this.TriggerTest_vec * (this.capsuleList[0].height - 1f)) / 2f)) - (this.moveDirection * this.TriggerTest_BeforeHit), this.mTransform.localScale.x * this.capsuleList[0].radius, this.moveDirection, this.FrameDistance + this.TriggerTest_BeforeHit, this.m_Data.GetLayer());
            }
            if ((this.TriggerTest_Hits != null) && (this.TriggerTest_Hits.Length > 0))
            {
                this.minCollider = null;
                this.mindis = 2.147484E+09f;
                int index = 0;
                int length = this.TriggerTest_Hits.Length;
                while (index < length)
                {
                    this.TriggerTest_Hit = this.TriggerTest_Hits[index];
                    if ((this.TriggerTest_Hit.collider.gameObject != null) && (((this.m_Entity != null) && (this.TriggerTest_Hit.collider.gameObject != this.m_Entity.gameObject)) || (this.m_Entity == null)))
                    {
                        this.mColliders.Add(this.TriggerTest_Hit.collider);
                        this.tempdis = this.TriggerTest_Hit.distance;
                        this.tempmin = MathDxx.Abs((float) (this.tempdis - this.TriggerTest_BeforeHit));
                        if (this.tempmin <= this.mindis)
                        {
                            this.mindis = this.tempmin;
                            this.minCollider = this.TriggerTest_Hit.collider;
                            this.raycastPoint = this.TriggerTest_Hit.point;
                        }
                    }
                    index++;
                }
                if (this.m_Data.bMoreHit)
                {
                    int num7 = 0;
                    int count = this.mColliders.Count;
                    while (num7 < count)
                    {
                        this.TriggerUpdateList(this.mColliders[num7]);
                        num7++;
                    }
                    this.TriggerListCheck();
                }
                else if (this.minCollider != null)
                {
                    this.TriggerEnter1(this.minCollider);
                }
                this.CurrentFrameCount = Time.frameCount;
            }
        }
    }

    private void TriggerUpdateList(Collider o)
    {
        if (!this.mTriggerList.TryGetValue(o.gameObject, out TriggerData data))
        {
            data = new TriggerData {
                target = o.gameObject
            };
            this.mTriggerList.Add(o.gameObject, data);
        }
        data.currentframe = Time.frameCount;
        data.collider = o;
    }

    public void UpdateBulletAttribute()
    {
        this.mReboundWallCount = this.mBulletTransmit.attribute.ReboundWall.Value;
        this.mReboundWallMaxCount = this.mReboundWallCount;
        this.mArrowEjectCount = this.mBulletTransmit.attribute.ArrowEject.Value;
        this.mArrowEjectMaxCount = this.mArrowEjectCount;
    }

    protected void UpdateMoveDirection()
    {
        this.moveX = MathDxx.Sin(this.bulletAngle);
        this.moveY = MathDxx.Cos(this.bulletAngle);
        if (this.bZScale)
        {
            this.moveY *= 1.23f;
        }
        if (this.bFlyRotate)
        {
            this.mTransform.rotation = Quaternion.Euler(this.mTransform.eulerAngles.x, Utils.getAngle(this.moveX, this.moveY), this.mTransform.eulerAngles.z);
        }
        this.moveDirection = new Vector3(this.moveX, 0f, this.moveY);
    }

    protected void UpdateParabolaArgs()
    {
        if (this.m_Data.Ballistic == 2)
        {
            this.beforeframe = this.Parabola_Curve.keys[0];
            this.afterframe.time = 0f;
            this.afterframe.value = this.StartPositionY / this.Parabola_MaxHeight;
            this.afterframe.outTangent = this.beforeframe.outTangent;
            this.Parabola_Curve.MoveKey(0, this.afterframe);
        }
        this.CreateTime = Updater.AliveTime;
        switch (this.m_Data.Ballistic)
        {
            case 0:
            case 1:
                this.LifeTime = this.mDistance / this.Speed;
                break;

            case 2:
                this.LifeTime = this.PosFromStart2Target / this.Speed;
                break;
        }
        this.RemoveTime = this.CreateTime + this.LifeTime;
    }

    protected override void UpdateProcess()
    {
        if (GameLogic.Hold.BattleData.Challenge_MonsterHide() && !this.m_Entity.IsSelf)
        {
            bool flag = Vector3.Distance(new Vector3(this.mTransform.position.x, 0f, this.mTransform.position.z), GameLogic.Self.position) <= GameLogic.Hold.BattleData.Challenge_MonsterHideRange();
            if (this.bShowBullet != flag)
            {
                this.bShowBullet = flag;
                int layer = !flag ? LayerManager.Hide : LayerManager.Bullet;
                this.mTransform.ChangeChildLayer(layer);
            }
        }
        this.UpdateSpeedRatio();
        this.UpdateScale();
        this.TriggerTest();
        this.ShadowDeal();
        this.RotateDeal();
        this.CheckFar();
        if ((this.mCondition != null) && this.mCondition.IsEnd())
        {
            this.overDistance();
            this.mCondition = null;
        }
    }

    private void UpdateScale()
    {
        if (((this.m_Entity != null) && this.m_Entity.m_EntityData.GetBulletScale()) && (this.CurrentDistance <= 7f))
        {
            if (this.CurrentDistance <= 5f)
            {
                this.bulletscale = (((5f - this.CurrentDistance) / 5f) * 0.5f) + 1f;
            }
            else
            {
                this.bulletscale = 1f;
            }
            this.mTransform.localScale = Vector3.one * this.bulletscale;
            this.mBulletTransmit.AddAttackRatio(this.bulletscale);
        }
    }

    private void UpdateSpeedRatio()
    {
        if (!GameLogic.IsSameTeam(this.m_Entity, GameLogic.Self))
        {
            this.mSpeedRatio = GameLogic.Self.m_EntityData.GetBulletSpeedRatio(this);
        }
    }

    protected bool bMoveEnable
    {
        get => 
            this.bbMoveEnable;
        set => 
            (this.bbMoveEnable = value);
    }

    protected virtual bool bFlyCantHit =>
        false;

    public EntityBase m_Entity { get; private set; }

    protected float CurrentDistance { get; set; }

    protected float Distance
    {
        get => 
            this.mDistance;
        set => 
            (this.mDistance = value);
    }

    protected float Speed
    {
        get => 
            this.mSpeed;
        set => 
            (this.mSpeed = value);
    }

    protected float FrameDistance =>
        ((this.Speed * this.mSpeedRatio) * Updater.delta);

    public int bulletids { get; private set; }

    protected virtual bool bZScale =>
        false;

    public class BulletLine
    {
        private GameObject mBulletLine;
        private BulletLineCtrl mLineCtrl;
        private BulletBase mBullet;
        private BulletBase mLastBullet;

        private void CreateBulletLine()
        {
            this.mBulletLine = GameLogic.EffectGet("Effect/Attributes/BulletLine");
            this.mBulletLine.transform.SetParent(GameNode.m_PoolParent.transform);
            this.mLineCtrl = this.mBulletLine.GetComponent<BulletLineCtrl>();
            this.mLineCtrl.Init(this.mBullet, this.mLastBullet);
            this.mLineCtrl.mOverDistanceEvent = new Action(this.DeInit);
        }

        public void DeInit()
        {
            if (this.mLineCtrl != null)
            {
                this.mLineCtrl.mOverDistanceEvent = null;
                this.mLineCtrl.Cache();
                this.mLineCtrl = null;
            }
        }

        public void Init(BulletBase bullet, BulletBase lastbullet)
        {
            this.mBullet = bullet;
            this.mLastBullet = lastbullet;
            this.CreateBulletLine();
        }
    }

    protected class TrailCtrl
    {
        public bool bShow;
        private GameObject trail;
        private List<TrailRenderer> mTrailRenderers = new List<TrailRenderer>();
        private List<float> mTrailTime = new List<float>();
        private List<MeshRenderer> mTrailMeshs = new List<MeshRenderer>();
        private List<ParticleSystem> mTrailParticles = new List<ParticleSystem>();
        private List<TrailWidth> mTrailsWidth = new List<TrailWidth>();

        public TrailCtrl(Transform trail)
        {
            if (trail != null)
            {
                this.trail = trail.gameObject;
                this.InitTrailRenderer();
                this.InitTrailMesh();
                this.InitParticles();
            }
        }

        public void Clear()
        {
        }

        public float GetTrailTime()
        {
            if (this.mTrailRenderers.Count > 0)
            {
                return this.mTrailRenderers[0].time;
            }
            return 0f;
        }

        private void InitParticles()
        {
            ParticleSystem[] componentsInChildren = this.trail.GetComponentsInChildren<ParticleSystem>(true);
            int index = 0;
            int length = componentsInChildren.Length;
            while (index < length)
            {
                this.mTrailParticles.Add(componentsInChildren[index]);
                index++;
            }
        }

        private void InitTrailMesh()
        {
            MeshRenderer[] componentsInChildren = this.trail.GetComponentsInChildren<MeshRenderer>(true);
            int index = 0;
            int length = componentsInChildren.Length;
            while (index < length)
            {
                MeshRenderer item = componentsInChildren[index];
                item.sortingLayerName = "BulletEffect";
                item.sortingOrder = -1;
                this.mTrailMeshs.Add(item);
                index++;
            }
        }

        private void InitTrailRenderer()
        {
            TrailRenderer[] componentsInChildren = this.trail.GetComponentsInChildren<TrailRenderer>(true);
            int index = 0;
            int length = componentsInChildren.Length;
            while (index < length)
            {
                TrailRenderer item = componentsInChildren[index];
                this.mTrailTime.Add(item.time);
                item.sortingLayerName = "Hit";
                this.mTrailRenderers.Add(item);
                TrailWidth width = new TrailWidth {
                    startWidth = item.startWidth,
                    endWidth = item.endWidth
                };
                this.mTrailsWidth.Add(width);
                index++;
            }
        }

        public void SetTrailTime(float ratio)
        {
            int num = 0;
            int count = this.mTrailRenderers.Count;
            while (num < count)
            {
                if (this.mTrailTime.Count > num)
                {
                    this.mTrailRenderers[num].time = this.mTrailTime[num] * ratio;
                }
                num++;
            }
        }

        private void TrailMeshShow(bool show)
        {
            int num = 0;
            int count = this.mTrailMeshs.Count;
            while (num < count)
            {
                if (this.mTrailMeshs[num] != null)
                {
                    this.mTrailMeshs[num].enabled = show;
                }
                num++;
            }
        }

        private void TrailParticlesShow(bool show)
        {
            int num = 0;
            int count = this.mTrailParticles.Count;
            while (num < count)
            {
                ParticleSystem system = this.mTrailParticles[num];
                if (system != null)
                {
                    system.Clear();
                    system.SetParticles(null, 0);
                }
                num++;
            }
        }

        private void TrailRendererShow(bool show)
        {
            int num = 0;
            int count = this.mTrailRenderers.Count;
            while (num < count)
            {
                if (this.mTrailRenderers[num] != null)
                {
                    this.mTrailRenderers[num].Clear();
                }
                num++;
            }
        }

        public void TrailShow(bool show)
        {
            this.TrailRendererShow(show);
            this.TrailMeshShow(show);
            this.TrailParticlesShow(show);
            if (this.trail != null)
            {
                this.trail.SetActive(show);
            }
        }

        public void UpdateTrailWidthScale(float scale)
        {
            int num = 0;
            int count = this.mTrailRenderers.Count;
            while (num < count)
            {
                TrailWidth width = this.mTrailsWidth[num];
                this.mTrailRenderers[num].startWidth = width.startWidth * scale;
                TrailWidth width2 = this.mTrailsWidth[num];
                this.mTrailRenderers[num].endWidth = width2.endWidth * scale;
                num++;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct TrailWidth
        {
            public float startWidth;
            public float endWidth;
        }
    }

    public class TriggerData
    {
        private const float delaytime = 1f;
        private float lastintime;
        public GameObject target;
        public Collider collider;
        private int lastinframe;
        public int currentframe;

        public bool GetCanHit()
        {
            if (Time.frameCount == this.currentframe)
            {
                if (this.lastinframe == (this.currentframe - 1))
                {
                    this.lastinframe = this.currentframe;
                    if ((Updater.AliveTime - this.lastintime) > 1f)
                    {
                        this.lastintime++;
                        return true;
                    }
                }
                else
                {
                    this.lastinframe = this.currentframe;
                    this.lastintime = Updater.AliveTime;
                    return true;
                }
            }
            return false;
        }
    }
}

