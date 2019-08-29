namespace Spine
{
    using System;

    public class PathAttachment : VertexAttachment
    {
        internal float[] lengths;
        internal bool closed;
        internal bool constantSpeed;

        public PathAttachment(string name) : base(name)
        {
        }

        public float[] Lengths
        {
            get => 
                this.lengths;
            set => 
                (this.lengths = value);
        }

        public bool Closed
        {
            get => 
                this.closed;
            set => 
                (this.closed = value);
        }

        public bool ConstantSpeed
        {
            get => 
                this.constantSpeed;
            set => 
                (this.constantSpeed = value);
        }
    }
}

