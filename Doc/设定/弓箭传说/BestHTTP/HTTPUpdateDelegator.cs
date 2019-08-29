namespace BestHTTP
{
    using BestHTTP.Caching;
    using BestHTTP.Cookies;
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using UnityEngine;

    [ExecuteInEditMode]
    public sealed class HTTPUpdateDelegator : MonoBehaviour
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static HTTPUpdateDelegator <Instance>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static bool <IsCreated>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static bool <IsThreaded>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static bool <IsThreadRunning>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static int <ThreadFrequencyInMS>k__BackingField;
        public static Func<bool> OnBeforeApplicationQuit;
        private static bool IsSetupCalled;

        static HTTPUpdateDelegator()
        {
            ThreadFrequencyInMS = 100;
        }

        public static void CheckInstance()
        {
            try
            {
                if (!IsCreated)
                {
                    GameObject obj2 = GameObject.Find("HTTP Update Delegator");
                    if (obj2 != null)
                    {
                        Instance = obj2.GetComponent<HTTPUpdateDelegator>();
                    }
                    if (Instance == null)
                    {
                        Instance = new GameObject("HTTP Update Delegator") { hideFlags = HideFlags.DontSave }.AddComponent<HTTPUpdateDelegator>();
                    }
                    IsCreated = true;
                }
                HTTPManager.Logger.Information("HTTPUpdateDelegator", "Instance Created!");
            }
            catch
            {
                HTTPManager.Logger.Error("HTTPUpdateDelegator", "Please call the BestHTTP.HTTPManager.Setup() from one of Unity's event(eg. awake, start) before you send any request!");
            }
        }

        private void OnApplicationQuit()
        {
            HTTPManager.Logger.Information("HTTPUpdateDelegator", "OnApplicationQuit Called!");
            if (OnBeforeApplicationQuit != null)
            {
                try
                {
                    if (!OnBeforeApplicationQuit())
                    {
                        HTTPManager.Logger.Information("HTTPUpdateDelegator", "OnBeforeApplicationQuit call returned false, postponing plugin shutdown.");
                        return;
                    }
                }
                catch (Exception exception)
                {
                    HTTPManager.Logger.Exception("HTTPUpdateDelegator", string.Empty, exception);
                }
            }
            IsThreadRunning = false;
            if (IsCreated)
            {
                IsCreated = false;
                HTTPManager.OnQuit();
            }
        }

        private void OnDisable()
        {
            HTTPManager.Logger.Information("HTTPUpdateDelegator", "OnDisable Called!");
            this.OnApplicationQuit();
        }

        private void Setup()
        {
            HTTPCacheService.SetupCacheFolder();
            CookieJar.SetupFolder();
            CookieJar.Load();
            if (IsThreaded)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.ThreadFunc));
            }
            IsSetupCalled = true;
            Object.DontDestroyOnLoad(base.gameObject);
            HTTPManager.Logger.Information("HTTPUpdateDelegator", "Setup done!");
        }

        private void ThreadFunc(object obj)
        {
            HTTPManager.Logger.Information("HTTPUpdateDelegator", "Update Thread Started");
            try
            {
                IsThreadRunning = true;
                while (IsThreadRunning)
                {
                    HTTPManager.OnUpdate();
                    Thread.Sleep(ThreadFrequencyInMS);
                }
            }
            finally
            {
                HTTPManager.Logger.Information("HTTPUpdateDelegator", "Update Thread Ended");
            }
        }

        private void Update()
        {
            if (!IsSetupCalled)
            {
                IsSetupCalled = true;
                this.Setup();
            }
            if (!IsThreaded)
            {
                HTTPManager.OnUpdate();
            }
        }

        public static HTTPUpdateDelegator Instance
        {
            [CompilerGenerated]
            get => 
                <Instance>k__BackingField;
            [CompilerGenerated]
            private set => 
                (<Instance>k__BackingField = value);
        }

        public static bool IsCreated
        {
            [CompilerGenerated]
            get => 
                <IsCreated>k__BackingField;
            [CompilerGenerated]
            private set => 
                (<IsCreated>k__BackingField = value);
        }

        public static bool IsThreaded
        {
            [CompilerGenerated]
            get => 
                <IsThreaded>k__BackingField;
            [CompilerGenerated]
            set => 
                (<IsThreaded>k__BackingField = value);
        }

        public static bool IsThreadRunning
        {
            [CompilerGenerated]
            get => 
                <IsThreadRunning>k__BackingField;
            [CompilerGenerated]
            private set => 
                (<IsThreadRunning>k__BackingField = value);
        }

        public static int ThreadFrequencyInMS
        {
            [CompilerGenerated]
            get => 
                <ThreadFrequencyInMS>k__BackingField;
            [CompilerGenerated]
            set => 
                (<ThreadFrequencyInMS>k__BackingField = value);
        }
    }
}

