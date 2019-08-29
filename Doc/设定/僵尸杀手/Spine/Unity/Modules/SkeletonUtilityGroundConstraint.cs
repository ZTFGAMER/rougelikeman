namespace Spine.Unity.Modules
{
    using Spine.Unity;
    using System;
    using UnityEngine;

    [RequireComponent(typeof(SkeletonUtilityBone)), ExecuteInEditMode]
    public class SkeletonUtilityGroundConstraint : SkeletonUtilityConstraint
    {
        [Tooltip("LayerMask for what objects to raycast against")]
        public LayerMask groundMask;
        [Tooltip("Use 2D")]
        public bool use2D;
        [Tooltip("Uses SphereCast for 3D mode and CircleCast for 2D mode")]
        public bool useRadius;
        [Tooltip("The Radius")]
        public float castRadius = 0.1f;
        [Tooltip("How high above the target bone to begin casting from")]
        public float castDistance = 5f;
        [Tooltip("X-Axis adjustment")]
        public float castOffset;
        [Tooltip("Y-Axis adjustment")]
        public float groundOffset;
        [Tooltip("How fast the target IK position adjusts to the ground.  Use smaller values to prevent snapping")]
        public float adjustSpeed = 5f;
        private Vector3 rayOrigin;
        private Vector3 rayDir = new Vector3(0f, -1f, 0f);
        private float hitY;
        private float lastHitY;

        public override void DoUpdate()
        {
            this.rayOrigin = base.transform.position + new Vector3(this.castOffset, this.castDistance, 0f);
            this.hitY = float.MinValue;
            if (this.use2D)
            {
                RaycastHit2D hitd;
                if (this.useRadius)
                {
                    hitd = Physics2D.CircleCast(this.rayOrigin, this.castRadius, this.rayDir, this.castDistance + this.groundOffset, (int) this.groundMask);
                }
                else
                {
                    hitd = Physics2D.Raycast(this.rayOrigin, this.rayDir, this.castDistance + this.groundOffset, (int) this.groundMask);
                }
                if (hitd.collider != null)
                {
                    this.hitY = hitd.point.y + this.groundOffset;
                    if (Application.isPlaying)
                    {
                        this.hitY = Mathf.MoveTowards(this.lastHitY, this.hitY, this.adjustSpeed * Time.deltaTime);
                    }
                }
                else if (Application.isPlaying)
                {
                    this.hitY = Mathf.MoveTowards(this.lastHitY, base.transform.position.y, this.adjustSpeed * Time.deltaTime);
                }
            }
            else
            {
                bool flag = false;
                if (this.useRadius)
                {
                    flag = Physics.SphereCast(this.rayOrigin, this.castRadius, this.rayDir, out RaycastHit hit, this.castDistance + this.groundOffset, (int) this.groundMask);
                }
                else
                {
                    flag = Physics.Raycast(this.rayOrigin, this.rayDir, out hit, this.castDistance + this.groundOffset, (int) this.groundMask);
                }
                if (flag)
                {
                    this.hitY = hit.point.y + this.groundOffset;
                    if (Application.isPlaying)
                    {
                        this.hitY = Mathf.MoveTowards(this.lastHitY, this.hitY, this.adjustSpeed * Time.deltaTime);
                    }
                }
                else if (Application.isPlaying)
                {
                    this.hitY = Mathf.MoveTowards(this.lastHitY, base.transform.position.y, this.adjustSpeed * Time.deltaTime);
                }
            }
            Vector3 position = base.transform.position;
            position.y = Mathf.Clamp(position.y, Mathf.Min(this.lastHitY, this.hitY), float.MaxValue);
            base.transform.position = position;
            base.utilBone.bone.X = base.transform.localPosition.x;
            base.utilBone.bone.Y = base.transform.localPosition.y;
            this.lastHitY = this.hitY;
        }

        private void OnDrawGizmos()
        {
            Vector3 to = this.rayOrigin + (this.rayDir * Mathf.Min(this.castDistance, this.rayOrigin.y - this.hitY));
            Vector3 vector2 = this.rayOrigin + (this.rayDir * this.castDistance);
            Gizmos.DrawLine(this.rayOrigin, to);
            if (this.useRadius)
            {
                Gizmos.DrawLine(new Vector3(to.x - this.castRadius, to.y - this.groundOffset, to.z), new Vector3(to.x + this.castRadius, to.y - this.groundOffset, to.z));
                Gizmos.DrawLine(new Vector3(vector2.x - this.castRadius, vector2.y, vector2.z), new Vector3(vector2.x + this.castRadius, vector2.y, vector2.z));
            }
            Gizmos.color = Color.red;
            Gizmos.DrawLine(to, vector2);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            this.lastHitY = base.transform.position.y;
        }
    }
}

