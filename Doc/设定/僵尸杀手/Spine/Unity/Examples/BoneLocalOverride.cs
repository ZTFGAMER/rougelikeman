namespace Spine.Unity.Examples
{
    using Spine;
    using Spine.Unity;
    using System;
    using UnityEngine;

    public class BoneLocalOverride : MonoBehaviour
    {
        [SpineBone("", "", true, false)]
        public string boneName;
        [Space, Range(0f, 1f)]
        public float alpha = 1f;
        [Space]
        public bool overridePosition = true;
        public Vector2 localPosition;
        [Space]
        public bool overrideRotation = true;
        [Range(0f, 360f)]
        public float rotation;
        private ISkeletonAnimation spineComponent;
        private Bone bone;

        private void Awake()
        {
            this.spineComponent = base.GetComponent<ISkeletonAnimation>();
            if (this.spineComponent == null)
            {
                base.enabled = false;
            }
            else
            {
                this.spineComponent.UpdateLocal += new UpdateBonesDelegate(this.OverrideLocal);
                if (this.bone == null)
                {
                    base.enabled = false;
                }
            }
        }

        private void OverrideLocal(ISkeletonAnimation animated)
        {
            if ((this.bone == null) || (this.bone.Data.Name != this.boneName))
            {
                if (string.IsNullOrEmpty(this.boneName))
                {
                    return;
                }
                this.bone = this.spineComponent.Skeleton.FindBone(this.boneName);
                if (this.bone == null)
                {
                    object[] args = new object[] { this.boneName };
                    Debug.LogFormat("Cannot find bone: '{0}'", args);
                    return;
                }
            }
            if (this.overridePosition)
            {
                this.bone.X = Mathf.Lerp(this.bone.X, this.localPosition.x, this.alpha);
                this.bone.Y = Mathf.Lerp(this.bone.Y, this.localPosition.y, this.alpha);
            }
            if (this.overrideRotation)
            {
                this.bone.Rotation = Mathf.Lerp(this.bone.Rotation, this.rotation, this.alpha);
            }
        }
    }
}

