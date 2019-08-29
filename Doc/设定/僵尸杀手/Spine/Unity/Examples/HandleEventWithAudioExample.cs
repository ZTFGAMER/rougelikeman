namespace Spine.Unity.Examples
{
    using Spine;
    using Spine.Unity;
    using System;
    using UnityEngine;

    public class HandleEventWithAudioExample : MonoBehaviour
    {
        public SkeletonAnimation skeletonAnimation;
        [SpineEvent("", "skeletonAnimation", true, true)]
        public string eventName;
        [Space]
        public AudioSource audioSource;
        public AudioClip audioClip;
        public float basePitch = 1f;
        public float randomPitchOffset = 0.1f;
        [Space]
        public bool logDebugMessage;
        private EventData eventData;

        private void HandleAnimationStateEvent(TrackEntry trackEntry, Event e)
        {
            if (this.logDebugMessage)
            {
                Debug.Log("Event fired! " + e.Data.Name);
            }
            if (this.eventData == e.Data)
            {
                this.Play();
            }
        }

        private void OnValidate()
        {
            if (this.skeletonAnimation == null)
            {
                base.GetComponent<SkeletonAnimation>();
            }
            if (this.audioSource == null)
            {
                base.GetComponent<AudioSource>();
            }
        }

        public void Play()
        {
            this.audioSource.pitch = this.basePitch + UnityEngine.Random.Range(-this.randomPitchOffset, this.randomPitchOffset);
            this.audioSource.clip = this.audioClip;
            this.audioSource.Play();
        }

        private void Start()
        {
            if ((this.audioSource != null) && (this.skeletonAnimation != null))
            {
                this.skeletonAnimation.Initialize(false);
                if (this.skeletonAnimation.valid)
                {
                    this.eventData = this.skeletonAnimation.Skeleton.Data.FindEvent(this.eventName);
                    this.skeletonAnimation.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
                }
            }
        }
    }
}

