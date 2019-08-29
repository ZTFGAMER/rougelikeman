namespace Spine
{
    using System;

    public class Animation
    {
        internal ExposedList<Timeline> timelines;
        internal float duration;
        internal string name;

        public Animation(string name, ExposedList<Timeline> timelines, float duration)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name", "name cannot be null.");
            }
            if (timelines == null)
            {
                throw new ArgumentNullException("timelines", "timelines cannot be null.");
            }
            this.name = name;
            this.timelines = timelines;
            this.duration = duration;
        }

        public void Apply(Skeleton skeleton, float lastTime, float time, bool loop, ExposedList<Event> events, float alpha, MixPose pose, MixDirection direction)
        {
            if (skeleton == null)
            {
                throw new ArgumentNullException("skeleton", "skeleton cannot be null.");
            }
            if (loop && (this.duration != 0f))
            {
                time = time % this.duration;
                if (lastTime > 0f)
                {
                    lastTime = lastTime % this.duration;
                }
            }
            ExposedList<Timeline> timelines = this.timelines;
            int index = 0;
            int count = timelines.Count;
            while (index < count)
            {
                timelines.Items[index].Apply(skeleton, lastTime, time, events, alpha, pose, direction);
                index++;
            }
        }

        internal static int BinarySearch(float[] values, float target)
        {
            int num = 0;
            int num2 = values.Length - 2;
            if (num2 == 0)
            {
                return 1;
            }
            int num3 = num2 >> 1;
            while (true)
            {
                if (values[num3 + 1] <= target)
                {
                    num = num3 + 1;
                }
                else
                {
                    num2 = num3;
                }
                if (num == num2)
                {
                    return (num + 1);
                }
                num3 = (num + num2) >> 1;
            }
        }

        internal static int BinarySearch(float[] values, float target, int step)
        {
            int num = 0;
            int num2 = (values.Length / step) - 2;
            if (num2 == 0)
            {
                return step;
            }
            int num3 = num2 >> 1;
            while (true)
            {
                if (values[(num3 + 1) * step] <= target)
                {
                    num = num3 + 1;
                }
                else
                {
                    num2 = num3;
                }
                if (num == num2)
                {
                    return ((num + 1) * step);
                }
                num3 = (num + num2) >> 1;
            }
        }

        internal static int LinearSearch(float[] values, float target, int step)
        {
            int index = 0;
            int num2 = values.Length - step;
            while (index <= num2)
            {
                if (values[index] > target)
                {
                    return index;
                }
                index += step;
            }
            return -1;
        }

        public string Name =>
            this.name;

        public ExposedList<Timeline> Timelines
        {
            get => 
                this.timelines;
            set => 
                (this.timelines = value);
        }

        public float Duration
        {
            get => 
                this.duration;
            set => 
                (this.duration = value);
        }
    }
}

