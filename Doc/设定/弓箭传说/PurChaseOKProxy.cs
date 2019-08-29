using PureMVC.Patterns;
using System;

public class PurChaseOKProxy : Proxy
{
    public const string NAME = "PurChaseOKProxy";

    public PurChaseOKProxy(object data) : base("PurChaseOKProxy", data)
    {
    }

    public class Transfer
    {
        public int purchase_state;
        public string id;
        public string receipt;
    }
}

