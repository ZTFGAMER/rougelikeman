namespace Dxx.UI
{
    using Dxx;
    using Dxx.Util;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class InfinityScrollGroup : UIBehaviour, ILayoutElement
    {
        [Header("<Infinity>"), AliasName("子物体", true), Tooltip("复制用子物体.")]
        public GameObject copyItemChild;
        [AliasName("数据个数", true), Tooltip("数据个数.必须大于0")]
        public int itemCount;
        public ScrollRect scrollRect;
        [Header("<Layout>")]
        public RectOffset padding = new RectOffset();
        public Vector2 cellSize = new Vector2(100f, 100f);
        public Vector2 spacing = Vector2.zero;
        public Axis sortAxis;
        [Tooltip("必须大于1")]
        public int constraintCount = 1;
        [Header("滚动最小长度，0为不限制")]
        public float MinScrollLength;
        protected Action<int, GameObject> updateChildCallBack;
        protected Dictionary<Type, ComponentAction> updateChildComponentCallBack = new Dictionary<Type, ComponentAction>();
        public Action<Vector2> onSizeChange;
        protected List<RectTransform> rectChildren = new List<RectTransform>();
        protected Dictionary<GameObject, Component[]> objToCompnent = new Dictionary<GameObject, Component[]>();
        protected DrivenRectTransformTracker m_Tracker;
        private int lastRowIndex;
        [NonSerialized]
        private RectTransform m_Rect;
        private List<GameObject> childCache = new List<GameObject>();
        private Vector2 m_TotalMinSize = Vector2.zero;
        private Vector2 m_TotalPreferredSize = Vector2.zero;
        private Vector2 m_TotalFlexibleSize = Vector2.zero;

        protected override void Awake()
        {
            this.scrollRect.onValueChanged.AddListener(edata => this.Scroll(edata));
            base.Awake();
        }

        public virtual void CalculateLayoutInputHorizontal()
        {
        }

        public virtual void CalculateLayoutInputVertical()
        {
        }

        private float ContentPositionToRowIndex(Vector2 pos)
        {
            Vector2 vector = new Vector2 {
                x = -pos.x - this.padding.left,
                y = pos.y - this.padding.top
            };
            float num = vector.x / (this.cellSize.x + this.spacing.x);
            float num2 = vector.y / (this.cellSize.y + this.spacing.y);
            if (this.sortAxis == Axis.Horizontal)
            {
                return num2;
            }
            if (this.sortAxis == Axis.Vertical)
            {
                return num;
            }
            return 0f;
        }

        private void CreateNewDisplayChild()
        {
            GameObject obj2;
            if (this.childCache.Count > 0)
            {
                obj2 = this.childCache[0];
                this.childCache.RemoveAt(0);
            }
            else
            {
                obj2 = Object.Instantiate<GameObject>(this.copyItemChild);
                this.objToCompnent.Add(obj2, obj2.GetComponents<Component>());
                obj2.SetActive(false);
            }
            if (!(obj2.transform is RectTransform))
            {
                throw new Exception($"copyItem({this.copyItemChild.name}) is not a RectTransform type");
            }
            obj2.transform.SetParent(this.rectTransform, false);
            this.rectChildren.Add(obj2.transform as RectTransform);
        }

        private void DestroyChild(RectTransform child)
        {
            this.rectChildren.Remove(child);
            child.gameObject.SetActive(false);
            this.childCache.Add(child.gameObject);
        }

        protected float GetTotalFlexibleSize(int axis) => 
            this.m_TotalFlexibleSize[axis];

        protected float GetTotalMinSize(int axis) => 
            this.m_TotalMinSize[axis];

        protected float GetTotalPreferredSize(int axis) => 
            this.m_TotalPreferredSize[axis];

        public void Init(int displayCount, int itemCount, GameObject copyItemChild = null)
        {
            while (this.rectChildren.Count > 0)
            {
                Object.Destroy(this.rectChildren[0].gameObject);
                this.rectChildren.RemoveAt(0);
            }
            this.rectChildren.Clear();
            if (copyItemChild != null)
            {
                this.copyItemChild = copyItemChild;
            }
            this.SetDisplayCount(displayCount);
            this.SetItemCount(itemCount, true);
            this.UpdateLayout();
        }

        private Vector2Int PositionToGrid(Vector2 pos)
        {
            int x = (int) ((pos.x - this.padding.left) / (this.cellSize.x + this.spacing.x));
            return new Vector2Int(x, (int) ((-pos.y - this.padding.top) / (this.cellSize.y + this.spacing.y)));
        }

        private int PositionToRealIndex(Vector2 pos)
        {
            Vector2Int num = this.PositionToGrid(pos);
            if (this.sortAxis == Axis.Horizontal)
            {
                return ((num.y * this.constraintCount) + num.x);
            }
            if (this.sortAxis == Axis.Vertical)
            {
                return ((num.x * this.constraintCount) + num.y);
            }
            return 0;
        }

        public void RefreshAll()
        {
            this.UpdateLayoutChildren(false, true);
        }

        public void RegUpdateCallback(Action<int, GameObject> callBack)
        {
            this.updateChildCallBack = (Action<int, GameObject>) Delegate.Combine(this.updateChildCallBack, callBack);
        }

        public void RegUpdateCallback<T>(Action<int, T> callBack) where T: Component
        {
            Type key = typeof(T);
            if (!this.updateChildComponentCallBack.ContainsKey(key))
            {
                this.updateChildComponentCallBack.Add(key, new ChildComponentAction<T>(callBack));
            }
            else
            {
                ChildComponentAction<T> action1 = this.updateChildComponentCallBack[key] as ChildComponentAction<T>;
                action1.callBack = (Action<int, T>) Delegate.Combine(action1.callBack, callBack);
            }
        }

        protected void Scroll(Vector2 value)
        {
            int a = (int) this.ContentPositionToRowIndex(this.scrollRect.get_content().anchoredPosition);
            a = Mathf.Min(Mathf.Max(a, 0), this.itemMaxRow - this.displayMaxRow);
            if (a != this.lastRowIndex)
            {
                this.ScrollChild(a - this.lastRowIndex);
                this.lastRowIndex = a;
                this.UpdateLayoutChildren(true, false);
            }
        }

        private void ScrollChild(int indexCount)
        {
            if (indexCount > 0)
            {
                while (indexCount-- > 0)
                {
                    for (int i = 0; i < this.constraintCount; i++)
                    {
                        RectTransform item = this.rectChildren[0];
                        this.rectChildren.RemoveAt(0);
                        this.rectChildren.Add(item);
                        int index = this.PositionToRealIndex(item.anchoredPosition) + this.displayItemCount;
                        if ((index >= 0) && (index < this.itemCount))
                        {
                            this.UpdateChildListCallback(index, item.gameObject);
                        }
                    }
                }
            }
            else if (indexCount < 0)
            {
                while (indexCount++ < 0)
                {
                    for (int i = 0; i < this.constraintCount; i++)
                    {
                        RectTransform item = this.rectChildren[this.rectChildren.Count - 1];
                        this.rectChildren.RemoveAt(this.rectChildren.Count - 1);
                        this.rectChildren.Insert(0, item);
                        int index = this.PositionToRealIndex(item.anchoredPosition) - this.displayItemCount;
                        if ((index >= 0) && (index < this.itemCount))
                        {
                            this.UpdateChildListCallback(index, item.gameObject);
                        }
                    }
                }
            }
        }

        public void ScrollToItem(int itemIndex)
        {
            if ((itemIndex >= 0) && (itemIndex < this.itemCount))
            {
                if (this.sortAxis == Axis.Horizontal)
                {
                    int num = itemIndex / this.constraintCount;
                    float y = this.padding.top + ((this.cellSize.y + this.spacing.y) * num);
                    float height = this.scrollRect.get_viewport().rect.height;
                    Vector2 anchoredPosition = this.scrollRect.get_content().anchoredPosition;
                    if ((y > anchoredPosition.y) || (y < (anchoredPosition.y + height)))
                    {
                        this.scrollRect.get_content().anchoredPosition = new Vector2(anchoredPosition.x, y);
                        base.StartCoroutine(this.ScrollToItemImpl());
                    }
                }
                else if (this.sortAxis == Axis.Vertical)
                {
                    int num4 = itemIndex / this.constraintCount;
                    float x = -this.padding.left - ((this.cellSize.x + this.spacing.x) * num4);
                    float width = this.scrollRect.get_viewport().rect.width;
                    Vector2 anchoredPosition = this.scrollRect.get_content().anchoredPosition;
                    if ((x < anchoredPosition.x) || (x > (anchoredPosition.x - width)))
                    {
                        this.scrollRect.get_content().anchoredPosition = new Vector2(x, anchoredPosition.y);
                        base.StartCoroutine(this.ScrollToItemImpl());
                    }
                }
            }
        }

        [DebuggerHidden]
        private IEnumerator ScrollToItemImpl() => 
            new <ScrollToItemImpl>c__Iterator0 { $this = this };

        private void SetDisplayCount(int newCount)
        {
            if (this.displayItemCount < newCount)
            {
                while (this.displayItemCount < newCount)
                {
                    this.CreateNewDisplayChild();
                }
            }
            else if (this.displayItemCount > newCount)
            {
                while (this.displayItemCount > newCount)
                {
                    this.DestroyChild(this.rectChildren[0]);
                }
            }
        }

        public void SetItemCount(int newCount, bool callUpdate = true)
        {
            this.itemCount = newCount;
            this.UpdateLayoutChildren(callUpdate, false);
        }

        protected void SetLayoutInputForAxis(float totalMin, float totalPreferred, float totalFlexible, int axis)
        {
            this.m_TotalMinSize[axis] = totalMin;
            this.m_TotalPreferredSize[axis] = totalPreferred;
            this.m_TotalFlexibleSize[axis] = totalFlexible;
        }

        public void UnRegUpdateCallback(Action<int, GameObject> callBack)
        {
            this.updateChildCallBack = (Action<int, GameObject>) Delegate.Remove(this.updateChildCallBack, callBack);
        }

        public void UnRegUpdateCallback<T>(Action<int, T> callBack) where T: Component
        {
            Type key = typeof(T);
            if (this.updateChildComponentCallBack.ContainsKey(key))
            {
                ChildComponentAction<T> action1 = this.updateChildComponentCallBack[key] as ChildComponentAction<T>;
                action1.callBack = (Action<int, T>) Delegate.Remove(action1.callBack, callBack);
            }
        }

        private void UpdateChildListCallback(int index, GameObject obj)
        {
            if (this.updateChildCallBack != null)
            {
                this.updateChildCallBack(index, obj);
            }
            if (this.objToCompnent[obj] != null)
            {
                for (int i = 0; i < this.objToCompnent[obj].Length; i++)
                {
                    Component component = this.objToCompnent[obj][i];
                    Type key = component.GetType();
                    if (this.updateChildComponentCallBack.ContainsKey(key))
                    {
                        this.updateChildComponentCallBack[key].Invoke(index, component);
                    }
                }
            }
        }

        private void UpdateLayout()
        {
            this.m_Tracker.Clear();
            for (int i = 0; i < this.rectChildren.Count; i++)
            {
                this.m_Tracker.Add(this, this.rectChildren[i], DrivenTransformProperties.Pivot | DrivenTransformProperties.SizeDelta | DrivenTransformProperties.Anchors | DrivenTransformProperties.AnchoredPosition);
                this.rectChildren[i].anchorMin = Vector2.up;
                this.rectChildren[i].anchorMax = Vector2.up;
                this.rectChildren[i].sizeDelta = this.cellSize;
                this.rectChildren[i].pivot = new Vector2(0f, 1f);
            }
            this.m_Tracker.Add(this, this.rectTransform, DrivenTransformProperties.SizeDelta);
        }

        private void UpdateLayoutChildren(bool callUpdate = true, bool callUpdateAlways = false)
        {
            Vector2 vector2;
            if (this.sortAxis == Axis.Horizontal)
            {
                int lastRowIndex = this.lastRowIndex;
                vector2 = new Vector2 {
                    x = this.padding.left,
                    y = -this.padding.top - ((this.cellSize.y + this.spacing.y) * lastRowIndex)
                };
                Vector2 vector = vector2;
                for (int i = 0; i < this.rectChildren.Count; i++)
                {
                    int num3 = i % this.constraintCount;
                    int num4 = i / this.constraintCount;
                    float x = vector.x + (num3 * (this.cellSize.x + this.spacing.x));
                    float y = vector.y - (num4 * (this.cellSize.y + this.spacing.y));
                    this.rectChildren[i].anchoredPosition = new Vector2(x, y);
                    this.rectChildren[i].SetSiblingIndex(i);
                    int index = (lastRowIndex * this.constraintCount) + i;
                    object[] args = new object[] { index };
                    this.rectChildren[i].gameObject.name = Utils.FormatString("Item_({0})", args);
                    if ((index < this.itemCount) && (index >= 0))
                    {
                        if (!this.rectChildren[i].gameObject.activeSelf)
                        {
                            this.rectChildren[i].gameObject.SetActive(true);
                            this.UpdateChildListCallback(index, this.rectChildren[i].gameObject);
                        }
                        if (callUpdateAlways)
                        {
                            this.UpdateChildListCallback(index, this.rectChildren[i].gameObject);
                        }
                    }
                    else
                    {
                        this.rectChildren[i].gameObject.SetActive(false);
                    }
                }
            }
            else if (this.sortAxis == Axis.Vertical)
            {
                int lastRowIndex = this.lastRowIndex;
                vector2 = new Vector2 {
                    x = this.padding.left + ((this.cellSize.x + this.spacing.x) * lastRowIndex),
                    y = -this.padding.top
                };
                Vector2 vector3 = vector2;
                for (int i = 0; i < this.rectChildren.Count; i++)
                {
                    int num10 = i / this.constraintCount;
                    int num11 = i % this.constraintCount;
                    float x = vector3.x + (num10 * (this.cellSize.x + this.spacing.x));
                    float y = vector3.y - (num11 * (this.cellSize.y + this.spacing.y));
                    this.rectChildren[i].anchoredPosition = new Vector2(x, y);
                    this.rectChildren[i].SetSiblingIndex(i);
                    int index = (lastRowIndex * this.constraintCount) + i;
                    object[] args = new object[] { index };
                    this.rectChildren[i].gameObject.name = Utils.FormatString("Item_({0})", args);
                    if ((index < this.itemCount) && (index >= 0))
                    {
                        if (!this.rectChildren[i].gameObject.activeSelf)
                        {
                            this.rectChildren[i].gameObject.SetActive(true);
                            this.UpdateChildListCallback(index, this.rectChildren[i].gameObject);
                        }
                        if (callUpdateAlways)
                        {
                            this.UpdateChildListCallback(index, this.rectChildren[i].gameObject);
                        }
                    }
                    else
                    {
                        this.rectChildren[i].gameObject.SetActive(false);
                    }
                }
            }
            this.UpdateLayoutContent();
        }

        private void UpdateLayoutContent()
        {
            if (this.sortAxis == Axis.Horizontal)
            {
                float totalMin = (this.padding.horizontal + (this.constraintCount * (this.cellSize.x + this.spacing.x))) - this.spacing.x;
                float minScrollLength = (this.padding.vertical + (this.itemMaxRow * (this.cellSize.y + this.spacing.y))) - this.spacing.y;
                if ((this.MinScrollLength > 0f) && (minScrollLength < this.MinScrollLength))
                {
                    minScrollLength = this.MinScrollLength;
                }
                this.SetLayoutInputForAxis(totalMin, totalMin, -1f, 0);
                this.SetLayoutInputForAxis(minScrollLength, minScrollLength, -1f, 1);
            }
            else if (this.sortAxis == Axis.Vertical)
            {
                float totalMin = (this.padding.horizontal + (this.itemMaxRow * (this.cellSize.x + this.spacing.x))) - this.spacing.x;
                float num4 = (this.padding.vertical + (this.constraintCount * (this.cellSize.y + this.spacing.y))) - this.spacing.y;
                if ((this.MinScrollLength > 0f) && (totalMin < this.MinScrollLength))
                {
                    totalMin = this.MinScrollLength;
                }
                this.SetLayoutInputForAxis(totalMin, totalMin, -1f, 0);
                this.SetLayoutInputForAxis(num4, num4, -1f, 1);
            }
            this.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, LayoutUtility.GetPreferredSize(this.rectTransform, 0));
            this.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, LayoutUtility.GetPreferredSize(this.rectTransform, 1));
            if (this.onSizeChange != null)
            {
                this.onSizeChange(new Vector2(this.preferredWidth, this.preferredHeight));
            }
        }

        public int displayItemCount =>
            this.rectChildren.Count;

        public int displayMaxRow =>
            ((((this.displayItemCount / this.constraintCount) + (this.displayItemCount % this.constraintCount)) <= 0) ? 0 : 1);

        public int itemMaxRow =>
            ((this.itemCount / this.constraintCount) + (((this.itemCount % this.constraintCount) <= 0) ? 0 : 1));

        public RectTransform rectTransform
        {
            get
            {
                if (this.m_Rect == null)
                {
                    this.m_Rect = base.GetComponent<RectTransform>();
                }
                return this.m_Rect;
            }
        }

        public virtual float minWidth =>
            this.GetTotalMinSize(0);

        public virtual float preferredWidth =>
            this.GetTotalPreferredSize(0);

        public virtual float flexibleWidth =>
            this.GetTotalFlexibleSize(0);

        public virtual float minHeight =>
            this.GetTotalMinSize(1);

        public virtual float preferredHeight =>
            this.GetTotalPreferredSize(1);

        public virtual float flexibleHeight =>
            this.GetTotalFlexibleSize(1);

        public virtual int layoutPriority =>
            0;

        [CompilerGenerated]
        private sealed class <ScrollToItemImpl>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal InfinityScrollGroup $this;
            internal object $current;
            internal bool $disposing;
            internal int $PC;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$disposing = true;
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        this.$current = new WaitForEndOfFrame();
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        return true;

                    case 1:
                        this.$this.UpdateLayoutChildren(false, true);
                        this.$PC = -1;
                        break;
                }
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current =>
                this.$current;

            object IEnumerator.Current =>
                this.$current;
        }

        public enum Axis
        {
            Horizontal,
            Vertical
        }

        protected class ChildComponentAction<T> : InfinityScrollGroup.ComponentAction where T: Component
        {
            public Action<int, T> callBack;

            public ChildComponentAction(Action<int, T> callBack)
            {
                this.callBack = callBack;
            }

            public void Invoke(int index, Component component)
            {
                if (this.callBack != null)
                {
                    this.callBack(index, component as T);
                }
            }
        }

        protected interface ComponentAction
        {
            void Invoke(int index, Component component);
        }
    }
}

