namespace Spine
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct BoneMatrix
    {
        public float a;
        public float b;
        public float c;
        public float d;
        public float x;
        public float y;
        public BoneMatrix(BoneData boneData)
        {
            float degrees = (boneData.rotation + 90f) + boneData.shearY;
            float num2 = boneData.rotation + boneData.shearX;
            this.a = MathUtils.CosDeg(num2) * boneData.scaleX;
            this.c = MathUtils.SinDeg(num2) * boneData.scaleX;
            this.b = MathUtils.CosDeg(degrees) * boneData.scaleY;
            this.d = MathUtils.SinDeg(degrees) * boneData.scaleY;
            this.x = boneData.x;
            this.y = boneData.y;
        }

        public BoneMatrix(Bone bone)
        {
            float degrees = (bone.rotation + 90f) + bone.shearY;
            float num2 = bone.rotation + bone.shearX;
            this.a = MathUtils.CosDeg(num2) * bone.scaleX;
            this.c = MathUtils.SinDeg(num2) * bone.scaleX;
            this.b = MathUtils.CosDeg(degrees) * bone.scaleY;
            this.d = MathUtils.SinDeg(degrees) * bone.scaleY;
            this.x = bone.x;
            this.y = bone.y;
        }

        public static BoneMatrix CalculateSetupWorld(BoneData boneData)
        {
            if (boneData == null)
            {
                return new BoneMatrix();
            }
            if (boneData.parent == null)
            {
                return GetInheritedInternal(boneData, new BoneMatrix());
            }
            BoneMatrix parentMatrix = CalculateSetupWorld(boneData.parent);
            return GetInheritedInternal(boneData, parentMatrix);
        }

        private static BoneMatrix GetInheritedInternal(BoneData boneData, BoneMatrix parentMatrix)
        {
            float num12;
            if (boneData.parent == null)
            {
                return new BoneMatrix(boneData);
            }
            float a = parentMatrix.a;
            float b = parentMatrix.b;
            float c = parentMatrix.c;
            float d = parentMatrix.d;
            BoneMatrix matrix = new BoneMatrix {
                x = ((a * boneData.x) + (b * boneData.y)) + parentMatrix.x,
                y = ((c * boneData.x) + (d * boneData.y)) + parentMatrix.y
            };
            switch (boneData.transformMode)
            {
                case TransformMode.Normal:
                {
                    float num5 = (boneData.rotation + 90f) + boneData.shearY;
                    float num6 = MathUtils.CosDeg(boneData.rotation + boneData.shearX) * boneData.scaleX;
                    float num7 = MathUtils.CosDeg(num5) * boneData.scaleY;
                    float num8 = MathUtils.SinDeg(boneData.rotation + boneData.shearX) * boneData.scaleX;
                    float num9 = MathUtils.SinDeg(num5) * boneData.scaleY;
                    matrix.a = (a * num6) + (b * num8);
                    matrix.b = (a * num7) + (b * num9);
                    matrix.c = (c * num6) + (d * num8);
                    matrix.d = (c * num7) + (d * num9);
                    return matrix;
                }
                case TransformMode.NoRotationOrReflection:
                {
                    float num11 = (a * a) + (c * c);
                    if (num11 <= 0.0001f)
                    {
                        a = 0f;
                        c = 0f;
                        num12 = 90f - (MathUtils.Atan2(d, b) * 57.29578f);
                        break;
                    }
                    num11 = Math.Abs((float) ((a * d) - (b * c))) / num11;
                    b = c * num11;
                    d = a * num11;
                    num12 = MathUtils.Atan2(c, a) * 57.29578f;
                    break;
                }
                case TransformMode.NoScale:
                case TransformMode.NoScaleOrReflection:
                {
                    float num19 = MathUtils.CosDeg(boneData.rotation);
                    float num20 = MathUtils.SinDeg(boneData.rotation);
                    float x = (a * num19) + (b * num20);
                    float y = (c * num19) + (d * num20);
                    float num23 = (float) Math.Sqrt((double) ((x * x) + (y * y)));
                    if (num23 > 1E-05f)
                    {
                        num23 = 1f / num23;
                    }
                    x *= num23;
                    y *= num23;
                    num23 = (float) Math.Sqrt((double) ((x * x) + (y * y)));
                    float radians = 1.570796f + MathUtils.Atan2(y, x);
                    float num25 = MathUtils.Cos(radians) * num23;
                    float num26 = MathUtils.Sin(radians) * num23;
                    float num27 = MathUtils.CosDeg(boneData.shearX) * boneData.scaleX;
                    float num28 = MathUtils.CosDeg(90f + boneData.shearY) * boneData.scaleY;
                    float num29 = MathUtils.SinDeg(boneData.shearX) * boneData.scaleX;
                    float num30 = MathUtils.SinDeg(90f + boneData.shearY) * boneData.scaleY;
                    if ((boneData.transformMode != TransformMode.NoScaleOrReflection) && (((a * d) - (b * c)) < 0f))
                    {
                        num25 = -num25;
                        num26 = -num26;
                    }
                    matrix.a = (x * num27) + (num25 * num29);
                    matrix.b = (x * num28) + (num25 * num30);
                    matrix.c = (y * num27) + (num26 * num29);
                    matrix.d = (y * num28) + (num26 * num30);
                    return matrix;
                }
                case (TransformMode.NoScale | TransformMode.NoRotationOrReflection):
                case 4:
                case 5:
                    return matrix;

                case TransformMode.OnlyTranslation:
                {
                    float num10 = (boneData.rotation + 90f) + boneData.shearY;
                    matrix.a = MathUtils.CosDeg(boneData.rotation + boneData.shearX) * boneData.scaleX;
                    matrix.b = MathUtils.CosDeg(num10) * boneData.scaleY;
                    matrix.c = MathUtils.SinDeg(boneData.rotation + boneData.shearX) * boneData.scaleX;
                    matrix.d = MathUtils.SinDeg(num10) * boneData.scaleY;
                    return matrix;
                }
                default:
                    return matrix;
            }
            float degrees = (boneData.rotation + boneData.shearX) - num12;
            float num14 = ((boneData.rotation + boneData.shearY) - num12) + 90f;
            float num15 = MathUtils.CosDeg(degrees) * boneData.scaleX;
            float num16 = MathUtils.CosDeg(num14) * boneData.scaleY;
            float num17 = MathUtils.SinDeg(degrees) * boneData.scaleX;
            float num18 = MathUtils.SinDeg(num14) * boneData.scaleY;
            matrix.a = (a * num15) - (b * num17);
            matrix.b = (a * num16) - (b * num18);
            matrix.c = (c * num15) + (d * num17);
            matrix.d = (c * num16) + (d * num18);
            return matrix;
        }

        public BoneMatrix TransformMatrix(BoneMatrix local) => 
            new BoneMatrix { 
                a = (this.a * local.a) + (this.b * local.c),
                b = (this.a * local.b) + (this.b * local.d),
                c = (this.c * local.a) + (this.d * local.c),
                d = (this.c * local.b) + (this.d * local.d),
                x = ((this.a * local.x) + (this.b * local.y)) + this.x,
                y = ((this.c * local.x) + (this.d * local.y)) + this.y
            };
    }
}

