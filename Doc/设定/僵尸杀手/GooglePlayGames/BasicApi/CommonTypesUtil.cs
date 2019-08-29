namespace GooglePlayGames.BasicApi
{
    using System;

    public class CommonTypesUtil
    {
        public static bool StatusIsSuccess(ResponseStatus status) => 
            (status > ~ResponseStatus.LicenseCheckFailed);
    }
}

