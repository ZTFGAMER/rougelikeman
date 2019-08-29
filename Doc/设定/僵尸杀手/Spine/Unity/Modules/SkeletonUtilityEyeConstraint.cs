namespace Spine.Unity.Modules
{
    using Spine.Unity;
    using System;
    using UnityEngine;

    public class SkeletonUtilityEyeConstraint : SkeletonUtilityConstraint
    {
        public Transform[] eyes;
        public float radius = 0.5f;
        public Transform target;
        public Vector3 targetPosition;
        public float speed = 10f;
        private Vector3[] origins;
        private Vector3 centerPoint;

        public override void DoUpdate()
        {
            if (this.target != null)
            {
                this.targetPosition = this.target.position;
            }
            Vector3 targetPosition = this.targetPosition;
            Vector3 vector2 = base.transform.TransformPoint(this.centerPoint);
            Vector3 vector3 = targetPosition - vector2;
            if (vector3.magnitude > 1f)
            {
                vector3.Normalize();
            }
            for (int i = 0; i < this.eyes.Length; i++)
            {
                vector2 = base.transform.TransformPoint(this.origins[i]);
                this.eyes[i].position = Vector3.MoveTowards(this.eyes[i].position, vector2 + (vector3 * this.radius), this.speed * Time.deltaTime);
            }
        }

        protected override void OnDisable()
        {
            if (Application.isPlaying)
            {
                base.OnDisable();
            }
        }

        protected override void OnEnable()
        {
            if (Application.isPlaying)
            {
                base.OnEnable();
                Bounds bounds = new Bounds(this.eyes[0].localPosition, Vector3.zero);
                this.origins = new Vector3[this.eyes.Length];
                for (int i = 0; i < this.eyes.Length; i++)
                {
                    this.origins[i] = this.eyes[i].localPosition;
                    bounds.Encapsulate(this.origins[i]);
                }
                this.centerPoint = bounds.center;
            }
        }
    }
}

