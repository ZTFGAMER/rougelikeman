namespace TableTool
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class Box_ChapterBox : LocalBean
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <ID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Chapter>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string[] <Reward>k__BackingField;

        public Box_ChapterBox Copy() => 
            new Box_ChapterBox { 
                ID = this.ID,
                Chapter = this.Chapter,
                Reward = this.Reward
            };

        protected override bool ReadImpl()
        {
            this.ID = base.readInt();
            this.Chapter = base.readInt();
            this.Reward = base.readArraystring();
            return true;
        }

        public int ID { get; private set; }

        public int Chapter { get; private set; }

        public string[] Reward { get; private set; }
    }
}

