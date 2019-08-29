namespace Dxx.UI
{
    using Dxx;
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    [AddComponentMenu("UI/Effects/TextColorDxx"), RequireComponent(typeof(Text))]
    public class TextColorDxx : BaseMeshEffect
    {
        public Color32 topColor;
        public Color32 bottomColor;

        public override void ModifyMesh(VertexHelper vh)
        {
            if (this.IsActive())
            {
                int currentVertCount = vh.currentVertCount;
                if (currentVertCount != 0)
                {
                    List<UIVertex> list = ListPool<UIVertex>.Get();
                    for (int i = 0; i < currentVertCount; i++)
                    {
                        UIVertex item = new UIVertex();
                        vh.PopulateUIVertex(ref item, i);
                        list.Add(item);
                    }
                    UIVertex vertex2 = list[0];
                    float y = vertex2.position.y;
                    UIVertex vertex3 = list[0];
                    float num4 = vertex3.position.y;
                    for (int j = 0; j < currentVertCount; j++)
                    {
                        UIVertex vertex4 = list[j];
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
                        UIVertex vertex5 = list[k];
                        Color32 color = Color32.Lerp(this.bottomColor, this.topColor, (vertex5.position.y - num4) / num7);
                        vertex5.color = color;
                        vh.SetUIVertex(vertex5, k);
                    }
                }
            }
        }
    }
}

