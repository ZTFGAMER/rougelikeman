namespace AudienceNetwork
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class AdHandler : MonoBehaviour
    {
        private static readonly Queue<Action> executeOnMainThreadQueue = new Queue<Action>();

        public void executeOnMainThread(Action action)
        {
            executeOnMainThreadQueue.Enqueue(action);
        }

        public void removeFromParent()
        {
            UnityEngine.Object.Destroy(this);
        }

        private void Update()
        {
            while (executeOnMainThreadQueue.Count > 0)
            {
                executeOnMainThreadQueue.Dequeue()();
            }
        }
    }
}

