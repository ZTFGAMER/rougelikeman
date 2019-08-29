namespace Spine
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;

    internal class EventQueue
    {
        private readonly List<EventQueueEntry> eventQueueEntries = new List<EventQueueEntry>();
        internal bool drainDisabled;
        private readonly AnimationState state;
        private readonly Pool<TrackEntry> trackEntryPool;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        internal event Action AnimationsChanged;

        internal EventQueue(AnimationState state, Action HandleAnimationsChanged, Pool<TrackEntry> trackEntryPool)
        {
            this.state = state;
            this.AnimationsChanged += HandleAnimationsChanged;
            this.trackEntryPool = trackEntryPool;
        }

        internal void Clear()
        {
            this.eventQueueEntries.Clear();
        }

        internal void Complete(TrackEntry entry)
        {
            this.eventQueueEntries.Add(new EventQueueEntry(EventType.Complete, entry, null));
        }

        internal void Dispose(TrackEntry entry)
        {
            this.eventQueueEntries.Add(new EventQueueEntry(EventType.Dispose, entry, null));
        }

        internal void Drain()
        {
            if (!this.drainDisabled)
            {
                this.drainDisabled = true;
                List<EventQueueEntry> eventQueueEntries = this.eventQueueEntries;
                AnimationState state = this.state;
                for (int i = 0; i < eventQueueEntries.Count; i++)
                {
                    EventQueueEntry entry = eventQueueEntries[i];
                    TrackEntry entry2 = entry.entry;
                    switch (entry.type)
                    {
                        case EventType.Start:
                        {
                            entry2.OnStart();
                            state.OnStart(entry2);
                            continue;
                        }
                        case EventType.Interrupt:
                        {
                            entry2.OnInterrupt();
                            state.OnInterrupt(entry2);
                            continue;
                        }
                        case EventType.End:
                            entry2.OnEnd();
                            state.OnEnd(entry2);
                            break;

                        case EventType.Dispose:
                            break;

                        case EventType.Complete:
                            goto Label_00C3;

                        case EventType.Event:
                            goto Label_00D7;

                        default:
                        {
                            continue;
                        }
                    }
                    entry2.OnDispose();
                    state.OnDispose(entry2);
                    this.trackEntryPool.Free(entry2);
                    continue;
                Label_00C3:
                    entry2.OnComplete();
                    state.OnComplete(entry2);
                    continue;
                Label_00D7:
                    entry2.OnEvent(entry.e);
                    state.OnEvent(entry2, entry.e);
                }
                this.eventQueueEntries.Clear();
                this.drainDisabled = false;
            }
        }

        internal void End(TrackEntry entry)
        {
            this.eventQueueEntries.Add(new EventQueueEntry(EventType.End, entry, null));
            if (this.AnimationsChanged != null)
            {
                this.AnimationsChanged();
            }
        }

        internal void Event(TrackEntry entry, Spine.Event e)
        {
            this.eventQueueEntries.Add(new EventQueueEntry(EventType.Event, entry, e));
        }

        internal void Interrupt(TrackEntry entry)
        {
            this.eventQueueEntries.Add(new EventQueueEntry(EventType.Interrupt, entry, null));
        }

        internal void Start(TrackEntry entry)
        {
            this.eventQueueEntries.Add(new EventQueueEntry(EventType.Start, entry, null));
            if (this.AnimationsChanged != null)
            {
                this.AnimationsChanged();
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct EventQueueEntry
        {
            public EventQueue.EventType type;
            public TrackEntry entry;
            public Event e;
            public EventQueueEntry(EventQueue.EventType eventType, TrackEntry trackEntry, Event e = null)
            {
                this.type = eventType;
                this.entry = trackEntry;
                this.e = e;
            }
        }

        private enum EventType
        {
            Start,
            Interrupt,
            End,
            Dispose,
            Complete,
            Event
        }
    }
}

