using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static TimeManager <instance>k__BackingField;
    private static DateTime currentTime;
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static bool <gotDateTime>k__BackingField;

    private void Awake()
    {
        if ((instance != null) && (instance != this))
        {
            UnityEngine.Object.Destroy(base.gameObject);
        }
        else
        {
            instance = this;
            UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
        }
    }

    public static bool GetNistTime(out DateTime dateTime)
    {
        if (!StaticConstants.NeedInternetConnection)
        {
            dateTime = DateTime.Now;
            return true;
        }
        dateTime = DateTime.MinValue;
        string str = string.Empty;
        string[] strArray = new string[] { 
            "time-a-g.nist.gov", "time-b-g.nist.gov", "time-c-g.nist.gov", "time-d-g.nist.gov", "time-a-wwv.nist.gov", "time-b-wwv.nist.gov", "time-c-wwv.nist.gov", "time-d-wwv.nist.gov", "time-a-b.nist.gov", "time-b-b.nist.gov", "utcnist.colorado.edu", "utcnist2.colorado.edu", "nist1-ny.ustiming.org", "time-a.nist.gov", "nist1-chi.ustiming.org", "time.nist.gov",
            "ntp-nist.ldsbc.edu", "nist1-la.ustiming.org"
        };
        for (int i = 0; i < strArray.Length; i++)
        {
            try
            {
                StreamReader reader = new StreamReader(new TcpClient(strArray[i], 13).GetStream());
                str = reader.ReadToEnd();
                reader.Close();
                if ((str.Length > 0x2f) && str.Substring(0x26, 9).Equals("UTC(NIST)"))
                {
                    int num2 = int.Parse(str.Substring(1, 5));
                    int year = int.Parse(str.Substring(7, 2));
                    int month = int.Parse(str.Substring(10, 2));
                    int day = int.Parse(str.Substring(13, 2));
                    int hour = int.Parse(str.Substring(0x10, 2));
                    int minute = int.Parse(str.Substring(0x13, 2));
                    int second = int.Parse(str.Substring(0x16, 2));
                    if (num2 > 0xc958)
                    {
                        year += 0x7d0;
                    }
                    else
                    {
                        year += 0x7cf;
                    }
                    dateTime = new DateTime(year, month, day, hour, minute, second).ToLocalTime();
                    break;
                }
            }
            catch (Exception)
            {
            }
        }
        return (dateTime != DateTime.MinValue);
    }

    public void OnApplicationPause(bool pause)
    {
        if (!pause)
        {
            this.UpdateTime();
        }
    }

    [DebuggerHidden]
    private IEnumerator SetFirstDate(SaveData saveData) => 
        new <SetFirstDate>c__Iterator1 { saveData = saveData };

    public void SetFirstEnterDate(SaveData saveData)
    {
        base.StartCoroutine(this.SetFirstDate(saveData));
    }

    private void Start()
    {
        Application.runInBackground = false;
        base.StartCoroutine(this.TimeCor());
    }

    [DebuggerHidden]
    private IEnumerator TimeCor() => 
        new <TimeCor>c__Iterator0 { $this = this };

    public void UpdateTime()
    {
        gotDateTime = GetNistTime(out currentTime);
    }

    public static TimeManager instance
    {
        [CompilerGenerated]
        get => 
            <instance>k__BackingField;
        [CompilerGenerated]
        private set => 
            (<instance>k__BackingField = value);
    }

    public static bool gotDateTime
    {
        [CompilerGenerated]
        get => 
            <gotDateTime>k__BackingField;
        [CompilerGenerated]
        private set => 
            (<gotDateTime>k__BackingField = value);
    }

    public static DateTime CurrentDateTime =>
        currentTime;

    [CompilerGenerated]
    private sealed class <SetFirstDate>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal SaveData saveData;
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
                    if (!TimeManager.gotDateTime)
                    {
                        this.$current = null;
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        return true;
                    }
                    if (this.saveData.firstEnterDate == DateTime.MinValue)
                    {
                        this.saveData.firstEnterDate = TimeManager.CurrentDateTime;
                        IOSCloudSave.instance.SaveFirstDate();
                        UnityEngine.Debug.Log("First enter date: " + this.saveData.firstEnterDate);
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

    [CompilerGenerated]
    private sealed class <TimeCor>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal TimeManager $this;
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
                    this.$this.UpdateTime();
                    break;

                case 1:
                    TimeManager.currentTime = TimeManager.currentTime.AddSeconds(1.0);
                    break;

                default:
                    return false;
            }
            this.$current = new WaitForSecondsRealtime(1f);
            if (!this.$disposing)
            {
                this.$PC = 1;
            }
            return true;
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

