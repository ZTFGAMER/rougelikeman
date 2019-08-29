using BestHTTP.Examples;
using BestHTTP.SocketIO;
using BestHTTP.SocketIO.Events;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public sealed class SocketIOWePlaySample : MonoBehaviour
{
    private string[] controls = new string[] { "left", "right", "a", "b", "up", "down", "select", "start" };
    private const float ratio = 1.5f;
    private int MaxMessages = 50;
    private States State;
    private BestHTTP.SocketIO.Socket Socket;
    private string Nick = string.Empty;
    private string messageToSend = string.Empty;
    private int connections;
    private List<string> messages = new List<string>();
    private Vector2 scrollPos;
    private Texture2D FrameTexture;
    [CompilerGenerated]
    private static Action <>f__am$cache0;

    private void AddMessage(string msg)
    {
        this.messages.Insert(0, msg);
        if (this.messages.Count > this.MaxMessages)
        {
            this.messages.RemoveRange(this.MaxMessages, this.messages.Count - this.MaxMessages);
        }
    }

    private void DrawChat(bool withInput = true)
    {
        GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
        this.scrollPos = GUILayout.BeginScrollView(this.scrollPos, false, false, Array.Empty<GUILayoutOption>());
        for (int i = 0; i < this.messages.Count; i++)
        {
            GUILayoutOption[] optionArray1 = new GUILayoutOption[] { GUILayout.MinWidth((float) Screen.width) };
            GUILayout.Label(this.messages[i], optionArray1);
        }
        GUILayout.EndScrollView();
        if (withInput)
        {
            GUILayout.Label("Your message: ", Array.Empty<GUILayoutOption>());
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            this.messageToSend = GUILayout.TextField(this.messageToSend, Array.Empty<GUILayoutOption>());
            GUILayoutOption[] optionArray2 = new GUILayoutOption[] { GUILayout.MaxWidth(100f) };
            if (GUILayout.Button("Send", optionArray2))
            {
                this.SendMessage();
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();
    }

    private void DrawControls()
    {
        GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
        GUILayout.Label("Controls:", Array.Empty<GUILayoutOption>());
        for (int i = 0; i < this.controls.Length; i++)
        {
            if (GUILayout.Button(this.controls[i], Array.Empty<GUILayoutOption>()))
            {
                object[] args = new object[] { this.controls[i] };
                this.Socket.Emit("move", args);
            }
        }
        GUILayout.Label(" Connections: " + this.connections, Array.Empty<GUILayoutOption>());
        GUILayout.EndHorizontal();
    }

    private void DrawLoginScreen()
    {
        GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
        GUILayout.FlexibleSpace();
        GUIHelper.DrawCenteredText("What's your nickname?");
        this.Nick = GUILayout.TextField(this.Nick, Array.Empty<GUILayoutOption>());
        if (GUILayout.Button("Join", Array.Empty<GUILayoutOption>()))
        {
            this.Join();
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
    }

    private void Join()
    {
        PlayerPrefs.SetString("Nick", this.Nick);
        object[] args = new object[] { this.Nick };
        this.Socket.Emit("join", args);
    }

    private void OnConnected(BestHTTP.SocketIO.Socket socket, Packet packet, params object[] args)
    {
        if (PlayerPrefs.HasKey("Nick"))
        {
            this.Nick = PlayerPrefs.GetString("Nick", "NickName");
            this.Join();
        }
        else
        {
            this.State = States.WaitForNick;
        }
        this.AddMessage("connected");
    }

    private void OnConnections(BestHTTP.SocketIO.Socket socket, Packet packet, params object[] args)
    {
        this.connections = Convert.ToInt32(args[0]);
    }

    private void OnDestroy()
    {
        this.Socket.Manager.Close();
    }

    private void OnError(BestHTTP.SocketIO.Socket socket, Packet packet, params object[] args)
    {
        this.AddMessage($"--ERROR - {args[0].ToString()}");
    }

    private void OnFrame(BestHTTP.SocketIO.Socket socket, Packet packet, params object[] args)
    {
        if (this.State == States.Joined)
        {
            if (this.FrameTexture == null)
            {
                this.FrameTexture = new Texture2D(0, 0, TextureFormat.RGBA32, false);
                this.FrameTexture.filterMode = FilterMode.Point;
            }
            byte[] data = packet.Attachments[0];
            this.FrameTexture.LoadImage(data);
        }
    }

    private void OnGUI()
    {
        switch (this.State)
        {
            case States.Connecting:
                if (<>f__am$cache0 == null)
                {
                    <>f__am$cache0 = delegate {
                        GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
                        GUILayout.FlexibleSpace();
                        GUIHelper.DrawCenteredText("Connecting to the server...");
                        GUILayout.FlexibleSpace();
                        GUILayout.EndVertical();
                    };
                }
                GUIHelper.DrawArea(GUIHelper.ClientArea, true, <>f__am$cache0);
                break;

            case States.WaitForNick:
                GUIHelper.DrawArea(GUIHelper.ClientArea, true, () => this.DrawLoginScreen());
                break;

            case States.Joined:
                GUIHelper.DrawArea(GUIHelper.ClientArea, true, delegate {
                    if (this.FrameTexture != null)
                    {
                        GUILayout.Box(this.FrameTexture, Array.Empty<GUILayoutOption>());
                    }
                    this.DrawControls();
                    this.DrawChat(true);
                });
                break;
        }
    }

    private void OnJoin(BestHTTP.SocketIO.Socket socket, Packet packet, params object[] args)
    {
        string str = (args.Length <= 1) ? string.Empty : $"({args[1]})";
        this.AddMessage($"{args[0]} joined {str}");
    }

    private void OnJoined(BestHTTP.SocketIO.Socket socket, Packet packet, params object[] args)
    {
        this.State = States.Joined;
    }

    private void OnMessage(BestHTTP.SocketIO.Socket socket, Packet packet, params object[] args)
    {
        if (args.Length == 1)
        {
            this.AddMessage(args[0] as string);
        }
        else
        {
            this.AddMessage($"{args[1]}: {args[0]}");
        }
    }

    private void OnMove(BestHTTP.SocketIO.Socket socket, Packet packet, params object[] args)
    {
        this.AddMessage($"{args[1]} pressed {args[0]}");
    }

    private void OnReload(BestHTTP.SocketIO.Socket socket, Packet packet, params object[] args)
    {
        this.Reload();
    }

    private void Reload()
    {
        this.FrameTexture = null;
        if (this.Socket != null)
        {
            this.Socket.Manager.Close();
            this.Socket = null;
            this.Start();
        }
    }

    private void SendMessage()
    {
        if (!string.IsNullOrEmpty(this.messageToSend))
        {
            object[] args = new object[] { this.messageToSend };
            this.Socket.Emit("message", args);
            this.AddMessage($"{this.Nick}: {this.messageToSend}");
            this.messageToSend = string.Empty;
        }
    }

    private void Start()
    {
        SocketOptions options = new SocketOptions {
            AutoConnect = false
        };
        SocketManager manager = new SocketManager(new Uri("http://io.weplay.io/socket.io/"), options);
        this.Socket = manager.Socket;
        this.Socket.On(SocketIOEventTypes.Connect, new SocketIOCallback(this.OnConnected));
        this.Socket.On("joined", new SocketIOCallback(this.OnJoined));
        this.Socket.On("connections", new SocketIOCallback(this.OnConnections));
        this.Socket.On("join", new SocketIOCallback(this.OnJoin));
        this.Socket.On("move", new SocketIOCallback(this.OnMove));
        this.Socket.On("message", new SocketIOCallback(this.OnMessage));
        this.Socket.On("reload", new SocketIOCallback(this.OnReload));
        this.Socket.On("frame", new SocketIOCallback(this.OnFrame), false);
        this.Socket.On(SocketIOEventTypes.Error, new SocketIOCallback(this.OnError));
        manager.Open();
        this.State = States.Connecting;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SampleSelector.SelectedSample.DestroyUnityObject();
        }
    }

    private enum States
    {
        Connecting,
        WaitForNick,
        Joined
    }
}

