namespace IAP
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    [Serializable]
    public class StarterPackPurchaseInfo : CoinsPurchaseInfo
    {
        public PackType starterType;
        public List<Boosters> boosters;
        public string displayedPackName = "Starter Pack";

        [Serializable, StructLayout(LayoutKind.Sequential)]
        public struct Boosters
        {
            public SaveData.BoostersData.BoosterType boosterType;
            public int amount;
        }
    }
}

