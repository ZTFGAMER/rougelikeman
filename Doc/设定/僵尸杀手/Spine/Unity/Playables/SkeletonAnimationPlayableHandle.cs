namespace Spine.Unity.Playables
{
    using Spine;
    using Spine.Unity;
    using System;
    using UnityEngine;

    [AddComponentMenu("Spine/Playables/SkeletonAnimation Playable Handle (Playables)")]
    public class SkeletonAnimationPlayableHandle : SpinePlayableHandleBase
    {
        public SkeletonAnimation skeletonAnimation;

        private void Awake()
        {
            if (this.skeletonAnimation == null)
            {
                this.skeletonAnimation = base.GetComponent<SkeletonAnimation>();
            }
        }

        public override Spine.Skeleton Skeleton =>
            this.skeletonAnimation.Skeleton;

        public override Spine.SkeletonData SkeletonData =>
            this.skeletonAnimation.Skeleton.data;
    }
}

