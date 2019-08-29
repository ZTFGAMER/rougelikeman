namespace TableTool
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class Box_SilverNormalBox : LocalBean
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <ID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Type>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int[] <Price1>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Time>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <SingleDrop>k__BackingField;

        public Box_SilverNormalBox Copy() => 
            new Box_SilverNormalBox { 
                ID = this.ID,
                Type = this.Type,
                Price1 = this.Price1,
                Time = this.Time,
                SingleDrop = this.SingleDrop
            };

        protected override bool ReadImpl()
        {
            this.ID = base.readInt();
            this.Type = base.readInt();
            this.Price1 = base.readArrayint();
            this.Time = base.readInt();
            this.SingleDrop = base.readInt();
            return true;
        }

        public int ID { get; private set; }

        public int Type { get; private set; }

        public int[] Price1 { get; private set; }

        public int Time { get; private set; }

        public int SingleDrop { get; private set; }
    }
}

