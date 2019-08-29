namespace TableTool
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class Box_TimeBox : LocalBean
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <ID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Time>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <DropId>k__BackingField;

        public Box_TimeBox Copy() => 
            new Box_TimeBox { 
                ID = this.ID,
                Time = this.Time,
                DropId = this.DropId
            };

        protected override bool ReadImpl()
        {
            this.ID = base.readInt();
            this.Time = base.readInt();
            this.DropId = base.readInt();
            return true;
        }

        public int ID { get; private set; }

        public int Time { get; private set; }

        public int DropId { get; private set; }
    }
}

