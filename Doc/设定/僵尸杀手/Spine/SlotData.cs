namespace Spine
{
    using System;

    public class SlotData
    {
        internal int index;
        internal string name;
        internal Spine.BoneData boneData;
        internal float r = 1f;
        internal float g = 1f;
        internal float b = 1f;
        internal float a = 1f;
        internal float r2;
        internal float g2;
        internal float b2;
        internal bool hasSecondColor;
        internal string attachmentName;
        internal Spine.BlendMode blendMode;

        public SlotData(int index, string name, Spine.BoneData boneData)
        {
            if (index < 0)
            {
                throw new ArgumentException("index must be >= 0.", "index");
            }
            if (name == null)
            {
                throw new ArgumentNullException("name", "name cannot be null.");
            }
            if (boneData == null)
            {
                throw new ArgumentNullException("boneData", "boneData cannot be null.");
            }
            this.index = index;
            this.name = name;
            this.boneData = boneData;
        }

        public override string ToString() => 
            this.name;

        public int Index =>
            this.index;

        public string Name =>
            this.name;

        public Spine.BoneData BoneData =>
            this.boneData;

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

        public float R2
        {
            get => 
                this.r2;
            set => 
                (this.r2 = value);
        }

        public float G2
        {
            get => 
                this.g2;
            set => 
                (this.g2 = value);
        }

        public float B2
        {
            get => 
                this.b2;
            set => 
                (this.b2 = value);
        }

        public bool HasSecondColor
        {
            get => 
                this.hasSecondColor;
            set => 
                (this.hasSecondColor = value);
        }

        public string AttachmentName
        {
            get => 
                this.attachmentName;
            set => 
                (this.attachmentName = value);
        }

        public Spine.BlendMode BlendMode
        {
            get => 
                this.blendMode;
            set => 
                (this.blendMode = value);
        }
    }
}

