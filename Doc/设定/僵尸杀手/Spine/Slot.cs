namespace Spine
{
    using System;

    public class Slot
    {
        internal SlotData data;
        internal Spine.Bone bone;
        internal float r;
        internal float g;
        internal float b;
        internal float a;
        internal float r2;
        internal float g2;
        internal float b2;
        internal bool hasSecondColor;
        internal Spine.Attachment attachment;
        internal float attachmentTime;
        internal ExposedList<float> attachmentVertices = new ExposedList<float>();

        public Slot(SlotData data, Spine.Bone bone)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data", "data cannot be null.");
            }
            if (bone == null)
            {
                throw new ArgumentNullException("bone", "bone cannot be null.");
            }
            this.data = data;
            this.bone = bone;
            this.SetToSetupPose();
        }

        public void SetToSetupPose()
        {
            this.r = this.data.r;
            this.g = this.data.g;
            this.b = this.data.b;
            this.a = this.data.a;
            if (this.data.attachmentName == null)
            {
                this.Attachment = null;
            }
            else
            {
                this.attachment = null;
                this.Attachment = this.bone.skeleton.GetAttachment(this.data.index, this.data.attachmentName);
            }
        }

        public override string ToString() => 
            this.data.name;

        public SlotData Data =>
            this.data;

        public Spine.Bone Bone =>
            this.bone;

        public Spine.Skeleton Skeleton =>
            this.bone.skeleton;

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
                this.data.hasSecondColor;
            set => 
                (this.data.hasSecondColor = value);
        }

        public Spine.Attachment Attachment
        {
            get => 
                this.attachment;
            set
            {
                if (this.attachment != value)
                {
                    this.attachment = value;
                    this.attachmentTime = this.bone.skeleton.time;
                    this.attachmentVertices.Clear(false);
                }
            }
        }

        public float AttachmentTime
        {
            get => 
                (this.bone.skeleton.time - this.attachmentTime);
            set => 
                (this.attachmentTime = this.bone.skeleton.time - value);
        }

        public ExposedList<float> AttachmentVertices
        {
            get => 
                this.attachmentVertices;
            set => 
                (this.attachmentVertices = value);
        }
    }
}

