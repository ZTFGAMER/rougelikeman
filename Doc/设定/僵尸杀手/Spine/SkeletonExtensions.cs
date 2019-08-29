namespace Spine
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public static class SkeletonExtensions
    {
        public static void FindAttachmentsForSlot(this Skin skin, string slotName, SkeletonData skeletonData, List<Attachment> results)
        {
            int slotIndex = skeletonData.FindSlotIndex(slotName);
            skin.FindAttachmentsForSlot(slotIndex, results);
        }

        public static void FindNamesForSlot(this Skin skin, string slotName, SkeletonData skeletonData, List<string> results)
        {
            int slotIndex = skeletonData.FindSlotIndex(slotName);
            skin.FindNamesForSlot(slotIndex, results);
        }

        public static bool InheritsRotation(this TransformMode mode) => 
            ((((long) mode) & 1L) == 0L);

        public static bool InheritsScale(this TransformMode mode) => 
            ((((long) mode) & 2L) == 0L);

        public static bool IsRenderable(this Attachment a) => 
            (a is IHasRendererObject);

        public static bool IsWeighted(this VertexAttachment va) => 
            ((va.bones != null) && (va.bones.Length > 0));

        public static void PoseSkeleton(this Animation animation, Skeleton skeleton, float time, bool loop = false)
        {
            animation.Apply(skeleton, 0f, time, loop, null, 1f, MixPose.Setup, MixDirection.In);
        }

        public static void PoseWithAnimation(this Skeleton skeleton, string animationName, float time, bool loop = false)
        {
            Animation animation = skeleton.data.FindAnimation(animationName);
            if (animation != null)
            {
                animation.Apply(skeleton, 0f, time, loop, null, 1f, MixPose.Setup, MixDirection.In);
            }
        }

        public static void SetAttachmentToSetupPose(this Slot slot)
        {
            SlotData data = slot.data;
            slot.Attachment = slot.bone.skeleton.GetAttachment(data.name, data.attachmentName);
        }

        public static void SetColorToSetupPose(this Slot slot)
        {
            slot.r = slot.data.r;
            slot.g = slot.data.g;
            slot.b = slot.data.b;
            slot.a = slot.data.a;
            slot.r2 = slot.data.r2;
            slot.g2 = slot.data.g2;
            slot.b2 = slot.data.b2;
        }

        public static void SetDrawOrderToSetupPose(this Skeleton skeleton)
        {
            Slot[] items = skeleton.slots.Items;
            int count = skeleton.slots.Count;
            ExposedList<Slot> drawOrder = skeleton.drawOrder;
            drawOrder.Clear(false);
            drawOrder.GrowIfNeeded(count);
            Array.Copy(items, drawOrder.Items, count);
        }

        public static void SetKeyedItemsToSetupPose(this Animation animation, Skeleton skeleton)
        {
            animation.Apply(skeleton, 0f, 0f, false, null, 0f, MixPose.Setup, MixDirection.Out);
        }

        internal static void SetPropertyToSetupPose(this Skeleton skeleton, int propertyID)
        {
            Bone bone;
            PathConstraint constraint2;
            int num = propertyID >> 0x18;
            TimelineType type = (TimelineType) num;
            int index = propertyID - (num << 0x18);
            switch (type)
            {
                case TimelineType.Rotate:
                    bone = skeleton.bones.Items[index];
                    bone.rotation = bone.data.rotation;
                    break;

                case TimelineType.Translate:
                    bone = skeleton.bones.Items[index];
                    bone.x = bone.data.x;
                    bone.y = bone.data.y;
                    break;

                case TimelineType.Scale:
                    bone = skeleton.bones.Items[index];
                    bone.scaleX = bone.data.scaleX;
                    bone.scaleY = bone.data.scaleY;
                    break;

                case TimelineType.Shear:
                    bone = skeleton.bones.Items[index];
                    bone.shearX = bone.data.shearX;
                    bone.shearY = bone.data.shearY;
                    break;

                case TimelineType.Attachment:
                    skeleton.SetSlotAttachmentToSetupPose(index);
                    break;

                case TimelineType.Color:
                    skeleton.slots.Items[index].SetColorToSetupPose();
                    break;

                case TimelineType.Deform:
                    skeleton.slots.Items[index].attachmentVertices.Clear(true);
                    break;

                case TimelineType.DrawOrder:
                    skeleton.SetDrawOrderToSetupPose();
                    break;

                case TimelineType.IkConstraint:
                {
                    IkConstraint constraint = skeleton.ikConstraints.Items[index];
                    constraint.mix = constraint.data.mix;
                    constraint.bendDirection = constraint.data.bendDirection;
                    break;
                }
                case TimelineType.TransformConstraint:
                {
                    TransformConstraint constraint3 = skeleton.transformConstraints.Items[index];
                    TransformConstraintData data = constraint3.data;
                    constraint3.rotateMix = data.rotateMix;
                    constraint3.translateMix = data.translateMix;
                    constraint3.scaleMix = data.scaleMix;
                    constraint3.shearMix = data.shearMix;
                    break;
                }
                case TimelineType.PathConstraintPosition:
                    constraint2 = skeleton.pathConstraints.Items[index];
                    constraint2.position = constraint2.data.position;
                    break;

                case TimelineType.PathConstraintSpacing:
                    constraint2 = skeleton.pathConstraints.Items[index];
                    constraint2.spacing = constraint2.data.spacing;
                    break;

                case TimelineType.PathConstraintMix:
                    constraint2 = skeleton.pathConstraints.Items[index];
                    constraint2.rotateMix = constraint2.data.rotateMix;
                    constraint2.translateMix = constraint2.data.translateMix;
                    break;

                case TimelineType.TwoColor:
                    skeleton.slots.Items[index].SetColorToSetupPose();
                    break;
            }
        }

        public static void SetSlotAttachmentToSetupPose(this Skeleton skeleton, int slotIndex)
        {
            Slot slot = skeleton.slots.Items[slotIndex];
            string attachmentName = slot.data.attachmentName;
            if (string.IsNullOrEmpty(attachmentName))
            {
                slot.Attachment = null;
            }
            else
            {
                Attachment attachment = skeleton.GetAttachment(slotIndex, attachmentName);
                slot.Attachment = attachment;
            }
        }
    }
}

