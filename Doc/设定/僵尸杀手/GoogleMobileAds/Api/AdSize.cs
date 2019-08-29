namespace GoogleMobileAds.Api
{
    using System;

    public class AdSize
    {
        private bool isSmartBanner;
        private int width;
        private int height;
        public static readonly AdSize Banner = new AdSize(320, 50);
        public static readonly AdSize MediumRectangle = new AdSize(300, 250);
        public static readonly AdSize IABBanner = new AdSize(0x1d4, 60);
        public static readonly AdSize Leaderboard = new AdSize(0x2d8, 90);
        public static readonly AdSize SmartBanner = new AdSize(true);
        public static readonly int FullWidth = -1;

        private AdSize(bool isSmartBanner) : this(0, 0)
        {
            this.isSmartBanner = isSmartBanner;
        }

        public AdSize(int width, int height)
        {
            this.isSmartBanner = false;
            this.width = width;
            this.height = height;
        }

        public int Width =>
            this.width;

        public int Height =>
            this.height;

        public bool IsSmartBanner =>
            this.isSmartBanner;
    }
}

