namespace GooglePlayGames.Native.PInvoke
{
    using AOT;
    using GooglePlayGames.Native.Cwrapper;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    internal class NearbyConnectionsManager : BaseReferenceHolder
    {
        private static readonly string sServiceId = ReadServiceId();
        [CompilerGenerated]
        private static Func<IntPtr, NativeStartAdvertisingResult> <>f__mg$cache0;
        [CompilerGenerated]
        private static Func<IntPtr, NativeConnectionRequest> <>f__mg$cache1;
        [CompilerGenerated]
        private static NearbyConnectionTypes.StartAdvertisingCallback <>f__mg$cache2;
        [CompilerGenerated]
        private static NearbyConnectionTypes.ConnectionRequestCallback <>f__mg$cache3;
        [CompilerGenerated]
        private static Func<NativeAppIdentifier, IntPtr> <>f__am$cache0;
        [CompilerGenerated]
        private static Func<IntPtr, NativeConnectionResponse> <>f__mg$cache4;
        [CompilerGenerated]
        private static NearbyConnectionTypes.ConnectionResponseCallback <>f__mg$cache5;

        internal NearbyConnectionsManager(IntPtr selfPointer) : base(selfPointer)
        {
        }

        internal void AcceptConnectionRequest(string remoteEndpointId, byte[] payload, NativeMessageListenerHelper listener)
        {
            NearbyConnections.NearbyConnections_AcceptConnectionRequest(base.SelfPtr(), remoteEndpointId, payload, new UIntPtr((ulong) payload.Length), listener.AsPointer());
        }

        protected override void CallDispose(HandleRef selfPointer)
        {
            NearbyConnections.NearbyConnections_Dispose(selfPointer);
        }

        internal void DisconnectFromEndpoint(string remoteEndpointId)
        {
            NearbyConnections.NearbyConnections_Disconnect(base.SelfPtr(), remoteEndpointId);
        }

        [MonoPInvokeCallback(typeof(NearbyConnectionTypes.ConnectionRequestCallback))]
        private static void InternalConnectionRequestCallback(long id, IntPtr result, IntPtr userData)
        {
            Callbacks.PerformInternalCallback<long>("NearbyConnectionsManager#InternalConnectionRequestCallback", Callbacks.Type.Permanent, id, result, userData);
        }

        [MonoPInvokeCallback(typeof(NearbyConnectionTypes.ConnectionResponseCallback))]
        private static void InternalConnectResponseCallback(long localClientId, IntPtr response, IntPtr userData)
        {
            Callbacks.PerformInternalCallback<long>("NearbyConnectionManager#InternalConnectResponseCallback", Callbacks.Type.Temporary, localClientId, response, userData);
        }

        [MonoPInvokeCallback(typeof(NearbyConnectionTypes.StartAdvertisingCallback))]
        private static void InternalStartAdvertisingCallback(long id, IntPtr result, IntPtr userData)
        {
            Callbacks.PerformInternalCallback<long>("NearbyConnectionsManager#InternalStartAdvertisingCallback", Callbacks.Type.Permanent, id, result, userData);
        }

        internal static string ReadServiceId()
        {
            string str3;
            Debug.Log("Initializing ServiceId property!!!!");
            using (AndroidJavaClass class2 = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject obj2 = class2.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    string str = obj2.Call<string>("getPackageName", Array.Empty<object>());
                    object[] args = new object[] { str, 0x80 };
                    object[] objArray2 = new object[] { "com.google.android.gms.nearby.connection.SERVICE_ID" };
                    string str2 = obj2.Call<AndroidJavaObject>("getPackageManager", Array.Empty<object>()).Call<AndroidJavaObject>("getApplicationInfo", args).Get<AndroidJavaObject>("metaData").Call<string>("getString", objArray2);
                    Debug.Log("SystemId from Manifest: " + str2);
                    str3 = str2;
                }
            }
            return str3;
        }

        internal void RejectConnectionRequest(string remoteEndpointId)
        {
            NearbyConnections.NearbyConnections_RejectConnectionRequest(base.SelfPtr(), remoteEndpointId);
        }

        internal void SendConnectionRequest(string name, string remoteEndpointId, byte[] payload, Action<long, NativeConnectionResponse> callback, NativeMessageListenerHelper listener)
        {
            if (<>f__mg$cache5 == null)
            {
                <>f__mg$cache5 = new NearbyConnectionTypes.ConnectionResponseCallback(NearbyConnectionsManager.InternalConnectResponseCallback);
            }
            if (<>f__mg$cache4 == null)
            {
                <>f__mg$cache4 = new Func<IntPtr, NativeConnectionResponse>(NativeConnectionResponse.FromPointer);
            }
            NearbyConnections.NearbyConnections_SendConnectionRequest(base.SelfPtr(), name, remoteEndpointId, payload, new UIntPtr((ulong) payload.Length), <>f__mg$cache5, Callbacks.ToIntPtr<long, NativeConnectionResponse>(callback, <>f__mg$cache4), listener.AsPointer());
        }

        internal void SendReliable(string remoteEndpointId, byte[] payload)
        {
            NearbyConnections.NearbyConnections_SendReliableMessage(base.SelfPtr(), remoteEndpointId, payload, new UIntPtr((ulong) payload.Length));
        }

        internal void SendUnreliable(string remoteEndpointId, byte[] payload)
        {
            NearbyConnections.NearbyConnections_SendUnreliableMessage(base.SelfPtr(), remoteEndpointId, payload, new UIntPtr((ulong) payload.Length));
        }

        internal void Shutdown()
        {
            NearbyConnections.NearbyConnections_Stop(base.SelfPtr());
        }

        internal void StartAdvertising(string name, List<NativeAppIdentifier> appIds, long advertisingDuration, Action<long, NativeStartAdvertisingResult> advertisingCallback, Action<long, NativeConnectionRequest> connectionRequestCallback)
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = id => id.AsPointer();
            }
            if (<>f__mg$cache2 == null)
            {
                <>f__mg$cache2 = new NearbyConnectionTypes.StartAdvertisingCallback(NearbyConnectionsManager.InternalStartAdvertisingCallback);
            }
            if (<>f__mg$cache0 == null)
            {
                <>f__mg$cache0 = new Func<IntPtr, NativeStartAdvertisingResult>(NativeStartAdvertisingResult.FromPointer);
            }
            if (<>f__mg$cache3 == null)
            {
                <>f__mg$cache3 = new NearbyConnectionTypes.ConnectionRequestCallback(NearbyConnectionsManager.InternalConnectionRequestCallback);
            }
            if (<>f__mg$cache1 == null)
            {
                <>f__mg$cache1 = new Func<IntPtr, NativeConnectionRequest>(NativeConnectionRequest.FromPointer);
            }
            NearbyConnections.NearbyConnections_StartAdvertising(base.SelfPtr(), name, appIds.Select<NativeAppIdentifier, IntPtr>(<>f__am$cache0).ToArray<IntPtr>(), new UIntPtr((ulong) appIds.Count), advertisingDuration, <>f__mg$cache2, Callbacks.ToIntPtr<long, NativeStartAdvertisingResult>(advertisingCallback, <>f__mg$cache0), <>f__mg$cache3, Callbacks.ToIntPtr<long, NativeConnectionRequest>(connectionRequestCallback, <>f__mg$cache1));
        }

        internal void StartDiscovery(string serviceId, long duration, NativeEndpointDiscoveryListenerHelper listener)
        {
            NearbyConnections.NearbyConnections_StartDiscovery(base.SelfPtr(), serviceId, duration, listener.AsPointer());
        }

        internal void StopAdvertising()
        {
            NearbyConnections.NearbyConnections_StopAdvertising(base.SelfPtr());
        }

        internal void StopAllConnections()
        {
            NearbyConnections.NearbyConnections_Stop(base.SelfPtr());
        }

        internal void StopDiscovery(string serviceId)
        {
            NearbyConnections.NearbyConnections_StopDiscovery(base.SelfPtr(), serviceId);
        }

        public string AppBundleId
        {
            get
            {
                using (AndroidJavaClass class2 = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                {
                    return class2.GetStatic<AndroidJavaObject>("currentActivity").Call<string>("getPackageName", Array.Empty<object>());
                }
            }
        }

        public static string ServiceId =>
            sServiceId;
    }
}

