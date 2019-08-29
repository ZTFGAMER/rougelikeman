namespace TableTool
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class Drop_FakeDrop : LocalBean
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <ID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <DropID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <RandNum>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <JumpDrop>k__BackingField;

        public Drop_FakeDrop Copy() => 
            new Drop_FakeDrop { 
                ID = this.ID,
                DropID = this.DropID,
                RandNum = this.RandNum,
                JumpDrop = this.JumpDrop
            };

        protected override bool ReadImpl()
        {
            this.ID = base.readInt();
            this.DropID = base.readInt();
            this.RandNum = base.readInt();
            this.JumpDrop = base.readInt();
            return true;
        }

        public int ID { get; private set; }

        public int DropID { get; private set; }

        public int RandNum { get; private set; }

        public int JumpDrop { get; private set; }
    }
}

