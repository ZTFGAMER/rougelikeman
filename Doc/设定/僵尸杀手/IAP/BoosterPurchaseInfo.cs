namespace IAP
{
    using System;

    [Serializable]
    public class BoosterPurchaseInfo : PurchaseInfo
    {
        public SaveData.BoostersData.BoosterType boosterType;
        public int boosterCount;
    }
}

