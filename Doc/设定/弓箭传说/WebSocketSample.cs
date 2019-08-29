using BestHTTP;
using BestHTTP.Examples;
using BestHTTP.WebSocket;
using System;
using UnityEngine;

public class WebSocketSample : MonoBehaviour
{
    private string address = "wss://echo.websocket.org";
    private string msgToSend = "Hello World!";
    private string Text = string.Empty;
    private BestHTTP.WebSocket.WebSocket webSocket;
    private Vector2 scrollPos;

    private void OnClosed(BestHTTP.WebSocket.WebSocket ws, ushort code, string message)
    {
        this.Text = this.Text + $"-WebSocket closed! Code: {code} Message: {message}
";
        this.webSocket = null;
    }

    private void OnDestroy()
    {
        if (this.webSocket != null)
        {
            this.webSocket.Close();
        }
    }

    private void OnError(BestHTTP.WebSocket.WebSocket ws, Exception ex)
    {
        string str = string.Empty;
        if (ws.InternalRequest.Response != null)
        {
            str = $"Status Code from Server: {ws.InternalRequest.Response.StatusCode} and Message: {ws.InternalRequest.Response.Message}";
        }
        this.Text = this.Text + $"-An error occured: {((ex == null) ? ("Unknown Error " + str) : ex.Message)}
";
        this.webSocket = null;
    }

    private void OnGUI()
    {
        GUIHelper.DrawArea(GUIHelper.ClientArea, true, delegate {
            this.scrollPos = GUILayout.BeginScrollView(this.scrollPos, Array.Empty<GUILayoutOption>());
            GUILayout.Label(this.Text, Array.Empty<GUILayoutOption>());
            GUILayout.EndScrollView();
            GUILayout.Space(5f);
            GUILayout.FlexibleSpace();
            this.address = GUILayout.TextField(this.address, Array.Empty<GUILayoutOption>());
            if ((this.webSocket == null) && GUILayout.Button("Open Web Socket", Array.Empty<GUILayoutOption>()))
            {
                this.webSocket = new BestHTTP.WebSocket.WebSocket(new Uri(this.address));
                if (HTTPManager.Proxy != null)
                {
                    this.webSocket.InternalRequest.Proxy = new HTTPProxy(HTTPManager.Proxy.Address, HTTPManager.Proxy.Credentials, false);
                }
                this.webSocket.OnOpen = (OnWebSocketOpenDelegate) Delegate.Combine(this.webSocket.OnOpen, new OnWebSocketOpenDelegate(this.OnOpen));
                this.webSocket.OnMessage = (OnWebSocketMessageDelegate) Delegate.Combine(this.webSocket.OnMessage, new OnWebSocketMessageDelegate(this.OnMessageReceived));
                this.webSocket.OnClosed = (OnWebSocketClosedDelegate) Delegate.Combine(this.webSocket.OnClosed, new OnWebSocketClosedDelegate(this.OnClosed));
                this.webSocket.OnError = (OnWebSocketErrorDelegate) Delegate.Combine(this.webSocket.OnError, new OnWebSocketErrorDelegate(this.OnError));
                this.webSocket.Open();
                this.Text = this.Text + "Opening Web Socket...\n";
            }
            if ((this.webSocket != null) && this.webSocket.IsOpen)
            {
                GUILayout.Space(10f);
                GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
                this.msgToSend = GUILayout.TextField(this.msgToSend, Array.Empty<GUILayoutOption>());
                GUILayoutOption[] optionArray1 = new GUILayoutOption[] { GUILayout.MaxWidth(70f) };
                if (GUILayout.Button("Send", optionArray1))
                {
                    this.Text = this.Text + "Sending message...\n";
                    this.webSocket.Send(this.msgToSend);
                }
                GUILayout.EndHorizontal();
                GUILayout.Space(10f);
                if (GUILayout.Button("Close", Array.Empty<GUILayoutOption>()))
                {
                    this.webSocket.Close(0x3e8, "Bye!");
                }
            }
        });
    }

    private void OnMessageReceived(BestHTTP.WebSocket.WebSocket ws, string message)
    {
        this.Text = this.Text + $"-Message received: {message}
";
    }

    private void OnOpen(BestHTTP.WebSocket.WebSocket ws)
    {
        this.Text = this.Text + string.Format("-WebSocket Open!\n", Array.Empty<object>());
    }
}

