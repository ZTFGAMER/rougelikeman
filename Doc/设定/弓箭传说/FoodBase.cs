using Dxx.Util;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using TableTool;
using UnityEngine;

public class FoodBase : PauseObject
{
    public int FoodID;
    protected string ClassName;
    protected int ClassID;
    private int GoodId;
    protected object data;
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private Goods_food <m_Data>k__BackingField;
    protected BoxCollider m_Box;
    protected Equip_equip m_Equip;
    protected GoodsDrop m_GoodsDrop;
    protected Vector3 EndPosition;
    protected bool bAbsorbImme;
    protected Animator Ani_Rotate;
    private GameObject trail;
    private bool bTrailShow;
    protected MeshRenderer[] meshes;
    private float flyStartDelayTime;
    protected float flyTime = 0.17f;
    protected float flyDelayTime = 0.3f;
    protected float flySpeed = 1f;
    private Vector3 mflyspeed;
    private Vector3 mflydir;
    private static AnimationCurve _curve;
    private bool bStartAbsorb;
    private float mAbsorbStartTime;
    private float mAbsoryUpdateTime;
    private EntityHero AbsorbEntity;
    private float flypercent;
    private float lastDis;
    private float tempdis;
    private const float maxspeed = 1f;
    private const float maxdis = 0.7f;
    private float foodAngle;
    protected bool bFlyRotate = true;
    private float startscalez;

    public void AbsorbEnd()
    {
        this.OnGetGoodsEnd();
        this.bStartAbsorb = false;
        GameLogic.EffectCache(base.gameObject);
    }

    private void Absorbing()
    {
        if (this.bStartAbsorb && ((this.AbsorbEntity != null) && !this.AbsorbEntity.GetIsDead()))
        {
            if (this.ClassID == 0x3e9)
            {
            }
            this.mAbsoryUpdateTime += Updater.delta;
            if ((this.mAbsoryUpdateTime - this.mAbsorbStartTime) >= this.flyStartDelayTime)
            {
                if ((this.mAbsoryUpdateTime - this.mAbsorbStartTime) < (this.flyTime + this.flyStartDelayTime))
                {
                    this.flypercent = ((this.mAbsoryUpdateTime - this.mAbsorbStartTime) - this.flyStartDelayTime) / this.flyTime;
                    base.transform.position = new Vector3(base.transform.position.x, curve.Evaluate(this.flypercent) * 1.7f, base.transform.position.z);
                }
                else if ((this.mAbsoryUpdateTime - this.mAbsorbStartTime) > (this.flyDelayTime + this.flyStartDelayTime))
                {
                    this.TrailShow(true);
                    this.foodAngle = Utils.getAngle(this.AbsorbEntity.position - base.transform.position);
                    if (this.bFlyRotate)
                    {
                        base.transform.localRotation = Quaternion.Euler(0f, this.foodAngle, 0f);
                    }
                    this.mflydir = this.AbsorbEntity.transform.position - base.transform.position;
                    this.mflyspeed = ((this.mflydir.normalized * (((this.mAbsoryUpdateTime - this.mAbsorbStartTime) - this.flyDelayTime) - this.flyStartDelayTime)) * 2.5f) * this.flySpeed;
                    if (this.mflyspeed.magnitude > 1f)
                    {
                        this.mflyspeed = this.mflyspeed.normalized * 1f;
                    }
                    this.startscalez += this.mflyspeed.magnitude / 2f;
                    this.SetTrailScaleZ(this.startscalez);
                    Transform transform = base.transform;
                    transform.position += this.mflyspeed;
                    this.tempdis = Vector3.Distance(this.AbsorbEntity.position, base.transform.position);
                    if (this.tempdis <= 0.7f)
                    {
                        base.transform.position = this.AbsorbEntity.position;
                        this.TrailShow(false);
                        this.AbsorbEntity.AbsorbFoods(this);
                        this.AbsorbEnd();
                    }
                    this.lastDis = this.tempdis;
                }
            }
        }
    }

    private void Awake()
    {
        this.m_GoodsDrop = base.gameObject.AddComponent<GoodsDrop>();
        this.m_GoodsDrop.SetDropEnd(new Action(this.DropEnd));
        this.InitTrail();
        this.OnAwakeInit();
    }

    private void BeAbsorb(EntityHero _entity)
    {
        if (!this.bStartAbsorb && (_entity != null))
        {
            this.lastDis = 2.147484E+09f;
            this.AbsorbEntity = _entity;
            this.bStartAbsorb = true;
            this.startscalez = 0f;
            this.SetTrailScaleZ(this.startscalez);
            this.mAbsorbStartTime = Updater.AliveTime;
            this.mAbsoryUpdateTime = this.mAbsorbStartTime;
            this.OnAbsorbStart();
        }
    }

    public virtual void ChildTriggerEnter(GameObject o)
    {
    }

    public virtual void ChildTriggetExit(GameObject o)
    {
    }

    private void DropEnd()
    {
        this.OnDropEnd();
    }

    public bool GetAbsorbImme() => 
        this.bAbsorbImme;

    public bool GetAbsorbing() => 
        this.bStartAbsorb;

    public object GetData() => 
        this.data;

    public Vector3 GetEndPosition() => 
        this.EndPosition;

    public void GetGoods(EntityBase entity)
    {
        this.OnGetGoods(entity);
    }

    public void Init(object data)
    {
        this.data = data;
        this.flyStartDelayTime = 0f;
        this.OnInit();
    }

    private void InitTrail()
    {
        Transform transform = base.transform.Find("child/trail");
        if (transform != null)
        {
            this.trail = transform.gameObject;
            this.bTrailShow = true;
            this.TrailShow(false);
        }
    }

    protected virtual void OnAbsorb()
    {
    }

    protected virtual void OnAbsorbStart()
    {
    }

    protected virtual void OnAwakeInit()
    {
        if (base.GetType() == typeof(FoodBase))
        {
            this.ClassID = this.FoodID;
        }
        else
        {
            this.ClassName = base.GetType().ToString();
            this.ClassName = this.ClassName.Substring(this.ClassName.Length - 4, 4);
            this.ClassID = int.Parse(this.ClassName);
            this.FoodID = this.ClassID;
        }
        this.m_Data = LocalModelManager.Instance.Goods_food.GetBeanById(this.ClassID);
        this.m_Box = base.GetComponent<BoxCollider>();
        this.meshes = base.transform.GetComponentsInChildren<MeshRenderer>(true);
        for (int i = 0; i < this.meshes.Length; i++)
        {
            this.meshes[i].sortingLayerName = "Hit";
            this.meshes[i].sortingOrder = this.ClassID;
        }
        Transform transform = base.transform.Find("child/rotate");
        if (transform != null)
        {
            this.Ani_Rotate = transform.GetComponent<Animator>();
            Transform transform2 = transform.Find("GameObject/mesh");
            if ((transform2 != null) && (this.Ani_Rotate != null))
            {
                transform2.parent.localRotation = Quaternion.Euler(0f, GameLogic.Random((float) 0f, (float) 360f), 0f);
            }
        }
    }

    protected virtual void OnDeInit()
    {
    }

    private void OnDestroy()
    {
        this.OnDeInit();
    }

    private void OnDisable()
    {
        if (this.Ani_Rotate != null)
        {
            this.Ani_Rotate.enabled = false;
        }
    }

    protected virtual void OnDropEnd()
    {
    }

    private void OnEnable()
    {
        if (this.Ani_Rotate != null)
        {
            this.Ani_Rotate.enabled = true;
        }
        this.OnEnables();
    }

    protected virtual void OnEnables()
    {
    }

    protected virtual void OnGetGoods(EntityBase entity)
    {
        this.m_Data.GetGoods(entity);
    }

    protected virtual void OnGetGoodsEnd()
    {
    }

    protected virtual void OnInit()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((other.gameObject.layer == LayerManager.PlayerAbsorb) || (other.gameObject.layer == LayerManager.PlayerAbsorbImme)) && (((other.gameObject.layer == LayerManager.PlayerAbsorb) && !this.bAbsorbImme) || ((other.gameObject.layer == LayerManager.PlayerAbsorbImme) && this.bAbsorbImme)))
        {
            this.OnAbsorb();
            this.BeAbsorb(GameLogic.Self);
        }
        this.ChildTriggerEnter(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        this.ChildTriggetExit(other.gameObject);
    }

    protected void RotateEnable(bool value)
    {
        if (this.Ani_Rotate != null)
        {
            this.Ani_Rotate.enabled = value;
        }
    }

    public virtual void SetEndPosition(Vector3 startpos, Vector3 endpos)
    {
        this.EndPosition = endpos;
        this.m_GoodsDrop.DropInitLast(startpos, endpos);
    }

    public virtual void SetEquip(Equip_equip equip)
    {
        this.m_Equip = equip;
    }

    private void SetTrailScaleZ(float scalez)
    {
        if (this.trail != null)
        {
            scalez = MathDxx.Clamp01(scalez);
            this.trail.transform.localScale = new Vector3(1f, 1f, scalez);
        }
    }

    private void Start()
    {
        this.StartInit();
    }

    protected virtual void StartInit()
    {
    }

    private void TrailShow(bool show)
    {
        if (this.bTrailShow != show)
        {
            this.bTrailShow = show;
            if (this.trail != null)
            {
                this.trail.SetActive(show);
            }
        }
    }

    protected override void UpdateProcess()
    {
        this.Absorbing();
    }

    public Goods_food m_Data { get; protected set; }

    private static AnimationCurve curve
    {
        get
        {
            if (_curve == null)
            {
                _curve = LocalModelManager.Instance.Curve_curve.GetCurve(0x186b5);
            }
            return _curve;
        }
    }
}

