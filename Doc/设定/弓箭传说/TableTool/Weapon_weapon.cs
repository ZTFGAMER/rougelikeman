namespace TableTool
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class Weapon_weapon : LocalBean
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <WeaponID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <SpecialAttribute>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <ModelID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <ModelScale>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string[] <Attributes>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <DebuffID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <LookCamera>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Attack>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <Distance>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <Speed>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <AttackSpeed>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <RandomAngle>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <WeaponNode>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <CreateNode>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <RotateSpeed>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <AttackPrevString>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <AttackEndString>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Ballistic>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <BackRatio>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <CreatePath>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <CreateSoundID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <DeadSoundID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <HitWallSoundID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <HittedEffectID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <AliveTime>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <DeadDelay>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <DeadEffectID>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <DeadNode>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string[] <Args>k__BackingField;
        private bool bInitSpecial;
        private bool bCachep;
        private bool bThroughWallp;
        private bool bThroughEntityp;
        private bool bThroughInsideWallp;
        private bool bMoreHitp;
        private LayerManager.BulletLayer mLayer;

        public Weapon_weapon Copy() => 
            new Weapon_weapon { 
                WeaponID = this.WeaponID,
                SpecialAttribute = this.SpecialAttribute,
                ModelID = this.ModelID,
                ModelScale = this.ModelScale,
                Attributes = this.Attributes,
                DebuffID = this.DebuffID,
                LookCamera = this.LookCamera,
                Attack = this.Attack,
                Distance = this.Distance,
                Speed = this.Speed,
                AttackSpeed = this.AttackSpeed,
                RandomAngle = this.RandomAngle,
                WeaponNode = this.WeaponNode,
                CreateNode = this.CreateNode,
                RotateSpeed = this.RotateSpeed,
                AttackPrevString = this.AttackPrevString,
                AttackEndString = this.AttackEndString,
                Ballistic = this.Ballistic,
                BackRatio = this.BackRatio,
                CreatePath = this.CreatePath,
                CreateSoundID = this.CreateSoundID,
                DeadSoundID = this.DeadSoundID,
                HitWallSoundID = this.HitWallSoundID,
                HittedEffectID = this.HittedEffectID,
                AliveTime = this.AliveTime,
                DeadDelay = this.DeadDelay,
                DeadEffectID = this.DeadEffectID,
                DeadNode = this.DeadNode,
                Args = this.Args
            };

        public int GetLayer()
        {
            if (this.bThroughWall)
            {
                this.mLayer = LayerManager.BulletLayer.eNone;
            }
            else if (this.bThroughInsideWall)
            {
                this.mLayer = LayerManager.BulletLayer.eOnlyOut;
            }
            else
            {
                this.mLayer = LayerManager.BulletLayer.eAll;
            }
            return LayerManager.GetBullet(this.mLayer);
        }

        private void InitSpecial()
        {
            if (!this.bInitSpecial)
            {
                this.bInitSpecial = true;
                this.bCachep = false;
                this.bThroughWallp = false;
                this.bThroughEntityp = false;
                this.bThroughInsideWallp = false;
                this.bMoreHitp = false;
                int specialAttribute = this.SpecialAttribute;
                if (specialAttribute >= 0x10)
                {
                    this.bMoreHitp = true;
                    specialAttribute -= 0x10;
                }
                if (specialAttribute >= 8)
                {
                    this.bThroughInsideWallp = true;
                    specialAttribute -= 8;
                }
                if (specialAttribute >= 4)
                {
                    this.bThroughEntityp = true;
                    specialAttribute -= 4;
                }
                if (specialAttribute >= 2)
                {
                    this.bThroughWallp = true;
                    specialAttribute -= 2;
                }
                if (specialAttribute >= 1)
                {
                    this.bCachep = true;
                    specialAttribute--;
                }
            }
        }

        protected override bool ReadImpl()
        {
            this.WeaponID = base.readInt();
            this.SpecialAttribute = base.readInt();
            this.ModelID = base.readLocalString();
            this.ModelScale = base.readFloat();
            this.Attributes = base.readArraystring();
            this.DebuffID = base.readInt();
            this.LookCamera = base.readInt();
            this.Attack = base.readInt();
            this.Distance = base.readFloat();
            this.Speed = base.readFloat();
            this.AttackSpeed = base.readFloat();
            this.RandomAngle = base.readFloat();
            this.WeaponNode = base.readInt();
            this.CreateNode = base.readInt();
            this.RotateSpeed = base.readFloat();
            this.AttackPrevString = base.readLocalString();
            this.AttackEndString = base.readLocalString();
            this.Ballistic = base.readInt();
            this.BackRatio = base.readFloat();
            this.CreatePath = base.readLocalString();
            this.CreateSoundID = base.readInt();
            this.DeadSoundID = base.readInt();
            this.HitWallSoundID = base.readInt();
            this.HittedEffectID = base.readInt();
            this.AliveTime = base.readInt();
            this.DeadDelay = base.readInt();
            this.DeadEffectID = base.readInt();
            this.DeadNode = base.readInt();
            this.Args = base.readArraystring();
            return true;
        }

        public int WeaponID { get; private set; }

        public int SpecialAttribute { get; private set; }

        public string ModelID { get; private set; }

        public float ModelScale { get; private set; }

        public string[] Attributes { get; private set; }

        public int DebuffID { get; private set; }

        public int LookCamera { get; private set; }

        public int Attack { get; private set; }

        public float Distance { get; private set; }

        public float Speed { get; private set; }

        public float AttackSpeed { get; private set; }

        public float RandomAngle { get; private set; }

        public int WeaponNode { get; private set; }

        public int CreateNode { get; private set; }

        public float RotateSpeed { get; private set; }

        public string AttackPrevString { get; private set; }

        public string AttackEndString { get; private set; }

        public int Ballistic { get; private set; }

        public float BackRatio { get; private set; }

        public string CreatePath { get; private set; }

        public int CreateSoundID { get; private set; }

        public int DeadSoundID { get; private set; }

        public int HitWallSoundID { get; private set; }

        public int HittedEffectID { get; private set; }

        public int AliveTime { get; private set; }

        public int DeadDelay { get; private set; }

        public int DeadEffectID { get; private set; }

        public int DeadNode { get; private set; }

        public string[] Args { get; private set; }

        public bool bCache
        {
            get
            {
                this.InitSpecial();
                return this.bCachep;
            }
        }

        public bool bThroughWall
        {
            get
            {
                this.InitSpecial();
                return this.bThroughWallp;
            }
        }

        public bool bThroughEntity
        {
            get
            {
                this.InitSpecial();
                return (this.bThroughEntityp || this.bCachep);
            }
        }

        public bool bThroughInsideWall
        {
            get
            {
                this.InitSpecial();
                return this.bThroughInsideWallp;
            }
        }

        public bool bMoreHit
        {
            get
            {
                this.InitSpecial();
                return this.bMoreHitp;
            }
        }
    }
}

