namespace Spine
{
    using System;

    public class IkConstraint : IConstraint, IUpdatable
    {
        internal IkConstraintData data;
        internal ExposedList<Bone> bones = new ExposedList<Bone>();
        internal Bone target;
        internal float mix;
        internal int bendDirection;

        public IkConstraint(IkConstraintData data, Skeleton skeleton)
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
            this.mix = data.mix;
            this.bendDirection = data.bendDirection;
            this.bones = new ExposedList<Bone>(data.bones.Count);
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

        public static void Apply(Bone bone, float targetX, float targetY, float alpha)
        {
            if (!bone.appliedValid)
            {
                bone.UpdateAppliedTransform();
            }
            Bone parent = bone.parent;
            float num = 1f / ((parent.a * parent.d) - (parent.b * parent.c));
            float num2 = targetX - parent.worldX;
            float num3 = targetY - parent.worldY;
            float num4 = (((num2 * parent.d) - (num3 * parent.b)) * num) - bone.ax;
            float num5 = (((num3 * parent.a) - (num2 * parent.c)) * num) - bone.ay;
            float num6 = ((((float) Math.Atan2((double) num5, (double) num4)) * 57.29578f) - bone.ashearX) - bone.arotation;
            if (bone.ascaleX < 0f)
            {
                num6 += 180f;
            }
            if (num6 > 180f)
            {
                num6 -= 360f;
            }
            else if (num6 < -180f)
            {
                num6 += 360f;
            }
            bone.UpdateWorldTransform(bone.ax, bone.ay, bone.arotation + (num6 * alpha), bone.ascaleX, bone.ascaleY, bone.ashearX, bone.ashearY);
        }

        public static void Apply(Bone parent, Bone child, float targetX, float targetY, int bendDir, float alpha)
        {
            int num6;
            int num7;
            int num8;
            float num10;
            float num11;
            float num12;
            float num26;
            float num27;
            float num47;
            if (alpha == 0f)
            {
                child.UpdateWorldTransform();
                return;
            }
            if (!parent.appliedValid)
            {
                parent.UpdateAppliedTransform();
            }
            if (!child.appliedValid)
            {
                child.UpdateAppliedTransform();
            }
            float ax = parent.ax;
            float ay = parent.ay;
            float ascaleX = parent.ascaleX;
            float ascaleY = parent.ascaleY;
            float num5 = child.ascaleX;
            if (ascaleX < 0f)
            {
                ascaleX = -ascaleX;
                num6 = 180;
                num8 = -1;
            }
            else
            {
                num6 = 0;
                num8 = 1;
            }
            if (ascaleY < 0f)
            {
                ascaleY = -ascaleY;
                num8 = -num8;
            }
            if (num5 < 0f)
            {
                num5 = -num5;
                num7 = 180;
            }
            else
            {
                num7 = 0;
            }
            float x = child.ax;
            float a = parent.a;
            float b = parent.b;
            float c = parent.c;
            float d = parent.d;
            bool flag = Math.Abs((float) (ascaleX - ascaleY)) <= 0.0001f;
            if (!flag)
            {
                num10 = 0f;
                num11 = (a * x) + parent.worldX;
                num12 = (c * x) + parent.worldY;
            }
            else
            {
                num10 = child.ay;
                num11 = ((a * x) + (b * num10)) + parent.worldX;
                num12 = ((c * x) + (d * num10)) + parent.worldY;
            }
            Bone bone = parent.parent;
            a = bone.a;
            b = bone.b;
            c = bone.c;
            d = bone.d;
            float num17 = 1f / ((a * d) - (b * c));
            float num18 = targetX - bone.worldX;
            float num19 = targetY - bone.worldY;
            float num20 = (((num18 * d) - (num19 * b)) * num17) - ax;
            float num21 = (((num19 * a) - (num18 * c)) * num17) - ay;
            num18 = num11 - bone.worldX;
            num19 = num12 - bone.worldY;
            float num22 = (((num18 * d) - (num19 * b)) * num17) - ax;
            float num23 = (((num19 * a) - (num18 * c)) * num17) - ay;
            float num24 = (float) Math.Sqrt((double) ((num22 * num22) + (num23 * num23)));
            float num25 = child.data.length * num5;
            if (flag)
            {
                num25 *= ascaleX;
                float num28 = ((((num20 * num20) + (num21 * num21)) - (num24 * num24)) - (num25 * num25)) / ((2f * num24) * num25);
                if (num28 < -1f)
                {
                    num28 = -1f;
                }
                else if (num28 > 1f)
                {
                    num28 = 1f;
                }
                num27 = ((float) Math.Acos((double) num28)) * bendDir;
                a = num24 + (num25 * num28);
                b = num25 * ((float) Math.Sin((double) num27));
                num26 = (float) Math.Atan2((double) ((num21 * a) - (num20 * b)), (double) ((num20 * a) + (num21 * b)));
            }
            else
            {
                a = ascaleX * num25;
                b = ascaleY * num25;
                float num29 = a * a;
                float num30 = b * b;
                float num31 = (num20 * num20) + (num21 * num21);
                float num32 = (float) Math.Atan2((double) num21, (double) num20);
                c = (((num30 * num24) * num24) + (num29 * num31)) - (num29 * num30);
                float num33 = (-2f * num30) * num24;
                float num34 = num30 - num29;
                d = (num33 * num33) - ((4f * num34) * c);
                if (d >= 0f)
                {
                    float num35 = (float) Math.Sqrt((double) d);
                    if (num33 < 0f)
                    {
                        num35 = -num35;
                    }
                    num35 = -(num33 + num35) / 2f;
                    float num36 = num35 / num34;
                    float num37 = c / num35;
                    float num38 = (Math.Abs(num36) >= Math.Abs(num37)) ? num37 : num36;
                    if ((num38 * num38) <= num31)
                    {
                        num19 = ((float) Math.Sqrt((double) (num31 - (num38 * num38)))) * bendDir;
                        num26 = num32 - ((float) Math.Atan2((double) num19, (double) num38));
                        num27 = (float) Math.Atan2((double) (num19 / ascaleY), (double) ((num38 - num24) / ascaleX));
                        goto Label_0504;
                    }
                }
                float num39 = 3.141593f;
                float num40 = num24 - a;
                float num41 = num40 * num40;
                float num42 = 0f;
                float num43 = 0f;
                float num44 = num24 + a;
                float num45 = num44 * num44;
                float num46 = 0f;
                c = (-a * num24) / (num29 - num30);
                if ((c >= -1f) && (c <= 1f))
                {
                    c = (float) Math.Acos((double) c);
                    num18 = (a * ((float) Math.Cos((double) c))) + num24;
                    num19 = b * ((float) Math.Sin((double) c));
                    d = (num18 * num18) + (num19 * num19);
                    if (d < num41)
                    {
                        num39 = c;
                        num41 = d;
                        num40 = num18;
                        num42 = num19;
                    }
                    if (d > num45)
                    {
                        num43 = c;
                        num45 = d;
                        num44 = num18;
                        num46 = num19;
                    }
                }
                if (num31 <= ((num41 + num45) / 2f))
                {
                    num26 = num32 - ((float) Math.Atan2((double) (num42 * bendDir), (double) num40));
                    num27 = num39 * bendDir;
                }
                else
                {
                    num26 = num32 - ((float) Math.Atan2((double) (num46 * bendDir), (double) num44));
                    num27 = num43 * bendDir;
                }
            }
        Label_0504:
            num47 = ((float) Math.Atan2((double) num10, (double) x)) * num8;
            float arotation = parent.arotation;
            num26 = (((num26 - num47) * 57.29578f) + num6) - arotation;
            if (num26 > 180f)
            {
                num26 -= 360f;
            }
            else if (num26 < -180f)
            {
                num26 += 360f;
            }
            parent.UpdateWorldTransform(ax, ay, arotation + (num26 * alpha), parent.scaleX, parent.ascaleY, 0f, 0f);
            arotation = child.arotation;
            num27 = (((((num27 + num47) * 57.29578f) - child.ashearX) * num8) + num7) - arotation;
            if (num27 > 180f)
            {
                num27 -= 360f;
            }
            else if (num27 < -180f)
            {
                num27 += 360f;
            }
            child.UpdateWorldTransform(x, num10, arotation + (num27 * alpha), child.ascaleX, child.ascaleY, child.ashearX, child.ashearY);
        }

        public override string ToString() => 
            this.data.name;

        public void Update()
        {
            Bone target = this.target;
            ExposedList<Bone> bones = this.bones;
            switch (bones.Count)
            {
                case 1:
                    Apply(bones.Items[0], target.worldX, target.worldY, this.mix);
                    break;

                case 2:
                    Apply(bones.Items[0], bones.Items[1], target.worldX, target.worldY, this.bendDirection, this.mix);
                    break;
            }
        }

        public IkConstraintData Data =>
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

        public int BendDirection
        {
            get => 
                this.bendDirection;
            set => 
                (this.bendDirection = value);
        }

        public float Mix
        {
            get => 
                this.mix;
            set => 
                (this.mix = value);
        }
    }
}

