using BestHTTP.Examples;
using BestHTTP.SignalR;
using System;
using UnityEngine;

internal sealed class SimpleStreamingSample : MonoBehaviour
{
    private readonly Uri URI = new Uri("https://besthttpsignalr.azurewebsites.net/streaming-connection");
    private Connection signalRConnection;
    private GUIMessageList messages = new GUIMessageList();

    private void OnDestroy()
    {
        this.signalRConnection.Close();
    }

    private void OnGUI()
    {
        GUIHelper.DrawArea(GUIHelper.ClientArea, true, delegate {
            GUILayout.Label("Messages", Array.Empty<GUILayoutOption>());
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Space(20f);
            this.messages.Draw((float) (Screen.width - 20), 0f);
            GUILayout.EndHorizontal();
        });
    }

    private void signalRConnection_OnError(Connection connection, string error)
    {
        this.messages.Add("[Error] " + error);
    }

    private void signalRConnection_OnNonHubMessage(Connection connection, object data)
    {
        this.messages.Add("[Server Message] " + data.ToString());
    }

    private void signalRConnection_OnStateChanged(Connection connection, ConnectionStates oldState, ConnectionStates newState)
    {
        this.messages.Add($"[State Change] {oldState} => {newState}");
    }

    private void Start()
    {
        this.signalRConnection = new Connection(this.URI);
        this.signalRConnection.OnNonHubMessage += new OnNonHubMessageDelegate(this.signalRConnection_OnNonHubMessage);
        this.signalRConnection.OnStateChanged += new OnStateChanged(this.signalRConnection_OnStateChanged);
        this.signalRConnection.OnError += new OnErrorDelegate(this.signalRConnection_OnError);
        this.signalRConnection.Open();
    }
}

