namespace Spine.Unity.Playables
{
    using Spine.Unity;
    using System;
    using UnityEngine;
    using UnityEngine.Playables;

    [Serializable]
    public class SpineAnimationStateBehaviour : PlayableBehaviour
    {
        public AnimationReferenceAsset animationReference;
        public bool loop;
        [Header("Mix Properties")]
        public bool customDuration;
        public float mixDuration = 0.1f;
        [Range(0f, 1f)]
        public float attachmentThreshold = 0.5f;
        [Range(0f, 1f)]
        public float eventThreshold = 0.5f;
        [Range(0f, 1f)]
        public float drawOrderThreshold = 0.5f;
    }
}

