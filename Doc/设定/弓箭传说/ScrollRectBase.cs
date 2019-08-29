using Dxx.Util;
using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollRectBase : ScrollRect, IPointerClickHandler, IEventSystemHandler
{
    public Action<PointerEventData> BeginDrag;
    public Action<PointerEventData> Drag;
    public Action<PointerEventData> EndDrag;
    public Action OnClick;
    public Action<int> EndDragItem;
    public Action<Vector2> ValueChanged;
    private float scrollpercent;
    public bool UseWhole;
    private bool _usegrag = true;
    public bool DragDisableForce;
    public bool bUseScrollEvent = true;
    public float SpeedCritical = 20f;
    public float Whole_PerOne;
    public int Whole_Count;
    public float AllWidth;
    private bool _dragging;
    private bool _sendfinish;
    private bool bUpdateEnd;
    private float speed;
    private int currentPage;
    private float scrollendpos;
    private Action mPageAniFinish;
    private bool[] mLocks;
    private bool bGotoStart;
    private float mGotoValue;
    private float mGotoTemp;

    protected override void Awake()
    {
        base.Awake();
        base.onValueChanged = new ScrollRect.ScrollRectEvent();
        base.onValueChanged.AddListener(new UnityAction<Vector2>(this.OnValueChanged));
    }

    private int GetNextUnlock(int currentindex, bool left)
    {
        if (this.mLocks == null)
        {
            if (left)
            {
                return (currentindex - 1);
            }
            return (currentindex + 1);
        }
        int num = currentindex;
        int num2 = !left ? 1 : -1;
        currentindex += num2;
        while ((currentindex >= 0) && (currentindex < this.mLocks.Length))
        {
            if (this.mLocks[currentindex])
            {
                currentindex += num2;
            }
            else
            {
                return currentindex;
            }
        }
        return num;
    }

    private int GetPage()
    {
        int num = (int) (this.scrollpercent * this.Whole_Count);
        return Mathf.Clamp(num, 0, this.Whole_Count - 1);
    }

    public void Goto(float value, bool playanimation = false)
    {
        if (!playanimation)
        {
            if (base.horizontal)
            {
                base.get_content().anchoredPosition = new Vector2(value, base.get_content().anchoredPosition.y);
                this.mGotoTemp = base.get_content().anchoredPosition.x;
            }
            else
            {
                base.get_content().anchoredPosition = new Vector2(base.get_content().anchoredPosition.x, value);
                this.mGotoTemp = base.get_content().anchoredPosition.y;
            }
        }
        else
        {
            if (base.horizontal)
            {
                this.mGotoTemp = base.get_content().anchoredPosition.x;
            }
            else
            {
                this.mGotoTemp = base.get_content().anchoredPosition.y;
            }
            this.mGotoValue = value;
            this.bGotoStart = true;
        }
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (this.bUseScrollEvent && this.UseDrag)
        {
            this.OnBeginDragInternal(eventData);
        }
    }

    public void OnBeginDragInternal(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        if (this.UseWhole)
        {
            this.OnBeginDragWhole(eventData);
        }
        if (this.BeginDrag != null)
        {
            this.BeginDrag(eventData);
        }
    }

    private void OnBeginDragWhole(PointerEventData eventData)
    {
        this.bDragging = true;
    }

    protected override void OnDisable()
    {
        this.OnDisableWhole();
    }

    private void OnDisableWhole()
    {
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (this.bUseScrollEvent && this.UseDrag)
        {
            this.OnDragInternal(eventData);
        }
    }

    public void OnDragInternal(PointerEventData eventData)
    {
        base.OnDrag(eventData);
        if (this.UseWhole)
        {
            this.OnDragWhole(eventData);
        }
        if (this.Drag != null)
        {
            this.Drag(eventData);
        }
    }

    private void OnDragWhole(PointerEventData eventData)
    {
        this.speed = eventData.get_delta().x;
    }

    protected override void OnEnable()
    {
        this.OnEnableWhole();
    }

    private void OnEnableWhole()
    {
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (this.bUseScrollEvent && this.UseDrag)
        {
            this.OnEndDragInternal(eventData);
        }
    }

    public void OnEndDragInternal(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        if (this.UseWhole)
        {
            this.OnEndDragWhole(eventData);
        }
        if (this.EndDrag != null)
        {
            this.EndDrag(eventData);
        }
    }

    private void OnEndDragWhole(PointerEventData eventData)
    {
        int currentPage = this.currentPage;
        if (Mathf.Abs(this.speed) < this.SpeedCritical)
        {
            int page = this.GetPage();
            if ((((this.mLocks != null) && (page >= 0)) && ((page < this.mLocks.Length) && !this.mLocks[page])) || (this.mLocks == null))
            {
                this.currentPage = page;
            }
        }
        else
        {
            if (this.speed > 0f)
            {
                this.currentPage = this.GetNextUnlock(this.currentPage, true);
            }
            else
            {
                this.currentPage = this.GetNextUnlock(this.currentPage, false);
            }
            this.currentPage = Mathf.Clamp(this.currentPage, 0, this.Whole_Count - 1);
        }
        this.bUpdateEnd = false;
        this.bDragging = false;
        this.speed = 0f;
        this.UpdateScrollEndPos();
        if (this.EndDragItem != null)
        {
            this.EndDragItem(this.currentPage);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (this.OnClick != null)
        {
            this.OnClick();
        }
    }

    private void OnUpdateGoto()
    {
        if (this.bGotoStart)
        {
            if (MathDxx.Abs((float) (this.mGotoTemp - this.mGotoValue)) < 3f)
            {
                this.bGotoStart = false;
            }
            if (base.horizontal)
            {
                this.mGotoTemp = Mathf.Lerp(base.get_content().anchoredPosition.x, this.mGotoValue, 0.2f);
                base.get_content().anchoredPosition = new Vector2(this.mGotoTemp, base.get_content().anchoredPosition.y);
            }
            else
            {
                this.mGotoTemp = Mathf.Lerp(base.get_content().anchoredPosition.y, this.mGotoValue, 0.2f);
                base.get_content().anchoredPosition = new Vector2(base.get_content().anchoredPosition.x, this.mGotoTemp);
            }
        }
    }

    private void OnUpdateWhole()
    {
        if (!this.bDragging && !this.bSendFinish)
        {
            if (base.horizontal)
            {
                base.horizontalNormalizedPosition = Mathf.Lerp(base.horizontalNormalizedPosition, this.scrollendpos, 7f * Updater.delta);
                if ((Mathf.Abs((float) (base.horizontalNormalizedPosition - this.scrollendpos)) * base.get_content().sizeDelta.x) < 2f)
                {
                    base.horizontalNormalizedPosition = this.scrollendpos;
                    this.bUpdateEnd = true;
                }
            }
            if (base.vertical)
            {
                base.verticalNormalizedPosition = Mathf.Lerp(base.verticalNormalizedPosition, this.scrollendpos, 7f * Updater.delta);
                if ((Mathf.Abs((float) (base.verticalNormalizedPosition - this.scrollendpos)) * base.get_content().sizeDelta.y) < 2f)
                {
                    base.verticalNormalizedPosition = this.scrollendpos;
                    this.bUpdateEnd = true;
                }
            }
            if (this.bUpdateEnd)
            {
                if (this.mPageAniFinish != null)
                {
                    this.mPageAniFinish();
                }
                this.bSendFinish = true;
            }
        }
    }

    private void OnValueChanged(Vector2 value)
    {
        this.scrollpercent = value.x;
        if (this.ValueChanged != null)
        {
            this.ValueChanged(value);
        }
    }

    public void RemoveAllListeners()
    {
        if (base.onValueChanged != null)
        {
            base.onValueChanged.RemoveAllListeners();
        }
    }

    public void SetLocks(bool[] locks)
    {
        this.mLocks = locks;
    }

    public void SetPage(int page, bool animate, Action onFinish = null)
    {
        this.mPageAniFinish = onFinish;
        this.currentPage = page;
        if (!animate)
        {
            if (base.horizontal)
            {
                this.UpdateScrollEndPos();
                base.horizontalNormalizedPosition = this.scrollendpos;
            }
            if (base.vertical)
            {
                this.UpdateScrollEndPos();
                base.verticalNormalizedPosition = this.scrollendpos;
            }
        }
        else
        {
            this.bSendFinish = false;
            this.bUpdateEnd = false;
        }
        this.UpdateScrollEndPos();
    }

    public void SetWhole(GridLayoutGroup grid, int count)
    {
        this.UseWhole = true;
        if (base.horizontal)
        {
            this.Whole_PerOne = grid.get_cellSize().x;
        }
        else
        {
            this.Whole_PerOne = grid.get_cellSize().y;
        }
        this.Whole_Count = count;
        this.AllWidth = this.Whole_PerOne * (this.Whole_Count - 1);
        base.get_content().sizeDelta = new Vector2(this.AllWidth, 0f);
    }

    private void Update()
    {
        if (this.UseWhole)
        {
            this.OnUpdateWhole();
        }
        this.OnUpdateGoto();
    }

    private void UpdateScrollEndPos()
    {
        this.scrollendpos = (this.Whole_PerOne * this.currentPage) / this.AllWidth;
    }

    public bool UseDrag
    {
        get
        {
            if (this.DragDisableForce)
            {
                return false;
            }
            return this._usegrag;
        }
        set
        {
            this._usegrag = value;
            if (!value)
            {
                this.bDragging = false;
            }
        }
    }

    private bool bDragging
    {
        get => 
            this._dragging;
        set
        {
            this._dragging = value;
            if (this._dragging)
            {
                this.bSendFinish = false;
            }
        }
    }

    private bool bSendFinish
    {
        get => 
            this._sendfinish;
        set => 
            (this._sendfinish = value);
    }
}

