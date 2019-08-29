using System;
using UnityEngine;

[Serializable]
public class NotificationInfo
{
    public string header1;
    public string description;
    public string header2;
    [Tooltip("Truncates notification time value")]
    public NotificationTimeType timeType;
    public DelayType delayType = DelayType.Day;
    [Tooltip("The value will not be considered if delayType equals Holiday or WorkDay")]
    public int delay;

    public enum DelayType
    {
        Year,
        Month,
        Day,
        Hour,
        Minute,
        Second,
        WorkDay,
        Holiday
    }

    public enum NotificationTimeType
    {
        Hours,
        Minutes,
        Seconds
    }
}

