namespace Spine.Unity.Modules.AnimationTools
{
    using Spine;
    using System;
    using System.Collections.Generic;

    public static class AnimationTools
    {
        private static AttachmentTimeline GetFillerTimeline(AttachmentTimeline timeline, SkeletonData skeletonData)
        {
            AttachmentTimeline timeline2 = new AttachmentTimeline(1) {
                slotIndex = timeline.slotIndex
            };
            SlotData data = skeletonData.slots.Items[timeline2.slotIndex];
            timeline2.SetFrame(0, 0f, data.attachmentName);
            return timeline2;
        }

        private static ColorTimeline GetFillerTimeline(ColorTimeline timeline, SkeletonData skeletonData)
        {
            ColorTimeline timeline2 = new ColorTimeline(1) {
                slotIndex = timeline.slotIndex
            };
            SlotData data = skeletonData.slots.Items[timeline2.slotIndex];
            timeline2.SetFrame(0, 0f, data.r, data.g, data.b, data.a);
            return timeline2;
        }

        private static DeformTimeline GetFillerTimeline(DeformTimeline timeline, SkeletonData skeletonData)
        {
            DeformTimeline timeline2 = new DeformTimeline(1) {
                slotIndex = timeline.slotIndex,
                attachment = timeline.attachment
            };
            SlotData data = skeletonData.slots.Items[timeline2.slotIndex];
            if (timeline2.attachment.IsWeighted())
            {
                timeline2.SetFrame(0, 0f, new float[timeline2.attachment.vertices.Length]);
                return timeline2;
            }
            timeline2.SetFrame(0, 0f, timeline2.attachment.vertices.Clone() as float[]);
            return timeline2;
        }

        private static DrawOrderTimeline GetFillerTimeline(DrawOrderTimeline timeline, SkeletonData skeletonData)
        {
            DrawOrderTimeline timeline2 = new DrawOrderTimeline(1);
            timeline2.SetFrame(0, 0f, null);
            return timeline2;
        }

        private static IkConstraintTimeline GetFillerTimeline(IkConstraintTimeline timeline, SkeletonData skeletonData)
        {
            IkConstraintTimeline timeline2 = new IkConstraintTimeline(1);
            IkConstraintData data = skeletonData.ikConstraints.Items[timeline.ikConstraintIndex];
            timeline2.SetFrame(0, 0f, data.mix, data.bendDirection);
            return timeline2;
        }

        private static PathConstraintMixTimeline GetFillerTimeline(PathConstraintMixTimeline timeline, SkeletonData skeletonData)
        {
            PathConstraintMixTimeline timeline2 = new PathConstraintMixTimeline(1);
            PathConstraintData data = skeletonData.pathConstraints.Items[timeline.pathConstraintIndex];
            timeline2.SetFrame(0, 0f, data.rotateMix, data.translateMix);
            return timeline2;
        }

        private static PathConstraintPositionTimeline GetFillerTimeline(PathConstraintPositionTimeline timeline, SkeletonData skeletonData)
        {
            PathConstraintPositionTimeline timeline2 = new PathConstraintPositionTimeline(1);
            PathConstraintData data = skeletonData.pathConstraints.Items[timeline.pathConstraintIndex];
            timeline2.SetFrame(0, 0f, data.position);
            return timeline2;
        }

        private static PathConstraintSpacingTimeline GetFillerTimeline(PathConstraintSpacingTimeline timeline, SkeletonData skeletonData)
        {
            PathConstraintSpacingTimeline timeline2 = new PathConstraintSpacingTimeline(1);
            PathConstraintData data = skeletonData.pathConstraints.Items[timeline.pathConstraintIndex];
            timeline2.SetFrame(0, 0f, data.spacing);
            return timeline2;
        }

        private static RotateTimeline GetFillerTimeline(RotateTimeline timeline, SkeletonData skeletonData)
        {
            RotateTimeline timeline2 = new RotateTimeline(1) {
                boneIndex = timeline.boneIndex
            };
            timeline2.SetFrame(0, 0f, 0f);
            return timeline2;
        }

        private static ScaleTimeline GetFillerTimeline(ScaleTimeline timeline, SkeletonData skeletonData)
        {
            ScaleTimeline timeline2 = new ScaleTimeline(1) {
                boneIndex = timeline.boneIndex
            };
            timeline2.SetFrame(0, 0f, 0f, 0f);
            return timeline2;
        }

        private static ShearTimeline GetFillerTimeline(ShearTimeline timeline, SkeletonData skeletonData)
        {
            ShearTimeline timeline2 = new ShearTimeline(1) {
                boneIndex = timeline.boneIndex
            };
            timeline2.SetFrame(0, 0f, 0f, 0f);
            return timeline2;
        }

        private static Timeline GetFillerTimeline(Timeline timeline, SkeletonData skeletonData)
        {
            int num2 = timeline.PropertyId >> 0x18;
            switch (((TimelineType) num2))
            {
                case TimelineType.Rotate:
                    return GetFillerTimeline((RotateTimeline) timeline, skeletonData);

                case TimelineType.Translate:
                    return GetFillerTimeline((TranslateTimeline) timeline, skeletonData);

                case TimelineType.Scale:
                    return GetFillerTimeline((ScaleTimeline) timeline, skeletonData);

                case TimelineType.Shear:
                    return GetFillerTimeline((ShearTimeline) timeline, skeletonData);

                case TimelineType.Attachment:
                    return GetFillerTimeline((AttachmentTimeline) timeline, skeletonData);

                case TimelineType.Color:
                    return GetFillerTimeline((ColorTimeline) timeline, skeletonData);

                case TimelineType.Deform:
                    return GetFillerTimeline((DeformTimeline) timeline, skeletonData);

                case TimelineType.DrawOrder:
                    return GetFillerTimeline((DrawOrderTimeline) timeline, skeletonData);

                case TimelineType.IkConstraint:
                    return GetFillerTimeline((IkConstraintTimeline) timeline, skeletonData);

                case TimelineType.TransformConstraint:
                    return GetFillerTimeline((TransformConstraintTimeline) timeline, skeletonData);

                case TimelineType.PathConstraintPosition:
                    return GetFillerTimeline((PathConstraintPositionTimeline) timeline, skeletonData);

                case TimelineType.PathConstraintSpacing:
                    return GetFillerTimeline((PathConstraintSpacingTimeline) timeline, skeletonData);

                case TimelineType.PathConstraintMix:
                    return GetFillerTimeline((PathConstraintMixTimeline) timeline, skeletonData);

                case TimelineType.TwoColor:
                    return GetFillerTimeline((TwoColorTimeline) timeline, skeletonData);
            }
            return null;
        }

        private static TransformConstraintTimeline GetFillerTimeline(TransformConstraintTimeline timeline, SkeletonData skeletonData)
        {
            TransformConstraintTimeline timeline2 = new TransformConstraintTimeline(1);
            TransformConstraintData data = skeletonData.transformConstraints.Items[timeline.transformConstraintIndex];
            timeline2.SetFrame(0, 0f, data.rotateMix, data.translateMix, data.scaleMix, data.shearMix);
            return timeline2;
        }

        private static TranslateTimeline GetFillerTimeline(TranslateTimeline timeline, SkeletonData skeletonData)
        {
            TranslateTimeline timeline2 = new TranslateTimeline(1) {
                boneIndex = timeline.boneIndex
            };
            timeline2.SetFrame(0, 0f, 0f, 0f);
            return timeline2;
        }

        private static TwoColorTimeline GetFillerTimeline(TwoColorTimeline timeline, SkeletonData skeletonData)
        {
            TwoColorTimeline timeline2 = new TwoColorTimeline(1) {
                slotIndex = timeline.slotIndex
            };
            SlotData data = skeletonData.slots.Items[timeline2.slotIndex];
            timeline2.SetFrame(0, 0f, data.r, data.g, data.b, data.a, data.r2, data.g2, data.b2);
            return timeline2;
        }

        public static void MatchAnimationTimelines(IEnumerable<Animation> animations, SkeletonData skeletonData)
        {
            if (animations != null)
            {
                if (skeletonData == null)
                {
                    throw new ArgumentNullException("skeletonData", "Timelines can't be matched without a SkeletonData source.");
                }
                Dictionary<int, Timeline> dictionary = new Dictionary<int, Timeline>();
                foreach (Animation animation in animations)
                {
                    foreach (Timeline timeline in animation.timelines)
                    {
                        if (!(timeline is EventTimeline))
                        {
                            int propertyId = timeline.PropertyId;
                            if (!dictionary.ContainsKey(propertyId))
                            {
                                dictionary.Add(propertyId, GetFillerTimeline(timeline, skeletonData));
                            }
                        }
                    }
                }
                List<int> list = new List<int>(dictionary.Keys);
                HashSet<int> set = new HashSet<int>();
                foreach (Animation animation2 in animations)
                {
                    set.Clear();
                    foreach (Timeline timeline2 in animation2.timelines)
                    {
                        if (!(timeline2 is EventTimeline))
                        {
                            set.Add(timeline2.PropertyId);
                        }
                    }
                    ExposedList<Timeline> timelines = animation2.timelines;
                    foreach (int num2 in list)
                    {
                        if (!set.Contains(num2))
                        {
                            timelines.Add(dictionary[num2]);
                        }
                    }
                }
                dictionary.Clear();
                dictionary = null;
                list.Clear();
                list = null;
                set.Clear();
                set = null;
            }
        }
    }
}

