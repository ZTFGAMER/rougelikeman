namespace TableTool
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class Room_eventgameturn : LocalBean
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <EventID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Notes>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <GetID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Weight>k__BackingField;

        public Room_eventgameturn Copy() => 
            new Room_eventgameturn { 
                EventID = this.EventID,
                Notes = this.Notes,
                GetID = this.GetID,
                Weight = this.Weight
            };

        protected override bool ReadImpl()
        {
            this.EventID = base.readInt();
            this.Notes = base.readLocalString();
            this.GetID = base.readInt();
            this.Weight = base.readInt();
            return true;
        }

        public int EventID { get; private set; }

        public string Notes { get; private set; }

        public int GetID { get; private set; }

        public int Weight { get; private set; }
    }
}

