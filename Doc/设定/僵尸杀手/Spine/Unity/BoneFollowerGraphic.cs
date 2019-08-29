namespace Spine.Unity
{
    using Spine;
    using System;
    using UnityEngine;

    [ExecuteInEditMode, DisallowMultipleComponent, AddComponentMenu("Spine/UI/BoneFollowerGraphic")]
    public class BoneFollowerGraphic : MonoBehaviour
    {
        public Spine.Unity.SkeletonGraphic skeletonGraphic;
        public bool initializeOnAwake = true;
        [SpineBone("", "skeletonGraphic", true, false), SerializeField]
        public string boneName;
        public bool followBoneRotation = true;
        [Tooltip("Follows the skeleton's flip state by controlling this Transform's local scale.")]
        public bool followSkeletonFlip = true;
        [Tooltip("Follows the target bone's local scale. BoneFollower cannot inherit world/skewed scale because of UnityEngine.Transform property limitations.")]
        public bool followLocalScale;
        public bool followZPosition = true;
        [NonSerialized]
        public Bone bone;
        private Transform skeletonTransform;
        private bool skeletonTransformIsParent;
        [NonSerialized]
        public bool valid;

        public void Awake()
        {
            if (this.initializeOnAwake)
            {
                this.Initialize();
            }
        }

        public void Initialize()
        {
            this.bone = null;
            this.valid = (this.skeletonGraphic != null) && this.skeletonGraphic.IsValid;
            if (this.valid)
            {
                this.skeletonTransform = this.skeletonGraphic.transform;
                this.skeletonTransformIsParent = object.ReferenceEquals(this.skeletonTransform, base.transform.parent);
                if (!string.IsNullOrEmpty(this.boneName))
                {
                    this.bone = this.skeletonGraphic.Skeleton.FindBone(this.boneName);
                }
            }
        }

        public void LateUpdate()
        {
            if (!this.valid)
            {
                this.Initialize();
            }
            else
            {
                if (this.bone == null)
                {
                    if (string.IsNullOrEmpty(this.boneName))
                    {
                        return;
                    }
                    this.bone = this.skeletonGraphic.Skeleton.FindBone(this.boneName);
                    if (!this.SetBone(this.boneName))
                    {
                        return;
                    }
                }
                RectTransform transform = base.transform as RectTransform;
                if (transform != null)
                {
                    Canvas componentInParent = this.skeletonGraphic.get_canvas();
                    if (componentInParent == null)
                    {
                        componentInParent = this.skeletonGraphic.GetComponentInParent<Canvas>();
                    }
                    float referencePixelsPerUnit = componentInParent.referencePixelsPerUnit;
                    if (this.skeletonTransformIsParent)
                    {
                        transform.localPosition = new Vector3(this.bone.worldX * referencePixelsPerUnit, this.bone.worldY * referencePixelsPerUnit, !this.followZPosition ? transform.localPosition.z : 0f);
                        if (this.followBoneRotation)
                        {
                            transform.localRotation = this.bone.GetQuaternion();
                        }
                    }
                    else
                    {
                        Vector3 position = this.skeletonTransform.TransformPoint(new Vector3(this.bone.worldX * referencePixelsPerUnit, this.bone.worldY * referencePixelsPerUnit, 0f));
                        if (!this.followZPosition)
                        {
                            position.z = transform.position.z;
                        }
                        float worldRotationX = this.bone.WorldRotationX;
                        Transform parent = transform.parent;
                        if (parent != null)
                        {
                            Matrix4x4 localToWorldMatrix = parent.localToWorldMatrix;
                            if (((localToWorldMatrix.m00 * localToWorldMatrix.m11) - (localToWorldMatrix.m01 * localToWorldMatrix.m10)) < 0f)
                            {
                                worldRotationX = -worldRotationX;
                            }
                        }
                        if (this.followBoneRotation)
                        {
                            Vector3 eulerAngles = this.skeletonTransform.rotation.eulerAngles;
                            transform.SetPositionAndRotation(position, Quaternion.Euler(eulerAngles.x, eulerAngles.y, this.skeletonTransform.rotation.eulerAngles.z + worldRotationX));
                        }
                        else
                        {
                            transform.position = position;
                        }
                    }
                    Vector3 vector6 = !this.followLocalScale ? new Vector3(1f, 1f, 1f) : new Vector3(this.bone.scaleX, this.bone.scaleY, 1f);
                    if (this.followSkeletonFlip)
                    {
                        vector6.y *= !(this.bone.skeleton.flipX ^ this.bone.skeleton.flipY) ? 1f : -1f;
                    }
                    transform.localScale = vector6;
                }
            }
        }

        public bool SetBone(string name)
        {
            this.bone = this.skeletonGraphic.Skeleton.FindBone(name);
            if (this.bone == null)
            {
                Debug.LogError("Bone not found: " + name, this);
                return false;
            }
            this.boneName = name;
            return true;
        }

        public Spine.Unity.SkeletonGraphic SkeletonGraphic
        {
            get => 
                this.skeletonGraphic;
            set
            {
                this.skeletonGraphic = value;
                this.Initialize();
            }
        }
    }
}

