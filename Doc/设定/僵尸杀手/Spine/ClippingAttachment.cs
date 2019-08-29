namespace Spine
{
    using System;

    public class ClippingAttachment : VertexAttachment
    {
        internal SlotData endSlot;

        public ClippingAttachment(string name) : base(name)
        {
        }

        public SlotData EndSlot
        {
            get => 
                this.endSlot;
            set => 
                (this.endSlot = value);
        }
    }
}

