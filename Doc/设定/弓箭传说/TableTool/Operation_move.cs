namespace TableTool
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class Operation_move : LocalBean
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <MoveStateID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Notes>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <AttackRemove>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string[] <Args>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Args_note>k__BackingField;

        public Operation_move Copy() => 
            new Operation_move { 
                MoveStateID = this.MoveStateID,
                Notes = this.Notes,
                AttackRemove = this.AttackRemove,
                Args = this.Args,
                Args_note = this.Args_note
            };

        protected override bool ReadImpl()
        {
            this.MoveStateID = base.readInt();
            this.Notes = base.readLocalString();
            this.AttackRemove = base.readInt();
            this.Args = base.readArraystring();
            this.Args_note = base.readLocalString();
            return true;
        }

        public int MoveStateID { get; private set; }

        public string Notes { get; private set; }

        public int AttackRemove { get; private set; }

        public string[] Args { get; private set; }

        public string Args_note { get; private set; }
    }
}

