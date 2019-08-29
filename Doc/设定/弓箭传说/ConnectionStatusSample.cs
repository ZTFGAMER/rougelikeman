using BestHTTP.Examples;
using BestHTTP.SignalR;
using BestHTTP.SignalR.Hubs;
using System;
using UnityEngine;

internal sealed class ConnectionStatusSample : MonoBehaviour
{
    private readonly Uri URI = new Uri("https://besthttpsignalr.azurewebsites.net/signalr");
    private Connection signalRConnection;
    private GUIMessageList messages = new GUIMessageList();

    private void OnDestroy()
    {
        this.signalRConnection.Close();
    }

    private void OnGUI()
    {
        GUIHelper.DrawArea(GUIHelper.ClientArea, true, delegate {
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            if (GUILayout.Button("START", Array.Empty<GUILayoutOption>()) && (this.signalRConnection.State != ConnectionStates.Connected))
            {
                this.signalRConnection.Open();
            }
            if (GUILayout.Button("STOP", Array.Empty<GUILayoutOption>()) && (this.signalRConnection.State == ConnectionStates.Connected))
            {
                this.signalRConnection.Close();
                this.messages.Clear();
            }
            if (GUILayout.Button("PING", Array.Empty<GUILayoutOption>()) && (this.signalRConnection.State == ConnectionStates.Connected))
            {
                this.signalRConnection["StatusHub"].Call("Ping", Array.Empty<object>());
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(20f);
            GUILayout.Label("Connection Status Messages", Array.Empty<GUILayoutOption>());
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Space(20f);
            this.messages.Draw((float) (Screen.width - 20), 0f);
            GUILayout.EndHorizontal();
        });
    }

    private void signalRConnection_OnError(Connection manager, string error)
    {
        this.messages.Add("[Error] " + error);
    }

    private void signalRConnection_OnNonHubMessage(Connection manager, object data)
    {
        this.messages.Add("[Server Message] " + data.ToString());
    }

    private void signalRConnection_OnStateChanged(Connection manager, ConnectionStates oldState, ConnectionStates newState)
    {
        this.messages.Add($"[State Change] {oldState} => {newState}");
    }

    private void Start()
    {
        string[] hubNames = new string[] { "StatusHub" };
        this.signalRConnection = new Connection(this.URI, hubNames);
        this.signalRConnection.OnNonHubMessage += new OnNonHubMessageDelegate(this.signalRConnection_OnNonHubMessage);
        this.signalRConnection.OnError += new OnErrorDelegate(this.signalRConnection_OnError);
        this.signalRConnection.OnStateChanged += new OnStateChanged(this.signalRConnection_OnStateChanged);
        this.signalRConnection["StatusHub"].OnMethodCall += new OnMethodCallDelegate(this.statusHub_OnMethodCall);
        this.signalRConnection.Open();
    }

    private void statusHub_OnMethodCall(Hub hub, string method, params object[] args)
    {
        string str = (args.Length <= 0) ? string.Empty : (args[0] as string);
        string str2 = (args.Length <= 1) ? string.Empty : args[1].ToString();
        if (method != null)
        {
            if (method == "joined")
            {
                this.messages.Add($"[{hub.Name}] {str} joined at {str2}");
                return;
            }
            if (method == "rejoined")
            {
                this.messages.Add($"[{hub.Name}] {str} reconnected at {str2}");
                return;
            }
            if (method == "leave")
            {
                this.messages.Add($"[{hub.Name}] {str} leaved at {str2}");
                return;
            }
        }
        this.messages.Add($"[{hub.Name}] {method}");
    }
}

