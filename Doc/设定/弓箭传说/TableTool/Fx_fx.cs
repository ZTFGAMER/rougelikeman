namespace TableTool
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class Fx_fx : LocalBean
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <FxID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Notes>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Path>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Node>k__BackingField;

        public Fx_fx Copy() => 
            new Fx_fx { 
                FxID = this.FxID,
                Notes = this.Notes,
                Path = this.Path,
                Node = this.Node
            };

        protected override bool ReadImpl()
        {
            this.FxID = base.readInt();
            this.Notes = base.readLocalString();
            this.Path = base.readLocalString();
            this.Node = base.readInt();
            return true;
        }

        public int FxID { get; private set; }

        public string Notes { get; private set; }

        public string Path { get; private set; }

        public int Node { get; private set; }
    }
}

