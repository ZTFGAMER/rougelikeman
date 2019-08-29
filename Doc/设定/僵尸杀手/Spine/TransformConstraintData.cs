namespace Spine
{
    using System;

    public class TransformConstraintData
    {
        internal string name;
        internal int order;
        internal ExposedList<BoneData> bones = new ExposedList<BoneData>();
        internal BoneData target;
        internal float rotateMix;
        internal float translateMix;
        internal float scaleMix;
        internal float shearMix;
        internal float offsetRotation;
        internal float offsetX;
        internal float offsetY;
        internal float offsetScaleX;
        internal float offsetScaleY;
        internal float offsetShearY;
        internal bool relative;
        internal bool local;

        public TransformConstraintData(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name", "name cannot be null.");
            }
            this.name = name;
        }

        public override string ToString() => 
            this.name;

        public string Name =>
            this.name;

        public int Order
        {
            get => 
                this.order;
            set => 
                (this.order = value);
        }

        public ExposedList<BoneData> Bones =>
            this.bones;

        public BoneData Target
        {
            get => 
                this.target;
            set => 
                (this.target = value);
        }

        public float RotateMix
        {
            get => 
                this.rotateMix;
            set => 
                (this.rotateMix = value);
        }

        public float TranslateMix
        {
            get => 
                this.translateMix;
            set => 
                (this.translateMix = value);
        }

        public float ScaleMix
        {
            get => 
                this.scaleMix;
            set => 
                (this.scaleMix = value);
        }

        public float ShearMix
        {
            get => 
                this.shearMix;
            set => 
                (this.shearMix = value);
        }

        public float OffsetRotation
        {
            get => 
                this.offsetRotation;
            set => 
                (this.offsetRotation = value);
        }

        public float OffsetX
        {
            get => 
                this.offsetX;
            set => 
                (this.offsetX = value);
        }

        public float OffsetY
        {
            get => 
                this.offsetY;
            set => 
                (this.offsetY = value);
        }

        public float OffsetScaleX
        {
            get => 
                this.offsetScaleX;
            set => 
                (this.offsetScaleX = value);
        }

        public float OffsetScaleY
        {
            get => 
                this.offsetScaleY;
            set => 
                (this.offsetScaleY = value);
        }

        public float OffsetShearY
        {
            get => 
                this.offsetShearY;
            set => 
                (this.offsetShearY = value);
        }

        public bool Relative
        {
            get => 
                this.relative;
            set => 
                (this.relative = value);
        }

        public bool Local
        {
            get => 
                this.local;
            set => 
                (this.local = value);
        }
    }
}

