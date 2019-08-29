namespace Spine
{
    using System;

    public class TransformConstraint : IConstraint, IUpdatable
    {
        internal TransformConstraintData data;
        internal ExposedList<Bone> bones;
        internal Bone target;
        internal float rotateMix;
        internal float translateMix;
        internal float scaleMix;
        internal float shearMix;

        public TransformConstraint(TransformConstraintData data, Skeleton skeleton)
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
            this.rotateMix = data.rotateMix;
            this.translateMix = data.translateMix;
            this.scaleMix = data.scaleMix;
            this.shearMix = data.shearMix;
            this.bones = new ExposedList<Bone>();
            foreach (BoneData data2 in data.bones)
            {
                this.bones.Add(skeleton.FindBone(data2.name));
            }
            this.target = skeleton.FindBone(data.target.name);
        }

        public void Apply()
        {
            this.Update();
        }

        private void ApplyAbsoluteLocal()
        {
            float rotateMix = this.rotateMix;
            float translateMix = this.translateMix;
            float scaleMix = this.scaleMix;
            float shearMix = this.shearMix;
            Bone target = this.target;
            if (!target.appliedValid)
            {
                target.UpdateAppliedTransform();
            }
            Bone[] items = this.bones.Items;
            int index = 0;
            int count = this.bones.Count;
            while (index < count)
            {
                Bone bone2 = items[index];
                if (!bone2.appliedValid)
                {
                    bone2.UpdateAppliedTransform();
                }
                float arotation = bone2.arotation;
                if (rotateMix != 0f)
                {
                    float num8 = (target.arotation - arotation) + this.data.offsetRotation;
                    num8 -= (0x4000 - ((int) (16384.499999999996 - (num8 / 360f)))) * 360;
                    arotation += num8 * rotateMix;
                }
                float ax = bone2.ax;
                float ay = bone2.ay;
                if (translateMix != 0f)
                {
                    ax += ((target.ax - ax) + this.data.offsetX) * translateMix;
                    ay += ((target.ay - ay) + this.data.offsetY) * translateMix;
                }
                float ascaleX = bone2.ascaleX;
                float ascaleY = bone2.ascaleY;
                if (scaleMix > 0f)
                {
                    if (ascaleX > 1E-05f)
                    {
                        ascaleX = (ascaleX + (((target.ascaleX - ascaleX) + this.data.offsetScaleX) * scaleMix)) / ascaleX;
                    }
                    if (ascaleY > 1E-05f)
                    {
                        ascaleY = (ascaleY + (((target.ascaleY - ascaleY) + this.data.offsetScaleY) * scaleMix)) / ascaleY;
                    }
                }
                float ashearY = bone2.ashearY;
                if (shearMix > 0f)
                {
                    float num14 = (target.ashearY - ashearY) + this.data.offsetShearY;
                    num14 -= (0x4000 - ((int) (16384.499999999996 - (num14 / 360f)))) * 360;
                    bone2.shearY += num14 * shearMix;
                }
                bone2.UpdateWorldTransform(ax, ay, arotation, ascaleX, ascaleY, bone2.ashearX, ashearY);
                index++;
            }
        }

        private void ApplyAbsoluteWorld()
        {
            float rotateMix = this.rotateMix;
            float translateMix = this.translateMix;
            float scaleMix = this.scaleMix;
            float shearMix = this.shearMix;
            Bone target = this.target;
            float a = target.a;
            float b = target.b;
            float c = target.c;
            float d = target.d;
            float num9 = (((a * d) - (b * c)) <= 0f) ? -0.01745329f : 0.01745329f;
            float num10 = this.data.offsetRotation * num9;
            float num11 = this.data.offsetShearY * num9;
            ExposedList<Bone> bones = this.bones;
            int index = 0;
            int count = bones.Count;
            while (index < count)
            {
                Bone bone2 = bones.Items[index];
                bool flag = false;
                if (rotateMix != 0f)
                {
                    float x = bone2.a;
                    float num15 = bone2.b;
                    float y = bone2.c;
                    float num17 = bone2.d;
                    float radians = (MathUtils.Atan2(c, a) - MathUtils.Atan2(y, x)) + num10;
                    if (radians > 3.141593f)
                    {
                        radians -= 6.283185f;
                    }
                    else if (radians < -3.141593f)
                    {
                        radians += 6.283185f;
                    }
                    radians *= rotateMix;
                    float num19 = MathUtils.Cos(radians);
                    float num20 = MathUtils.Sin(radians);
                    bone2.a = (num19 * x) - (num20 * y);
                    bone2.b = (num19 * num15) - (num20 * num17);
                    bone2.c = (num20 * x) + (num19 * y);
                    bone2.d = (num20 * num15) + (num19 * num17);
                    flag = true;
                }
                if (translateMix != 0f)
                {
                    target.LocalToWorld(this.data.offsetX, this.data.offsetY, out float num21, out float num22);
                    bone2.worldX += (num21 - bone2.worldX) * translateMix;
                    bone2.worldY += (num22 - bone2.worldY) * translateMix;
                    flag = true;
                }
                if (scaleMix > 0f)
                {
                    float num23 = (float) Math.Sqrt((double) ((bone2.a * bone2.a) + (bone2.c * bone2.c)));
                    if (num23 > 1E-05f)
                    {
                        num23 = (num23 + (((((float) Math.Sqrt((double) ((a * a) + (c * c)))) - num23) + this.data.offsetScaleX) * scaleMix)) / num23;
                    }
                    bone2.a *= num23;
                    bone2.c *= num23;
                    num23 = (float) Math.Sqrt((double) ((bone2.b * bone2.b) + (bone2.d * bone2.d)));
                    if (num23 > 1E-05f)
                    {
                        num23 = (num23 + (((((float) Math.Sqrt((double) ((b * b) + (d * d)))) - num23) + this.data.offsetScaleY) * scaleMix)) / num23;
                    }
                    bone2.b *= num23;
                    bone2.d *= num23;
                    flag = true;
                }
                if (shearMix > 0f)
                {
                    float x = bone2.b;
                    float y = bone2.d;
                    float num26 = MathUtils.Atan2(y, x);
                    float radians = (MathUtils.Atan2(d, b) - MathUtils.Atan2(c, a)) - (num26 - MathUtils.Atan2(bone2.c, bone2.a));
                    if (radians > 3.141593f)
                    {
                        radians -= 6.283185f;
                    }
                    else if (radians < -3.141593f)
                    {
                        radians += 6.283185f;
                    }
                    radians = num26 + ((radians + num11) * shearMix);
                    float num28 = (float) Math.Sqrt((double) ((x * x) + (y * y)));
                    bone2.b = MathUtils.Cos(radians) * num28;
                    bone2.d = MathUtils.Sin(radians) * num28;
                    flag = true;
                }
                if (flag)
                {
                    bone2.appliedValid = false;
                }
                index++;
            }
        }

        private void ApplyRelativeLocal()
        {
            float rotateMix = this.rotateMix;
            float translateMix = this.translateMix;
            float scaleMix = this.scaleMix;
            float shearMix = this.shearMix;
            Bone target = this.target;
            if (!target.appliedValid)
            {
                target.UpdateAppliedTransform();
            }
            Bone[] items = this.bones.Items;
            int index = 0;
            int count = this.bones.Count;
            while (index < count)
            {
                Bone bone2 = items[index];
                if (!bone2.appliedValid)
                {
                    bone2.UpdateAppliedTransform();
                }
                float arotation = bone2.arotation;
                if (rotateMix != 0f)
                {
                    arotation += (target.arotation + this.data.offsetRotation) * rotateMix;
                }
                float ax = bone2.ax;
                float ay = bone2.ay;
                if (translateMix != 0f)
                {
                    ax += (target.ax + this.data.offsetX) * translateMix;
                    ay += (target.ay + this.data.offsetY) * translateMix;
                }
                float ascaleX = bone2.ascaleX;
                float ascaleY = bone2.ascaleY;
                if (scaleMix > 0f)
                {
                    if (ascaleX > 1E-05f)
                    {
                        ascaleX *= (((target.ascaleX - 1f) + this.data.offsetScaleX) * scaleMix) + 1f;
                    }
                    if (ascaleY > 1E-05f)
                    {
                        ascaleY *= (((target.ascaleY - 1f) + this.data.offsetScaleY) * scaleMix) + 1f;
                    }
                }
                float ashearY = bone2.ashearY;
                if (shearMix > 0f)
                {
                    ashearY += (target.ashearY + this.data.offsetShearY) * shearMix;
                }
                bone2.UpdateWorldTransform(ax, ay, arotation, ascaleX, ascaleY, bone2.ashearX, ashearY);
                index++;
            }
        }

        private void ApplyRelativeWorld()
        {
            float rotateMix = this.rotateMix;
            float translateMix = this.translateMix;
            float scaleMix = this.scaleMix;
            float shearMix = this.shearMix;
            Bone target = this.target;
            float a = target.a;
            float b = target.b;
            float c = target.c;
            float d = target.d;
            float num9 = (((a * d) - (b * c)) <= 0f) ? -0.01745329f : 0.01745329f;
            float num10 = this.data.offsetRotation * num9;
            float num11 = this.data.offsetShearY * num9;
            ExposedList<Bone> bones = this.bones;
            int index = 0;
            int count = bones.Count;
            while (index < count)
            {
                Bone bone2 = bones.Items[index];
                bool flag = false;
                if (rotateMix != 0f)
                {
                    float num14 = bone2.a;
                    float num15 = bone2.b;
                    float num16 = bone2.c;
                    float num17 = bone2.d;
                    float radians = MathUtils.Atan2(c, a) + num10;
                    if (radians > 3.141593f)
                    {
                        radians -= 6.283185f;
                    }
                    else if (radians < -3.141593f)
                    {
                        radians += 6.283185f;
                    }
                    radians *= rotateMix;
                    float num19 = MathUtils.Cos(radians);
                    float num20 = MathUtils.Sin(radians);
                    bone2.a = (num19 * num14) - (num20 * num16);
                    bone2.b = (num19 * num15) - (num20 * num17);
                    bone2.c = (num20 * num14) + (num19 * num16);
                    bone2.d = (num20 * num15) + (num19 * num17);
                    flag = true;
                }
                if (translateMix != 0f)
                {
                    target.LocalToWorld(this.data.offsetX, this.data.offsetY, out float num21, out float num22);
                    bone2.worldX += num21 * translateMix;
                    bone2.worldY += num22 * translateMix;
                    flag = true;
                }
                if (scaleMix > 0f)
                {
                    float num23 = (((((float) Math.Sqrt((double) ((a * a) + (c * c)))) - 1f) + this.data.offsetScaleX) * scaleMix) + 1f;
                    bone2.a *= num23;
                    bone2.c *= num23;
                    num23 = (((((float) Math.Sqrt((double) ((b * b) + (d * d)))) - 1f) + this.data.offsetScaleY) * scaleMix) + 1f;
                    bone2.b *= num23;
                    bone2.d *= num23;
                    flag = true;
                }
                if (shearMix > 0f)
                {
                    float radians = MathUtils.Atan2(d, b) - MathUtils.Atan2(c, a);
                    if (radians > 3.141593f)
                    {
                        radians -= 6.283185f;
                    }
                    else if (radians < -3.141593f)
                    {
                        radians += 6.283185f;
                    }
                    float x = bone2.b;
                    float y = bone2.d;
                    radians = MathUtils.Atan2(y, x) + (((radians - 1.570796f) + num11) * shearMix);
                    float num27 = (float) Math.Sqrt((double) ((x * x) + (y * y)));
                    bone2.b = MathUtils.Cos(radians) * num27;
                    bone2.d = MathUtils.Sin(radians) * num27;
                    flag = true;
                }
                if (flag)
                {
                    bone2.appliedValid = false;
                }
                index++;
            }
        }

        public override string ToString() => 
            this.data.name;

        public void Update()
        {
            if (this.data.local)
            {
                if (this.data.relative)
                {
                    this.ApplyRelativeLocal();
                }
                else
                {
                    this.ApplyAbsoluteLocal();
                }
            }
            else if (this.data.relative)
            {
                this.ApplyRelativeWorld();
            }
            else
            {
                this.ApplyAbsoluteWorld();
            }
        }

        public TransformConstraintData Data =>
            this.data;

        public int Order =>
            this.data.order;

        public ExposedList<Bone> Bones =>
            this.bones;

        public Bone Target
        {
            get => 
                this.target;
            set => 
                (this.target = value);
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

        public float ScaleMix
        {
            get => 
                this.scaleMix;
            set => 
                (this.scaleMix = value);
        }

        public float ShearMix
        {
            get => 
                this.shearMix;
            set => 
                (this.shearMix = value);
        }
    }
}

