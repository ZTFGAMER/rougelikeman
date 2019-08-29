using Dxx.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScrollCircle : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler, IEventSystemHandler
{
    public static Action OnDoubleClick;
    [SerializeField]
    private JoyNameType JoyType;
    private Dictionary<JoyNameType, string> JoyDic;
    protected Vector2 Origin;
    protected float mRadius;
    protected float mRadiusSmall;
    protected Transform child;
    protected Transform bgParent;
    protected Transform bgParengbgbg;
    protected Transform touch;
    protected Transform direction;
    private Vector3 StartPos;
    private bool bShowDirection;
    protected JoyData m_Data;
    private bool disable;
    private bool touchdown;
    private Vector3 touchdownpos;
    private static bool TouchIn = true;
    private int mTouchID;
    private float fClickTime;
    private float ClickDelayTime;
    private Animator mAni_ScreenTouch;
    private bool bDrag;
    private Vector3 pos_v;
    private float pos_w;
    private Vector3 pos_2;
    private Vector2 DealDrag_touchpos;
    private Vector2 DealDrag_touchpos1;

    [field: CompilerGenerated, DebuggerBrowsable(0)]
    public static  event JoyTouchEnd On_JoyTouchEnd;

    [field: CompilerGenerated, DebuggerBrowsable(0)]
    public static  event JoyTouching On_JoyTouching;

    [field: CompilerGenerated, DebuggerBrowsable(0)]
    public static  event JoyTouchStart On_JoyTouchStart;

    public ScrollCircle()
    {
        Dictionary<JoyNameType, string> dictionary = new Dictionary<JoyNameType, string> {
            { 
                JoyNameType.MoveJoy,
                "MoveJoy"
            },
            { 
                JoyNameType.AttackJoy,
                "AttackJoy"
            }
        };
        this.JoyDic = dictionary;
        this.bShowDirection = true;
        this.m_Data = new JoyData();
        this.mTouchID = -1;
        this.ClickDelayTime = 0.2f;
        this.pos_2 = new Vector3(0.5f, 0.5f, 0f);
    }

    private void Awake()
    {
        this.ClickDelayTime = SettingDebugMediator.DoubleClick;
        this.child = base.transform.Find("panel/bg");
        this.bgParent = this.child.transform.Find("bgParent");
        this.bgParengbgbg = this.bgParent.Find("bg/bg");
        this.touch = this.child.transform.Find("touch");
        this.direction = this.bgParent.transform.Find("direction");
        this.StartPos = this.child.localPosition;
        this.touch.localScale = (Vector3.one * SettingDebugMediator.JoyScaleTouch) / 100f;
        this.mRadius = (((this.child as RectTransform).sizeDelta.x * 0.5f) * SettingDebugMediator.JoyRadius) / 100f;
        this.mRadiusSmall = this.mRadius;
        if (TouchIn)
        {
            this.mRadiusSmall = this.mRadius - (((((this.touch as RectTransform).sizeDelta.x * 0.5f) * SettingDebugMediator.JoyRadius) / 100f) * this.touch.localScale.x);
        }
        this.mRadius *= ((float) SettingDebugMediator.JoyScaleBG) / 100f;
        this.mRadiusSmall *= ((float) SettingDebugMediator.JoyScaleBG) / 100f;
        this.m_Data.name = this.JoyDic[this.JoyType];
        if (this.m_Data.name == "MoveJoy")
        {
            this.m_Data.action = "Run";
        }
        this.bgParent.Find("bg/bg").localScale = (Vector3.one * SettingDebugMediator.JoyScaleBG) / 100f;
        this.direction.gameObject.SetActive(this.bShowDirection);
        SettingDebugMediator.OnValueChange = new Action(this.OnValueChange);
        this.m_Data.direction = new Vector3();
    }

    private void DealDrag(Vector2 pos, bool updateui = true)
    {
        this.DealDrag_touchpos = pos - this.Origin;
        if (this.DealDrag_touchpos.magnitude > this.mRadius)
        {
            this.DealDrag_touchpos = this.DealDrag_touchpos.normalized * this.mRadius;
        }
        this.DealDrag_touchpos1 = this.DealDrag_touchpos;
        if (this.DealDrag_touchpos1.magnitude > this.mRadiusSmall)
        {
            this.DealDrag_touchpos1 = this.DealDrag_touchpos1.normalized * this.mRadiusSmall;
        }
        this.m_Data.length = this.DealDrag_touchpos.magnitude;
        this.m_Data.direction.x = this.DealDrag_touchpos.normalized.x;
        this.m_Data.direction.z = this.DealDrag_touchpos.normalized.y * 1.23f;
        this.m_Data.angle = Utils.getAngle(this.m_Data.direction);
        if (updateui)
        {
            this.touch.localPosition = (Vector3) this.DealDrag_touchpos1;
            this.direction.localRotation = Quaternion.Euler(0f, 0f, -this.m_Data.angle);
        }
    }

    private Vector3 GetPos(Vector3 pos)
    {
        this.pos_v = GameNode.m_Camera.ScreenToViewportPoint(pos) - this.pos_2;
        this.pos_w = (((float) GameLogic.DesignHeight) / ((float) Screen.height)) * Screen.width;
        return new Vector3(this.pos_w * this.pos_v.x, GameLogic.DesignHeight * this.pos_v.y, 0f);
    }

    private void OnDisable()
    {
        this.OnPointerUp(null);
        this.disable = true;
        this.touchdown = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!this.disable && (this.mTouchID == eventData.pointerId))
        {
            if (!this.touchdown)
            {
                this.child.gameObject.SetActive(true);
                Vector3 pos = this.GetPos((Vector3) eventData.get_position());
                this.touchdownpos = pos;
                this.touchdown = true;
                this.child.localPosition = pos;
                this.Origin = pos;
                ProfilerHelper.BeginSample("Circle drag 1", Array.Empty<object>());
                this.DealDrag(this.Origin, true);
                if (On_JoyTouchStart != null)
                {
                    On_JoyTouchStart(this.m_Data);
                }
                ProfilerHelper.EndSample();
            }
            this.bDrag = true;
            ProfilerHelper.BeginSample("Circle drag 2", Array.Empty<object>());
            this.DealDrag(this.GetPos((Vector3) eventData.get_position()), true);
            if (On_JoyTouching != null)
            {
                On_JoyTouching(this.m_Data);
            }
            ProfilerHelper.EndSample();
        }
    }

    private void OnEnable()
    {
        this.disable = false;
        this.touchdown = false;
        (base.transform.parent as RectTransform).SetAsFirstSibling();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        this.mTouchID = eventData.pointerId;
        this.OnPointerDown(this.GetPos((Vector3) eventData.get_position()));
    }

    private void OnPointerDown(Vector3 pos)
    {
        if (!this.disable)
        {
            this.bDrag = false;
            if (OnDoubleClick != null)
            {
                GameObject obj2 = GameLogic.EffectGet("Game/UI/ScreenTouch");
                obj2.transform.SetParent(GameNode.m_Joy);
                obj2.transform.position = pos;
                if ((Updater.AliveTime - this.fClickTime) < this.ClickDelayTime)
                {
                    this.fClickTime = -1f;
                    if (GameLogic.Self != null)
                    {
                        OnDoubleClick();
                    }
                }
                else
                {
                    this.fClickTime = Updater.AliveTime;
                }
            }
            else
            {
                this.touchdownpos = pos;
                this.touchdown = true;
                this.child.localPosition = pos;
                this.child.gameObject.SetActive(true);
                this.Origin = pos;
                this.DealDrag(this.Origin, true);
                if (On_JoyTouchStart != null)
                {
                    On_JoyTouchStart(this.m_Data);
                }
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (((eventData != null) && !this.disable) && (this.mTouchID == eventData.pointerId))
        {
            this.touchdown = false;
            if (this.bDrag)
            {
                this.fClickTime = -1f;
            }
            if (On_JoyTouchEnd != null)
            {
                this.touch.localPosition = (Vector3) Vector2.zero;
                On_JoyTouchEnd(this.m_Data);
            }
            if (this.child != null)
            {
                this.child.localPosition = this.StartPos;
                this.direction.localRotation = Quaternion.identity;
            }
        }
    }

    private void OnValueChange()
    {
        this.mRadius = (((this.child as RectTransform).sizeDelta.x * 0.5f) * SettingDebugMediator.JoyRadius) / 100f;
        this.mRadiusSmall = this.mRadius;
        if (TouchIn)
        {
            this.mRadiusSmall = this.mRadius - (((((this.touch as RectTransform).sizeDelta.x * 0.5f) * SettingDebugMediator.JoyRadius) / 100f) * this.touch.localScale.x);
        }
        this.mRadius *= ((float) SettingDebugMediator.JoyScaleBG) / 100f;
        this.mRadiusSmall *= ((float) SettingDebugMediator.JoyScaleBG) / 100f;
        this.bgParengbgbg.localScale = (Vector3.one * SettingDebugMediator.JoyScaleBG) / 100f;
    }

    public delegate void JoyTouchEnd(JoyData data);

    public delegate void JoyTouching(JoyData data);

    public delegate void JoyTouchStart(JoyData data);
}

