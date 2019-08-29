namespace Spine.Unity
{
    using System;
    using UnityEngine;

    [RequireComponent(typeof(SkeletonUtilityBone)), ExecuteInEditMode]
    public abstract class SkeletonUtilityConstraint : MonoBehaviour
    {
        protected SkeletonUtilityBone utilBone;
        protected SkeletonUtility skeletonUtility;

        protected SkeletonUtilityConstraint()
        {
        }

        public abstract void DoUpdate();
        protected virtual void OnDisable()
        {
            this.skeletonUtility.UnregisterConstraint(this);
        }

        protected virtual void OnEnable()
        {
            this.utilBone = base.GetComponent<SkeletonUtilityBone>();
            this.skeletonUtility = base.transform.GetComponentInParent<SkeletonUtility>();
            this.skeletonUtility.RegisterConstraint(this);
        }
    }
}

