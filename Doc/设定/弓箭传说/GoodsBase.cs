using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using TableTool;
using UnityEngine;

public class GoodsBase : PauseObject
{
    protected string ClassName;
    protected int ClassID;
    private int GoodId;
    private TMXGoodsData GoodData;
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private Goods_goods <m_Data>k__BackingField;
    protected BoxCollider m_Box;
    protected Equip_equip m_Equip;
    protected GoodsDrop m_GoodsDrop;
    protected Vector3 EndPosition;
    protected bool bAbsorbImme;
    private Animator Ani_Rotate;

    private void Awake()
    {
        this.ClassName = base.GetType().ToString();
        this.ClassName = this.ClassName.Substring(this.ClassName.Length - 4, 4);
        this.ClassID = int.Parse(this.ClassName);
        this.m_Data = LocalModelManager.Instance.Goods_goods.GetBeanById(this.ClassID);
        this.m_Box = base.GetComponent<BoxCollider>();
        this.GoodData = new TMXGoodsData();
        this.GoodData.SetGoodsId(this.ClassID);
        this.GoodData.Init(this.m_Data.GoodsType);
        Transform transform = base.transform.Find("child/rotate");
        if (transform != null)
        {
            this.Ani_Rotate = transform.GetComponent<Animator>();
        }
        this.AwakeInit();
    }

    protected virtual void AwakeInit()
    {
    }

    public virtual void ChildTriggerEnter(GameObject o)
    {
    }

    public virtual void ChildTriggetExit(GameObject o)
    {
    }

    public bool GetAbsorbImme() => 
        this.bAbsorbImme;

    protected virtual void Init()
    {
    }

    protected virtual void OnAbsorb()
    {
    }

    protected virtual void OnDeInit()
    {
    }

    private void OnDisable()
    {
        this.OnDeInit();
    }

    private void OnEnable()
    {
        this.Init();
    }

    private void OnTriggerEnter(Collider other)
    {
        this.ChildTriggerEnter(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        this.ChildTriggetExit(other.gameObject);
    }

    private void Start()
    {
        this.StartInit();
    }

    protected virtual void StartInit()
    {
    }

    public Goods_goods m_Data { get; protected set; }
}

