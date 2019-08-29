namespace Spine
{
    using System;

    public class TransformConstraintTimeline : CurveTimeline
    {
        public const int ENTRIES = 5;
        private const int PREV_TIME = -5;
        private const int PREV_ROTATE = -4;
        private const int PREV_TRANSLATE = -3;
        private const int PREV_SCALE = -2;
        private const int PREV_SHEAR = -1;
        private const int ROTATE = 1;
        private const int TRANSLATE = 2;
        private const int SCALE = 3;
        private const int SHEAR = 4;
        internal int transformConstraintIndex;
        internal float[] frames;

        public TransformConstraintTimeline(int frameCount) : base(frameCount)
        {
            this.frames = new float[frameCount * 5];
        }

        public override void Apply(Skeleton skeleton, float lastTime, float time, ExposedList<Event> firedEvents, float alpha, MixPose pose, MixDirection direction)
        {
            TransformConstraint constraint = skeleton.transformConstraints.Items[this.transformConstraintIndex];
            float[] frames = this.frames;
            if (time < frames[0])
            {
                TransformConstraintData data = constraint.data;
                if (pose != MixPose.Setup)
                {
                    if (pose != MixPose.Current)
                    {
                        return;
                    }
                }
                else
                {
                    constraint.rotateMix = data.rotateMix;
                    constraint.translateMix = data.translateMix;
                    constraint.scaleMix = data.scaleMix;
                    constraint.shearMix = data.shearMix;
                    return;
                }
                constraint.rotateMix += (data.rotateMix - constraint.rotateMix) * alpha;
                constraint.translateMix += (data.translateMix - constraint.translateMix) * alpha;
                constraint.scaleMix += (data.scaleMix - constraint.scaleMix) * alpha;
                constraint.shearMix += (data.shearMix - constraint.shearMix) * alpha;
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
                if (pose == MixPose.Setup)
                {
                    TransformConstraintData data = constraint.data;
                    constraint.rotateMix = data.rotateMix + ((num - data.rotateMix) * alpha);
                    constraint.translateMix = data.translateMix + ((num2 - data.translateMix) * alpha);
                    constraint.scaleMix = data.scaleMix + ((num3 - data.scaleMix) * alpha);
                    constraint.shearMix = data.shearMix + ((num4 - data.shearMix) * alpha);
                }
                else
                {
                    constraint.rotateMix += (num - constraint.rotateMix) * alpha;
                    constraint.translateMix += (num2 - constraint.translateMix) * alpha;
                    constraint.scaleMix += (num3 - constraint.scaleMix) * alpha;
                    constraint.shearMix += (num4 - constraint.shearMix) * alpha;
                }
            }
        }

        public void SetFrame(int frameIndex, float time, float rotateMix, float translateMix, float scaleMix, float shearMix)
        {
            frameIndex *= 5;
            this.frames[frameIndex] = time;
            this.frames[frameIndex + 1] = rotateMix;
            this.frames[frameIndex + 2] = translateMix;
            this.frames[frameIndex + 3] = scaleMix;
            this.frames[frameIndex + 4] = shearMix;
        }

        public int TransformConstraintIndex
        {
            get => 
                this.transformConstraintIndex;
            set => 
                (this.transformConstraintIndex = value);
        }

        public float[] Frames
        {
            get => 
                this.frames;
            set => 
                (this.frames = value);
        }

        public override int PropertyId =>
            (0xa000000 + this.transformConstraintIndex);
    }
}

