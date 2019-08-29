namespace Spine
{
    using System;

    public class DeformTimeline : CurveTimeline
    {
        internal int slotIndex;
        internal float[] frames;
        internal float[][] frameVertices;
        internal VertexAttachment attachment;

        public DeformTimeline(int frameCount) : base(frameCount)
        {
            this.frames = new float[frameCount];
            this.frameVertices = new float[frameCount][];
        }

        public override void Apply(Skeleton skeleton, float lastTime, float time, ExposedList<Event> firedEvents, float alpha, MixPose pose, MixDirection direction)
        {
            Slot slot = skeleton.slots.Items[this.slotIndex];
            VertexAttachment attachment = slot.attachment as VertexAttachment;
            if ((attachment != null) && attachment.ApplyDeform(this.attachment))
            {
                float[] items;
                ExposedList<float> attachmentVertices = slot.attachmentVertices;
                if (attachmentVertices.Count == 0)
                {
                    alpha = 1f;
                }
                float[][] frameVertices = this.frameVertices;
                int length = frameVertices[0].Length;
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
                        attachmentVertices.Clear(true);
                        return;
                    }
                    if (alpha == 1f)
                    {
                        attachmentVertices.Clear(true);
                    }
                    else
                    {
                        if (attachmentVertices.Capacity < length)
                        {
                            attachmentVertices.Capacity = length;
                        }
                        attachmentVertices.Count = length;
                        items = attachmentVertices.Items;
                        if (attachment.bones == null)
                        {
                            float[] vertices = attachment.vertices;
                            for (int i = 0; i < length; i++)
                            {
                                items[i] += (vertices[i] - items[i]) * alpha;
                            }
                        }
                        else
                        {
                            alpha = 1f - alpha;
                            for (int i = 0; i < length; i++)
                            {
                                items[i] *= alpha;
                            }
                        }
                    }
                }
                else
                {
                    if (attachmentVertices.Capacity < length)
                    {
                        attachmentVertices.Capacity = length;
                    }
                    attachmentVertices.Count = length;
                    items = attachmentVertices.Items;
                    if (time >= frames[frames.Length - 1])
                    {
                        float[] sourceArray = frameVertices[frames.Length - 1];
                        if (alpha == 1f)
                        {
                            Array.Copy(sourceArray, 0, items, 0, length);
                        }
                        else if (pose == MixPose.Setup)
                        {
                            if (attachment.bones == null)
                            {
                                float[] vertices = attachment.vertices;
                                for (int i = 0; i < length; i++)
                                {
                                    float num5 = vertices[i];
                                    items[i] = num5 + ((sourceArray[i] - num5) * alpha);
                                }
                            }
                            else
                            {
                                for (int i = 0; i < length; i++)
                                {
                                    items[i] = sourceArray[i] * alpha;
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < length; i++)
                            {
                                items[i] += (sourceArray[i] - items[i]) * alpha;
                            }
                        }
                    }
                    else
                    {
                        int index = Animation.BinarySearch(frames, time);
                        float[] numArray7 = frameVertices[index - 1];
                        float[] numArray8 = frameVertices[index];
                        float num9 = frames[index];
                        float curvePercent = base.GetCurvePercent(index - 1, 1f - ((time - num9) / (frames[index - 1] - num9)));
                        if (alpha == 1f)
                        {
                            for (int i = 0; i < length; i++)
                            {
                                float num12 = numArray7[i];
                                items[i] = num12 + ((numArray8[i] - num12) * curvePercent);
                            }
                        }
                        else if (pose == MixPose.Setup)
                        {
                            if (attachment.bones == null)
                            {
                                float[] vertices = attachment.vertices;
                                for (int i = 0; i < length; i++)
                                {
                                    float num14 = numArray7[i];
                                    float num15 = vertices[i];
                                    items[i] = num15 + (((num14 + ((numArray8[i] - num14) * curvePercent)) - num15) * alpha);
                                }
                            }
                            else
                            {
                                for (int i = 0; i < length; i++)
                                {
                                    float num17 = numArray7[i];
                                    items[i] = (num17 + ((numArray8[i] - num17) * curvePercent)) * alpha;
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < length; i++)
                            {
                                float num19 = numArray7[i];
                                items[i] += ((num19 + ((numArray8[i] - num19) * curvePercent)) - items[i]) * alpha;
                            }
                        }
                    }
                }
            }
        }

        public void SetFrame(int frameIndex, float time, float[] vertices)
        {
            this.frames[frameIndex] = time;
            this.frameVertices[frameIndex] = vertices;
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

        public float[][] Vertices
        {
            get => 
                this.frameVertices;
            set => 
                (this.frameVertices = value);
        }

        public VertexAttachment Attachment
        {
            get => 
                this.attachment;
            set => 
                (this.attachment = value);
        }

        public override int PropertyId =>
            ((0x6000000 + this.attachment.id) + this.slotIndex);
    }
}

