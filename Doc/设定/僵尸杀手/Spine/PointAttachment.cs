namespace Spine
{
    using System;
    using System.Runtime.InteropServices;

    public class PointAttachment : Attachment
    {
        internal float x;
        internal float y;
        internal float rotation;

        public PointAttachment(string name) : base(name)
        {
        }

        public void ComputeWorldPosition(Bone bone, out float ox, out float oy)
        {
            bone.LocalToWorld(this.x, this.y, out ox, out oy);
        }

        public float ComputeWorldRotation(Bone bone)
        {
            float num = MathUtils.CosDeg(this.rotation);
            float num2 = MathUtils.SinDeg(this.rotation);
            float x = (num * bone.a) + (num2 * bone.b);
            float y = (num * bone.c) + (num2 * bone.d);
            return (MathUtils.Atan2(y, x) * 57.29578f);
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
    }
}

