namespace Dxx.UI
{
    using Dxx;
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    [AddComponentMenu("UI/Effects/OutLineDxx"), RequireComponent(typeof(Text))]
    public class OutLineDxx : Shadow
    {
        private const float DOWNWIDTH = 6f;
        private Text m_Text;
        private List<Vector2> shadowPosList;

        public OutLineDxx()
        {
            List<Vector2> list = new List<Vector2> {
                new Vector2(0f, 0f),
                new Vector2(0f, -6f)
            };
            this.shadowPosList = list;
        }

        public override void ModifyMesh(VertexHelper vh)
        {
            if (this.IsActive())
            {
                List<UIVertex> toRelease = ListPool<UIVertex>.Get();
                vh.GetUIVertexStream(toRelease);
                int num = (toRelease.Count * this.shadowPosList.Count) * 5;
                if (toRelease.Capacity < num)
                {
                    toRelease.Capacity = num;
                }
                int num2 = 0;
                int count = toRelease.Count;
                for (int i = 0; i < this.shadowPosList.Count; i++)
                {
                    Vector2 vector = (this.shadowPosList[i] * this.text.fontSize) * 0.02f;
                    if (this.shadowPosList[i] != Vector2.zero)
                    {
                        base.ApplyShadowZeroAlloc(toRelease, base.get_effectColor(), num2, toRelease.Count, vector.x, vector.y);
                        num2 = count;
                        count = toRelease.Count;
                    }
                    base.ApplyShadowZeroAlloc(toRelease, base.get_effectColor(), num2, toRelease.Count, vector.x + base.get_effectDistance().x, vector.y + base.get_effectDistance().y);
                    num2 = count;
                    count = toRelease.Count;
                    base.ApplyShadowZeroAlloc(toRelease, base.get_effectColor(), num2, toRelease.Count, vector.x + base.get_effectDistance().x, vector.y - base.get_effectDistance().y);
                    num2 = count;
                    count = toRelease.Count;
                    base.ApplyShadowZeroAlloc(toRelease, base.get_effectColor(), num2, toRelease.Count, vector.x - base.get_effectDistance().x, vector.y + base.get_effectDistance().y);
                    num2 = count;
                    count = toRelease.Count;
                    base.ApplyShadowZeroAlloc(toRelease, base.get_effectColor(), num2, toRelease.Count, vector.x - base.get_effectDistance().x, vector.y - base.get_effectDistance().y);
                    num2 = count;
                    count = toRelease.Count;
                }
                vh.Clear();
                vh.AddUIVertexTriangleStream(toRelease);
                ListPool<UIVertex>.Release(toRelease);
            }
        }

        public Text text
        {
            get
            {
                if (this.m_Text == null)
                {
                    this.m_Text = base.GetComponent<Text>();
                }
                return this.m_Text;
            }
        }
    }
}

