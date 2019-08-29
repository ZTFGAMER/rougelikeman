using System;
using UnityEngine;
using UnityEngine.UI;

public class NotifyController : MonoBehaviour
{
    public Button[] bttns;
    private float currentTime;
    private int id = 1;

    public void turnOnNotif(int time)
    {
        AndroidNotification.SendNotification(1, (long) time, "Отличный заголовок", "Тут может быть ваша реклама!", "Посмотри сюда!", Color.white, true, AndroidNotification.NotificationExecuteMode.Inexact, true, 2, true, 0L, 500L);
        this.currentTime = time;
    }

    public void turnOnNotifNew(int time)
    {
        AndroidNotification.SendNotification(this.id, (long) time, "Отличный заголовок", "Тут может быть ваша реклама!", "Посмотри сюда!", Color.white, true, AndroidNotification.NotificationExecuteMode.Inexact, true, 2, true, 0L, 500L);
        this.id++;
        this.currentTime = time;
    }

    private void Update()
    {
        if (this.currentTime > 0f)
        {
            this.currentTime -= Time.deltaTime;
            foreach (Button button in this.bttns)
            {
                button.interactable = false;
            }
        }
        else if (this.currentTime <= 0f)
        {
            foreach (Button button2 in this.bttns)
            {
                button2.interactable = true;
            }
        }
    }
}

