namespace GooglePlayGames.Native
{
    using GooglePlayGames.BasicApi;
    using GooglePlayGames.Native.Cwrapper;
    using System;
    using UnityEngine;

    internal static class ConversionUtils
    {
        internal static Types.DataSource AsDataSource(DataSource source)
        {
            if (source != DataSource.ReadCacheOrNetwork)
            {
                if (source != DataSource.ReadNetworkOnly)
                {
                    throw new InvalidOperationException("Found unhandled DataSource: " + source);
                }
                return Types.DataSource.NETWORK_ONLY;
            }
            return Types.DataSource.CACHE_OR_NETWORK;
        }

        internal static VideoCaptureMode ConvertNativeVideoCaptureMode(Types.VideoCaptureMode nativeCaptureMode)
        {
            switch ((nativeCaptureMode + 1))
            {
                case Types.VideoCaptureMode.FILE:
                    return VideoCaptureMode.Unknown;

                case Types.VideoCaptureMode.STREAM:
                    return VideoCaptureMode.File;

                case ((Types.VideoCaptureMode) 2):
                    return VideoCaptureMode.Stream;
            }
            Debug.LogWarning("Unknown Types.VideoCaptureMode: " + nativeCaptureMode + ", defaulting to VideoCaptureMode.Unknown.");
            return VideoCaptureMode.Unknown;
        }

        internal static VideoCaptureOverlayState ConvertNativeVideoCaptureOverlayState(Types.VideoCaptureOverlayState nativeOverlayState)
        {
            switch ((nativeOverlayState + 1))
            {
                case ~Types.VideoCaptureOverlayState.UNKNOWN:
                    return VideoCaptureOverlayState.Unknown;

                case Types.VideoCaptureOverlayState.STARTED:
                    return VideoCaptureOverlayState.Shown;

                case Types.VideoCaptureOverlayState.STOPPED:
                    return VideoCaptureOverlayState.Started;

                case Types.VideoCaptureOverlayState.DISMISSED:
                    return VideoCaptureOverlayState.Stopped;

                case (Types.VideoCaptureOverlayState.DISMISSED | Types.VideoCaptureOverlayState.SHOWN):
                    return VideoCaptureOverlayState.Dismissed;
            }
            Debug.LogWarning("Unknown Types.VideoCaptureOverlayState: " + nativeOverlayState + ", defaulting to VideoCaptureOverlayState.Unknown.");
            return VideoCaptureOverlayState.Unknown;
        }

        internal static VideoQualityLevel ConvertNativeVideoQualityLevel(Types.VideoQualityLevel nativeQualityLevel)
        {
            switch ((nativeQualityLevel + 1))
            {
                case Types.VideoQualityLevel.SD:
                    return VideoQualityLevel.Unknown;

                case Types.VideoQualityLevel.HD:
                    return VideoQualityLevel.SD;

                case Types.VideoQualityLevel.XHD:
                    return VideoQualityLevel.HD;

                case Types.VideoQualityLevel.FULLHD:
                    return VideoQualityLevel.XHD;

                case ((Types.VideoQualityLevel) 4):
                    return VideoQualityLevel.FullHD;
            }
            Debug.LogWarning("Unknown Types.VideoQualityLevel: " + nativeQualityLevel + ", defaulting to VideoQualityLevel.Unknown.");
            return VideoQualityLevel.Unknown;
        }

        internal static ResponseStatus ConvertResponseStatus(CommonErrorStatus.ResponseStatus status)
        {
            switch ((status + 5))
            {
                case ~CommonErrorStatus.ResponseStatus.ERROR_LICENSE_CHECK_FAILED:
                    return ResponseStatus.Timeout;

                case CommonErrorStatus.ResponseStatus.VALID:
                    return ResponseStatus.VersionUpdateRequired;

                case CommonErrorStatus.ResponseStatus.VALID_BUT_STALE:
                    return ResponseStatus.NotAuthorized;

                case ~CommonErrorStatus.ResponseStatus.ERROR_VERSION_UPDATE_REQUIRED:
                    return ResponseStatus.InternalError;

                case ~CommonErrorStatus.ResponseStatus.ERROR_TIMEOUT:
                    return ResponseStatus.LicenseCheckFailed;

                case ((CommonErrorStatus.ResponseStatus) 6):
                    return ResponseStatus.Success;

                case ((CommonErrorStatus.ResponseStatus) 7):
                    return ResponseStatus.SuccessWithStale;
            }
            throw new InvalidOperationException("Unknown status: " + status);
        }

        internal static CommonStatusCodes ConvertResponseStatusToCommonStatus(CommonErrorStatus.ResponseStatus status)
        {
            switch ((status + 5))
            {
                case ~CommonErrorStatus.ResponseStatus.ERROR_LICENSE_CHECK_FAILED:
                    return CommonStatusCodes.Timeout;

                case CommonErrorStatus.ResponseStatus.VALID:
                    return CommonStatusCodes.ServiceVersionUpdateRequired;

                case CommonErrorStatus.ResponseStatus.VALID_BUT_STALE:
                    return CommonStatusCodes.AuthApiAccessForbidden;

                case ~CommonErrorStatus.ResponseStatus.ERROR_VERSION_UPDATE_REQUIRED:
                    return CommonStatusCodes.InternalError;

                case ~CommonErrorStatus.ResponseStatus.ERROR_TIMEOUT:
                    return CommonStatusCodes.LicenseCheckFailed;

                case ((CommonErrorStatus.ResponseStatus) 6):
                    return CommonStatusCodes.Success;

                case ((CommonErrorStatus.ResponseStatus) 7):
                    return CommonStatusCodes.SuccessCached;
            }
            Debug.LogWarning("Unknown ResponseStatus: " + status + ", defaulting to CommonStatusCodes.Error");
            return CommonStatusCodes.Error;
        }

        internal static UIStatus ConvertUIStatus(CommonErrorStatus.UIStatus status)
        {
            switch ((status + 6))
            {
                case ~(CommonErrorStatus.UIStatus.VALID | CommonErrorStatus.UIStatus.ERROR_INTERNAL):
                    return UIStatus.UserClosedUI;

                case CommonErrorStatus.UIStatus.VALID:
                    return UIStatus.Timeout;

                case ~CommonErrorStatus.UIStatus.ERROR_NOT_AUTHORIZED:
                    return UIStatus.VersionUpdateRequired;

                case ~CommonErrorStatus.UIStatus.ERROR_VERSION_UPDATE_REQUIRED:
                    return UIStatus.NotAuthorized;

                case ~CommonErrorStatus.UIStatus.ERROR_TIMEOUT:
                    return UIStatus.InternalError;

                case ((CommonErrorStatus.UIStatus) 7):
                    return UIStatus.Valid;
            }
            if (status != CommonErrorStatus.UIStatus.ERROR_UI_BUSY)
            {
                throw new InvalidOperationException("Unknown status: " + status);
            }
            return UIStatus.UiBusy;
        }

        internal static Types.VideoCaptureMode ConvertVideoCaptureMode(VideoCaptureMode captureMode)
        {
            switch ((captureMode + 1))
            {
                case VideoCaptureMode.File:
                    return Types.VideoCaptureMode.UNKNOWN;

                case VideoCaptureMode.Stream:
                    return Types.VideoCaptureMode.FILE;

                case ((VideoCaptureMode) 2):
                    return Types.VideoCaptureMode.STREAM;
            }
            Debug.LogWarning("Unknown VideoCaptureMode: " + captureMode + ", defaulting to Types.VideoCaptureMode.UNKNOWN.");
            return Types.VideoCaptureMode.UNKNOWN;
        }
    }
}

