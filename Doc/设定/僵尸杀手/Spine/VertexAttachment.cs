namespace Spine
{
    using System;
    using System.Runtime.InteropServices;

    public class VertexAttachment : Attachment
    {
        private static int nextID = 0;
        private static readonly object nextIdLock = new object();
        internal readonly int id;
        internal int[] bones;
        internal float[] vertices;
        internal int worldVerticesLength;

        public VertexAttachment(string name) : base(name)
        {
            object nextIdLock = VertexAttachment.nextIdLock;
            lock (nextIdLock)
            {
                this.id = (nextID++ & 0xffff) << 11;
            }
        }

        public virtual bool ApplyDeform(VertexAttachment sourceAttachment) => 
            (this == sourceAttachment);

        public void ComputeWorldVertices(Slot slot, float[] worldVertices)
        {
            this.ComputeWorldVertices(slot, 0, this.worldVerticesLength, worldVertices, 0, 2);
        }

        public void ComputeWorldVertices(Slot slot, int start, int count, float[] worldVertices, int offset, int stride = 2)
        {
            count = offset + ((count >> 1) * stride);
            Skeleton skeleton = slot.bone.skeleton;
            ExposedList<float> attachmentVertices = slot.attachmentVertices;
            float[] vertices = this.vertices;
            int[] bones = this.bones;
            if (bones == null)
            {
                if (attachmentVertices.Count > 0)
                {
                    vertices = attachmentVertices.Items;
                }
                Bone bone = slot.bone;
                float worldX = bone.worldX;
                float worldY = bone.worldY;
                float a = bone.a;
                float b = bone.b;
                float c = bone.c;
                float d = bone.d;
                int index = start;
                for (int i = offset; i < count; i += stride)
                {
                    float num9 = vertices[index];
                    float num10 = vertices[index + 1];
                    worldVertices[i] = ((num9 * a) + (num10 * b)) + worldX;
                    worldVertices[i + 1] = ((num9 * c) + (num10 * d)) + worldY;
                    index += 2;
                }
            }
            else
            {
                int index = 0;
                int num12 = 0;
                for (int i = 0; i < start; i += 2)
                {
                    int num14 = bones[index];
                    index += num14 + 1;
                    num12 += num14;
                }
                Bone[] items = skeleton.bones.Items;
                if (attachmentVertices.Count == 0)
                {
                    int num15 = offset;
                    int num16 = num12 * 3;
                    while (num15 < count)
                    {
                        float num17 = 0f;
                        float num18 = 0f;
                        int num19 = bones[index++];
                        num19 += index;
                        while (index < num19)
                        {
                            Bone bone2 = items[bones[index]];
                            float num20 = vertices[num16];
                            float num21 = vertices[num16 + 1];
                            float num22 = vertices[num16 + 2];
                            num17 += (((num20 * bone2.a) + (num21 * bone2.b)) + bone2.worldX) * num22;
                            num18 += (((num20 * bone2.c) + (num21 * bone2.d)) + bone2.worldY) * num22;
                            index++;
                            num16 += 3;
                        }
                        worldVertices[num15] = num17;
                        worldVertices[num15 + 1] = num18;
                        num15 += stride;
                    }
                }
                else
                {
                    float[] numArray3 = attachmentVertices.Items;
                    int num23 = offset;
                    int num24 = num12 * 3;
                    int num25 = num12 << 1;
                    while (num23 < count)
                    {
                        float num26 = 0f;
                        float num27 = 0f;
                        int num28 = bones[index++];
                        num28 += index;
                        while (index < num28)
                        {
                            Bone bone3 = items[bones[index]];
                            float num29 = vertices[num24] + numArray3[num25];
                            float num30 = vertices[num24 + 1] + numArray3[num25 + 1];
                            float num31 = vertices[num24 + 2];
                            num26 += (((num29 * bone3.a) + (num30 * bone3.b)) + bone3.worldX) * num31;
                            num27 += (((num29 * bone3.c) + (num30 * bone3.d)) + bone3.worldY) * num31;
                            index++;
                            num24 += 3;
                            num25 += 2;
                        }
                        worldVertices[num23] = num26;
                        worldVertices[num23 + 1] = num27;
                        num23 += stride;
                    }
                }
            }
        }

        public int Id =>
            this.id;

        public int[] Bones
        {
            get => 
                this.bones;
            set => 
                (this.bones = value);
        }

        public float[] Vertices
        {
            get => 
                this.vertices;
            set => 
                (this.vertices = value);
        }

        public int WorldVerticesLength
        {
            get => 
                this.worldVerticesLength;
            set => 
                (this.worldVerticesLength = value);
        }
    }
}

