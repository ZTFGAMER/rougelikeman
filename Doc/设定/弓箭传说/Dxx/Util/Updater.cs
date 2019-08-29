namespace Dxx.Util
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;
    using UnityEngine;

    public class Updater : MonoBehaviour
    {
        private static bool bCreate;
        private static float _AliveTime;
        private static float _deltatime;
        private static float _unscaleAliveTime;
        private static float _unscaledeltatime;
        public int count;
        private static Updater updater;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event Action onFixedUpdate;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event Action onLateUpdate;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event Action<float> onUpdate;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event Action<float> onUpdateIgnoreTime;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event Action<float> onUpdateUI;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event Action<float> onUpdateUIIgnoreTime;

        public static void AddFixedUpdate(Action func)
        {
            GetUpdater().onFixedUpdate += func;
        }

        public static void AddLateUpdate(Action func)
        {
            GetUpdater().onLateUpdate += func;
        }

        public static void AddUpdate(string name, Action<float> func, bool IgnoreTimeScale = false)
        {
            Updater updater = GetUpdater();
            if (!IgnoreTimeScale)
            {
                updater.onUpdate += func;
            }
            else
            {
                updater.onUpdateIgnoreTime += func;
            }
        }

        public static void AddUpdateUI(Action<float> func, bool IgnoreTimeScale = false)
        {
            if (!IgnoreTimeScale)
            {
                GetUpdater().onUpdateUI += func;
            }
            else
            {
                GetUpdater().onUpdateUIIgnoreTime += func;
            }
        }

        private void FixedUpdate()
        {
            if ((this.onFixedUpdate != null) && !GameLogic.Paused)
            {
                this.onFixedUpdate();
            }
        }

        public static Updater Get(GameObject go)
        {
            Updater component = go.GetComponent<Updater>();
            if (component != null)
            {
                return component;
            }
            return go.AddComponent<Updater>();
        }

        public static Updater GetUpdater()
        {
            if (updater == null)
            {
                GameObject target = new GameObject("updater");
                Object.DontDestroyOnLoad(target);
                updater = Get(target);
                bCreate = true;
            }
            return updater;
        }

        public void Init()
        {
        }

        private void LateUpdate()
        {
            if ((this.onLateUpdate != null) && !GameLogic.Paused)
            {
                this.onLateUpdate();
            }
        }

        public void OnRelease()
        {
            this.count = 0;
        }

        public static void RemoveFixedUpdate(Action func)
        {
            if (bCreate)
            {
                GetUpdater().onFixedUpdate -= func;
            }
        }

        public static void RemoveLateUpdate(Action func)
        {
            if (bCreate)
            {
                GetUpdater().onLateUpdate -= func;
            }
        }

        public static void RemoveUpdate(string name, Action<float> func)
        {
            if (bCreate)
            {
                Updater updater = GetUpdater();
                updater.onUpdate -= func;
                updater.onUpdateIgnoreTime -= func;
            }
        }

        public static void RemoveUpdateUI(Action<float> func)
        {
            if (bCreate)
            {
                GetUpdater().onUpdateUI -= func;
                GetUpdater().onUpdateUIIgnoreTime -= func;
            }
        }

        private void Update()
        {
            _deltatime = Time.deltaTime;
            _AliveTime += _deltatime;
            if (!GameLogic.Paused)
            {
                _unscaledeltatime = Time.unscaledDeltaTime;
                _unscaleAliveTime += _unscaledeltatime;
            }
            if (this.onUpdateIgnoreTime != null)
            {
                this.onUpdateIgnoreTime(deltaIgnoreTime);
            }
            if ((this.onUpdate != null) && !GameLogic.Paused)
            {
                this.onUpdate(delta);
            }
            if ((this.onUpdateUI != null) && !GameLogic.Paused)
            {
                this.onUpdateUI(delta);
            }
        }

        public static void UpdaterDeinit()
        {
            if (updater != null)
            {
                Object.Destroy(updater.gameObject);
                updater = null;
                bCreate = false;
            }
        }

        public static float AliveTime =>
            Time.time;

        public static float delta =>
            Time.deltaTime;

        public static float deltaIgnoreTime =>
            Time.unscaledDeltaTime;

        public static float unscaleAliveTime =>
            Time.unscaledTime;
    }
}

