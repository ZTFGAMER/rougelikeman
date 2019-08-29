namespace Spine.Unity.Examples
{
    using Spine;
    using Spine.Unity;
    using System;
    using UnityEngine;

    [ExecuteInEditMode, RequireComponent(typeof(SkeletonRenderer))]
    public class SpineGauge : MonoBehaviour
    {
        [Range(0f, 1f)]
        public float fillPercent;
        public AnimationReferenceAsset fillAnimation;
        private SkeletonRenderer skeletonRenderer;

        private void Awake()
        {
            this.skeletonRenderer = base.GetComponent<SkeletonRenderer>();
        }

        public void SetGaugePercent(float percent)
        {
            if (this.skeletonRenderer != null)
            {
                Skeleton skeleton = this.skeletonRenderer.skeleton;
                if (skeleton != null)
                {
                    this.fillAnimation.Animation.Apply(skeleton, 0f, percent, false, null, 1f, MixPose.Setup, MixDirection.In);
                    skeleton.Update(Time.deltaTime);
                    skeleton.UpdateWorldTransform();
                }
            }
        }

        private void Update()
        {
            this.SetGaugePercent(this.fillPercent);
        }
    }
}

