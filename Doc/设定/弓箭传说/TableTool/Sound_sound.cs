namespace TableTool
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class Sound_sound : LocalBean
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <ID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Notes>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Path>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <Volumn>k__BackingField;

        public Sound_sound Copy() => 
            new Sound_sound { 
                ID = this.ID,
                Notes = this.Notes,
                Path = this.Path,
                Volumn = this.Volumn
            };

        protected override bool ReadImpl()
        {
            this.ID = base.readInt();
            this.Notes = base.readLocalString();
            this.Path = base.readLocalString();
            this.Volumn = base.readFloat();
            return true;
        }

        public int ID { get; private set; }

        public string Notes { get; private set; }

        public string Path { get; private set; }

        public float Volumn { get; private set; }
    }
}

