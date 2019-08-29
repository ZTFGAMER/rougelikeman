namespace Spine.Unity.Modules
{
    using Spine;
    using Spine.Unity;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.Events;

    public class SpineEventUnityHandler : MonoBehaviour
    {
        public List<EventPair> events = new List<EventPair>();
        private ISkeletonComponent skeletonComponent;
        private IAnimationStateComponent animationStateComponent;

        private void OnDestroy()
        {
            if (this.animationStateComponent == null)
            {
            }
            this.animationStateComponent = base.GetComponent<IAnimationStateComponent>();
            if (this.animationStateComponent != null)
            {
                Spine.AnimationState animationState = this.animationStateComponent.AnimationState;
                foreach (EventPair pair in this.events)
                {
                    if (pair.eventDelegate != null)
                    {
                        animationState.Event -= pair.eventDelegate;
                    }
                    pair.eventDelegate = null;
                }
            }
        }

        private void Start()
        {
            if (this.skeletonComponent == null)
            {
            }
            this.skeletonComponent = base.GetComponent<ISkeletonComponent>();
            if (this.skeletonComponent != null)
            {
                if (this.animationStateComponent == null)
                {
                }
                this.animationStateComponent = this.skeletonComponent as IAnimationStateComponent;
                if (this.animationStateComponent != null)
                {
                    Skeleton skeleton = this.skeletonComponent.Skeleton;
                    if (skeleton != null)
                    {
                        SkeletonData data = skeleton.Data;
                        Spine.AnimationState animationState = this.animationStateComponent.AnimationState;
                        using (List<EventPair>.Enumerator enumerator = this.events.GetEnumerator())
                        {
                            while (enumerator.MoveNext())
                            {
                                <Start>c__AnonStorey1 storey = new <Start>c__AnonStorey1 {
                                    ep = enumerator.Current
                                };
                                <Start>c__AnonStorey0 storey2 = new <Start>c__AnonStorey0 {
                                    <>f__ref$1 = storey,
                                    eventData = data.FindEvent(storey.ep.spineEvent)
                                };
                                if (storey.ep.eventDelegate == null)
                                {
                                }
                                storey.ep.eventDelegate = new Spine.AnimationState.TrackEntryEventDelegate(storey2.<>m__0);
                                animationState.Event += storey.ep.eventDelegate;
                            }
                        }
                    }
                }
            }
        }

        [CompilerGenerated]
        private sealed class <Start>c__AnonStorey0
        {
            internal EventData eventData;
            internal SpineEventUnityHandler.<Start>c__AnonStorey1 <>f__ref$1;

            internal void <>m__0(TrackEntry trackEntry, Event e)
            {
                if (e.Data == this.eventData)
                {
                    this.<>f__ref$1.ep.unityHandler.Invoke();
                }
            }
        }

        [CompilerGenerated]
        private sealed class <Start>c__AnonStorey1
        {
            internal SpineEventUnityHandler.EventPair ep;
        }

        [Serializable]
        public class EventPair
        {
            [SpineEvent("", "", true, false)]
            public string spineEvent;
            public UnityEvent unityHandler;
            public Spine.AnimationState.TrackEntryEventDelegate eventDelegate;
        }
    }
}

