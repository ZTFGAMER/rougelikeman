namespace Spine.Unity
{
    using Spine;
    using System;
    using UnityEngine;

    [CreateAssetMenu(menuName="Spine/Animation Reference Asset")]
    public class AnimationReferenceAsset : ScriptableObject, IHasSkeletonDataAsset
    {
        private const bool QuietSkeletonData = true;
        [SerializeField]
        protected Spine.Unity.SkeletonDataAsset skeletonDataAsset;
        [SerializeField, SpineAnimation("", "", true, false)]
        protected string animationName;
        private Spine.Animation animation;

        public void Initialize()
        {
            if (this.skeletonDataAsset != null)
            {
                this.animation = this.skeletonDataAsset.GetSkeletonData(true).FindAnimation(this.animationName);
                if (this.animation == null)
                {
                    object[] args = new object[] { this.animationName, this.skeletonDataAsset.name };
                    Debug.LogWarningFormat("Animation '{0}' not found in SkeletonData : {1}.", args);
                }
            }
        }

        public static implicit operator Spine.Animation(AnimationReferenceAsset asset) => 
            asset.Animation;

        public Spine.Unity.SkeletonDataAsset SkeletonDataAsset =>
            this.skeletonDataAsset;

        public Spine.Animation Animation
        {
            get
            {
                if (this.animation == null)
                {
                    this.Initialize();
                }
                return this.animation;
            }
        }
    }
}

