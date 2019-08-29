namespace TableTool
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class Character_Baby : LocalBean
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <BabyID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <AttackValue>k__BackingField;

        public Character_Baby Copy() => 
            new Character_Baby { 
                BabyID = this.BabyID,
                AttackValue = this.AttackValue
            };

        protected override bool ReadImpl()
        {
            this.BabyID = base.readLocalString();
            this.AttackValue = base.readInt();
            return true;
        }

        public string BabyID { get; private set; }

        public int AttackValue { get; private set; }
    }
}

