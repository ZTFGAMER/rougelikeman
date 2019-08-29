namespace Spine.Unity.Examples
{
    using Spine;
    using Spine.Unity;
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    public class AttackSpineboy : MonoBehaviour
    {
        public SkeletonAnimation spineboy;
        public SkeletonAnimation attackerSpineboy;
        public SpineGauge gauge;
        public Text healthText;
        private int currentHealth = 100;
        private const int maxHealth = 100;
        public AnimationReferenceAsset shoot;
        public AnimationReferenceAsset hit;
        public AnimationReferenceAsset idle;
        public AnimationReferenceAsset death;
        public UnityEvent onAttack;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                this.currentHealth -= 10;
                this.healthText.text = this.currentHealth + "/" + 100;
                this.attackerSpineboy.AnimationState.SetAnimation(1, (Spine.Animation) this.shoot, false);
                this.attackerSpineboy.AnimationState.AddEmptyAnimation(1, 0.5f, 2f);
                if (this.currentHealth > 0)
                {
                    this.spineboy.AnimationState.SetAnimation(0, (Spine.Animation) this.hit, false);
                    this.spineboy.AnimationState.AddAnimation(0, (Spine.Animation) this.idle, true, 0f);
                    this.gauge.fillPercent = ((float) this.currentHealth) / 100f;
                    this.onAttack.Invoke();
                }
                else if (this.currentHealth >= 0)
                {
                    this.gauge.fillPercent = 0f;
                    this.spineboy.AnimationState.SetAnimation(0, (Spine.Animation) this.death, false).TrackEnd = float.PositiveInfinity;
                }
            }
        }
    }
}

