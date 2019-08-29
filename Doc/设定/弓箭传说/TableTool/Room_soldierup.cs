namespace TableTool
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class Room_soldierup : LocalBean
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <RoomID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Notes>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string[] <Attributes>k__BackingField;

        public Room_soldierup Copy() => 
            new Room_soldierup { 
                RoomID = this.RoomID,
                Notes = this.Notes,
                Attributes = this.Attributes
            };

        protected override bool ReadImpl()
        {
            this.RoomID = base.readInt();
            this.Notes = base.readLocalString();
            this.Attributes = base.readArraystring();
            return true;
        }

        public int RoomID { get; private set; }

        public string Notes { get; private set; }

        public string[] Attributes { get; private set; }
    }
}

