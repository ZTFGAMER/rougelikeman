using System;
using UnityEngine;
using UnityEngine.UI;

public class UIOptions : MonoBehaviour
{
    public Text languageText;
    public RectTransform rect;
    public GameObject leaderboardObj;
    public GameObject restorePurchasesObj;
    public GameObject SignInObj;
    public GameObject SignOutObj;

    private void OnEnable()
    {
        this.UpdateLanguageText();
        this.RefreshGooglePlayUI();
    }

    public void RefreshGooglePlayUI()
    {
        this.restorePurchasesObj.SetActive(false);
        this.SignOutObj.SetActive(LeaderboardManager.loginSuccessful);
        this.SignInObj.SetActive(!LeaderboardManager.loginSuccessful);
    }

    public void SetLanguageText()
    {
        Language currentLanguage = LanguageManager.instance.currentLanguage;
        this.languageText.text = currentLanguage.languageName;
        this.languageText.set_font(currentLanguage.font);
    }

    public void ShowLeaderboard()
    {
        LeaderboardManager.instance.ShowLeaderboard();
    }

    public void SignIn()
    {
        LeaderboardManager.instance.AuthenticateUser();
    }

    public void SignOut()
    {
        LeaderboardManager.instance.SignOut();
        this.RefreshGooglePlayUI();
    }

    public void UpdateLanguageText()
    {
        this.SetLanguageText();
    }
}

