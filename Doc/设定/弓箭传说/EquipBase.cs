using Dxx.Util;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using TableTool;
using UnityEngine;

public class EquipBase : PauseObject
{
    protected string ClassName;
    protected int ClassID;
    private int GoodId;
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private Goods_food <m_Data>k__BackingField;
    protected BoxCollider m_Box;
    protected Equip_equip m_Equip;
    protected GoodsDrop m_GoodsDrop;
    protected Vector3 EndPosition;
    protected bool bAbsorbImme;
    private Animator Ani_Rotate;
    private bool bStartAbsorb;
    [SerializeField]
    private float mAbsorbTime;
    private EntityHero AbsorbEntity;

    public void AbsorbEnd()
    {
        this.bStartAbsorb = false;
        GameLogic.EffectCache(base.gameObject);
    }

    private void Absorbing()
    {
        if (this.bStartAbsorb)
        {
            if ((this.AbsorbEntity == null) || this.AbsorbEntity.GetIsDead())
            {
                this.bStartAbsorb = false;
            }
            else if ((Updater.AliveTime - this.mAbsorbTime) < 0.17f)
            {
                base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y + (Updater.delta * 10f), base.transform.position.z);
            }
            else if ((Updater.AliveTime - this.mAbsorbTime) > 0.3f)
            {
                Vector3 vector4 = this.AbsorbEntity.transform.position - base.transform.position;
                Vector3 vector5 = (vector4.normalized * ((Updater.AliveTime - this.mAbsorbTime) - 0.3f)) * 2.5f;
                if (vector5.magnitude > 2.5f)
                {
                    vector5 = vector5.normalized * 2.5f;
                }
                Transform transform = base.transform;
                transform.position += vector5;
                if (Vector3.Distance(this.AbsorbEntity.position, base.transform.position) <= 1f)
                {
                    base.transform.position = this.AbsorbEntity.position;
                    this.AbsorbEntity.AbsorbEquips(this);
                    this.AbsorbEnd();
                }
            }
        }
    }

    private void Awake()
    {
        this.ClassName = base.GetType().ToString();
        this.ClassName = this.ClassName.Substring(this.ClassName.Length - 7, 7);
        this.ClassID = int.Parse(this.ClassName);
        this.m_Data = LocalModelManager.Instance.Goods_food.GetBeanById(this.ClassID);
        this.m_Box = base.GetComponent<BoxCollider>();
        Transform transform = base.transform.Find("child/rotate");
        if (transform != null)
        {
            this.Ani_Rotate = transform.GetComponent<Animator>();
        }
        this.AwakeInit();
    }

    protected virtual void AwakeInit()
    {
        this.m_GoodsDrop = base.gameObject.AddComponent<GoodsDrop>();
        this.m_GoodsDrop.SetDropEnd(new Action(this.DropEnd));
    }

    private void BeAbsorb(EntityHero _entity)
    {
        if ((!this.bStartAbsorb && (_entity != null)) && !_entity.GetIsDead())
        {
            this.AbsorbEntity = _entity;
            this.bStartAbsorb = true;
            this.mAbsorbTime = Updater.AliveTime;
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

    public Vector3 GetEndPosition() => 
        this.EndPosition;

    public void GetGoods(EntityBase entity)
    {
        this.m_Data.GetGoods(entity);
        this.OnGetGoods();
    }

    protected virtual void Init()
    {
    }

    protected virtual void OnAbsorb()
    {
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
        this.Init();
    }

    protected virtual void OnGetGoods()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((other.gameObject.layer == LayerManager.PlayerAbsorb) && !this.bAbsorbImme) || ((other.gameObject.layer == LayerManager.PlayerAbsorbImme) && this.bAbsorbImme))
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

    public virtual void SetEndPosition(Vector3 startpos, Vector3 endpos)
    {
        this.EndPosition = endpos;
        this.m_GoodsDrop.DropInitLast(startpos, endpos);
    }

    public virtual void SetEquip(Equip_equip equip)
    {
        this.m_Equip = equip;
    }

    private void Start()
    {
        this.StartInit();
    }

    protected virtual void StartInit()
    {
    }

    protected override void UpdateProcess()
    {
        this.Absorbing();
    }

    public Goods_food m_Data { get; protected set; }
}

