namespace Spine
{
    using System;

    public class PathConstraint : IConstraint, IUpdatable
    {
        private const int NONE = -1;
        private const int BEFORE = -2;
        private const int AFTER = -3;
        private const float Epsilon = 1E-05f;
        internal PathConstraintData data;
        internal ExposedList<Bone> bones;
        internal Slot target;
        internal float position;
        internal float spacing;
        internal float rotateMix;
        internal float translateMix;
        internal ExposedList<float> spaces = new ExposedList<float>();
        internal ExposedList<float> positions = new ExposedList<float>();
        internal ExposedList<float> world = new ExposedList<float>();
        internal ExposedList<float> curves = new ExposedList<float>();
        internal ExposedList<float> lengths = new ExposedList<float>();
        internal float[] segments = new float[10];

        public PathConstraint(PathConstraintData data, Skeleton skeleton)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data", "data cannot be null.");
            }
            if (skeleton == null)
            {
                throw new ArgumentNullException("skeleton", "skeleton cannot be null.");
            }
            this.data = data;
            this.bones = new ExposedList<Bone>(data.Bones.Count);
            foreach (BoneData data2 in data.bones)
            {
                this.bones.Add(skeleton.FindBone(data2.name));
            }
            this.target = skeleton.FindSlot(data.target.name);
            this.position = data.position;
            this.spacing = data.spacing;
            this.rotateMix = data.rotateMix;
            this.translateMix = data.translateMix;
        }

        private static void AddAfterPosition(float p, float[] temp, int i, float[] output, int o)
        {
            float num = temp[i + 2];
            float num2 = temp[i + 3];
            float x = num - temp[i];
            float y = num2 - temp[i + 1];
            float radians = MathUtils.Atan2(y, x);
            output[o] = num + (p * MathUtils.Cos(radians));
            output[o + 1] = num2 + (p * MathUtils.Sin(radians));
            output[o + 2] = radians;
        }

        private static void AddBeforePosition(float p, float[] temp, int i, float[] output, int o)
        {
            float num = temp[i];
            float num2 = temp[i + 1];
            float x = temp[i + 2] - num;
            float y = temp[i + 3] - num2;
            float radians = MathUtils.Atan2(y, x);
            output[o] = num + (p * MathUtils.Cos(radians));
            output[o + 1] = num2 + (p * MathUtils.Sin(radians));
            output[o + 2] = radians;
        }

        private static void AddCurvePosition(float p, float x1, float y1, float cx1, float cy1, float cx2, float cy2, float x2, float y2, float[] output, int o, bool tangents)
        {
            if ((p < 1E-05f) || float.IsNaN(p))
            {
                p = 1E-05f;
            }
            float num = p * p;
            float num2 = num * p;
            float num3 = 1f - p;
            float num4 = num3 * num3;
            float num5 = num4 * num3;
            float num6 = num3 * p;
            float num7 = num6 * 3f;
            float num8 = num3 * num7;
            float num9 = num7 * p;
            float num10 = (((x1 * num5) + (cx1 * num8)) + (cx2 * num9)) + (x2 * num2);
            float num11 = (((y1 * num5) + (cy1 * num8)) + (cy2 * num9)) + (y2 * num2);
            output[o] = num10;
            output[o + 1] = num11;
            if (tangents)
            {
                output[o + 2] = (float) Math.Atan2((double) (num11 - (((y1 * num4) + ((cy1 * num6) * 2f)) + (cy2 * num))), (double) (num10 - (((x1 * num4) + ((cx1 * num6) * 2f)) + (cx2 * num))));
            }
        }

        public void Apply()
        {
            this.Update();
        }

        private float[] ComputeWorldPositions(PathAttachment path, int spacesCount, bool tangents, bool percentPosition, bool percentSpacing)
        {
            float[] numArray3;
            float num5;
            float num22;
            float num23;
            float num24;
            float num25;
            float num26;
            float num27;
            float num28;
            float num29;
            Slot target = this.target;
            float position = this.position;
            float[] items = this.spaces.Items;
            float[] output = this.positions.Resize((spacesCount * 3) + 2).Items;
            bool closed = path.Closed;
            int worldVerticesLength = path.WorldVerticesLength;
            int index = worldVerticesLength / 6;
            int num4 = -1;
            if (!path.ConstantSpeed)
            {
                float[] lengths = path.Lengths;
                index -= !closed ? 2 : 1;
                num5 = lengths[index];
                if (percentPosition)
                {
                    position *= num5;
                }
                if (percentSpacing)
                {
                    for (int j = 0; j < spacesCount; j++)
                    {
                        items[j] *= num5;
                    }
                }
                numArray3 = this.world.Resize(8).Items;
                int num7 = 0;
                int num8 = 0;
                int num9 = 0;
                while (num7 < spacesCount)
                {
                    float num12;
                    float num10 = items[num7];
                    position += num10;
                    float p = position;
                    if (closed)
                    {
                        p = p % num5;
                        if (p < 0f)
                        {
                            p += num5;
                        }
                        num9 = 0;
                    }
                    else
                    {
                        if (p < 0f)
                        {
                            if (num4 != -2)
                            {
                                num4 = -2;
                                path.ComputeWorldVertices(target, 2, 4, numArray3, 0, 2);
                            }
                            AddBeforePosition(p, numArray3, 0, output, num8);
                            goto Label_0263;
                        }
                        if (p > num5)
                        {
                            if (num4 != -3)
                            {
                                num4 = -3;
                                path.ComputeWorldVertices(target, worldVerticesLength - 6, 4, numArray3, 0, 2);
                            }
                            AddAfterPosition(p - num5, numArray3, 0, output, num8);
                            goto Label_0263;
                        }
                    }
                Label_017D:
                    num12 = lengths[num9];
                    if (p <= num12)
                    {
                        if (num9 == 0)
                        {
                            p /= num12;
                        }
                        else
                        {
                            float num13 = lengths[num9 - 1];
                            p = (p - num13) / (num12 - num13);
                        }
                    }
                    else
                    {
                        num9++;
                        goto Label_017D;
                    }
                    if (num9 != num4)
                    {
                        num4 = num9;
                        if (closed && (num9 == index))
                        {
                            path.ComputeWorldVertices(target, worldVerticesLength - 4, 4, numArray3, 0, 2);
                            path.ComputeWorldVertices(target, 0, 4, numArray3, 4, 2);
                        }
                        else
                        {
                            path.ComputeWorldVertices(target, (num9 * 6) + 2, 8, numArray3, 0, 2);
                        }
                    }
                    AddCurvePosition(p, numArray3[0], numArray3[1], numArray3[2], numArray3[3], numArray3[4], numArray3[5], numArray3[6], numArray3[7], output, num8, tangents || ((num7 > 0) && (num10 < 1E-05f)));
                Label_0263:
                    num7++;
                    num8 += 3;
                }
                return output;
            }
            if (closed)
            {
                worldVerticesLength += 2;
                numArray3 = this.world.Resize(worldVerticesLength).Items;
                path.ComputeWorldVertices(target, 2, worldVerticesLength - 4, numArray3, 0, 2);
                path.ComputeWorldVertices(target, 0, 2, numArray3, worldVerticesLength - 4, 2);
                numArray3[worldVerticesLength - 2] = numArray3[0];
                numArray3[worldVerticesLength - 1] = numArray3[1];
            }
            else
            {
                index--;
                worldVerticesLength -= 4;
                numArray3 = this.world.Resize(worldVerticesLength).Items;
                path.ComputeWorldVertices(target, 2, worldVerticesLength, numArray3, 0, 2);
            }
            float[] numArray5 = this.curves.Resize(index).Items;
            num5 = 0f;
            float num14 = numArray3[0];
            float num15 = numArray3[1];
            float num16 = 0f;
            float num17 = 0f;
            float num18 = 0f;
            float num19 = 0f;
            float num20 = 0f;
            float num21 = 0f;
            int num30 = 0;
            for (int i = 2; num30 < index; i += 6)
            {
                num16 = numArray3[i];
                num17 = numArray3[i + 1];
                num18 = numArray3[i + 2];
                num19 = numArray3[i + 3];
                num20 = numArray3[i + 4];
                num21 = numArray3[i + 5];
                num22 = ((num14 - (num16 * 2f)) + num18) * 0.1875f;
                num23 = ((num15 - (num17 * 2f)) + num19) * 0.1875f;
                num24 = ((((num16 - num18) * 3f) - num14) + num20) * 0.09375f;
                num25 = ((((num17 - num19) * 3f) - num15) + num21) * 0.09375f;
                num26 = (num22 * 2f) + num24;
                num27 = (num23 * 2f) + num25;
                num28 = (((num16 - num14) * 0.75f) + num22) + (num24 * 0.1666667f);
                num29 = (((num17 - num15) * 0.75f) + num23) + (num25 * 0.1666667f);
                num5 += (float) Math.Sqrt((double) ((num28 * num28) + (num29 * num29)));
                num28 += num26;
                num29 += num27;
                num26 += num24;
                num27 += num25;
                num5 += (float) Math.Sqrt((double) ((num28 * num28) + (num29 * num29)));
                num28 += num26;
                num29 += num27;
                num5 += (float) Math.Sqrt((double) ((num28 * num28) + (num29 * num29)));
                num28 += num26 + num24;
                num29 += num27 + num25;
                num5 += (float) Math.Sqrt((double) ((num28 * num28) + (num29 * num29)));
                numArray5[num30] = num5;
                num14 = num20;
                num15 = num21;
                num30++;
            }
            if (percentPosition)
            {
                position *= num5;
            }
            if (percentSpacing)
            {
                for (int j = 0; j < spacesCount; j++)
                {
                    items[j] *= num5;
                }
            }
            float[] segments = this.segments;
            float num33 = 0f;
            int num34 = 0;
            int o = 0;
            int num36 = 0;
            int num37 = 0;
            while (num34 < spacesCount)
            {
                float num40;
                float num38 = items[num34];
                position += num38;
                float p = position;
                if (closed)
                {
                    p = p % num5;
                    if (p < 0f)
                    {
                        p += num5;
                    }
                    num36 = 0;
                }
                else
                {
                    if (p < 0f)
                    {
                        AddBeforePosition(p, numArray3, 0, output, o);
                        goto Label_0883;
                    }
                    if (p > num5)
                    {
                        AddAfterPosition(p - num5, numArray3, worldVerticesLength - 4, output, o);
                        goto Label_0883;
                    }
                }
            Label_05CD:
                num40 = numArray5[num36];
                if (p <= num40)
                {
                    if (num36 == 0)
                    {
                        p /= num40;
                    }
                    else
                    {
                        float num41 = numArray5[num36 - 1];
                        p = (p - num41) / (num40 - num41);
                    }
                }
                else
                {
                    num36++;
                    goto Label_05CD;
                }
                if (num36 != num4)
                {
                    num4 = num36;
                    int num42 = num36 * 6;
                    num14 = numArray3[num42];
                    num15 = numArray3[num42 + 1];
                    num16 = numArray3[num42 + 2];
                    num17 = numArray3[num42 + 3];
                    num18 = numArray3[num42 + 4];
                    num19 = numArray3[num42 + 5];
                    num20 = numArray3[num42 + 6];
                    num21 = numArray3[num42 + 7];
                    num22 = ((num14 - (num16 * 2f)) + num18) * 0.03f;
                    num23 = ((num15 - (num17 * 2f)) + num19) * 0.03f;
                    num24 = ((((num16 - num18) * 3f) - num14) + num20) * 0.006f;
                    num25 = ((((num17 - num19) * 3f) - num15) + num21) * 0.006f;
                    num26 = (num22 * 2f) + num24;
                    num27 = (num23 * 2f) + num25;
                    num28 = (((num16 - num14) * 0.3f) + num22) + (num24 * 0.1666667f);
                    num29 = (((num17 - num15) * 0.3f) + num23) + (num25 * 0.1666667f);
                    num33 = (float) Math.Sqrt((double) ((num28 * num28) + (num29 * num29)));
                    segments[0] = num33;
                    for (num42 = 1; num42 < 8; num42++)
                    {
                        num28 += num26;
                        num29 += num27;
                        num26 += num24;
                        num27 += num25;
                        num33 += (float) Math.Sqrt((double) ((num28 * num28) + (num29 * num29)));
                        segments[num42] = num33;
                    }
                    num28 += num26;
                    num29 += num27;
                    num33 += (float) Math.Sqrt((double) ((num28 * num28) + (num29 * num29)));
                    segments[8] = num33;
                    num28 += num26 + num24;
                    num29 += num27 + num25;
                    num33 += (float) Math.Sqrt((double) ((num28 * num28) + (num29 * num29)));
                    segments[9] = num33;
                    num37 = 0;
                }
                p *= num33;
                while (true)
                {
                    float num43 = segments[num37];
                    if (p <= num43)
                    {
                        if (num37 == 0)
                        {
                            p /= num43;
                        }
                        else
                        {
                            float num44 = segments[num37 - 1];
                            p = num37 + ((p - num44) / (num43 - num44));
                        }
                        break;
                    }
                    num37++;
                }
                AddCurvePosition(p * 0.1f, num14, num15, num16, num17, num18, num19, num20, num21, output, o, tangents || ((num34 > 0) && (num38 < 1E-05f)));
            Label_0883:
                num34++;
                o += 3;
            }
            return output;
        }

        public void Update()
        {
            PathAttachment path = this.target.Attachment as PathAttachment;
            if (path != null)
            {
                float rotateMix = this.rotateMix;
                float translateMix = this.translateMix;
                bool flag = translateMix > 0f;
                bool flag2 = rotateMix > 0f;
                if (flag || flag2)
                {
                    bool flag6;
                    PathConstraintData data = this.data;
                    SpacingMode spacingMode = data.spacingMode;
                    bool flag3 = spacingMode == SpacingMode.Length;
                    RotateMode rotateMode = data.rotateMode;
                    bool tangents = rotateMode == RotateMode.Tangent;
                    bool flag5 = rotateMode == RotateMode.ChainScale;
                    int count = this.bones.Count;
                    int newSize = !tangents ? (count + 1) : count;
                    Bone[] items = this.bones.Items;
                    ExposedList<float> list = this.spaces.Resize(newSize);
                    ExposedList<float> list2 = null;
                    float spacing = this.spacing;
                    if (flag5 || flag3)
                    {
                        if (flag5)
                        {
                            list2 = this.lengths.Resize(count);
                        }
                        int num6 = 0;
                        int num7 = newSize - 1;
                        while (num6 < num7)
                        {
                            Bone bone = items[num6];
                            float length = bone.data.length;
                            if (length < 1E-05f)
                            {
                                if (flag5)
                                {
                                    list2.Items[num6] = 0f;
                                }
                                list.Items[++num6] = 0f;
                            }
                            else
                            {
                                float num9 = length * bone.a;
                                float num10 = length * bone.c;
                                float num11 = (float) Math.Sqrt((double) ((num9 * num9) + (num10 * num10)));
                                if (flag5)
                                {
                                    list2.Items[num6] = num11;
                                }
                                list.Items[++num6] = ((!flag3 ? spacing : (length + spacing)) * num11) / length;
                            }
                        }
                    }
                    else
                    {
                        for (int j = 1; j < newSize; j++)
                        {
                            list.Items[j] = spacing;
                        }
                    }
                    float[] numArray = this.ComputeWorldPositions(path, newSize, tangents, data.positionMode == PositionMode.Percent, spacingMode == SpacingMode.Percent);
                    float num13 = numArray[0];
                    float num14 = numArray[1];
                    float offsetRotation = data.offsetRotation;
                    if (offsetRotation == 0f)
                    {
                        flag6 = rotateMode == RotateMode.Chain;
                    }
                    else
                    {
                        flag6 = false;
                        Bone bone = this.target.bone;
                        offsetRotation *= (((bone.a * bone.d) - (bone.b * bone.c)) <= 0f) ? -0.01745329f : 0.01745329f;
                    }
                    int index = 0;
                    for (int i = 3; index < count; i += 3)
                    {
                        Bone bone3 = items[index];
                        bone3.worldX += (num13 - bone3.worldX) * translateMix;
                        bone3.worldY += (num14 - bone3.worldY) * translateMix;
                        float num18 = numArray[i];
                        float num19 = numArray[i + 1];
                        float x = num18 - num13;
                        float y = num19 - num14;
                        if (flag5)
                        {
                            float num22 = list2.Items[index];
                            if (num22 >= 1E-05f)
                            {
                                float num23 = (((((float) Math.Sqrt((double) ((x * x) + (y * y)))) / num22) - 1f) * rotateMix) + 1f;
                                bone3.a *= num23;
                                bone3.c *= num23;
                            }
                        }
                        num13 = num18;
                        num14 = num19;
                        if (flag2)
                        {
                            float num28;
                            float num29;
                            float num30;
                            float a = bone3.a;
                            float b = bone3.b;
                            float c = bone3.c;
                            float d = bone3.d;
                            if (tangents)
                            {
                                num28 = numArray[i - 1];
                            }
                            else if (list.Items[index + 1] < 1E-05f)
                            {
                                num28 = numArray[i + 2];
                            }
                            else
                            {
                                num28 = MathUtils.Atan2(y, x);
                            }
                            num28 -= MathUtils.Atan2(c, a);
                            if (flag6)
                            {
                                num29 = MathUtils.Cos(num28);
                                num30 = MathUtils.Sin(num28);
                                float length = bone3.data.length;
                                num13 += ((length * ((num29 * a) - (num30 * c))) - x) * rotateMix;
                                num14 += ((length * ((num30 * a) + (num29 * c))) - y) * rotateMix;
                            }
                            else
                            {
                                num28 += offsetRotation;
                            }
                            if (num28 > 3.141593f)
                            {
                                num28 -= 6.283185f;
                            }
                            else if (num28 < -3.141593f)
                            {
                                num28 += 6.283185f;
                            }
                            num28 *= rotateMix;
                            num29 = MathUtils.Cos(num28);
                            num30 = MathUtils.Sin(num28);
                            bone3.a = (num29 * a) - (num30 * c);
                            bone3.b = (num29 * b) - (num30 * d);
                            bone3.c = (num30 * a) + (num29 * c);
                            bone3.d = (num30 * b) + (num29 * d);
                        }
                        bone3.appliedValid = false;
                        index++;
                    }
                }
            }
        }

        public int Order =>
            this.data.order;

        public float Position
        {
            get => 
                this.position;
            set => 
                (this.position = value);
        }

        public float Spacing
        {
            get => 
                this.spacing;
            set => 
                (this.spacing = value);
        }

        public float RotateMix
        {
            get => 
                this.rotateMix;
            set => 
                (this.rotateMix = value);
        }

        public float TranslateMix
        {
            get => 
                this.translateMix;
            set => 
                (this.translateMix = value);
        }

        public ExposedList<Bone> Bones =>
            this.bones;

        public Slot Target
        {
            get => 
                this.target;
            set => 
                (this.target = value);
        }

        public PathConstraintData Data =>
            this.data;
    }
}

