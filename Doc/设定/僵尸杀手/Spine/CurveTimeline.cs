namespace Spine
{
    using System;

    public abstract class CurveTimeline : Timeline
    {
        protected const float LINEAR = 0f;
        protected const float STEPPED = 1f;
        protected const float BEZIER = 2f;
        protected const int BEZIER_SIZE = 0x13;
        internal float[] curves;

        public CurveTimeline(int frameCount)
        {
            if (frameCount <= 0)
            {
                throw new ArgumentException("frameCount must be > 0: " + frameCount, "frameCount");
            }
            this.curves = new float[(frameCount - 1) * 0x13];
        }

        public abstract void Apply(Skeleton skeleton, float lastTime, float time, ExposedList<Event> firedEvents, float alpha, MixPose pose, MixDirection direction);
        public float GetCurvePercent(int frameIndex, float percent)
        {
            percent = MathUtils.Clamp(percent, 0f, 1f);
            float[] curves = this.curves;
            int index = frameIndex * 0x13;
            switch (curves[index])
            {
                case 0f:
                    return percent;

                case 1f:
                    return 0f;
            }
            index++;
            float num3 = 0f;
            int num4 = index;
            int num5 = (index + 0x13) - 1;
            while (index < num5)
            {
                num3 = curves[index];
                if (num3 >= percent)
                {
                    float num6;
                    float num7;
                    if (index == num4)
                    {
                        num6 = 0f;
                        num7 = 0f;
                    }
                    else
                    {
                        num6 = curves[index - 2];
                        num7 = curves[index - 1];
                    }
                    return (num7 + (((curves[index + 1] - num7) * (percent - num6)) / (num3 - num6)));
                }
                index += 2;
            }
            float num8 = curves[index - 1];
            return (num8 + (((1f - num8) * (percent - num3)) / (1f - num3)));
        }

        public float GetCurveType(int frameIndex) => 
            this.curves[frameIndex * 0x13];

        public void SetCurve(int frameIndex, float cx1, float cy1, float cx2, float cy2)
        {
            float num = ((-cx1 * 2f) + cx2) * 0.03f;
            float num2 = ((-cy1 * 2f) + cy2) * 0.03f;
            float num3 = (((cx1 - cx2) * 3f) + 1f) * 0.006f;
            float num4 = (((cy1 - cy2) * 3f) + 1f) * 0.006f;
            float num5 = (num * 2f) + num3;
            float num6 = (num2 * 2f) + num4;
            float num7 = ((cx1 * 0.3f) + num) + (num3 * 0.1666667f);
            float num8 = ((cy1 * 0.3f) + num2) + (num4 * 0.1666667f);
            int index = frameIndex * 0x13;
            float[] curves = this.curves;
            curves[index++] = 2f;
            float num10 = num7;
            float num11 = num8;
            int num12 = (index + 0x13) - 1;
            while (index < num12)
            {
                curves[index] = num10;
                curves[index + 1] = num11;
                num7 += num5;
                num8 += num6;
                num5 += num3;
                num6 += num4;
                num10 += num7;
                num11 += num8;
                index += 2;
            }
        }

        public void SetLinear(int frameIndex)
        {
            this.curves[frameIndex * 0x13] = 0f;
        }

        public void SetStepped(int frameIndex)
        {
            this.curves[frameIndex * 0x13] = 1f;
        }

        public int FrameCount =>
            ((this.curves.Length / 0x13) + 1);

        public abstract int PropertyId { get; }
    }
}

