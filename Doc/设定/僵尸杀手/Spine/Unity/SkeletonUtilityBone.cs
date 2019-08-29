namespace Spine.Unity
{
    using Spine;
    using System;
    using UnityEngine;

    [ExecuteInEditMode, AddComponentMenu("Spine/SkeletonUtilityBone")]
    public class SkeletonUtilityBone : MonoBehaviour
    {
        public string boneName;
        public Transform parentReference;
        public Mode mode;
        public bool position;
        public bool rotation;
        public bool scale;
        public bool zPosition = true;
        [Range(0f, 1f)]
        public float overrideAlpha = 1f;
        [NonSerialized]
        public SkeletonUtility skeletonUtility;
        [NonSerialized]
        public Bone bone;
        [NonSerialized]
        public bool transformLerpComplete;
        [NonSerialized]
        public bool valid;
        private Transform cachedTransform;
        private Transform skeletonTransform;
        private bool incompatibleTransformMode;

        public void AddBoundingBox(string skinName, string slotName, string attachmentName)
        {
            SkeletonUtility.AddBoundingBoxGameObject(this.bone.skeleton, skinName, slotName, attachmentName, base.transform, true);
        }

        public static bool BoneTransformModeIncompatible(Bone bone) => 
            !bone.data.transformMode.InheritsScale();

        public void DoUpdate(UpdatePhase phase)
        {
            if (!this.valid)
            {
                this.Reset();
            }
            else
            {
                Skeleton skeleton = this.skeletonUtility.skeletonRenderer.skeleton;
                if (this.bone == null)
                {
                    if (string.IsNullOrEmpty(this.boneName))
                    {
                        return;
                    }
                    this.bone = skeleton.FindBone(this.boneName);
                    if (this.bone == null)
                    {
                        Debug.LogError("Bone not found: " + this.boneName, this);
                        return;
                    }
                }
                Transform cachedTransform = this.cachedTransform;
                float num = !(skeleton.flipX ^ skeleton.flipY) ? 1f : -1f;
                if (this.mode != Mode.Follow)
                {
                    if ((this.mode == Mode.Override) && !this.transformLerpComplete)
                    {
                        if (this.parentReference == null)
                        {
                            if (this.position)
                            {
                                Vector3 localPosition = cachedTransform.localPosition;
                                this.bone.x = Mathf.Lerp(this.bone.x, localPosition.x, this.overrideAlpha);
                                this.bone.y = Mathf.Lerp(this.bone.y, localPosition.y, this.overrideAlpha);
                            }
                            if (this.rotation)
                            {
                                float num2 = Mathf.LerpAngle(this.bone.Rotation, cachedTransform.localRotation.eulerAngles.z, this.overrideAlpha);
                                this.bone.Rotation = num2;
                                this.bone.AppliedRotation = num2;
                            }
                            if (this.scale)
                            {
                                Vector3 localScale = cachedTransform.localScale;
                                this.bone.scaleX = Mathf.Lerp(this.bone.scaleX, localScale.x, this.overrideAlpha);
                                this.bone.scaleY = Mathf.Lerp(this.bone.scaleY, localScale.y, this.overrideAlpha);
                            }
                        }
                        else
                        {
                            if (this.transformLerpComplete)
                            {
                                return;
                            }
                            if (this.position)
                            {
                                Vector3 vector6 = this.parentReference.InverseTransformPoint(cachedTransform.position);
                                this.bone.x = Mathf.Lerp(this.bone.x, vector6.x, this.overrideAlpha);
                                this.bone.y = Mathf.Lerp(this.bone.y, vector6.y, this.overrideAlpha);
                            }
                            if (this.rotation)
                            {
                                float num3 = Mathf.LerpAngle(this.bone.Rotation, Quaternion.LookRotation(Vector3.forward, this.parentReference.InverseTransformDirection(cachedTransform.up)).eulerAngles.z, this.overrideAlpha);
                                this.bone.Rotation = num3;
                                this.bone.AppliedRotation = num3;
                            }
                            if (this.scale)
                            {
                                Vector3 localScale = cachedTransform.localScale;
                                this.bone.scaleX = Mathf.Lerp(this.bone.scaleX, localScale.x, this.overrideAlpha);
                                this.bone.scaleY = Mathf.Lerp(this.bone.scaleY, localScale.y, this.overrideAlpha);
                            }
                            this.incompatibleTransformMode = BoneTransformModeIncompatible(this.bone);
                        }
                        this.transformLerpComplete = true;
                    }
                }
                else if (phase != UpdatePhase.Local)
                {
                    if (((phase == UpdatePhase.World) || (phase == UpdatePhase.Complete)) && !this.bone.appliedValid)
                    {
                        this.bone.UpdateAppliedTransform();
                        if (this.position)
                        {
                            cachedTransform.localPosition = new Vector3(this.bone.ax, this.bone.ay, 0f);
                        }
                        if (this.rotation)
                        {
                            if (this.bone.data.transformMode.InheritsRotation())
                            {
                                cachedTransform.localRotation = Quaternion.Euler(0f, 0f, this.bone.AppliedRotation);
                            }
                            else
                            {
                                Vector3 eulerAngles = this.skeletonTransform.rotation.eulerAngles;
                                cachedTransform.rotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, eulerAngles.z + (this.bone.WorldRotationX * num));
                            }
                        }
                        if (this.scale)
                        {
                            cachedTransform.localScale = new Vector3(this.bone.ascaleX, this.bone.ascaleY, 1f);
                            this.incompatibleTransformMode = BoneTransformModeIncompatible(this.bone);
                        }
                    }
                }
                else
                {
                    if (this.position)
                    {
                        cachedTransform.localPosition = new Vector3(this.bone.x, this.bone.y, 0f);
                    }
                    if (this.rotation)
                    {
                        if (this.bone.data.transformMode.InheritsRotation())
                        {
                            cachedTransform.localRotation = Quaternion.Euler(0f, 0f, this.bone.rotation);
                        }
                        else
                        {
                            Vector3 eulerAngles = this.skeletonTransform.rotation.eulerAngles;
                            cachedTransform.rotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, eulerAngles.z + (this.bone.WorldRotationX * num));
                        }
                    }
                    if (this.scale)
                    {
                        cachedTransform.localScale = new Vector3(this.bone.scaleX, this.bone.scaleY, 1f);
                        this.incompatibleTransformMode = BoneTransformModeIncompatible(this.bone);
                    }
                }
            }
        }

        private void HandleOnReset()
        {
            this.Reset();
        }

        private void OnDisable()
        {
            if (this.skeletonUtility != null)
            {
                this.skeletonUtility.OnReset -= new SkeletonUtility.SkeletonUtilityDelegate(this.HandleOnReset);
                this.skeletonUtility.UnregisterBone(this);
            }
        }

        private void OnEnable()
        {
            this.skeletonUtility = base.transform.GetComponentInParent<SkeletonUtility>();
            if (this.skeletonUtility != null)
            {
                this.skeletonUtility.RegisterBone(this);
                this.skeletonUtility.OnReset += new SkeletonUtility.SkeletonUtilityDelegate(this.HandleOnReset);
            }
        }

        public void Reset()
        {
            this.bone = null;
            this.cachedTransform = base.transform;
            this.valid = ((this.skeletonUtility != null) && (this.skeletonUtility.skeletonRenderer != null)) && this.skeletonUtility.skeletonRenderer.valid;
            if (this.valid)
            {
                this.skeletonTransform = this.skeletonUtility.transform;
                this.skeletonUtility.OnReset -= new SkeletonUtility.SkeletonUtilityDelegate(this.HandleOnReset);
                this.skeletonUtility.OnReset += new SkeletonUtility.SkeletonUtilityDelegate(this.HandleOnReset);
                this.DoUpdate(UpdatePhase.Local);
            }
        }

        public bool IncompatibleTransformMode =>
            this.incompatibleTransformMode;

        public enum Mode
        {
            Follow,
            Override
        }

        public enum UpdatePhase
        {
            Local,
            World,
            Complete
        }
    }
}

