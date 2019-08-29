namespace Spine.Unity.Examples
{
    using Spine;
    using Spine.Unity;
    using System;
    using UnityEngine;

    public class Goblins : MonoBehaviour
    {
        private SkeletonAnimation skeletonAnimation;
        private Bone headBone;
        private bool girlSkin;
        [Range(-360f, 360f)]
        public float extraRotation;

        public void OnMouseDown()
        {
            this.skeletonAnimation.Skeleton.SetSkin(!this.girlSkin ? "goblingirl" : "goblin");
            this.skeletonAnimation.Skeleton.SetSlotsToSetupPose();
            this.girlSkin = !this.girlSkin;
            if (this.girlSkin)
            {
                this.skeletonAnimation.Skeleton.SetAttachment("right hand item", null);
                this.skeletonAnimation.Skeleton.SetAttachment("left hand item", "spear");
            }
            else
            {
                this.skeletonAnimation.Skeleton.SetAttachment("left hand item", "dagger");
            }
        }

        public void Start()
        {
            this.skeletonAnimation = base.GetComponent<SkeletonAnimation>();
            this.headBone = this.skeletonAnimation.Skeleton.FindBone("head");
            this.skeletonAnimation.UpdateLocal += new UpdateBonesDelegate(this.UpdateLocal);
        }

        public void UpdateLocal(ISkeletonAnimation skeletonRenderer)
        {
            this.headBone.Rotation += this.extraRotation;
        }
    }
}

