using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameManager
{
    private RoomState roomState;
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private GameState <gameState>k__BackingField;
    private int talentId;
    private int skillId;
    private int sickId;

    public GameManager()
    {
        this.Init();
    }

    public void EndGame()
    {
        CameraControlM.Instance.CameraPositionZero();
        this.RemoveJoy();
    }

    private void Init()
    {
    }

    public void JoyEnable(bool enable)
    {
        if (GameLogic.Self != null)
        {
            if (enable)
            {
                GameLogic.Self.m_MoveCtrl.RegisterJoyEvent();
                GameLogic.Self.m_AttackCtrl.RegisterJoyEvent();
                this.ShowJoy(true);
            }
            else
            {
                GameLogic.Self.m_MoveCtrl.RemoveJoyEvent();
                GameLogic.Self.m_AttackCtrl.RemoveJoyEvent();
                GameLogic.Self.m_MoveCtrl.OnMoveEnd();
                GameLogic.Self.m_AttackCtrl.OnMoveEnd();
                this.ShowJoy(false);
            }
        }
    }

    public void Release()
    {
    }

    public void RemoveJoy()
    {
        if (this.MoveJoy != null)
        {
            Object.Destroy(this.MoveJoy.gameObject);
        }
    }

    public void SaveHeirChooseData(int talentId, int skillId, int sickId)
    {
        this.talentId = talentId;
        this.skillId = skillId;
        this.sickId = sickId;
    }

    public void SetGameState(GameState state)
    {
        this.gameState = state;
        switch (this.gameState)
        {
            case GameState.eMain:
                WindowUI.GameOver();
                break;

            case GameState.eGaming:
                WindowUI.GameBegin();
                break;
        }
    }

    public void SetRoomState(RoomState state)
    {
        this.roomState = state;
    }

    public void SetRunning()
    {
        this.SetRoomState(RoomState.Runing);
    }

    public void ShowJoy(bool show)
    {
        if (this.MoveJoy != null)
        {
            this.MoveJoy.SetActive(show);
        }
    }

    public void StartGame()
    {
    }

    private GameObject MoveJoy =>
        GameLogic.Release.Mode.GetMoveJoy();

    public RoomState RoomState =>
        this.roomState;

    public GameState gameState { get; private set; }

    public enum GameState
    {
        eMain,
        eGaming
    }
}

