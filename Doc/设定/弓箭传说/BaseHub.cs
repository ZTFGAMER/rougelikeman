using BestHTTP.Examples;
using BestHTTP.SignalR.Hubs;
using BestHTTP.SignalR.Messages;
using System;
using System.Collections.Generic;
using UnityEngine;

internal class BaseHub : Hub
{
    private string Title;
    private GUIMessageList messages;

    public BaseHub(string name, string title) : base(name)
    {
        this.messages = new GUIMessageList();
        this.Title = title;
        base.On("joined", new OnMethodCallCallbackDelegate(this.Joined));
        base.On("rejoined", new OnMethodCallCallbackDelegate(this.Rejoined));
        base.On("left", new OnMethodCallCallbackDelegate(this.Left));
        base.On("invoked", new OnMethodCallCallbackDelegate(this.Invoked));
    }

    public void Draw()
    {
        GUILayout.Label(this.Title, Array.Empty<GUILayoutOption>());
        GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
        GUILayout.Space(20f);
        this.messages.Draw((float) (Screen.width - 20), 100f);
        GUILayout.EndHorizontal();
    }

    private void Invoked(Hub hub, MethodCallMessage methodCall)
    {
        this.messages.Add($"{methodCall.Arguments[0]} invoked hub method at {methodCall.Arguments[1]}");
    }

    public void InvokedFromClient()
    {
        base.Call("invokedFromClient", new OnMethodResultDelegate(this.OnInvoked), new OnMethodFailedDelegate(this.OnInvokeFailed), Array.Empty<object>());
    }

    private void Joined(Hub hub, MethodCallMessage methodCall)
    {
        Dictionary<string, object> dictionary = methodCall.Arguments[2] as Dictionary<string, object>;
        this.messages.Add($"{methodCall.Arguments[0]} joined at {methodCall.Arguments[1]}
	IsAuthenticated: {dictionary["IsAuthenticated"]} IsAdmin: {dictionary["IsAdmin"]} UserName: {dictionary["UserName"]}");
    }

    private void Left(Hub hub, MethodCallMessage methodCall)
    {
        this.messages.Add($"{methodCall.Arguments[0]} left at {methodCall.Arguments[1]}");
    }

    private void OnInvoked(Hub hub, ClientMessage originalMessage, ResultMessage result)
    {
        Debug.Log(hub.Name + " invokedFromClient success!");
    }

    private void OnInvokeFailed(Hub hub, ClientMessage originalMessage, FailureMessage result)
    {
        Debug.LogWarning(hub.Name + " " + result.ErrorMessage);
    }

    private void Rejoined(Hub hub, MethodCallMessage methodCall)
    {
        this.messages.Add($"{methodCall.Arguments[0]} reconnected at {methodCall.Arguments[1]}");
    }
}

