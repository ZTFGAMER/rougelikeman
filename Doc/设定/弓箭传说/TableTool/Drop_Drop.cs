namespace TableTool
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class Drop_Drop : LocalBean
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <DropID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Notes>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <DropType>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string[] <Prob>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string[] <Rand1>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string[] <Rand2>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string[] <Rand3>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string[] <Rand4>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string[] <Rand5>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string[] <Fixed>k__BackingField;

        public Drop_Drop Copy() => 
            new Drop_Drop { 
                DropID = this.DropID,
                Notes = this.Notes,
                DropType = this.DropType,
                Prob = this.Prob,
                Rand1 = this.Rand1,
                Rand2 = this.Rand2,
                Rand3 = this.Rand3,
                Rand4 = this.Rand4,
                Rand5 = this.Rand5,
                Fixed = this.Fixed
            };

        protected override bool ReadImpl()
        {
            this.DropID = base.readInt();
            this.Notes = base.readLocalString();
            this.DropType = base.readInt();
            this.Prob = base.readArraystring();
            this.Rand1 = base.readArraystring();
            this.Rand2 = base.readArraystring();
            this.Rand3 = base.readArraystring();
            this.Rand4 = base.readArraystring();
            this.Rand5 = base.readArraystring();
            this.Fixed = base.readArraystring();
            return true;
        }

        public int DropID { get; private set; }

        public string Notes { get; private set; }

        public int DropType { get; private set; }

        public string[] Prob { get; private set; }

        public string[] Rand1 { get; private set; }

        public string[] Rand2 { get; private set; }

        public string[] Rand3 { get; private set; }

        public string[] Rand4 { get; private set; }

        public string[] Rand5 { get; private set; }

        public string[] Fixed { get; private set; }
    }
}

