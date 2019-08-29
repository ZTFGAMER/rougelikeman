using BestHTTP.Examples;
using BestHTTP.SignalR;
using BestHTTP.SignalR.Authentication;
using BestHTTP.SignalR.Hubs;
using System;
using UnityEngine;

internal class AuthenticationSample : MonoBehaviour
{
    private readonly Uri URI = new Uri("https://besthttpsignalr.azurewebsites.net/signalr");
    private Connection signalRConnection;
    private string userName = string.Empty;
    private string role = string.Empty;
    private Vector2 scrollPos;

    private void OnDestroy()
    {
        this.signalRConnection.Close();
    }

    private void OnGUI()
    {
        GUIHelper.DrawArea(GUIHelper.ClientArea, true, delegate {
            this.scrollPos = GUILayout.BeginScrollView(this.scrollPos, false, false, Array.Empty<GUILayoutOption>());
            GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
            if (this.signalRConnection.AuthenticationProvider == null)
            {
                GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
                GUILayout.Label("Username (Enter 'User'):", Array.Empty<GUILayoutOption>());
                GUILayoutOption[] optionArray1 = new GUILayoutOption[] { GUILayout.MinWidth(100f) };
                this.userName = GUILayout.TextField(this.userName, optionArray1);
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
                GUILayout.Label("Roles (Enter 'Invoker' or 'Admin'):", Array.Empty<GUILayoutOption>());
                GUILayoutOption[] optionArray2 = new GUILayoutOption[] { GUILayout.MinWidth(100f) };
                this.role = GUILayout.TextField(this.role, optionArray2);
                GUILayout.EndHorizontal();
                if (GUILayout.Button("Log in", Array.Empty<GUILayoutOption>()))
                {
                    this.Restart();
                }
            }
            for (int i = 0; i < this.signalRConnection.Hubs.Length; i++)
            {
                (this.signalRConnection.Hubs[i] as BaseHub).Draw();
            }
            GUILayout.EndVertical();
            GUILayout.EndScrollView();
        });
    }

    private void Restart()
    {
        this.signalRConnection.OnConnected -= new OnConnectedDelegate(this.signalRConnection_OnConnected);
        this.signalRConnection.Close();
        this.signalRConnection = null;
        this.Start();
    }

    private void signalRConnection_OnConnected(Connection manager)
    {
        for (int i = 0; i < this.signalRConnection.Hubs.Length; i++)
        {
            (this.signalRConnection.Hubs[i] as BaseHub).InvokedFromClient();
        }
    }

    private void Start()
    {
        Hub[] hubs = new Hub[] { new BaseHub("noauthhub", "Messages"), new BaseHub("invokeauthhub", "Messages Invoked By Admin or Invoker"), new BaseHub("authhub", "Messages Requiring Authentication to Send or Receive"), new BaseHub("inheritauthhub", "Messages Requiring Authentication to Send or Receive Because of Inheritance"), new BaseHub("incomingauthhub", "Messages Requiring Authentication to Send"), new BaseHub("adminauthhub", "Messages Requiring Admin Membership to Send or Receive"), new BaseHub("userandroleauthhub", "Messages Requiring Name to be \"User\" and Role to be \"Admin\" to Send or Receive") };
        this.signalRConnection = new Connection(this.URI, hubs);
        if (!string.IsNullOrEmpty(this.userName) && !string.IsNullOrEmpty(this.role))
        {
            this.signalRConnection.AuthenticationProvider = new HeaderAuthenticator(this.userName, this.role);
        }
        this.signalRConnection.OnConnected += new OnConnectedDelegate(this.signalRConnection_OnConnected);
        this.signalRConnection.Open();
    }
}

