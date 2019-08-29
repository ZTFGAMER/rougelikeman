namespace Spine
{
    using System;

    public class ScaleTimeline : TranslateTimeline
    {
        public ScaleTimeline(int frameCount) : base(frameCount)
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
                    bone.scaleX = bone.data.scaleX;
                    bone.scaleY = bone.data.scaleY;
                    return;
                }
                bone.scaleX += (bone.data.scaleX - bone.scaleX) * alpha;
                bone.scaleY += (bone.data.scaleY - bone.scaleY) * alpha;
            }
            else
            {
                float num;
                float num2;
                if (time >= frames[frames.Length - 3])
                {
                    num = frames[frames.Length + -2] * bone.data.scaleX;
                    num2 = frames[frames.Length + -1] * bone.data.scaleY;
                }
                else
                {
                    int index = Animation.BinarySearch(frames, time, 3);
                    num = frames[index + -2];
                    num2 = frames[index + -1];
                    float num4 = frames[index];
                    float curvePercent = base.GetCurvePercent((index / 3) - 1, 1f - ((time - num4) / (frames[index + -3] - num4)));
                    num = (num + ((frames[index + 1] - num) * curvePercent)) * bone.data.scaleX;
                    num2 = (num2 + ((frames[index + 2] - num2) * curvePercent)) * bone.data.scaleY;
                }
                if (alpha == 1f)
                {
                    bone.scaleX = num;
                    bone.scaleY = num2;
                }
                else
                {
                    float scaleX;
                    float scaleY;
                    if (pose == MixPose.Setup)
                    {
                        scaleX = bone.data.scaleX;
                        scaleY = bone.data.scaleY;
                    }
                    else
                    {
                        scaleX = bone.scaleX;
                        scaleY = bone.scaleY;
                    }
                    if (direction == MixDirection.Out)
                    {
                        num = ((num < 0f) ? -num : num) * ((scaleX < 0f) ? ((float) (-1)) : ((float) 1));
                        num2 = ((num2 < 0f) ? -num2 : num2) * ((scaleY < 0f) ? ((float) (-1)) : ((float) 1));
                    }
                    else
                    {
                        scaleX = ((scaleX < 0f) ? -scaleX : scaleX) * ((num < 0f) ? ((float) (-1)) : ((float) 1));
                        scaleY = ((scaleY < 0f) ? -scaleY : scaleY) * ((num2 < 0f) ? ((float) (-1)) : ((float) 1));
                    }
                    bone.scaleX = scaleX + ((num - scaleX) * alpha);
                    bone.scaleY = scaleY + ((num2 - scaleY) * alpha);
                }
            }
        }

        public override int PropertyId =>
            (0x2000000 + base.boneIndex);
    }
}

