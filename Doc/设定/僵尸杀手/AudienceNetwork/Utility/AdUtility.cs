namespace AudienceNetwork.Utility
{
    using System;

    public class AdUtility
    {
        internal static double convert(double deviceSize) => 
            AdUtilityBridge.Instance.convert(deviceSize);

        internal static double height() => 
            AdUtilityBridge.Instance.height();

        internal static void prepare()
        {
            AdUtilityBridge.Instance.prepare();
        }

        internal static double width() => 
            AdUtilityBridge.Instance.width();
    }
}

