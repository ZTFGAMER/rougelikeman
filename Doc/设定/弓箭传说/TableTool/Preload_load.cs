namespace TableTool
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class Preload_load : LocalBean
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <RoomID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Notes>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string[] <PlayerBulletsPath>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string[] <BulletsPath>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string[] <EffectsPath>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string[] <MapEffectsPath>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string[] <GoodsPath>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int[] <SoundPath>k__BackingField;

        public Preload_load Copy() => 
            new Preload_load { 
                RoomID = this.RoomID,
                Notes = this.Notes,
                PlayerBulletsPath = this.PlayerBulletsPath,
                BulletsPath = this.BulletsPath,
                EffectsPath = this.EffectsPath,
                MapEffectsPath = this.MapEffectsPath,
                GoodsPath = this.GoodsPath,
                SoundPath = this.SoundPath
            };

        protected override bool ReadImpl()
        {
            this.RoomID = base.readInt();
            this.Notes = base.readLocalString();
            this.PlayerBulletsPath = base.readArraystring();
            this.BulletsPath = base.readArraystring();
            this.EffectsPath = base.readArraystring();
            this.MapEffectsPath = base.readArraystring();
            this.GoodsPath = base.readArraystring();
            this.SoundPath = base.readArrayint();
            return true;
        }

        public int RoomID { get; private set; }

        public string Notes { get; private set; }

        public string[] PlayerBulletsPath { get; private set; }

        public string[] BulletsPath { get; private set; }

        public string[] EffectsPath { get; private set; }

        public string[] MapEffectsPath { get; private set; }

        public string[] GoodsPath { get; private set; }

        public int[] SoundPath { get; private set; }
    }
}

