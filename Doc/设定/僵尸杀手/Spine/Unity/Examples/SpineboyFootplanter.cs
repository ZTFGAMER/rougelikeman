namespace Spine.Unity.Examples
{
    using Spine;
    using Spine.Unity;
    using System;
    using UnityEngine;

    public class SpineboyFootplanter : MonoBehaviour
    {
        public float timeScale = 0.5f;
        [SpineBone("", "", true, false)]
        public string nearBoneName;
        [SpineBone("", "", true, false)]
        public string farBoneName;
        [Header("Settings")]
        public Vector2 footSize;
        public float footRayRaise = 2f;
        public float comfyDistance = 1f;
        public float centerOfGravityXOffset = -0.25f;
        public float feetTooFarApartThreshold = 3f;
        public float offBalanceThreshold = 1.4f;
        public float minimumSpaceBetweenFeet = 0.5f;
        public float maxNewStepDisplacement = 2f;
        public float shuffleDistance = 1f;
        public float baseLerpSpeed = 3.5f;
        public FootMovement forward;
        public FootMovement backward;
        [Header("Debug"), SerializeField]
        private float balance;
        [SerializeField]
        private float distanceBetweenFeet;
        [SerializeField]
        private Foot nearFoot;
        [SerializeField]
        private Foot farFoot;
        private Skeleton skeleton;
        private Bone nearFootBone;
        private Bone farFootBone;
        private RaycastHit2D[] hits = new RaycastHit2D[1];

        private void OnDrawGizmos()
        {
            if (Application.isPlaying)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere((Vector3) this.nearFoot.worldPos, 0.15f);
                Gizmos.DrawWireSphere((Vector3) this.nearFoot.worldPosNext, 0.15f);
                Gizmos.color = Color.magenta;
                Gizmos.DrawSphere((Vector3) this.farFoot.worldPos, 0.15f);
                Gizmos.DrawWireSphere((Vector3) this.farFoot.worldPosNext, 0.15f);
            }
        }

        private void Start()
        {
            Time.timeScale = this.timeScale;
            Vector3 position = base.transform.position;
            this.nearFoot.worldPos = position;
            this.nearFoot.worldPos.x -= this.comfyDistance;
            this.nearFoot.worldPosPrev = this.nearFoot.worldPosNext = this.nearFoot.worldPos;
            this.farFoot.worldPos = position;
            this.farFoot.worldPos.x += this.comfyDistance;
            this.farFoot.worldPosPrev = this.farFoot.worldPosNext = this.farFoot.worldPos;
            SkeletonAnimation component = base.GetComponent<SkeletonAnimation>();
            this.skeleton = component.Skeleton;
            component.UpdateLocal += new UpdateBonesDelegate(this.UpdateLocal);
            this.nearFootBone = this.skeleton.FindBone(this.nearBoneName);
            this.farFootBone = this.skeleton.FindBone(this.farBoneName);
            this.nearFoot.lerp = 1f;
            this.farFoot.lerp = 1f;
        }

        private void UpdateLocal(ISkeletonAnimation animated)
        {
            Transform transform = base.transform;
            Vector2 position = transform.position;
            float centerOfGravityX = position.x + this.centerOfGravityXOffset;
            this.nearFoot.UpdateDistance(centerOfGravityX);
            this.farFoot.UpdateDistance(centerOfGravityX);
            this.balance = this.nearFoot.displacementFromCenter + this.farFoot.displacementFromCenter;
            this.distanceBetweenFeet = Mathf.Abs((float) (this.nearFoot.worldPos.x - this.farFoot.worldPos.x));
            bool flag = Mathf.Abs(this.balance) > this.offBalanceThreshold;
            if ((this.distanceBetweenFeet > this.feetTooFarApartThreshold) || flag)
            {
                Foot nearFoot;
                Foot farFoot;
                if (this.nearFoot.distanceFromCenter > this.farFoot.distanceFromCenter)
                {
                    nearFoot = this.nearFoot;
                    farFoot = this.farFoot;
                }
                else
                {
                    nearFoot = this.farFoot;
                    farFoot = this.nearFoot;
                }
                if (!nearFoot.IsStepInProgress && farFoot.IsPrettyMuchDoneStepping)
                {
                    float newDistance = Foot.GetNewDisplacement(farFoot.displacementFromCenter, this.comfyDistance, this.minimumSpaceBetweenFeet, this.maxNewStepDisplacement, this.forward, this.backward);
                    nearFoot.StartNewStep(newDistance, centerOfGravityX, position.y, this.footRayRaise, this.hits, this.footSize);
                }
            }
            float deltaTime = Time.deltaTime;
            float stepSpeed = this.baseLerpSpeed + ((Mathf.Abs(this.balance) - 0.6f) * 2.5f);
            this.nearFoot.UpdateStepProgress(deltaTime, stepSpeed, this.shuffleDistance, this.forward, this.backward);
            this.farFoot.UpdateStepProgress(deltaTime, stepSpeed, this.shuffleDistance, this.forward, this.backward);
            this.nearFootBone.SetPosition(transform.InverseTransformPoint((Vector3) this.nearFoot.worldPos));
            this.farFootBone.SetPosition(transform.InverseTransformPoint((Vector3) this.farFoot.worldPos));
        }

        public float Balance =>
            this.balance;

        [Serializable]
        public class Foot
        {
            public Vector2 worldPos;
            public float displacementFromCenter;
            public float distanceFromCenter;
            [Space]
            public float lerp;
            public Vector2 worldPosPrev;
            public Vector2 worldPosNext;

            public static float GetNewDisplacement(float otherLegDisplacementFromCenter, float comfyDistance, float minimumFootDistanceX, float maxNewStepDisplacement, SpineboyFootplanter.FootMovement forwardMovement, SpineboyFootplanter.FootMovement backwardMovement)
            {
                SpineboyFootplanter.FootMovement movement = (Mathf.Sign(otherLegDisplacementFromCenter) >= 0f) ? backwardMovement : forwardMovement;
                float num = UnityEngine.Random.Range(movement.minDistanceCompensate, movement.maxDistanceCompensate);
                float f = otherLegDisplacementFromCenter * num;
                if ((Mathf.Abs(f) <= maxNewStepDisplacement) && (Mathf.Abs(otherLegDisplacementFromCenter) >= minimumFootDistanceX))
                {
                    return f;
                }
                return ((comfyDistance * Mathf.Sign(f)) * num);
            }

            public void StartNewStep(float newDistance, float centerOfGravityX, float tentativeY, float footRayRaise, RaycastHit2D[] hits, Vector2 footSize)
            {
                this.lerp = 0f;
                this.worldPosPrev = this.worldPos;
                float x = centerOfGravityX - newDistance;
                Vector2 origin = new Vector2(x, tentativeY + footRayRaise);
                ContactFilter2D contactFilter = new ContactFilter2D {
                    useTriggers = false
                };
                int num2 = Physics2D.BoxCast(origin, footSize, 0f, Vector2.down, contactFilter, hits);
                this.worldPosNext = (num2 <= 0) ? new Vector2(x, tentativeY) : hits[0].point;
            }

            public void UpdateDistance(float centerOfGravityX)
            {
                this.displacementFromCenter = this.worldPos.x - centerOfGravityX;
                this.distanceFromCenter = Mathf.Abs(this.displacementFromCenter);
            }

            public void UpdateStepProgress(float deltaTime, float stepSpeed, float shuffleDistance, SpineboyFootplanter.FootMovement forwardMovement, SpineboyFootplanter.FootMovement backwardMovement)
            {
                if (this.IsStepInProgress)
                {
                    this.lerp += deltaTime * stepSpeed;
                    float f = this.worldPosNext.x - this.worldPosPrev.x;
                    float num2 = Mathf.Sign(f);
                    float num3 = Mathf.Abs(f);
                    SpineboyFootplanter.FootMovement movement = (num2 <= 0f) ? backwardMovement : forwardMovement;
                    this.worldPos.x = Mathf.Lerp(this.worldPosPrev.x, this.worldPosNext.x, movement.xMoveCurve.Evaluate(this.lerp));
                    float num4 = Mathf.Lerp(this.worldPosPrev.y, this.worldPosNext.y, this.lerp);
                    if (num3 > shuffleDistance)
                    {
                        float num5 = Mathf.Clamp((float) (num3 * 0.5f), (float) 1f, (float) 2f);
                        this.worldPos.y = num4 + ((movement.raiseCurve.Evaluate(this.lerp) * movement.maxRaise) * num5);
                    }
                    else
                    {
                        this.lerp += Time.deltaTime;
                        this.worldPos.y = num4;
                    }
                    if (this.lerp > 1f)
                    {
                        this.lerp = 1f;
                    }
                }
            }

            public bool IsStepInProgress =>
                (this.lerp < 1f);

            public bool IsPrettyMuchDoneStepping =>
                (this.lerp > 0.7f);
        }

        [Serializable]
        public class FootMovement
        {
            public AnimationCurve xMoveCurve;
            public AnimationCurve raiseCurve;
            public float maxRaise;
            public float minDistanceCompensate;
            public float maxDistanceCompensate;
        }
    }
}

