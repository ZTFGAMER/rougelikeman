namespace Dxx.UI
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    [AddComponentMenu("UI/Effects/UI Flip"), RequireComponent(typeof(Graphic)), DisallowMultipleComponent]
    public class UIFlip : BaseMeshEffect
    {
        [SerializeField]
        private bool m_Horizontal;
        [SerializeField]
        private bool m_Veritical;

        public override void ModifyMesh(VertexHelper vh)
        {
            RectTransform transform = base.graphic.get_rectTransform();
            UIVertex vertex = new UIVertex();
            Vector2 center = transform.rect.center;
            for (int i = 0; i < vh.currentVertCount; i++)
            {
                vh.PopulateUIVertex(ref vertex, i);
                Vector3 position = vertex.position;
                vertex.position = new Vector3(!this.m_Horizontal ? position.x : -position.x, !this.m_Veritical ? position.y : -position.y);
                vh.SetUIVertex(vertex, i);
            }
        }

        public bool horizontal
        {
            get => 
                this.m_Horizontal;
            set => 
                (this.m_Horizontal = value);
        }

        public bool vertical
        {
            get => 
                this.m_Veritical;
            set => 
                (this.m_Veritical = value);
        }
    }
}

