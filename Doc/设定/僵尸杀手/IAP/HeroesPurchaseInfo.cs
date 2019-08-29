namespace IAP
{
    using System;

    [Serializable]
    public class HeroesPurchaseInfo : PurchaseInfo
    {
        public SaveData.HeroData.HeroType heroType;
    }
}

