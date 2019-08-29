namespace Spine.Unity.Modules
{
    using Spine;
    using Spine.Unity;
    using System;
    using UnityEngine;

    public class SkeletonGraphicMirror : MonoBehaviour
    {
        public SkeletonRenderer source;
        public bool mirrorOnStart = true;
        public bool restoreOnDisable = true;
        private SkeletonGraphic skeletonGraphic;
        private Skeleton originalSkeleton;
        private bool originalFreeze;
        private Texture2D overrideTexture;

        private void Awake()
        {
            this.skeletonGraphic = base.GetComponent<SkeletonGraphic>();
        }

        private void LateUpdate()
        {
            this.skeletonGraphic.UpdateMesh();
        }

        private void OnDisable()
        {
            if (this.restoreOnDisable)
            {
                this.RestoreIndependentSkeleton();
            }
        }

        public void RestoreIndependentSkeleton()
        {
            if (this.originalSkeleton != null)
            {
                this.skeletonGraphic.Skeleton = this.originalSkeleton;
                this.skeletonGraphic.freeze = this.originalFreeze;
                this.skeletonGraphic.OverrideTexture = null;
                this.originalSkeleton = null;
            }
        }

        private void Start()
        {
            if (this.mirrorOnStart)
            {
                this.StartMirroring();
            }
        }

        public void StartMirroring()
        {
            if ((this.source != null) && (this.skeletonGraphic != null))
            {
                this.skeletonGraphic.startingAnimation = string.Empty;
                if (this.originalSkeleton == null)
                {
                    this.originalSkeleton = this.skeletonGraphic.Skeleton;
                    this.originalFreeze = this.skeletonGraphic.freeze;
                }
                this.skeletonGraphic.Skeleton = this.source.skeleton;
                this.skeletonGraphic.freeze = true;
                if (this.overrideTexture != null)
                {
                    this.skeletonGraphic.OverrideTexture = this.overrideTexture;
                }
            }
        }

        public void UpdateTexture(Texture2D newOverrideTexture)
        {
            this.overrideTexture = newOverrideTexture;
            if (newOverrideTexture != null)
            {
                this.skeletonGraphic.OverrideTexture = this.overrideTexture;
            }
        }
    }
}

