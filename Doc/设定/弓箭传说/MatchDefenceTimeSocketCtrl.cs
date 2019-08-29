using BestHTTP.WebSocket;
using Newtonsoft.Json;
using System;
using System.Runtime.InteropServices;
using System.Text;

public class MatchDefenceTimeSocketCtrl : Singleton<MatchDefenceTimeSocketCtrl>
{
    private const string url = "wss://api-archer.habby.mobi:4443/matching";
    private BestHTTP.WebSocket.WebSocket webSocket;
    private ConnectState mState = ConnectState.eClose;
    private string otheruserid = string.Empty;
    private string myname = string.Empty;

    public void Close()
    {
        if (this.webSocket != null)
        {
            this.Send(MatchMessageType.eUserLeave, 0);
            this.webSocket.OnOpen = null;
            this.webSocket.OnMessage = null;
            this.webSocket.OnError = null;
            this.webSocket.OnClosed = null;
            this.webSocket.Close();
            this.webSocket = null;
            this.mState = ConnectState.eClose;
        }
    }

    public void Connect()
    {
        if (this.webSocket == null)
        {
            this.mState = ConnectState.eConnecting;
            this.init();
            this.otheruserid = string.Empty;
            this.webSocket.Open();
        }
    }

    public void Deinit()
    {
        if (this.webSocket != null)
        {
            this.webSocket.Close();
        }
    }

    private byte[] getBytes(string message) => 
        Encoding.Default.GetBytes(message);

    public void init()
    {
        if (this.webSocket == null)
        {
            this.webSocket = new BestHTTP.WebSocket.WebSocket(new Uri("wss://api-archer.habby.mobi:4443/matching"));
            this.webSocket.OnOpen = (OnWebSocketOpenDelegate) Delegate.Combine(this.webSocket.OnOpen, new OnWebSocketOpenDelegate(this.OnOpen));
            this.webSocket.OnMessage = (OnWebSocketMessageDelegate) Delegate.Combine(this.webSocket.OnMessage, new OnWebSocketMessageDelegate(this.OnMessageReceived));
            this.webSocket.OnBinary = (OnWebSocketBinaryDelegate) Delegate.Combine(this.webSocket.OnBinary, new OnWebSocketBinaryDelegate(this.OnBinaryReceived));
            this.webSocket.OnError = (OnWebSocketErrorDelegate) Delegate.Combine(this.webSocket.OnError, new OnWebSocketErrorDelegate(this.OnError));
            this.webSocket.OnClosed = (OnWebSocketClosedDelegate) Delegate.Combine(this.webSocket.OnClosed, new OnWebSocketClosedDelegate(this.OnClosed));
        }
    }

    private void OnBinaryReceived(BestHTTP.WebSocket.WebSocket ws, byte[] bytes)
    {
        Debugger.Log(Encoding.ASCII.GetString(bytes));
    }

    private void OnClosed(BestHTTP.WebSocket.WebSocket ws, ushort code, string message)
    {
        Debugger.Log("onclose " + message);
    }

    private void OnDestroy()
    {
        if ((this.webSocket != null) && this.webSocket.IsOpen)
        {
            this.webSocket.Close();
            this.Deinit();
        }
    }

    private void OnError(BestHTTP.WebSocket.WebSocket ws, Exception ex)
    {
        string str = string.Empty;
        if (ws.InternalRequest.Response != null)
        {
            str = $"Status Code from Server: {ws.InternalRequest.Response.StatusCode} and Message: {ws.InternalRequest.Response.Message}";
        }
        Debugger.Log("OnError " + str);
    }

    private void OnMessageReceived(BestHTTP.WebSocket.WebSocket ws, string message)
    {
        try
        {
            MatchMessage message2 = JsonConvert.DeserializeObject<MatchMessage>(message);
            if (!string.IsNullOrEmpty(message2.userid))
            {
                if (string.IsNullOrEmpty(this.otheruserid))
                {
                    this.otheruserid = message2.userid;
                }
                else if (this.otheruserid != message2.userid)
                {
                    return;
                }
            }
            switch (((MatchMessageType) message2.msgtype))
            {
                case MatchMessageType.eLearnSkill:
                    GameLogic.Hold.BattleData.Challenge_SendEvent("MatchDefenceTime_other_learn_skill", message2.argint);
                    return;

                case MatchMessageType.eDead:
                    GameLogic.Hold.BattleData.Challenge_SendEvent("MatchDefenceTime_other_dead", null);
                    return;

                case MatchMessageType.eReborn:
                    GameLogic.Hold.BattleData.Challenge_SendEvent("MatchDefenceTime_other_reborn", null);
                    return;

                case MatchMessageType.eScoreUpdate:
                    GameLogic.Hold.BattleData.Challenge_SendEvent("MatchDefenceTime_other_updatescore", message2.argint);
                    return;

                case MatchMessageType.eGameStart:
                {
                    long argint = message2.argint;
                    GameLogic.Hold.BattleData.Challenge_UpdateMode(0x332d, BattleSource.eMatch);
                    GameLogic.Hold.BattleData.Challenge_SendEvent("MatchDefenceTime_set_random_seed", argint);
                    WindowUI.ShowWindow(WindowID.WindowID_Battle);
                    GameLogic.Hold.BattleData.Challenge_SendEvent("MatchDefenceTime_me_updatename", this.myname);
                    GameLogic.Hold.BattleData.Challenge_SendEvent("MatchDefenceTime_other_updatename", message2.nickname);
                    return;
                }
                case MatchMessageType.eGameEnd:
                    return;
            }
        }
        catch
        {
        }
    }

    private void OnOpen(BestHTTP.WebSocket.WebSocket ws)
    {
        Debugger.Log("connected");
        this.mState = ConnectState.eConnected;
        this.Send(MatchMessageType.eUserComeIn, 0);
    }

    public void Send(string str)
    {
        this.webSocket.Send(str);
    }

    public void Send(MatchMessageType msgtype, int arg = 0)
    {
        if ((this.webSocket != null) && this.webSocket.IsOpen)
        {
            MatchMessage message = new MatchMessage();
            this.myname = LocalSave.Instance.GetUserName();
            message.userid = LocalSave.Instance.GetServerUserID().ToString();
            if (this.myname == string.Empty)
            {
                this.myname = message.userid;
            }
            message.nickname = this.myname;
            message.msgtype = (short) msgtype;
            message.argint = arg;
            this.Send(JsonConvert.SerializeObject(message));
        }
    }

    public ConnectState State =>
        this.mState;

    public bool IsConnected =>
        (this.mState == ConnectState.eConnected);

    public enum ConnectState
    {
        eConnecting,
        eConnected,
        eClose
    }
}

