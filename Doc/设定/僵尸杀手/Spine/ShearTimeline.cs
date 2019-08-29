namespace Spine
{
    using System;

    public class ShearTimeline : TranslateTimeline
    {
        public ShearTimeline(int frameCount) : base(frameCount)
        {
        }

        public override void Apply(Skeleton skeleton, float lastTime, float time, ExposedList<Event> firedEvents, float alpha, MixPose pose, MixDirection direction)
        {
            Bone bone = skeleton.bones.Items[base.boneIndex];
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
                    bone.shearX = bone.data.shearX;
                    bone.shearY = bone.data.shearY;
                    return;
                }
                bone.shearX += (bone.data.shearX - bone.shearX) * alpha;
                bone.shearY += (bone.data.shearY - bone.shearY) * alpha;
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
                    bone.shearX = bone.data.shearX + (num * alpha);
                    bone.shearY = bone.data.shearY + (num2 * alpha);
                }
                else
                {
                    bone.shearX += ((bone.data.shearX + num) - bone.shearX) * alpha;
                    bone.shearY += ((bone.data.shearY + num2) - bone.shearY) * alpha;
                }
            }
        }

        public override int PropertyId =>
            (0x3000000 + base.boneIndex);
    }
}

