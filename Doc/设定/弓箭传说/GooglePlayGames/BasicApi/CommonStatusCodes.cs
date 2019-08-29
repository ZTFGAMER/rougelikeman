namespace GooglePlayGames.BasicApi
{
    using System;

    public enum CommonStatusCodes
    {
        SuccessCached = -1,
        Success = 0,
        ServiceMissing = 1,
        ServiceVersionUpdateRequired = 2,
        ServiceDisabled = 3,
        SignInRequired = 4,
        InvalidAccount = 5,
        ResolutionRequired = 6,
        NetworkError = 7,
        InternalError = 8,
        ServiceInvalid = 9,
        DeveloperError = 10,
        LicenseCheckFailed = 11,
        Error = 13,
        Interrupted = 14,
        Timeout = 15,
        Canceled = 0x10,
        ApiNotConnected = 0x11,
        AuthApiInvalidCredentials = 0xbb8,
        AuthApiAccessForbidden = 0xbb9,
        AuthApiClientError = 0xbba,
        AuthApiServerError = 0xbbb,
        AuthTokenError = 0xbbc,
        AuthUrlResolution = 0xbbd
    }
}

