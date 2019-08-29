namespace TableTool
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class Exp_exp : LocalBean
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <LevelID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Notes>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Exp>k__BackingField;

        public Exp_exp Copy() => 
            new Exp_exp { 
                LevelID = this.LevelID,
                Notes = this.Notes,
                Exp = this.Exp
            };

        protected override bool ReadImpl()
        {
            this.LevelID = base.readInt();
            this.Notes = base.readLocalString();
            this.Exp = base.readInt();
            return true;
        }

        public int LevelID { get; private set; }

        public string Notes { get; private set; }

        public int Exp { get; private set; }
    }
}

