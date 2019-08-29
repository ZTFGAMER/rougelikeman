namespace TableTool
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class Room_level : LocalBean
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <LevelID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Notes>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string[] <RoomIDs>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string[] <RoomIDs1>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string[] <RoomIDs2>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string[] <RoomIDs3>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string[] <RoomIDs4>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string[] <RoomIDs5>k__BackingField;
        private List<string[]> mList = new List<string[]>();

        public Room_level Copy() => 
            new Room_level { 
                LevelID = this.LevelID,
                Notes = this.Notes,
                RoomIDs = this.RoomIDs,
                RoomIDs1 = this.RoomIDs1,
                RoomIDs2 = this.RoomIDs2,
                RoomIDs3 = this.RoomIDs3,
                RoomIDs4 = this.RoomIDs4,
                RoomIDs5 = this.RoomIDs5
            };

        public string[] GetList(int layer, int count)
        {
            if (this.mList.Count == 0)
            {
                this.mList.Add(this.RoomIDs1);
                this.mList.Add(this.RoomIDs2);
                this.mList.Add(this.RoomIDs3);
                this.mList.Add(this.RoomIDs4);
                this.mList.Add(this.RoomIDs5);
            }
            if ((count < this.mList.Count) && (this.mList[count].Length > 0))
            {
                return this.mList[count];
            }
            return this.RoomIDs;
        }

        protected override bool ReadImpl()
        {
            this.LevelID = base.readInt();
            this.Notes = base.readLocalString();
            this.RoomIDs = base.readArraystring();
            this.RoomIDs1 = base.readArraystring();
            this.RoomIDs2 = base.readArraystring();
            this.RoomIDs3 = base.readArraystring();
            this.RoomIDs4 = base.readArraystring();
            this.RoomIDs5 = base.readArraystring();
            return true;
        }

        public int LevelID { get; private set; }

        public string Notes { get; private set; }

        public string[] RoomIDs { get; private set; }

        public string[] RoomIDs1 { get; private set; }

        public string[] RoomIDs2 { get; private set; }

        public string[] RoomIDs3 { get; private set; }

        public string[] RoomIDs4 { get; private set; }

        public string[] RoomIDs5 { get; private set; }
    }
}

