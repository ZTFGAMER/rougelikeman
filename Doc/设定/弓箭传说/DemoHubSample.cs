using BestHTTP.Examples;
using BestHTTP.SignalR;
using BestHTTP.SignalR.Hubs;
using BestHTTP.SignalR.JsonEncoders;
using BestHTTP.SignalR.Messages;
using System;
using UnityEngine;

internal class DemoHubSample : MonoBehaviour
{
    private readonly Uri URI = new Uri("https://besthttpsignalr.azurewebsites.net/signalr");
    private Connection signalRConnection;
    private DemoHub demoHub;
    private TypedDemoHub typedDemoHub;
    private Hub vbDemoHub;
    private string vbReadStateResult = string.Empty;
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
            this.demoHub.Draw();
            this.typedDemoHub.Draw();
            GUILayout.Label("Read State Value", Array.Empty<GUILayoutOption>());
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Space(20f);
            GUILayout.Label(this.vbReadStateResult, Array.Empty<GUILayoutOption>());
            GUILayout.EndHorizontal();
            GUILayout.Space(10f);
            GUILayout.EndVertical();
            GUILayout.EndScrollView();
        });
    }

    private void Start()
    {
        this.demoHub = new DemoHub();
        this.typedDemoHub = new TypedDemoHub();
        this.vbDemoHub = new Hub("vbdemo");
        Hub[] hubs = new Hub[] { this.demoHub, this.typedDemoHub, this.vbDemoHub };
        this.signalRConnection = new Connection(this.URI, hubs);
        this.signalRConnection.JsonEncoder = new LitJsonEncoder();
        this.signalRConnection.OnConnected += delegate (Connection connection) {
            <>__AnonType2<string, int, <>__AnonType1<string, string>> person = new <>__AnonType2<string, int, <>__AnonType1<string, string>>("Foo", 20, new <>__AnonType1<string, string>("One Microsoft Way", "98052"));
            this.demoHub.AddToGroups();
            this.demoHub.GetValue();
            this.demoHub.TaskWithException();
            this.demoHub.GenericTaskWithException();
            this.demoHub.SynchronousException();
            this.demoHub.DynamicTask();
            this.demoHub.PassingDynamicComplex(person);
            this.demoHub.SimpleArray(new int[] { 5, 5, 6 });
            this.demoHub.ComplexType(person);
            object[] complexArray = new object[] { person, person, person };
            this.demoHub.ComplexArray(complexArray);
            this.demoHub.ReportProgress("Long running job!");
            this.demoHub.Overload();
            this.demoHub.State["name"] = "Testing state!";
            this.demoHub.ReadStateValue();
            this.demoHub.PlainTask();
            this.demoHub.GenericTaskWithContinueWith();
            this.typedDemoHub.Echo("Typed echo callback");
            this.vbDemoHub.Call("readStateValue", (hub, msg, result) => this.vbReadStateResult = $"Read some state from VB.NET! => {(result.ReturnValue != null) ? result.ReturnValue.ToString() : "undefined"}", Array.Empty<object>());
        };
        this.signalRConnection.Open();
    }
}

