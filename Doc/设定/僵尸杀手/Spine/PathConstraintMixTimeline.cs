namespace Spine
{
    using System;

    public class PathConstraintMixTimeline : CurveTimeline
    {
        public const int ENTRIES = 3;
        private const int PREV_TIME = -3;
        private const int PREV_ROTATE = -2;
        private const int PREV_TRANSLATE = -1;
        private const int ROTATE = 1;
        private const int TRANSLATE = 2;
        internal int pathConstraintIndex;
        internal float[] frames;

        public PathConstraintMixTimeline(int frameCount) : base(frameCount)
        {
            this.frames = new float[frameCount * 3];
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
                    constraint.rotateMix = constraint.data.rotateMix;
                    constraint.translateMix = constraint.data.translateMix;
                    return;
                }
                constraint.rotateMix += (constraint.data.rotateMix - constraint.rotateMix) * alpha;
                constraint.translateMix += (constraint.data.translateMix - constraint.translateMix) * alpha;
            }
            else
            {
                float num;
                float num2;
                if (time >= frames[frames.Length - 3])
                {
                    num = frames[frames.Length + -2];
                    num2 = frames[frames.Length + -1];
                }
                else
                {
                    int index = Animation.BinarySearch(frames, time, 3);
                    num = frames[index + -2];
                    num2 = frames[index + -1];
                    float num4 = frames[index];
                    float curvePercent = base.GetCurvePercent((index / 3) - 1, 1f - ((time - num4) / (frames[index + -3] - num4)));
                    num += (frames[index + 1] - num) * curvePercent;
                    num2 += (frames[index + 2] - num2) * curvePercent;
                }
                if (pose == MixPose.Setup)
                {
                    constraint.rotateMix = constraint.data.rotateMix + ((num - constraint.data.rotateMix) * alpha);
                    constraint.translateMix = constraint.data.translateMix + ((num2 - constraint.data.translateMix) * alpha);
                }
                else
                {
                    constraint.rotateMix += (num - constraint.rotateMix) * alpha;
                    constraint.translateMix += (num2 - constraint.translateMix) * alpha;
                }
            }
        }

        public void SetFrame(int frameIndex, float time, float rotateMix, float translateMix)
        {
            frameIndex *= 3;
            this.frames[frameIndex] = time;
            this.frames[frameIndex + 1] = rotateMix;
            this.frames[frameIndex + 2] = translateMix;
        }

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

        public override int PropertyId =>
            (0xd000000 + this.pathConstraintIndex);
    }
}

