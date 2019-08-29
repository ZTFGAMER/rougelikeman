namespace Spine
{
    using System;

    public class PathConstraintData
    {
        internal string name;
        internal int order;
        internal ExposedList<BoneData> bones = new ExposedList<BoneData>();
        internal SlotData target;
        internal Spine.PositionMode positionMode;
        internal Spine.SpacingMode spacingMode;
        internal Spine.RotateMode rotateMode;
        internal float offsetRotation;
        internal float position;
        internal float spacing;
        internal float rotateMix;
        internal float translateMix;

        public PathConstraintData(string name)
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

        public SlotData Target
        {
            get => 
                this.target;
            set => 
                (this.target = value);
        }

        public Spine.PositionMode PositionMode
        {
            get => 
                this.positionMode;
            set => 
                (this.positionMode = value);
        }

        public Spine.SpacingMode SpacingMode
        {
            get => 
                this.spacingMode;
            set => 
                (this.spacingMode = value);
        }

        public Spine.RotateMode RotateMode
        {
            get => 
                this.rotateMode;
            set => 
                (this.rotateMode = value);
        }

        public float OffsetRotation
        {
            get => 
                this.offsetRotation;
            set => 
                (this.offsetRotation = value);
        }

        public float Position
        {
            get => 
                this.position;
            set => 
                (this.position = value);
        }

        public float Spacing
        {
            get => 
                this.spacing;
            set => 
                (this.spacing = value);
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
    }
}

