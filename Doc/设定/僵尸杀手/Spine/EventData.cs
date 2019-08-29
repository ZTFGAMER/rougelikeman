namespace Spine
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class EventData
    {
        internal string name;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Int>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <Float>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <String>k__BackingField;

        public EventData(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name", "name cannot be null.");
            }
            this.name = name;
        }

        public override string ToString() => 
            this.Name;

        public string Name =>
            this.name;

        public int Int { get; set; }

        public float Float { get; set; }

        public string String { get; set; }
    }
}

