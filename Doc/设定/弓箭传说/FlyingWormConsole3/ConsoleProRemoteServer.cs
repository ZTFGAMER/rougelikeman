namespace FlyingWormConsole3
{
    using System;
    using UnityEngine;

    public class ConsoleProRemoteServer : MonoBehaviour
    {
        public bool useNATPunch;
        public int port = 0xc738;

        public void Awake()
        {
            Debug.Log("Console Pro Remote Server is disabled in release mode, please use a Development build or define DEBUG to use it");
        }
    }
}

