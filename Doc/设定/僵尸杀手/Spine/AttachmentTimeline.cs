namespace Spine
{
    using System;

    public class AttachmentTimeline : Timeline
    {
        internal int slotIndex;
        internal float[] frames;
        internal string[] attachmentNames;

        public AttachmentTimeline(int frameCount)
        {
            this.frames = new float[frameCount];
            this.attachmentNames = new string[frameCount];
        }

        public void Apply(Skeleton skeleton, float lastTime, float time, ExposedList<Event> firedEvents, float alpha, MixPose pose, MixDirection direction)
        {
            string attachmentName;
            Slot slot = skeleton.slots.Items[this.slotIndex];
            if ((direction == MixDirection.Out) && (pose == MixPose.Setup))
            {
                attachmentName = slot.data.attachmentName;
                slot.Attachment = (attachmentName != null) ? skeleton.GetAttachment(this.slotIndex, attachmentName) : null;
            }
            else
            {
                float[] frames = this.frames;
                if (time < frames[0])
                {
                    if (pose == MixPose.Setup)
                    {
                        attachmentName = slot.data.attachmentName;
                        slot.Attachment = (attachmentName != null) ? skeleton.GetAttachment(this.slotIndex, attachmentName) : null;
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
                        num = Animation.BinarySearch(frames, time, 1) - 1;
                    }
                    attachmentName = this.attachmentNames[num];
                    slot.Attachment = (attachmentName != null) ? skeleton.GetAttachment(this.slotIndex, attachmentName) : null;
                }
            }
        }

        public void SetFrame(int frameIndex, float time, string attachmentName)
        {
            this.frames[frameIndex] = time;
            this.attachmentNames[frameIndex] = attachmentName;
        }

        public int SlotIndex
        {
            get => 
                this.slotIndex;
            set => 
                (this.slotIndex = value);
        }

        public float[] Frames
        {
            get => 
                this.frames;
            set => 
                (this.frames = value);
        }

        public string[] AttachmentNames
        {
            get => 
                this.attachmentNames;
            set => 
                (this.attachmentNames = value);
        }

        public int FrameCount =>
            this.frames.Length;

        public int PropertyId =>
            (0x4000000 + this.slotIndex);
    }
}

