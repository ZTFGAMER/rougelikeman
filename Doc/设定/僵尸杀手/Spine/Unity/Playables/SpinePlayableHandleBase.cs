namespace Spine.Unity.Playables
{
    using Spine;
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using UnityEngine;

    public abstract class SpinePlayableHandleBase : MonoBehaviour
    {
        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event SpineEventDelegate AnimationEvents;

        protected SpinePlayableHandleBase()
        {
        }

        public virtual void HandleEvents(ExposedList<Event> eventBuffer)
        {
            if ((eventBuffer != null) && (this.AnimationEvents != null))
            {
                int index = 0;
                int count = eventBuffer.Count;
                while (index < count)
                {
                    this.AnimationEvents(eventBuffer.Items[index]);
                    index++;
                }
            }
        }

        public abstract Spine.SkeletonData SkeletonData { get; }

        public abstract Spine.Skeleton Skeleton { get; }
    }
}

