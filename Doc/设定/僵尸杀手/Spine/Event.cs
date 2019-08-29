namespace Spine
{
    using System;

    public class Event
    {
        internal readonly EventData data;
        internal readonly float time;
        internal int intValue;
        internal float floatValue;
        internal string stringValue;

        public Event(float time, EventData data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data", "data cannot be null.");
            }
            this.time = time;
            this.data = data;
        }

        public override string ToString() => 
            this.data.Name;

        public EventData Data =>
            this.data;

        public float Time =>
            this.time;

        public int Int
        {
            get => 
                this.intValue;
            set => 
                (this.intValue = value);
        }

        public float Float
        {
            get => 
                this.floatValue;
            set => 
                (this.floatValue = value);
        }

        public string String
        {
            get => 
                this.stringValue;
            set => 
                (this.stringValue = value);
        }
    }
}

