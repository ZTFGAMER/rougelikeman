namespace Spine
{
    using System;

    public class BoneData
    {
        internal int index;
        internal string name;
        internal BoneData parent;
        internal float length;
        internal float x;
        internal float y;
        internal float rotation;
        internal float scaleX = 1f;
        internal float scaleY = 1f;
        internal float shearX;
        internal float shearY;
        internal Spine.TransformMode transformMode;

        public BoneData(int index, string name, BoneData parent)
        {
            if (index < 0)
            {
                throw new ArgumentException("index must be >= 0", "index");
            }
            if (name == null)
            {
                throw new ArgumentNullException("name", "name cannot be null.");
            }
            this.index = index;
            this.name = name;
            this.parent = parent;
        }

        public override string ToString() => 
            this.name;

        public int Index =>
            this.index;

        public string Name =>
            this.name;

        public BoneData Parent =>
            this.parent;

        public float Length
        {
            get => 
                this.length;
            set => 
                (this.length = value);
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

        public float ShearX
        {
            get => 
                this.shearX;
            set => 
                (this.shearX = value);
        }

        public float ShearY
        {
            get => 
                this.shearY;
            set => 
                (this.shearY = value);
        }

        public Spine.TransformMode TransformMode
        {
            get => 
                this.transformMode;
            set => 
                (this.transformMode = value);
        }
    }
}

