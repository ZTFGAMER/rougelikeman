namespace AudienceNetwork.Utility
{
    using System;
    using UnityEngine;

    internal class AdUtilityBridge : IAdUtilityBridge
    {
        public static readonly IAdUtilityBridge Instance = createInstance();

        internal AdUtilityBridge()
        {
        }

        public virtual double convert(double deviceSize) => 
            2.0;

        private static IAdUtilityBridge createInstance()
        {
            if (Application.platform != RuntimePlatform.OSXEditor)
            {
                return new AdUtilityBridgeAndroid();
            }
            return new AdUtilityBridge();
        }

        public virtual double deviceHeight() => 
            1242.0;

        public virtual double deviceWidth() => 
            2208.0;

        public virtual double height() => 
            621.0;

        public virtual void prepare()
        {
        }

        public virtual double width() => 
            1104.0;
    }
}

