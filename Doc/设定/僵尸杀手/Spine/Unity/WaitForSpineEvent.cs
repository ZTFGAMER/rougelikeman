namespace Spine.Unity
{
    using Spine;
    using System;
    using System.Collections;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class WaitForSpineEvent : IEnumerator
    {
        private EventData m_TargetEvent;
        private string m_EventName;
        private Spine.AnimationState m_AnimationState;
        private bool m_WasFired;
        private bool m_unsubscribeAfterFiring;

        public WaitForSpineEvent(Spine.AnimationState state, EventData eventDataReference, bool unsubscribeAfterFiring = true)
        {
            this.Subscribe(state, eventDataReference, unsubscribeAfterFiring);
        }

        public WaitForSpineEvent(Spine.AnimationState state, string eventName, bool unsubscribeAfterFiring = true)
        {
            this.SubscribeByName(state, eventName, unsubscribeAfterFiring);
        }

        public WaitForSpineEvent(SkeletonAnimation skeletonAnimation, EventData eventDataReference, bool unsubscribeAfterFiring = true)
        {
            this.Subscribe(skeletonAnimation.state, eventDataReference, unsubscribeAfterFiring);
        }

        public WaitForSpineEvent(SkeletonAnimation skeletonAnimation, string eventName, bool unsubscribeAfterFiring = true)
        {
            this.SubscribeByName(skeletonAnimation.state, eventName, unsubscribeAfterFiring);
        }

        private void Clear(Spine.AnimationState state)
        {
            state.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
            state.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEventByName);
        }

        private void HandleAnimationStateEvent(TrackEntry trackEntry, Event e)
        {
            this.m_WasFired |= e.Data == this.m_TargetEvent;
            if (this.m_WasFired && this.m_unsubscribeAfterFiring)
            {
                this.m_AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
            }
        }

        private void HandleAnimationStateEventByName(TrackEntry trackEntry, Event e)
        {
            this.m_WasFired |= e.Data.Name == this.m_EventName;
            if (this.m_WasFired && this.m_unsubscribeAfterFiring)
            {
                this.m_AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEventByName);
            }
        }

        public WaitForSpineEvent NowWaitFor(Spine.AnimationState state, EventData eventDataReference, bool unsubscribeAfterFiring = true)
        {
            ((IEnumerator) this).Reset();
            this.Clear(state);
            this.Subscribe(state, eventDataReference, unsubscribeAfterFiring);
            return this;
        }

        public WaitForSpineEvent NowWaitFor(Spine.AnimationState state, string eventName, bool unsubscribeAfterFiring = true)
        {
            ((IEnumerator) this).Reset();
            this.Clear(state);
            this.SubscribeByName(state, eventName, unsubscribeAfterFiring);
            return this;
        }

        private void Subscribe(Spine.AnimationState state, EventData eventDataReference, bool unsubscribe)
        {
            if (state == null)
            {
                Debug.LogWarning("AnimationState argument was null. Coroutine will continue immediately.");
                this.m_WasFired = true;
            }
            else if (eventDataReference == null)
            {
                Debug.LogWarning("eventDataReference argument was null. Coroutine will continue immediately.");
                this.m_WasFired = true;
            }
            else
            {
                this.m_AnimationState = state;
                this.m_TargetEvent = eventDataReference;
                state.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
                this.m_unsubscribeAfterFiring = unsubscribe;
            }
        }

        private void SubscribeByName(Spine.AnimationState state, string eventName, bool unsubscribe)
        {
            if (state == null)
            {
                Debug.LogWarning("AnimationState argument was null. Coroutine will continue immediately.");
                this.m_WasFired = true;
            }
            else if (string.IsNullOrEmpty(eventName))
            {
                Debug.LogWarning("eventName argument was null. Coroutine will continue immediately.");
                this.m_WasFired = true;
            }
            else
            {
                this.m_AnimationState = state;
                this.m_EventName = eventName;
                state.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEventByName);
                this.m_unsubscribeAfterFiring = unsubscribe;
            }
        }

        bool IEnumerator.MoveNext()
        {
            if (this.m_WasFired)
            {
                ((IEnumerator) this).Reset();
                return false;
            }
            return true;
        }

        void IEnumerator.Reset()
        {
            this.m_WasFired = false;
        }

        object IEnumerator.Current =>
            null;

        public bool WillUnsubscribeAfterFiring
        {
            get => 
                this.m_unsubscribeAfterFiring;
            set => 
                (this.m_unsubscribeAfterFiring = value);
        }
    }
}

