using PureMVC.Patterns;
using System;
using System.Collections.Generic;

public class BoxOpenProxy : Proxy
{
    public const string NAME = "BoxOpenProxy";

    public BoxOpenProxy(object data) : base("BoxOpenProxy", data)
    {
    }

    public class Transfer
    {
        public EquipSource source;
        public List<Drop_DropModel.DropData> list;
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
                if (this.count < this.diamonds.Length)
                {
                    return this.diamonds[this.count];
                }
                return this.diamonds[this.diamonds.Length - 1];
            }
            SdkManager.Bugly_Report("BoxOpenProxy", "diamonds is null");
            return 0;
        }

        public int GetStartDiamond()
        {
            if (this.diamonds.Length > 0)
            {
                return this.diamonds[0];
            }
            return 0;
        }
    }
}

