namespace TableTool
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class Box_SilverBox : LocalBean
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <ID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Type>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int[] <Price1>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Price10>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Time>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <SingleDrop>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <GiftDrop>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <PurpleDrop>k__BackingField;

        public Box_SilverBox Copy() => 
            new Box_SilverBox { 
                ID = this.ID,
                Type = this.Type,
                Price1 = this.Price1,
                Price10 = this.Price10,
                Time = this.Time,
                SingleDrop = this.SingleDrop,
                GiftDrop = this.GiftDrop,
                PurpleDrop = this.PurpleDrop
            };

        protected override bool ReadImpl()
        {
            this.ID = base.readInt();
            this.Type = base.readInt();
            this.Price1 = base.readArrayint();
            this.Price10 = base.readInt();
            this.Time = base.readInt();
            this.SingleDrop = base.readInt();
            this.GiftDrop = base.readInt();
            this.PurpleDrop = base.readInt();
            return true;
        }

        public int ID { get; private set; }

        public int Type { get; private set; }

        public int[] Price1 { get; private set; }

        public int Price10 { get; private set; }

        public int Time { get; private set; }

        public int SingleDrop { get; private set; }

        public int GiftDrop { get; private set; }

        public int PurpleDrop { get; private set; }
    }
}

