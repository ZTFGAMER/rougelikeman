namespace Spine
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class RegionAttachment : Attachment, IHasRendererObject
    {
        public const int BLX = 0;
        public const int BLY = 1;
        public const int ULX = 2;
        public const int ULY = 3;
        public const int URX = 4;
        public const int URY = 5;
        public const int BRX = 6;
        public const int BRY = 7;
        internal float x;
        internal float y;
        internal float rotation;
        internal float scaleX;
        internal float scaleY;
        internal float width;
        internal float height;
        internal float regionOffsetX;
        internal float regionOffsetY;
        internal float regionWidth;
        internal float regionHeight;
        internal float regionOriginalWidth;
        internal float regionOriginalHeight;
        internal float[] offset;
        internal float[] uvs;
        internal float r;
        internal float g;
        internal float b;
        internal float a;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Path>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private object <RendererObject>k__BackingField;

        public RegionAttachment(string name) : base(name)
        {
            this.scaleX = 1f;
            this.scaleY = 1f;
            this.offset = new float[8];
            this.uvs = new float[8];
            this.r = 1f;
            this.g = 1f;
            this.b = 1f;
            this.a = 1f;
        }

        public void ComputeWorldVertices(Bone bone, float[] worldVertices, int offset, int stride = 2)
        {
            float[] numArray = this.offset;
            float worldX = bone.worldX;
            float worldY = bone.worldY;
            float a = bone.a;
            float b = bone.b;
            float c = bone.c;
            float d = bone.d;
            float num7 = numArray[6];
            float num8 = numArray[7];
            worldVertices[offset] = ((num7 * a) + (num8 * b)) + worldX;
            worldVertices[offset + 1] = ((num7 * c) + (num8 * d)) + worldY;
            offset += stride;
            num7 = numArray[0];
            num8 = numArray[1];
            worldVertices[offset] = ((num7 * a) + (num8 * b)) + worldX;
            worldVertices[offset + 1] = ((num7 * c) + (num8 * d)) + worldY;
            offset += stride;
            num7 = numArray[2];
            num8 = numArray[3];
            worldVertices[offset] = ((num7 * a) + (num8 * b)) + worldX;
            worldVertices[offset + 1] = ((num7 * c) + (num8 * d)) + worldY;
            offset += stride;
            num7 = numArray[4];
            num8 = numArray[5];
            worldVertices[offset] = ((num7 * a) + (num8 * b)) + worldX;
            worldVertices[offset + 1] = ((num7 * c) + (num8 * d)) + worldY;
        }

        public void SetUVs(float u, float v, float u2, float v2, bool rotate)
        {
            float[] uvs = this.uvs;
            if (rotate)
            {
                uvs[4] = u;
                uvs[5] = v2;
                uvs[6] = u;
                uvs[7] = v;
                uvs[0] = u2;
                uvs[1] = v;
                uvs[2] = u2;
                uvs[3] = v2;
            }
            else
            {
                uvs[2] = u;
                uvs[3] = v2;
                uvs[4] = u;
                uvs[5] = v;
                uvs[6] = u2;
                uvs[7] = v;
                uvs[0] = u2;
                uvs[1] = v2;
            }
        }

        public void UpdateOffset()
        {
            float width = this.width;
            float height = this.height;
            float num3 = width * 0.5f;
            float num4 = height * 0.5f;
            float num5 = -num3;
            float num6 = -num4;
            if (this.regionOriginalWidth != 0f)
            {
                num5 += (this.regionOffsetX / this.regionOriginalWidth) * width;
                num6 += (this.regionOffsetY / this.regionOriginalHeight) * height;
                num3 -= (((this.regionOriginalWidth - this.regionOffsetX) - this.regionWidth) / this.regionOriginalWidth) * width;
                num4 -= (((this.regionOriginalHeight - this.regionOffsetY) - this.regionHeight) / this.regionOriginalHeight) * height;
            }
            float scaleX = this.scaleX;
            float scaleY = this.scaleY;
            num5 *= scaleX;
            num6 *= scaleY;
            num3 *= scaleX;
            num4 *= scaleY;
            float rotation = this.rotation;
            float num10 = MathUtils.CosDeg(rotation);
            float num11 = MathUtils.SinDeg(rotation);
            float x = this.x;
            float y = this.y;
            float num14 = (num5 * num10) + x;
            float num15 = num5 * num11;
            float num16 = (num6 * num10) + y;
            float num17 = num6 * num11;
            float num18 = (num3 * num10) + x;
            float num19 = num3 * num11;
            float num20 = (num4 * num10) + y;
            float num21 = num4 * num11;
            float[] offset = this.offset;
            offset[0] = num14 - num17;
            offset[1] = num16 + num15;
            offset[2] = num14 - num21;
            offset[3] = num20 + num15;
            offset[4] = num18 - num21;
            offset[5] = num20 + num19;
            offset[6] = num18 - num17;
            offset[7] = num16 + num19;
        }

        public float X
        {
            get => 
                this.x;
            set => 
                (this.x = value);
        }

        public float Y
        {
            get => 
                this.y;
            set => 
                (this.y = value);
        }

        public float Rotation
        {
            get => 
                this.rotation;
            set => 
                (this.rotation = value);
        }

        public float ScaleX
        {
            get => 
                this.scaleX;
            set => 
                (this.scaleX = value);
        }

        public float ScaleY
        {
            get => 
                this.scaleY;
            set => 
                (this.scaleY = value);
        }

        public float Width
        {
            get => 
                this.width;
            set => 
                (this.width = value);
        }

        public float Height
        {
            get => 
                this.height;
            set => 
                (this.height = value);
        }

        public float R
        {
            get => 
                this.r;
            set => 
                (this.r = value);
        }

        public float G
        {
            get => 
                this.g;
            set => 
                (this.g = value);
        }

        public float B
        {
            get => 
                this.b;
            set => 
                (this.b = value);
        }

        public float A
        {
            get => 
                this.a;
            set => 
                (this.a = value);
        }

        public string Path { get; set; }

        public object RendererObject { get; set; }

        public float RegionOffsetX
        {
            get => 
                this.regionOffsetX;
            set => 
                (this.regionOffsetX = value);
        }

        public float RegionOffsetY
        {
            get => 
                this.regionOffsetY;
            set => 
                (this.regionOffsetY = value);
        }

        public float RegionWidth
        {
            get => 
                this.regionWidth;
            set => 
                (this.regionWidth = value);
        }

        public float RegionHeight
        {
            get => 
                this.regionHeight;
            set => 
                (this.regionHeight = value);
        }

        public float RegionOriginalWidth
        {
            get => 
                this.regionOriginalWidth;
            set => 
                (this.regionOriginalWidth = value);
        }

        public float RegionOriginalHeight
        {
            get => 
                this.regionOriginalHeight;
            set => 
                (this.regionOriginalHeight = value);
        }

        public float[] Offset =>
            this.offset;

        public float[] UVs =>
            this.uvs;
    }
}

