using System;
using System.Reflection;
using UnityEngine;

public class UniWebViewAndroidStaticListener : MonoBehaviour
{
    private void Awake()
    {
        Object.DontDestroyOnLoad(base.gameObject);
    }

    private void OnJavaMessage(string message)
    {
        char[] separator = new char[] { "@"[0] };
        string[] strArray = message.Split(separator);
        if (strArray.Length < 3)
        {
            Debug.Log("Not enough parts for receiving a message.");
        }
        else
        {
            UniWebViewNativeListener listener = UniWebViewNativeListener.GetListener(strArray[0]);
            if (listener == null)
            {
                Debug.Log("Unable to find listener");
            }
            else
            {
                MethodInfo method = typeof(UniWebViewNativeListener).GetMethod(strArray[1]);
                if (method == null)
                {
                    Debug.Log("Cannot find correct method to invoke: " + strArray[1]);
                }
                int num = strArray.Length - 2;
                string[] strArray2 = new string[num];
                for (int i = 0; i < num; i++)
                {
                    strArray2[i] = strArray[i + 2];
                }
                object[] parameters = new object[] { string.Join("@", strArray2) };
                method.Invoke(listener, parameters);
            }
        }
    }
}

