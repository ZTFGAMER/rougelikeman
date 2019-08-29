namespace TableTool
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class Room_eventdemontext2lose : LocalBean
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <EventID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Notes>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Content1>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Content2>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Image1>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <LoseID>k__BackingField;

        public Room_eventdemontext2lose Copy() => 
            new Room_eventdemontext2lose { 
                EventID = this.EventID,
                Notes = this.Notes,
                Content1 = this.Content1,
                Content2 = this.Content2,
                Image1 = this.Image1,
                LoseID = this.LoseID
            };

        protected override bool ReadImpl()
        {
            this.EventID = base.readInt();
            this.Notes = base.readLocalString();
            this.Content1 = base.readLocalString();
            this.Content2 = base.readLocalString();
            this.Image1 = base.readLocalString();
            this.LoseID = base.readInt();
            return true;
        }

        public int EventID { get; private set; }

        public string Notes { get; private set; }

        public string Content1 { get; private set; }

        public string Content2 { get; private set; }

        public string Image1 { get; private set; }

        public int LoseID { get; private set; }
    }
}

