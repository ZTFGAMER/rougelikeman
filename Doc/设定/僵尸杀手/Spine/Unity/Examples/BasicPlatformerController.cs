namespace Spine.Unity.Examples
{
    using Spine;
    using Spine.Unity;
    using System;
    using UnityEngine;

    [RequireComponent(typeof(CharacterController))]
    public class BasicPlatformerController : MonoBehaviour
    {
        [Header("Controls")]
        public string XAxis = "Horizontal";
        public string YAxis = "Vertical";
        public string JumpButton = "Jump";
        [Header("Moving")]
        public float walkSpeed = 1.5f;
        public float runSpeed = 7f;
        public float gravityScale = 6.6f;
        [Header("Jumping")]
        public float jumpSpeed = 25f;
        public float minimumJumpDuration = 0.5f;
        public float jumpInterruptFactor = 0.5f;
        public float forceCrouchVelocity = 25f;
        public float forceCrouchDuration = 0.5f;
        [Header("Visuals")]
        public SkeletonAnimation skeletonAnimation;
        [Header("Animation")]
        public TransitionDictionaryExample transitions;
        public AnimationReferenceAsset walk;
        public AnimationReferenceAsset run;
        public AnimationReferenceAsset idle;
        public AnimationReferenceAsset jump;
        public AnimationReferenceAsset fall;
        public AnimationReferenceAsset crouch;
        public AnimationReferenceAsset runFromFall;
        [Header("Effects")]
        public AudioSource jumpAudioSource;
        public AudioSource hardfallAudioSource;
        public ParticleSystem landParticles;
        public HandleEventWithAudioExample footstepHandler;
        private CharacterController controller;
        private Vector2 input = new Vector2();
        private Vector3 velocity = new Vector3();
        private float minimumJumpEndTime;
        private float forceCrouchEndTime;
        private bool wasGrounded;
        private AnimationReferenceAsset targetAnimation;
        private AnimationReferenceAsset previousTargetAnimation;

        private void Awake()
        {
            this.controller = base.GetComponent<CharacterController>();
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;
            bool isGrounded = this.controller.isGrounded;
            bool flag2 = !this.wasGrounded && isGrounded;
            this.input.x = Input.GetAxis(this.XAxis);
            this.input.y = Input.GetAxis(this.YAxis);
            bool buttonUp = Input.GetButtonUp(this.JumpButton);
            bool buttonDown = Input.GetButtonDown(this.JumpButton);
            bool flag5 = (isGrounded && (this.input.y < -0.5f)) || (this.forceCrouchEndTime > Time.time);
            bool flag6 = false;
            bool flag7 = false;
            bool flag8 = false;
            if (flag2 && (-this.velocity.y > this.forceCrouchVelocity))
            {
                flag8 = true;
                flag5 = true;
                this.forceCrouchEndTime = Time.time + this.forceCrouchDuration;
            }
            if (!flag5)
            {
                if (isGrounded)
                {
                    if (buttonDown)
                    {
                        flag7 = true;
                    }
                }
                else
                {
                    flag6 = buttonUp && (Time.time < this.minimumJumpEndTime);
                }
            }
            Vector3 vector = (Physics.gravity * this.gravityScale) * deltaTime;
            if (flag7)
            {
                this.velocity.y = this.jumpSpeed;
                this.minimumJumpEndTime = Time.time + this.minimumJumpDuration;
            }
            else if (flag6 && (this.velocity.y > 0f))
            {
                this.velocity.y *= this.jumpInterruptFactor;
            }
            this.velocity.x = 0f;
            if (!flag5 && (this.input.x != 0f))
            {
                this.velocity.x = (Mathf.Abs(this.input.x) <= 0.6f) ? this.walkSpeed : this.runSpeed;
                this.velocity.x *= Mathf.Sign(this.input.x);
            }
            if (!isGrounded)
            {
                if (this.wasGrounded)
                {
                    if (this.velocity.y < 0f)
                    {
                        this.velocity.y = 0f;
                    }
                }
                else
                {
                    this.velocity += vector;
                }
            }
            this.controller.Move(this.velocity * deltaTime);
            if (isGrounded)
            {
                if (flag5)
                {
                    this.targetAnimation = this.crouch;
                }
                else if (this.input.x == 0f)
                {
                    this.targetAnimation = this.idle;
                }
                else
                {
                    this.targetAnimation = (Mathf.Abs(this.input.x) <= 0.6f) ? this.walk : this.run;
                }
            }
            else
            {
                this.targetAnimation = (this.velocity.y <= 0f) ? this.fall : this.jump;
            }
            if (this.previousTargetAnimation != this.targetAnimation)
            {
                Spine.Animation transition = null;
                if ((this.transitions != null) && (this.previousTargetAnimation != null))
                {
                    transition = this.transitions.GetTransition((Spine.Animation) this.previousTargetAnimation, (Spine.Animation) this.targetAnimation);
                }
                if (transition != null)
                {
                    this.skeletonAnimation.AnimationState.SetAnimation(0, transition, false).MixDuration = 0.05f;
                    this.skeletonAnimation.AnimationState.AddAnimation(0, (Spine.Animation) this.targetAnimation, true, 0f);
                }
                else
                {
                    this.skeletonAnimation.AnimationState.SetAnimation(0, (Spine.Animation) this.targetAnimation, true);
                }
            }
            this.previousTargetAnimation = this.targetAnimation;
            if (this.input.x != 0f)
            {
                this.skeletonAnimation.Skeleton.FlipX = this.input.x < 0f;
            }
            if (flag7)
            {
                this.jumpAudioSource.Stop();
                this.jumpAudioSource.Play();
            }
            if (flag2)
            {
                if (flag8)
                {
                    this.hardfallAudioSource.Play();
                }
                else
                {
                    this.footstepHandler.Play();
                }
                this.landParticles.Emit((int) (((int) (this.velocity.y / -9f)) + 2));
            }
            this.wasGrounded = isGrounded;
        }
    }
}

