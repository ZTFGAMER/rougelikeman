namespace GooglePlayGames.OurUtils
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class PlayGamesHelperObject : MonoBehaviour
    {
        private static PlayGamesHelperObject instance = null;
        private static bool sIsDummy = false;
        private static List<Action> sQueue = new List<Action>();
        private List<Action> localQueue = new List<Action>();
        private static volatile bool sQueueEmpty = true;
        private static List<Action<bool>> sPauseCallbackList = new List<Action<bool>>();
        private static List<Action<bool>> sFocusCallbackList = new List<Action<bool>>();

        public static void AddFocusCallback(Action<bool> callback)
        {
            if (!sFocusCallbackList.Contains(callback))
            {
                sFocusCallbackList.Add(callback);
            }
        }

        public static void AddPauseCallback(Action<bool> callback)
        {
            if (!sPauseCallbackList.Contains(callback))
            {
                sPauseCallbackList.Add(callback);
            }
        }

        public void Awake()
        {
            UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
        }

        public static void CreateObject()
        {
            if (instance == null)
            {
                if (Application.isPlaying)
                {
                    GameObject target = new GameObject("PlayGames_QueueRunner");
                    UnityEngine.Object.DontDestroyOnLoad(target);
                    instance = target.AddComponent<PlayGamesHelperObject>();
                }
                else
                {
                    instance = new PlayGamesHelperObject();
                    sIsDummy = true;
                }
            }
        }

        public void OnApplicationFocus(bool focused)
        {
            foreach (Action<bool> action in sFocusCallbackList)
            {
                try
                {
                    action(focused);
                }
                catch (Exception exception)
                {
                    Debug.LogError("Exception in OnApplicationFocus:" + exception.Message + "\n" + exception.StackTrace);
                }
            }
        }

        public void OnApplicationPause(bool paused)
        {
            foreach (Action<bool> action in sPauseCallbackList)
            {
                try
                {
                    action(paused);
                }
                catch (Exception exception)
                {
                    Debug.LogError("Exception in OnApplicationPause:" + exception.Message + "\n" + exception.StackTrace);
                }
            }
        }

        public void OnDisable()
        {
            if (instance == this)
            {
                instance = null;
            }
        }

        public static bool RemoveFocusCallback(Action<bool> callback) => 
            sFocusCallbackList.Remove(callback);

        public static bool RemovePauseCallback(Action<bool> callback) => 
            sPauseCallbackList.Remove(callback);

        public static void RunCoroutine(IEnumerator action)
        {
            <RunCoroutine>c__AnonStorey0 storey = new <RunCoroutine>c__AnonStorey0 {
                action = action
            };
            if (instance != null)
            {
                RunOnGameThread(new Action(storey.<>m__0));
            }
        }

        public static void RunOnGameThread(Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }
            if (!sIsDummy)
            {
                object sQueue = PlayGamesHelperObject.sQueue;
                lock (sQueue)
                {
                    PlayGamesHelperObject.sQueue.Add(action);
                    sQueueEmpty = false;
                }
            }
        }

        public void Update()
        {
            if (!sIsDummy && !sQueueEmpty)
            {
                this.localQueue.Clear();
                object sQueue = PlayGamesHelperObject.sQueue;
                lock (sQueue)
                {
                    this.localQueue.AddRange(PlayGamesHelperObject.sQueue);
                    PlayGamesHelperObject.sQueue.Clear();
                    sQueueEmpty = true;
                }
                for (int i = 0; i < this.localQueue.Count; i++)
                {
                    this.localQueue[i]();
                }
            }
        }

        [CompilerGenerated]
        private sealed class <RunCoroutine>c__AnonStorey0
        {
            internal IEnumerator action;

            internal void <>m__0()
            {
                PlayGamesHelperObject.instance.StartCoroutine(this.action);
            }
        }
    }
}

