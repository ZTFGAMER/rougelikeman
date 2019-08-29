namespace Spine
{
    using System;

    public class EventTimeline : Timeline
    {
        internal float[] frames;
        private Event[] events;

        public EventTimeline(int frameCount)
        {
            this.frames = new float[frameCount];
            this.events = new Event[frameCount];
        }

        public void Apply(Skeleton skeleton, float lastTime, float time, ExposedList<Event> firedEvents, float alpha, MixPose pose, MixDirection direction)
        {
            if (firedEvents != null)
            {
                float[] frames = this.frames;
                int length = frames.Length;
                if (lastTime > time)
                {
                    this.Apply(skeleton, lastTime, 2.147484E+09f, firedEvents, alpha, pose, direction);
                    lastTime = -1f;
                }
                else if (lastTime >= frames[length - 1])
                {
                    return;
                }
                if (time >= frames[0])
                {
                    int num2;
                    if (lastTime < frames[0])
                    {
                        num2 = 0;
                    }
                    else
                    {
                        num2 = Animation.BinarySearch(frames, lastTime);
                        float num3 = frames[num2];
                        while (num2 > 0)
                        {
                            if (frames[num2 - 1] != num3)
                            {
                                break;
                            }
                            num2--;
                        }
                    }
                    while ((num2 < length) && (time >= frames[num2]))
                    {
                        firedEvents.Add(this.events[num2]);
                        num2++;
                    }
                }
            }
        }

        public void SetFrame(int frameIndex, Event e)
        {
            this.frames[frameIndex] = e.Time;
            this.events[frameIndex] = e;
        }

        public float[] Frames
        {
            get => 
                this.frames;
            set => 
                (this.frames = value);
        }

        public Event[] Events
        {
            get => 
                this.events;
            set => 
                (this.events = value);
        }

        public int FrameCount =>
            this.frames.Length;

        public int PropertyId =>
            0x7000000;
    }
}

