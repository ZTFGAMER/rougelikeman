namespace Spine.Unity.Examples
{
    using Spine;
    using Spine.Unity;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class Spineboy : MonoBehaviour
    {
        private SkeletonAnimation skeletonAnimation;
        [CompilerGenerated]
        private static Spine.AnimationState.TrackEntryDelegate <>f__am$cache0;

        private void HandleEvent(TrackEntry trackEntry, Event e)
        {
            Debug.Log(string.Concat(new object[] { trackEntry.TrackIndex, " ", trackEntry.Animation.Name, ": event ", e, ", ", e.Int }));
        }

        public void OnMouseDown()
        {
            this.skeletonAnimation.AnimationState.SetAnimation(0, "jump", false);
            this.skeletonAnimation.AnimationState.AddAnimation(0, "run", true, 0f);
        }

        public void Start()
        {
            this.skeletonAnimation = base.GetComponent<SkeletonAnimation>();
            Spine.AnimationState animationState = this.skeletonAnimation.AnimationState;
            animationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = entry => Debug.Log("start: " + entry.TrackIndex);
            }
            animationState.End += <>f__am$cache0;
            animationState.AddAnimation(0, "jump", false, 2f);
            animationState.AddAnimation(0, "run", true, 0f);
        }
    }
}

