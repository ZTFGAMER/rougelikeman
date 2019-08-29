namespace Spine.Unity
{
    using Spine;
    using System;
    using UnityEngine;
    using UnityEngine.Serialization;

    [ExecuteInEditMode, AddComponentMenu("Spine/BoneFollower")]
    public class BoneFollower : MonoBehaviour
    {
        public Spine.Unity.SkeletonRenderer skeletonRenderer;
        [SpineBone("", "skeletonRenderer", true, false), SerializeField]
        public string boneName;
        public bool followZPosition = true;
        public bool followBoneRotation = true;
        [Tooltip("Follows the skeleton's flip state by controlling this Transform's local scale.")]
        public bool followSkeletonFlip = true;
        [Tooltip("Follows the target bone's local scale. BoneFollower cannot inherit world/skewed scale because of UnityEngine.Transform property limitations.")]
        public bool followLocalScale;
        [FormerlySerializedAs("resetOnAwake")]
        public bool initializeOnAwake = true;
        [NonSerialized]
        public bool valid;
        [NonSerialized]
        public Bone bone;
        private Transform skeletonTransform;
        private bool skeletonTransformIsParent;

        public void Awake()
        {
            if (this.initializeOnAwake)
            {
                this.Initialize();
            }
        }

        public void HandleRebuildRenderer(Spine.Unity.SkeletonRenderer skeletonRenderer)
        {
            this.Initialize();
        }

        public void Initialize()
        {
            this.bone = null;
            this.valid = (this.skeletonRenderer != null) && this.skeletonRenderer.valid;
            if (this.valid)
            {
                this.skeletonTransform = this.skeletonRenderer.transform;
                this.skeletonRenderer.OnRebuild -= new Spine.Unity.SkeletonRenderer.SkeletonRendererDelegate(this.HandleRebuildRenderer);
                this.skeletonRenderer.OnRebuild += new Spine.Unity.SkeletonRenderer.SkeletonRendererDelegate(this.HandleRebuildRenderer);
                this.skeletonTransformIsParent = object.ReferenceEquals(this.skeletonTransform, base.transform.parent);
                if (!string.IsNullOrEmpty(this.boneName))
                {
                    this.bone = this.skeletonRenderer.skeleton.FindBone(this.boneName);
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
                    this.bone = this.skeletonRenderer.skeleton.FindBone(this.boneName);
                    if (!this.SetBone(this.boneName))
                    {
                        return;
                    }
                }
                Transform transform = base.transform;
                if (this.skeletonTransformIsParent)
                {
                    transform.localPosition = new Vector3(this.bone.worldX, this.bone.worldY, !this.followZPosition ? transform.localPosition.z : 0f);
                    if (this.followBoneRotation)
                    {
                        float f = Mathf.Atan2(this.bone.c, this.bone.a) * 0.5f;
                        if (this.followLocalScale && (this.bone.scaleX < 0f))
                        {
                            f += 1.570796f;
                        }
                        Quaternion quaternion = new Quaternion {
                            z = Mathf.Sin(f),
                            w = Mathf.Cos(f)
                        };
                        transform.localRotation = quaternion;
                    }
                }
                else
                {
                    Vector3 position = this.skeletonTransform.TransformPoint(new Vector3(this.bone.worldX, this.bone.worldY, 0f));
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
                        if (this.followLocalScale && (this.bone.scaleX < 0f))
                        {
                            worldRotationX += 180f;
                        }
                        transform.SetPositionAndRotation(position, Quaternion.Euler(eulerAngles.x, eulerAngles.y, eulerAngles.z + worldRotationX));
                    }
                    else
                    {
                        transform.position = position;
                    }
                }
                Vector3 vector5 = !this.followLocalScale ? new Vector3(1f, 1f, 1f) : new Vector3(this.bone.scaleX, this.bone.scaleY, 1f);
                if (this.followSkeletonFlip)
                {
                    vector5.y *= !(this.bone.skeleton.flipX ^ this.bone.skeleton.flipY) ? 1f : -1f;
                }
                transform.localScale = vector5;
            }
        }

        private void OnDestroy()
        {
            if (this.skeletonRenderer != null)
            {
                this.skeletonRenderer.OnRebuild -= new Spine.Unity.SkeletonRenderer.SkeletonRendererDelegate(this.HandleRebuildRenderer);
            }
        }

        public bool SetBone(string name)
        {
            this.bone = this.skeletonRenderer.skeleton.FindBone(name);
            if (this.bone == null)
            {
                Debug.LogError("Bone not found: " + name, this);
                return false;
            }
            this.boneName = name;
            return true;
        }

        public Spine.Unity.SkeletonRenderer SkeletonRenderer
        {
            get => 
                this.skeletonRenderer;
            set
            {
                this.skeletonRenderer = value;
                this.Initialize();
            }
        }
    }
}

