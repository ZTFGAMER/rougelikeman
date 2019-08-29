namespace Dxx.UI
{
    using Dxx;
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    [AddComponentMenu("UI/Effects/TextColor3Dxx"), RequireComponent(typeof(Text))]
    public class TextColor3Dxx : Shadow
    {
        private const float DOWNWIDTH = 6f;
        private Text m_Text;
        public Color topColor = new Color(0.9490196f, 0.9882353f, 0.8901961f, 1f);
        public Color bottomColor = new Color(0.6392157f, 0.8823529f, 0.4039216f, 1f);
        [SerializeField]
        private List<Vector2> shadowPosList;
        [SerializeField]
        public List<Color32> colorList;
        public Color32 middleoutline;
        [SerializeField]
        private Vector2 middleoutlineoffset;

        public TextColor3Dxx()
        {
            List<Vector2> list = new List<Vector2> {
                new Vector2(0f, 0f),
                new Vector2(0f, -6f),
                new Vector2(0f, -6f)
            };
            this.shadowPosList = list;
            List<Color32> list2 = new List<Color32> {
                new Color(1f, 1f, 1f, 1f),
                new Color(0.3372549f, 0.4509804f, 0.2313726f, 1f),
                new Color(0.4352941f, 0.4352941f, 0.4352941f, 1f)
            };
            this.colorList = list2;
            this.middleoutline = Color.black;
            this.middleoutlineoffset = new Vector2(0.5f, -0.5f);
        }

        private void ApplyShadowZeroAllocSelf(List<UIVertex> verts, Color32 color, int start, int end, float x, float y)
        {
            for (int i = start; i < end; i++)
            {
                UIVertex vertex = verts[i];
                Vector3 position = vertex.position;
                position.x += x;
                position.y += y;
                vertex.position = position;
                Color32 color2 = color;
                UIVertex vertex2 = verts[i];
                color2.a = (byte) ((color2.a * vertex2.color.a) / 0xff);
                vertex.color = color2;
                verts[i] = vertex;
            }
            base.ApplyShadowZeroAlloc(verts, color, start, end, x, y);
        }

        private byte GetAlpha(Color c, List<UIVertex> verts)
        {
            UIVertex vertex = verts[0];
            return (byte) ((c.a * vertex.color.a) / 255f);
        }

        public override void ModifyMesh(VertexHelper vh)
        {
            if (this.IsActive())
            {
                List<UIVertex> toRelease = ListPool<UIVertex>.Get();
                int currentVertCount = vh.currentVertCount;
                if (currentVertCount != 0)
                {
                    for (int i = 0; i < currentVertCount; i++)
                    {
                        UIVertex item = new UIVertex();
                        vh.PopulateUIVertex(ref item, i);
                        toRelease.Add(item);
                    }
                    UIVertex vertex2 = toRelease[0];
                    float y = vertex2.position.y;
                    UIVertex vertex3 = toRelease[0];
                    float num4 = vertex3.position.y;
                    for (int j = 0; j < currentVertCount; j++)
                    {
                        UIVertex vertex4 = toRelease[j];
                        float num6 = vertex4.position.y;
                        if (num6 > y)
                        {
                            y = num6;
                        }
                        else if (num6 < num4)
                        {
                            num4 = num6;
                        }
                    }
                    float num7 = y - num4;
                    for (int k = 0; k < currentVertCount; k++)
                    {
                        UIVertex vertex5 = toRelease[k];
                        Color32 color = Color32.Lerp(this.bottomColor, this.topColor, (vertex5.position.y - num4) / num7);
                        vertex5.color = color;
                        vh.SetUIVertex(vertex5, k);
                    }
                    vh.GetUIVertexStream(toRelease);
                    int num9 = (toRelease.Count * this.shadowPosList.Count) * 5;
                    if (toRelease.Capacity < num9)
                    {
                        toRelease.Capacity = num9;
                    }
                    for (int m = 0; m < this.shadowPosList.Count; m++)
                    {
                        Vector2 vector = (this.shadowPosList[m] * this.text.fontSize) * 0.02f;
                        switch (m)
                        {
                            case 1:
                            {
                                Color32 color2 = this.colorList[m];
                                base.ApplyShadowZeroAlloc(toRelease, color2, 0, toRelease.Count, vector.x, vector.y);
                                base.ApplyShadow(toRelease, this.middleoutline, 0, toRelease.Count, this.middleoutlineoffset.x, this.middleoutlineoffset.y);
                                break;
                            }
                            case 2:
                            {
                                Color32 color3 = this.colorList[m];
                                base.ApplyShadowZeroAlloc(toRelease, color3, 0, toRelease.Count, vector.x, vector.y);
                                break;
                            }
                            case 0:
                            {
                                Color color4 = base.get_effectColor();
                                base.ApplyShadowZeroAlloc(toRelease, color4, 0, toRelease.Count, vector.x + base.get_effectDistance().x, vector.y + base.get_effectDistance().y);
                                base.ApplyShadowZeroAlloc(toRelease, color4, 0, toRelease.Count, vector.x + base.get_effectDistance().x, vector.y - base.get_effectDistance().y);
                                base.ApplyShadowZeroAlloc(toRelease, color4, 0, toRelease.Count, vector.x - base.get_effectDistance().x, vector.y + base.get_effectDistance().y);
                                base.ApplyShadowZeroAlloc(toRelease, color4, 0, toRelease.Count, vector.x - base.get_effectDistance().x, vector.y - base.get_effectDistance().y);
                                break;
                            }
                        }
                    }
                    vh.Clear();
                    vh.AddUIVertexTriangleStream(toRelease);
                    ListPool<UIVertex>.Release(toRelease);
                }
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

