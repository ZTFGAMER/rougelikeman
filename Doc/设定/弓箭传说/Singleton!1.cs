using System;
using System.Text;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T: MonoBehaviour
{
    private static T _instance;
    private static object _lock;
    private static StringBuilder strTemp;
    private static bool applicationIsQuitting;

    static Singleton()
    {
        Singleton<T>._lock = new object();
        Singleton<T>.strTemp = new StringBuilder();
        Singleton<T>.applicationIsQuitting = false;
    }

    public void OnDestroy()
    {
        Singleton<T>._instance = null;
        Singleton<T>.applicationIsQuitting = true;
    }

    public static T Instance
    {
        get
        {
            if (Singleton<T>.applicationIsQuitting)
            {
                return null;
            }
            object obj2 = Singleton<T>._lock;
            lock (obj2)
            {
                if (Singleton<T>._instance == null)
                {
                    Singleton<T>._instance = (T) Object.FindObjectOfType(typeof(T));
                    if (Object.FindObjectsOfType(typeof(T)).Length > 1)
                    {
                        return Singleton<T>._instance;
                    }
                    if (Singleton<T>._instance == null)
                    {
                        GameObject target = new GameObject();
                        Singleton<T>._instance = target.AddComponent<T>();
                        Singleton<T>.strTemp.Clear();
                        Singleton<T>.strTemp.AppendFormat("(singleton) {0}", typeof(T).ToString());
                        target.name = Singleton<T>.strTemp.ToString();
                        Object.DontDestroyOnLoad(target);
                    }
                }
                return Singleton<T>._instance;
            }
        }
    }
}

