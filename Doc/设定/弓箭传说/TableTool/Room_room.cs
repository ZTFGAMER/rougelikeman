namespace TableTool
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class Room_room : LocalBean
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <RoomID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Notes>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Difficult>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float[] <GoodsOffset>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Shape>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float[] <CameraRound>k__BackingField;

        public Room_room Copy() => 
            new Room_room { 
                RoomID = this.RoomID,
                Notes = this.Notes,
                Difficult = this.Difficult,
                GoodsOffset = this.GoodsOffset,
                Shape = this.Shape,
                CameraRound = this.CameraRound
            };

        protected override bool ReadImpl()
        {
            this.RoomID = base.readInt();
            this.Notes = base.readLocalString();
            this.Difficult = base.readInt();
            this.GoodsOffset = base.readArrayfloat();
            this.Shape = base.readInt();
            this.CameraRound = base.readArrayfloat();
            return true;
        }

        public int RoomID { get; private set; }

        public string Notes { get; private set; }

        public int Difficult { get; private set; }

        public float[] GoodsOffset { get; private set; }

        public int Shape { get; private set; }

        public float[] CameraRound { get; private set; }
    }
}

