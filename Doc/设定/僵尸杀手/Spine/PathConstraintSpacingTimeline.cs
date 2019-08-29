namespace Spine
{
    using System;

    public class PathConstraintSpacingTimeline : PathConstraintPositionTimeline
    {
        public PathConstraintSpacingTimeline(int frameCount) : base(frameCount)
        {
        }

        public override void Apply(Skeleton skeleton, float lastTime, float time, ExposedList<Event> firedEvents, float alpha, MixPose pose, MixDirection direction)
        {
            PathConstraint constraint = skeleton.pathConstraints.Items[base.pathConstraintIndex];
            float[] frames = base.frames;
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
                    constraint.spacing = constraint.data.spacing;
                    return;
                }
                constraint.spacing += (constraint.data.spacing - constraint.spacing) * alpha;
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
                    constraint.spacing = constraint.data.spacing + ((num - constraint.data.spacing) * alpha);
                }
                else
                {
                    constraint.spacing += (num - constraint.spacing) * alpha;
                }
            }
        }

        public override int PropertyId =>
            (0xc000000 + base.pathConstraintIndex);
    }
}

