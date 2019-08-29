namespace Spine
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;

    public class AnimationState
    {
        private static readonly Animation EmptyAnimation = new Animation("<empty>", new ExposedList<Timeline>(), 0f);
        internal const int Subsequent = 0;
        internal const int First = 1;
        internal const int Dip = 2;
        internal const int DipMix = 3;
        private AnimationStateData data;
        private Pool<TrackEntry> trackEntryPool = new Pool<TrackEntry>(0x10, 0x7fffffff);
        private readonly ExposedList<TrackEntry> tracks = new ExposedList<TrackEntry>();
        private readonly ExposedList<Spine.Event> events = new ExposedList<Spine.Event>();
        private readonly EventQueue queue;
        private readonly HashSet<int> propertyIDs = new HashSet<int>();
        private readonly ExposedList<TrackEntry> mixingTo = new ExposedList<TrackEntry>();
        private bool animationsChanged;
        private float timeScale = 1f;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event TrackEntryDelegate Complete;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event TrackEntryDelegate Dispose;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event TrackEntryDelegate End;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event TrackEntryEventDelegate Event;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event TrackEntryDelegate Interrupt;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event TrackEntryDelegate Start;

        public AnimationState(AnimationStateData data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data", "data cannot be null.");
            }
            this.data = data;
            this.queue = new EventQueue(this, new Action(this.<AnimationState>m__0), this.trackEntryPool);
        }

        [CompilerGenerated]
        private void <AnimationState>m__0()
        {
            this.animationsChanged = true;
        }

        public TrackEntry AddAnimation(int trackIndex, Animation animation, bool loop, float delay)
        {
            if (animation == null)
            {
                throw new ArgumentNullException("animation", "animation cannot be null.");
            }
            TrackEntry last = this.ExpandToIndex(trackIndex);
            if (last != null)
            {
                while (last.next != null)
                {
                    last = last.next;
                }
            }
            TrackEntry current = this.NewTrackEntry(trackIndex, animation, loop, last);
            if (last == null)
            {
                this.SetCurrent(trackIndex, current, true);
                this.queue.Drain();
            }
            else
            {
                last.next = current;
                if (delay <= 0f)
                {
                    float num = last.animationEnd - last.animationStart;
                    if (num != 0f)
                    {
                        if (last.loop)
                        {
                            delay += num * (1 + ((int) (last.trackTime / num)));
                        }
                        else
                        {
                            delay += num;
                        }
                        delay -= this.data.GetMix(last.animation, animation);
                    }
                    else
                    {
                        delay = 0f;
                    }
                }
            }
            current.delay = delay;
            return current;
        }

        public TrackEntry AddAnimation(int trackIndex, string animationName, bool loop, float delay)
        {
            Animation animation = this.data.skeletonData.FindAnimation(animationName);
            if (animation == null)
            {
                throw new ArgumentException("Animation not found: " + animationName, "animationName");
            }
            return this.AddAnimation(trackIndex, animation, loop, delay);
        }

        public TrackEntry AddEmptyAnimation(int trackIndex, float mixDuration, float delay)
        {
            if (delay <= 0f)
            {
                delay -= mixDuration;
            }
            TrackEntry entry = this.AddAnimation(trackIndex, EmptyAnimation, false, delay);
            entry.mixDuration = mixDuration;
            entry.trackEnd = mixDuration;
            return entry;
        }

        private void AnimationsChanged()
        {
            this.animationsChanged = false;
            HashSet<int> propertyIDs = this.propertyIDs;
            propertyIDs.Clear();
            ExposedList<TrackEntry> mixingTo = this.mixingTo;
            TrackEntry[] items = this.tracks.Items;
            int index = 0;
            int count = this.tracks.Count;
            while (index < count)
            {
                TrackEntry entry = items[index];
                if (entry != null)
                {
                    entry.SetTimelineData(null, mixingTo, propertyIDs);
                }
                index++;
            }
        }

        public bool Apply(Skeleton skeleton)
        {
            if (skeleton == null)
            {
                throw new ArgumentNullException("skeleton", "skeleton cannot be null.");
            }
            if (this.animationsChanged)
            {
                this.AnimationsChanged();
            }
            ExposedList<Spine.Event> events = this.events;
            bool flag = false;
            TrackEntry[] items = this.tracks.Items;
            int index = 0;
            int count = this.tracks.Count;
            while (index < count)
            {
                TrackEntry to = items[index];
                if ((to != null) && (to.delay <= 0f))
                {
                    flag = true;
                    MixPose currentPose = (index != 0) ? MixPose.CurrentLayered : MixPose.Current;
                    float alpha = to.alpha;
                    if (to.mixingFrom != null)
                    {
                        alpha *= this.ApplyMixingFrom(to, skeleton, currentPose);
                    }
                    else if ((to.trackTime >= to.trackEnd) && (to.next == null))
                    {
                        alpha = 0f;
                    }
                    float animationLast = to.animationLast;
                    float animationTime = to.AnimationTime;
                    int num6 = to.animation.timelines.Count;
                    ExposedList<Timeline> timelines = to.animation.timelines;
                    Timeline[] timelineArray = timelines.Items;
                    if (alpha == 1f)
                    {
                        for (int i = 0; i < num6; i++)
                        {
                            timelineArray[i].Apply(skeleton, animationLast, animationTime, events, 1f, MixPose.Setup, MixDirection.In);
                        }
                    }
                    else
                    {
                        int[] numArray = to.timelineData.Items;
                        bool firstFrame = to.timelinesRotation.Count == 0;
                        if (firstFrame)
                        {
                            to.timelinesRotation.EnsureCapacity(timelines.Count << 1);
                        }
                        float[] timelinesRotation = to.timelinesRotation.Items;
                        for (int i = 0; i < num6; i++)
                        {
                            Timeline timeline = timelineArray[i];
                            MixPose pose = (numArray[i] < 1) ? currentPose : MixPose.Setup;
                            RotateTimeline rotateTimeline = timeline as RotateTimeline;
                            if (rotateTimeline != null)
                            {
                                ApplyRotateTimeline(rotateTimeline, skeleton, animationTime, alpha, pose, timelinesRotation, i << 1, firstFrame);
                            }
                            else
                            {
                                timeline.Apply(skeleton, animationLast, animationTime, events, alpha, pose, MixDirection.In);
                            }
                        }
                    }
                    this.QueueEvents(to, animationTime);
                    events.Clear(false);
                    to.nextAnimationLast = animationTime;
                    to.nextTrackLast = to.trackTime;
                }
                index++;
            }
            this.queue.Drain();
            return flag;
        }

        private float ApplyMixingFrom(TrackEntry to, Skeleton skeleton, MixPose currentPose)
        {
            float num;
            TrackEntry mixingFrom = to.mixingFrom;
            if (mixingFrom.mixingFrom != null)
            {
                this.ApplyMixingFrom(mixingFrom, skeleton, currentPose);
            }
            if (to.mixDuration == 0f)
            {
                num = 1f;
                currentPose = MixPose.Setup;
            }
            else
            {
                num = to.mixTime / to.mixDuration;
                if (num > 1f)
                {
                    num = 1f;
                }
            }
            ExposedList<Spine.Event> events = (num >= mixingFrom.eventThreshold) ? null : this.events;
            bool flag = num < mixingFrom.attachmentThreshold;
            bool flag2 = num < mixingFrom.drawOrderThreshold;
            float animationLast = mixingFrom.animationLast;
            float animationTime = mixingFrom.AnimationTime;
            ExposedList<Timeline> timelines = mixingFrom.animation.timelines;
            int count = timelines.Count;
            Timeline[] items = timelines.Items;
            int[] numArray = mixingFrom.timelineData.Items;
            TrackEntry[] entryArray = mixingFrom.timelineDipMix.Items;
            bool firstFrame = mixingFrom.timelinesRotation.Count == 0;
            if (firstFrame)
            {
                mixingFrom.timelinesRotation.Resize(timelines.Count << 1);
            }
            float[] timelinesRotation = mixingFrom.timelinesRotation.Items;
            float num5 = mixingFrom.alpha * to.interruptAlpha;
            float num6 = num5 * (1f - num);
            mixingFrom.totalAlpha = 0f;
            for (int i = 0; i < count; i++)
            {
                MixPose setup;
                float num7;
                Timeline timeline = items[i];
                switch (numArray[i])
                {
                    case 0:
                    {
                        if (flag || !(timeline is AttachmentTimeline))
                        {
                            break;
                        }
                        continue;
                    }
                    case 1:
                        setup = MixPose.Setup;
                        num7 = num6;
                        goto Label_01E3;

                    case 2:
                        setup = MixPose.Setup;
                        num7 = num5;
                        goto Label_01E3;

                    default:
                    {
                        setup = MixPose.Setup;
                        TrackEntry entry2 = entryArray[i];
                        num7 = num5 * Math.Max((float) 0f, (float) (1f - (entry2.mixTime / entry2.mixDuration)));
                        goto Label_01E3;
                    }
                }
                if (!flag2 && (timeline is DrawOrderTimeline))
                {
                    continue;
                }
                setup = currentPose;
                num7 = num6;
            Label_01E3:
                mixingFrom.totalAlpha += num7;
                RotateTimeline rotateTimeline = timeline as RotateTimeline;
                if (rotateTimeline != null)
                {
                    ApplyRotateTimeline(rotateTimeline, skeleton, animationTime, num7, setup, timelinesRotation, i << 1, firstFrame);
                }
                else
                {
                    timeline.Apply(skeleton, animationLast, animationTime, events, num7, setup, MixDirection.Out);
                }
            }
            if (to.mixDuration > 0f)
            {
                this.QueueEvents(mixingFrom, animationTime);
            }
            this.events.Clear(false);
            mixingFrom.nextAnimationLast = animationTime;
            mixingFrom.nextTrackLast = mixingFrom.trackTime;
            return num;
        }

        private static void ApplyRotateTimeline(RotateTimeline rotateTimeline, Skeleton skeleton, float time, float alpha, MixPose pose, float[] timelinesRotation, int i, bool firstFrame)
        {
            if (firstFrame)
            {
                timelinesRotation[i] = 0f;
            }
            if (alpha == 1f)
            {
                rotateTimeline.Apply(skeleton, 0f, time, null, 1f, pose, MixDirection.In);
            }
            else
            {
                Bone bone = skeleton.bones.Items[rotateTimeline.boneIndex];
                float[] frames = rotateTimeline.frames;
                if (time < frames[0])
                {
                    if (pose == MixPose.Setup)
                    {
                        bone.rotation = bone.data.rotation;
                    }
                }
                else
                {
                    float num;
                    float num7;
                    if (time >= frames[frames.Length - 2])
                    {
                        num = bone.data.rotation + frames[frames.Length + -1];
                    }
                    else
                    {
                        int index = Animation.BinarySearch(frames, time, 2);
                        float num3 = frames[index + -1];
                        float num4 = frames[index];
                        float curvePercent = rotateTimeline.GetCurvePercent((index >> 1) - 1, 1f - ((time - num4) / (frames[index + -2] - num4)));
                        num = frames[index + 1] - num3;
                        num -= (0x4000 - ((int) (16384.499999999996 - (num / 360f)))) * 360;
                        num = (num3 + (num * curvePercent)) + bone.data.rotation;
                        num -= (0x4000 - ((int) (16384.499999999996 - (num / 360f)))) * 360;
                    }
                    float num6 = (pose != MixPose.Setup) ? bone.rotation : bone.data.rotation;
                    float num8 = num - num6;
                    if (num8 == 0f)
                    {
                        num7 = timelinesRotation[i];
                    }
                    else
                    {
                        float num9;
                        float num10;
                        num8 -= (0x4000 - ((int) (16384.499999999996 - (num8 / 360f)))) * 360;
                        if (firstFrame)
                        {
                            num9 = 0f;
                            num10 = num8;
                        }
                        else
                        {
                            num9 = timelinesRotation[i];
                            num10 = timelinesRotation[i + 1];
                        }
                        bool flag = num8 > 0f;
                        bool flag2 = num9 >= 0f;
                        if ((Math.Sign(num10) != Math.Sign(num8)) && (Math.Abs(num10) <= 90f))
                        {
                            if (Math.Abs(num9) > 180f)
                            {
                                num9 += 360 * Math.Sign(num9);
                            }
                            flag2 = flag;
                        }
                        num7 = (num8 + num9) - (num9 % 360f);
                        if (flag2 != flag)
                        {
                            num7 += 360 * Math.Sign(num9);
                        }
                        timelinesRotation[i] = num7;
                    }
                    timelinesRotation[i + 1] = num8;
                    num6 += num7 * alpha;
                    bone.rotation = num6 - ((0x4000 - ((int) (16384.499999999996 - (num6 / 360f)))) * 360);
                }
            }
        }

        public void ClearTrack(int trackIndex)
        {
            if (trackIndex < this.tracks.Count)
            {
                TrackEntry entry = this.tracks.Items[trackIndex];
                if (entry != null)
                {
                    this.queue.End(entry);
                    this.DisposeNext(entry);
                    TrackEntry entry2 = entry;
                    while (true)
                    {
                        TrackEntry mixingFrom = entry2.mixingFrom;
                        if (mixingFrom == null)
                        {
                            break;
                        }
                        this.queue.End(mixingFrom);
                        entry2.mixingFrom = null;
                        entry2 = mixingFrom;
                    }
                    this.tracks.Items[entry.trackIndex] = null;
                    this.queue.Drain();
                }
            }
        }

        public void ClearTracks()
        {
            bool drainDisabled = this.queue.drainDisabled;
            this.queue.drainDisabled = true;
            int trackIndex = 0;
            int count = this.tracks.Count;
            while (trackIndex < count)
            {
                this.ClearTrack(trackIndex);
                trackIndex++;
            }
            this.tracks.Clear(true);
            this.queue.drainDisabled = drainDisabled;
            this.queue.Drain();
        }

        private void DisposeNext(TrackEntry entry)
        {
            for (TrackEntry entry2 = entry.next; entry2 != null; entry2 = entry2.next)
            {
                this.queue.Dispose(entry2);
            }
            entry.next = null;
        }

        private TrackEntry ExpandToIndex(int index)
        {
            if (index < this.tracks.Count)
            {
                return this.tracks.Items[index];
            }
            while (index >= this.tracks.Count)
            {
                this.tracks.Add(null);
            }
            return null;
        }

        public TrackEntry GetCurrent(int trackIndex) => 
            ((trackIndex < this.tracks.Count) ? this.tracks.Items[trackIndex] : null);

        private TrackEntry NewTrackEntry(int trackIndex, Animation animation, bool loop, TrackEntry last)
        {
            TrackEntry entry = this.trackEntryPool.Obtain();
            entry.trackIndex = trackIndex;
            entry.animation = animation;
            entry.loop = loop;
            entry.eventThreshold = 0f;
            entry.attachmentThreshold = 0f;
            entry.drawOrderThreshold = 0f;
            entry.animationStart = 0f;
            entry.animationEnd = animation.Duration;
            entry.animationLast = -1f;
            entry.nextAnimationLast = -1f;
            entry.delay = 0f;
            entry.trackTime = 0f;
            entry.trackLast = -1f;
            entry.nextTrackLast = -1f;
            entry.trackEnd = float.MaxValue;
            entry.timeScale = 1f;
            entry.alpha = 1f;
            entry.interruptAlpha = 1f;
            entry.mixTime = 0f;
            entry.mixDuration = (last != null) ? this.data.GetMix(last.animation, animation) : 0f;
            return entry;
        }

        internal void OnComplete(TrackEntry entry)
        {
            if (this.Complete != null)
            {
                this.Complete(entry);
            }
        }

        internal void OnDispose(TrackEntry entry)
        {
            if (this.Dispose != null)
            {
                this.Dispose(entry);
            }
        }

        internal void OnEnd(TrackEntry entry)
        {
            if (this.End != null)
            {
                this.End(entry);
            }
        }

        internal void OnEvent(TrackEntry entry, Spine.Event e)
        {
            if (this.Event != null)
            {
                this.Event(entry, e);
            }
        }

        internal void OnInterrupt(TrackEntry entry)
        {
            if (this.Interrupt != null)
            {
                this.Interrupt(entry);
            }
        }

        internal void OnStart(TrackEntry entry)
        {
            if (this.Start != null)
            {
                this.Start(entry);
            }
        }

        private void QueueEvents(TrackEntry entry, float animationTime)
        {
            float animationStart = entry.animationStart;
            float animationEnd = entry.animationEnd;
            float num3 = animationEnd - animationStart;
            float num4 = entry.trackLast % num3;
            ExposedList<Spine.Event> events = this.events;
            Spine.Event[] items = events.Items;
            int index = 0;
            int count = events.Count;
            while (index < count)
            {
                Spine.Event e = items[index];
                if (e.time < num4)
                {
                    break;
                }
                if (e.time <= animationEnd)
                {
                    this.queue.Event(entry, e);
                }
                index++;
            }
            bool flag = false;
            if (entry.loop)
            {
                flag = (num3 == 0f) || (num4 > (entry.trackTime % num3));
            }
            else
            {
                flag = (animationTime >= animationEnd) && (entry.animationLast < animationEnd);
            }
            if (flag)
            {
                this.queue.Complete(entry);
            }
            while (index < count)
            {
                Spine.Event event3 = items[index];
                if (event3.time >= animationStart)
                {
                    this.queue.Event(entry, items[index]);
                }
                index++;
            }
        }

        public TrackEntry SetAnimation(int trackIndex, Animation animation, bool loop)
        {
            if (animation == null)
            {
                throw new ArgumentNullException("animation", "animation cannot be null.");
            }
            bool interrupt = true;
            TrackEntry mixingFrom = this.ExpandToIndex(trackIndex);
            if (mixingFrom != null)
            {
                if (mixingFrom.nextTrackLast == -1f)
                {
                    this.tracks.Items[trackIndex] = mixingFrom.mixingFrom;
                    this.queue.Interrupt(mixingFrom);
                    this.queue.End(mixingFrom);
                    this.DisposeNext(mixingFrom);
                    mixingFrom = mixingFrom.mixingFrom;
                    interrupt = false;
                }
                else
                {
                    this.DisposeNext(mixingFrom);
                }
            }
            TrackEntry current = this.NewTrackEntry(trackIndex, animation, loop, mixingFrom);
            this.SetCurrent(trackIndex, current, interrupt);
            this.queue.Drain();
            return current;
        }

        public TrackEntry SetAnimation(int trackIndex, string animationName, bool loop)
        {
            Animation animation = this.data.skeletonData.FindAnimation(animationName);
            if (animation == null)
            {
                throw new ArgumentException("Animation not found: " + animationName, "animationName");
            }
            return this.SetAnimation(trackIndex, animation, loop);
        }

        private void SetCurrent(int index, TrackEntry current, bool interrupt)
        {
            TrackEntry entry = this.ExpandToIndex(index);
            this.tracks.Items[index] = current;
            if (entry != null)
            {
                if (interrupt)
                {
                    this.queue.Interrupt(entry);
                }
                current.mixingFrom = entry;
                current.mixTime = 0f;
                if ((entry.mixingFrom != null) && (entry.mixDuration > 0f))
                {
                    current.interruptAlpha *= Math.Min((float) 1f, (float) (entry.mixTime / entry.mixDuration));
                }
                entry.timelinesRotation.Clear(true);
            }
            this.queue.Start(current);
        }

        public TrackEntry SetEmptyAnimation(int trackIndex, float mixDuration)
        {
            TrackEntry entry = this.SetAnimation(trackIndex, EmptyAnimation, false);
            entry.mixDuration = mixDuration;
            entry.trackEnd = mixDuration;
            return entry;
        }

        public void SetEmptyAnimations(float mixDuration)
        {
            bool drainDisabled = this.queue.drainDisabled;
            this.queue.drainDisabled = true;
            int index = 0;
            int count = this.tracks.Count;
            while (index < count)
            {
                if (this.tracks.Items[index] != null)
                {
                    this.SetEmptyAnimation(index, mixDuration);
                }
                index++;
            }
            this.queue.drainDisabled = drainDisabled;
            this.queue.Drain();
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            int index = 0;
            int count = this.tracks.Count;
            while (index < count)
            {
                TrackEntry entry = this.tracks.Items[index];
                if (entry != null)
                {
                    if (builder.Length > 0)
                    {
                        builder.Append(", ");
                    }
                    builder.Append(entry.ToString());
                }
                index++;
            }
            return ((builder.Length != 0) ? builder.ToString() : "<none>");
        }

        public void Update(float delta)
        {
            delta *= this.timeScale;
            TrackEntry[] items = this.tracks.Items;
            int index = 0;
            int count = this.tracks.Count;
            while (index < count)
            {
                TrackEntry entry = items[index];
                if (entry == null)
                {
                    goto Label_01C9;
                }
                entry.animationLast = entry.nextAnimationLast;
                entry.trackLast = entry.nextTrackLast;
                float num3 = delta * entry.timeScale;
                if (entry.delay > 0f)
                {
                    entry.delay -= num3;
                    if (entry.delay > 0f)
                    {
                        goto Label_01C9;
                    }
                    num3 = -entry.delay;
                    entry.delay = 0f;
                }
                TrackEntry next = entry.next;
                if (next != null)
                {
                    float num4 = entry.trackLast - next.delay;
                    if (num4 < 0f)
                    {
                        goto Label_0171;
                    }
                    next.delay = 0f;
                    next.trackTime = num4 + (delta * next.timeScale);
                    entry.trackTime += num3;
                    this.SetCurrent(index, next, true);
                    while (next.mixingFrom != null)
                    {
                        next.mixTime += num3;
                        next = next.mixingFrom;
                    }
                    goto Label_01C9;
                }
                if ((entry.trackLast >= entry.trackEnd) && (entry.mixingFrom == null))
                {
                    items[index] = null;
                    this.queue.End(entry);
                    this.DisposeNext(entry);
                    goto Label_01C9;
                }
            Label_0171:
                if ((entry.mixingFrom != null) && this.UpdateMixingFrom(entry, delta))
                {
                    TrackEntry mixingFrom = entry.mixingFrom;
                    entry.mixingFrom = null;
                    while (mixingFrom != null)
                    {
                        this.queue.End(mixingFrom);
                        mixingFrom = mixingFrom.mixingFrom;
                    }
                }
                entry.trackTime += num3;
            Label_01C9:
                index++;
            }
            this.queue.Drain();
        }

        private bool UpdateMixingFrom(TrackEntry to, float delta)
        {
            TrackEntry mixingFrom = to.mixingFrom;
            if (mixingFrom == null)
            {
                return true;
            }
            bool flag = this.UpdateMixingFrom(mixingFrom, delta);
            mixingFrom.animationLast = mixingFrom.nextAnimationLast;
            mixingFrom.trackLast = mixingFrom.nextTrackLast;
            if ((to.mixTime > 0f) && ((to.mixTime >= to.mixDuration) || (to.timeScale == 0f)))
            {
                if ((mixingFrom.totalAlpha == 0f) || (to.mixDuration == 0f))
                {
                    to.mixingFrom = mixingFrom.mixingFrom;
                    to.interruptAlpha = mixingFrom.interruptAlpha;
                    this.queue.End(mixingFrom);
                }
                return flag;
            }
            mixingFrom.trackTime += delta * mixingFrom.timeScale;
            to.mixTime += delta * to.timeScale;
            return false;
        }

        public AnimationStateData Data =>
            this.data;

        public ExposedList<TrackEntry> Tracks =>
            this.tracks;

        public float TimeScale
        {
            get => 
                this.timeScale;
            set => 
                (this.timeScale = value);
        }

        public delegate void TrackEntryDelegate(TrackEntry trackEntry);

        public delegate void TrackEntryEventDelegate(TrackEntry trackEntry, Event e);
    }
}

