namespace GooglePlayGames.Native
{
    using GooglePlayGames.BasicApi;
    using GooglePlayGames.Native.Cwrapper;
    using System;
    using UnityEngine;

    internal static class ConversionUtils
    {
        internal static GooglePlayGames.Native.Cwrapper.Types.DataSource AsDataSource(GooglePlayGames.BasicApi.DataSource source)
        {
            if (source != GooglePlayGames.BasicApi.DataSource.ReadCacheOrNetwork)
            {
                if (source != GooglePlayGames.BasicApi.DataSource.ReadNetworkOnly)
                {
                    throw new InvalidOperationException("Found unhandled DataSource: " + source);
                }
                return GooglePlayGames.Native.Cwrapper.Types.DataSource.NETWORK_ONLY;
            }
            return GooglePlayGames.Native.Cwrapper.Types.DataSource.CACHE_OR_NETWORK;
        }

        internal static GooglePlayGames.BasicApi.VideoCaptureMode ConvertNativeVideoCaptureMode(GooglePlayGames.Native.Cwrapper.Types.VideoCaptureMode nativeCaptureMode)
        {
            switch ((nativeCaptureMode + 1))
            {
                case GooglePlayGames.Native.Cwrapper.Types.VideoCaptureMode.FILE:
                    return GooglePlayGames.BasicApi.VideoCaptureMode.Unknown;

                case GooglePlayGames.Native.Cwrapper.Types.VideoCaptureMode.STREAM:
                    return GooglePlayGames.BasicApi.VideoCaptureMode.File;

                case ((GooglePlayGames.Native.Cwrapper.Types.VideoCaptureMode) 2):
                    return GooglePlayGames.BasicApi.VideoCaptureMode.Stream;
            }
            Debug.LogWarning("Unknown Types.VideoCaptureMode: " + nativeCaptureMode + ", defaulting to VideoCaptureMode.Unknown.");
            return GooglePlayGames.BasicApi.VideoCaptureMode.Unknown;
        }

        internal static GooglePlayGames.BasicApi.VideoCaptureOverlayState ConvertNativeVideoCaptureOverlayState(GooglePlayGames.Native.Cwrapper.Types.VideoCaptureOverlayState nativeOverlayState)
        {
            switch ((nativeOverlayState + 1))
            {
                case ~GooglePlayGames.Native.Cwrapper.Types.VideoCaptureOverlayState.UNKNOWN:
                    return GooglePlayGames.BasicApi.VideoCaptureOverlayState.Unknown;

                case GooglePlayGames.Native.Cwrapper.Types.VideoCaptureOverlayState.STARTED:
                    return GooglePlayGames.BasicApi.VideoCaptureOverlayState.Shown;

                case GooglePlayGames.Native.Cwrapper.Types.VideoCaptureOverlayState.STOPPED:
                    return GooglePlayGames.BasicApi.VideoCaptureOverlayState.Started;

                case GooglePlayGames.Native.Cwrapper.Types.VideoCaptureOverlayState.DISMISSED:
                    return GooglePlayGames.BasicApi.VideoCaptureOverlayState.Stopped;

                case (GooglePlayGames.Native.Cwrapper.Types.VideoCaptureOverlayState.DISMISSED | GooglePlayGames.Native.Cwrapper.Types.VideoCaptureOverlayState.SHOWN):
                    return GooglePlayGames.BasicApi.VideoCaptureOverlayState.Dismissed;
            }
            Debug.LogWarning("Unknown Types.VideoCaptureOverlayState: " + nativeOverlayState + ", defaulting to VideoCaptureOverlayState.Unknown.");
            return GooglePlayGames.BasicApi.VideoCaptureOverlayState.Unknown;
        }

        internal static GooglePlayGames.BasicApi.VideoQualityLevel ConvertNativeVideoQualityLevel(GooglePlayGames.Native.Cwrapper.Types.VideoQualityLevel nativeQualityLevel)
        {
            switch ((nativeQualityLevel + 1))
            {
                case GooglePlayGames.Native.Cwrapper.Types.VideoQualityLevel.SD:
                    return GooglePlayGames.BasicApi.VideoQualityLevel.Unknown;

                case GooglePlayGames.Native.Cwrapper.Types.VideoQualityLevel.HD:
                    return GooglePlayGames.BasicApi.VideoQualityLevel.SD;

                case GooglePlayGames.Native.Cwrapper.Types.VideoQualityLevel.XHD:
                    return GooglePlayGames.BasicApi.VideoQualityLevel.HD;

                case GooglePlayGames.Native.Cwrapper.Types.VideoQualityLevel.FULLHD:
                    return GooglePlayGames.BasicApi.VideoQualityLevel.XHD;

                case ((GooglePlayGames.Native.Cwrapper.Types.VideoQualityLevel) 4):
                    return GooglePlayGames.BasicApi.VideoQualityLevel.FullHD;
            }
            Debug.LogWarning("Unknown Types.VideoQualityLevel: " + nativeQualityLevel + ", defaulting to VideoQualityLevel.Unknown.");
            return GooglePlayGames.BasicApi.VideoQualityLevel.Unknown;
        }

        internal static GooglePlayGames.BasicApi.ResponseStatus ConvertResponseStatus(GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus status)
        {
            switch ((status + 5))
            {
                case ~GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.ERROR_LICENSE_CHECK_FAILED:
                    return GooglePlayGames.BasicApi.ResponseStatus.Timeout;

                case GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.VALID:
                    return GooglePlayGames.BasicApi.ResponseStatus.VersionUpdateRequired;

                case GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.VALID_BUT_STALE:
                    return GooglePlayGames.BasicApi.ResponseStatus.NotAuthorized;

                case ~GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.ERROR_VERSION_UPDATE_REQUIRED:
                    return GooglePlayGames.BasicApi.ResponseStatus.InternalError;

                case ~GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.ERROR_TIMEOUT:
                    return GooglePlayGames.BasicApi.ResponseStatus.LicenseCheckFailed;

                case ((GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus) 6):
                    return GooglePlayGames.BasicApi.ResponseStatus.Success;

                case ((GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus) 7):
                    return GooglePlayGames.BasicApi.ResponseStatus.SuccessWithStale;
            }
            throw new InvalidOperationException("Unknown status: " + status);
        }

        internal static CommonStatusCodes ConvertResponseStatusToCommonStatus(GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus status)
        {
            switch ((status + 5))
            {
                case ~GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.ERROR_LICENSE_CHECK_FAILED:
                    return CommonStatusCodes.Timeout;

                case GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.VALID:
                    return CommonStatusCodes.ServiceVersionUpdateRequired;

                case GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.VALID_BUT_STALE:
                    return CommonStatusCodes.AuthApiAccessForbidden;

                case ~GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.ERROR_VERSION_UPDATE_REQUIRED:
                    return CommonStatusCodes.InternalError;

                case ~GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.ERROR_TIMEOUT:
                    return CommonStatusCodes.LicenseCheckFailed;

                case ((GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus) 6):
                    return CommonStatusCodes.Success;

                case ((GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus) 7):
                    return CommonStatusCodes.SuccessCached;
            }
            Debug.LogWarning("Unknown ResponseStatus: " + status + ", defaulting to CommonStatusCodes.Error");
            return CommonStatusCodes.Error;
        }

        internal static GooglePlayGames.BasicApi.UIStatus ConvertUIStatus(GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus status)
        {
            switch ((status + 6))
            {
                case ~(GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus.VALID | GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus.ERROR_INTERNAL):
                    return GooglePlayGames.BasicApi.UIStatus.UserClosedUI;

                case GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus.VALID:
                    return GooglePlayGames.BasicApi.UIStatus.Timeout;

                case ~GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus.ERROR_NOT_AUTHORIZED:
                    return GooglePlayGames.BasicApi.UIStatus.VersionUpdateRequired;

                case ~GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus.ERROR_VERSION_UPDATE_REQUIRED:
                    return GooglePlayGames.BasicApi.UIStatus.NotAuthorized;

                case ~GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus.ERROR_TIMEOUT:
                    return GooglePlayGames.BasicApi.UIStatus.InternalError;

                case ((GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus) 7):
                    return GooglePlayGames.BasicApi.UIStatus.Valid;
            }
            if (status != GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus.ERROR_UI_BUSY)
            {
                throw new InvalidOperationException("Unknown status: " + status);
            }
            return GooglePlayGames.BasicApi.UIStatus.UiBusy;
        }

        internal static GooglePlayGames.Native.Cwrapper.Types.VideoCaptureMode ConvertVideoCaptureMode(GooglePlayGames.BasicApi.VideoCaptureMode captureMode)
        {
            switch ((captureMode + 1))
            {
                case GooglePlayGames.BasicApi.VideoCaptureMode.File:
                    return GooglePlayGames.Native.Cwrapper.Types.VideoCaptureMode.UNKNOWN;

                case GooglePlayGames.BasicApi.VideoCaptureMode.Stream:
                    return GooglePlayGames.Native.Cwrapper.Types.VideoCaptureMode.FILE;

                case ((GooglePlayGames.BasicApi.VideoCaptureMode) 2):
                    return GooglePlayGames.Native.Cwrapper.Types.VideoCaptureMode.STREAM;
            }
            Debug.LogWarning("Unknown VideoCaptureMode: " + captureMode + ", defaulting to Types.VideoCaptureMode.UNKNOWN.");
            return GooglePlayGames.Native.Cwrapper.Types.VideoCaptureMode.UNKNOWN;
        }
    }
}

