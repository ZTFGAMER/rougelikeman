namespace Spine
{
    using System;

    public class TranslateTimeline : CurveTimeline
    {
        public const int ENTRIES = 3;
        protected const int PREV_TIME = -3;
        protected const int PREV_X = -2;
        protected const int PREV_Y = -1;
        protected const int X = 1;
        protected const int Y = 2;
        internal int boneIndex;
        internal float[] frames;

        public TranslateTimeline(int frameCount) : base(frameCount)
        {
            this.frames = new float[frameCount * 3];
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
                    bone.x = bone.data.x;
                    bone.y = bone.data.y;
                    return;
                }
                bone.x += (bone.data.x - bone.x) * alpha;
                bone.y += (bone.data.y - bone.y) * alpha;
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
                    bone.x = bone.data.x + (num * alpha);
                    bone.y = bone.data.y + (num2 * alpha);
                }
                else
                {
                    bone.x += ((bone.data.x + num) - bone.x) * alpha;
                    bone.y += ((bone.data.y + num2) - bone.y) * alpha;
                }
            }
        }

        public void SetFrame(int frameIndex, float time, float x, float y)
        {
            frameIndex *= 3;
            this.frames[frameIndex] = time;
            this.frames[frameIndex + 1] = x;
            this.frames[frameIndex + 2] = y;
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
            (0x1000000 + this.boneIndex);
    }
}

