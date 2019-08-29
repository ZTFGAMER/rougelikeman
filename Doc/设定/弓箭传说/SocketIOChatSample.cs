using BestHTTP.Examples;
using BestHTTP.SocketIO;
using BestHTTP.SocketIO.Events;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public sealed class SocketIOChatSample : MonoBehaviour
{
    private readonly TimeSpan TYPING_TIMER_LENGTH = TimeSpan.FromMilliseconds(700.0);
    private SocketManager Manager;
    private ChatStates State;
    private string userName = string.Empty;
    private string message = string.Empty;
    private string chatLog = string.Empty;
    private Vector2 scrollPos;
    private bool typing;
    private DateTime lastTypingTime = DateTime.MinValue;
    private List<string> typingUsers = new List<string>();
    [CompilerGenerated]
    private static SocketIOCallback <>f__am$cache0;
    [CompilerGenerated]
    private static SocketIOCallback <>f__am$cache1;
    [CompilerGenerated]
    private static SocketIOCallback <>f__am$cache2;

    private void addChatMessage(Dictionary<string, object> data)
    {
        string str = data["username"] as string;
        string str2 = data["message"] as string;
        this.chatLog = this.chatLog + $"{str}: {str2}
";
    }

    private void AddChatTyping(Dictionary<string, object> data)
    {
        string item = data["username"] as string;
        this.typingUsers.Add(item);
    }

    private void addParticipantsMessage(Dictionary<string, object> data)
    {
        int num = Convert.ToInt32(data["numUsers"]);
        if (num == 1)
        {
            this.chatLog = this.chatLog + "there's 1 participant\n";
        }
        else
        {
            string chatLog = this.chatLog;
            object[] objArray1 = new object[] { chatLog, "there are ", num, " participants\n" };
            this.chatLog = string.Concat(objArray1);
        }
    }

    private void DrawChatScreen()
    {
        GUIHelper.DrawArea(GUIHelper.ClientArea, true, delegate {
            GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
            this.scrollPos = GUILayout.BeginScrollView(this.scrollPos, Array.Empty<GUILayoutOption>());
            GUILayoutOption[] optionArray1 = new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true) };
            GUILayout.Label(this.chatLog, optionArray1);
            GUILayout.EndScrollView();
            string str = string.Empty;
            if (this.typingUsers.Count > 0)
            {
                str = str + $"{this.typingUsers[0]}";
                for (int i = 1; i < this.typingUsers.Count; i++)
                {
                    str = str + $", {this.typingUsers[i]}";
                }
                if (this.typingUsers.Count == 1)
                {
                    str = str + " is typing!";
                }
                else
                {
                    str = str + " are typing!";
                }
            }
            GUILayout.Label(str, Array.Empty<GUILayoutOption>());
            GUILayout.Label("Type here:", Array.Empty<GUILayoutOption>());
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            this.message = GUILayout.TextField(this.message, Array.Empty<GUILayoutOption>());
            GUILayoutOption[] optionArray2 = new GUILayoutOption[] { GUILayout.MaxWidth(100f) };
            if (GUILayout.Button("Send", optionArray2))
            {
                this.SendMessage();
            }
            GUILayout.EndHorizontal();
            if (GUI.get_changed())
            {
                this.UpdateTyping();
            }
            GUILayout.EndVertical();
        });
    }

    private void DrawLoginScreen()
    {
        GUIHelper.DrawArea(GUIHelper.ClientArea, true, delegate {
            GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
            GUILayout.FlexibleSpace();
            GUIHelper.DrawCenteredText("What's your nickname?");
            this.userName = GUILayout.TextField(this.userName, Array.Empty<GUILayoutOption>());
            if (GUILayout.Button("Join", Array.Empty<GUILayoutOption>()))
            {
                this.SetUserName();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
        });
    }

    private void OnDestroy()
    {
        this.Manager.Close();
    }

    private void OnGUI()
    {
        switch (this.State)
        {
            case ChatStates.Login:
                this.DrawLoginScreen();
                break;

            case ChatStates.Chat:
                this.DrawChatScreen();
                break;
        }
    }

    private void OnLogin(Socket socket, Packet packet, params object[] args)
    {
        this.chatLog = "Welcome to Socket.IO Chat — \n";
        this.addParticipantsMessage(args[0] as Dictionary<string, object>);
    }

    private void OnNewMessage(Socket socket, Packet packet, params object[] args)
    {
        this.addChatMessage(args[0] as Dictionary<string, object>);
    }

    private void OnStopTyping(Socket socket, Packet packet, params object[] args)
    {
        this.RemoveChatTyping(args[0] as Dictionary<string, object>);
    }

    private void OnTyping(Socket socket, Packet packet, params object[] args)
    {
        this.AddChatTyping(args[0] as Dictionary<string, object>);
    }

    private void OnUserJoined(Socket socket, Packet packet, params object[] args)
    {
        Dictionary<string, object> data = args[0] as Dictionary<string, object>;
        string str = data["username"] as string;
        this.chatLog = this.chatLog + $"{str} joined
";
        this.addParticipantsMessage(data);
    }

    private void OnUserLeft(Socket socket, Packet packet, params object[] args)
    {
        Dictionary<string, object> data = args[0] as Dictionary<string, object>;
        string str = data["username"] as string;
        this.chatLog = this.chatLog + $"{str} left
";
        this.addParticipantsMessage(data);
    }

    private void RemoveChatTyping(Dictionary<string, object> data)
    {
        <RemoveChatTyping>c__AnonStorey0 storey = new <RemoveChatTyping>c__AnonStorey0 {
            username = data["username"] as string
        };
        int index = this.typingUsers.FindIndex(new Predicate<string>(storey.<>m__0));
        if (index != -1)
        {
            this.typingUsers.RemoveAt(index);
        }
    }

    private void SendMessage()
    {
        if (!string.IsNullOrEmpty(this.message))
        {
            object[] args = new object[] { this.message };
            this.Manager.Socket.Emit("new message", args);
            this.chatLog = this.chatLog + $"{this.userName}: {this.message}
";
            this.message = string.Empty;
        }
    }

    private void SetUserName()
    {
        if (!string.IsNullOrEmpty(this.userName))
        {
            this.State = ChatStates.Chat;
            object[] args = new object[] { this.userName };
            this.Manager.Socket.Emit("add user", args);
        }
    }

    private void Start()
    {
        this.State = ChatStates.Login;
        SocketOptions options = new SocketOptions {
            AutoConnect = false
        };
        this.Manager = new SocketManager(new Uri("http://localhost:3000/socket.io/"), options);
        this.Manager.Socket.On("login", new SocketIOCallback(this.OnLogin));
        this.Manager.Socket.On("new message", new SocketIOCallback(this.OnNewMessage));
        this.Manager.Socket.On("user joined", new SocketIOCallback(this.OnUserJoined));
        this.Manager.Socket.On("user left", new SocketIOCallback(this.OnUserLeft));
        this.Manager.Socket.On("typing", new SocketIOCallback(this.OnTyping));
        this.Manager.Socket.On("stop typing", new SocketIOCallback(this.OnStopTyping));
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = (socket, packet, args) => Debug.LogError($"Error: {args[0].ToString()}");
        }
        this.Manager.Socket.On(SocketIOEventTypes.Error, <>f__am$cache0);
        if (<>f__am$cache1 == null)
        {
            <>f__am$cache1 = delegate (Socket socket, Packet packet, object[] arg) {
                Debug.LogWarning("Connected to /nsp");
                object[] args = new object[] { "Message from /nsp 'on connect'" };
                socket.Emit("testmsg", args);
            };
        }
        this.Manager.GetSocket("/nsp").On(SocketIOEventTypes.Connect, <>f__am$cache1);
        if (<>f__am$cache2 == null)
        {
            <>f__am$cache2 = (socket, packet, arg) => Debug.LogWarning("nsp_message: " + arg[0]);
        }
        this.Manager.GetSocket("/nsp").On("nsp_message", <>f__am$cache2);
        this.Manager.Open();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SampleSelector.SelectedSample.DestroyUnityObject();
        }
        if (this.typing)
        {
            TimeSpan span = (TimeSpan) (DateTime.UtcNow - this.lastTypingTime);
            if (span >= this.TYPING_TIMER_LENGTH)
            {
                this.Manager.Socket.Emit("stop typing", Array.Empty<object>());
                this.typing = false;
            }
        }
    }

    private void UpdateTyping()
    {
        if (!this.typing)
        {
            this.typing = true;
            this.Manager.Socket.Emit("typing", Array.Empty<object>());
        }
        this.lastTypingTime = DateTime.UtcNow;
    }

    [CompilerGenerated]
    private sealed class <RemoveChatTyping>c__AnonStorey0
    {
        internal string username;

        internal bool <>m__0(string name) => 
            name.Equals(this.username);
    }

    private enum ChatStates
    {
        Login,
        Chat
    }
}

