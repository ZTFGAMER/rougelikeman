using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;

public class AppFacade : Facade, IFacade, INotifier, IDisposable
{
    protected override void InitializeController()
    {
        base.InitializeController();
    }

    protected override void InitializeModel()
    {
        base.InitializeModel();
    }

    protected override void InitializeView()
    {
        base.InitializeView();
        if (!PlayerPrefsEncrypt.HasKey("first_cg"))
        {
            PlayerPrefsEncrypt.SetInt("first_cg", 0);
            GameLogic.Hold.Sound.StopBackgroundMusic();
            WindowUI.ShowWindow(WindowID.WindowID_VideoPlay);
        }
        else
        {
            LocalSave.Instance.DoLogin_Start(null);
            WindowUI.ShowWindow(WindowID.WindowID_Login);
        }
    }
}

