namespace Spine
{
    using System;
    using System.Collections.Generic;

    public class IkConstraintData
    {
        internal string name;
        internal int order;
        internal List<BoneData> bones = new List<BoneData>();
        internal BoneData target;
        internal int bendDirection = 1;
        internal float mix = 1f;

        public IkConstraintData(string name)
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

        public List<BoneData> Bones =>
            this.bones;

        public BoneData Target
        {
            get => 
                this.target;
            set => 
                (this.target = value);
        }

        public int BendDirection
        {
            get => 
                this.bendDirection;
            set => 
                (this.bendDirection = value);
        }

        public float Mix
        {
            get => 
                this.mix;
            set => 
                (this.mix = value);
        }
    }
}

