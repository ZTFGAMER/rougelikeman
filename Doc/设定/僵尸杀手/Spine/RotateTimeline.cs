namespace Spine
{
    using System;

    public class RotateTimeline : CurveTimeline
    {
        public const int ENTRIES = 2;
        internal const int PREV_TIME = -2;
        internal const int PREV_ROTATION = -1;
        internal const int ROTATION = 1;
        internal int boneIndex;
        internal float[] frames;

        public RotateTimeline(int frameCount) : base(frameCount)
        {
            this.frames = new float[frameCount << 1];
        }

        public override void Apply(Skeleton skeleton, float lastTime, float time, ExposedList<Event> firedEvents, float alpha, MixPose pose, MixDirection direction)
        {
            Bone bone = skeleton.bones.Items[this.boneIndex];
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
                    bone.rotation = bone.data.rotation;
                    return;
                }
                float num = bone.data.rotation - bone.rotation;
                num -= (0x4000 - ((int) (16384.499999999996 - (num / 360f)))) * 360;
                bone.rotation += num * alpha;
            }
            else if (time >= frames[frames.Length - 2])
            {
                if (pose == MixPose.Setup)
                {
                    bone.rotation = bone.data.rotation + (frames[frames.Length + -1] * alpha);
                }
                else
                {
                    float num2 = (bone.data.rotation + frames[frames.Length + -1]) - bone.rotation;
                    num2 -= (0x4000 - ((int) (16384.499999999996 - (num2 / 360f)))) * 360;
                    bone.rotation += num2 * alpha;
                }
            }
            else
            {
                int index = Animation.BinarySearch(frames, time, 2);
                float num4 = frames[index + -1];
                float num5 = frames[index];
                float curvePercent = base.GetCurvePercent((index >> 1) - 1, 1f - ((time - num5) / (frames[index + -2] - num5)));
                float num7 = frames[index + 1] - num4;
                num7 -= (0x4000 - ((int) (16384.499999999996 - (num7 / 360f)))) * 360;
                num7 = num4 + (num7 * curvePercent);
                if (pose == MixPose.Setup)
                {
                    num7 -= (0x4000 - ((int) (16384.499999999996 - (num7 / 360f)))) * 360;
                    bone.rotation = bone.data.rotation + (num7 * alpha);
                }
                else
                {
                    num7 = (bone.data.rotation + num7) - bone.rotation;
                    num7 -= (0x4000 - ((int) (16384.499999999996 - (num7 / 360f)))) * 360;
                    bone.rotation += num7 * alpha;
                }
            }
        }

        public void SetFrame(int frameIndex, float time, float degrees)
        {
            frameIndex = frameIndex << 1;
            this.frames[frameIndex] = time;
            this.frames[frameIndex + 1] = degrees;
        }

        public int BoneIndex
        {
            get => 
                this.boneIndex;
            set => 
                (this.boneIndex = value);
        }

        public float[] Frames
        {
            get => 
                this.frames;
            set => 
                (this.frames = value);
        }

        public override int PropertyId =>
            this.boneIndex;
    }
}

