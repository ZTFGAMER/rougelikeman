namespace Spine
{
    using System;

    public class IkConstraintTimeline : CurveTimeline
    {
        public const int ENTRIES = 3;
        private const int PREV_TIME = -3;
        private const int PREV_MIX = -2;
        private const int PREV_BEND_DIRECTION = -1;
        private const int MIX = 1;
        private const int BEND_DIRECTION = 2;
        internal int ikConstraintIndex;
        internal float[] frames;

        public IkConstraintTimeline(int frameCount) : base(frameCount)
        {
            this.frames = new float[frameCount * 3];
        }

        public override void Apply(Skeleton skeleton, float lastTime, float time, ExposedList<Event> firedEvents, float alpha, MixPose pose, MixDirection direction)
        {
            IkConstraint constraint = skeleton.ikConstraints.Items[this.ikConstraintIndex];
            float[] frames = this.frames;
            if (time < frames[0])
            {
                if (pose != MixPose.Setup)
                {
                    if (pose != MixPose.Current)
                    {
                        return;
                    }
                }
                else
                {
                    constraint.mix = constraint.data.mix;
                    constraint.bendDirection = constraint.data.bendDirection;
                    return;
                }
                constraint.mix += (constraint.data.mix - constraint.mix) * alpha;
                constraint.bendDirection = constraint.data.bendDirection;
            }
            else if (time >= frames[frames.Length - 3])
            {
                if (pose == MixPose.Setup)
                {
                    constraint.mix = constraint.data.mix + ((frames[frames.Length + -2] - constraint.data.mix) * alpha);
                    constraint.bendDirection = (direction != MixDirection.Out) ? ((int) frames[frames.Length + -1]) : constraint.data.bendDirection;
                }
                else
                {
                    constraint.mix += (frames[frames.Length + -2] - constraint.mix) * alpha;
                    if (direction == MixDirection.In)
                    {
                        constraint.bendDirection = (int) frames[frames.Length + -1];
                    }
                }
            }
            else
            {
                int index = Animation.BinarySearch(frames, time, 3);
                float num2 = frames[index + -2];
                float num3 = frames[index];
                float curvePercent = base.GetCurvePercent((index / 3) - 1, 1f - ((time - num3) / (frames[index + -3] - num3)));
                if (pose == MixPose.Setup)
                {
                    constraint.mix = constraint.data.mix + (((num2 + ((frames[index + 1] - num2) * curvePercent)) - constraint.data.mix) * alpha);
                    constraint.bendDirection = (direction != MixDirection.Out) ? ((int) frames[index + -1]) : constraint.data.bendDirection;
                }
                else
                {
                    constraint.mix += ((num2 + ((frames[index + 1] - num2) * curvePercent)) - constraint.mix) * alpha;
                    if (direction == MixDirection.In)
                    {
                        constraint.bendDirection = (int) frames[index + -1];
                    }
                }
            }
        }

        public void SetFrame(int frameIndex, float time, float mix, int bendDirection)
        {
            frameIndex *= 3;
            this.frames[frameIndex] = time;
            this.frames[frameIndex + 1] = mix;
            this.frames[frameIndex + 2] = bendDirection;
        }

        public int IkConstraintIndex
        {
            get => 
                this.ikConstraintIndex;
            set => 
                (this.ikConstraintIndex = value);
        }

        public float[] Frames
        {
            get => 
                this.frames;
            set => 
                (this.frames = value);
        }

        public override int PropertyId =>
            (0x9000000 + this.ikConstraintIndex);
    }
}

