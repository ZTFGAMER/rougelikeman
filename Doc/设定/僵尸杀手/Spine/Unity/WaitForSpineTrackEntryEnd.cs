namespace Spine.Unity
{
    using Spine;
    using System;
    using System.Collections;
    using UnityEngine;

    public class WaitForSpineTrackEntryEnd : IEnumerator
    {
        private bool m_WasFired;

        public WaitForSpineTrackEntryEnd(TrackEntry trackEntry)
        {
            this.SafeSubscribe(trackEntry);
        }

        private void HandleEnd(TrackEntry trackEntry)
        {
            this.m_WasFired = true;
        }

        public WaitForSpineTrackEntryEnd NowWaitFor(TrackEntry trackEntry)
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
                trackEntry.End += new Spine.AnimationState.TrackEntryDelegate(this.HandleEnd);
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

