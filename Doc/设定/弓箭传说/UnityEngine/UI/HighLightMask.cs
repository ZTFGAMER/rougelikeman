namespace UnityEngine.UI
{
    using System;
    using UnityEngine;

    public class HighLightMask : MaskableGraphic, ICanvasRaycastFilter
    {
        [SerializeField]
        private RectTransform _target;
        private Vector3 _targetMin = Vector3.zero;
        private Vector3 _targetMax = Vector3.zero;
        private bool _canRefresh = true;
        private Transform _cacheTrans;

        private void _RefreshView()
        {
            if (this._canRefresh)
            {
                this._canRefresh = false;
                if (null == this._target)
                {
                    this._SetTarget(Vector3.zero, Vector3.zero);
                    this.SetAllDirty();
                }
                else
                {
                    Bounds bounds = RectTransformUtility.CalculateRelativeRectTransformBounds(this._cacheTrans, this._target);
                    this._SetTarget(bounds.min, bounds.max);
                }
            }
        }

        private void _SetTarget(Vector3 tarMin, Vector3 tarMax)
        {
            if ((tarMin != this._targetMin) || (tarMax != this._targetMax))
            {
                this._targetMin = tarMin;
                this._targetMax = tarMax;
                this.SetAllDirty();
            }
        }

        protected override void Awake()
        {
            base.Awake();
            this._cacheTrans = base.GetComponent<RectTransform>();
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            if ((this._targetMin == Vector3.zero) && (this._targetMax == Vector3.zero))
            {
                base.OnPopulateMesh(vh);
            }
            else
            {
                vh.Clear();
                UIVertex simpleVert = UIVertex.simpleVert;
                simpleVert.color = this.get_color();
                Vector2 pivot = base.get_rectTransform().pivot;
                Rect rect = base.get_rectTransform().rect;
                float x = -pivot.x * rect.width;
                float y = -pivot.y * rect.height;
                float num3 = (1f - pivot.x) * rect.width;
                float num4 = (1f - pivot.y) * rect.height;
                simpleVert.position = new Vector3(x, num4);
                vh.AddVert(simpleVert);
                simpleVert.position = new Vector3(num3, num4);
                vh.AddVert(simpleVert);
                simpleVert.position = new Vector3(num3, y);
                vh.AddVert(simpleVert);
                simpleVert.position = new Vector3(x, y);
                vh.AddVert(simpleVert);
                simpleVert.position = new Vector3(this._targetMin.x, this._targetMax.y);
                vh.AddVert(simpleVert);
                simpleVert.position = new Vector3(this._targetMax.x, this._targetMax.y);
                vh.AddVert(simpleVert);
                simpleVert.position = new Vector3(this._targetMax.x, this._targetMin.y);
                vh.AddVert(simpleVert);
                simpleVert.position = new Vector3(this._targetMin.x, this._targetMin.y);
                vh.AddVert(simpleVert);
                vh.AddTriangle(4, 0, 1);
                vh.AddTriangle(4, 1, 5);
                vh.AddTriangle(5, 1, 2);
                vh.AddTriangle(5, 2, 6);
                vh.AddTriangle(6, 2, 3);
                vh.AddTriangle(6, 3, 7);
                vh.AddTriangle(7, 3, 0);
                vh.AddTriangle(7, 0, 4);
            }
        }

        public void SetTarget(RectTransform target)
        {
            this._canRefresh = true;
            this._target = target;
            this._RefreshView();
        }

        bool ICanvasRaycastFilter.IsRaycastLocationValid(Vector2 screenPos, Camera eventCamera) => 
            ((null == this._target) || !RectTransformUtility.RectangleContainsScreenPoint(this._target, screenPos, eventCamera));
    }
}

