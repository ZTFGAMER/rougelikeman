using BestHTTP.Examples;
using BestHTTP.SignalR.Hubs;
using BestHTTP.SignalR.Messages;
using System;
using UnityEngine;

internal class DemoHub : Hub
{
    private float longRunningJobProgress;
    private string longRunningJobStatus;
    private string fromArbitraryCodeResult;
    private string groupAddedResult;
    private string dynamicTaskResult;
    private string genericTaskResult;
    private string taskWithExceptionResult;
    private string genericTaskWithExceptionResult;
    private string synchronousExceptionResult;
    private string invokingHubMethodWithDynamicResult;
    private string simpleArrayResult;
    private string complexTypeResult;
    private string complexArrayResult;
    private string voidOverloadResult;
    private string intOverloadResult;
    private string readStateResult;
    private string plainTaskResult;
    private string genericTaskWithContinueWithResult;
    private GUIMessageList invokeResults;

    public DemoHub() : base("demo")
    {
        this.longRunningJobStatus = "Not Started!";
        this.fromArbitraryCodeResult = string.Empty;
        this.groupAddedResult = string.Empty;
        this.dynamicTaskResult = string.Empty;
        this.genericTaskResult = string.Empty;
        this.taskWithExceptionResult = string.Empty;
        this.genericTaskWithExceptionResult = string.Empty;
        this.synchronousExceptionResult = string.Empty;
        this.invokingHubMethodWithDynamicResult = string.Empty;
        this.simpleArrayResult = string.Empty;
        this.complexTypeResult = string.Empty;
        this.complexArrayResult = string.Empty;
        this.voidOverloadResult = string.Empty;
        this.intOverloadResult = string.Empty;
        this.readStateResult = string.Empty;
        this.plainTaskResult = string.Empty;
        this.genericTaskWithContinueWithResult = string.Empty;
        this.invokeResults = new GUIMessageList();
        base.On("invoke", new OnMethodCallCallbackDelegate(this.Invoke));
        base.On("signal", new OnMethodCallCallbackDelegate(this.Signal));
        base.On("groupAdded", new OnMethodCallCallbackDelegate(this.GroupAdded));
        base.On("fromArbitraryCode", new OnMethodCallCallbackDelegate(this.FromArbitraryCode));
    }

    public void AddToGroups()
    {
        base.Call("addToGroups", Array.Empty<object>());
    }

    public void ComplexArray(object[] complexArray)
    {
        object[] args = new object[] { complexArray };
        base.Call("ComplexArray", (hub, msg, result) => this.complexArrayResult = "Complex Array Works!", args);
    }

    public void ComplexType(object person)
    {
        object[] args = new object[] { person };
        base.Call("complexType", (hub, msg, result) => this.complexTypeResult = $"Complex Type -> {this.Connection.JsonEncoder.Encode(base.State["person"])}", args);
    }

    public void Draw()
    {
        GUILayout.Label("Arbitrary Code", Array.Empty<GUILayoutOption>());
        GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
        GUILayout.Space(20f);
        GUILayout.Label($"Sending {this.fromArbitraryCodeResult} from arbitrary code without the hub itself!", Array.Empty<GUILayoutOption>());
        GUILayout.EndHorizontal();
        GUILayout.Space(10f);
        GUILayout.Label("Group Added", Array.Empty<GUILayoutOption>());
        GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
        GUILayout.Space(20f);
        GUILayout.Label(this.groupAddedResult, Array.Empty<GUILayoutOption>());
        GUILayout.EndHorizontal();
        GUILayout.Space(10f);
        GUILayout.Label("Dynamic Task", Array.Empty<GUILayoutOption>());
        GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
        GUILayout.Space(20f);
        GUILayout.Label(this.dynamicTaskResult, Array.Empty<GUILayoutOption>());
        GUILayout.EndHorizontal();
        GUILayout.Space(10f);
        GUILayout.Label("Report Progress", Array.Empty<GUILayoutOption>());
        GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
        GUILayout.Space(20f);
        GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
        GUILayout.Label(this.longRunningJobStatus, Array.Empty<GUILayoutOption>());
        GUILayout.HorizontalSlider(this.longRunningJobProgress, 0f, 100f, Array.Empty<GUILayoutOption>());
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        GUILayout.Space(10f);
        GUILayout.Label("Generic Task", Array.Empty<GUILayoutOption>());
        GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
        GUILayout.Space(20f);
        GUILayout.Label(this.genericTaskResult, Array.Empty<GUILayoutOption>());
        GUILayout.EndHorizontal();
        GUILayout.Space(10f);
        GUILayout.Label("Task With Exception", Array.Empty<GUILayoutOption>());
        GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
        GUILayout.Space(20f);
        GUILayout.Label(this.taskWithExceptionResult, Array.Empty<GUILayoutOption>());
        GUILayout.EndHorizontal();
        GUILayout.Space(10f);
        GUILayout.Label("Generic Task With Exception", Array.Empty<GUILayoutOption>());
        GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
        GUILayout.Space(20f);
        GUILayout.Label(this.genericTaskWithExceptionResult, Array.Empty<GUILayoutOption>());
        GUILayout.EndHorizontal();
        GUILayout.Space(10f);
        GUILayout.Label("Synchronous Exception", Array.Empty<GUILayoutOption>());
        GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
        GUILayout.Space(20f);
        GUILayout.Label(this.synchronousExceptionResult, Array.Empty<GUILayoutOption>());
        GUILayout.EndHorizontal();
        GUILayout.Space(10f);
        GUILayout.Label("Invoking hub method with dynamic", Array.Empty<GUILayoutOption>());
        GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
        GUILayout.Space(20f);
        GUILayout.Label(this.invokingHubMethodWithDynamicResult, Array.Empty<GUILayoutOption>());
        GUILayout.EndHorizontal();
        GUILayout.Space(10f);
        GUILayout.Label("Simple Array", Array.Empty<GUILayoutOption>());
        GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
        GUILayout.Space(20f);
        GUILayout.Label(this.simpleArrayResult, Array.Empty<GUILayoutOption>());
        GUILayout.EndHorizontal();
        GUILayout.Space(10f);
        GUILayout.Label("Complex Type", Array.Empty<GUILayoutOption>());
        GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
        GUILayout.Space(20f);
        GUILayout.Label(this.complexTypeResult, Array.Empty<GUILayoutOption>());
        GUILayout.EndHorizontal();
        GUILayout.Space(10f);
        GUILayout.Label("Complex Array", Array.Empty<GUILayoutOption>());
        GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
        GUILayout.Space(20f);
        GUILayout.Label(this.complexArrayResult, Array.Empty<GUILayoutOption>());
        GUILayout.EndHorizontal();
        GUILayout.Space(10f);
        GUILayout.Label("Overloads", Array.Empty<GUILayoutOption>());
        GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
        GUILayout.Space(20f);
        GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
        GUILayout.Label(this.voidOverloadResult, Array.Empty<GUILayoutOption>());
        GUILayout.Label(this.intOverloadResult, Array.Empty<GUILayoutOption>());
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        GUILayout.Space(10f);
        GUILayout.Label("Read State Value", Array.Empty<GUILayoutOption>());
        GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
        GUILayout.Space(20f);
        GUILayout.Label(this.readStateResult, Array.Empty<GUILayoutOption>());
        GUILayout.EndHorizontal();
        GUILayout.Space(10f);
        GUILayout.Label("Plain Task", Array.Empty<GUILayoutOption>());
        GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
        GUILayout.Space(20f);
        GUILayout.Label(this.plainTaskResult, Array.Empty<GUILayoutOption>());
        GUILayout.EndHorizontal();
        GUILayout.Space(10f);
        GUILayout.Label("Generic Task With ContinueWith", Array.Empty<GUILayoutOption>());
        GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
        GUILayout.Space(20f);
        GUILayout.Label(this.genericTaskWithContinueWithResult, Array.Empty<GUILayoutOption>());
        GUILayout.EndHorizontal();
        GUILayout.Space(10f);
        GUILayout.Label("Message Pump", Array.Empty<GUILayoutOption>());
        GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
        GUILayout.Space(20f);
        this.invokeResults.Draw((float) (Screen.width - 40), 270f);
        GUILayout.EndHorizontal();
        GUILayout.Space(10f);
    }

    public void DynamicTask()
    {
        base.Call("dynamicTask", new OnMethodResultDelegate(this.OnDynamicTask_Done), new OnMethodFailedDelegate(this.OnDynamicTask_Failed), Array.Empty<object>());
    }

    private void FromArbitraryCode(Hub hub, MethodCallMessage methodCall)
    {
        this.fromArbitraryCodeResult = methodCall.Arguments[0] as string;
    }

    public void GenericTaskWithContinueWith()
    {
        base.Call("genericTaskWithContinueWith", (hub, msg, result) => this.genericTaskWithContinueWithResult = result.ReturnValue.ToString(), Array.Empty<object>());
    }

    public void GenericTaskWithException()
    {
        base.Call("genericTaskWithException", null, (hub, msg, error) => this.genericTaskWithExceptionResult = $"Error: {error.ErrorMessage}", Array.Empty<object>());
    }

    public void GetValue()
    {
        base.Call("getValue", (hub, msg, result) => this.genericTaskResult = $"The value is {result.ReturnValue} after 5 seconds", Array.Empty<object>());
    }

    private void GroupAdded(Hub hub, MethodCallMessage methodCall)
    {
        if (!string.IsNullOrEmpty(this.groupAddedResult))
        {
            this.groupAddedResult = "Group Already Added!";
        }
        else
        {
            this.groupAddedResult = "Group Added!";
        }
    }

    private void Invoke(Hub hub, MethodCallMessage methodCall)
    {
        this.invokeResults.Add($"{methodCall.Arguments[0]} client state index -> {base.State["index"]}");
    }

    public void MultipleCalls()
    {
        base.Call("multipleCalls", Array.Empty<object>());
    }

    private void OnDynamicTask_Done(Hub hub, ClientMessage originalMessage, ResultMessage result)
    {
        this.dynamicTaskResult = $"The dynamic task! {result.ReturnValue}";
    }

    private void OnDynamicTask_Failed(Hub hub, ClientMessage originalMessage, FailureMessage result)
    {
        this.dynamicTaskResult = $"The dynamic task failed :( {result.ErrorMessage}";
    }

    private void OnIntOverload_Done(Hub hub, ClientMessage originalMessage, ResultMessage result)
    {
        this.intOverloadResult = $"Overload with return value called => {result.ReturnValue.ToString()}";
    }

    public void OnLongRunningJob_Done(Hub hub, ClientMessage originalMessage, ResultMessage result)
    {
        this.longRunningJobStatus = result.ReturnValue.ToString();
        this.MultipleCalls();
    }

    public void OnLongRunningJob_Progress(Hub hub, ClientMessage originialMessage, ProgressMessage progress)
    {
        this.longRunningJobProgress = (float) progress.Progress;
        this.longRunningJobStatus = progress.Progress.ToString() + "%";
    }

    private void OnVoidOverload_Done(Hub hub, ClientMessage originalMessage, ResultMessage result)
    {
        this.voidOverloadResult = "Void Overload called";
        this.Overload(0x65);
    }

    public void Overload()
    {
        base.Call("Overload", new OnMethodResultDelegate(this.OnVoidOverload_Done), Array.Empty<object>());
    }

    public void Overload(int number)
    {
        object[] args = new object[] { number };
        base.Call("Overload", new OnMethodResultDelegate(this.OnIntOverload_Done), args);
    }

    public void PassingDynamicComplex(object person)
    {
        object[] args = new object[] { person };
        base.Call("passingDynamicComplex", (hub, msg, result) => this.invokingHubMethodWithDynamicResult = $"The person's age is {result.ReturnValue}", args);
    }

    public void PlainTask()
    {
        base.Call("plainTask", (hub, msg, result) => this.plainTaskResult = "Plain Task Result", Array.Empty<object>());
    }

    public void ReadStateValue()
    {
        base.Call("readStateValue", (hub, msg, result) => this.readStateResult = $"Read some state! => {result.ReturnValue}", Array.Empty<object>());
    }

    public void ReportProgress(string arg)
    {
        object[] args = new object[] { arg };
        base.Call("reportProgress", new OnMethodResultDelegate(this.OnLongRunningJob_Done), null, new OnMethodProgressDelegate(this.OnLongRunningJob_Progress), args);
    }

    private void Signal(Hub hub, MethodCallMessage methodCall)
    {
        this.dynamicTaskResult = $"The dynamic task! {methodCall.Arguments[0]}";
    }

    public void SimpleArray(int[] array)
    {
        object[] args = new object[] { array };
        base.Call("simpleArray", (hub, msg, result) => this.simpleArrayResult = "Simple array works!", args);
    }

    public void SynchronousException()
    {
        base.Call("synchronousException", null, (hub, msg, error) => this.synchronousExceptionResult = $"Error: {error.ErrorMessage}", Array.Empty<object>());
    }

    public void TaskWithException()
    {
        base.Call("taskWithException", null, (hub, msg, error) => this.taskWithExceptionResult = $"Error: {error.ErrorMessage}", Array.Empty<object>());
    }
}

