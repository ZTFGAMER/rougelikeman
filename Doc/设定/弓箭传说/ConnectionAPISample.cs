using BestHTTP.Cookies;
using BestHTTP.Examples;
using BestHTTP.JSON;
using BestHTTP.SignalR;
using BestHTTP.SignalR.JsonEncoders;
using System;
using UnityEngine;

public sealed class ConnectionAPISample : MonoBehaviour
{
    private readonly Uri URI = new Uri("https://besthttpsignalr.azurewebsites.net/raw-connection/");
    private Connection signalRConnection;
    private string ToEveryBodyText = string.Empty;
    private string ToMeText = string.Empty;
    private string PrivateMessageText = string.Empty;
    private string PrivateMessageUserOrGroupName = string.Empty;
    private GUIMessageList messages = new GUIMessageList();

    private void Broadcast(string text)
    {
        this.signalRConnection.Send(new <>__AnonType0<MessageTypes, string>(MessageTypes.Broadcast, text));
    }

    private void BroadcastExceptMe(string text)
    {
        this.signalRConnection.Send(new <>__AnonType0<MessageTypes, string>(MessageTypes.BroadcastExceptMe, text));
    }

    private void EnterName(string name)
    {
        this.signalRConnection.Send(new <>__AnonType0<MessageTypes, string>(MessageTypes.Join, name));
    }

    private void JoinGroup(string groupName)
    {
        this.signalRConnection.Send(new <>__AnonType0<MessageTypes, string>(MessageTypes.AddToGroup, groupName));
    }

    private void LeaveGroup(string groupName)
    {
        this.signalRConnection.Send(new <>__AnonType0<MessageTypes, string>(MessageTypes.RemoveFromGroup, groupName));
    }

    private void OnDestroy()
    {
        this.signalRConnection.Close();
    }

    private void OnGUI()
    {
        GUIHelper.DrawArea(GUIHelper.ClientArea, true, delegate {
            GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
            GUILayout.Label("To Everybody", Array.Empty<GUILayoutOption>());
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayoutOption[] optionArray1 = new GUILayoutOption[] { GUILayout.MinWidth(100f) };
            this.ToEveryBodyText = GUILayout.TextField(this.ToEveryBodyText, optionArray1);
            if (GUILayout.Button("Broadcast", Array.Empty<GUILayoutOption>()))
            {
                this.Broadcast(this.ToEveryBodyText);
            }
            if (GUILayout.Button("Broadcast (All Except Me)", Array.Empty<GUILayoutOption>()))
            {
                this.BroadcastExceptMe(this.ToEveryBodyText);
            }
            if (GUILayout.Button("Enter Name", Array.Empty<GUILayoutOption>()))
            {
                this.EnterName(this.ToEveryBodyText);
            }
            if (GUILayout.Button("Join Group", Array.Empty<GUILayoutOption>()))
            {
                this.JoinGroup(this.ToEveryBodyText);
            }
            if (GUILayout.Button("Leave Group", Array.Empty<GUILayoutOption>()))
            {
                this.LeaveGroup(this.ToEveryBodyText);
            }
            GUILayout.EndHorizontal();
            GUILayout.Label("To Me", Array.Empty<GUILayoutOption>());
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayoutOption[] optionArray2 = new GUILayoutOption[] { GUILayout.MinWidth(100f) };
            this.ToMeText = GUILayout.TextField(this.ToMeText, optionArray2);
            if (GUILayout.Button("Send to me", Array.Empty<GUILayoutOption>()))
            {
                this.SendToMe(this.ToMeText);
            }
            GUILayout.EndHorizontal();
            GUILayout.Label("Private Message", Array.Empty<GUILayoutOption>());
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Label("Message:", Array.Empty<GUILayoutOption>());
            GUILayoutOption[] optionArray3 = new GUILayoutOption[] { GUILayout.MinWidth(100f) };
            this.PrivateMessageText = GUILayout.TextField(this.PrivateMessageText, optionArray3);
            GUILayout.Label("User or Group name:", Array.Empty<GUILayoutOption>());
            GUILayoutOption[] optionArray4 = new GUILayoutOption[] { GUILayout.MinWidth(100f) };
            this.PrivateMessageUserOrGroupName = GUILayout.TextField(this.PrivateMessageUserOrGroupName, optionArray4);
            if (GUILayout.Button("Send to user", Array.Empty<GUILayoutOption>()))
            {
                this.SendToUser(this.PrivateMessageUserOrGroupName, this.PrivateMessageText);
            }
            if (GUILayout.Button("Send to group", Array.Empty<GUILayoutOption>()))
            {
                this.SendToGroup(this.PrivateMessageUserOrGroupName, this.PrivateMessageText);
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(20f);
            if (this.signalRConnection.State == ConnectionStates.Closed)
            {
                if (GUILayout.Button("Start Connection", Array.Empty<GUILayoutOption>()))
                {
                    this.signalRConnection.Open();
                }
            }
            else if (GUILayout.Button("Stop Connection", Array.Empty<GUILayoutOption>()))
            {
                this.signalRConnection.Close();
            }
            GUILayout.Space(20f);
            GUILayout.Label("Messages", Array.Empty<GUILayoutOption>());
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Space(20f);
            this.messages.Draw((float) (Screen.width - 20), 0f);
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        });
    }

    private void SendToGroup(string userOrGroupName, string text)
    {
        this.signalRConnection.Send(new <>__AnonType0<MessageTypes, string>(MessageTypes.SendToGroup, $"{userOrGroupName}|{text}"));
    }

    private void SendToMe(string text)
    {
        this.signalRConnection.Send(new <>__AnonType0<MessageTypes, string>(MessageTypes.Send, text));
    }

    private void SendToUser(string userOrGroupName, string text)
    {
        this.signalRConnection.Send(new <>__AnonType0<MessageTypes, string>(MessageTypes.PrivateMessage, $"{userOrGroupName}|{text}"));
    }

    private void signalRConnection_OnGeneralMessage(Connection manager, object data)
    {
        this.messages.Add("[Server Message] " + Json.Encode(data));
    }

    private void signalRConnection_OnStateChanged(Connection manager, ConnectionStates oldState, ConnectionStates newState)
    {
        this.messages.Add($"[State Change] {oldState.ToString()} => {newState.ToString()}");
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("userName"))
        {
            CookieJar.Set(this.URI, new Cookie("user", PlayerPrefs.GetString("userName")));
        }
        this.signalRConnection = new Connection(this.URI);
        this.signalRConnection.JsonEncoder = new LitJsonEncoder();
        this.signalRConnection.OnStateChanged += new OnStateChanged(this.signalRConnection_OnStateChanged);
        this.signalRConnection.OnNonHubMessage += new OnNonHubMessageDelegate(this.signalRConnection_OnGeneralMessage);
        this.signalRConnection.Open();
    }

    private enum MessageTypes
    {
        Send,
        Broadcast,
        Join,
        PrivateMessage,
        AddToGroup,
        RemoveFromGroup,
        SendToGroup,
        BroadcastExceptMe
    }
}

