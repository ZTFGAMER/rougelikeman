namespace Spine.Unity.Examples
{
    using Spine;
    using Spine.Unity;
    using System;
    using UnityEngine;

    public class SpineboyFacialExpression : MonoBehaviour
    {
        public SpineboyFootplanter footPlanter;
        [SpineSlot("", "", false, true, false)]
        public string eyeSlotName;
        [SpineSlot("", "", false, true, false)]
        public string mouthSlotName;
        [SpineAttachment(true, false, false, "eyeSlotName", "", "", true, false)]
        public string shockEyeName;
        [SpineAttachment(true, false, false, "eyeSlotName", "", "", true, false)]
        public string normalEyeName;
        [SpineAttachment(true, false, false, "mouthSlotName", "", "", true, false)]
        public string shockMouthName;
        [SpineAttachment(true, false, false, "mouthSlotName", "", "", true, false)]
        public string normalMouthName;
        public Slot eyeSlot;
        public Slot mouthSlot;
        public Attachment shockEye;
        public Attachment normalEye;
        public Attachment shockMouth;
        public Attachment normalMouth;
        public float balanceThreshold = 2.5f;
        public float shockDuration = 1f;
        [Header("Debug")]
        public float shockTimer;

        private void Start()
        {
            Skeleton skeleton = base.GetComponent<SkeletonAnimation>().Skeleton;
            this.eyeSlot = skeleton.FindSlot(this.eyeSlotName);
            this.mouthSlot = skeleton.FindSlot(this.mouthSlotName);
            int slotIndex = skeleton.FindSlotIndex(this.eyeSlotName);
            this.shockEye = skeleton.GetAttachment(slotIndex, this.shockEyeName);
            this.normalEye = skeleton.GetAttachment(slotIndex, this.normalEyeName);
            int num2 = skeleton.FindSlotIndex(this.mouthSlotName);
            this.shockMouth = skeleton.GetAttachment(num2, this.shockMouthName);
            this.normalMouth = skeleton.GetAttachment(num2, this.normalMouthName);
        }

        private void Update()
        {
            if (Mathf.Abs(this.footPlanter.Balance) > this.balanceThreshold)
            {
                this.shockTimer = this.shockDuration;
            }
            if (this.shockTimer > 0f)
            {
                this.shockTimer -= Time.deltaTime;
            }
            if (this.shockTimer > 0f)
            {
                this.eyeSlot.Attachment = this.shockEye;
                this.mouthSlot.Attachment = this.shockMouth;
            }
            else
            {
                this.eyeSlot.Attachment = this.normalEye;
                this.mouthSlot.Attachment = this.normalMouth;
            }
        }
    }
}

