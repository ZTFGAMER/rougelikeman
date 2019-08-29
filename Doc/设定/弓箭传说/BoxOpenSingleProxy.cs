using PureMVC.Patterns;
using System;
using TableTool;

public class BoxOpenSingleProxy : Proxy
{
    public const string NAME = "BoxOpenSingleProxy";

    public BoxOpenSingleProxy(object data) : base("BoxOpenSingleProxy", data)
    {
    }

    public class Transfer
    {
        public EquipSource source;
        public LocalSave.TimeBoxType boxtype;
        public Drop_DropModel.DropData data;
        public int[] diamonds;
        public int count = 1;
        public Action retry_callback;

        public void AddCount()
        {
            this.count++;
        }

        public int GetCurrentDiamond()
        {
            if (this.diamonds.Length > 0)
            {
                string str = string.Empty;
                for (int i = 0; i < this.diamonds.Length; i++)
                {
                    str = str + this.diamonds[i] + ",";
                }
                if (this.count < this.diamonds.Length)
                {
                    return this.diamonds[this.count];
                }
                return this.diamonds[this.diamonds.Length - 1];
            }
            SdkManager.Bugly_Report("BoxOpenSingleProxy", "diamonds is null");
            return 0;
        }

        public int GetStartDiamond()
        {
            if (this.diamonds.Length > 0)
            {
                return this.diamonds[0];
            }
            SdkManager.Bugly_Report("BoxOpenSingleProxy", "diamonds is null");
            return 0;
        }

        public void ResetCount()
        {
            this.count = 0;
        }
    }
}

