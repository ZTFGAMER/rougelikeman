namespace Dxx.Util
{
    using Dxx.Collections;
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;
    using UnityEngine;

    public abstract class TimerBase<T> : MonoBehaviour where T: TimerBase<T>
    {
        private static T s_instance;
        private long mIdGen;
        private KeyedPriorityQueue<long, TimerData<T>, double> mQueue;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event Action onTimerChanged;

        protected TimerBase()
        {
            this.mQueue = new KeyedPriorityQueue<long, TimerData<T>, double>();
        }

        public float GetCurrentTime() => 
            (!this.InGameUpdate ? Time.time : Updater.AliveTime);

        private static T GetInstance()
        {
            if ((TimerBase<T>.s_instance == null) && Application.isPlaying)
            {
                GameObject obj2 = new GameObject(typeof(T).Name);
                TimerBase<T>.s_instance = obj2.AddComponent<T>();
                obj2.transform.parent = !TimerBase<T>.s_instance.InGameUpdate ? null : GameNode.m_Battle.transform;
            }
            return TimerBase<T>.s_instance;
        }

        protected int GetTimers() => 
            this.mQueue.Count;

        protected abstract void OnChanged();
        public void OnRemove()
        {
            Object.Destroy(TimerBase<T>.s_instance.gameObject);
            TimerBase<T>.s_instance = null;
        }

        protected abstract void OnUpdate();
        public static ulong Register(float timeout, Action onTimer) => 
            TimerBase<T>.GetInstance()?.RegisterInternal(null, timeout, timeout, null, onTimer, null, 1);

        public static ulong Register(float timeout, Action<float> onTimer) => 
            TimerBase<T>.GetInstance()?.RegisterInternal(null, timeout, timeout, onTimer, null, null, 1);

        public static ulong Register(float timeout, Action onTimer, Action onCanceled) => 
            TimerBase<T>.GetInstance()?.RegisterInternal(null, timeout, timeout, null, onTimer, onCanceled, 1);

        public static ulong Register(float timeout, Action onTimer, int times) => 
            TimerBase<T>.GetInstance()?.RegisterInternal(null, timeout, timeout, null, onTimer, null, times);

        public static ulong Register(float timeout, Action<float> onTimer, Action onCanceled) => 
            TimerBase<T>.GetInstance()?.RegisterInternal(null, timeout, timeout, onTimer, null, onCanceled, 1);

        public static ulong Register(float timeout, Action<float> onTimer, int times) => 
            TimerBase<T>.GetInstance()?.RegisterInternal(null, timeout, timeout, onTimer, null, null, times);

        public static ulong Register(float delay, float interval, Action onTimer) => 
            TimerBase<T>.GetInstance()?.RegisterInternal(null, delay, interval, null, onTimer, null, 1);

        public static ulong Register(float delay, float interval, Action<float> onTimer) => 
            TimerBase<T>.GetInstance()?.RegisterInternal(null, delay, interval, onTimer, null, null, 1);

        public static void Register(string key, float timeout, Action onTimer)
        {
            if (key == null)
            {
                throw new ArgumentNullException(key);
            }
            T instance = TimerBase<T>.GetInstance();
            if (instance != null)
            {
                instance.RegisterInternal(key, timeout, timeout, null, onTimer, null, 1);
            }
        }

        public static void Register(string key, float timeout, Action<float> onTimer)
        {
            if (key == null)
            {
                throw new ArgumentNullException(key);
            }
            T instance = TimerBase<T>.GetInstance();
            if (instance != null)
            {
                instance.RegisterInternal(key, timeout, timeout, onTimer, null, null, 1);
            }
        }

        public static ulong Register(float timeout, Action onTimer, Action onCanceled, int times) => 
            TimerBase<T>.GetInstance()?.RegisterInternal(null, timeout, timeout, null, onTimer, onCanceled, times);

        public static ulong Register(float timeout, Action<float> onTimer, Action onCanceled, int times) => 
            TimerBase<T>.GetInstance()?.RegisterInternal(null, timeout, timeout, onTimer, null, onCanceled, times);

        public static ulong Register(float delay, float interval, Action onTimer, Action onCanceled) => 
            TimerBase<T>.GetInstance()?.RegisterInternal(null, delay, interval, null, onTimer, onCanceled, 1);

        public static ulong Register(float delay, float interval, Action onTimer, int times) => 
            TimerBase<T>.GetInstance()?.RegisterInternal(null, delay, interval, null, onTimer, null, times);

        public static ulong Register(float delay, float interval, Action<float> onTimer, Action onCanceled) => 
            TimerBase<T>.GetInstance()?.RegisterInternal(null, delay, interval, onTimer, null, onCanceled, 1);

        public static ulong Register(float delay, float interval, Action<float> onTimer, int times) => 
            TimerBase<T>.GetInstance()?.RegisterInternal(null, delay, interval, onTimer, null, null, times);

        public static void Register(string key, float timeout, Action onTimer, Action onCanceled)
        {
            if (key == null)
            {
                throw new ArgumentNullException(key);
            }
            T instance = TimerBase<T>.GetInstance();
            if (instance != null)
            {
                instance.RegisterInternal(key, timeout, timeout, null, onTimer, onCanceled, 1);
            }
        }

        public static void Register(string key, float timeout, Action onTimer, int times)
        {
            if (key == null)
            {
                throw new ArgumentNullException(key);
            }
            T instance = TimerBase<T>.GetInstance();
            if (instance != null)
            {
                instance.RegisterInternal(key, timeout, timeout, null, onTimer, null, times);
            }
        }

        public static void Register(string key, float timeout, Action<float> onTimer, Action onCanceled)
        {
            if (key == null)
            {
                throw new ArgumentNullException(key);
            }
            T instance = TimerBase<T>.GetInstance();
            if (instance != null)
            {
                instance.RegisterInternal(key, timeout, timeout, onTimer, null, onCanceled, 1);
            }
        }

        public static void Register(string key, float timeout, Action<float> onTimer, int times)
        {
            if (key == null)
            {
                throw new ArgumentNullException(key);
            }
            T instance = TimerBase<T>.GetInstance();
            if (instance != null)
            {
                instance.RegisterInternal(key, timeout, timeout, onTimer, null, null, times);
            }
        }

        public static void Register(string key, float delay, float interval, Action onTimer)
        {
            if (key == null)
            {
                throw new ArgumentNullException(key);
            }
            T instance = TimerBase<T>.GetInstance();
            if (instance != null)
            {
                instance.RegisterInternal(key, delay, interval, null, onTimer, null, 1);
            }
        }

        public static void Register(string key, float delay, float interval, Action<float> onTimer)
        {
            if (key == null)
            {
                throw new ArgumentNullException(key);
            }
            T instance = TimerBase<T>.GetInstance();
            if (instance != null)
            {
                instance.RegisterInternal(key, delay, interval, onTimer, null, null, 1);
            }
        }

        public static ulong Register(float delay, float interval, Action onTimer, Action onCanceled, int times) => 
            TimerBase<T>.GetInstance()?.RegisterInternal(null, delay, interval, null, onTimer, onCanceled, times);

        public static ulong Register(float delay, float interval, Action<float> onTimer, Action onCanceled, int times) => 
            TimerBase<T>.GetInstance()?.RegisterInternal(null, delay, interval, onTimer, null, onCanceled, times);

        public static void Register(string key, float timeout, Action onTimer, Action onCanceled, int times)
        {
            if (key == null)
            {
                throw new ArgumentNullException(key);
            }
            T instance = TimerBase<T>.GetInstance();
            if (instance != null)
            {
                instance.RegisterInternal(key, timeout, timeout, null, onTimer, onCanceled, times);
            }
        }

        public static void Register(string key, float timeout, Action<float> onTimer, Action onCanceled, int times)
        {
            if (key == null)
            {
                throw new ArgumentNullException(key);
            }
            T instance = TimerBase<T>.GetInstance();
            if (instance != null)
            {
                instance.RegisterInternal(key, timeout, timeout, onTimer, null, onCanceled, times);
            }
        }

        public static void Register(string key, float delay, float interval, Action onTimer, Action onCanceled)
        {
            if (key == null)
            {
                throw new ArgumentNullException(key);
            }
            T instance = TimerBase<T>.GetInstance();
            if (instance != null)
            {
                instance.RegisterInternal(key, delay, interval, null, onTimer, onCanceled, 1);
            }
        }

        public static void Register(string key, float delay, float interval, Action onTimer, int times)
        {
            if (key == null)
            {
                throw new ArgumentNullException(key);
            }
            T instance = TimerBase<T>.GetInstance();
            if (instance != null)
            {
                instance.RegisterInternal(key, delay, interval, null, onTimer, null, times);
            }
        }

        public static void Register(string key, float delay, float interval, Action<float> onTimer, Action onCanceled)
        {
            if (key == null)
            {
                throw new ArgumentNullException(key);
            }
            T instance = TimerBase<T>.GetInstance();
            if (instance != null)
            {
                instance.RegisterInternal(key, delay, interval, onTimer, null, onCanceled, 1);
            }
        }

        public static void Register(string key, float delay, float interval, Action<float> onTimer, int times)
        {
            if (key == null)
            {
                throw new ArgumentNullException(key);
            }
            T instance = TimerBase<T>.GetInstance();
            if (instance != null)
            {
                instance.RegisterInternal(key, delay, interval, onTimer, null, null, times);
            }
        }

        public static void Register(string key, float delay, float interval, Action onTimer, Action onCanceled, int times)
        {
            if (key == null)
            {
                throw new ArgumentNullException(key);
            }
            T instance = TimerBase<T>.GetInstance();
            if (instance != null)
            {
                instance.RegisterInternal(key, delay, interval, null, onTimer, onCanceled, times);
            }
        }

        public static void Register(string key, float delay, float interval, Action<float> onTimer, Action onCanceled, int times)
        {
            if (key == null)
            {
                throw new ArgumentNullException(key);
            }
            T instance = TimerBase<T>.GetInstance();
            if (instance != null)
            {
                instance.RegisterInternal(key, delay, interval, onTimer, null, onCanceled, times);
            }
        }

        private ulong RegisterInternal(string key, float delay, float interval, Action<float> onTimer1, Action onTimer2, Action onCanceled, int times)
        {
            long num = 0L;
            if (key == null)
            {
                num = this.mIdGen += 1L;
            }
            else
            {
                num += key.GetHashCode();
                num -= 0x7fffffffL;
                if (this.mQueue.TryGetItem(num, out TimerData<T> data))
                {
                    this.mQueue.RemoveFromQueue(num);
                    if (data.onCanceled != null)
                    {
                        try
                        {
                            data.onCanceled();
                        }
                        catch (Exception exception)
                        {
                            Debug.LogException(exception);
                        }
                    }
                }
            }
            TimerData<T> data2 = new TimerData<T> {
                id = num,
                key = key,
                interval = interval,
                onTimer1 = onTimer1,
                onTimer2 = onTimer2,
                onCanceled = onCanceled,
                times = times
            };
            this.mQueue.Enqueue(num, data2, (double) (this.GetCurrentTime() + delay));
            if (!base.enabled)
            {
                base.enabled = true;
            }
            this.OnChanged();
            if (this.onTimerChanged != null)
            {
                this.onTimerChanged();
            }
            return ((num > 0L) ? ((ulong) num) : ((ulong) 0L));
        }

        public static void Unregister()
        {
            T instance = TimerBase<T>.GetInstance();
            if (instance != null)
            {
                instance.OnRemove();
            }
        }

        public static bool Unregister(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(key);
            }
            return TimerBase<T>.GetInstance()?.UnregisterInternal(key);
        }

        public static bool Unregister(ulong id) => 
            TimerBase<T>.GetInstance()?.UnregisterInternal((long) id);

        private bool UnregisterInternal(long id)
        {
            bool flag = this.mQueue.RemoveFromQueue(id);
            if ((flag && (this.mQueue.Count <= 0)) && base.enabled)
            {
                base.enabled = false;
            }
            if (flag)
            {
                this.OnChanged();
                if (this.onTimerChanged != null)
                {
                    this.onTimerChanged();
                }
            }
            return flag;
        }

        private bool UnregisterInternal(string key)
        {
            long hashCode = key.GetHashCode();
            hashCode -= 0x7fffffffL;
            return this.UnregisterInternal(hashCode);
        }

        protected void Update()
        {
            if (!this.InGameUpdate || GameLogic.InGame)
            {
                this.OnUpdate();
                double currentTime = this.GetCurrentTime();
                while (this.mQueue.Count > 0)
                {
                    this.mQueue.Peek(out long num2, out double num3);
                    if (num3 > currentTime)
                    {
                        break;
                    }
                    TimerData<T> data = this.mQueue.Dequeue(out num2, out num3);
                    if (data.times != 1)
                    {
                        if (data.times > 0)
                        {
                            data.times--;
                        }
                        this.mQueue.Enqueue(num2, data, num3 + data.interval);
                    }
                    if (data.onTimer1 != null)
                    {
                        try
                        {
                            data.onTimer1((float) (currentTime - num3));
                        }
                        catch (Exception exception)
                        {
                            Debug.LogException(exception);
                        }
                    }
                    if (data.onTimer2 != null)
                    {
                        try
                        {
                            data.onTimer2();
                        }
                        catch (Exception exception2)
                        {
                            Debug.LogException(exception2);
                        }
                    }
                    if ((this.mQueue.Count <= 0) && base.enabled)
                    {
                        base.enabled = false;
                    }
                    this.OnChanged();
                    if (this.onTimerChanged != null)
                    {
                        this.onTimerChanged();
                    }
                }
            }
        }

        protected virtual bool InGameUpdate =>
            true;

        [StructLayout(LayoutKind.Sequential)]
        private struct TimerData
        {
            public long id;
            public string key;
            public float interval;
            public Action<float> onTimer1;
            public Action onTimer2;
            public Action onCanceled;
            public int times;
        }
    }
}

