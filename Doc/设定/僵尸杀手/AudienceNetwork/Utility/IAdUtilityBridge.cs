namespace AudienceNetwork.Utility
{
    using System;

    internal interface IAdUtilityBridge
    {
        double convert(double deviceSize);
        double deviceHeight();
        double deviceWidth();
        double height();
        void prepare();
        double width();
    }
}

