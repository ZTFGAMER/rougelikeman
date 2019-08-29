namespace Spine
{
    using System;

    public class SkeletonData
    {
        internal string name;
        internal ExposedList<BoneData> bones = new ExposedList<BoneData>();
        internal ExposedList<SlotData> slots = new ExposedList<SlotData>();
        internal ExposedList<Skin> skins = new ExposedList<Skin>();
        internal Skin defaultSkin;
        internal ExposedList<EventData> events = new ExposedList<EventData>();
        internal ExposedList<Animation> animations = new ExposedList<Animation>();
        internal ExposedList<IkConstraintData> ikConstraints = new ExposedList<IkConstraintData>();
        internal ExposedList<TransformConstraintData> transformConstraints = new ExposedList<TransformConstraintData>();
        internal ExposedList<PathConstraintData> pathConstraints = new ExposedList<PathConstraintData>();
        internal float width;
        internal float height;
        internal string version;
        internal string hash;
        internal float fps;
        internal string imagesPath;

        public Animation FindAnimation(string animationName)
        {
            if (animationName == null)
            {
                throw new ArgumentNullException("animationName", "animationName cannot be null.");
            }
            ExposedList<Animation> animations = this.animations;
            int index = 0;
            int count = animations.Count;
            while (index < count)
            {
                Animation animation = animations.Items[index];
                if (animation.name == animationName)
                {
                    return animation;
                }
                index++;
            }
            return null;
        }

        public BoneData FindBone(string boneName)
        {
            if (boneName == null)
            {
                throw new ArgumentNullException("boneName", "boneName cannot be null.");
            }
            ExposedList<BoneData> bones = this.bones;
            BoneData[] items = bones.Items;
            int index = 0;
            int count = bones.Count;
            while (index < count)
            {
                BoneData data = items[index];
                if (data.name == boneName)
                {
                    return data;
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
            ExposedList<BoneData> bones = this.bones;
            BoneData[] items = bones.Items;
            int index = 0;
            int count = bones.Count;
            while (index < count)
            {
                if (items[index].name == boneName)
                {
                    return index;
                }
                index++;
            }
            return -1;
        }

        public EventData FindEvent(string eventDataName)
        {
            if (eventDataName == null)
            {
                throw new ArgumentNullException("eventDataName", "eventDataName cannot be null.");
            }
            foreach (EventData data in this.events)
            {
                if (data.name == eventDataName)
                {
                    return data;
                }
            }
            return null;
        }

        public IkConstraintData FindIkConstraint(string constraintName)
        {
            if (constraintName == null)
            {
                throw new ArgumentNullException("constraintName", "constraintName cannot be null.");
            }
            ExposedList<IkConstraintData> ikConstraints = this.ikConstraints;
            int index = 0;
            int count = ikConstraints.Count;
            while (index < count)
            {
                IkConstraintData data = ikConstraints.Items[index];
                if (data.name == constraintName)
                {
                    return data;
                }
                index++;
            }
            return null;
        }

        public PathConstraintData FindPathConstraint(string constraintName)
        {
            if (constraintName == null)
            {
                throw new ArgumentNullException("constraintName", "constraintName cannot be null.");
            }
            ExposedList<PathConstraintData> pathConstraints = this.pathConstraints;
            int index = 0;
            int count = pathConstraints.Count;
            while (index < count)
            {
                PathConstraintData data = pathConstraints.Items[index];
                if (data.name.Equals(constraintName))
                {
                    return data;
                }
                index++;
            }
            return null;
        }

        public int FindPathConstraintIndex(string pathConstraintName)
        {
            if (pathConstraintName == null)
            {
                throw new ArgumentNullException("pathConstraintName", "pathConstraintName cannot be null.");
            }
            ExposedList<PathConstraintData> pathConstraints = this.pathConstraints;
            int index = 0;
            int count = pathConstraints.Count;
            while (index < count)
            {
                if (pathConstraints.Items[index].name.Equals(pathConstraintName))
                {
                    return index;
                }
                index++;
            }
            return -1;
        }

        public Skin FindSkin(string skinName)
        {
            if (skinName == null)
            {
                throw new ArgumentNullException("skinName", "skinName cannot be null.");
            }
            foreach (Skin skin in this.skins)
            {
                if (skin.name == skinName)
                {
                    return skin;
                }
            }
            return null;
        }

        public SlotData FindSlot(string slotName)
        {
            if (slotName == null)
            {
                throw new ArgumentNullException("slotName", "slotName cannot be null.");
            }
            ExposedList<SlotData> slots = this.slots;
            int index = 0;
            int count = slots.Count;
            while (index < count)
            {
                SlotData data = slots.Items[index];
                if (data.name == slotName)
                {
                    return data;
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
            ExposedList<SlotData> slots = this.slots;
            int index = 0;
            int count = slots.Count;
            while (index < count)
            {
                if (slots.Items[index].name == slotName)
                {
                    return index;
                }
                index++;
            }
            return -1;
        }

        public TransformConstraintData FindTransformConstraint(string constraintName)
        {
            if (constraintName == null)
            {
                throw new ArgumentNullException("constraintName", "constraintName cannot be null.");
            }
            ExposedList<TransformConstraintData> transformConstraints = this.transformConstraints;
            int index = 0;
            int count = transformConstraints.Count;
            while (index < count)
            {
                TransformConstraintData data = transformConstraints.Items[index];
                if (data.name == constraintName)
                {
                    return data;
                }
                index++;
            }
            return null;
        }

        public override string ToString()
        {
            if (this.name == null)
            {
            }
            return base.ToString();
        }

        public string Name
        {
            get => 
                this.name;
            set => 
                (this.name = value);
        }

        public ExposedList<BoneData> Bones =>
            this.bones;

        public ExposedList<SlotData> Slots =>
            this.slots;

        public ExposedList<Skin> Skins
        {
            get => 
                this.skins;
            set => 
                (this.skins = value);
        }

        public Skin DefaultSkin
        {
            get => 
                this.defaultSkin;
            set => 
                (this.defaultSkin = value);
        }

        public ExposedList<EventData> Events
        {
            get => 
                this.events;
            set => 
                (this.events = value);
        }

        public ExposedList<Animation> Animations
        {
            get => 
                this.animations;
            set => 
                (this.animations = value);
        }

        public ExposedList<IkConstraintData> IkConstraints
        {
            get => 
                this.ikConstraints;
            set => 
                (this.ikConstraints = value);
        }

        public ExposedList<TransformConstraintData> TransformConstraints
        {
            get => 
                this.transformConstraints;
            set => 
                (this.transformConstraints = value);
        }

        public ExposedList<PathConstraintData> PathConstraints
        {
            get => 
                this.pathConstraints;
            set => 
                (this.pathConstraints = value);
        }

        public float Width
        {
            get => 
                this.width;
            set => 
                (this.width = value);
        }

        public float Height
        {
            get => 
                this.height;
            set => 
                (this.height = value);
        }

        public string Version
        {
            get => 
                this.version;
            set => 
                (this.version = value);
        }

        public string Hash
        {
            get => 
                this.hash;
            set => 
                (this.hash = value);
        }

        public string ImagesPath
        {
            get => 
                this.imagesPath;
            set => 
                (this.imagesPath = value);
        }

        public float Fps
        {
            get => 
                this.fps;
            set => 
                (this.fps = value);
        }
    }
}

