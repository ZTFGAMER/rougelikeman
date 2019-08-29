using DG.Tweening;
using Dxx.Util;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScrollIntCtrl<T> : ScrollRectBase where T: Component
{
    public GameObject copyItem;
    public Transform mScrollChild;
    [Header("滚动加速系数")]
    public float Speed;
    public Action<int, T> OnUpdateOne;
    public Action<int, T> OnUpdateSize;
    public Action<int, T> OnScrollEnd;
    public Action OnBeginDragEvent;
    public float maxScale;
    public float minScale;
    private bool bInit;
    private int showCount;
    private int count;
    private float allWidth;
    private float itemWidth;
    private float offsetx;
    private float lastscrollpos;
    private float lastspeed;
    private int mCurrentIndex;
    private LocalUnityObjctPool mObjPool;
    private List<ScrollData<T>> mList;
    private Sequence seq;
    private int mGotoIntIndex;

    public ScrollIntCtrl()
    {
        this.Speed = 3f;
        this.maxScale = 1.5f;
        this.minScale = 1f;
        this.showCount = 10;
        this.count = 40;
        this.offsetx = 360f;
        this.mList = new List<ScrollData<T>>();
    }

    protected override void Awake()
    {
        base.Awake();
        this.offsetx = (base.transform as RectTransform).sizeDelta.x / 2f;
        base.BeginDrag = new Action<PointerEventData>(this.OnDragBegin);
        base.Drag = new Action<PointerEventData>(this.OnDrags);
        base.EndDrag = new Action<PointerEventData>(this.OnDragEnd);
    }

    public void DeInit()
    {
        if (this.mObjPool != null)
        {
            this.mObjPool.Collect<T>();
        }
        Updater.RemoveUpdateUI(new Action<float>(this.OnUpdate));
        if (this.seq != null)
        {
            TweenExtensions.Kill(this.seq, false);
        }
    }

    public void GotoInt(int index, bool playanimation = false)
    {
        if ((index < this.mList.Count) && (index >= 0))
        {
            if (!playanimation)
            {
                float x = -this.mList[index].normalize * this.allWidth;
                base.get_content().localPosition = new Vector3(x, 0f, 0f);
                this.mCurrentIndex = index;
                this.UpdateSize();
                if (this.OnScrollEnd != null)
                {
                    this.OnScrollEnd(index, this.mList[index].one);
                }
            }
            else
            {
                <GotoInt>c__AnonStorey0<T> storey = new <GotoInt>c__AnonStorey0<T> {
                    $this = this
                };
                this.mGotoIntIndex = index;
                storey.posx = -this.mList[this.mCurrentIndex].normalize * this.allWidth;
                base.get_content().localPosition = new Vector3(storey.posx, 0f, 0f);
                float normalize = this.mList[this.mGotoIntIndex].normalize;
                storey.nextxx = -normalize * this.allWidth;
                float x = base.get_content().localPosition.x;
                storey.starth = this.mList[this.mCurrentIndex].normalize;
                storey.endh = this.mList[this.mGotoIntIndex].normalize;
                TweenSettingsExtensions.OnComplete<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(TweenSettingsExtensions.OnUpdate<Tweener>(ShortcutExtensions.DOLocalMoveX(base.get_content(), storey.nextxx, 0.5f, false), new TweenCallback(storey, this.<>m__0)), 6), new TweenCallback(storey, this.<>m__1));
            }
        }
    }

    public void Init(int count)
    {
        if (!this.bInit)
        {
            this.InitOnce();
            this.bInit = true;
        }
        this.mObjPool.Collect<T>();
        base.horizontalNormalizedPosition = 0f;
        base.set_velocity(Vector2.zero);
        this.lastscrollpos = -1f;
        this.mCurrentIndex = 0;
        this.lastspeed = 0f;
        this.mList.Clear();
        this.count = count;
        this.allWidth = (count - 1) * this.itemWidth;
        base.get_content().sizeDelta = new Vector2(this.allWidth + (this.offsetx * 2f), base.get_content().sizeDelta.y);
        for (int i = 0; i < count; i++)
        {
            ScrollData<T> item = new ScrollData<T>(i, null) {
                maxScale = this.maxScale,
                minScale = this.minScale
            };
            this.mList.Add(item);
            if (i < this.showCount)
            {
                T local = this.mObjPool.DeQueue<T>();
                if (this.OnUpdateOne != null)
                {
                    this.OnUpdateOne(i, local);
                }
                this.UpdateOne(i, local);
            }
            else
            {
                this.UpdateOne(i, null);
            }
        }
        this.UpdateSize();
        if ((this.mCurrentIndex < this.mList.Count) && (this.OnScrollEnd != null))
        {
            this.OnScrollEnd(this.mCurrentIndex, this.mList[this.mCurrentIndex].one);
        }
    }

    public void InitOnce()
    {
        this.mObjPool = LocalUnityObjctPool.Create(base.gameObject);
        this.mObjPool.CreateCache<T>(this.copyItem);
        this.itemWidth = (this.copyItem.transform as RectTransform).sizeDelta.x;
    }

    private void OnDragBegin(PointerEventData eventData)
    {
        if (this.OnBeginDragEvent != null)
        {
            this.OnBeginDragEvent();
        }
        Updater.RemoveUpdateUI(new Action<float>(this.OnUpdate));
    }

    private void OnDragEnd(PointerEventData eventData)
    {
        base.set_velocity(base.get_velocity() * this.Speed);
        Updater.RemoveUpdateUI(new Action<float>(this.OnUpdate));
        Updater.AddUpdateUI(new Action<float>(this.OnUpdate), false);
    }

    private void OnDrags(PointerEventData eventData)
    {
        this.UpdateSize();
    }

    private void OnUpdate(float delta)
    {
        this.UpdateScroll();
        this.lastspeed = base.get_velocity().x;
    }

    public void SetScale(float min, float max)
    {
        this.minScale = min;
        this.maxScale = max;
    }

    private void UpdateInfinity()
    {
        if ((Mathf.Abs((float) (base.get_velocity().x - this.lastspeed)) < 50f) && (this.mCurrentIndex < this.mList.Count))
        {
            float horizontalNormalizedPosition = base.horizontalNormalizedPosition;
            base.horizontalNormalizedPosition = this.mList[this.mCurrentIndex].normalize;
            float x = (base.horizontalNormalizedPosition - horizontalNormalizedPosition) * this.allWidth;
            this.mScrollChild.transform.localPosition = new Vector3(x, 0f, 0f);
            Updater.RemoveUpdateUI(new Action<float>(this.OnUpdate));
            Tweener tweener = TweenSettingsExtensions.OnUpdate<Tweener>(ShortcutExtensions.DOLocalMoveX(this.mScrollChild, (x <= 0f) ? ((float) 10) : ((float) (-10)), 0.2f, false), new TweenCallback(this, this.UpdateSize));
            Tweener tweener2 = TweenSettingsExtensions.OnUpdate<Tweener>(ShortcutExtensions.DOLocalMoveX(this.mScrollChild, 0f, 0.2f, false), new TweenCallback(this, this.UpdateSize));
            this.seq = TweenSettingsExtensions.Append(TweenSettingsExtensions.Append(DOTween.Sequence(), tweener), tweener2);
            if (this.OnScrollEnd != null)
            {
                this.OnScrollEnd(this.mCurrentIndex, this.mList[this.mCurrentIndex].one);
            }
        }
    }

    private void UpdateOne(int i, T one)
    {
        ScrollData<T> data = this.mList[i];
        data.Refresh(i, one);
        if (one != null)
        {
            one.transform.SetParentNormal(this.mScrollChild);
            RectTransform transform = one.transform as RectTransform;
            transform.anchoredPosition = new Vector2((i * this.itemWidth) + this.offsetx, 0f);
        }
        if (this.count > 1)
        {
            data.normalize = ((float) i) / (this.count - 1f);
            data.normalize_range = this.itemWidth / this.allWidth;
        }
        else if (this.count == 1)
        {
            data.normalize = 0f;
            data.normalize_range = 0f;
        }
    }

    private void UpdateScroll()
    {
        this.UpdateSize();
        this.UpdateInfinity();
    }

    private void UpdateSize()
    {
        if (this.lastscrollpos != base.horizontalNormalizedPosition)
        {
            this.lastscrollpos = base.horizontalNormalizedPosition;
            this.lastscrollpos = Mathf.Clamp01(this.lastscrollpos);
            float num = 0f;
            int i = 0;
            int count = this.mList.Count;
            while (i < count)
            {
                ScrollData<T> data = this.mList[i];
                if (Mathf.Abs((float) ((this.lastscrollpos - data.normalize) * this.allWidth)) > 600f)
                {
                    if (data.one != null)
                    {
                        this.mObjPool.EnQueue<T>(data.one.gameObject);
                        data.Miss();
                    }
                }
                else if (data.one == null)
                {
                    this.UpdateOne(i, this.mObjPool.DeQueue<T>());
                    if (this.OnUpdateOne != null)
                    {
                        this.OnUpdateOne(i, data.one);
                    }
                }
                float num2 = data.UpdateScale(this.lastscrollpos);
                if (num < num2)
                {
                    num = num2;
                    this.mCurrentIndex = i;
                }
                i++;
            }
            if (this.mCurrentIndex < this.mList.Count)
            {
                this.mList[this.mCurrentIndex].SetFront();
                if (this.OnUpdateSize != null)
                {
                    this.OnUpdateSize(this.mCurrentIndex, this.mList[this.mCurrentIndex].one);
                }
            }
        }
    }

    [CompilerGenerated]
    private sealed class <GotoInt>c__AnonStorey0
    {
        internal float posx;
        internal float nextxx;
        internal float endh;
        internal float starth;
        internal ScrollIntCtrl<T> $this;

        internal void <>m__0()
        {
            this.$this.lastscrollpos = -1f;
            float num = MathDxx.Abs((float) ((this.$this.get_content().localPosition.x - this.posx) / (this.nextxx - this.posx)));
            this.$this.horizontalNormalizedPosition = ((this.endh - this.starth) * num) + this.starth;
            this.$this.UpdateSize();
        }

        internal void <>m__1()
        {
            this.$this.mCurrentIndex = this.$this.mGotoIntIndex;
            if (this.$this.OnScrollEnd != null)
            {
                this.$this.OnScrollEnd(this.$this.mCurrentIndex, this.$this.mList[this.$this.mCurrentIndex].one);
            }
        }
    }

    public class ScrollData
    {
        public float maxScale;
        public float minScale;
        public T one;
        public RectTransform transform;
        public int index;
        public float normalize;
        public float normalize_range;
        private float scale;
        private float scalex;

        public ScrollData(int index, T one)
        {
            this.maxScale = 1.5f;
            this.minScale = 1f;
            this.Refresh(index, one);
        }

        public void Miss()
        {
            this.one = null;
            this.transform = null;
        }

        public void Refresh(int index, T one)
        {
            this.index = index;
            this.one = one;
            if (one != null)
            {
                this.transform = one.transform as RectTransform;
            }
        }

        public void SetFront()
        {
            this.transform.SetAsLastSibling();
        }

        public float UpdateScale(float normalizepos)
        {
            if ((this.one == null) || (this.transform == null))
            {
                return 0f;
            }
            if (this.normalize_range > 0f)
            {
                this.scale = Mathf.Abs((float) (this.normalize - normalizepos)) / this.normalize_range;
                this.scale = Mathf.Clamp01(this.scale);
            }
            else
            {
                this.scale = 0f;
            }
            this.scalex = this.maxScale - (this.scale * (this.maxScale - this.minScale));
            if (this.transform.localScale.x != this.scalex)
            {
                this.transform.localScale = Vector3.one * this.scalex;
            }
            return this.scalex;
        }
    }
}

