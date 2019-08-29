namespace TableTool
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class Room_eventdemontext2skill : LocalBean
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <EventID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Notes>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int[] <Loses>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <GetID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Weight>k__BackingField;

        public Room_eventdemontext2skill Copy() => 
            new Room_eventdemontext2skill { 
                EventID = this.EventID,
                Notes = this.Notes,
                Loses = this.Loses,
                GetID = this.GetID,
                Weight = this.Weight
            };

        protected override bool ReadImpl()
        {
            this.EventID = base.readInt();
            this.Notes = base.readLocalString();
            this.Loses = base.readArrayint();
            this.GetID = base.readInt();
            this.Weight = base.readInt();
            return true;
        }

        public int EventID { get; private set; }

        public string Notes { get; private set; }

        public int[] Loses { get; private set; }

        public int GetID { get; private set; }

        public int Weight { get; private set; }
    }
}

