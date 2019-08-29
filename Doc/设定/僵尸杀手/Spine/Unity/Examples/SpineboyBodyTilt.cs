namespace Spine.Unity.Examples
{
    using Spine;
    using Spine.Unity;
    using System;
    using UnityEngine;

    public class SpineboyBodyTilt : MonoBehaviour
    {
        [Header("Settings")]
        public SpineboyFootplanter planter;
        [SpineBone("", "", true, false)]
        public string hip = "hip";
        [SpineBone("", "", true, false)]
        public string head = "head";
        public float hipTiltScale = 7f;
        public float headTiltScale = 0.7f;
        public float hipRotationMoveScale = 60f;
        [Header("Debug")]
        public float hipRotationTarget;
        public float hipRotationSmoothed;
        public float baseHeadRotation;
        private Bone hipBone;
        private Bone headBone;

        private void Start()
        {
            SkeletonAnimation component = base.GetComponent<SkeletonAnimation>();
            Skeleton skeleton = component.Skeleton;
            this.hipBone = skeleton.FindBone(this.hip);
            this.headBone = skeleton.FindBone(this.head);
            this.baseHeadRotation = this.headBone.Rotation;
            component.UpdateLocal += new UpdateBonesDelegate(this.UpdateLocal);
        }

        private void UpdateLocal(ISkeletonAnimation animated)
        {
            this.hipRotationTarget = this.planter.Balance * this.hipTiltScale;
            this.hipRotationSmoothed = Mathf.MoveTowards(this.hipRotationSmoothed, this.hipRotationTarget, (Time.deltaTime * this.hipRotationMoveScale) * Mathf.Abs((float) ((2f * this.planter.Balance) / this.planter.offBalanceThreshold)));
            this.hipBone.Rotation = this.hipRotationSmoothed;
            this.headBone.Rotation = this.baseHeadRotation + (-this.hipRotationSmoothed * this.headTiltScale);
        }
    }
}

