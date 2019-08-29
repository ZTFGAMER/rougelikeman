namespace Spine
{
    using System;

    public class DrawOrderTimeline : Timeline
    {
        internal float[] frames;
        private int[][] drawOrders;

        public DrawOrderTimeline(int frameCount)
        {
            this.frames = new float[frameCount];
            this.drawOrders = new int[frameCount][];
        }

        public void Apply(Skeleton skeleton, float lastTime, float time, ExposedList<Event> firedEvents, float alpha, MixPose pose, MixDirection direction)
        {
            ExposedList<Slot> drawOrder = skeleton.drawOrder;
            ExposedList<Slot> slots = skeleton.slots;
            if ((direction == MixDirection.Out) && (pose == MixPose.Setup))
            {
                Array.Copy(slots.Items, 0, drawOrder.Items, 0, slots.Count);
            }
            else
            {
                float[] frames = this.frames;
                if (time < frames[0])
                {
                    if (pose == MixPose.Setup)
                    {
                        Array.Copy(slots.Items, 0, drawOrder.Items, 0, slots.Count);
                    }
                }
                else
                {
                    int num;
                    if (time >= frames[frames.Length - 1])
                    {
                        num = frames.Length - 1;
                    }
                    else
                    {
                        num = Animation.BinarySearch(frames, time) - 1;
                    }
                    int[] numArray2 = this.drawOrders[num];
                    if (numArray2 == null)
                    {
                        drawOrder.Clear(true);
                        int index = 0;
                        int count = slots.Count;
                        while (index < count)
                        {
                            drawOrder.Add(slots.Items[index]);
                            index++;
                        }
                    }
                    else
                    {
                        Slot[] items = drawOrder.Items;
                        Slot[] slotArray2 = slots.Items;
                        int index = 0;
                        int length = numArray2.Length;
                        while (index < length)
                        {
                            items[index] = slotArray2[numArray2[index]];
                            index++;
                        }
                    }
                }
            }
        }

        public void SetFrame(int frameIndex, float time, int[] drawOrder)
        {
            this.frames[frameIndex] = time;
            this.drawOrders[frameIndex] = drawOrder;
        }

        public float[] Frames
        {
            get => 
                this.frames;
            set => 
                (this.frames = value);
        }

        public int[][] DrawOrders
        {
            get => 
                this.drawOrders;
            set => 
                (this.drawOrders = value);
        }

        public int FrameCount =>
            this.frames.Length;

        public int PropertyId =>
            0x8000000;
    }
}

