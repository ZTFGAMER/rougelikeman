namespace Spine
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class TrackEntry : Pool<TrackEntry>.IPoolable
    {
        internal Spine.Animation animation;
        internal TrackEntry next;
        internal TrackEntry mixingFrom;
        internal int trackIndex;
        internal bool loop;
        internal float eventThreshold;
        internal float attachmentThreshold;
        internal float drawOrderThreshold;
        internal float animationStart;
        internal float animationEnd;
        internal float animationLast;
        internal float nextAnimationLast;
        internal float delay;
        internal float trackTime;
        internal float trackLast;
        internal float nextTrackLast;
        internal float trackEnd;
        internal float timeScale = 1f;
        internal float alpha;
        internal float mixTime;
        internal float mixDuration;
        internal float interruptAlpha;
        internal float totalAlpha;
        internal readonly ExposedList<int> timelineData = new ExposedList<int>();
        internal readonly ExposedList<TrackEntry> timelineDipMix = new ExposedList<TrackEntry>();
        internal readonly ExposedList<float> timelinesRotation = new ExposedList<float>();

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event AnimationState.TrackEntryDelegate Complete;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event AnimationState.TrackEntryDelegate Dispose;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event AnimationState.TrackEntryDelegate End;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event AnimationState.TrackEntryEventDelegate Event;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event AnimationState.TrackEntryDelegate Interrupt;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event AnimationState.TrackEntryDelegate Start;

        private bool HasTimeline(int id)
        {
            Timeline[] items = this.animation.timelines.Items;
            int index = 0;
            int count = this.animation.timelines.Count;
            while (index < count)
            {
                if (items[index].PropertyId == id)
                {
                    return true;
                }
                index++;
            }
            return false;
        }

        internal void OnComplete()
        {
            if (this.Complete != null)
            {
                this.Complete(this);
            }
        }

        internal void OnDispose()
        {
            if (this.Dispose != null)
            {
                this.Dispose(this);
            }
        }

        internal void OnEnd()
        {
            if (this.End != null)
            {
                this.End(this);
            }
        }

        internal void OnEvent(Spine.Event e)
        {
            if (this.Event != null)
            {
                this.Event(this, e);
            }
        }

        internal void OnInterrupt()
        {
            if (this.Interrupt != null)
            {
                this.Interrupt(this);
            }
        }

        internal void OnStart()
        {
            if (this.Start != null)
            {
                this.Start(this);
            }
        }

        public void Reset()
        {
            this.next = null;
            this.mixingFrom = null;
            this.animation = null;
            this.timelineData.Clear(true);
            this.timelineDipMix.Clear(true);
            this.timelinesRotation.Clear(true);
            this.Start = null;
            this.Interrupt = null;
            this.End = null;
            this.Dispose = null;
            this.Complete = null;
            this.Event = null;
        }

        public void ResetRotationDirections()
        {
            this.timelinesRotation.Clear(true);
        }

        internal TrackEntry SetTimelineData(TrackEntry to, ExposedList<TrackEntry> mixingToArray, HashSet<int> propertyIDs)
        {
            if (to != null)
            {
                mixingToArray.Add(to);
            }
            TrackEntry entry = (this.mixingFrom == null) ? this : this.mixingFrom.SetTimelineData(this, mixingToArray, propertyIDs);
            if (to != null)
            {
                mixingToArray.Pop();
            }
            TrackEntry[] items = mixingToArray.Items;
            int num = mixingToArray.Count - 1;
            Timeline[] timelineArray = this.animation.timelines.Items;
            int count = this.animation.timelines.Count;
            int[] numArray = this.timelineData.Resize(count).Items;
            this.timelineDipMix.Clear(true);
            TrackEntry[] entryArray2 = this.timelineDipMix.Resize(count).Items;
            for (int i = 0; i < count; i++)
            {
                int propertyId = timelineArray[i].PropertyId;
                if (!propertyIDs.Add(propertyId))
                {
                    numArray[i] = 0;
                    continue;
                }
                if ((to == null) || !to.HasTimeline(propertyId))
                {
                    numArray[i] = 1;
                    continue;
                }
                for (int j = num; j >= 0; j--)
                {
                    TrackEntry entry2 = items[j];
                    if (!entry2.HasTimeline(propertyId))
                    {
                        if (entry2.mixDuration <= 0f)
                        {
                            break;
                        }
                        numArray[i] = 3;
                        entryArray2[i] = entry2;
                        continue;
                    }
                }
                numArray[i] = 2;
            }
            return entry;
        }

        public override string ToString() => 
            ((this.animation != null) ? this.animation.name : "<none>");

        public int TrackIndex =>
            this.trackIndex;

        public Spine.Animation Animation =>
            this.animation;

        public bool Loop
        {
            get => 
                this.loop;
            set => 
                (this.loop = value);
        }

        public float Delay
        {
            get => 
                this.delay;
            set => 
                (this.delay = value);
        }

        public float TrackTime
        {
            get => 
                this.trackTime;
            set => 
                (this.trackTime = value);
        }

        public float TrackEnd
        {
            get => 
                this.trackEnd;
            set => 
                (this.trackEnd = value);
        }

        public float AnimationStart
        {
            get => 
                this.animationStart;
            set => 
                (this.animationStart = value);
        }

        public float AnimationEnd
        {
            get => 
                this.animationEnd;
            set => 
                (this.animationEnd = value);
        }

        public float AnimationLast
        {
            get => 
                this.animationLast;
            set
            {
                this.animationLast = value;
                this.nextAnimationLast = value;
            }
        }

        public float AnimationTime
        {
            get
            {
                if (!this.loop)
                {
                    return Math.Min(this.trackTime + this.animationStart, this.animationEnd);
                }
                float num = this.animationEnd - this.animationStart;
                if (num == 0f)
                {
                    return this.animationStart;
                }
                return ((this.trackTime % num) + this.animationStart);
            }
        }

        public float TimeScale
        {
            get => 
                this.timeScale;
            set => 
                (this.timeScale = value);
        }

        public float Alpha
        {
            get => 
                this.alpha;
            set => 
                (this.alpha = value);
        }

        public float EventThreshold
        {
            get => 
                this.eventThreshold;
            set => 
                (this.eventThreshold = value);
        }

        public float AttachmentThreshold
        {
            get => 
                this.attachmentThreshold;
            set => 
                (this.attachmentThreshold = value);
        }

        public float DrawOrderThreshold
        {
            get => 
                this.drawOrderThreshold;
            set => 
                (this.drawOrderThreshold = value);
        }

        public TrackEntry Next =>
            this.next;

        public bool IsComplete =>
            (this.trackTime >= (this.animationEnd - this.animationStart));

        public float MixTime
        {
            get => 
                this.mixTime;
            set => 
                (this.mixTime = value);
        }

        public float MixDuration
        {
            get => 
                this.mixDuration;
            set => 
                (this.mixDuration = value);
        }

        public TrackEntry MixingFrom =>
            this.mixingFrom;
    }
}

