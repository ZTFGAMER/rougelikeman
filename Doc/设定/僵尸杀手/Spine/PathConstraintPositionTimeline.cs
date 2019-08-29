namespace Spine
{
    using System;

    public class PathConstraintPositionTimeline : CurveTimeline
    {
        public const int ENTRIES = 2;
        protected const int PREV_TIME = -2;
        protected const int PREV_VALUE = -1;
        protected const int VALUE = 1;
        internal int pathConstraintIndex;
        internal float[] frames;

        public PathConstraintPositionTimeline(int frameCount) : base(frameCount)
        {
            this.frames = new float[frameCount * 2];
        }

        public override void Apply(Skeleton skeleton, float lastTime, float time, ExposedList<Event> firedEvents, float alpha, MixPose pose, MixDirection direction)
        {
            PathConstraint constraint = skeleton.pathConstraints.Items[this.pathConstraintIndex];
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
                    constraint.position = constraint.data.position;
                    return;
                }
                constraint.position += (constraint.data.position - constraint.position) * alpha;
            }
            else
            {
                float num;
                if (time >= frames[frames.Length - 2])
                {
                    num = frames[frames.Length + -1];
                }
                else
                {
                    int index = Animation.BinarySearch(frames, time, 2);
                    num = frames[index + -1];
                    float num3 = frames[index];
                    float curvePercent = base.GetCurvePercent((index / 2) - 1, 1f - ((time - num3) / (frames[index + -2] - num3)));
                    num += (frames[index + 1] - num) * curvePercent;
                }
                if (pose == MixPose.Setup)
                {
                    constraint.position = constraint.data.position + ((num - constraint.data.position) * alpha);
                }
                else
                {
                    constraint.position += (num - constraint.position) * alpha;
                }
            }
        }

        public void SetFrame(int frameIndex, float time, float value)
        {
            frameIndex *= 2;
            this.frames[frameIndex] = time;
            this.frames[frameIndex + 1] = value;
        }

        public override int PropertyId =>
            (0xb000000 + this.pathConstraintIndex);

        public int PathConstraintIndex
        {
            get => 
                this.pathConstraintIndex;
            set => 
                (this.pathConstraintIndex = value);
        }

        public float[] Frames
        {
            get => 
                this.frames;
            set => 
                (this.frames = value);
        }
    }
}

