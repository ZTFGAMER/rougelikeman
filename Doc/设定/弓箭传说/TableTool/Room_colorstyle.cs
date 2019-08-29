namespace TableTool
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class Room_colorstyle : LocalBean
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <ID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Notes>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <StyleID>k__BackingField;

        public Room_colorstyle Copy() => 
            new Room_colorstyle { 
                ID = this.ID,
                Notes = this.Notes,
                StyleID = this.StyleID
            };

        protected override bool ReadImpl()
        {
            this.ID = base.readInt();
            this.Notes = base.readLocalString();
            this.StyleID = base.readInt();
            return true;
        }

        public int ID { get; private set; }

        public string Notes { get; private set; }

        public int StyleID { get; private set; }
    }
}

