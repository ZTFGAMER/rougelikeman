namespace Spine.Unity.Examples
{
    using Spine;
    using Spine.Unity;
    using System;
    using UnityEngine;

    public class SpineboyTargetController : MonoBehaviour
    {
        public SkeletonAnimation skeletonAnimation;
        [SpineBone("", "skeletonAnimation", true, false)]
        public string boneName;
        public Camera camera;
        private Bone bone;

        private void OnValidate()
        {
            if (this.skeletonAnimation == null)
            {
                this.skeletonAnimation = base.GetComponent<SkeletonAnimation>();
            }
        }

        private void Start()
        {
            this.bone = this.skeletonAnimation.Skeleton.FindBone(this.boneName);
        }

        private void Update()
        {
            Vector3 mousePosition = Input.mousePosition;
            Vector3 position = this.camera.ScreenToWorldPoint(mousePosition);
            Vector3 vector3 = this.skeletonAnimation.transform.InverseTransformPoint(position);
            if (this.skeletonAnimation.Skeleton.FlipX)
            {
                vector3.x *= -1f;
            }
            this.bone.SetPosition(vector3);
        }
    }
}

