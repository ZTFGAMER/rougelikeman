namespace Spine
{
    using System;

    internal class Triangulator
    {
        private readonly ExposedList<ExposedList<float>> convexPolygons = new ExposedList<ExposedList<float>>();
        private readonly ExposedList<ExposedList<int>> convexPolygonsIndices = new ExposedList<ExposedList<int>>();
        private readonly ExposedList<int> indicesArray = new ExposedList<int>();
        private readonly ExposedList<bool> isConcaveArray = new ExposedList<bool>();
        private readonly ExposedList<int> triangles = new ExposedList<int>();
        private readonly Pool<ExposedList<float>> polygonPool = new Pool<ExposedList<float>>(0x10, 0x7fffffff);
        private readonly Pool<ExposedList<int>> polygonIndicesPool = new Pool<ExposedList<int>>(0x10, 0x7fffffff);

        public ExposedList<ExposedList<float>> Decompose(ExposedList<float> verticesArray, ExposedList<int> triangles)
        {
            float[] items = verticesArray.Items;
            ExposedList<ExposedList<float>> convexPolygons = this.convexPolygons;
            int index = 0;
            int count = convexPolygons.Count;
            while (index < count)
            {
                this.polygonPool.Free(convexPolygons.Items[index]);
                index++;
            }
            convexPolygons.Clear(true);
            ExposedList<ExposedList<int>> convexPolygonsIndices = this.convexPolygonsIndices;
            int num3 = 0;
            int num4 = convexPolygonsIndices.Count;
            while (num3 < num4)
            {
                this.polygonIndicesPool.Free(convexPolygonsIndices.Items[num3]);
                num3++;
            }
            convexPolygonsIndices.Clear(true);
            ExposedList<int> item = this.polygonIndicesPool.Obtain();
            item.Clear(true);
            ExposedList<float> list4 = this.polygonPool.Obtain();
            list4.Clear(true);
            int num5 = -1;
            int num6 = 0;
            int[] numArray2 = triangles.Items;
            int num7 = 0;
            int num8 = triangles.Count;
            while (num7 < num8)
            {
                int num9 = numArray2[num7] << 1;
                int num10 = numArray2[num7 + 1] << 1;
                int num11 = numArray2[num7 + 2] << 1;
                float num12 = items[num9];
                float num13 = items[num9 + 1];
                float num14 = items[num10];
                float num15 = items[num10 + 1];
                float num16 = items[num11];
                float num17 = items[num11 + 1];
                bool flag = false;
                if (num5 == num9)
                {
                    int num18 = list4.Count - 4;
                    float[] numArray3 = list4.Items;
                    int num19 = Winding(numArray3[num18], numArray3[num18 + 1], numArray3[num18 + 2], numArray3[num18 + 3], num16, num17);
                    int num20 = Winding(num16, num17, numArray3[0], numArray3[1], numArray3[2], numArray3[3]);
                    if ((num19 == num6) && (num20 == num6))
                    {
                        list4.Add(num16);
                        list4.Add(num17);
                        item.Add(num11);
                        flag = true;
                    }
                }
                if (!flag)
                {
                    if (list4.Count > 0)
                    {
                        convexPolygons.Add(list4);
                        convexPolygonsIndices.Add(item);
                    }
                    else
                    {
                        this.polygonPool.Free(list4);
                        this.polygonIndicesPool.Free(item);
                    }
                    list4 = this.polygonPool.Obtain();
                    list4.Clear(true);
                    list4.Add(num12);
                    list4.Add(num13);
                    list4.Add(num14);
                    list4.Add(num15);
                    list4.Add(num16);
                    list4.Add(num17);
                    item = this.polygonIndicesPool.Obtain();
                    item.Clear(true);
                    item.Add(num9);
                    item.Add(num10);
                    item.Add(num11);
                    num6 = Winding(num12, num13, num14, num15, num16, num17);
                    num5 = num9;
                }
                num7 += 3;
            }
            if (list4.Count > 0)
            {
                convexPolygons.Add(list4);
                convexPolygonsIndices.Add(item);
            }
            int num21 = 0;
            int num22 = convexPolygons.Count;
            while (num21 < num22)
            {
                item = convexPolygonsIndices.Items[num21];
                if (item.Count != 0)
                {
                    int num23 = item.Items[0];
                    int num24 = item.Items[item.Count - 1];
                    list4 = convexPolygons.Items[num21];
                    int num25 = list4.Count - 4;
                    float[] numArray4 = list4.Items;
                    float num26 = numArray4[num25];
                    float num27 = numArray4[num25 + 1];
                    float num28 = numArray4[num25 + 2];
                    float num29 = numArray4[num25 + 3];
                    float num30 = numArray4[0];
                    float num31 = numArray4[1];
                    float num32 = numArray4[2];
                    float num33 = numArray4[3];
                    int num34 = Winding(num26, num27, num28, num29, num30, num31);
                    for (int j = 0; j < num22; j++)
                    {
                        if (j != num21)
                        {
                            ExposedList<int> list5 = convexPolygonsIndices.Items[j];
                            if (list5.Count == 3)
                            {
                                int num36 = list5.Items[0];
                                int num37 = list5.Items[1];
                                int num38 = list5.Items[2];
                                ExposedList<float> list6 = convexPolygons.Items[j];
                                float num39 = list6.Items[list6.Count - 2];
                                float num40 = list6.Items[list6.Count - 1];
                                if ((num36 == num23) && (num37 == num24))
                                {
                                    int num41 = Winding(num26, num27, num28, num29, num39, num40);
                                    int num42 = Winding(num39, num40, num30, num31, num32, num33);
                                    if ((num41 == num34) && (num42 == num34))
                                    {
                                        list6.Clear(true);
                                        list5.Clear(true);
                                        list4.Add(num39);
                                        list4.Add(num40);
                                        item.Add(num38);
                                        num26 = num28;
                                        num27 = num29;
                                        num28 = num39;
                                        num29 = num40;
                                        j = 0;
                                    }
                                }
                            }
                        }
                    }
                }
                num21++;
            }
            for (int i = convexPolygons.Count - 1; i >= 0; i--)
            {
                list4 = convexPolygons.Items[i];
                if (list4.Count == 0)
                {
                    convexPolygons.RemoveAt(i);
                    this.polygonPool.Free(list4);
                    item = convexPolygonsIndices.Items[i];
                    convexPolygonsIndices.RemoveAt(i);
                    this.polygonIndicesPool.Free(item);
                }
            }
            return convexPolygons;
        }

        private static bool IsConcave(int index, int vertexCount, float[] vertices, int[] indices)
        {
            int num = indices[((vertexCount + index) - 1) % vertexCount] << 1;
            int num2 = indices[index] << 1;
            int num3 = indices[(index + 1) % vertexCount] << 1;
            return !PositiveArea(vertices[num], vertices[num + 1], vertices[num2], vertices[num2 + 1], vertices[num3], vertices[num3 + 1]);
        }

        private static bool PositiveArea(float p1x, float p1y, float p2x, float p2y, float p3x, float p3y) => 
            ((((p1x * (p3y - p2y)) + (p2x * (p1y - p3y))) + (p3x * (p2y - p1y))) >= 0f);

        public ExposedList<int> Triangulate(ExposedList<float> verticesArray)
        {
            float[] items = verticesArray.Items;
            int newSize = verticesArray.Count >> 1;
            ExposedList<int> indicesArray = this.indicesArray;
            indicesArray.Clear(true);
            int[] indices = indicesArray.Resize(newSize).Items;
            for (int i = 0; i < newSize; i++)
            {
                indices[i] = i;
            }
            ExposedList<bool> isConcaveArray = this.isConcaveArray;
            bool[] flagArray = isConcaveArray.Resize(newSize).Items;
            int index = 0;
            int num4 = newSize;
            while (index < num4)
            {
                flagArray[index] = IsConcave(index, newSize, items, indices);
                index++;
            }
            ExposedList<int> triangles = this.triangles;
            triangles.Clear(true);
            triangles.EnsureCapacity(Math.Max(0, newSize - 2) << 2);
            while (newSize > 3)
            {
                int num5 = newSize - 1;
                int num6 = 0;
                int num7 = 1;
            Label_00B9:
                if (!flagArray[num6])
                {
                    int num8 = indices[num5] << 1;
                    int num9 = indices[num6] << 1;
                    int num10 = indices[num7] << 1;
                    float num11 = items[num8];
                    float num12 = items[num8 + 1];
                    float num13 = items[num9];
                    float num14 = items[num9 + 1];
                    float num15 = items[num10];
                    float num16 = items[num10 + 1];
                    for (int j = (num7 + 1) % newSize; j != num5; j = (j + 1) % newSize)
                    {
                        if (flagArray[j])
                        {
                            int num18 = indices[j] << 1;
                            float num19 = items[num18];
                            float num20 = items[num18 + 1];
                            if ((PositiveArea(num15, num16, num11, num12, num19, num20) && PositiveArea(num11, num12, num13, num14, num19, num20)) && PositiveArea(num13, num14, num15, num16, num19, num20))
                            {
                                goto Label_0194;
                            }
                        }
                    }
                    goto Label_01D2;
                }
            Label_0194:
                if (num7 == 0)
                {
                    do
                    {
                        if (!flagArray[num6])
                        {
                            break;
                        }
                        num6--;
                    }
                    while (num6 > 0);
                }
                else
                {
                    num5 = num6;
                    num6 = num7;
                    num7 = (num7 + 1) % newSize;
                    goto Label_00B9;
                }
            Label_01D2:
                triangles.Add(indices[((newSize + num6) - 1) % newSize]);
                triangles.Add(indices[num6]);
                triangles.Add(indices[(num6 + 1) % newSize]);
                indicesArray.RemoveAt(num6);
                isConcaveArray.RemoveAt(num6);
                newSize--;
                int num21 = ((newSize + num6) - 1) % newSize;
                int num22 = (num6 != newSize) ? num6 : 0;
                flagArray[num21] = IsConcave(num21, newSize, items, indices);
                flagArray[num22] = IsConcave(num22, newSize, items, indices);
            }
            if (newSize == 3)
            {
                triangles.Add(indices[2]);
                triangles.Add(indices[0]);
                triangles.Add(indices[1]);
            }
            return triangles;
        }

        private static int Winding(float p1x, float p1y, float p2x, float p2y, float p3x, float p3y)
        {
            float num = p2x - p1x;
            float num2 = p2y - p1y;
            return ((((((p3x * num2) - (p3y * num)) + (num * p1y)) - (p1x * num2)) < 0f) ? -1 : 1);
        }
    }
}

