namespace TableTool
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class Shop_item : LocalBean
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <ItemID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Type>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Quality>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <EffectType>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string[] <EffectArgs>k__BackingField;

        public Shop_item Copy() => 
            new Shop_item { 
                ItemID = this.ItemID,
                Type = this.Type,
                Quality = this.Quality,
                EffectType = this.EffectType,
                EffectArgs = this.EffectArgs
            };

        protected override bool ReadImpl()
        {
            this.ItemID = base.readInt();
            this.Type = base.readInt();
            this.Quality = base.readInt();
            this.EffectType = base.readInt();
            this.EffectArgs = base.readArraystring();
            return true;
        }

        public int ItemID { get; private set; }

        public int Type { get; private set; }

        public int Quality { get; private set; }

        public int EffectType { get; private set; }

        public string[] EffectArgs { get; private set; }
    }
}

