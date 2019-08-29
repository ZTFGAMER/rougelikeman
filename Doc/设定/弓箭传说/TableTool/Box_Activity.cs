namespace TableTool
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class Box_Activity : LocalBean
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <ID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <PayId>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string[] <ShowCond>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string[] <CloseCond>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string[] <Reward>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Multiple>k__BackingField;

        public Box_Activity Copy() => 
            new Box_Activity { 
                ID = this.ID,
                PayId = this.PayId,
                ShowCond = this.ShowCond,
                CloseCond = this.CloseCond,
                Reward = this.Reward,
                Multiple = this.Multiple
            };

        protected override bool ReadImpl()
        {
            this.ID = base.readInt();
            this.PayId = base.readFloat();
            this.ShowCond = base.readArraystring();
            this.CloseCond = base.readArraystring();
            this.Reward = base.readArraystring();
            this.Multiple = base.readInt();
            return true;
        }

        public int ID { get; private set; }

        public float PayId { get; private set; }

        public string[] ShowCond { get; private set; }

        public string[] CloseCond { get; private set; }

        public string[] Reward { get; private set; }

        public int Multiple { get; private set; }
    }
}

