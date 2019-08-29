namespace TableTool
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class Drop_Gold : LocalBean
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <ID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string[] <GoldDropLevel>k__BackingField;

        public Drop_Gold Copy() => 
            new Drop_Gold { 
                ID = this.ID,
                GoldDropLevel = this.GoldDropLevel
            };

        protected override bool ReadImpl()
        {
            this.ID = base.readInt();
            this.GoldDropLevel = base.readArraystring();
            return true;
        }

        public int ID { get; private set; }

        public string[] GoldDropLevel { get; private set; }
    }
}

