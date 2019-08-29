namespace Spine
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class MeshAttachment : VertexAttachment, IHasRendererObject
    {
        internal float regionOffsetX;
        internal float regionOffsetY;
        internal float regionWidth;
        internal float regionHeight;
        internal float regionOriginalWidth;
        internal float regionOriginalHeight;
        private MeshAttachment parentMesh;
        internal float[] uvs;
        internal float[] regionUVs;
        internal int[] triangles;
        internal float r;
        internal float g;
        internal float b;
        internal float a;
        internal int hulllength;
        internal bool inheritDeform;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Path>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private object <RendererObject>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <RegionU>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <RegionV>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <RegionU2>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <RegionV2>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <RegionRotate>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int[] <Edges>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <Width>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <Height>k__BackingField;

        public MeshAttachment(string name) : base(name)
        {
            this.r = 1f;
            this.g = 1f;
            this.b = 1f;
            this.a = 1f;
        }

        public override bool ApplyDeform(VertexAttachment sourceAttachment) => 
            ((this == sourceAttachment) || (this.inheritDeform && (this.parentMesh == sourceAttachment)));

        public void UpdateUVs()
        {
            float regionU = this.RegionU;
            float regionV = this.RegionV;
            float num3 = this.RegionU2 - this.RegionU;
            float num4 = this.RegionV2 - this.RegionV;
            float[] regionUVs = this.regionUVs;
            if ((this.uvs == null) || (this.uvs.Length != regionUVs.Length))
            {
                this.uvs = new float[regionUVs.Length];
            }
            float[] uvs = this.uvs;
            if (this.RegionRotate)
            {
                int index = 0;
                int length = uvs.Length;
                while (index < length)
                {
                    uvs[index] = regionU + (regionUVs[index + 1] * num3);
                    uvs[index + 1] = (regionV + num4) - (regionUVs[index] * num4);
                    index += 2;
                }
            }
            else
            {
                int index = 0;
                int length = uvs.Length;
                while (index < length)
                {
                    uvs[index] = regionU + (regionUVs[index] * num3);
                    uvs[index + 1] = regionV + (regionUVs[index + 1] * num4);
                    index += 2;
                }
            }
        }

        public int HullLength
        {
            get => 
                this.hulllength;
            set => 
                (this.hulllength = value);
        }

        public float[] RegionUVs
        {
            get => 
                this.regionUVs;
            set => 
                (this.regionUVs = value);
        }

        public float[] UVs
        {
            get => 
                this.uvs;
            set => 
                (this.uvs = value);
        }

        public int[] Triangles
        {
            get => 
                this.triangles;
            set => 
                (this.triangles = value);
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

        public float RegionU { get; set; }

        public float RegionV { get; set; }

        public float RegionU2 { get; set; }

        public float RegionV2 { get; set; }

        public bool RegionRotate { get; set; }

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

        public bool InheritDeform
        {
            get => 
                this.inheritDeform;
            set => 
                (this.inheritDeform = value);
        }

        public MeshAttachment ParentMesh
        {
            get => 
                this.parentMesh;
            set
            {
                this.parentMesh = value;
                if (value != null)
                {
                    base.bones = value.bones;
                    base.vertices = value.vertices;
                    base.worldVerticesLength = value.worldVerticesLength;
                    this.regionUVs = value.regionUVs;
                    this.triangles = value.triangles;
                    this.HullLength = value.HullLength;
                    this.Edges = value.Edges;
                    this.Width = value.Width;
                    this.Height = value.Height;
                }
            }
        }

        public int[] Edges { get; set; }

        public float Width { get; set; }

        public float Height { get; set; }
    }
}

