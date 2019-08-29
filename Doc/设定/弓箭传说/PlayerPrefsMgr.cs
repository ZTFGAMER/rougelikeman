using Dxx.Util;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

public class PlayerPrefsMgr : CInstance<PlayerPrefsMgr>
{
    public PrefDataInt gametime = new PrefDataInt("gametime");
    public PrefDataInt apptime = new PrefDataInt("apptime");

    public void Init()
    {
    }

    public abstract class PrefDataBase
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <name>k__BackingField;

        public PrefDataBase(string name)
        {
            object[] args = new object[] { name };
            this.name = Utils.FormatString("PlayerPrefsMgr_{0}", args);
        }

        public void flush()
        {
            this.OnFlush();
        }

        protected abstract void OnFlush();

        protected string name { get; private set; }
    }

    public class PrefDataInt : PlayerPrefsMgr.PrefDataBase
    {
        private int value;

        public PrefDataInt(string name) : base(name)
        {
            this.value = PlayerPrefsEncrypt.GetInt(base.name, 0);
        }

        public int get_value() => 
            this.value;

        protected override void OnFlush()
        {
            PlayerPrefsEncrypt.SetInt(base.name, this.value);
        }

        public void set_value(int t)
        {
            this.value = t;
        }
    }
}

