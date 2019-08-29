namespace Spine.Unity.Examples
{
    using Spine;
    using Spine.Unity;
    using System;
    using UnityEngine;

    public class SpineboyBeginnerView : MonoBehaviour
    {
        [Header("Components")]
        public SpineboyBeginnerModel model;
        public SkeletonAnimation skeletonAnimation;
        public AnimationReferenceAsset run;
        public AnimationReferenceAsset idle;
        public AnimationReferenceAsset shoot;
        public AnimationReferenceAsset jump;
        public EventDataReferenceAsset footstepEvent;
        [Header("Audio")]
        public float footstepPitchOffset = 0.2f;
        public float gunsoundPitchOffset = 0.13f;
        public AudioSource footstepSource;
        public AudioSource gunSource;
        public AudioSource jumpSource;
        [Header("Effects")]
        public ParticleSystem gunParticles;
        private SpineBeginnerBodyState previousViewState;

        [ContextMenu("Check Tracks")]
        private void CheckTracks()
        {
            Spine.AnimationState animationState = this.skeletonAnimation.AnimationState;
            Debug.Log(animationState.GetCurrent(0));
            Debug.Log(animationState.GetCurrent(1));
        }

        public float GetRandomPitch(float maxPitchOffset) => 
            (1f + UnityEngine.Random.Range(-maxPitchOffset, maxPitchOffset));

        private void HandleEvent(TrackEntry trackEntry, Event e)
        {
            if (e.Data == this.footstepEvent.EventData)
            {
                this.PlayFootstepSound();
            }
        }

        private void PlayFootstepSound()
        {
            this.footstepSource.Play();
            this.footstepSource.pitch = this.GetRandomPitch(this.footstepPitchOffset);
        }

        private void PlayNewStableAnimation()
        {
            Spine.Animation jump;
            SpineBeginnerBodyState state = this.model.state;
            if ((this.previousViewState == SpineBeginnerBodyState.Jumping) && (state != SpineBeginnerBodyState.Jumping))
            {
                this.PlayFootstepSound();
            }
            if (state == SpineBeginnerBodyState.Jumping)
            {
                this.jumpSource.Play();
                jump = (Spine.Animation) this.jump;
            }
            else if (state == SpineBeginnerBodyState.Running)
            {
                jump = (Spine.Animation) this.run;
            }
            else
            {
                jump = (Spine.Animation) this.idle;
            }
            this.skeletonAnimation.AnimationState.SetAnimation(0, jump, true);
        }

        public void PlayShoot()
        {
            TrackEntry entry = this.skeletonAnimation.AnimationState.SetAnimation(1, (Spine.Animation) this.shoot, false);
            entry.AttachmentThreshold = 1f;
            entry.MixDuration = 0f;
            this.skeletonAnimation.state.AddEmptyAnimation(1, 0.5f, 0.1f).AttachmentThreshold = 1f;
            this.gunSource.pitch = this.GetRandomPitch(this.gunsoundPitchOffset);
            this.gunSource.Play();
            this.gunParticles.Play();
        }

        private void Start()
        {
            if (this.skeletonAnimation != null)
            {
                this.model.ShootEvent += new Action(this.PlayShoot);
                this.skeletonAnimation.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
            }
        }

        public void Turn(bool facingLeft)
        {
            this.skeletonAnimation.Skeleton.FlipX = facingLeft;
        }

        private void Update()
        {
            if ((this.skeletonAnimation != null) && (this.model != null))
            {
                if (this.skeletonAnimation.skeleton.FlipX != this.model.facingLeft)
                {
                    this.Turn(this.model.facingLeft);
                }
                SpineBeginnerBodyState state = this.model.state;
                if (this.previousViewState != state)
                {
                    this.PlayNewStableAnimation();
                }
                this.previousViewState = state;
            }
        }
    }
}

