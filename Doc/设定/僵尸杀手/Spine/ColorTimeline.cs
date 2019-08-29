namespace Spine
{
    using System;

    public class ColorTimeline : CurveTimeline
    {
        public const int ENTRIES = 5;
        protected const int PREV_TIME = -5;
        protected const int PREV_R = -4;
        protected const int PREV_G = -3;
        protected const int PREV_B = -2;
        protected const int PREV_A = -1;
        protected const int R = 1;
        protected const int G = 2;
        protected const int B = 3;
        protected const int A = 4;
        internal int slotIndex;
        internal float[] frames;

        public ColorTimeline(int frameCount) : base(frameCount)
        {
            this.frames = new float[frameCount * 5];
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
                    return;
                }
                slot.r += (slot.r - data.r) * alpha;
                slot.g += (slot.g - data.g) * alpha;
                slot.b += (slot.b - data.b) * alpha;
                slot.a += (slot.a - data.a) * alpha;
            }
            else
            {
                float num;
                float num2;
                float num3;
                float num4;
                if (time >= frames[frames.Length - 5])
                {
                    int length = frames.Length;
                    num = frames[length + -4];
                    num2 = frames[length + -3];
                    num3 = frames[length + -2];
                    num4 = frames[length + -1];
                }
                else
                {
                    int index = Animation.BinarySearch(frames, time, 5);
                    num = frames[index + -4];
                    num2 = frames[index + -3];
                    num3 = frames[index + -2];
                    num4 = frames[index + -1];
                    float num7 = frames[index];
                    float curvePercent = base.GetCurvePercent((index / 5) - 1, 1f - ((time - num7) / (frames[index + -5] - num7)));
                    num += (frames[index + 1] - num) * curvePercent;
                    num2 += (frames[index + 2] - num2) * curvePercent;
                    num3 += (frames[index + 3] - num3) * curvePercent;
                    num4 += (frames[index + 4] - num4) * curvePercent;
                }
                if (alpha == 1f)
                {
                    slot.r = num;
                    slot.g = num2;
                    slot.b = num3;
                    slot.a = num4;
                }
                else
                {
                    float r;
                    float g;
                    float b;
                    float a;
                    if (pose == MixPose.Setup)
                    {
                        r = slot.data.r;
                        g = slot.data.g;
                        b = slot.data.b;
                        a = slot.data.a;
                    }
                    else
                    {
                        r = slot.r;
                        g = slot.g;
                        b = slot.b;
                        a = slot.a;
                    }
                    slot.r = r + ((num - r) * alpha);
                    slot.g = g + ((num2 - g) * alpha);
                    slot.b = b + ((num3 - b) * alpha);
                    slot.a = a + ((num4 - a) * alpha);
                }
            }
        }

        public void SetFrame(int frameIndex, float time, float r, float g, float b, float a)
        {
            frameIndex *= 5;
            this.frames[frameIndex] = time;
            this.frames[frameIndex + 1] = r;
            this.frames[frameIndex + 2] = g;
            this.frames[frameIndex + 3] = b;
            this.frames[frameIndex + 4] = a;
        }

        public int SlotIndex
        {
            get => 
                this.slotIndex;
            set => 
                (this.slotIndex = value);
        }

        public float[] Frames
        {
            get => 
                this.frames;
            set => 
                (this.frames = value);
        }

        public override int PropertyId =>
            (0x5000000 + this.slotIndex);
    }
}

