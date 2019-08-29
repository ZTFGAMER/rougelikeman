namespace Spine.Unity
{
    using Spine;
    using System;
    using System.Collections;
    using UnityEngine;

    public class WaitForSpineAnimationComplete : IEnumerator
    {
        private bool m_WasFired;

        public WaitForSpineAnimationComplete(TrackEntry trackEntry)
        {
            this.SafeSubscribe(trackEntry);
        }

        private void HandleComplete(TrackEntry trackEntry)
        {
            this.m_WasFired = true;
        }

        public WaitForSpineAnimationComplete NowWaitFor(TrackEntry trackEntry)
        {
            this.SafeSubscribe(trackEntry);
            return this;
        }

        private void SafeSubscribe(TrackEntry trackEntry)
        {
            if (trackEntry == null)
            {
                Debug.LogWarning("TrackEntry was null. Coroutine will continue immediately.");
                this.m_WasFired = true;
            }
            else
            {
                trackEntry.Complete += new Spine.AnimationState.TrackEntryDelegate(this.HandleComplete);
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
    }
}

