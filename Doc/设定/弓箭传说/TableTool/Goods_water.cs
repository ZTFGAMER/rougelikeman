namespace TableTool
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class Goods_water : LocalBean
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <CheckID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int[] <WaterID>k__BackingField;

        public Goods_water Copy() => 
            new Goods_water { 
                CheckID = this.CheckID,
                WaterID = this.WaterID
            };

        protected override bool ReadImpl()
        {
            this.CheckID = base.readLocalString();
            this.WaterID = base.readArrayint();
            return true;
        }

        public string CheckID { get; private set; }

        public int[] WaterID { get; private set; }
    }
}

