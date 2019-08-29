namespace TableTool
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class Beat_beat : LocalBean
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <ID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Score>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <Rate>k__BackingField;

        public Beat_beat Copy() => 
            new Beat_beat { 
                ID = this.ID,
                Score = this.Score,
                Rate = this.Rate
            };

        protected override bool ReadImpl()
        {
            this.ID = base.readInt();
            this.Score = base.readInt();
            this.Rate = base.readFloat();
            return true;
        }

        public int ID { get; private set; }

        public int Score { get; private set; }

        public float Rate { get; private set; }
    }
}

