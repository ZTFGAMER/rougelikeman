namespace Spine
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public class Skeleton
    {
        internal SkeletonData data;
        internal ExposedList<Bone> bones;
        internal ExposedList<Slot> slots;
        internal ExposedList<Slot> drawOrder;
        internal ExposedList<IkConstraint> ikConstraints;
        internal ExposedList<TransformConstraint> transformConstraints;
        internal ExposedList<PathConstraint> pathConstraints;
        internal ExposedList<IUpdatable> updateCache = new ExposedList<IUpdatable>();
        internal ExposedList<Bone> updateCacheReset = new ExposedList<Bone>();
        internal Spine.Skin skin;
        internal float r = 1f;
        internal float g = 1f;
        internal float b = 1f;
        internal float a = 1f;
        internal float time;
        internal bool flipX;
        internal bool flipY;
        internal float x;
        internal float y;

        public Skeleton(SkeletonData data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data", "data cannot be null.");
            }
            this.data = data;
            this.bones = new ExposedList<Bone>(data.bones.Count);
            foreach (BoneData data2 in data.bones)
            {
                Bone bone;
                if (data2.parent == null)
                {
                    bone = new Bone(data2, this, null);
                }
                else
                {
                    Bone parent = this.bones.Items[data2.parent.index];
                    bone = new Bone(data2, this, parent);
                    parent.children.Add(bone);
                }
                this.bones.Add(bone);
            }
            this.slots = new ExposedList<Slot>(data.slots.Count);
            this.drawOrder = new ExposedList<Slot>(data.slots.Count);
            foreach (SlotData data3 in data.slots)
            {
                Bone bone = this.bones.Items[data3.boneData.index];
                Slot item = new Slot(data3, bone);
                this.slots.Add(item);
                this.drawOrder.Add(item);
            }
            this.ikConstraints = new ExposedList<IkConstraint>(data.ikConstraints.Count);
            foreach (IkConstraintData data4 in data.ikConstraints)
            {
                this.ikConstraints.Add(new IkConstraint(data4, this));
            }
            this.transformConstraints = new ExposedList<TransformConstraint>(data.transformConstraints.Count);
            foreach (TransformConstraintData data5 in data.transformConstraints)
            {
                this.transformConstraints.Add(new TransformConstraint(data5, this));
            }
            this.pathConstraints = new ExposedList<PathConstraint>(data.pathConstraints.Count);
            foreach (PathConstraintData data6 in data.pathConstraints)
            {
                this.pathConstraints.Add(new PathConstraint(data6, this));
            }
            this.UpdateCache();
            this.UpdateWorldTransform();
        }

        public Bone FindBone(string boneName)
        {
            if (boneName == null)
            {
                throw new ArgumentNullException("boneName", "boneName cannot be null.");
            }
            ExposedList<Bone> bones = this.bones;
            Bone[] items = bones.Items;
            int index = 0;
            int count = bones.Count;
            while (index < count)
            {
                Bone bone = items[index];
                if (bone.data.name == boneName)
                {
                    return bone;
                }
                index++;
            }
            return null;
        }

        public int FindBoneIndex(string boneName)
        {
            if (boneName == null)
            {
                throw new ArgumentNullException("boneName", "boneName cannot be null.");
            }
            ExposedList<Bone> bones = this.bones;
            Bone[] items = bones.Items;
            int index = 0;
            int count = bones.Count;
            while (index < count)
            {
                if (items[index].data.name == boneName)
                {
                    return index;
                }
                index++;
            }
            return -1;
        }

        public IkConstraint FindIkConstraint(string constraintName)
        {
            if (constraintName == null)
            {
                throw new ArgumentNullException("constraintName", "constraintName cannot be null.");
            }
            ExposedList<IkConstraint> ikConstraints = this.ikConstraints;
            int index = 0;
            int count = ikConstraints.Count;
            while (index < count)
            {
                IkConstraint constraint = ikConstraints.Items[index];
                if (constraint.data.name == constraintName)
                {
                    return constraint;
                }
                index++;
            }
            return null;
        }

        public PathConstraint FindPathConstraint(string constraintName)
        {
            if (constraintName == null)
            {
                throw new ArgumentNullException("constraintName", "constraintName cannot be null.");
            }
            ExposedList<PathConstraint> pathConstraints = this.pathConstraints;
            int index = 0;
            int count = pathConstraints.Count;
            while (index < count)
            {
                PathConstraint constraint = pathConstraints.Items[index];
                if (constraint.data.name.Equals(constraintName))
                {
                    return constraint;
                }
                index++;
            }
            return null;
        }

        public Slot FindSlot(string slotName)
        {
            if (slotName == null)
            {
                throw new ArgumentNullException("slotName", "slotName cannot be null.");
            }
            ExposedList<Slot> slots = this.slots;
            Slot[] items = slots.Items;
            int index = 0;
            int count = slots.Count;
            while (index < count)
            {
                Slot slot = items[index];
                if (slot.data.name == slotName)
                {
                    return slot;
                }
                index++;
            }
            return null;
        }

        public int FindSlotIndex(string slotName)
        {
            if (slotName == null)
            {
                throw new ArgumentNullException("slotName", "slotName cannot be null.");
            }
            ExposedList<Slot> slots = this.slots;
            Slot[] items = slots.Items;
            int index = 0;
            int count = slots.Count;
            while (index < count)
            {
                if (items[index].data.name.Equals(slotName))
                {
                    return index;
                }
                index++;
            }
            return -1;
        }

        public TransformConstraint FindTransformConstraint(string constraintName)
        {
            if (constraintName == null)
            {
                throw new ArgumentNullException("constraintName", "constraintName cannot be null.");
            }
            ExposedList<TransformConstraint> transformConstraints = this.transformConstraints;
            int index = 0;
            int count = transformConstraints.Count;
            while (index < count)
            {
                TransformConstraint constraint = transformConstraints.Items[index];
                if (constraint.data.name == constraintName)
                {
                    return constraint;
                }
                index++;
            }
            return null;
        }

        public Attachment GetAttachment(int slotIndex, string attachmentName)
        {
            if (attachmentName == null)
            {
                throw new ArgumentNullException("attachmentName", "attachmentName cannot be null.");
            }
            if (this.skin != null)
            {
                Attachment attachment = this.skin.GetAttachment(slotIndex, attachmentName);
                if (attachment != null)
                {
                    return attachment;
                }
            }
            return this.data.defaultSkin?.GetAttachment(slotIndex, attachmentName);
        }

        public Attachment GetAttachment(string slotName, string attachmentName) => 
            this.GetAttachment(this.data.FindSlotIndex(slotName), attachmentName);

        public void GetBounds(out float x, out float y, out float width, out float height, ref float[] vertexBuffer)
        {
            float[] worldVertices = vertexBuffer;
            if (worldVertices != null)
            {
                worldVertices = worldVertices;
            }
            else
            {
                worldVertices = new float[8];
            }
            Slot[] items = this.drawOrder.Items;
            float num = 2.147484E+09f;
            float num2 = 2.147484E+09f;
            float num3 = -2.147484E+09f;
            float num4 = -2.147484E+09f;
            int index = 0;
            int count = this.drawOrder.Count;
            while (index < count)
            {
                Slot slot = items[index];
                int worldVerticesLength = 0;
                float[] numArray2 = null;
                Attachment attachment = slot.attachment;
                RegionAttachment attachment2 = attachment as RegionAttachment;
                if (attachment2 != null)
                {
                    worldVerticesLength = 8;
                    numArray2 = worldVertices;
                    if (numArray2.Length < 8)
                    {
                        numArray2 = worldVertices = new float[8];
                    }
                    attachment2.ComputeWorldVertices(slot.bone, worldVertices, 0, 2);
                }
                else
                {
                    MeshAttachment attachment3 = attachment as MeshAttachment;
                    if (attachment3 != null)
                    {
                        MeshAttachment attachment4 = attachment3;
                        worldVerticesLength = attachment4.WorldVerticesLength;
                        numArray2 = worldVertices;
                        if (numArray2.Length < worldVerticesLength)
                        {
                            numArray2 = worldVertices = new float[worldVerticesLength];
                        }
                        attachment4.ComputeWorldVertices(slot, 0, worldVerticesLength, worldVertices, 0, 2);
                    }
                }
                if (numArray2 != null)
                {
                    for (int i = 0; i < worldVerticesLength; i += 2)
                    {
                        float num9 = numArray2[i];
                        float num10 = numArray2[i + 1];
                        num = Math.Min(num, num9);
                        num2 = Math.Min(num2, num10);
                        num3 = Math.Max(num3, num9);
                        num4 = Math.Max(num4, num10);
                    }
                }
                index++;
            }
            x = num;
            y = num2;
            width = num3 - num;
            height = num4 - num2;
            vertexBuffer = worldVertices;
        }

        public void SetAttachment(string slotName, string attachmentName)
        {
            if (slotName == null)
            {
                throw new ArgumentNullException("slotName", "slotName cannot be null.");
            }
            ExposedList<Slot> slots = this.slots;
            int index = 0;
            int count = slots.Count;
            while (index < count)
            {
                Slot slot = slots.Items[index];
                if (slot.data.name == slotName)
                {
                    Attachment attachment = null;
                    if (attachmentName != null)
                    {
                        attachment = this.GetAttachment(index, attachmentName);
                        if (attachment == null)
                        {
                            throw new Exception("Attachment not found: " + attachmentName + ", for slot: " + slotName);
                        }
                    }
                    slot.Attachment = attachment;
                    return;
                }
                index++;
            }
            throw new Exception("Slot not found: " + slotName);
        }

        public void SetBonesToSetupPose()
        {
            Bone[] items = this.bones.Items;
            int index = 0;
            int count = this.bones.Count;
            while (index < count)
            {
                items[index].SetToSetupPose();
                index++;
            }
            IkConstraint[] constraintArray = this.ikConstraints.Items;
            int num3 = 0;
            int num4 = this.ikConstraints.Count;
            while (num3 < num4)
            {
                IkConstraint constraint = constraintArray[num3];
                constraint.bendDirection = constraint.data.bendDirection;
                constraint.mix = constraint.data.mix;
                num3++;
            }
            TransformConstraint[] constraintArray2 = this.transformConstraints.Items;
            int num5 = 0;
            int num6 = this.transformConstraints.Count;
            while (num5 < num6)
            {
                TransformConstraint constraint2 = constraintArray2[num5];
                TransformConstraintData data = constraint2.data;
                constraint2.rotateMix = data.rotateMix;
                constraint2.translateMix = data.translateMix;
                constraint2.scaleMix = data.scaleMix;
                constraint2.shearMix = data.shearMix;
                num5++;
            }
            PathConstraint[] constraintArray3 = this.pathConstraints.Items;
            int num7 = 0;
            int num8 = this.pathConstraints.Count;
            while (num7 < num8)
            {
                PathConstraint constraint3 = constraintArray3[num7];
                PathConstraintData data = constraint3.data;
                constraint3.position = data.position;
                constraint3.spacing = data.spacing;
                constraint3.rotateMix = data.rotateMix;
                constraint3.translateMix = data.translateMix;
                num7++;
            }
        }

        public void SetSkin(Spine.Skin newSkin)
        {
            if (newSkin != null)
            {
                if (this.skin != null)
                {
                    newSkin.AttachAll(this, this.skin);
                }
                else
                {
                    ExposedList<Slot> slots = this.slots;
                    int index = 0;
                    int count = slots.Count;
                    while (index < count)
                    {
                        Slot slot = slots.Items[index];
                        string attachmentName = slot.data.attachmentName;
                        if (attachmentName != null)
                        {
                            Attachment attachment = newSkin.GetAttachment(index, attachmentName);
                            if (attachment != null)
                            {
                                slot.Attachment = attachment;
                            }
                        }
                        index++;
                    }
                }
            }
            this.skin = newSkin;
        }

        public void SetSkin(string skinName)
        {
            Spine.Skin newSkin = this.data.FindSkin(skinName);
            if (newSkin == null)
            {
                throw new ArgumentException("Skin not found: " + skinName, "skinName");
            }
            this.SetSkin(newSkin);
        }

        public void SetSlotsToSetupPose()
        {
            ExposedList<Slot> slots = this.slots;
            Slot[] items = slots.Items;
            this.drawOrder.Clear(true);
            int index = 0;
            int count = slots.Count;
            while (index < count)
            {
                this.drawOrder.Add(items[index]);
                index++;
            }
            int num3 = 0;
            int num4 = slots.Count;
            while (num3 < num4)
            {
                items[num3].SetToSetupPose();
                num3++;
            }
        }

        public void SetToSetupPose()
        {
            this.SetBonesToSetupPose();
            this.SetSlotsToSetupPose();
        }

        private void SortBone(Bone bone)
        {
            if (!bone.sorted)
            {
                Bone parent = bone.parent;
                if (parent != null)
                {
                    this.SortBone(parent);
                }
                bone.sorted = true;
                this.updateCache.Add(bone);
            }
        }

        private void SortIkConstraint(IkConstraint constraint)
        {
            Bone target = constraint.target;
            this.SortBone(target);
            ExposedList<Bone> bones = constraint.bones;
            Bone bone = bones.Items[0];
            this.SortBone(bone);
            if (bones.Count > 1)
            {
                Bone item = bones.Items[bones.Count - 1];
                if (!this.updateCache.Contains(item))
                {
                    this.updateCacheReset.Add(item);
                }
            }
            this.updateCache.Add(constraint);
            SortReset(bone.children);
            bones.Items[bones.Count - 1].sorted = true;
        }

        private void SortPathConstraint(PathConstraint constraint)
        {
            Slot target = constraint.target;
            int index = target.data.index;
            Bone slotBone = target.bone;
            if (this.skin != null)
            {
                this.SortPathConstraintAttachment(this.skin, index, slotBone);
            }
            if ((this.data.defaultSkin != null) && (this.data.defaultSkin != this.skin))
            {
                this.SortPathConstraintAttachment(this.data.defaultSkin, index, slotBone);
            }
            int num2 = 0;
            int count = this.data.skins.Count;
            while (num2 < count)
            {
                this.SortPathConstraintAttachment(this.data.skins.Items[num2], index, slotBone);
                num2++;
            }
            Attachment attachment = target.attachment;
            if (attachment is PathAttachment)
            {
                this.SortPathConstraintAttachment(attachment, slotBone);
            }
            ExposedList<Bone> bones = constraint.bones;
            int num4 = bones.Count;
            for (int i = 0; i < num4; i++)
            {
                this.SortBone(bones.Items[i]);
            }
            this.updateCache.Add(constraint);
            for (int j = 0; j < num4; j++)
            {
                SortReset(bones.Items[j].children);
            }
            for (int k = 0; k < num4; k++)
            {
                bones.Items[k].sorted = true;
            }
        }

        private void SortPathConstraintAttachment(Attachment attachment, Bone slotBone)
        {
            if (attachment is PathAttachment)
            {
                int[] bones = ((PathAttachment) attachment).bones;
                if (bones == null)
                {
                    this.SortBone(slotBone);
                }
                else
                {
                    ExposedList<Bone> list = this.bones;
                    int num = 0;
                    int length = bones.Length;
                    while (num < length)
                    {
                        int num3 = bones[num++];
                        num3 += num;
                        while (num < num3)
                        {
                            this.SortBone(list.Items[bones[num++]]);
                        }
                    }
                }
            }
        }

        private void SortPathConstraintAttachment(Spine.Skin skin, int slotIndex, Bone slotBone)
        {
            foreach (KeyValuePair<Spine.Skin.AttachmentKeyTuple, Attachment> pair in skin.Attachments)
            {
                if (pair.Key.slotIndex == slotIndex)
                {
                    this.SortPathConstraintAttachment(pair.Value, slotBone);
                }
            }
        }

        private static void SortReset(ExposedList<Bone> bones)
        {
            Bone[] items = bones.Items;
            int index = 0;
            int count = bones.Count;
            while (index < count)
            {
                Bone bone = items[index];
                if (bone.sorted)
                {
                    SortReset(bone.children);
                }
                bone.sorted = false;
                index++;
            }
        }

        private void SortTransformConstraint(TransformConstraint constraint)
        {
            this.SortBone(constraint.target);
            ExposedList<Bone> bones = constraint.bones;
            int count = bones.Count;
            if (constraint.data.local)
            {
                for (int k = 0; k < count; k++)
                {
                    Bone item = bones.Items[k];
                    this.SortBone(item.parent);
                    if (!this.updateCache.Contains(item))
                    {
                        this.updateCacheReset.Add(item);
                    }
                }
            }
            else
            {
                for (int k = 0; k < count; k++)
                {
                    this.SortBone(bones.Items[k]);
                }
            }
            this.updateCache.Add(constraint);
            for (int i = 0; i < count; i++)
            {
                SortReset(bones.Items[i].children);
            }
            for (int j = 0; j < count; j++)
            {
                bones.Items[j].sorted = true;
            }
        }

        public void Update(float delta)
        {
            this.time += delta;
        }

        public void UpdateCache()
        {
            this.updateCache.Clear(true);
            this.updateCacheReset.Clear(true);
            ExposedList<Bone> bones = this.bones;
            int index = 0;
            int count = bones.Count;
            while (index < count)
            {
                bones.Items[index].sorted = false;
                index++;
            }
            ExposedList<IkConstraint> ikConstraints = this.ikConstraints;
            ExposedList<TransformConstraint> transformConstraints = this.transformConstraints;
            ExposedList<PathConstraint> pathConstraints = this.pathConstraints;
            int num3 = this.IkConstraints.Count;
            int num4 = transformConstraints.Count;
            int num5 = pathConstraints.Count;
            int num6 = (num3 + num4) + num5;
            for (int i = 0; i < num6; i++)
            {
                for (int j = 0; j < num3; j++)
                {
                    IkConstraint constraint = ikConstraints.Items[j];
                    if (constraint.data.order == i)
                    {
                        this.SortIkConstraint(constraint);
                        continue;
                    }
                }
                for (int k = 0; k < num4; k++)
                {
                    TransformConstraint constraint = transformConstraints.Items[k];
                    if (constraint.data.order == i)
                    {
                        this.SortTransformConstraint(constraint);
                        continue;
                    }
                }
                for (int m = 0; m < num5; m++)
                {
                    PathConstraint constraint = pathConstraints.Items[m];
                    if (constraint.data.order == i)
                    {
                        this.SortPathConstraint(constraint);
                        break;
                    }
                }
            }
            int num11 = 0;
            int num12 = bones.Count;
            while (num11 < num12)
            {
                this.SortBone(bones.Items[num11]);
                num11++;
            }
        }

        public void UpdateWorldTransform()
        {
            ExposedList<Bone> updateCacheReset = this.updateCacheReset;
            Bone[] items = updateCacheReset.Items;
            int index = 0;
            int count = updateCacheReset.Count;
            while (index < count)
            {
                Bone bone = items[index];
                bone.ax = bone.x;
                bone.ay = bone.y;
                bone.arotation = bone.rotation;
                bone.ascaleX = bone.scaleX;
                bone.ascaleY = bone.scaleY;
                bone.ashearX = bone.shearX;
                bone.ashearY = bone.shearY;
                bone.appliedValid = true;
                index++;
            }
            IUpdatable[] updatableArray = this.updateCache.Items;
            int num3 = 0;
            int num4 = this.updateCache.Count;
            while (num3 < num4)
            {
                updatableArray[num3].Update();
                num3++;
            }
        }

        public SkeletonData Data =>
            this.data;

        public ExposedList<Bone> Bones =>
            this.bones;

        public ExposedList<IUpdatable> UpdateCacheList =>
            this.updateCache;

        public ExposedList<Slot> Slots =>
            this.slots;

        public ExposedList<Slot> DrawOrder =>
            this.drawOrder;

        public ExposedList<IkConstraint> IkConstraints =>
            this.ikConstraints;

        public ExposedList<PathConstraint> PathConstraints =>
            this.pathConstraints;

        public ExposedList<TransformConstraint> TransformConstraints =>
            this.transformConstraints;

        public Spine.Skin Skin
        {
            get => 
                this.skin;
            set => 
                (this.skin = value);
        }

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

        public float Time
        {
            get => 
                this.time;
            set => 
                (this.time = value);
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

        public bool FlipX
        {
            get => 
                this.flipX;
            set => 
                (this.flipX = value);
        }

        public bool FlipY
        {
            get => 
                this.flipY;
            set => 
                (this.flipY = value);
        }

        public Bone RootBone =>
            ((this.bones.Count != 0) ? this.bones.Items[0] : null);
    }
}

