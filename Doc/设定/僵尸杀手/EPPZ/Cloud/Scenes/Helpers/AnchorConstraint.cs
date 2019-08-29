namespace EPPZ.Cloud.Scenes.Helpers
{
    using System;
    using UnityEngine;

    [ExecuteInEditMode]
    public class AnchorConstraint : MonoBehaviour
    {
        private RectTransform _parentRectTransform;
        private RectTransform _rectTransform;

        private void OnEnable()
        {
            this._rectTransform = base.GetComponent<RectTransform>();
            this._parentRectTransform = base.transform.parent.GetComponent<RectTransform>();
        }

        private void Update()
        {
            if (base.enabled)
            {
                float num = this._parentRectTransform.rect.width / this._parentRectTransform.rect.height;
                this._rectTransform.anchorMin = new Vector2(this._rectTransform.anchorMin.x, this._rectTransform.anchorMin.x * num);
                this._rectTransform.anchorMax = new Vector2(this._rectTransform.anchorMax.x, 1f - ((1f - this._rectTransform.anchorMax.x) * num));
                Vector2 zero = Vector2.zero;
                this._rectTransform.offsetMax = zero;
                this._rectTransform.offsetMin = zero;
            }
        }
    }
}

