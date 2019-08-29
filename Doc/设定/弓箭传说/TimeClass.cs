using System;
using System.Collections.Generic;

public class TimeClass
{
    private float StartTime;
    private float DelayTime;
    private Action callback;
    private List<TimeClass> m_TimeList = new List<TimeClass>();
    private List<Action> m_TimePerFrameList = new List<Action>();
    private int TimeCount;
    private int TimePerFrameCount;

    public bool IsCallBackOver(Action callback)
    {
        for (int i = 0; i < this.TimeCount; i++)
        {
            if (this.m_TimeList[i].callback == callback)
            {
                return false;
            }
        }
        return true;
    }

    private bool IsDelayOver(float CurrentTime) => 
        ((CurrentTime - this.StartTime) >= this.DelayTime);

    public void RemoveCallBack(Action callback)
    {
        for (int i = 0; i < this.TimeCount; i++)
        {
            if (this.m_TimeList[i].callback == callback)
            {
                this.m_TimeList.RemoveAt(i);
                this.TimeCount--;
                break;
            }
        }
    }

    public void Reset()
    {
        this.m_TimeList.Clear();
        this.m_TimePerFrameList.Clear();
        this.TimeCount = 0;
        this.TimePerFrameCount = 0;
    }

    public void StartCallBack(float AliveTime, Action callback)
    {
        this.StartCallBack(AliveTime, 0f, callback);
    }

    public void StartCallBack(float AliveTime, float DelayTime, Action callback)
    {
        TimeClass item = new TimeClass {
            StartTime = AliveTime,
            DelayTime = DelayTime,
            callback = callback
        };
        this.m_TimeList.Add(item);
        this.TimeCount = this.m_TimeList.Count;
    }

    public void StartPerFrame(Action callback)
    {
        this.StopPerFrame(callback);
        this.m_TimePerFrameList.Add(callback);
        this.TimePerFrameCount++;
    }

    public void StopPerFrame(Action callback)
    {
        if (this.m_TimePerFrameList.Contains(callback))
        {
            this.m_TimePerFrameList.Remove(callback);
            this.TimePerFrameCount--;
        }
    }

    public void UpdateTimeClass(float AliveTime)
    {
        if (this.TimeCount > 0)
        {
            for (int i = this.TimeCount - 1; i >= 0; i--)
            {
                if (this.m_TimeList[i].IsDelayOver(AliveTime))
                {
                    this.m_TimeList[i].callback();
                    this.m_TimeList.RemoveAt(i);
                    this.TimeCount--;
                }
            }
        }
        if (this.TimePerFrameCount > 0)
        {
            for (int i = 0; i < this.TimePerFrameCount; i++)
            {
                this.m_TimePerFrameList[i]();
            }
        }
    }
}

