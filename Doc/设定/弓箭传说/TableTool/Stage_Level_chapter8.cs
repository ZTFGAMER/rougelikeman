namespace TableTool
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class Stage_Level_chapter8 : LocalBean
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <RoomID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Notes>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string[] <Attributes>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string[] <MapAttributes>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private long <StandardDefence>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string[] <RoomIDs>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string[] <RoomIDs1>k__BackingField;

        public Stage_Level_chapter8 Copy() => 
            new Stage_Level_chapter8 { 
                RoomID = this.RoomID,
                Notes = this.Notes,
                Attributes = this.Attributes,
                MapAttributes = this.MapAttributes,
                StandardDefence = this.StandardDefence,
                RoomIDs = this.RoomIDs,
                RoomIDs1 = this.RoomIDs1
            };

        protected override bool ReadImpl()
        {
            this.RoomID = base.readLocalString();
            this.Notes = base.readLocalString();
            this.Attributes = base.readArraystring();
            this.MapAttributes = base.readArraystring();
            this.StandardDefence = base.readLong();
            this.RoomIDs = base.readArraystring();
            this.RoomIDs1 = base.readArraystring();
            return true;
        }

        public string RoomID { get; private set; }

        public string Notes { get; private set; }

        public string[] Attributes { get; private set; }

        public string[] MapAttributes { get; private set; }

        public long StandardDefence { get; private set; }

        public string[] RoomIDs { get; private set; }

        public string[] RoomIDs1 { get; private set; }
    }
}

