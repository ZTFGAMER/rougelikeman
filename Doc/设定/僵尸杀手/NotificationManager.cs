using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static NotificationManager <instance>k__BackingField;
    private readonly Dictionary<TimeSpan, TimeSpan> bestNotificationTime;
    [SerializeField]
    private bool debugMode;
    [SerializeField]
    private List<NotificationInfo> notifications;
    [SerializeField]
    private NotificationInfo testNotification;

    public NotificationManager()
    {
        Dictionary<TimeSpan, TimeSpan> dictionary = new Dictionary<TimeSpan, TimeSpan> {
            { 
                TimeSpan.FromHours(20.0),
                TimeSpan.FromHours(20.0)
            }
        };
        this.bestNotificationTime = dictionary;
    }

    private void Awake()
    {
        instance = this;
    }

    private DateTime GetRandomNotificationTime(DateTime date, NotificationInfo.NotificationTimeType type)
    {
        int index = UnityEngine.Random.Range(0, this.bestNotificationTime.Count);
        TimeSpan span = this.bestNotificationTime.ElementAt<KeyValuePair<TimeSpan, TimeSpan>>(index).Value - this.bestNotificationTime.ElementAt<KeyValuePair<TimeSpan, TimeSpan>>(index).Key;
        int totalSeconds = (int) span.TotalSeconds;
        TimeSpan span2 = TimeSpan.FromHours(12.0);
        if (type != NotificationInfo.NotificationTimeType.Hours)
        {
            if (type == NotificationInfo.NotificationTimeType.Minutes)
            {
                span2 = this.bestNotificationTime.ElementAt<KeyValuePair<TimeSpan, TimeSpan>>(index).Key.Add(TimeSpan.FromMinutes((double) UnityEngine.Random.Range(0, totalSeconds / 60)));
            }
            else if (type == NotificationInfo.NotificationTimeType.Seconds)
            {
                span2 = this.bestNotificationTime.ElementAt<KeyValuePair<TimeSpan, TimeSpan>>(index).Key.Add(TimeSpan.FromSeconds((double) UnityEngine.Random.Range(0, totalSeconds)));
            }
        }
        else
        {
            span2 = this.bestNotificationTime.ElementAt<KeyValuePair<TimeSpan, TimeSpan>>(index).Key.Add(TimeSpan.FromHours((double) UnityEngine.Random.Range(0, (totalSeconds / 0xe10) + 1)));
        }
        return date.Date.Add(span2);
    }

    private void PrintText(string text)
    {
        if (this.debugMode)
        {
            UnityEngine.Debug.Log(text);
        }
    }

    private void RegisterIosNotification(int seconds, string description)
    {
    }

    public void RegisterNotifications()
    {
        UnityEngine.Debug.Log("RegisteredNotifications");
        this.Start();
    }

    public void SendTestNotification()
    {
        this.PrintText("Initializing test notification");
        AndroidNotification.SendNotification(0x3e7, 10L, this.testNotification.header1, this.testNotification.description, this.testNotification.header2, Color.white, true, AndroidNotification.NotificationExecuteMode.Inexact, true, 2, true, 0L, 500L);
    }

    private void Start()
    {
        AndroidNotification.CancelAllNotifications();
        base.StartCoroutine(this.WaitForLocalization());
    }

    public void SwichDelayType(int notificationID)
    {
        DateTime now = DateTime.Now;
        switch (this.notifications[notificationID].delayType)
        {
            case NotificationInfo.DelayType.Year:
                now = now.AddYears(this.notifications[notificationID].delay);
                break;

            case NotificationInfo.DelayType.Month:
                now = now.AddMonths(this.notifications[notificationID].delay);
                break;

            case NotificationInfo.DelayType.Day:
                now = now.AddDays((double) this.notifications[notificationID].delay);
                break;

            case NotificationInfo.DelayType.Hour:
                now = now.AddHours((double) this.notifications[notificationID].delay);
                break;

            case NotificationInfo.DelayType.Minute:
                now = now.AddMinutes((double) this.notifications[notificationID].delay);
                break;

            case NotificationInfo.DelayType.Second:
                now = now.AddSeconds((double) this.notifications[notificationID].delay);
                break;

            case NotificationInfo.DelayType.WorkDay:
                if (DateTime.Now.DayOfWeek != DayOfWeek.Friday)
                {
                    if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday)
                    {
                        now = now.AddDays(2.0);
                    }
                    else
                    {
                        now = now.AddDays(1.0);
                    }
                    break;
                }
                now = now.AddDays(3.0);
                break;

            case NotificationInfo.DelayType.Holiday:
                now = now.AddDays((double) (UnityEngine.Random.Range(6, 8) - now.DayOfWeek));
                break;
        }
        int totalSeconds = (int) this.GetRandomNotificationTime(now, this.notifications[notificationID].timeType).Subtract(DateTime.Now).TotalSeconds;
        AndroidNotification.SendNotification(notificationID + 1, (long) totalSeconds, LanguageManager.instance.GetLocalizedText(this.notifications[notificationID].header1), LanguageManager.instance.GetLocalizedText(this.notifications[notificationID].description), LanguageManager.instance.GetLocalizedText(this.notifications[notificationID].header2), Color.white, true, AndroidNotification.NotificationExecuteMode.Inexact, true, 2, true, 0L, 500L);
    }

    [DebuggerHidden]
    public IEnumerator WaitForLocalization() => 
        new <WaitForLocalization>c__Iterator0 { $this = this };

    public static NotificationManager instance
    {
        [CompilerGenerated]
        get => 
            <instance>k__BackingField;
        [CompilerGenerated]
        private set => 
            (<instance>k__BackingField = value);
    }

    [CompilerGenerated]
    private sealed class <WaitForLocalization>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal NotificationManager $this;
        internal object $current;
        internal bool $disposing;
        internal int $PC;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$disposing = true;
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                case 1:
                    if (LanguageManager.instance == null)
                    {
                        this.$current = null;
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                    }
                    else
                    {
                        this.$current = new WaitForSeconds(1f);
                        if (!this.$disposing)
                        {
                            this.$PC = 2;
                        }
                    }
                    return true;

                case 2:
                    for (int i = 0; i < this.$this.notifications.Count; i++)
                    {
                        this.$this.SwichDelayType(i);
                    }
                    this.$PC = -1;
                    break;
            }
            return false;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current =>
            this.$current;

        object IEnumerator.Current =>
            this.$current;
    }
}

