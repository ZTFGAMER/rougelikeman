using System;

public class CInstance<T> where T: new()
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (CInstance<T>._instance == null)
            {
                CInstance<T>._instance = Activator.CreateInstance<T>();
            }
            return CInstance<T>._instance;
        }
    }
}

