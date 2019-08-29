using PureMVC.Patterns;
using System;

public class BattleLoadProxy : Proxy
{
    public const string NAME = "BattleLoadProxy";

    public BattleLoadProxy(object data) : base("BattleLoadProxy", data)
    {
    }

    public class BattleLoadData
    {
        public Action LoadingDo;
        public Action LoadEnd1Do;
        public Action LoadEnd2Do;
        public BattleLoadProxy.LoadingType loadingType;

        public bool showLoading =>
            (this.loadingType == BattleLoadProxy.LoadingType.eFirstBattle);
    }

    public enum LoadingType
    {
        eMiss,
        eFirstBattle
    }
}

