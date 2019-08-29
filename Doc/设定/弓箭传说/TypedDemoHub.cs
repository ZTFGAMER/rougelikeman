using BestHTTP.SignalR.Hubs;
using BestHTTP.SignalR.Messages;
using System;
using UnityEngine;

internal class TypedDemoHub : Hub
{
    private string typedEchoResult;
    private string typedEchoClientResult;

    public TypedDemoHub() : base("typeddemohub")
    {
        this.typedEchoResult = string.Empty;
        this.typedEchoClientResult = string.Empty;
        base.On("Echo", new OnMethodCallCallbackDelegate(this.Echo));
    }

    public void Draw()
    {
        GUILayout.Label("Typed callback", Array.Empty<GUILayoutOption>());
        GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
        GUILayout.Space(20f);
        GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
        GUILayout.Label(this.typedEchoResult, Array.Empty<GUILayoutOption>());
        GUILayout.Label(this.typedEchoClientResult, Array.Empty<GUILayoutOption>());
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        GUILayout.Space(10f);
    }

    public void Echo(string msg)
    {
        object[] args = new object[] { msg };
        base.Call("echo", new OnMethodResultDelegate(this.OnEcho_Done), args);
    }

    private void Echo(Hub hub, MethodCallMessage methodCall)
    {
        this.typedEchoClientResult = $"{methodCall.Arguments[0]} #{methodCall.Arguments[1]} triggered!";
    }

    private void OnEcho_Done(Hub hub, ClientMessage originalMessage, ResultMessage result)
    {
        this.typedEchoResult = "TypedDemoHub.Echo(string message) invoked!";
    }
}

