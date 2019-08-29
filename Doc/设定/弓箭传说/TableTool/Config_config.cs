namespace TableTool
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class Config_config : LocalBean
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <ID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Notes>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <Value>k__BackingField;

        public Config_config Copy() => 
            new Config_config { 
                ID = this.ID,
                Notes = this.Notes,
                Value = this.Value
            };

        protected override bool ReadImpl()
        {
            this.ID = base.readInt();
            this.Notes = base.readLocalString();
            this.Value = base.readFloat();
            return true;
        }

        public int ID { get; private set; }

        public string Notes { get; private set; }

        public float Value { get; private set; }
    }
}

