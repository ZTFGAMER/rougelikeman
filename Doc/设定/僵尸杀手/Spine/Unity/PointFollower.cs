namespace Spine.Unity
{
    using Spine;
    using System;
    using UnityEngine;

    [ExecuteInEditMode, AddComponentMenu("Spine/Point Follower")]
    public class PointFollower : MonoBehaviour, IHasSkeletonRenderer, IHasSkeletonComponent
    {
        [SerializeField]
        public Spine.Unity.SkeletonRenderer skeletonRenderer;
        [SpineSlot("", "skeletonRenderer", false, true, false)]
        public string slotName;
        [SpineAttachment(true, false, false, "slotName", "skeletonRenderer", "", true, true)]
        public string pointAttachmentName;
        public bool followRotation = true;
        public bool followSkeletonFlip = true;
        public bool followSkeletonZPosition;
        private Transform skeletonTransform;
        private bool skeletonTransformIsParent;
        private PointAttachment point;
        private Bone bone;
        private bool valid;

        private void HandleRebuildRenderer(Spine.Unity.SkeletonRenderer skeletonRenderer)
        {
            this.Initialize();
        }

        public void Initialize()
        {
            this.valid = (this.skeletonRenderer != null) && this.skeletonRenderer.valid;
            if (this.valid)
            {
                this.UpdateReferences();
            }
        }

        public void LateUpdate()
        {
            Vector2 vector;
            if (this.point == null)
            {
                if (string.IsNullOrEmpty(this.pointAttachmentName))
                {
                    return;
                }
                this.UpdateReferences();
                if (this.point == null)
                {
                    return;
                }
            }
            this.point.ComputeWorldPosition(this.bone, out vector.x, out vector.y);
            float num = this.point.ComputeWorldRotation(this.bone);
            Transform transform = base.transform;
            if (this.skeletonTransformIsParent)
            {
                transform.localPosition = new Vector3(vector.x, vector.y, !this.followSkeletonZPosition ? transform.localPosition.z : 0f);
                if (this.followRotation)
                {
                    float f = (num * 0.5f) * 0.01745329f;
                    Quaternion quaternion = new Quaternion {
                        z = Mathf.Sin(f),
                        w = Mathf.Cos(f)
                    };
                    transform.localRotation = quaternion;
                }
            }
            else
            {
                Vector3 position = this.skeletonTransform.TransformPoint(new Vector3(vector.x, vector.y, 0f));
                if (!this.followSkeletonZPosition)
                {
                    position.z = transform.position.z;
                }
                Transform parent = transform.parent;
                if (parent != null)
                {
                    Matrix4x4 localToWorldMatrix = parent.localToWorldMatrix;
                    if (((localToWorldMatrix.m00 * localToWorldMatrix.m11) - (localToWorldMatrix.m01 * localToWorldMatrix.m10)) < 0f)
                    {
                        num = -num;
                    }
                }
                if (this.followRotation)
                {
                    Vector3 eulerAngles = this.skeletonTransform.rotation.eulerAngles;
                    transform.SetPositionAndRotation(position, Quaternion.Euler(eulerAngles.x, eulerAngles.y, eulerAngles.z + num));
                }
                else
                {
                    transform.position = position;
                }
            }
            if (this.followSkeletonFlip)
            {
                Vector3 localScale = transform.localScale;
                localScale.y = Mathf.Abs(localScale.y) * (!(this.bone.skeleton.flipX ^ this.bone.skeleton.flipY) ? 1f : -1f);
                transform.localScale = localScale;
            }
        }

        private void UpdateReferences()
        {
            this.skeletonTransform = this.skeletonRenderer.transform;
            this.skeletonRenderer.OnRebuild -= new Spine.Unity.SkeletonRenderer.SkeletonRendererDelegate(this.HandleRebuildRenderer);
            this.skeletonRenderer.OnRebuild += new Spine.Unity.SkeletonRenderer.SkeletonRendererDelegate(this.HandleRebuildRenderer);
            this.skeletonTransformIsParent = object.ReferenceEquals(this.skeletonTransform, base.transform.parent);
            this.bone = null;
            this.point = null;
            if (!string.IsNullOrEmpty(this.pointAttachmentName))
            {
                Skeleton skeleton = this.skeletonRenderer.skeleton;
                int index = skeleton.FindSlotIndex(this.slotName);
                if (index >= 0)
                {
                    Slot slot = skeleton.slots.Items[index];
                    this.bone = slot.bone;
                    this.point = skeleton.GetAttachment(index, this.pointAttachmentName) as PointAttachment;
                }
            }
        }

        public Spine.Unity.SkeletonRenderer SkeletonRenderer =>
            this.skeletonRenderer;

        public ISkeletonComponent SkeletonComponent =>
            this.skeletonRenderer;

        public bool IsValid =>
            this.valid;
    }
}

