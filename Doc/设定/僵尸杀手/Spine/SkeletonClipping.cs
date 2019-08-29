namespace Spine
{
    using System;

    public class SkeletonClipping
    {
        internal readonly Triangulator triangulator = new Triangulator();
        internal readonly ExposedList<float> clippingPolygon = new ExposedList<float>();
        internal readonly ExposedList<float> clipOutput = new ExposedList<float>(0x80);
        internal readonly ExposedList<float> clippedVertices = new ExposedList<float>(0x80);
        internal readonly ExposedList<int> clippedTriangles = new ExposedList<int>(0x80);
        internal readonly ExposedList<float> clippedUVs = new ExposedList<float>(0x80);
        internal readonly ExposedList<float> scratch = new ExposedList<float>();
        internal ClippingAttachment clipAttachment;
        internal ExposedList<ExposedList<float>> clippingPolygons;

        internal bool Clip(float x1, float y1, float x2, float y2, float x3, float y3, ExposedList<float> clippingArea, ExposedList<float> output)
        {
            ExposedList<float> list = output;
            bool flag = false;
            ExposedList<float> scratch = null;
            if ((clippingArea.Count % 4) >= 2)
            {
                scratch = output;
                output = this.scratch;
            }
            else
            {
                scratch = this.scratch;
            }
            scratch.Clear(true);
            scratch.Add(x1);
            scratch.Add(y1);
            scratch.Add(x2);
            scratch.Add(y2);
            scratch.Add(x3);
            scratch.Add(y3);
            scratch.Add(x1);
            scratch.Add(y1);
            output.Clear(true);
            float[] items = clippingArea.Items;
            int num = clippingArea.Count - 4;
            int index = 0;
            while (true)
            {
                float num3 = items[index];
                float num4 = items[index + 1];
                float num5 = items[index + 2];
                float num6 = items[index + 3];
                float num7 = num3 - num5;
                float num8 = num4 - num6;
                float[] numArray2 = scratch.Items;
                int num9 = scratch.Count - 2;
                int count = output.Count;
                for (int i = 0; i < num9; i += 2)
                {
                    float num12 = numArray2[i];
                    float num13 = numArray2[i + 1];
                    float item = numArray2[i + 2];
                    float num15 = numArray2[i + 3];
                    bool flag2 = ((num7 * (num15 - num6)) - (num8 * (item - num5))) > 0f;
                    if (((num7 * (num13 - num6)) - (num8 * (num12 - num5))) > 0f)
                    {
                        if (flag2)
                        {
                            output.Add(item);
                            output.Add(num15);
                            continue;
                        }
                        float num16 = num15 - num13;
                        float num17 = item - num12;
                        float num18 = ((num17 * (num4 - num13)) - (num16 * (num3 - num12))) / ((num16 * (num5 - num3)) - (num17 * (num6 - num4)));
                        output.Add(num3 + ((num5 - num3) * num18));
                        output.Add(num4 + ((num6 - num4) * num18));
                    }
                    else if (flag2)
                    {
                        float num19 = num15 - num13;
                        float num20 = item - num12;
                        float num21 = ((num20 * (num4 - num13)) - (num19 * (num3 - num12))) / ((num19 * (num5 - num3)) - (num20 * (num6 - num4)));
                        output.Add(num3 + ((num5 - num3) * num21));
                        output.Add(num4 + ((num6 - num4) * num21));
                        output.Add(item);
                        output.Add(num15);
                    }
                    flag = true;
                }
                if (count == output.Count)
                {
                    list.Clear(true);
                    return true;
                }
                output.Add(output.Items[0]);
                output.Add(output.Items[1]);
                if (index == num)
                {
                    break;
                }
                ExposedList<float> list3 = output;
                output = scratch;
                output.Clear(true);
                scratch = list3;
                index += 2;
            }
            if (list != output)
            {
                list.Clear(true);
                int num22 = 0;
                int num23 = output.Count - 2;
                while (num22 < num23)
                {
                    list.Add(output.Items[num22]);
                    num22++;
                }
                return flag;
            }
            list.Resize(list.Count - 2);
            return flag;
        }

        public void ClipEnd()
        {
            if (this.clipAttachment != null)
            {
                this.clipAttachment = null;
                this.clippingPolygons = null;
                this.clippedVertices.Clear(true);
                this.clippedTriangles.Clear(true);
                this.clippingPolygon.Clear(true);
            }
        }

        public void ClipEnd(Slot slot)
        {
            if ((this.clipAttachment != null) && (this.clipAttachment.endSlot == slot.data))
            {
                this.ClipEnd();
            }
        }

        public int ClipStart(Slot slot, ClippingAttachment clip)
        {
            if (this.clipAttachment != null)
            {
                return 0;
            }
            this.clipAttachment = clip;
            int worldVerticesLength = clip.worldVerticesLength;
            float[] items = this.clippingPolygon.Resize(worldVerticesLength).Items;
            clip.ComputeWorldVertices(slot, 0, worldVerticesLength, items, 0, 2);
            MakeClockwise(this.clippingPolygon);
            this.clippingPolygons = this.triangulator.Decompose(this.clippingPolygon, this.triangulator.Triangulate(this.clippingPolygon));
            foreach (ExposedList<float> list in this.clippingPolygons)
            {
                MakeClockwise(list);
                list.Add(list.Items[0]);
                list.Add(list.Items[1]);
            }
            return this.clippingPolygons.Count;
        }

        public void ClipTriangles(float[] vertices, int verticesLength, int[] triangles, int trianglesLength, float[] uvs)
        {
            ExposedList<float> clipOutput = this.clipOutput;
            ExposedList<float> clippedVertices = this.clippedVertices;
            ExposedList<int> clippedTriangles = this.clippedTriangles;
            ExposedList<float>[] items = this.clippingPolygons.Items;
            int count = this.clippingPolygons.Count;
            int num2 = 0;
            clippedVertices.Clear(true);
            this.clippedUVs.Clear(true);
            clippedTriangles.Clear(true);
            for (int i = 0; i < trianglesLength; i += 3)
            {
                int index = triangles[i] << 1;
                float num5 = vertices[index];
                float num6 = vertices[index + 1];
                float num7 = uvs[index];
                float num8 = uvs[index + 1];
                index = triangles[i + 1] << 1;
                float num9 = vertices[index];
                float num10 = vertices[index + 1];
                float num11 = uvs[index];
                float num12 = uvs[index + 1];
                index = triangles[i + 2] << 1;
                float num13 = vertices[index];
                float num14 = vertices[index + 1];
                float num15 = uvs[index];
                float num16 = uvs[index + 1];
                for (int j = 0; j < count; j++)
                {
                    int num18 = clippedVertices.Count;
                    if (this.Clip(num5, num6, num9, num10, num13, num14, items[j], clipOutput))
                    {
                        int num19 = clipOutput.Count;
                        if (num19 != 0)
                        {
                            float num20 = num10 - num14;
                            float num21 = num13 - num9;
                            float num22 = num5 - num13;
                            float num23 = num14 - num6;
                            float num24 = 1f / ((num20 * num22) + (num21 * (num6 - num14)));
                            int num25 = num19 >> 1;
                            float[] numArray = clipOutput.Items;
                            float[] numArray2 = clippedVertices.Resize(num18 + (num25 * 2)).Items;
                            float[] numArray3 = this.clippedUVs.Resize(num18 + (num25 * 2)).Items;
                            for (int k = 0; k < num19; k += 2)
                            {
                                float num27 = numArray[k];
                                float num28 = numArray[k + 1];
                                numArray2[num18] = num27;
                                numArray2[num18 + 1] = num28;
                                float num29 = num27 - num13;
                                float num30 = num28 - num14;
                                float num31 = ((num20 * num29) + (num21 * num30)) * num24;
                                float num32 = ((num23 * num29) + (num22 * num30)) * num24;
                                float num33 = (1f - num31) - num32;
                                numArray3[num18] = ((num7 * num31) + (num11 * num32)) + (num15 * num33);
                                numArray3[num18 + 1] = ((num8 * num31) + (num12 * num32)) + (num16 * num33);
                                num18 += 2;
                            }
                            num18 = clippedTriangles.Count;
                            int[] numArray4 = clippedTriangles.Resize(num18 + (3 * (num25 - 2))).Items;
                            num25--;
                            for (int m = 1; m < num25; m++)
                            {
                                numArray4[num18] = num2;
                                numArray4[num18 + 1] = num2 + m;
                                numArray4[num18 + 2] = (num2 + m) + 1;
                                num18 += 3;
                            }
                            num2 += num25 + 1;
                        }
                    }
                    else
                    {
                        float[] numArray5 = clippedVertices.Resize(num18 + 6).Items;
                        float[] numArray6 = this.clippedUVs.Resize(num18 + 6).Items;
                        numArray5[num18] = num5;
                        numArray5[num18 + 1] = num6;
                        numArray5[num18 + 2] = num9;
                        numArray5[num18 + 3] = num10;
                        numArray5[num18 + 4] = num13;
                        numArray5[num18 + 5] = num14;
                        numArray6[num18] = num7;
                        numArray6[num18 + 1] = num8;
                        numArray6[num18 + 2] = num11;
                        numArray6[num18 + 3] = num12;
                        numArray6[num18 + 4] = num15;
                        numArray6[num18 + 5] = num16;
                        num18 = clippedTriangles.Count;
                        int[] numArray7 = clippedTriangles.Resize(num18 + 3).Items;
                        numArray7[num18] = num2;
                        numArray7[num18 + 1] = num2 + 1;
                        numArray7[num18 + 2] = num2 + 2;
                        num2 += 3;
                        break;
                    }
                }
            }
        }

        private static void MakeClockwise(ExposedList<float> polygon)
        {
            float[] items = polygon.Items;
            int count = polygon.Count;
            float num2 = (items[count - 2] * items[1]) - (items[0] * items[count - 1]);
            int index = 0;
            int num8 = count - 3;
            while (index < num8)
            {
                float num3 = items[index];
                float num4 = items[index + 1];
                float num5 = items[index + 2];
                float num6 = items[index + 3];
                num2 += (num3 * num6) - (num5 * num4);
                index += 2;
            }
            if (num2 >= 0f)
            {
                int num9 = 0;
                int num10 = count - 2;
                int num11 = count >> 1;
                while (num9 < num11)
                {
                    float num12 = items[num9];
                    float num13 = items[num9 + 1];
                    int num14 = num10 - num9;
                    items[num9] = items[num14];
                    items[num9 + 1] = items[num14 + 1];
                    items[num14] = num12;
                    items[num14 + 1] = num13;
                    num9 += 2;
                }
            }
        }

        public ExposedList<float> ClippedVertices =>
            this.clippedVertices;

        public ExposedList<int> ClippedTriangles =>
            this.clippedTriangles;

        public ExposedList<float> ClippedUVs =>
            this.clippedUVs;

        public bool IsClipping =>
            (this.clipAttachment != null);
    }
}

