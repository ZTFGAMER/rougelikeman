namespace Spine
{
    using System;

    public class TwoColorTimeline : CurveTimeline
    {
        public const int ENTRIES = 8;
        protected const int PREV_TIME = -8;
        protected const int PREV_R = -7;
        protected const int PREV_G = -6;
        protected const int PREV_B = -5;
        protected const int PREV_A = -4;
        protected const int PREV_R2 = -3;
        protected const int PREV_G2 = -2;
        protected const int PREV_B2 = -1;
        protected const int R = 1;
        protected const int G = 2;
        protected const int B = 3;
        protected const int A = 4;
        protected const int R2 = 5;
        protected const int G2 = 6;
        protected const int B2 = 7;
        internal int slotIndex;
        internal float[] frames;

        public TwoColorTimeline(int frameCount) : base(frameCount)
        {
            this.frames = new float[frameCount * 8];
        }

        public override void Apply(Skeleton skeleton, float lastTime, float time, ExposedList<Event> firedEvents, float alpha, MixPose pose, MixDirection direction)
        {
            Slot slot = skeleton.slots.Items[this.slotIndex];
            float[] frames = this.frames;
            if (time < frames[0])
            {
                SlotData data = slot.data;
                if (pose != MixPose.Setup)
                {
                    if (pose != MixPose.Current)
                    {
                        return;
                    }
                }
                else
                {
                    slot.r = data.r;
                    slot.g = data.g;
                    slot.b = data.b;
                    slot.a = data.a;
                    slot.r2 = data.r2;
                    slot.g2 = data.g2;
                    slot.b2 = data.b2;
                    return;
                }
                slot.r += (slot.r - data.r) * alpha;
                slot.g += (slot.g - data.g) * alpha;
                slot.b += (slot.b - data.b) * alpha;
                slot.a += (slot.a - data.a) * alpha;
                slot.r2 += (slot.r2 - data.r2) * alpha;
                slot.g2 += (slot.g2 - data.g2) * alpha;
                slot.b2 += (slot.b2 - data.b2) * alpha;
            }
            else
            {
                float num;
                float num2;
                float num3;
                float num4;
                float num5;
                float num6;
                float num7;
                if (time >= frames[frames.Length - 8])
                {
                    int length = frames.Length;
                    num = frames[length + -7];
                    num2 = frames[length + -6];
                    num3 = frames[length + -5];
                    num4 = frames[length + -4];
                    num5 = frames[length + -3];
                    num6 = frames[length + -2];
                    num7 = frames[length + -1];
                }
                else
                {
                    int index = Animation.BinarySearch(frames, time, 8);
                    num = frames[index + -7];
                    num2 = frames[index + -6];
                    num3 = frames[index + -5];
                    num4 = frames[index + -4];
                    num5 = frames[index + -3];
                    num6 = frames[index + -2];
                    num7 = frames[index + -1];
                    float num10 = frames[index];
                    float curvePercent = base.GetCurvePercent((index / 8) - 1, 1f - ((time - num10) / (frames[index + -8] - num10)));
                    num += (frames[index + 1] - num) * curvePercent;
                    num2 += (frames[index + 2] - num2) * curvePercent;
                    num3 += (frames[index + 3] - num3) * curvePercent;
                    num4 += (frames[index + 4] - num4) * curvePercent;
                    num5 += (frames[index + 5] - num5) * curvePercent;
                    num6 += (frames[index + 6] - num6) * curvePercent;
                    num7 += (frames[index + 7] - num7) * curvePercent;
                }
                if (alpha == 1f)
                {
                    slot.r = num;
                    slot.g = num2;
                    slot.b = num3;
                    slot.a = num4;
                    slot.r2 = num5;
                    slot.g2 = num6;
                    slot.b2 = num7;
                }
                else
                {
                    float r;
                    float g;
                    float b;
                    float a;
                    float num16;
                    float num17;
                    float num18;
                    if (pose == MixPose.Setup)
                    {
                        r = slot.data.r;
                        g = slot.data.g;
                        b = slot.data.b;
                        a = slot.data.a;
                        num16 = slot.data.r2;
                        num17 = slot.data.g2;
                        num18 = slot.data.b2;
                    }
                    else
                    {
                        r = slot.r;
                        g = slot.g;
                        b = slot.b;
                        a = slot.a;
                        num16 = slot.r2;
                        num17 = slot.g2;
                        num18 = slot.b2;
                    }
                    slot.r = r + ((num - r) * alpha);
                    slot.g = g + ((num2 - g) * alpha);
                    slot.b = b + ((num3 - b) * alpha);
                    slot.a = a + ((num4 - a) * alpha);
                    slot.r2 = num16 + ((num5 - num16) * alpha);
                    slot.g2 = num17 + ((num6 - num17) * alpha);
                    slot.b2 = num18 + ((num7 - num18) * alpha);
                }
            }
        }

        public void SetFrame(int frameIndex, float time, float r, float g, float b, float a, float r2, float g2, float b2)
        {
            frameIndex *= 8;
            this.frames[frameIndex] = time;
            this.frames[frameIndex + 1] = r;
            this.frames[frameIndex + 2] = g;
            this.frames[frameIndex + 3] = b;
            this.frames[frameIndex + 4] = a;
            this.frames[frameIndex + 5] = r2;
            this.frames[frameIndex + 6] = g2;
            this.frames[frameIndex + 7] = b2;
        }

        public int SlotIndex
        {
            get => 
                this.slotIndex;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("index must be >= 0.");
                }
                this.slotIndex = value;
            }
        }

        public float[] Frames =>
            this.frames;

        public override int PropertyId =>
            (0xe000000 + this.slotIndex);
    }
}

