using System;
using UnityEngine;
using UnityEngine.UI;

public class DailyRewardPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject askAboutNotificationsPanel;
    [SerializeField]
    private GameObject dailyCloseBg;
    [SerializeField]
    private Button buttonYes;

    public void OnEnable()
    {
        this.dailyCloseBg.SetActive(false);
        this.askAboutNotificationsPanel.SetActive(false);
    }

    public void ShowNotificationAskPanel()
    {
        DataLoader.gui.popUpsPanel.gameObject.SetActive(false);
    }

    private void Start()
    {
    }
}

