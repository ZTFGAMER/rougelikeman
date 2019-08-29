namespace Dxx.UI
{
    using Dxx;
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    [AddComponentMenu("UI/Effects/Gradient Angle Dxx")]
    public class GradientAngle : BaseMeshEffect
    {
        [SerializeField]
        private Color32 startColor = Color.white;
        [SerializeField]
        private Color32 endColor = Color.black;
        [SerializeField, Range(0f, 360f), Tooltip("渐变旋转角度")]
        private float angle;
        [SerializeField, Tooltip("渐变偏移")]
        private float offset;

        protected Vector2 GetProjectivePoint(Vector2 pLine, float k, Vector2 pOut)
        {
            Vector2 zero = Vector2.zero;
            if ((k == 0f) || (k == float.NaN))
            {
                zero.x = pOut.x;
                zero.y = pLine.y;
                return zero;
            }
            zero.x = ((((k * pLine.x) + (pOut.x / k)) + pOut.y) - pLine.y) / ((1f / k) + k);
            zero.y = ((-1f / k) * (zero.x - pOut.x)) + pOut.y;
            return zero;
        }

        public override void ModifyMesh(VertexHelper vh)
        {
            if (this.IsActive())
            {
                List<UIVertex> toRelease = ListPool<UIVertex>.Get();
                vh.GetUIVertexStream(toRelease);
                int count = toRelease.Count;
                if (count > 0)
                {
                    UIVertex vertex = toRelease[0];
                    float y = vertex.position.y;
                    UIVertex vertex2 = toRelease[0];
                    float num3 = vertex2.position.y;
                    UIVertex vertex3 = toRelease[0];
                    float x = vertex3.position.x;
                    UIVertex vertex4 = toRelease[0];
                    float num5 = vertex4.position.x;
                    for (int i = 1; i < count; i++)
                    {
                        UIVertex vertex5 = toRelease[i];
                        float num7 = vertex5.position.x;
                        UIVertex vertex6 = toRelease[i];
                        float num8 = vertex6.position.y;
                        if (num8 > num3)
                        {
                            num3 = num8;
                        }
                        if (num8 < y)
                        {
                            y = num8;
                        }
                        if (num7 < x)
                        {
                            x = num7;
                        }
                        if (num7 > num5)
                        {
                            num5 = num7;
                        }
                    }
                    float num9 = num3 - y;
                    float num10 = num5 - x;
                    int num11 = (int) (this.angle / 90f);
                    float num12 = this.angle - (num11 * 90f);
                    if ((num11 % 2) == 1)
                    {
                        num12 = 90f - num12;
                    }
                    float k = Mathf.Tan((num12 / 180f) * 3.141593f);
                    Vector2 pOut = new Vector2(num10 * 0.5f, num9 * 0.5f);
                    Vector2 vector2 = new Vector2(-num10 * 0.5f, -num9 * 0.5f);
                    if ((num11 % 2) == 1)
                    {
                        pOut = new Vector2(-num10 * 0.5f, num9 * 0.5f);
                        vector2 = new Vector2(num10 * 0.5f, -num9 * 0.5f);
                    }
                    Vector2 a = this.GetProjectivePoint(Vector2.zero, k, pOut);
                    Vector2 b = this.GetProjectivePoint(Vector2.zero, k, vector2);
                    float num14 = Vector2.Distance(a, b);
                    for (int j = 0; j < count; j++)
                    {
                        UIVertex vertex7 = toRelease[j];
                        float num16 = vertex7.position.x - (x + (num10 * 0.5f));
                        UIVertex vertex8 = toRelease[j];
                        float num17 = vertex8.position.y - (y + (num9 * 0.5f));
                        float num18 = Vector2.Distance(this.GetProjectivePoint(Vector2.zero, k, new Vector2(num16, num17)), a);
                        Color white = Color.white;
                        float t = num18 / num14;
                        t += Mathf.Abs(this.offset);
                        t -= (int) t;
                        if ((num11 % 4) < 2)
                        {
                            white = (Color) Color32.Lerp(this.startColor, this.endColor, t);
                        }
                        else
                        {
                            white = (Color) Color32.Lerp(this.endColor, this.startColor, t);
                        }
                        UIVertex vertex9 = new UIVertex {
                            color = white
                        };
                        UIVertex vertex10 = toRelease[j];
                        vertex9.normal = vertex10.normal;
                        UIVertex vertex11 = toRelease[j];
                        vertex9.position = vertex11.position;
                        UIVertex vertex12 = toRelease[j];
                        vertex9.tangent = vertex12.tangent;
                        UIVertex vertex13 = toRelease[j];
                        vertex9.uv0 = vertex13.uv0;
                        UIVertex vertex14 = toRelease[j];
                        vertex9.uv1 = vertex14.uv1;
                        toRelease[j] = vertex9;
                    }
                    vh.Clear();
                    vh.AddUIVertexTriangleStream(toRelease);
                }
                ListPool<UIVertex>.Release(toRelease);
            }
        }
    }
}

