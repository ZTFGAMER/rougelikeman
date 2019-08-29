using PureMVC.Patterns;
using System;

public class ChooseSkillProxy : Proxy
{
    public const string NAME = "ChooseSkillProxy";

    public ChooseSkillProxy(object data) : base("ChooseSkillProxy", data)
    {
    }

    public enum ChooseSkillType
    {
        eLevel,
        eFirst
    }

    public class Transfer
    {
        public ChooseSkillProxy.ChooseSkillType type;
    }
}

