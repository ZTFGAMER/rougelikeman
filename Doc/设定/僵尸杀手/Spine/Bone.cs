namespace Spine
{
    using System;
    using System.Runtime.InteropServices;

    public class Bone : IUpdatable
    {
        public static bool yDown;
        internal BoneData data;
        internal Spine.Skeleton skeleton;
        internal Bone parent;
        internal ExposedList<Bone> children = new ExposedList<Bone>();
        internal float x;
        internal float y;
        internal float rotation;
        internal float scaleX;
        internal float scaleY;
        internal float shearX;
        internal float shearY;
        internal float ax;
        internal float ay;
        internal float arotation;
        internal float ascaleX;
        internal float ascaleY;
        internal float ashearX;
        internal float ashearY;
        internal bool appliedValid;
        internal float a;
        internal float b;
        internal float worldX;
        internal float c;
        internal float d;
        internal float worldY;
        internal bool sorted;

        public Bone(BoneData data, Spine.Skeleton skeleton, Bone parent)
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
            this.skeleton = skeleton;
            this.parent = parent;
            this.SetToSetupPose();
        }

        public void LocalToWorld(float localX, float localY, out float worldX, out float worldY)
        {
            worldX = ((localX * this.a) + (localY * this.b)) + this.worldX;
            worldY = ((localX * this.c) + (localY * this.d)) + this.worldY;
        }

        public float LocalToWorldRotation(float localRotation)
        {
            float num = MathUtils.SinDeg(localRotation);
            float num2 = MathUtils.CosDeg(localRotation);
            return (MathUtils.Atan2((num2 * this.c) + (num * this.d), (num2 * this.a) + (num * this.b)) * 57.29578f);
        }

        public void RotateWorld(float degrees)
        {
            float a = this.a;
            float b = this.b;
            float c = this.c;
            float d = this.d;
            float num5 = MathUtils.CosDeg(degrees);
            float num6 = MathUtils.SinDeg(degrees);
            this.a = (num5 * a) - (num6 * c);
            this.b = (num5 * b) - (num6 * d);
            this.c = (num6 * a) + (num5 * c);
            this.d = (num6 * b) + (num5 * d);
            this.appliedValid = false;
        }

        public void SetToSetupPose()
        {
            BoneData data = this.data;
            this.x = data.x;
            this.y = data.y;
            this.rotation = data.rotation;
            this.scaleX = data.scaleX;
            this.scaleY = data.scaleY;
            this.shearX = data.shearX;
            this.shearY = data.shearY;
        }

        public override string ToString() => 
            this.data.name;

        public void Update()
        {
            this.UpdateWorldTransform(this.x, this.y, this.rotation, this.scaleX, this.scaleY, this.shearX, this.shearY);
        }

        internal void UpdateAppliedTransform()
        {
            this.appliedValid = true;
            Bone parent = this.parent;
            if (parent == null)
            {
                this.ax = this.worldX;
                this.ay = this.worldY;
                this.arotation = MathUtils.Atan2(this.c, this.a) * 57.29578f;
                this.ascaleX = (float) Math.Sqrt((double) ((this.a * this.a) + (this.c * this.c)));
                this.ascaleY = (float) Math.Sqrt((double) ((this.b * this.b) + (this.d * this.d)));
                this.ashearX = 0f;
                this.ashearY = MathUtils.Atan2((this.a * this.b) + (this.c * this.d), (this.a * this.d) - (this.b * this.c)) * 57.29578f;
            }
            else
            {
                float a = parent.a;
                float b = parent.b;
                float c = parent.c;
                float d = parent.d;
                float num5 = 1f / ((a * d) - (b * c));
                float num6 = this.worldX - parent.worldX;
                float num7 = this.worldY - parent.worldY;
                this.ax = ((num6 * d) * num5) - ((num7 * b) * num5);
                this.ay = ((num7 * a) * num5) - ((num6 * c) * num5);
                float num8 = num5 * d;
                float num9 = num5 * a;
                float num10 = num5 * b;
                float num11 = num5 * c;
                float x = (num8 * this.a) - (num10 * this.c);
                float num13 = (num8 * this.b) - (num10 * this.d);
                float y = (num9 * this.c) - (num11 * this.a);
                float num15 = (num9 * this.d) - (num11 * this.b);
                this.ashearX = 0f;
                this.ascaleX = (float) Math.Sqrt((double) ((x * x) + (y * y)));
                if (this.ascaleX > 0.0001f)
                {
                    float num16 = (x * num15) - (num13 * y);
                    this.ascaleY = num16 / this.ascaleX;
                    this.ashearY = MathUtils.Atan2((x * num13) + (y * num15), num16) * 57.29578f;
                    this.arotation = MathUtils.Atan2(y, x) * 57.29578f;
                }
                else
                {
                    this.ascaleX = 0f;
                    this.ascaleY = (float) Math.Sqrt((double) ((num13 * num13) + (num15 * num15)));
                    this.ashearY = 0f;
                    this.arotation = 90f - (MathUtils.Atan2(num15, num13) * 57.29578f);
                }
            }
        }

        public void UpdateWorldTransform()
        {
            this.UpdateWorldTransform(this.x, this.y, this.rotation, this.scaleX, this.scaleY, this.shearX, this.shearY);
        }

        public void UpdateWorldTransform(float x, float y, float rotation, float scaleX, float scaleY, float shearX, float shearY)
        {
            float num17;
            this.ax = x;
            this.ay = y;
            this.arotation = rotation;
            this.ascaleX = scaleX;
            this.ascaleY = scaleY;
            this.ashearX = shearX;
            this.ashearY = shearY;
            this.appliedValid = true;
            Spine.Skeleton skeleton = this.skeleton;
            Bone parent = this.parent;
            if (parent == null)
            {
                float num = (rotation + 90f) + shearY;
                float num2 = MathUtils.CosDeg(rotation + shearX) * scaleX;
                float num3 = MathUtils.CosDeg(num) * scaleY;
                float num4 = MathUtils.SinDeg(rotation + shearX) * scaleX;
                float num5 = MathUtils.SinDeg(num) * scaleY;
                if (skeleton.flipX)
                {
                    x = -x;
                    num2 = -num2;
                    num3 = -num3;
                }
                if (skeleton.flipY != yDown)
                {
                    y = -y;
                    num4 = -num4;
                    num5 = -num5;
                }
                this.a = num2;
                this.b = num3;
                this.c = num4;
                this.d = num5;
                this.worldX = x + skeleton.x;
                this.worldY = y + skeleton.y;
                return;
            }
            float a = parent.a;
            float b = parent.b;
            float c = parent.c;
            float d = parent.d;
            this.worldX = ((a * x) + (b * y)) + parent.worldX;
            this.worldY = ((c * x) + (d * y)) + parent.worldY;
            switch (this.data.transformMode)
            {
                case TransformMode.Normal:
                {
                    float num10 = (rotation + 90f) + shearY;
                    float num11 = MathUtils.CosDeg(rotation + shearX) * scaleX;
                    float num12 = MathUtils.CosDeg(num10) * scaleY;
                    float num13 = MathUtils.SinDeg(rotation + shearX) * scaleX;
                    float num14 = MathUtils.SinDeg(num10) * scaleY;
                    this.a = (a * num11) + (b * num13);
                    this.b = (a * num12) + (b * num14);
                    this.c = (c * num11) + (d * num13);
                    this.d = (c * num12) + (d * num14);
                    return;
                }
                case TransformMode.NoRotationOrReflection:
                {
                    float num16 = (a * a) + (c * c);
                    if (num16 <= 0.0001f)
                    {
                        a = 0f;
                        c = 0f;
                        num17 = 90f - (MathUtils.Atan2(d, b) * 57.29578f);
                        break;
                    }
                    num16 = Math.Abs((float) ((a * d) - (b * c))) / num16;
                    b = c * num16;
                    d = a * num16;
                    num17 = MathUtils.Atan2(c, a) * 57.29578f;
                    break;
                }
                case TransformMode.NoScale:
                case TransformMode.NoScaleOrReflection:
                {
                    float num24 = MathUtils.CosDeg(rotation);
                    float num25 = MathUtils.SinDeg(rotation);
                    float num26 = (a * num24) + (b * num25);
                    float num27 = (c * num24) + (d * num25);
                    float num28 = (float) Math.Sqrt((double) ((num26 * num26) + (num27 * num27)));
                    if (num28 > 1E-05f)
                    {
                        num28 = 1f / num28;
                    }
                    num26 *= num28;
                    num27 *= num28;
                    num28 = (float) Math.Sqrt((double) ((num26 * num26) + (num27 * num27)));
                    float radians = 1.570796f + MathUtils.Atan2(num27, num26);
                    float num30 = MathUtils.Cos(radians) * num28;
                    float num31 = MathUtils.Sin(radians) * num28;
                    float num32 = MathUtils.CosDeg(shearX) * scaleX;
                    float num33 = MathUtils.CosDeg(90f + shearY) * scaleY;
                    float num34 = MathUtils.SinDeg(shearX) * scaleX;
                    float num35 = MathUtils.SinDeg(90f + shearY) * scaleY;
                    if ((this.data.transformMode == TransformMode.NoScaleOrReflection) ? (skeleton.flipX != skeleton.flipY) : (((a * d) - (b * c)) < 0f))
                    {
                        num30 = -num30;
                        num31 = -num31;
                    }
                    this.a = (num26 * num32) + (num30 * num34);
                    this.b = (num26 * num33) + (num30 * num35);
                    this.c = (num27 * num32) + (num31 * num34);
                    this.d = (num27 * num33) + (num31 * num35);
                    return;
                }
                case TransformMode.OnlyTranslation:
                {
                    float num15 = (rotation + 90f) + shearY;
                    this.a = MathUtils.CosDeg(rotation + shearX) * scaleX;
                    this.b = MathUtils.CosDeg(num15) * scaleY;
                    this.c = MathUtils.SinDeg(rotation + shearX) * scaleX;
                    this.d = MathUtils.SinDeg(num15) * scaleY;
                    goto Label_04CC;
                }
                default:
                    goto Label_04CC;
            }
            float degrees = (rotation + shearX) - num17;
            float num19 = ((rotation + shearY) - num17) + 90f;
            float num20 = MathUtils.CosDeg(degrees) * scaleX;
            float num21 = MathUtils.CosDeg(num19) * scaleY;
            float num22 = MathUtils.SinDeg(degrees) * scaleX;
            float num23 = MathUtils.SinDeg(num19) * scaleY;
            this.a = (a * num20) - (b * num22);
            this.b = (a * num21) - (b * num23);
            this.c = (c * num20) + (d * num22);
            this.d = (c * num21) + (d * num23);
        Label_04CC:
            if (skeleton.flipX)
            {
                this.a = -this.a;
                this.b = -this.b;
            }
            if (skeleton.flipY != yDown)
            {
                this.c = -this.c;
                this.d = -this.d;
            }
        }

        public void WorldToLocal(float worldX, float worldY, out float localX, out float localY)
        {
            float a = this.a;
            float b = this.b;
            float c = this.c;
            float d = this.d;
            float num5 = 1f / ((a * d) - (b * c));
            float num6 = worldX - this.worldX;
            float num7 = worldY - this.worldY;
            localX = ((num6 * d) * num5) - ((num7 * b) * num5);
            localY = ((num7 * a) * num5) - ((num6 * c) * num5);
        }

        public float WorldToLocalRotation(float worldRotation)
        {
            float num = MathUtils.SinDeg(worldRotation);
            float num2 = MathUtils.CosDeg(worldRotation);
            return (MathUtils.Atan2((this.a * num) - (this.c * num2), (this.d * num2) - (this.b * num)) * 57.29578f);
        }

        public BoneData Data =>
            this.data;

        public Spine.Skeleton Skeleton =>
            this.skeleton;

        public Bone Parent =>
            this.parent;

        public ExposedList<Bone> Children =>
            this.children;

        public float X
        {
            get => 
                this.x;
            set => 
                (this.x = value);
        }

        public float Y
        {
            get => 
                this.y;
            set => 
                (this.y = value);
        }

        public float Rotation
        {
            get => 
                this.rotation;
            set => 
                (this.rotation = value);
        }

        public float ScaleX
        {
            get => 
                this.scaleX;
            set => 
                (this.scaleX = value);
        }

        public float ScaleY
        {
            get => 
                this.scaleY;
            set => 
                (this.scaleY = value);
        }

        public float ShearX
        {
            get => 
                this.shearX;
            set => 
                (this.shearX = value);
        }

        public float ShearY
        {
            get => 
                this.shearY;
            set => 
                (this.shearY = value);
        }

        public float AppliedRotation
        {
            get => 
                this.arotation;
            set => 
                (this.arotation = value);
        }

        public float AX
        {
            get => 
                this.ax;
            set => 
                (this.ax = value);
        }

        public float AY
        {
            get => 
                this.ay;
            set => 
                (this.ay = value);
        }

        public float AScaleX
        {
            get => 
                this.ascaleX;
            set => 
                (this.ascaleX = value);
        }

        public float AScaleY
        {
            get => 
                this.ascaleY;
            set => 
                (this.ascaleY = value);
        }

        public float AShearX
        {
            get => 
                this.ashearX;
            set => 
                (this.ashearX = value);
        }

        public float AShearY
        {
            get => 
                this.ashearY;
            set => 
                (this.ashearY = value);
        }

        public float A =>
            this.a;

        public float B =>
            this.b;

        public float C =>
            this.c;

        public float D =>
            this.d;

        public float WorldX =>
            this.worldX;

        public float WorldY =>
            this.worldY;

        public float WorldRotationX =>
            (MathUtils.Atan2(this.c, this.a) * 57.29578f);

        public float WorldRotationY =>
            (MathUtils.Atan2(this.d, this.b) * 57.29578f);

        public float WorldScaleX =>
            ((float) Math.Sqrt((double) ((this.a * this.a) + (this.c * this.c))));

        public float WorldScaleY =>
            ((float) Math.Sqrt((double) ((this.b * this.b) + (this.d * this.d))));

        public float WorldToLocalRotationX
        {
            get
            {
                Bone parent = this.parent;
                if (parent == null)
                {
                    return this.arotation;
                }
                float a = parent.a;
                float b = parent.b;
                float c = parent.c;
                float d = parent.d;
                float num5 = this.a;
                float num6 = this.c;
                return (MathUtils.Atan2((a * num6) - (c * num5), (d * num5) - (b * num6)) * 57.29578f);
            }
        }

        public float WorldToLocalRotationY
        {
            get
            {
                Bone parent = this.parent;
                if (parent == null)
                {
                    return this.arotation;
                }
                float a = parent.a;
                float b = parent.b;
                float c = parent.c;
                float d = parent.d;
                float num5 = this.b;
                float num6 = this.d;
                return (MathUtils.Atan2((a * num6) - (c * num5), (d * num5) - (b * num6)) * 57.29578f);
            }
        }
    }
}

