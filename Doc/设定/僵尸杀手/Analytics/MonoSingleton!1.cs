namespace Analytics
{
    using System;
    using UnityEngine;

    public class MonoSingleton<T> : MonoBehaviour where T: MonoBehaviour
    {
        private static T s_Instance;
        private static bool s_IsDestroyed;

        protected virtual void OnDestroy()
        {
            if (MonoSingleton<T>.s_Instance != null)
            {
                UnityEngine.Object.Destroy(MonoSingleton<T>.s_Instance);
            }
            MonoSingleton<T>.s_Instance = null;
            MonoSingleton<T>.s_IsDestroyed = true;
        }

        public static T Instance
        {
            get
            {
                if (MonoSingleton<T>.s_IsDestroyed)
                {
                    return null;
                }
                if (MonoSingleton<T>.s_Instance == null)
                {
                    MonoSingleton<T>.s_Instance = UnityEngine.Object.FindObjectOfType(typeof(T)) as T;
                    if (MonoSingleton<T>.s_Instance == null)
                    {
                        GameObject target = new GameObject(typeof(T).Name);
                        UnityEngine.Object.DontDestroyOnLoad(target);
                        MonoSingleton<T>.s_Instance = target.AddComponent(typeof(T)) as T;
                    }
                }
                return MonoSingleton<T>.s_Instance;
            }
        }
    }
}

