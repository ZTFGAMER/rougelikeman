namespace Dxx
{
    using UnityEngine;

    public class SingletonMono<T> : SingletonableMono where T: SingletonableMono
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (SingletonMono<T>._instance == null)
                {
                    GameObject target = new GameObject(typeof(T).Name);
                    SingletonMono<T>._instance = target.AddComponent<T>();
                    SingletonMono<T>._instance.OnInstanceCreate();
                    Object.DontDestroyOnLoad(target);
                }
                return SingletonMono<T>._instance;
            }
        }
    }
}

