namespace TableTool
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class Equip2_equip2 : LocalBean
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Id>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Name>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Position>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Type>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Icon>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <DropIcon>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <BaseAttributes>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <LevelBaseAttributes>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string[] <AdditionSkills>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int[] <UnlockCondition>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int[] <SuperID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int[] <CoinCost>k__BackingField;

        public Equip2_equip2 Copy() => 
            new Equip2_equip2 { 
                Id = this.Id,
                Name = this.Name,
                Position = this.Position,
                Type = this.Type,
                Icon = this.Icon,
                DropIcon = this.DropIcon,
                BaseAttributes = this.BaseAttributes,
                LevelBaseAttributes = this.LevelBaseAttributes,
                AdditionSkills = this.AdditionSkills,
                UnlockCondition = this.UnlockCondition,
                SuperID = this.SuperID,
                CoinCost = this.CoinCost
            };

        protected override bool ReadImpl()
        {
            this.Id = base.readInt();
            this.Name = base.readLocalString();
            this.Position = base.readInt();
            this.Type = base.readInt();
            this.Icon = base.readInt();
            this.DropIcon = base.readInt();
            this.BaseAttributes = base.readLocalString();
            this.LevelBaseAttributes = base.readInt();
            this.AdditionSkills = base.readArraystring();
            this.UnlockCondition = base.readArrayint();
            this.SuperID = base.readArrayint();
            this.CoinCost = base.readArrayint();
            return true;
        }

        public int Id { get; private set; }

        public string Name { get; private set; }

        public int Position { get; private set; }

        public int Type { get; private set; }

        public int Icon { get; private set; }

        public int DropIcon { get; private set; }

        public string BaseAttributes { get; private set; }

        public int LevelBaseAttributes { get; private set; }

        public string[] AdditionSkills { get; private set; }

        public int[] UnlockCondition { get; private set; }

        public int[] SuperID { get; private set; }

        public int[] CoinCost { get; private set; }
    }
}

