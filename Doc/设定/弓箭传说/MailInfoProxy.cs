using GameProtocol;
using PureMVC.Patterns;
using System;

public class MailInfoProxy : Proxy
{
    public const string NAME = "MailInfoProxy";

    public MailInfoProxy(object data) : base("MailInfoProxy", data)
    {
    }

    public enum EMailPopType
    {
        eNormal,
        eMain
    }

    public class Transfer
    {
        public CMailInfo data;
        public MailOneCtrl ctrl;
        public MailInfoProxy.EMailPopType poptype;
    }
}

