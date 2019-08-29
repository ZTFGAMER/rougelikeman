namespace Dxx.UI
{
    using System;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class InfinityScrollContent : UIBehaviour
    {
        public InfinityScrollGroup mGroup;
        protected DrivenRectTransformTracker m_Tracker;
        private RectTransform m_rectTransform;

        protected override void Awake()
        {
            if (this.mGroup == null)
            {
                this.mGroup = base.GetComponentInChildren<InfinityScrollGroup>();
            }
            this.mGroup.onSizeChange = (Action<Vector2>) Delegate.Combine(this.mGroup.onSizeChange, new Action<Vector2>(this.FitContent));
            this.m_Tracker.Clear();
            this.m_Tracker.Add(this, this.rectTransform, DrivenTransformProperties.SizeDelta);
            base.Awake();
        }

        public void FitContent(Vector2 size)
        {
            if (this.mGroup.sortAxis == InfinityScrollGroup.Axis.Horizontal)
            {
                this.rectTransform.sizeDelta = new Vector2(this.rectTransform.sizeDelta.x, size.y);
            }
            else
            {
                this.rectTransform.sizeDelta = new Vector2(size.x, this.rectTransform.sizeDelta.y);
            }
        }

        public RectTransform rectTransform
        {
            get
            {
                if (this.m_rectTransform == null)
                {
                    this.m_rectTransform = base.transform as RectTransform;
                }
                return this.m_rectTransform;
            }
        }
    }
}

