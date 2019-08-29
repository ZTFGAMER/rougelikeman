namespace Spine
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public class Skin
    {
        internal string name;
        private Dictionary<AttachmentKeyTuple, Attachment> attachments = new Dictionary<AttachmentKeyTuple, Attachment>(AttachmentKeyTupleComparer.Instance);

        public Skin(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name", "name cannot be null.");
            }
            this.name = name;
        }

        public void AddAttachment(int slotIndex, string name, Attachment attachment)
        {
            if (attachment == null)
            {
                throw new ArgumentNullException("attachment", "attachment cannot be null.");
            }
            this.attachments[new AttachmentKeyTuple(slotIndex, name)] = attachment;
        }

        internal void AttachAll(Skeleton skeleton, Skin oldSkin)
        {
            foreach (KeyValuePair<AttachmentKeyTuple, Attachment> pair in oldSkin.attachments)
            {
                int slotIndex = pair.Key.slotIndex;
                Slot slot = skeleton.slots.Items[slotIndex];
                if (slot.Attachment == pair.Value)
                {
                    Attachment attachment = this.GetAttachment(slotIndex, pair.Key.name);
                    if (attachment != null)
                    {
                        slot.Attachment = attachment;
                    }
                }
            }
        }

        public void FindAttachmentsForSlot(int slotIndex, List<Attachment> attachments)
        {
            if (attachments == null)
            {
                throw new ArgumentNullException("attachments", "attachments cannot be null.");
            }
            foreach (KeyValuePair<AttachmentKeyTuple, Attachment> pair in this.attachments)
            {
                if (pair.Key.slotIndex == slotIndex)
                {
                    attachments.Add(pair.Value);
                }
            }
        }

        public void FindNamesForSlot(int slotIndex, List<string> names)
        {
            if (names == null)
            {
                throw new ArgumentNullException("names", "names cannot be null.");
            }
            foreach (AttachmentKeyTuple tuple in this.attachments.Keys)
            {
                if (tuple.slotIndex == slotIndex)
                {
                    names.Add(tuple.name);
                }
            }
        }

        public Attachment GetAttachment(int slotIndex, string name)
        {
            this.attachments.TryGetValue(new AttachmentKeyTuple(slotIndex, name), out Attachment attachment);
            return attachment;
        }

        public override string ToString() => 
            this.name;

        public string Name =>
            this.name;

        public Dictionary<AttachmentKeyTuple, Attachment> Attachments =>
            this.attachments;

        [StructLayout(LayoutKind.Sequential)]
        public struct AttachmentKeyTuple
        {
            public readonly int slotIndex;
            public readonly string name;
            internal readonly int nameHashCode;
            public AttachmentKeyTuple(int slotIndex, string name)
            {
                this.slotIndex = slotIndex;
                this.name = name;
                this.nameHashCode = this.name.GetHashCode();
            }
        }

        private class AttachmentKeyTupleComparer : IEqualityComparer<Skin.AttachmentKeyTuple>
        {
            internal static readonly Skin.AttachmentKeyTupleComparer Instance = new Skin.AttachmentKeyTupleComparer();

            bool IEqualityComparer<Skin.AttachmentKeyTuple>.Equals(Skin.AttachmentKeyTuple o1, Skin.AttachmentKeyTuple o2) => 
                (((o1.slotIndex == o2.slotIndex) && (o1.nameHashCode == o2.nameHashCode)) && string.Equals(o1.name, o2.name, StringComparison.Ordinal));

            int IEqualityComparer<Skin.AttachmentKeyTuple>.GetHashCode(Skin.AttachmentKeyTuple o) => 
                o.slotIndex;
        }
    }
}

