namespace Dxx
{
    using System;

    public class Singleton<T> : Singletonable where T: Singletonable, new()
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (Singleton<T>._instance == null)
                {
                    Singleton<T>._instance = Activator.CreateInstance<T>();
                }
                Singleton<T>._instance.OnInstanceCreate();
                return Singleton<T>._instance;
            }
        }
    }
}

