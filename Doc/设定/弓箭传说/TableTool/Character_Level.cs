namespace TableTool
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class Character_Level : LocalBean
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <ID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Exp>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string[] <Rewards>k__BackingField;

        public Character_Level Copy() => 
            new Character_Level { 
                ID = this.ID,
                Exp = this.Exp,
                Rewards = this.Rewards
            };

        protected override bool ReadImpl()
        {
            this.ID = base.readInt();
            this.Exp = base.readInt();
            this.Rewards = base.readArraystring();
            return true;
        }

        public int ID { get; private set; }

        public int Exp { get; private set; }

        public string[] Rewards { get; private set; }
    }
}

